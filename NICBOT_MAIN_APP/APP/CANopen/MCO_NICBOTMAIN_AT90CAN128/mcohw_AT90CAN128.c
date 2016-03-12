/** ****************************************************************************	
	@file mcohw_AVR.c

	@brief Atmel AVR hardware drivers for MicroCANopen plus

	@details
	This file contains all of the hardware drivers required to support 
	MicroCANopen plus on the Atmel AVR family of microcontrollers
	
	@author John Jayne

	@version 1.00

	@date 02/26/16

	@bug
	
	@todo
	
	@copyright Copyright 2016 ULC Robotics
	
*******************************************************************************/

/*_____ I N C L U D E S ______________________________________________________*/

#include <avr/io.h>	        /* Microcontroller hardware header file */
#include <avr/interrupt.h>  /* Microcontroller interrupt header file */
#include "mcohw_cfg.h"      /* AVR configuration header file */
#include "mcohw.h"
#include "mco.h"
#include "canfifo.h"

/*_____ D E F I N I T I O N S ________________________________________________*/

#define MAX_DLC 0x08U   /**< maximum number of data bytes in a CAN frame */
#define DLC_MSK ((1 << DLC0) + (1 << DLC1) + (1 << DLC2) + (1 << DLC3))
#define FIRST_CANIE1_MOB 0x08U
#define OVERFLOW_COUNT 0x10000UL

/* From datasheet: Tclk = Tclkio * 8 * (PRESCALE + 1) */
/* In present configuration Tclkio = 1/16MHz */
/* Thus: Fclk =  2MHz / (PRESCALE + 1) */
/* where PRESCALE = 0 to 255 */

#define PRESCALE_8KHZ 0x05


/** ****************************************************************************
	This macro services a MOb which has generated a receive (RXOK) interrupt 
	
	@warning none
	
	@param none
	
	@return none
*******************************************************************************/
#define READ_MOB()  													       \
{																		       \
	if (((1 << AERR) + (1 << FERR) + (1 << CERR) + (1 << SERR)) & CANSTMOB)    \
    {                                                                          \
        can_status |= HW_CERR;	/* set CERR - comm error */                    \
    }				                                                           \
	if ((1 << RXOK) & CANSTMOB)												   \
	{																		   \
		p_buffer = CANRXFIFO_GetInPtr();									   \
		if (NULL != p_buffer)												   \
		/* no overflow so place received CAN message in Rx buffer */		   \
		{																	   \
			p_buffer->ID = (CANIDT2 >> 5) | ((uint16_t)CANIDT1 << 3);          \
			p_buffer->LEN = (CANCDMOB & DLC_MSK);	/* get DLC */			   \
			if (MAX_DLC < p_buffer->LEN)                                       \
            {                                                                  \
                p_buffer->LEN = MAX_DLC;                                       \
            }                                                                  \
            data_index = 0;                                					   \
			while (data_index < p_buffer->LEN)	                               \
			{                                                                  \
                p_buffer->BUF[data_index] = CANMSG;	/* load data */            \
                data_index++;                                                  \
            }                                               				   \
			CANCDMOB = (1 << CONMOB1);  /* enable std Rx */                    \
			CANRXFIFO_InDone();												   \
		}																	   \
		else                                                                   \
		/* overflow so set Rx buffer overrun indicator */           		   \
        {                                                                      \
            can_status |= HW_RXOR;	/* set RXOR - Rx buffer overrun */         \
        }			                                                           \
	}																		   \
	CANSTMOB &= 0x00;	/* clear MOb status (acknowledge interrupt) */		   \
}

