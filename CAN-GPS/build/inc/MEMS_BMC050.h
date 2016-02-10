/***************************************************************************//**
* @file		MEMS_BMC050.h
* @brief	Functions for BMC050 Sensor
* @version	1.0.0
* @date		18. July 2014
* @author	PEAK-SYSTEM TECHNIK
*
* Copyright (c): PEAK-SYSTEM TECHNIK GMBH, DARMSTADT
* *****************************************************************************/

#ifndef MEMS_BMC050_H_
#define MEMS_BMC050_H_

/*******************************************************************************
 include files
 ******************************************************************************/
#include "hardware.h"

/*******************************************************************************
 global data definitions
 ******************************************************************************/
#define X_AXIS 1
#define Y_AXIS 2
#define Z_AXIS 3

typedef enum BMC050_Orientations{
	flat = 0,
	flat_upside_down = 1,
	landscape_left = 2,
	landscape_right = 3,
	portrait = 4,
	portrait_upside_down = 5
}E_BMC050_Orientations;

typedef struct {
	u8_t AcclDevID;
	u8_t CompDevID;
	s16_t Acceleration_X;
	s16_t Acceleration_Y;
	s16_t Acceleration_Z;
	u8_t Temperature;
	s16_t MagField_X;
	s16_t MagField_Y;
	s16_t MagField_Z;
	u8_t orientation;
	u8_t data_valid;
} MEMS_BMC050_Result_Type;



typedef struct {
	u8_t cmd;						//<! command for ssp writing / dummy for reading
	//! chip id 					(register 0x00)
	u8_t chip_id;
	//! reserved 					(register 0x01)
	u8_t reserved_0x01;
	//! acceleration x axis lsb 	(register 0x02)
	struct {
		u8_t reserved :6; 				//<! reserved bits.
		u8_t acc_x_lsb :2; 				//<! acceleration x axis lsb.
	} Reg_0x02;
	//! acceleration x axis msb 	(register 0x03)
	u8_t acc_x_msb;					//<! acceleration x axis msb.
	//! acceleration y axis lsb 	(register 0x04)
	struct {
		u8_t reserved :6; 				//<! reserved bits.
		u8_t acc_y_lsb :2; 				//<! acceleration y axis lsb.
	} Reg_0x04;
	//! acceleration x axis msb 	(register 0x05)
	u8_t acc_y_msb;					//<! acceleration y axis msb.
	//! acceleration z axis lsb 	(register 0x06)
	struct {
		u8_t reserved :6; 				//<! reserved bits.
		u8_t acc_z_lsb :2; 				//<! acceleration z axis lsb.
	} Reg_0x06;
	//! acceleration z axis msb 	(register 0x07)
	u8_t acc_z_msb;					//<! acceleration z axis msb.
	//! temperature 				(register 0x08)
	u8_t temp;						//<! temperature.
} MEMS_BMC050_Acc_Data_Type; //<! Accelerometer Compensation Type

typedef struct {
	u8_t cmd;						//<! command for ssp writing / dummy for reading
	//! chip id 					(register 0x40)
	u8_t chip_id;
	//! reserved 					(register 0x41)
	u8_t reserved_0x41;
	//! magnetometer data x axis lsb 	(register 0x42)
	struct {
		u8_t x_slf_tst :1; 				//<! self test for x-axis.
		u8_t reserved :2; 				//<! reserved bits.
		u8_t mag_x_lsb :5; 				//<! magnetometer data x axis lsb.
	} Reg_0x42;
	u8_t mag_x_msb;				//<! acceleration x axis msb (register 0x43).
	//! magnetometer data y axis lsb 	(register 0x44)
	struct {
		u8_t y_slf_tst :1; 				//<! self test for y-axis.
		u8_t reserved :2; 				//<! reserved bits.
		u8_t mag_y_lsb :5; 				//<! magnetometer data y axis lsb.
	} Reg_0x44;
	u8_t mag_y_msb;			//<! magnetometer data y axis msb (register 0x45).
	//! magnetometer data z axis lsb 	(register 0x46)
	struct {
		u8_t z_slf_tst :1; 				//<! self test for z-axis.
		u8_t mag_z_lsb :7; 				//<! magnetometer data z axis lsb.
	} Reg_0x46;
	u8_t mag_z_msb;			//<! magnetometer data z axis msb (register 0x47).
	//! hall data lsb 				(register 0x48)
	struct {
		u8_t data_rdy :1; 				//<! data ready bit.
		u8_t reserved :1; 				//<!
		u8_t hall_lsb :6; 				//<! hall data lsb.
	} Reg_0x48;
	u8_t hall_msb;					//<! hall data msb (register 0x49).
} MEMS_BMC050_Mag_Data_Type; //<! Accelerometer Compensation Type

