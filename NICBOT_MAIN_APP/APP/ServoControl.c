/*
 * ServoControl.c
 *
 * Created: 9/30/2015 6:53:43 PM
 *  Author: ULC Robotics
 */ 

#define LFIL 0x1E
#define LTRJ 0x1F
#define PORT8 0x05
#define LPEI 0x1b
#define LPES 0x1A
#define STT 0x01
#define RDDP 0x08
#define RDRP 0x0A
#define DFH 0x02
#define SBPA 0x20
#define SBPR 0x21
#define UDF 0x04
#define RDSIGS 0x0c
#define RDDB 0x07
#define RDRV 0x0b
#define RDSUM 0x0d
#define MSKI 0x1C
#define RSTI 0x1D

#define F_CPU 16E6

#include "ServoControl.h"
#include "CommonFunctions.h"
#include <util/delay.h>
unsigned char axis = 0;
unsigned char acceldata[4] = {0,0,0,0};
unsigned char veldata[4] = {0,0,0,0};
unsigned char posdata[4] = {0,0,0,0};
unsigned char kp[2] = {0,100};
unsigned char ki[2] = {0,15};
unsigned char kd[2] = {3,0};
unsigned char il[2] = {0,15};	
			
void servo_delay(void);
void Servo_Write(unsigned char OutData);
void servo_write_cmd( unsigned char axisnum, unsigned char cmd);
void servo_write_word(unsigned int servodata);
void busywait(unsigned char axisnum);
unsigned char servo_read_char(void);
unsigned char bitmask = 0;
unsigned char axistemp = 0;
void servo_write_data( unsigned char axis, unsigned char cmd);
void servo_write_cmd( unsigned char axis, unsigned char cmd);
unsigned char servo_read_data(unsigned char axis);
unsigned char servo_read_cmd(unsigned char axis);


// initialize servo control

unsigned char servo_init(unsigned char axisnum){
	unsigned char x =0;
	servo_write_cmd(axisnum,LFIL);
	servo_write_data(axisnum,0x00);
	servo_write_data(axisnum,0x0F);
	servo_write_data(axisnum,0);
	servo_write_data(axisnum,30);
	servo_write_data(axisnum,0);
	servo_write_data(axisnum,15);
	servo_write_data(axisnum,2);
	servo_write_data(axisnum,0xff);
	servo_write_data(axisnum,0);
	servo_write_data(axisnum,15);
    servo_write_cmd(axisnum, UDF);
	servo_write_cmd(axisnum, DFH);
	servo_write_cmd(axisnum, LTRJ);
	servo_write_data(axisnum,0b00000000);
	servo_write_data(axisnum,0b00101010);
	
	servo_write_data(axisnum,0);
    servo_write_data(axisnum,0);
    servo_write_data(axisnum,0x0e);
    servo_write_data(axisnum,0xff);
	
	servo_write_data(axisnum,0);
	servo_write_data(axisnum,0);
	servo_write_data(axisnum,0x0f);
	servo_write_data(axisnum,0xff);
	
	servo_write_data(axisnum,0);
	servo_write_data(axisnum,0);
	servo_write_data(axisnum,0);
	servo_write_data(axisnum,0x01);
	servo_write_cmd(axisnum, STT);

	x = servo_read_cmd(axisnum);
	return(x);

}

// store acceleration rate

unsigned char servo_load_accel(unsigned char r0, unsigned char r1, unsigned char r2, unsigned char r3){
	acceldata[0] = r0;
	acceldata[1] = r1;
	acceldata[2] = r2;
	acceldata[3] = r3;
	return(0);
}

// store velocity

unsigned char servo_load_velocity(unsigned char r0, unsigned char r1, unsigned char r2, unsigned char r3){
	veldata[0] = r0;
	veldata[1] = r1;
	veldata[2] = r2;
	veldata[3] = r3;
	return(0);
}

// store position

unsigned char servo_load_position(unsigned char r0, unsigned char r1, unsigned char r2, unsigned char r3){
	posdata[0] = r0;
	posdata[1] = r1;
	posdata[2] = r2;
	posdata[3] = r3;
	return(0);
}


// Move to absolute position
unsigned char servo_move_abs(unsigned char axisnum)
{
	unsigned char x =0;
	servo_write_cmd(axisnum, LTRJ);
	servo_write_data(axisnum,0b00000000);
	servo_write_data(axisnum,0b00101010);
	servo_write_data(axisnum, acceldata[0]);
    servo_write_data(axisnum, acceldata[1]);
	servo_write_data(axisnum, acceldata[2]);
	servo_write_data(axisnum, acceldata[3]);
	
    servo_write_data(axisnum, veldata[0]);
	servo_write_data(axisnum, veldata[1]);
	servo_write_data(axisnum, veldata[2]);
    servo_write_data(axisnum, veldata[3]);
	
	servo_write_data(axisnum, posdata[0]);
    servo_write_data(axisnum, posdata[1]);
    servo_write_data(axisnum, posdata[2]);
    servo_write_data(axisnum, posdata[3]);
	servo_write_cmd(axisnum, STT);
	x = servo_read_cmd(axisnum);
	return(x);
}