/** ****************************************************************************
	This macro services a MOb which has generated a transmit (TXOK) interrupt 
	
	@warning none
	
	@param none
	
	@return none
*******************************************************************************/
#define WRITE_MOB()  													       \
{		                                                                       \
	if (((1 << AERR) + (1 << BERR)) & CANSTMOB)                                \
	{                                                                          \
    	can_status |= HW_CERR;	/* set CERR status */                          \
	}                                                                          \
	if ((1 << TXOK) & CANSTMOB)                                                \
	{                                                                          \
    	p_buffer = CANTXFIFO_GetOutPtr();                                      \
    	if (NULL != p_buffer)                                                  \
    	/* Tx buffer is not empty so transmit the next message */              \
    	{                                                                      \
       		CANIDT1 = ((*((uint8_t *)((&(p_buffer->ID)) + 1))) << 5) +         \
       		((*((uint8_t *)(&(p_buffer->ID)))) >> 3);	      /* ID[10 - 3] */ \
       		CANIDT2 = (*((uint8_t *)(&(p_buffer->ID)))) << 5; /* ID[2-0] */    \
       		CANIDT4 = 0x00; /* clear RTR and RB0 */                            \
       		if (MAX_DLC < p_buffer->LEN)                                       \
       		{                                                                  \
           		p_buffer->LEN = MAX_DLC;                                       \
       		}                                                                  \
            data_index = 0;                                                    \
       		while (data_index < p_buffer->LEN)                                 \
       		{                                                                  \
           		CANMSG = p_buffer->BUF[data_index];	/* load data */            \
                data_index++;                                                  \
       		}                                                                  \
       		CANCDMOB = (p_buffer->LEN + (1 << CONMOB0));  /* start std Tx */   \
       		CANTXFIFO_OutDone();                                               \
    	}                                                                      \
    	else                                                                   \
    	/* Tx buffer is empty so disable the transmitter */                    \
    	{                                                                      \
       		CANCDMOB &= 0x00;	/* disable MOb */                              \
            CANGIE &= ~(1 << ENTX);	/* disable transmit interrupt */           \
            if (FIRST_CANIE1_MOB > MAX_MOB)                                    \
            /* disable MOb interrupt in CANIE2 */                              \
            {                                                                  \
                CANIE2 &= ~(1 << MAX_MOB);	/* disable MOb interrupt */        \
            }                                                                  \
            else                                                               \
            /* disable MOb interrupt in CANIE1 */                              \
            {                                                                  \
                CANIE1 &= ~(1 << (MAX_MOB - FIRST_CANIE1_MOB));	               \
            }                                                                  \
    	}                                                                      \
	}                                                                          \
	CANSTMOB &= 0x00;   /* clear MOb status (acknowledge interrupt) */         \
}    

/*_____ D E C L A R A T I O N S ______________________________________________*/

static uint8_t can_status = 0;	/**< CAN status variable */
static uint8_t b_tovf = 0;	/**< timer overflow flag */
static uint32_t can_timer = 0;	/**< free running (8KHz) 125usec timer */ 


/*_____ F U N C T I O N S ____________________________________________________*/

/** ****************************************************************************
	This function initializes/resets the CAN interface 
	
	@warning default CAN baud rate = 50K
	
	@param[in] BaudRate =  CAN baud rate (10000, 800, 500, 250, 125, 50, 20, 10)
	
	@return 1 if initialization completed successfully
			0 if initialization failed 
*******************************************************************************/
uint8_t MCOHW_Init(uint16_t baud_rate)
{
	uint8_t result;
	uint8_t mob_num;
	
	CANGIE = 0x00;		        /* disable all CAN interrupts */
	CANGCON = (1 << SWRES);		/* reset CAN Controller */ 
    mob_num = 0;
	while (mob_num <= MAX_MOB)
	{
		CANPAGE = (mob_num << MOBNB0);	/* set MOb number */
		CANCDMOB = 0x00;	/* disable MOb */
		CANSTMOB &= 0x00;	/* clear MOb status */
        mob_num++;
	}	
#if (TXFIFOSIZE > 0)
	CANTXFIFO_Flush();	/* flush the Tx buffer */
#endif
#if (RXFIFOSIZE > 0)
	CANRXFIFO_Flush();	/* flush the Rx buffer */
#endif
	can_timer = 0;	/* reset the CAN timer */
	result = 1;
	switch (baud_rate)
	{
		case 1000:
			CANBT1 = 0x00;  /* set CAN baud rate to 1M */
			CANBT2 = 0x0C;
			CANBT3 = 0x36;
			break;
		case 800:
			CANBT1 = 0x00;	/* set CAN baud rate to 800K */
			CANBT2 = 0x0E;
			CANBT3 = 0x4B;
			break;
		case 500:
			CANBT1 = 0x02;	/* set CAN baud rate to 500K */
			CANBT2 = 0x0C;
			CANBT3 = 0x37;
			break;
		case 250:
			CANBT1 = 0x06;	/* set CAN baud rate to 250K */
			CANBT2 = 0x0C;
			CANBT3 = 0x37;
			break;
		case 125:
			CANBT1 = 0x0E;	/* set CAN baud rate to 125K */
			CANBT2 = 0x0C;
			CANBT3 = 0x37;
			break;
		case 50:
			CANBT1 = 0x26;	/* set CAN baud rate to 50K */
			CANBT2 = 0x0C;
			CANBT3 = 0x37;
			break;
		case 20:
			CANBT1 = 0x62;	/* set CAN baud rate to 20K */
			CANBT2 = 0x0C;
			CANBT3 = 0x37;
			break;
		case 10:
			CANBT1 = 0x7F;	/* set CAN baud rate to 10K */
			CANBT2 = 0x0E;
			CANBT3 = 0x7F;
			break;
		default:
			CANBT1 = 0x26;	/* set default CAN baud rate to 50K */
			CANBT2 = 0x0C;
			CANBT3 = 0x37;
			result = 0;
			break;
	}
	CANTCON = PRESCALE_8KHZ;	/* set CAN timer prescaler for 8KHz clock */
    CANGCON = (1 << ENASTB); /* enable CAN Controller */
	CANGIE = (1 << ENIT) + (1 << ENRX) + (1 << ENERR) + (1 << ENOVRT);
        /* enable Rx, error, and timer overflow interrupts */
	can_status |= result;   /* set CAN INIT status */
    
	return (result);
}

