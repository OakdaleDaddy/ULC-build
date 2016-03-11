/*
 * ServoControl.h
 *
 * Created: 9/30/2015 6:56:24 PM
 *  Author: ULC Robotics
 */ 


#ifndef SERVOCONTROL_H_
#define SERVOCONTROL_H_

unsigned char* servo_get_position(unsigned char axisnum);

//  Axis selection:  axisnum is 0 for sensor spinner for inspection robot
//  for repair robot 0 for drill 1 or 1 for drill 2.


//************************************************************
//             Status bits for servo_get_status
//   Bit 0: Busy when set, cannot accept commands
//   Bit 1: Command error
//   Bit 2: Trajectory complete. Check this to determine if motion is complete
//   Bit 3: Index pulse found. (this only applies to motors with an index pulse and is optional)
//   Bit 4: Wraparound occurred
//   Bit 5: Excessive position error
//   Bit 6: Breakpoint reached
//   Bit 7: Motor off
//**************************************************************

unsigned char servo_get_status(unsigned char axisnum); 
void DrillInit(void); // Call this before drill speed may be set.
void drillspeed(unsigned char axisnum, unsigned char drillspeedh, unsigned char drillspeedl);  // drillspeedh must be set to 0x30, drillspeedl is a value between 0x40 & 0x7f.
// the speed is proportional to the value set with minimum speed being 0x40 and 0x7f being the full speed.
void timedelay(void);
void SetPID(unsigned char axisnum, unsigned int kp, unsigned int ki, unsigned int kd, unsigned int il);  //  Setup PID Parameters
void servo_hard_reset(void);  // Perform hardware reset of servo controllers
unsigned char servo_reset(unsigned char axisnum);  // perform soft reset of servo controller
unsigned char servo_init(unsigned char axisnum);  //  Initialization of servo controller. Must be called before servo control may be used
unsigned char servo_move_abs(unsigned char axisnum);  // absolute move to position set in servo_load_position
unsigned char servo_move_abs_while_moving(unsigned char axisnum); // absolute move to position set in servo_load_position while moving
unsigned char servo_set_velocity_while_moving(unsigned char axisnum); // absolute move to position set in servo_load_position while moving
unsigned char servo_move_rel(unsigned char axisnum);  // relative move in increment set in servo_load position set position at set accel and velocity
unsigned char servo_set_origin(unsigned char axisnum);  // Define origin. The current position becomes the origin
unsigned char servo_stop_decel(unsigned char axisnum);  // stop with deceleration. Deceleration rate is accel set for current trajectory
unsigned char servo_stop_abrupt(unsigned char axisnum); // Stop with maximum deceleration
unsigned char servo_motor_off(unsigned char axisnum);


//**************************************************************
// Range for distance: C0000000 to 3fffffff encoder pulses
// Range for velocity: C0000000 to 3fffffff
// Range for accel: 00000001 to 3fffffff
// Value loaded for accel must not exceed value loaded for velocity
//
// Conversion factor to convert desired RPM to value loaded for velocity:
// Velocity value = (quadrature encoder resolution) * 341E-6 uS * (1 min/60 sec) * desired RPM
// Encoder resolution for sensor spinner is 1024 or 4096 in quadrature
// Acceleration rate:
// Value loaded for accel = (quadrature encoder resolution)*(341E-6 uS)^2 *(desired accel rate in rev/sec^2)
unsigned char servo_load_accel(unsigned char r0, unsigned char r1, unsigned char r2, unsigned char r3);    // Load acceleration rate
unsigned char servo_load_velocity(unsigned char r0, unsigned char r1, unsigned char r2, unsigned char r3); // Load velocity
unsigned char servo_load_position(unsigned char r0, unsigned char r1, unsigned char r2, unsigned char r3); // Load position for absolute mode or increment for relative mode

// Acceleration. velocity, and position must be loaded prior to starting a trajectory
// The trajectory complete bit in the status register must be monitored to determine when motion is complete. 
// 

// set an error limit for the maximum allowable error between desired position, as determined by the trajectory generator, and actual position.
// and stop if the error limit is exceeded. axisnum is the axis number, 0 for sensor spinner and pos_error is allowable position error, a value between 1 & 32768 encoder
// pulses.  
unsigned char servo_set_error(unsigned char axisnum, unsigned int pos_error);

unsigned char set_kp(unsigned char axisnum, unsigned char r0, unsigned char r1);
unsigned char set_ki(unsigned char axisnum, unsigned char r0, unsigned char r1);
unsigned char set_kd(unsigned char axisnum, unsigned char r0, unsigned char r1);

#endif /* SERVOCONTROL_H_ */