// Move to absolute position while moving
unsigned char servo_move_abs_while_moving(unsigned char axisnum)
{
    unsigned char x =0;
    servo_write_cmd(axisnum, LTRJ);
    servo_write_data(axisnum,0b00000000);
    servo_write_data(axisnum,0b00000010);    
    servo_write_data(axisnum, posdata[0]);
    servo_write_data(axisnum, posdata[1]);
    servo_write_data(axisnum, posdata[2]);
    servo_write_data(axisnum, posdata[3]);
    servo_write_cmd(axisnum, STT);
    x = servo_read_cmd(axisnum);
    return(x);
}

// load velocity while moving
unsigned char servo_set_velocity_while_moving(unsigned char axisnum)
{
	unsigned char x =0;
	servo_write_cmd(axisnum, LTRJ);
	servo_write_data(axisnum,0b00000000);
	servo_write_data(axisnum,0b00001000);
	
	servo_write_data(axisnum, veldata[0]);
	servo_write_data(axisnum, veldata[1]);
	servo_write_data(axisnum, veldata[2]);
	servo_write_data(axisnum, veldata[3]);
	servo_write_cmd(axisnum, STT);
	x = servo_read_cmd(axisnum);
	return(x);
}

// Move to relative position

unsigned char servo_move_rel(unsigned char axisnum){
	unsigned char x =0;
	servo_write_cmd(axisnum, LTRJ);
	servo_write_data(axisnum,0b00000000);
	servo_write_data(axisnum,0b00101011);
	servo_write_data(axisnum, acceldata[0]);
	servo_write_data(axisnum, acceldata[1]);
	servo_write_data(axisnum, acceldata[2]);
	servo_write_data(axisnum, acceldata[3]);
	
	servo_write_data(axisnum, veldata[0]);
	servo_write_data(axisnum, veldata[1]);
	servo_write_data(axisnum, veldata[2]);
	servo_write_data(axisnum, veldata[3]);
	
	servo_write_data(axisnum, posdata[0]);
	servo_write_data(axisnum, posdata[1]);
	servo_write_data(axisnum, posdata[2]);
	servo_write_data(axisnum, posdata[3]);
	servo_write_cmd(axisnum, STT);
	x = servo_read_cmd(axisnum);
	return(x);
}

// Set position error for stopping
// axisnum = number of axis, sensor spinner is axis 0, pos_error is the maximum allowable position error
// in encoder pulses.  This is a value between 1 & 32768.
unsigned char servo_set_error(unsigned char axisnum, unsigned int pos_error){
	
	unsigned int temp = pos_error;
	unsigned char r0 = 0;
	unsigned char r1 = 0;
	r0 = (unsigned char)temp;
	r1 = (unsigned char)(temp >>8);
	servo_write_cmd(axisnum, LPES);
	servo_write_data(axisnum, r1);
	servo_write_data(axisnum, r0);
	return (0);	
}

// Stop with -acceleration as programmed

unsigned char servo_stop_decel(unsigned char axisnum){
	unsigned char x =0;
	servo_write_cmd(axisnum, LTRJ);
	servo_write_data(axisnum,0b00000100);
	servo_write_data(axisnum,0b00000000);
	servo_write_cmd(axisnum, STT);
	x = servo_read_cmd(axisnum);
	return(x);
}


unsigned char servo_motor_off(unsigned char axisnum){
	unsigned char x =0;
	servo_write_cmd(axisnum, LTRJ);
	servo_write_data(axisnum,0b00000001);
	servo_write_data(axisnum,0b00000000);
	servo_write_cmd(axisnum, STT);
	x = servo_read_cmd(axisnum);
	return(x);
}


// Stop with maximum deceleration
unsigned char servo_stop_abrupt(unsigned char axisnum){
	unsigned char x =0;
	servo_write_cmd(axisnum, LTRJ);
	servo_write_data(axisnum,0b00000010);
	servo_write_data(axisnum,0b00000000);
	servo_write_cmd(axisnum, STT);
	x = servo_read_cmd(axisnum);
	return(x);
}

// Set origin
unsigned char servo_set_origin(unsigned char axisnum){
	unsigned char x=0;
	servo_write_cmd(axisnum,DFH);
		x = servo_read_cmd(axisnum);
		return(x);
	
}

