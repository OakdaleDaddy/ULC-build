/*
 * CommonFunctions.c
 *
 * Created: 9/30/2015 7:11:56 PM
 *  Author: ULC Robotics
 */ 

#include "CommonFunctions.h"
#include "ServoControl.h"


#if 0

void IO_Write(unsigned char addr, unsigned char OutData){
	volatile unsigned char x = 0;
	unsigned char temp = addr;
	temp = temp << 4;
	temp = temp & 0b11110000;
	PORTC = PORTC & 0b00001111;
	PORTC = PORTC | temp;
	DDRA = 0xFF;
	PORTA = OutData;
	PORTG = PORTG & 0b11111110;
	for(x = 0; x<10; x++);
	x = 0;
	PORTG = PORTG | 0b00000001;
	for(x = 0; x<10; x++);
	x = 0;
}

#endif


void IO_Write(unsigned char addr, unsigned char OutData){
	volatile unsigned char x = 0;
	unsigned char temp = addr;
	temp = temp << 4;
	temp = temp & 0b11110000;
#if 1
	PORTC = temp | (PORTC & 0b00001111);
#endif
#if 0
	PORTC = PORTC & 0b00001111;
	PORTC = PORTC | temp;
#endif	
	PORTB &=0b11111110;
	DDRA = 0xFF;
	PORTA = OutData;
	PORTG = PORTG & 0b11111110;
	for(x = 0; x<50; x++);
	x = 0;
	PORTG = PORTG | 0b00000001;
	for(x = 0; x<50; x++);
	x = 0;
	PORTC &=0b00001111;
}




/*
unsigned char IO_Read(unsigned char addr){
	volatile unsigned char x = 0;
	//unsigned char invalue = 0;
	unsigned char invalue = 0x55;
	unsigned char temp = addr;
	temp = temp << 4;
#if 1
	temp = temp & 0b11110000;
	PORTC = temp | (PORTC & 0b00001111);
#endif
#if 0
	temp = temp & 0b11110000; // todo fix to protect b0 of PORTC
	PORTC = PORTC & 0b00001111;
	PORTC = temp;
#endif
	DDRA = 0x00;
	PORTA = 0x00;
	PORTG = PORTG & 0b11111101;
	for(x = 0; x<10; x++);
	x = 0;
	invalue = PINA;
	PORTG = PORTG | 0b00000010;
	DDRA = 0xff;
	for(x = 0; x<10; x++);
	x = 0;
	return(invalue);
}

*/

unsigned char IO_Read(unsigned char addr){
	volatile unsigned char x = 0;
	unsigned char invalue = 0;
	unsigned char temp = addr;
	temp = temp << 4;
	temp = temp & 0b11110000;
#if 1
	PORTC = temp | (PORTC & 0b00001111);
#endif
#if 0	
	PORTC = temp & 0b00001111;
	PORTC |= temp;
	temp = temp & 0b11110000; // todo fix to protect b0 of PORTC
#endif	
	PORTB |= 0b00000001;
	DDRA = 0x00;
	PORTA = 0x00;
	PORTG = PORTG & 0b11111101;
	for(x = 0; x<50; x++);
	x = 0;
	invalue = PINA;
	PORTG = PORTG | 0b00000010;
	DDRA = 0xff;
	for(x = 0; x<50; x++);
	x = 0;
	PORTC &=0b00001111;
	return(invalue);
}



void canled(unsigned char ledstate)
{
	PORTC &=0xfe;
	PORTC |= ledstate;
}

void ledblink(){

	canled(1);
	timedelay();
	canled(0);
	timedelay();
}

void HobbyDriveInit(void){
	
}


/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */

//  position = 0..35999
void Set_Sensor_Position(U16 position, U16 maxRate, U16 accelleration)
{
}

// gets position
U16 Get_Sensor_Position(void)
{	
	return(0);	
}

// Accelerometer initialization.  This must be called before the accelerometer may be used.
unsigned char accelerometer_init(void){
	DDRB |= 0b00000110;
	IO_Write(0x0b,1);
	SPCR |=0b01011111;
	accel_write(0x20, 0x67);
	return(accel_read(0x0f));
}

// Get identity of accelerometer should return 0x49 for ST LSM90DS0
unsigned char accelerometer_identify(void){
	return accel_read(0x0f);
}

// get x-acceleration
int accelerometer_get_x(void){
	unsigned char l = 0;
	unsigned char h = 0;
	int accelrate = 0;
	l = accel_read(0x28);
	h = accel_read(0x29);
	accelrate = (h<<8);
	accelrate |=l;
	return(accelrate);
}

// get y-acceleration
int accelerometer_get_y(void){
	unsigned char l = 0;
	unsigned char h = 0;
	int accelrate = 0;
	l = accel_read(0x2A);
	h = accel_read(0x2B);
	accelrate = (h<<8);
	accelrate |=l;
	return(accelrate);
}


// Get z-acceleration
int accelerometer_get_z(void){
	unsigned char l = 0;
	unsigned char h = 0;
	int accelrate = 0;
	l = accel_read(0x2C);
	h = accel_read(0x2D);
	accelrate = (h<<8);
	accelrate |=l;
	return(accelrate);
}


unsigned char accel_read(unsigned char accelcmd){
	unsigned char accelcmd1 = 0;
	unsigned char x = 0;
	accelcmd1 = (0xc0 | (accelcmd & 0x3f));
	IO_Write(0x0b,0);
	SPDR = accelcmd1;
	while (!(SPSR & (1<<SPIF)));
	SPDR = 0x00;
	while (!(SPSR & (1<<SPIF)));
	x = SPDR;
	IO_Write(0x0b,1);
	return(x);
}


unsigned char accel_write(unsigned char acceladdr, unsigned char acceldata){
	IO_Write(0x0b,0);
	SPDR = (acceladdr & 0x3f);
	while (!(SPSR & (1<<SPIF)));
	SPDR = acceldata;
	while (!(SPSR & (1<<SPIF)));
	IO_Write(0x0b,1);
	return (0);
}
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
/* !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! */
