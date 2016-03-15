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

#define EEPROM_CONFIGURATION_ADDRESS (256)
#define EEPROM_CHECK_VALUE (0xA5)

#define REPAIR_MODE (0)
#define INSPECT_MODE (1)


static U8 readEEPROM(U16 address, U8 * dest, U8 length);
static U8 writeEEPROM(U16 address, U8 * source, U8 length);

static void processConfiguration(void);

static U8 deviceMode; /*!< mode of device, 0=repair, 1=inspect */

static char * softwareVersion = "v1.02 abc def";

/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

// read EEPROM memory and stores to destination, return 1 on load of pointer, return 0 on no load
static U8 readEEPROM(U16 address, U8 * dest, U8 length)
{
	U8 i;
	
	/* Wait for completion of previous write */
	while(EECR & (1<<EEWE));

	for (i=0; i<length; i++)
	{
		/* Set up address register */
		EEAR = (address + i);

		/* Start eeprom read by writing EERE */
		EECR |= (1<<EERE);
		
		dest[i] = EEDR;
	}

	return(1);
}

// write EEPROM memory from source, return 1 on success, return 0 on failure
static U8 writeEEPROM(U16 address, U8 * source, U8 length)
{
	U8 i;	

	for (i=0; i<length; i++)
	{
		/* Wait for completion of previous write */
		while(EECR & (1<<EEWE));
		
		/* Set up address register */
		EEAR = (address + i);

		/* Store Data */
		EEDR = source[i];

		/* Write logical one to EEMWE */
		EECR |= (1<<EEMWE);

		/* Start eeprom write by setting EEWE */
		EECR |= (1<<EEWE);
	}

	return(1);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
static void processConfiguration(void)
{
   U32 configuration;

   if ( (readEEPROM(EEPROM_CONFIGURATION_ADDRESS, (U8 *)&configuration, sizeof(configuration)) != 0) )
   {
      U8 check = ((configuration >> 24) & 0xFF);

      if ( (EEPROM_CHECK_VALUE == check) )
      {
         U16 bps;

         U8 mode = (configuration >> 16) & 0xFF;;
         U8 nodeId = (configuration >> 8) & 0xFF;
         U8 baudCode = (configuration >> 0) & 0xFF;

         // 0=10K, 1=20K, 2=50K, 3=100K, 4=125K, 5=250K, 6=500K, 7=1M
         if (0 == baudCode)
         {
            bps = 10;
         }
         else if (1 == baudCode)
         {
            bps = 20;
         }
         else if (3 == baudCode)
         {
            bps = 100;
         }
         else if (4 == baudCode)
         {
            bps = 125;
         }
         else if (5 == baudCode)
         {
            bps = 250;
         }
         else if (6 == baudCode)
         {
            bps = 500;
         }
         else if (7 == baudCode)
         {
            bps = 1000;
         }
         else
         {
            bps = 50;
         }

         CAN_WriteProcessImage(0x2100, 0, &baudCode, sizeof(baudCode));
         CAN_WriteProcessImage(0x2101, 0, &nodeId, sizeof(nodeId));
         CAN_WriteProcessImage(0x2102, 0, &mode, sizeof(mode));
         
         CAN_SetNodeId(nodeId);
         CAN_SetBaudrate(bps);

         deviceMode = mode;
      }
   }
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
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
   if (127 == state)
   {
      // pre-operational
   }
   else if (5 == state)
   {
      // running
   }
   else if (4 == state)
   {
      // stopped
   }
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
      *totalSize = strlen(softwareVersion);
      *pData = (U8 *)softwareVersion;      
      result = 1;
   }

   *size = *totalSize;

   return(result);
}

/* See can_callbacks.h for function description */
void CAN_AppSDOReadComplete(U8 server, U16 index, U8 subIndex, U32 * size)
{
   *size = 0;
}

/* See can_callbacks.h for function description */
void CAN_ODData(U16 index, U8 subIndex, U8 * pData, U8 length)
{
   // evaluate
   if (0x2105 == index)
   {
      if ( (4 == length) )
      {
         U32 command = 0;
         memcpy(&command, pData, length);

         if ( (0x65766173 == command) )
         {
            U8 baudrate;
            U8 nodeId;
            U8 mode;
            U32 configuration;

            CAN_ReadProcessImage(0x2100, 0, &baudrate, sizeof(baudrate));
            CAN_ReadProcessImage(0x2101, 0, &nodeId, sizeof(nodeId));
            CAN_ReadProcessImage(0x2102, 0, &mode, sizeof(mode));

            configuration = EEPROM_CHECK_VALUE;
            configuration <<= 8;
            configuration |= mode;
            configuration <<= 8;
            configuration |= nodeId;
            configuration <<= 8;
            configuration |= baudrate;

            writeEEPROM(EEPROM_CONFIGURATION_ADDRESS, (U8 *)&configuration, sizeof(configuration));
         }
      }
   }
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
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
    processConfiguration();
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
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