typedef struct {
	u8_t cmd;				//<! command for ssp writing / dummy for reading
	//! bandwidths configuration (register 0x10)
	struct {
		u8_t bandwidth :5;	//<! 00xxxb, 01000b = 7.81[Hz], 01001b = 15.63[Hz], 01010b = 31.25[Hz], 01011b = 62.5[Hz],
							//<! 01100b = 125[Hz], 01101b = 250[Hz], 01110b = 500[Hz], 01111b, 1xxxxb = 1000[Hz].
							//<! default 11111b
		u8_t reserved :3;	//<! reserved
	} Reg_0x10;
	//! power mode configuration (register 0x11)
	struct {
		u8_t reserved_01 :1;	//<! reserved
		u8_t sleep_dur :4;		//<! setting for sleep phase duration; 0000b-0101b = 0.5[ms], 0110b = 1[ms],
								//<! 0111b = 2[ms], 1000b = 4[ms], 1001b = 6 [ms], 1010b = 10 [ms], 1011b = 25[ms],
								//<! 1100b = 50 [ms], 1101b = 100[ms], 1110b = 500[ms], 1111b = 1[s]
								//<! default is 0000b
		u8_t reserved_02 :1;	//<! reserved
		u8_t lowpower_en :1;	//<! sets/resets low-power mode, default is 0
		u8_t suspend :1; 		//<! sets/resets suspend mode, default is 0
	} Reg_0x11;
} MEMS_BMC050_Acc_BwPwr_Type; //<! Accelerometer Compensation Type

typedef struct {
	u8_t cmd;			//<! command for ssp writing / dummy for reading
	//! compenstation configuration (register 0x36)
	struct {
		u8_t hp_x_en :1; //<! hp_x_en enables (disables) slow offset compensation for the x-axis.
		u8_t hp_y_en :1; //<! hp_y_en enables (disables) slow offset compensation for the y-axis.
		u8_t hp_z_en :1; //<! hp_z_en enables (disables) slow offset compensation for the z-axis.
		u8_t reseved_36_3 :1; 		//<!
		u8_t cal_rdy :1;//<! indicates the state of the fast compensation. 0 when cal_trigger has a nonzero value
		u8_t cal_trigger :2;//<! starts fast compensation for axis 00=none, 01=x, 10=y, 11=z
		u8_t offset_reset :1;		//<! resets reg 0x38 to 0x3D
	} Reg_0x36;
	//! compenstation configuration (register 0x37)
	struct {
		u8_t Cut_off :1;//<! Compensation Period for slow compensation. 0 = 8 Samples,, 1 = 16 Samples
		u8_t OffsetTarget_X :2;	//<! 00 = 0[g], 01 = +1[g], 10 = -1[g], 11 = 0[g]
		u8_t OffsetTarget_Y :2;
		u8_t OffsetTarget_Z :2;
		u8_t reseved :1; 			//<!
	} Reg_0x37;
	//! compensation values for filtered data for LSB = 7,8 [mg] MBS = 500 [mg] (register 0x38-0x3A)
	u8_t AccelerationCompFilt_X;		//<!
	u8_t AccelerationCompFilt_Y;
	u8_t AccelerationCompFilt_Z;
	//! compensation values for unfiltered data for LSB = 7,8 [mg] MBS = 500 [mg] (register 0x3B-0x3D)
	u8_t AccelerationComp_X;			//<!
	u8_t AccelerationComp_Y;
	u8_t AccelerationComp_Z;

	u8_t data_valid;
} MEMS_BMC050_Acc_Comp_Type; //<! Accelerometer Compensation Type

