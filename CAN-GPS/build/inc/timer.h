/***************************************************************************//**
* @file		timer.h
* @brief	User level timer functions
* @version	1.0.0
* @date		18. July 2014
* @author	PEAK-SYSTEM TECHNIK
*
* Copyright (c): PEAK-SYSTEM TECHNIK GMBH, DARMSTADT
* *****************************************************************************/

#ifndef _TIMER0_H_
#define _TIMER0_H_

#define	SYSTIME_t						u32_t
#define	SYSTIME_MAX						0xFFFFFFFF
#define	SYSTIME_NOW						(LPC_TIM0->TC)
#define	SYSTIME_DIFF( _First, _Second)	(( _First <= _Second ? ( _Second - _First) : (( SYSTIME_MAX - _First) + _Second)))

void Init_Timer0 (void);
void Wait_Usec (u32_t  Usec);

#endif
