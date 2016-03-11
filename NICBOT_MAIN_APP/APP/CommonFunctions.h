
#include "compiler.h"
#include <avr/io.h>

void IO_Write(unsigned char addr, unsigned char OutData);
void DrillSpeed(unsigned char ChannelNum, unsigned char Magnitude);
void HobbyDriveInit(void);

unsigned char IO_Read(unsigned char addr);
void canled(unsigned char ledstate);
void ledblink();

//  position = 0..35999
void Set_Sensor_Position(U16 position, U16 maxRate, U16 accelleration);

// gets position
U16 Get_Sensor_Position(void);

unsigned char accel_read(unsigned char accelcmd);  // Read value from accelerometer address
unsigned char accel_write(unsigned char acceladdr, unsigned char acceldata); // write to an accelerometer address
unsigned char accelerometer_init(void); // Initialize accelerometer & SPI bus.  This must be called before using accelerometer
 
int accelerometer_get_x(void); // Get x-axis acceleration.  This is a two's complement 16 bit value.
int accelerometer_get_y(void); // Get y-axis acceleration.
int accelerometer_get_z(void); //get z-axis acceleration.
