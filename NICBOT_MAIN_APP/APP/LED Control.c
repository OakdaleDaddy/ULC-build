/*
 * LED_Control.c
 *
 * Created: 10/13/2015 3:43:18 PM
 *  Author: ULC Robotics
 */ 
#ifndef F_CPU
#define F_CPU 16000000UL
#endif

#include <stdint.h>
#include "i2c_master.h"
#include "LED_Control.h"
#include "CommonFunctions.h"
#include <util/delay.h>
// #include <util/twi.h>





void Init_I2C(){
	i2c_init();
	}

	
void LED_Intensity(unsigned char LED_Node, unsigned char intensity){
	unsigned char LEDaddr = 0;
	switch(LED_Node){
		case 1:
		LEDaddr = 0x22;
		break;
		
		case 2:
	    LEDaddr = 0x24;		
		break;
		
		case 3:
		LEDaddr = 0x26;
		break;
		
		case 4:
		LEDaddr = 0x28;
		break;
		
		case 5:
		LEDaddr = 0x2a;
		break;
		
		case 6:
		LEDaddr = 0x2c;
		break;
		
		case 7:
		LEDaddr = 0x2E;	
		break;
		
		case 8:
		LEDaddr = 0x30;
		break;
		
		case 9:
		LEDaddr = 0x32;
		break;
		
		case 10:
		LEDaddr = 0x34;	
		break;
			
		case 11:
		LEDaddr = 0x36;
		break;
			
		case 12:
		LEDaddr = 0x38;
		break;
			
		case 13:
		LEDaddr = 0x3a;
		break;
			
		case 14:
		LEDaddr = 0x3c;
		break;
		
		case 15:
		LEDaddr = 0x3e;
		break;
		
		default:
		return;
		break;						
			
	}
				
	
	i2c_init();
	i2c_start(LEDaddr);
	i2c_write(~intensity);
	i2c_stop();
	return;
}


void Camera_Select(unsigned char r2, unsigned char r3){
  unsigned char Monitor1 = r2;
  switch(Monitor1){
	  
	  case 1:
	  IO_Write(0x02, 0x18);
	  break;
	  
	  case 2:
	  IO_Write(0x02, 0x19);
	  break;
	  
	  case 3:
	  IO_Write(0x02, 0x1A);
	  break;
	  
	  case 4:
	  IO_Write(0x02, 0x1B);
	  break;
	  
	  case 5:
	  IO_Write(0x02, 0x1C);
	  break;
	  
	  case 6:
	  IO_Write(0x02, 0x1D);
	  break;
	  
	  case 7:
	  IO_Write(0x02, 0x28);
	  break;
	  
	  case 8:
	  IO_Write(0x02, 0x29);
	  break;
	  
	  case 9:
	  IO_Write(0x02, 0x2A);
	  break;
	  
	  case 10:
	  IO_Write(0x02, 0x2B);
	  break;
	  
	  case 11:
	  IO_Write(0x02, 0x2C);
	  break;
	  
	  case 12:
	  IO_Write(0x02, 0x2D);
	  break;
  }
  

  unsigned char Monitor2 = r3;
  
  switch(Monitor2){
	  case 1:
	  IO_Write(0x03, 0x1B);
	  break;
  
	  case 2:
	  IO_Write(0x03, 0x1A);
	  break;
	  
	  case 3:
	  IO_Write(0x03, 0x19);
	  break;
	  
	  case 4:
	  IO_Write(0x03, 0x18);
	  break;
	  
	  case 5:
	  IO_Write(0x03, 0x1D);
	  break;
	  
	  case 6:
	  IO_Write(0x03, 0x1C);
	  break;
	  
	  case 7:
	  IO_Write(0x03, 0x3B);
	  break;
	  
	  case 8:
	  IO_Write(0x03, 0x3A);
	  break;
	  
	  case 9:
	  IO_Write(0x03, 0x39);
	  break;
	  
	  case 10:
	  IO_Write(0x03, 0x38);
	  break;
	  
	  case 11:
	  IO_Write(0x03, 0x3D);
	  break;
	  
	  case 12:
	  IO_Write(0x03, 0x3C);
	  break;
	  
  }
}

