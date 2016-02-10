
#include "datatypes.h"
#include "systime.h"
#include "lpc21xx.h"


// wait for 'Time' microseconds
void  SYSTIME_WaitMicros ( SYSTIME_t  Time)
{

	if ( Time)
	{
		SYSTIME_t  StartTime, timenow;


		StartTime = SYSTIME_NOW;

		do {
			timenow = SYSTIME_NOW;
		} while ( SYSTIME_DIFF ( StartTime, timenow) < Time);
	}
}


// init timer 1 as systemtimer
void  SYSTIME_Init ( void)
{

	// Timer config, 1 µs Resolution

	// Timer halt
	T1TCR = 1 << 1;

	// Set Prescaler
   T1PR = 59; // init-value for 60MHz CPU-Clock
   //T1PR = 47;  // init-value for 48MHz CPU-Clock

	// no Matches
	T1MCR = 0;

	//no Capture
	T1CCR = 0;

	// no external Toggles
	T1EMR = 0;

	// Clear all interrupts
	// we do not use interrupts
	T1IR = 0xFF;

	// Timer start
	T1TCR = 1;

}