typedef struct MEMS_BMC050_Acc_IRQ{
	u8_t cmd;					//<! command for ssp writing / dummy for reading
	//! irq part 1 (register 0x09)
	struct {
		u8_t low_int :1; 		//<! Low-g interrupt status
		u8_t high_int :1; 		//<! High-g interrupt status
		u8_t slope_int :1; 		//<! Slope interrupt status
		u8_t reseved :1; 		//<!
		u8_t d_tap_int :1; 		//<! Double tap interrupt status
		u8_t s_tap_int :1; 		//<! Single tap interrupt status
		u8_t orient_int :1; 		//<! Orientation interrupt status
		u8_t flat_int :1; 		//<! Flat interrupt status
	} Reg_0x09;
	//! irq part 2 (register 0x0A)
	struct {
		u8_t reseved :7; 		//<!
		u8_t data_int :1; 		//<! New data interrupt status
	} Reg_0x0A;
	//! irq part 3 (register 0x0B)
	struct {
		u8_t slope_first_x :1;	//<! "1" indicates that x-axis has triggered slope interrupt
		u8_t slope_first_y :1; 	//<! "1" indicates that y-axis has triggered slope interrupt
		u8_t slope_first_z :1; 	//<! "1" indicates that z-axis has triggered slope interrupt
		u8_t slope_sign :1; 	//<! Sign of slope that triggered the interrupt ("0"=positive, "1"=negative)
		u8_t tap_first_x :1; 	//<! "1" indicates that x-axis has triggered tap interrupt
		u8_t tap_first_y :1; 	//<! "1" indicates that y-axis has triggered tap interrupt
		u8_t tap_first_z :1; 	//<! "1" indicates that z-axis has triggered tap interrupt
		u8_t tap_sign :1; 		//<! Sign of first tap that triggered the tap interrupt ("0"=positive,"1"=negative)
	} Reg_0x0B;
	//! irq part 4 (register 0x0C)
	struct {
		u8_t high_first_x :1; 	//<! "1" indicates that x-axis has triggered high-g interrupt
		u8_t high_first_y :1; 	//<! "1" indicates that y-axis has triggered high-g interrupt
		u8_t high_first_z :1; 	//<! "1" indicates that z-axis has triggered high-g interrupt
		u8_t high_sign :1; 		//<! "0"=positive, "1"=negative
		u8_t orient_xy :2;		//<! Orientation value of x-y plane ("00"=portrait  upright,
								//<! "01"=portrait upside-down, "10"=landscape left, "11"=landscape right)
		u8_t orient_z :1; 		//<! Orientation value of z-axis ("0" upward, "1" downward)
		u8_t flat :1; 			//<! Flat detection ("1" if flat condition fulfilled, "0" otherwise)
	} Reg_0x0C;
} MEMS_BMC050_Acc_IRQ_Type; 	//<! Accelerometer IRQ Type