// Get encoder position
unsigned char* servo_get_position(unsigned char axisnum){
	
	static unsigned char servodata[6];
	//unsigned char* servodata = malloc(sizeof(char) *10);
	
//servo_write_cmd(axisnum, RDDP);
servo_write_cmd(axisnum, RDRP);
servodata[0] = servo_read_data(axisnum);
servodata[1] = servo_read_data(axisnum);
servodata[2] = servo_read_data(axisnum);
servodata[3] = servo_read_data(axisnum);
servodata[4] =0x05;
servodata[5] = servo_read_cmd(axisnum);
return(servodata);
}


/*
unsigned char* servo_get_status(unsigned char axisnum){
	unsigned char x = servo_read_cmd(axisnum);
	return(x);
}
*/

// Get status 
// b0=1 is busy, b1=1 is command error, b2=1 when end of trajectory reached, b3=1 not used, b4=1 wrap around (not likely), b5=1 excessive position error, b6=1break point reached, b7=motor off
unsigned char servo_get_status(unsigned char axisnum){
	unsigned char x = servo_read_cmd(axisnum);
	return(x);
}

unsigned char servo_reset(unsigned char axisnum){
	unsigned char x =0;
	servo_write_cmd(axisnum,0);
	servo_write_cmd(axisnum, 0x1d);
	servo_write_data(axisnum,0x00);
	servo_write_data(axisnum, 0x00);
	x = servo_read_cmd(axisnum);
	return(x);
}


void SetPID(unsigned char axisnum, unsigned int kp, unsigned int ki, unsigned int kd, unsigned int il){
	axis = axisnum;
	servo_write_cmd(axis, 0xff); // Set derivative sampling rate
	servo_write_cmd(axis, 0x0f); // kp, ki, kd, il will be written
	servo_write_word(kp);
	servo_write_word(ki);
	servo_write_word(il);	
}

void servo_write_cmd( unsigned char axis, unsigned char cmd)
{
	unsigned char servoaddr = 0;

	if (!(axis & 0xFE)) // make sure axis = 0 or 1
	{
		if(axis == 0) servoaddr = 0x08;
		if(axis == 1) servoaddr = 0x09;
		PORTG &= 0b11111011;
		IO_Write(servoaddr, cmd);
		busywait(axis);	
	}
}


void servo_write_data( unsigned char axis, unsigned char cmd)
{
	unsigned char servoaddr = 0;

	if (!(axis & 0xFE)) // make sure axis = 0 or 1
	{
		if(axis == 0) servoaddr = 0x08;
		if(axis == 1) servoaddr = 0x09;
		PORTG |= 0b00000100;
		IO_Write(servoaddr, cmd);
		busywait(axis);
	}
}


unsigned char servo_read_data(unsigned char axis)
{
	unsigned char servodata = 0;
	unsigned char servoaddr= 0;

	if (!(axis & 0xFE)) // make sure axis = 0 or 1
	{	
		if(axis == 0) servoaddr =0x08;
		if(axis == 1) servoaddr =0x09;
		PORTG |= 0b00000100;
		servodata = IO_Read(servoaddr);
		busywait(axis); // last change made after functional
	}
	return(servodata);
}


unsigned char servo_read_cmd(unsigned char axis)
{
	unsigned char servodata = 0;
	unsigned char servoaddr= 0;

	if (!(axis & 0xFE)) // make sure axis = 0 or 1
	{
		if(axis == 0) servoaddr =0x08;
		if(axis == 1) servoaddr =0x09;
		PORTG &= 0b11111011;
		servodata = IO_Read(servoaddr);
	}
	return(servodata);
}



unsigned char set_kp(unsigned char axisnum, unsigned char r0, unsigned char r1){
	unsigned char x =0;
	servo_write_cmd(axisnum,LFIL);
	servo_write_data(axisnum,0x00);
	servo_write_data(axisnum,0b00001000);
	servo_write_data(axisnum,r0);
	servo_write_data(axisnum, r1);
	servo_write_cmd(axisnum, UDF);
	x = servo_read_cmd(axisnum);
	return(x);
}

unsigned char set_ki(unsigned char axisnum, unsigned char r0, unsigned char r1){
	unsigned char x =0;
	servo_write_cmd(axisnum,LFIL);
	servo_write_data(axisnum,0x00);
	servo_write_data(axisnum,0b00000100);
	servo_write_data(axisnum,r0);
	servo_write_data(axisnum, r1);
	servo_write_cmd(axisnum, UDF);
	x = servo_read_cmd(axisnum);
	return(x);
}



unsigned char set_kd(unsigned char axisnum, unsigned char r0, unsigned char r1){
	unsigned char x =0;
	servo_write_cmd(axisnum,LFIL);
	servo_write_data(axisnum,0x00);
	servo_write_data(axisnum,0b00000010);
	servo_write_data(axisnum,r0);
	servo_write_data(axisnum, r1);
	servo_write_cmd(axisnum, UDF);
	x = servo_read_cmd(axisnum);
	return(x);
}




