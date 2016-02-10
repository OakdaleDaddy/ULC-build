
//---------------------------------------------------------------------
//
//	Module       : TIMER0.C
//
//  Project      : PCAN-GPS
//
//  Version/Date : 1.0 , 06/2014
//
//  Copyright (c): PEAK-SYSTEM TECHNIK GMBH, DARMSTADT
//
//---------------------------------------------------------------------
/*******************************************************************************
 include files
 ******************************************************************************/
//
// System header files
//

//
// Library header files
//
#include <system_LPC407x_8x_177x_8x.h>
#include <lpc407x_8x_177x_8x.h>
//
// Source code header files
//
#include "typedefs.h"
#include "hardware.h"
#include "timer.h"


/*******************************************************************************
 global definitions
 ******************************************************************************/


/*******************************************************************************
 local definitions
 ******************************************************************************/
extern void Timer_1000usec(void);


/*******************************************************************************
 local function prototypes
 ******************************************************************************/
void  TIMER0_IRQHandler (void);

/*******************************************************************************
 global functions
 ******************************************************************************/


//------------------------------------------------------------------------------
//! void Init_Timer0 (void)
//------------------------------------------------------------------------------
//! @brief	initializes timer 0 as system timer
//------------------------------------------------------------------------------
void Init_Timer0 (void){
	NVIC_DisableIRQ ( TIMER0_IRQn);
	
	LPC_SC->PCONP  |= 1 << 1; 	// Power on for Timer0

	LPC_TIM0->TCR = 2;					// Timer0 stop

	// Set Prescaler
	LPC_TIM0->PR = 120; 				// Divide PCLK=120MHz by 120
	//from here Timer0 = 1MHz = 1usec
	
	LPC_TIM0->MCR = 1<<0;

	LPC_TIM0->MR0 = SYSTIME_NOW + 1000;
	
	
	// start timer
	LPC_TIM0->IR 	= 0xFFFFFFFF;	// Clear all interrupts
	LPC_TIM0->TCR = 1;					// Timer0 start
	
	NVIC_SetPriority ( TIMER0_IRQn, 0);
	NVIC_EnableIRQ ( TIMER0_IRQn);
}


//------------------------------------------------------------------------------
//! void  TIMER0_IRQHandler (void)
//------------------------------------------------------------------------------
//! @brief	ISR for timer 0
//------------------------------------------------------------------------------
void  TIMER0_IRQHandler (void)
{
	if ( LPC_TIM0->IR & 1) 	// Timer0 interrupt
	{
		// update match value for next interrupt
		LPC_TIM0->MR0 = SYSTIME_NOW + 1000;

		LPC_TIM0->IR  = 0x00000001;	// ack this interrupts

		Timer_1000usec();			// process 1000usec task in main.c
	}
	else
	{
		LPC_TIM0->IR  = 0x0000003E;		// ack all other interrupts
	}
}


//------------------------------------------------------------------------------
//! void	Wait_Usec (u32_t  Usec)
//------------------------------------------------------------------------------
//! @brief	waits n microseconds (blocking).
//------------------------------------------------------------------------------
void Wait_Usec (u32_t  Usec){
	u32_t  TimeStart, TimeNow;

	if (Usec) 
	{
		TimeStart = SYSTIME_NOW;	

		do 
		{
			TimeNow = SYSTIME_NOW;
		} 
		while ((SYSTIME_DIFF (TimeStart, TimeNow)) < Usec);
	}
}