typedef struct {
	u8_t cmd;				//<! command for ssp writing / dummy for reading
	//! interrupt enable part 1 (register 0x16)
	struct {
		u8_t slope_en_x :1; //<! En-/disables slope interrupt for x-axis
		u8_t slope_en_y :1; //<! En-/disables slope interrupt for y-axis
		u8_t slope_en_z :1; //<! En-/disables slope interrupt for z-axis
		u8_t reseved :1; 	//<! Reserved bits
		u8_t d_tap_en :1;	//<! En-/disables double tap interrupt
		u8_t s_tap_en :1; 	//<! En-/disables single tap interrupt
		u8_t orient_en :1; 	//<! En-/disables orientation interrupt
		u8_t flat_en :1; 	//<! En-/disables flat interrupt
	} Reg_0x16;
	//! interrupt enable part 2 (register 0x17)
	struct {
		u8_t high_en_x :1;	//<! En-/disables high-g interrupt for x-axis
		u8_t high_en_y :1; 	//<! En-/disables high-g interrupt for y-axis
		u8_t high_en_z :1; 	//<! En-/disables high-g interrupt for z-axis
		u8_t low_en :1; 	//<! En-/disables low-g interrupt
		u8_t data_en :1; 	//<! En-/disables new data interrupt
		u8_t reserved :3;	 //<! Reserved bits
	} Reg_0x17;
	//! reserved bits 		(register 0x18)
	u8_t reserved_0x18;
	//! interrupt mapping 	(register 0x19 -1B)
	//! only pin INT1 is connected to uC
	u8_t int_map_01;
	u8_t int_map_02;
	u8_t int_map_03;
	//! reserved bits 		(register 0x1C -0x1D)
	u8_t reserved_0x1C;
	u8_t reserved_0x1D;
	//! interrupt source definition	(register 0x1E)
	struct {
		u8_t int_src_low :1;	//<! Selects data source of low-g interrupt (1 raw, 0 filtered)
		u8_t int_src_high :1;	//<! Selects data source of high-g interrupt (1 raw, 0 filtered)
		u8_t int_src_slope :1;	//<! Selects data source of slope interrupt (1 raw, 0 filtered)
		u8_t reserved_01 :1; 	//<! Reserved bits
		u8_t int_src_tap :1; 	//<! Selects data source of tap interrupts (1 raw, 0 filtered)
		u8_t int_src_data :1;	//<! Selects data source of data interrupt (1 raw, 0 filtered)
		u8_t reserved_02 :2;	//<! Reserved bits
	} Reg_0x1E;
	//! reserved bits 		(register 0x1F)
	u8_t reserved_0x1F;
	//! interrupt pin behaviour	(register 0x20)
	struct {
		u8_t int1_lvl :1; 	//<! Active level for pin INT1 ("0" => pin is "0" when int active, "1" => pin is "1" when int active)
		u8_t int1_od :1; 	//<! "0" selects push-pull, "1" selects open drive pin INT1
		u8_t int2_lvl :1; 	//<! Active level for pin INT2 ("0" => pin is "0" when int active, "1" => pin is "1" when int active)
		u8_t int2_od :1; 	//<! "0" selects push-pull, "1" selects open drive pin INT2
		u8_t reserved :4; 	//<! Reserved bits
	} Reg_0x20;
	//! interrupt mode / reset	(register 0x20)
	struct {
		u8_t latch_int :4; 	//<! "0000b" == "1000b" non-latched, "0001b" temporary 250 [ms], "0010b" temporary 500[ms],
							//<! "0011b" temporary 1[s], "0100b" temporary 2[s], "0101b" temporary 4[s],
							//<! "0110b" temporary 8[s], "1001b" temporary 500 [us], "1010b" temporary 500 [us],
							//<! "1011b" temporary 1 [ms], "1100b" temporary 12.5 [ms], "1101b" temporary 25[ms],
							//<! "1110b" temporary 50[ms], "0111b" == "1111b" latched
		u8_t reserved :3; 	//<! Reserved bits
		u8_t reset_int :1; 	//<! Reset latched interrupts (
							//<! !!! Note: orientation, flat and new data interrupts
							//<! Are always automatically cleared after a fixed time) ???!!!
	} Reg_0x21;
	//! low-g interrupt delay time	(register 0x22)
	u8_t low_dur;	//<! delay [ms] = (low_dur + 1) � 2 [ms] (default = 0x09 = 20[ms]
	//! low-g interrupt threshold	(register 0x23)
	u8_t low_th;	//<! 7.81[mg]/digit => (default = 0x30 = 375[mg])
	//! interrupt hysteresis	(register 0x24)
	struct {
		u8_t low_hy :2; 	//<! Low-g interrupt hysteresis 125[mg]/digit (default = 0x01)
		u8_t low_mode :1; 	//<! "0" = single mode, "1" = sum mode (default = 0)
		u8_t reserved :3; 	//<! Reserved bits
		u8_t high_hy :2; 	//<! High-g interrupt hysteresis. Value depends on g-range
							//<! 125[mg] / 250[mg] / 500[mg] / 1000[mg] per digit in
							//<!   2[g]  /   4[g]  /   8[g]  /   16[g]  range (default 0x02)
	} Reg_0x24;
	//! high-g interrupt delay time	(register 0x25)
	u8_t high_dur;			//<! delay [ms] = (high_dur + 1) � 2 [ms] (default = 0x25 = 32[ms]
	//! high-g interrupt threshold	(register 0x26)
	u8_t high_th;			//<! Value depends on g-range
							//<! 7.81[mg] / 15.63[mg] / 31.25[mg] / 62.5[mg]	per digit in
							//<! 2[g]     /  4[g]     /  8[g]     / 16[g]  		range (default 0x02)
	//! slope duration 		(register 0x27)
	struct {
		u8_t slope_dur :2; 	//<! number of samples taken for slope interrupt (slope_dur+1; default 0x00)
		u8_t reserved :6; 	//<!
	}Reg_0x27;
	//! slope threshold		(register 0x28)
	u8_t slope_th; 			//<! Value/digit corresponds to acc data registers and depends on the g-range
	//! reserved bits 		(register 0x29)
	u8_t reserved_0x29;
	//! tap interrupt timing(register 0x2A)
	struct {
		u8_t tap_dur :3; 	//<! Selects time window for second tap of double tap detection
							//<! "000b" = 50 [ms], "001b" = 100[ms], "010b" = 150[ms], "011b" 200 = [ms]
							//<! "100b" = 250 [ms], "101b" = 375[ms], "110b" = 500 [ms], "111b" = 700 [ms]
		u8_t reserved :3; 	//<!
		u8_t tap_shock :1; 	//<! Selects quiet duration (0 = 30[ms], 1 = 20[ms])
		u8_t tap_quiet :1; 	//<! Selects shock duration (0 = 50[ms], 1 = 75[ms])
	}Reg_0x2A;
	//! tap threshold and samples (register 0x2B)
	struct {
		u8_t tap_th :5; 	//<! Tap theshold. Value depends on g-range
							//<! 62.5[mg] / 125[mg] / 250[mg] / 500[mg]	per digit in
							//<!  2[g]    /   4[g]  /   8[g]  /  16[g]  range (default 0x0A)
		u8_t reserved :1; 	//<!
		u8_t tap_samp :2; 	//<! Number of samples to be processed after wake-up in low-power mode
							//<! "00b" 2, "01b" 4, "10b" 8, "11b" 16 samples (default 0x00)
	}Reg_0x2B;
	//! orientation interrupt settings (register 0x2C)
	struct {
		u8_t orient_mode :2; 		//<! Sets threshold for switching between different orientations
									//<! "00b" symmetrical, "01b" high-asymmetrical,
									//<! "10b" low-asymmetrical, "11b" symmetrical (default "00b")
		u8_t orient_blocking :2; 	//<! Blocking mode. "00b" no blocking, "01b" theta blocking,
									//<! "10b" theta blocking or slope in any axis>0.2[g]
									//<! "11b" orient value not stable for at least 100[ms]
									//<!       or theta blocking or slope in any axis>0.4[g] (default "10b")
		u8_t orient_hyst :3;	 	//<! Hysteresis of orientation interrupt. Always 62.5[mg]/digit
		u8_t reserved :1; 			//<!
	}Reg_0x2C;
	//! orientation interrupt blocking angle (register 0x2D)
	struct {
		u8_t orientation_theta :6; 	//<! blocking angel. 0-44.8 degree (default 0x08)
		u8_t reserved :2; 			//<!
	}Reg_0x2D;
	//! flat interrupt blocking angle (register 0x2E)
	struct {
		u8_t flat_theta :6; //<! blocking angel. 0-44.8 degree (default 0x08)
		u8_t reserved :2; 	//<!
	}Reg_0x2E;
	//! interrupt (register 0x2F)
	struct {
		u8_t reserved_01 :4; 	//<!
		u8_t flat_hold_time :2;	//<! Sets time flat value has to be stable before interrupt is generated
								//<! "00b" 0[ms], "01b" 512[ms], "10b" 1024 [ms], "11b" 2048 [ms]
		u8_t reserved_02 :2; 	//<!
	}Reg_0x2F;
} MEMS_BMC050_Acc_IRQCfg_Type; 	//<! Accelerometer IRQ Type