/** ****************************************************************************
	This function initializes a CAN ID hardware filter if available 
	
	@warning  (ATmega) this function uses MOb0-MOb4 (MOb5 is reserved for Tx) 
              (AT90) this function uses MOb0-MOb13 (MOb14 is reserved for Tx) 
	
	@param[in] can_id - the CAN ID to be accepted by a hardware filter 
	
	@return 0 if no more hardware filters are available
			1 if hardware filter was set
			2 if hardware filtering is disabled (must use software) 
*******************************************************************************/
uint8_t MCOHW_SetCANFilter(uint16_t can_id)
{
	uint8_t mob_num;
    uint16_t canie_t;
	uint8_t result;
    
    canie_t = ((CANIE1 << 8) + CANIE2) & (~(~0 << MAX_MOB));
	mob_num = 0; 
    while (canie_t >> mob_num)  /* get the next free MOb */   
    {
        mob_num++;  
    }
    if (MAX_MOB > mob_num)
    {
		CANPAGE = (mob_num << MOBNB0);	/* set MOb page number */
		CANSTMOB &= 0x00;	/* clear MOb status */
		CANIDT1 = (uint8_t)((can_id >> 3) & 0xFF);
		CANIDT2 = (uint8_t)((can_id & 0x7) << 5);
		CANIDT4 = 0x00;	/* clear RTR and RB0 */
		CANIDM1 = 0xFF;	/* set IDmask[10 - 3] */
		CANIDM2 = 0x03;	/* set IDmask[2 - 0] */
		CANIDM4 = (1 << RTRMSK) + (1 << IDEMSK); /* set RTRmask and IDEmask */
		CANCDMOB = (1 << CONMOB1);  /* enable std Rx */
        if (FIRST_CANIE1_MOB > mob_num)
        {
		    CANIE2 |= (1 << mob_num);  /* enable MOb interrupt */
        }
        else
        {
            CANIE1 |= (1 << (mob_num - FIRST_CANIE1_MOB));  /* enable MOb int */
        }                
		result = 1;	/* hardware filter is set */
	}
	else 
    {
        result = 0;	/* no free MOb's */
    }     

	return (result);
}

