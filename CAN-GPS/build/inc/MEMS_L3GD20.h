/***************************************************************************//**
* @file		MEMS_L3GD20.h
* @brief	Functions for L3GD20 Sensor
* @version	1.0.0
* @date		18. July 2014
* @author	PEAK-SYSTEM TECHNIK
*
* Copyright (c): PEAK-SYSTEM TECHNIK GMBH, DARMSTADT
* *****************************************************************************/

#ifndef MEMS_L3GD20_H_
#define MEMS_L3GD20_H_

/*****************************************************************************
 include files
 ****************************************************************************/

/*****************************************************************************
 global data definitions
 ****************************************************************************/

#define MEMS_L3GD20_RANGE_250_DPS 	0x00
#define MEMS_L3GD20_RANGE_500_DPS 	0x01
#define MEMS_L3GD20_RANGE_2000_DPS 	0x20

typedef struct {
	u8_t GyroDevID;
	u8_t Temperature;
	float Gyro_X;
	float Gyro_Y;
	float Gyro_Z;
	u8_t data_valid;
} MEMS_L3GD20_Result_Type;

typedef struct {
	u8_t dummy;				//<! dummy for ssp reading
	//! Control Register 1 	(register 0x20)
	struct {
		u8_t Xen :1; 		//<! enable x axis.
		u8_t Yen :1; 		//<! enable y axis.
		u8_t Zen :1; 		//<! enable Z axis.
		u8_t pd :1;	 		//<! ~power down bit.
		u8_t bw :2;	 		//<! bandwidth selection bits.
		u8_t dr :2; 		//<! data output rate seletion bits.
	} Reg_0x20;
	//! Control Register 2 	(register 0x21)
	struct {
		u8_t hpcf :4; 		//<! High-pass filter mode selection.
		u8_t hpm :2; 		//<! High-pass filter cutoff frequency selection.
		u8_t reserved :2;	//<! must be set to "0" to ensure proper operation of the device.
	} Reg_0x21;
	//! Control Register 3 	(register 0x22)
	struct {
		u8_t i2_empty :1; 	//<! FIFO empty int on DRDY/INT2. (Default: 0 = disable; 1: enable)
		u8_t i2_orun :1; 	//<! FIFO overrun int on DRDY/INT2 (Default: 0 = disable; 1: enable)
		u8_t i2_wtm :1; 	//<! FIFO watermark int on DRDY/INT2. (Default: 0 = disable; 1: enable)
		u8_t i2_drdy :1; 	//<! Date-ready on DRDY/INT2. (Default: 0 = disable; 1: enable)
		u8_t pp_od :1; 		//<! Push-pull / Open drain. (Default: 0 = push-pull; 1: open drain)
		u8_t h_lactive :1;	//<! Interrupt active configuration on INT1. (Default: 0 = high; 1:low)
		u8_t i1_boot :1; 	//<! Boot status available on INT1. (Default: 0 = disable; 1: enable)
		u8_t i1_int1 :1; 	//<! Interrupt enable on INT1 pin. (Default: 0 = disable; 1: enable)
	} Reg_0x22;
	//! Control Register 4 	(register 0x23)
	struct {
		u8_t sim :1; 		//<! SPI interface mode selection. (Default: 0 = 4-wire interface; 1: 3-wire interface).
		u8_t reserved :3;	//<! "0" value must not be changed
		u8_t fs :2; 		//<! Full scale selection. (Default: 00 = 250 dps; 01: 500 dps; 10: 2000 dps; 11: 2000 dps)
		u8_t ble :1;		//<! Big/little endian data selection. (Default: 0 = Data LSb @ lower address; 1: Data MSb @ lower address)
		u8_t bdu :1;		//<! Block data update. (Default: 0 = continuos update; 1: output registers not updated until MSb and LSb reading)
	} Reg_0x23;
	//! Control Register 5 	(register 0x24)
	struct {
		u8_t out_sel :2;	//<! Out selection configuration. Default value: 0
		u8_t int_sel :2;	//<! INT1 selection configuration. Default value: 0
		u8_t hpen :1;		//<! High-pass filter enable. (Default: 0 = HPF disabled; 1: HPF enabled See Figure 20)
		u8_t reserved :1;	//<! Reserved
		u8_t fifo_en :1; 	//<! FIFO enable. (Default: 0 = FIFO disable; 1: FIFO Enable)
		u8_t boot :1;		//<! Reboot memory content. (Default: 0 = normal mode; 1: reboot memory content)
	} Reg_0x24;

} MEMS_L3GD20_Config_Register_Type; //<! Accelerometer Compensation Type

typedef struct {
	u8_t dummy;			//<! dummy for ssp reading/writing
	//! REF/DATACAPTURE (register 0x25)
	u8_t ref;	 		//<! Reference value for interrupt generation. Default: 0
	//! OUT_TEMP 		(register 0x26)
	u8_t temp;	 		//<! Temperature data
	//! STATUS	 		(register 0x27)
	struct {
		u8_t xda :1; 	//<! X axis new data available. (Default: 0 = new data for the X-axis not yet available; 1: new data for the X-axis available)
		u8_t yda :1; 	//<! Y axis new data available. (Default: 0 = new data for the Y-axis not yet available; 1: new data for the Y-axis available)
		u8_t zda :1; 	//<! Z axis new data available. (Default: 0 = new data for the Z-axis not yet available; 1: new data for the Z-axis available)
		u8_t zyxda :1; 	//<! X, Y, Z -axis new data available. (Default: 0 = a new set of data not yet available; 1: a new set of data available)
		u8_t xor :1; 	//<! X axis data overrun. (Default: 0 = no overrun occurred; 1: new data for the X-axis has overwritten previous data)
		u8_t yor :1; 	//<! Y axis data overrun. (Default: 0 = no overrun occurred; 1: new data for the Y-axis has overwritten previous data)
		u8_t zor :1; 	//<! Z axis data overrun. (Default: 0 = no overrun occurred; 1: new data for the Z-axis has overwritten previous data)
		u8_t zyxor :1; 	//<! X, Y, Z -axis data overrun. (Default: 0 = no overrun occurred; 1: new data has overwritten previous data)
	} Reg_0x27;
	//! OUT_X_L 		(register 0x28)
	u8_t x_l; 			//<! X-axis angular rate data low. The value is expressed as two�s complement.
	//! OUT_X_H 		(register 0x29)
	u8_t x_h; 			//<! X-axis angular rate data high. The value is expressed as two�s complement.
	//! OUT_Y_L 		(register 0x2A)
	u8_t y_l; 			//<! Y-axis angular rate data low. The value is expressed as two�s complement.
	//! OUT_Y_H 		(register 0x2B)
	u8_t y_h; 			//<! Y-axis angular rate data high. The value is expressed as two�s complement.
	//! OUT_Z_L 		(register 0x2C)
	u8_t z_l; 			//<! Z-axis angular rate data low. The value is expressed as two�s complement.
	//! OUT_Z_H 		(register 0x2D)
	u8_t z_h; 			//<! Z-axis angular rate data high. The value is expressed as two�s complement.

} MEMS_L3GD20_Data_Register_Type; //<! Accelerometer Compensation Type

extern MEMS_L3GD20_Result_Type L3GD20_Readings;

/*****************************************************************************
 global function prototypes
 ****************************************************************************/

HWStatus_t MEMS_L3GD20_init(void);
void MEMS_L3GD20_task(void);
HWStatus_t MEMS_L3GD20_SetRange(u8_t range);
HWStatus_t MEMS_L3GD20_GetRange(u8_t *p_val);

#endif /* MEMS_L3GD20_H_ */