extern MEMS_BMC050_Result_Type BMC050_Readings;
extern MEMS_BMC050_Acc_Comp_Type BMC050_AccCompensate;

/*****************************************************************************
 global function prototypes
 ****************************************************************************/

void Init_SSP1(void);
HWStatus_t MEMS_BMC050_init_Accelerometer(void);
HWStatus_t MEMS_BMC050_init_Magnetometer(void);
void MEMS_BMC050_init_Data(void);
void MEMS_BMC050_readAccCompValues(void);
void MEMS_BMC050_StartFastAccCompensation(void);
void MEMS_BMC050_ResetAccCompensation(void);
void MEMS_BMC050_StartMagCompensation(void);
void MEMS_BMC050_StopMagCompensation(void);
void MEMS_BMC050_ResetMagCompensation(void);
void MEMS_BMC050_ResetMagFactors(void);
void MEMS_BMC050_SetMagFactors(float x, float y, float z);
HWStatus_t MEMS_BMC050_GetMagFactors(float *p_x, float *p_y, float *p_z);
void MEMS_BMC050_debugAccRegisters(void);
void MEMS_BMC050_debugMagRegisters(void);
HWStatus_t MEMS_BMC050_GetAccRange(u8_t *p_val);
HWStatus_t MEMS_BMC050_SetAccRange(u8_t val);

void MEMS_BMC050_SetDebugDataMode(u8_t val);

HWStatus_t MEMS_BMC050_SetAccCalTargets(u8_t *p_x, u8_t *p_y, u8_t *p_z);
HWStatus_t MEMS_BMC050_GetAccCalTargets(u8_t *p_x, u8_t *p_y, u8_t *p_z);
HWStatus_t MEMS_BMC050_GetMagOffsets(s16_t *p_x, s16_t *p_y, s16_t *p_z);
HWStatus_t MEMS_BMC050_SetMagOffsets(s16_t x, s16_t y, s16_t z);

HWStatus_t MEMS_BMC050_SetVertialAxis(u8_t axis);
HWStatus_t MEMS_BMC050_GetVertialAxis(u8_t *p_axis);

HWStatus_t MEMS_BMC050_GetAccCalFiltValues(u8_t *p_x, u8_t *p_y, u8_t *p_z);
HWStatus_t MEMS_BMC050_GetAccCalRawValues(u8_t *p_x, u8_t *p_y, u8_t *p_z);

void MEMS_BMC050_task(void);

#endif /* MEMS_BMC050_H_ */