/** ****************************************************************************
	This function releases/clears a CAN ID hardware filter 
	
	@warning  (ATmega) this function uses MOb0-MOb4 (MOb5 is reserved for Tx) 
              (AT90) this function uses MOb0-MOb13 (MOb14 is reserved for Tx) 
	
	@param[in] can_id - the CAN ID of the hardware filter to be released 
	
	@return 0 if hardware filter was not released
			1 if hardware filter was released
*******************************************************************************/
uint8_t MCOHW_ClearCANFilter(uint16_t can_id)
{
	uint8_t mob_num;
    uint16_t filter;
	uint8_t result;
    
	result = 0; /* no hardware filter has been released */
    mob_num = 0; 
    while (MAX_MOB > mob_num)  /* check all of the filters */   
    {
        CANPAGE = (mob_num << MOBNB0);	/* set MOb page number */                                                  
        *((uint8_t *)((&(filter)) + 1)) =							   
            CANIDT1 >> 5;	                    /* get cobId filter MSB */	       
        *((uint8_t *)(&(filter))) =								   
            (CANIDT2 >> 5) + (CANIDT1 << 3);	/* get cobId filter LSB */
        if (filter == can_id)
        /* present filter matches requested so release the hardware filter */
        {
            if (FIRST_CANIE1_MOB > mob_num)
            {
                CANIE2 &= ~(1 << mob_num);  /* disable MOb interrupt */
            }
            else
            {
                CANIE1 &= ~(1 << (mob_num - FIRST_CANIE1_MOB));  
                /* disable MOb interrupt */
            }
            CANIDT1 = 0x00;	/* clear cobId filter MSB */
            CANIDT2 = 0x00;	/* clear cobId filter LSB */
            CANCDMOB = 0x00;	/* disable MOb */
            CANSTMOB &= 0x00;	/* clear MOb status */
            result = 1;	/* a hardware filter has been released */
        }
        mob_num++;  
    }

	return (result);
}

/** ****************************************************************************
	This function returns the global CAN status variable 
	
	@warning none 
	
	@param none
	
	@return global CAN status variable
			bit 0: INIT - set after CAN has been successfully initialized
			bit 1: CERR - set if CAN bit or frame error occurred
			bit 2: ERPA - set if CAN "error passive" occurred
			bit 3: RXOR - set if Rx buffer overrun occurred
			bit 4: TXOR - set if Tx buffer overrun occurred
			bit 5: Reserved
			bit 6: TXBSY - set if Tx buffer is not empty
			bit 7: BOFF - set if CAN "bus off" error occurred  
*******************************************************************************/
uint8_t MCOHW_GetStatus(void)
{
	uint8_t result;
	
	if ((1 << ERRP) & CANGSTA) 
    {
        can_status |= HW_ERPA;	/* set ERPA status */
    }
	if ((1 << BOFF) & CANGSTA) 
    {
        can_status |= HW_BOFF; /* set BOFF status */
    }
    if ((1 << ENTX) & CANGIE)
    {
        can_status |= HW_TXBSY; /* set TXBSY status */
    }    
	result = can_status;
	can_status &= HW_INIT; /* clear all but INIT status */
	
	return (result);                                                                                                                                                                                                                                	
}

/** ****************************************************************************
	This function compares a timestamp to the value of the free running timer 
	
	@warning this function can only resolve expirations < 0x8000 (32 seconds)
	
	@param[in] time_stamp - the timestamp to be checked
	
	@return 1 if the timestamp has expired
			0 if the timestamp has not expired            
*******************************************************************************/
uint8_t MCOHW_IsTimeExpired(uint16_t time_stamp)
{
	uint8_t result;
    uint16_t time_now;
    
    result = 0;
    time_now = MCOHW_GetTime();
	if (time_now >= time_stamp)
    {
        if ((time_now - time_stamp) < 0x8000)
        {
            result = 1; /* timestamp has expired */  
        }
    }    
    else
    {
        if ((time_stamp - time_now) >= 0x8000)
        {
            result = 1; /* timestamp has expired */
        }
    }
        
	return (result); 
}

/** ****************************************************************************
	This function simulates a 16-bit free running msec timer 
	
	@warning units of (8KHz) can_timer are 125 usec 
	
	@param none
	
	@return 16-bit free running timer value (in msec)     
*******************************************************************************/
uint16_t MCOHW_GetTime(void)
{
	uint32_t msec_timer;
	
	do
	{
	    b_tovf = 0;
		*((uint8_t *)(&can_timer)) = CANTIML;
		*((uint8_t *)((&can_timer) + 1)) = CANTIMH;
		msec_timer = ((can_timer + 4) >> 3); /* round and divide by 8 */
	} 
	while (b_tovf); /* get time again if timer overflow interrupt occurred */
	
	return (msec_timer&0xFFFF);
}

