/** ****************************************************************************	
	@file mcohw_nvol_AVR.c

	@brief Atmel AVR EEPROM drivers for MicroCANopen plus

	@details
	This file contains all of the hardware drivers required to support 
	MicroCANopen Plus EEPROM access on the Atmel AVR family of microcontrollers
	
	@author John Jayne

	@version 1.00

	@date 02/26/16

	@bug
	
	@todo
	
	@copyright Copyright 2016 ULC Robotics
	
*******************************************************************************/

/*_____ I N C L U D E S ______________________________________________________*/

#include <avr/interrupt.h>  /* Microcontroller interrupt header file */
#include <avr/io.h>	        /* Microcontroller hardware header file */

/*_____ D E F I N I T I O N S ________________________________________________*/

#define ERASED_BYTE 0xFF

/*_____ D E C L A R A T I O N S ______________________________________________*/


/*_____ F U N C T I O N S ____________________________________________________*/

/** ****************************************************************************
	This function initializes access to non-volatile (EEPROM) memory 
	
	@warning 
	
	@param 
	
	@return  
*******************************************************************************/
void NVOL_Init(void)
{
	
}

/** ****************************************************************************
	This function reads one byte from non-volatile (EEPROM) memory 
	
	@warning 
	
	@param 
	
	@return  
*******************************************************************************/
uint8_t NVOL_ReadByte(uint16_t ee_addr)
{
	while((1 << EEWE) & EECR);  /* wait until EEPROM is not busy */
	EEARL = *((uint8_t *)(&ee_addr));	        /* LSB of address */
	EEARH = *((uint8_t *)((&ee_addr) + 1)); 	/* MSB of address */ 
	EECR |= (1 << EERE);                        /* read the byte */
    
	return (EEDR);
}

/** ****************************************************************************
	This function writes one byte to non-volatile (EEPROM) memory 
	
	@warning 
	
	@param 
	
	@return  
*******************************************************************************/
void NVOL_WriteByte(uint16_t ee_addr, uint8_t ee_data)
{
	while((1 << EEWE) & EECR);  /* wait until EEPROM is not busy */ 
	EEARL = *((uint8_t *)(&ee_addr));	    /* LSB of address */
	EEARH = *((uint8_t *)((&ee_addr) + 1));	/* MSB of address */ 
    EECR |= (1 << EERE);                    /* read the byte */
    if (EEDR != ee_data)
    /* present EEPROM memory does not equal requested value */ 
    {
        if (ERASED_BYTE == EEDR)
        /* present EEPROM location is already erased */  
        {
            EECR = 0x20;    /* set up for write only */
        }
        else if (ERASED_BYTE == ee_data)
        /* present EEPROM location needs to be erased only */
        {
            EECR = 0x10;    /* set up for erase only */
        }
        else
        /* present EEPROM location needs to be erased and re-written */
        {
            EECR = 0x00;    /* set up for atomic operation */
        }
        EEDR = ee_data;
        cli();   /* turn off interrupts */
        EECR |= (1 << EEMWE);   /* enable the write */
        EECR |= (1 << EEWE);    /* write the byte */
        sei();   /* turn on interrupts */        
    }    
}

/** ****************************************************************************
	This function signifies the end of a block of consecutively addressed bytes
	of non-volatile (EEPROM) memory. 
	
	@warning 
	
	@param 
	
	@return  
*******************************************************************************/
void NVOL_WriteComplete(void)
{

}


/*----------------------------- END OF FILE ----------------------------------*/