unsigned char set_il(unsigned char axisnum, unsigned char r0, unsigned char r1){
	unsigned char x =0;
	servo_write_cmd(axisnum,LFIL);
	servo_write_data(axisnum,0x00);
	servo_write_data(axisnum,0b00000001);
	servo_write_data(axisnum,r0);
	servo_write_data(axisnum, r1);
	servo_write_cmd(axisnum, UDF);
	x = servo_read_cmd(axisnum);
	return(x);
}



void servo_hard_reset(void){
	IO_Write(0x0a, 0xFF);
	PORTE &= 0b11111011;
	_delay_ms(1);
	PORTE |= 0b00000100;
	_delay_ms(1);
}

void busywait(unsigned char axisnum){
	
#if 0

	volatile int t;
	volatile unsigned char status;

	for(t=0; t<2000; t++)
	{
		status = servo_get_status(axisnum);
		
		if ((status & 0x01) != 0)
		{
			break;
		}

		_delay_us(1);
	}

	for(t=0; t<2000; t++)
	{
		status = servo_get_status(axisnum);
		
		if ((status & 0x01) == 0)
		{
			break;
		}

		_delay_us(1);
	}

#endif	
	
#if 1
	volatile int t = 0;
	for(t=0; t<1000; t++);
#endif    
}


void servo_delay(void){
	volatile unsigned int x = 0;
for(x = 0; x<1000; x++);
	 
}


void Servo_Write(unsigned char OutData){
	volatile unsigned char x = 0;
		bitmask &= 0b11101111;
		bitmask |= 0b00001000;
		IO_Write(5,bitmask);
    DDRA = 0xFF;
	PORTA = OutData;
	PORTG = PORTG & 0b11111110;
	for(x = 0; x<50; x++);
	x = 0;
	PORTG = PORTG | 0b00000001;
	for(x = 0; x<200; x++);
	x = 0;
	bitmask &= 0b11110111;
	IO_Write(5,bitmask);
}


unsigned char servo_read_char(){
//	unsigned char tempdata = 0;
	bitmask &= 0b11110111;
	bitmask |= 0b00010000;
	IO_Write(5, bitmask);
	servo_delay();
//		volatile unsigned char x = 0;
		unsigned char invalue = 0;
		DDRA = 0x00;
		PORTA = 0x00;
		PORTG = PORTG & 0b11111101;
		servo_delay();
	//	x = 0;
		invalue = PINA;
		PORTG = PORTG | 0b00000010;
	//	DDRA = 0xff;
		servo_delay();
	//	x = 0;
		DDRA =0xff;
		bitmask &=0b11101111;
		IO_Write(5,bitmask);
		return(invalue);
	}







void timedelay(){
	_delay_ms(1000);
}


void DrillInit(void){
	DDRB |=0b11110000;
	 TCCR1A |= 0b00000011;
	TCCR1B |= 0b00001100;
	  OCR1AH= 0x30;
	  OCR1AL = 0x00;
	  OCR1BH=0x30;
	  OCR1BL = 0x00;
	  
	  }

/*
void drillspeed(unsigned char drillspeedh, unsigned char drillspeedl){

	//	TCCR1A |= 1<<WGM11 | 1<<COM1A1 | 1<<COM1A0;
	//	TCCR1B |= 1<<WGM13 | 1<<WGM12 | 1<<CS10 | 1<<CS12;
	if(drillspeedl ==0){
		OCR1AH = 0x30;
		OCR1AL = 0x00;
		TCCR1A &=0b00111111;
	}
	
	else {
		
		// TCCR1A |= 0b10000000;
		OCR1AL = drillspeedl;
		OCR1AH = 0x30;
		TCCR1A |= 0b10000000;
	}
}
*/

void drillspeed(unsigned char axisnum, unsigned char drillspeedh, unsigned char drillspeedl){

//	TCCR1A |= 1<<WGM11 | 1<<COM1A1 | 1<<COM1A0;
//	TCCR1B |= 1<<WGM13 | 1<<WGM12 | 1<<CS10 | 1<<CS12;
if(axisnum == 0){
	if(drillspeedl ==0){ 
		OCR1AH = 0x30;
		OCR1AL = 0x00;
		TCCR1A &=0b00111111;
	}
	
	else {
			
   // TCCR1A |= 0b10000000;
	OCR1AL = drillspeedl;
	OCR1AH = 0x30;
	TCCR1A |= 0b10000000;
	}
	
}
	
if(axisnum == 1){
	if(drillspeedl ==0){
		OCR1BH = 0x30;
		OCR1BL = 0x00;
		TCCR1A &=0b11001111;
	}
	
	else {
		
		// TCCR1A |= 0b10000000;
		OCR1BL = drillspeedl;
		OCR1BH = 0x30;
		TCCR1A |= 0b00100000;
	}
	
}	
	
	
}