/** ****************************************************************************
	This function attempts to move a CAN message from the Rx buffer into a 
	mailbox 
	
	@warning none 
	
	@param[in] p_mbox = pointer to CAN mailbox
	
	@return 1 if message was pulled from the Rx buffer and copied to the mailbox
			0 if Rx buffer was empty	            
*******************************************************************************/
uint8_t MCOHW_PullMessage(CAN_MSG * p_mbox)
{
	uint8_t result;
	CAN_MSG * p_buffer;
	
	p_buffer = CANRXFIFO_GetOutPtr();
	if (NULL != p_buffer)
	/* Rx buffer is not empty so pull CAN message into mailbox */
	{
		memcpy(p_mbox, p_buffer, sizeof(CAN_MSG)); /* copy message to mailbox */ 
		CANRXFIFO_OutDone();
		result = 1; /* a message was placed in mailbox */
	}
	else 
    {
        result = 0;	/* no message was placed in mailbox */
    }

	return (result);
}

/** ****************************************************************************
	This function attempts to copy a CAN message from a mailbox into the Tx 
	buffer
	
	@warning none
		
	@param[in] p_mbox = pointer to CAN mailbox
		
	@return 1 if msg was copied from the mailbox and pushed into the Tx buffer
			0 if Tx buffer was full              
*******************************************************************************/
uint8_t MCOHW_PushMessage(CAN_MSG * p_mbox)
{
	uint8_t result;
	uint8_t data_index;
	CAN_MSG * p_buffer;
	
	p_buffer = CANTXFIFO_GetInPtr();
	if (NULL != p_buffer)
	/* no overflow so push CAN message into Tx buffer */
	{
		memcpy(p_buffer, p_mbox, sizeof(CAN_MSG));  /* copy msg to Tx buffer */
		CANTXFIFO_InDone();
		result = 1; /* message was placed in Tx buffer */
		if (!((1 << ENTX) & CANGIE))
		/* transmit interrupt is not enabled so transmitter is not busy */
		{
			p_buffer = CANTXFIFO_GetOutPtr();
			CANPAGE = MAX_MOB << MOBNB0;		/* set MOb = Max (Tx) */
			CANSTMOB &= 0x00;	/* clear MOb status */
			CANIDT1 = ((*((uint8_t *)(&(p_buffer->ID)) + 1)) << 5) +
			    ((*(uint8_t *)(&(p_buffer->ID))) >> 3);     /* ID[10 - 3] */
			CANIDT2 = (*(uint8_t *)(&(p_buffer->ID))) << 5; /* ID[2 - 0] */
			CANIDT4 = 0x00;	/* clear RTR and RB0 */
			if (MAX_DLC < p_buffer->LEN)
            {
                p_buffer->LEN = MAX_DLC;
            }
            data_index = 0;
			while (data_index < p_buffer->LEN)
            {
                CANMSG = p_buffer->BUF[data_index];	/* load data */
                data_index++;
            }
			CANCDMOB = (p_buffer->LEN + (1 << CONMOB0)); /* start Tx */
			CANTXFIFO_OutDone();
   			CANGIE |= (1 << ENTX);	/* enable transmit interrupt */
            if (FIRST_CANIE1_MOB > MAX_MOB)
            /* enable CANIE2 MOb interrupt */
            {
                CANIE2 |= (1 << MAX_MOB);	
            }    
            else
            /* enable CANIE1 MOb interrupt */
            {
                CANIE1 |= (1 << (MAX_MOB - FIRST_CANIE1_MOB));	
            }     
		}
	}
	else 
	/* overflow */
	{
		can_status |= HW_TXOR; /* set TXOR - transmit overflow */
		result = 0; /* message was not placed in Tx buffer */
	}
	
	return (result);
}

