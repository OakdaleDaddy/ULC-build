/*------------------------------------------------------------------------/
/  LPC176x RTC control module
/-------------------------------------------------------------------------/
/
/  Copyright (C) 2011, ChaN, all right reserved.
/
/ * This software is a free software and there is NO WARRANTY.
/ * No restriction on use. You can use, modify and redistribute it for
/   personal, non-profit or commercial products UNDER YOUR RESPONSIBILITY.
/ * Redistributions of source code must retain the above copyright notice.
/
/-------------------------------------------------------------------------*/

#include <lpc407x_8x_177x_8x.h>
#include "rtc.h"
#include "hardware.h"


int rtc_initialize (void)
{
	/* Enable PCLK to the RTC */
	LPC_SC->PCONP |= 1<<9;

	/* Start RTC with external XTAL */
	LPC_RTC->CCR = 0x11;

	LPC_RTC->RTC_AUX = 1<<4;
	return 1;
}



int rtc_gettime (RTC *rtc)
{
	DWORD d, t;


	do {
		t = LPC_RTC->CTIME0;
		d = LPC_RTC->CTIME1;
	} while (t != LPC_RTC->CTIME0 || d != LPC_RTC->CTIME1);

	if(LPC_RTC->RTC_AUX & 1<<4){
		rtc->sec = 0;
		rtc->min = 0;
		rtc->hour = 0;
		rtc->wday = 0;
		rtc->mday = 1;
		rtc->month = 1;
		rtc->year = 2011;
		return 0;
	}

	rtc->sec = t & 63;
	rtc->min = (t >> 8) & 63;
	rtc->hour = (t >> 16) & 31;
	rtc->wday = (t >> 24) & 7;
	rtc->mday = d & 31;
	rtc->month = (d >> 8) & 15;
	rtc->year = (d >> 16) & 4095;
	return 1;
}




int rtc_settime (const RTC *rtc)
{
	LPC_RTC->CCR = 0x12;		/* Stop RTC */

	/* Update RTC registers */
	LPC_RTC->SEC = rtc->sec;
	LPC_RTC->MIN = rtc->min;
	LPC_RTC->HOUR = rtc->hour;
	LPC_RTC->DOW = rtc->wday;
	LPC_RTC->DOM = rtc->mday;
	LPC_RTC->MONTH = rtc->month;
	LPC_RTC->YEAR = rtc->year;

	LPC_RTC->RTC_AUX = 1<<4;	/* Clear power fail flag */
	LPC_RTC->CCR = 0x11;			/* Restart RTC, Disable calibration feature */

	return 1;
}