#ifdef USE_MICROCAN_OPEN
/** ****************************************************************************
	This function is the service routine for CAN transmit and receive 
	interrupts
	
	@warning none
	
	@param none
	
	@return none
*******************************************************************************/
ISR(_VECTOR(18))
{
	uint8_t data_index;
	CAN_MSG * p_buffer;
	
	CANGIE &= ~(1 << ENIT); /* disable CAN interrupts */

	if ((1 << SIT0) & CANSIT2) 
	/* MOb_0 Rx interrupt handler */
	{
		CANPAGE = (0 << MOBNB0);		/* set MOb = 0 */
		READ_MOB();
	}
	if ((1 << SIT1) & CANSIT2)
	/* MOb_1 Rx interrupt handler */
	{
		CANPAGE = (1 << MOBNB0);		/* set MOb = 1 */
		READ_MOB();
	}
	if ((1 << SIT2) & CANSIT2)
	/* MOb_2 Rx interrupt handler */
	{
		CANPAGE = (2 << MOBNB0);		/* set MOb = 2 */
		READ_MOB();
	}
	if ((1 << SIT3) & CANSIT2)
	/* MOb_3 Rx interrupt handler */
	{
		CANPAGE = (3 << MOBNB0);		/* set MOb = 3 */
		READ_MOB();
	}
	if ((1 << SIT4) & CANSIT2)
	/* MOb_4 Rx interrupt handler */
	{
		CANPAGE = (4 << MOBNB0);		/* set MOb = 4 */
		READ_MOB();
	}
	if ((1 << SIT5)& CANSIT2)
	/* MOb_5 Rx/Tx interrupt handler */
	{
		CANPAGE = (5 << MOBNB0);		/* set MOb = 5 */
#ifndef _AT90
        WRITE_MOB();         /* service Tx interrupt */
    }    
#else
        READ_MOB();        /* service Rx interrupt */
	}
    if ((1 << SIT6) & CANSIT2)
    /* MOb_6 Rx interrupt handler */
    {
        CANPAGE = (6 << MOBNB0);		/* set MOb = 6 */
        READ_MOB();
    }   
    if ((1 << SIT7) & CANSIT2)
    /* MOb_7 Rx interrupt handler */
    {
        CANPAGE = (7 << MOBNB0);		/* set MOb = 7 */
        READ_MOB();
    }
    if ((1 << SIT8) & CANSIT1)
    /* MOb_8 Rx interrupt handler */
    {
        CANPAGE = (8 << MOBNB0);		/* set MOb = 8 */
        READ_MOB();
    }
    if ((1 << SIT9) & CANSIT1)
    /* MOb_9 Rx interrupt handler */
    {
        CANPAGE = (9 << MOBNB0);		/* set MOb = 9 */
        READ_MOB();
    }
    if ((1 << SIT10) & CANSIT1)
    /* MOb_10 Rx interrupt handler */
    {
        CANPAGE = (10 << MOBNB0);		/* set MOb = 10 */
        READ_MOB();
    }
    if ((1 << SIT11) & CANSIT1)
    /* MOb_11 Rx interrupt handler */
    {
        CANPAGE = (11 << MOBNB0);		/* set MOb = 11 */
        READ_MOB();
    }
    if ((1 << SIT12) & CANSIT1)
    /* MOb_12 Rx interrupt handler */
    {
        CANPAGE = (12 << MOBNB0);		/* set MOb = 12 */
        READ_MOB();
    }
    if ((1 << SIT13) & CANSIT1)
    /* MOb_13 Rx interrupt handler */
    {
        CANPAGE = (13 << MOBNB0);		/* set MOb = 13 */
        READ_MOB();
    }
    if ((1 << SIT14) & CANSIT1)
    /* MOb_14 Tx interrupt handler */
    {
        CANPAGE = (14 << MOBNB0);		/* set MOb = 14 */
        WRITE_MOB();
    }    
#endif
	CANGIE |= (1 << ENIT);  /* enable CAN interrupts */
}

/** ****************************************************************************
	This function is the service routine for CAN timer overflow interrupt
	
	@warning this function interrupts once every 8.2 seconds  
	
	@param none
	
	@return none
*******************************************************************************/
ISR(_VECTOR(19))
{
	b_tovf = 1;
	can_timer += OVERFLOW_COUNT;
}
#endif /* USE_MICROCAN_OPEN */
/*----------------------------- END OF FILE ----------------------------------*/
