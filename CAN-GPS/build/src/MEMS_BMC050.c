/*******************************************************************************
 *
 * Project  :	PCAN-GPS
 * Module   :
 * Filename :	MEMS_BMC050.c
 * System   :
 * Compiler :
 * Switches :
 * Rights   : 	(c) PEAK-System Technik GmbH
 *            	www.peak-system.com
 *
 *******************************************************************************
 * Implementation description
 *
 *
 *
 *******************************************************************************
 * History: (newer entries first!)
 *------------------------------------------------------------------------------
 * Date / Name      Vers.   changes made
 *------------------------------------------------------------------------------
 * 2015 Mar  06/StM	0.0.3	bug fixed in acceleration calculation
 * 2014 June 04/StM	0.0.2	clean-up
 * 2013 Sep xx/StS	0.0.1	Initial Version
 ******************************************************************************/

/* @ ToDo: 	- remove debug functions	*/

/*******************************************************************************
 include files
 ******************************************************************************/
//
// System header files
//
#include <math.h>
#include <lpc407x_8x_177x_8x.h>
#include <system_LPC407x_8x_177x_8x.h>

//
// Library header files
//
#include "typedefs.h"
#include "hardware.h"
#include "timer.h"
#include "ssp.h"

//
// Source code header files
//
#include "MEMS_BMC050.h"

/*******************************************************************************
 global definitions
 ******************************************************************************/
#ifdef DEBUG
#undef DEBUG
#endif
#define DEBUG 1

#ifdef DEBUG
static u8_t use_dbg_data=0;
MEMS_BMC050_Mag_Data_Type BMC050_dbg_data_cmp;
MEMS_BMC050_Acc_Data_Type BMC050_dbg_data_acc;
#endif

// holds results of the BMC050
MEMS_BMC050_Result_Type BMC050_Readings;
MEMS_BMC050_Acc_Comp_Type BMC050_AccCompensate;

extern S_CONFIG_DATA_t cfg_data;

/*******************************************************************************
 local definitions
 ******************************************************************************/
#define bufsize 17


typedef enum _BMC050States {
	readAccelerometer,
	readMagnetometer,
	processResults,
	slowCompensation,
	startFastCompensation,
	fastCompensation_x,
	fastCompensation_y,
	fastCompensation_z,
	readAccelerometerCompenstaionValues,
	manualAccCompensation,
	resetAccCompensation,
} MEMS_BMC050_States_Type;

// holds current state of state machine
static MEMS_BMC050_States_Type BMC050State = readAccelerometer;
// holds state to set state machine to after next "processResults" cycle is done
static MEMS_BMC050_States_Type GoToState = readAccelerometer;


static MEMS_BMC050_Acc_IRQCfg_Type  AccIRQConfig;
static MEMS_BMC050_Acc_BwPwr_Type  AccBWPwrConfig;

static u8_t vertical_axis = Z_AXIS;

static MEMS_BMC050_Acc_Data_Type AccDataRaw;
static MEMS_BMC050_Mag_Data_Type CmpDataRaw;

static u8_t ssp_init=0;			//<! ssp initialized yet?
static u8_t data_init=0;		//<! data structure initialized yet?

//! multiplier for acceleration values to have always the same scale in data
//! structure independent from sensors measurement range
static u8_t AccSensitivity = 4;


/*******************************************************************************
 local function prototypes
 ******************************************************************************/
int AccCalRdy(void);

static HWStatus_t initSSP(void);
static void initData(void);
static int prcRawAccValues(MEMS_BMC050_Acc_Data_Type *pData,
		MEMS_BMC050_Result_Type *pRes);
static int prcRawCmpValues(MEMS_BMC050_Mag_Data_Type *pData,
		MEMS_BMC050_Result_Type *pRes);
static int readCurrAccelerationValues(MEMS_BMC050_Acc_Data_Type *pBuff);
static int readCurrMagValues(MEMS_BMC050_Mag_Data_Type *pBuff);
static int readAccRange(void);
static void setAccRange(void);
static int readAccIRQConfig(void);
static int setAccCompensationTargets(void);
static int readAccCompenstaionValues(void);
static int accFastCompensationX(void);
static int accFastCompensationY(void);
static int accFastCompensationZ(void);
static int resetAccCompensationValues(void);
static u8_t checkAccIRQ(void);
static u8_t checkMagIRQ(void);
static void handleAccIRQ(void);
static void handleMagIRQ(void);
static u8_t rwACC(u8_t *p_wBuff, u8_t *p_rBuff, u8_t len);
static u8_t rwMAG(u8_t *p_wBuff, u8_t *p_rBuff, u8_t len);
static u8_t wAccReg(u8_t reg, u8_t val);
static u8_t wMagReg(u8_t reg, u8_t val);
static u8_t rMagReg(u8_t reg, u8_t *p_val);

/*******************************************************************************
 global functions
 ******************************************************************************/



//------------------------------------------------------------------------------
//! HWStatus_t MEMS_BMC050_init_Accelerometer(void)
//------------------------------------------------------------------------------
//! @brief	initializes accelerometer part of BMC50/150 sensor with default
//!			settings. If not done yet SSP is also initialized
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
HWStatus_t MEMS_BMC050_init_Accelerometer(void){
	u8_t wBuff[bufsize];
	u8_t rBuff[bufsize];

	if(!ssp_init){
		if(initSSP() != HW_ERR_OK)
			return HW_ERR_RESOURCE;
	}

	if(!data_init)
		initData();

	// this initializes the accelerometer
	// write register 34h: spi 4-wire
	wAccReg(0x34, 0x00);

	// write register 0Fh: G-range
	setAccRange();
	
	AccBWPwrConfig.cmd=0x10;
	AccBWPwrConfig.Reg_0x10.bandwidth = 0x4;
	AccBWPwrConfig.Reg_0x11.lowpower_en=0;
	AccBWPwrConfig.Reg_0x11.sleep_dur=0;
	AccBWPwrConfig.Reg_0x11.suspend=0;
	rwACC((u8_t*)&AccBWPwrConfig, rBuff, 3);

	// write register 13h: Filtered Data & Shadow on
	wAccReg(0x13, 0x00);
	
	readAccIRQConfig();

	// disable desired interrupt, change parameters, wait for about 1[ms],
	// enable the desired interrupt
	AccIRQConfig.Reg_0x2C.orient_blocking = 3;
	AccIRQConfig.Reg_0x2C.orient_hyst = 3;
	wAccReg(0x2C, *((u8_t*)&AccIRQConfig.Reg_0x2C));
	
	AccIRQConfig.Reg_0x2F.flat_hold_time = 0;
	wAccReg(0x2F, *((u8_t*)&AccIRQConfig.Reg_0x2F));
	
	AccIRQConfig.int_map_01 = 0xC0;
	wAccReg(0x19, AccIRQConfig.int_map_01);
	AccIRQConfig.int_map_02 = 1;
	wAccReg(0x1A, AccIRQConfig.int_map_02);

	if (cfg_data.Acc.flags & ACC_USE_EEPROM_FILT_COMPENSATION_VALUES){
		BMC050_AccCompensate.AccelerationCompFilt_X = cfg_data.Acc.cmp_filt_x;
		BMC050_AccCompensate.AccelerationCompFilt_Y = cfg_data.Acc.cmp_filt_y;
		BMC050_AccCompensate.AccelerationCompFilt_Z = cfg_data.Acc.cmp_filt_z;

		wAccReg(0x38, BMC050_AccCompensate.AccelerationCompFilt_X);
		wAccReg(0x39, BMC050_AccCompensate.AccelerationCompFilt_Y);
		wAccReg(0x3A, BMC050_AccCompensate.AccelerationCompFilt_Z);
	}
	if (cfg_data.Acc.flags & ACC_USE_EEPROM_RAW_COMPENSATION_VALUES){
		BMC050_AccCompensate.AccelerationComp_X = cfg_data.Acc.cmp_raw_x;
		BMC050_AccCompensate.AccelerationComp_Y = cfg_data.Acc.cmp_raw_y;
		BMC050_AccCompensate.AccelerationComp_Z = cfg_data.Acc.cmp_raw_z;

		wAccReg(0x3B, BMC050_AccCompensate.AccelerationComp_X);
		wAccReg(0x3C, BMC050_AccCompensate.AccelerationComp_Y);
		wAccReg(0x3D, BMC050_AccCompensate.AccelerationComp_Z);
	}

	AccIRQConfig.Reg_0x20.int1_lvl = 1;
	AccIRQConfig.Reg_0x20.int2_lvl = 1;
	AccIRQConfig.Reg_0x21.latch_int = 0xC;
	AccIRQConfig.Reg_0x21.reset_int = 1;
	wBuff[0] = 0x20;  // command: write register 20h
	wBuff[1] = *((u8_t*)&AccIRQConfig.Reg_0x20);
	wBuff[2] = *((u8_t*)&AccIRQConfig.Reg_0x21);
	rwACC(wBuff, rBuff, 3);

	/* @ ToDo check if necessary */
	Wait_Usec(2000);

	AccIRQConfig.Reg_0x16.orient_en = 1;
	AccIRQConfig.Reg_0x16.flat_en = 1;
	wAccReg(0x16, *((u8_t*)&AccIRQConfig.Reg_0x16));

	return HW_ERR_OK;
}



//------------------------------------------------------------------------------
//! HWStatus_t MEMS_BMC050_init_Magnetometer(void)
//------------------------------------------------------------------------------
//! @brief	initializes magnetometer part of BMC50/150 sensor with default
//!			settings. If not done yet SSP is also initialized
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
HWStatus_t MEMS_BMC050_init_Magnetometer(void){
	u32_t time = 0;

	if(!ssp_init){
		if(initSSP() != HW_ERR_OK)
			return HW_ERR_RESOURCE;
	}

	if(!data_init)
		initData();

	// wait at least 1 [ms] cause from to allow magnetometer to switch from OFF to suspend mode
	time = SYSTIME_NOW;
	while(SYSTIME_DIFF (time, SYSTIME_NOW) < 2000);

	// initialize the magnetometer
	wMagReg(0x4B, 0x83); 	//<! 4Bh: spi 4-wire + PowerControlBit, (SuspendMode=>SleepMode) + SoftResetBits

	// wait at least 3[ms] to allow magnetometer to switch from suspend to sleep mode
	time = SYSTIME_NOW;
	while(SYSTIME_DIFF (time, SYSTIME_NOW) < 3000);
	
	wMagReg(0x4C, 0x00); 	//<! 4Ch: OpMode = Normal (SleepMode=>ActiveMode)
	wMagReg(0x4D, 0x3F); 	//<! 4Dh: generate no Interrupts
	wMagReg(0x4E, 0x00); 	//<! 4Eh: each axis enable, line polarity dont care.
	wMagReg(0x51, 0x07); 	//<! 51h: repetition count for X-Y value smoothing.
	wMagReg(0x52, 0x1B); 	//<! 52h: repetition count for Z value smoothing.
	
	return HW_ERR_OK;
}



void MEMS_BMC050_readAccCompValues(void) {
	GoToState = readAccelerometerCompenstaionValues;
}

void MEMS_BMC050_StartFastAccCompensation(void) {
	GoToState = startFastCompensation;
}

void MEMS_BMC050_ResetAccCompensation(void) {
	GoToState = resetAccCompensation;
}

HWStatus_t MEMS_BMC050_SetVertialAxis(u8_t axis) {
	if (axis != X_AXIS && axis != Y_AXIS && axis != Z_AXIS)
		return HW_ERR_ILLPARAMVAL;
	vertical_axis = axis;
	return HW_ERR_OK;
}

HWStatus_t MEMS_BMC050_GetVertialAxis(u8_t *p_axis) {
	if (!p_axis)
		return HW_ERR_ILLPARAMVAL;
	*p_axis = vertical_axis;
	return HW_ERR_OK;
}

//------------------------------------------------------------------------------
//!	int AccCalRdy(void)
//------------------------------------------------------------------------------
//! @brief  checks cal_rdy bit in register 0x37
//------------------------------------------------------------------------------
int AccCalRdy(void) {
	u8_t wBuff[bufsize];

	wBuff[0] = ((1 << 7) | 0x36);  // command: read from register 36h onward
	wBuff[1] = 0x00; // + 8 clocks for compensation configuration part I
	rwACC(wBuff, (u8_t*)&BMC050_AccCompensate, 2);

	return !(BMC050_AccCompensate.Reg_0x36.cal_rdy);
}



//------------------------------------------------------------------------------
//! HWStatus_t MEMS_BMC050_SetAccCalTargets(u8_t *p_x, u8_t *p_y, u8_t *p_z)
//------------------------------------------------------------------------------
//! @brief	sets calibration targets in structure (writing to BMC050 is done
//			when starting the fast compensation!)
//			00b = 0[g], 01b = +1[g], 10b = -1[g], 11b = 0[g]
//------------------------------------------------------------------------------
//! @param p_x	pointer to x-axis calibration target
//! @param p_y	pointer to y-axis calibration target
//! @param p_z	pointer to z-axis calibration target
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
HWStatus_t MEMS_BMC050_SetAccCalTargets(u8_t *p_x, u8_t *p_y, u8_t *p_z) {

	if (p_x && *p_x > 0x3)
		return HW_ERR_ILLPARAMVAL;
	if (p_y && *p_y > 0x3)
		return HW_ERR_ILLPARAMVAL;
	if (p_z && *p_z > 0x3)
		return HW_ERR_ILLPARAMVAL;

	if (p_x)
		BMC050_AccCompensate.Reg_0x37.OffsetTarget_X = *p_x;
	if (p_y)
		BMC050_AccCompensate.Reg_0x37.OffsetTarget_Y = *p_y;
	if (p_z)
		BMC050_AccCompensate.Reg_0x37.OffsetTarget_Z = *p_z;

	return HW_ERR_OK;
}

//------------------------------------------------------------------------------
//! HWStatus_t MEMS_BMC050_GetAccCalTargets(u8_t *p_x, u8_t *p_y, u8_t *p_z)
//------------------------------------------------------------------------------
//! @brief	get calibration targets
//------------------------------------------------------------------------------
//! @param p_x	pointer to buffer for x-axis calibration target
//! @param p_y	pointer to buffer for y-axis calibration target
//! @param p_z	pointer to buffer for z-axis calibration target
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
HWStatus_t MEMS_BMC050_GetAccCalTargets(u8_t *p_x, u8_t *p_y, u8_t *p_z) {

	if (p_x)
		*p_x = BMC050_AccCompensate.Reg_0x37.OffsetTarget_X;
	if (p_y)
		*p_y = BMC050_AccCompensate.Reg_0x37.OffsetTarget_Y;
	if (p_z)
		*p_z = BMC050_AccCompensate.Reg_0x37.OffsetTarget_Z;

	return HW_ERR_OK;
}

//------------------------------------------------------------------------------
//! HWStatus_t MEMS_BMC050_GetAccCalRawValues(u8_t *p_x, u8_t *p_y, u8_t *p_z)
//------------------------------------------------------------------------------
//! @brief	get calibration targets
//------------------------------------------------------------------------------
//! @param p_x	pointer to buffer for x-axis calibration value
//! @param p_y	pointer to buffer for y-axis calibration value
//! @param p_z	pointer to buffer for z-axis calibration value
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
HWStatus_t MEMS_BMC050_GetAccCalRawValues(u8_t *p_x, u8_t *p_y, u8_t *p_z) {

	readAccCompenstaionValues();

	if (p_x)
		*p_x = BMC050_AccCompensate.AccelerationComp_X;
	if (p_y)
		*p_y = BMC050_AccCompensate.AccelerationComp_Y;
	if (p_z)
		*p_z = BMC050_AccCompensate.AccelerationComp_Z;

	return HW_ERR_OK;
}

//------------------------------------------------------------------------------
//! HWStatus_t MEMS_BMC050_GetAccCalFiltValues(u8_t *p_x, u8_t *p_y, u8_t *p_z)
//------------------------------------------------------------------------------
//! @brief	get calibration targets
//------------------------------------------------------------------------------
//! @param p_x	pointer to buffer for x-axis calibration value
//! @param p_y	pointer to buffer for y-axis calibration value
//! @param p_z	pointer to buffer for z-axis calibration value
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
HWStatus_t MEMS_BMC050_GetAccCalFiltValues(u8_t *p_x, u8_t *p_y, u8_t *p_z) {

	readAccCompenstaionValues();

	if (p_x)
		*p_x = BMC050_AccCompensate.AccelerationCompFilt_X;
	if (p_y)
		*p_y = BMC050_AccCompensate.AccelerationCompFilt_Y;
	if (p_z)
		*p_z = BMC050_AccCompensate.AccelerationCompFilt_Z;

	return HW_ERR_OK;
}

//------------------------------------------------------------------------------
//! HWStatus_t MEMS_BMC050_GetAccRange(u8_t *p_val)
//------------------------------------------------------------------------------
//! @brief  get range of acceleration measurement
//------------------------------------------------------------------------------
HWStatus_t MEMS_BMC050_GetAccRange(u8_t *p_val) {
	if(!p_val)
		return HW_ERR_ILLPARAMVAL;
	
	if(ssp_init) 	// only try to read value if spi is already running
		readAccRange();
	
	switch(AccSensitivity){
	default:
	case 1:
		*p_val= 1;	
		break;
	case 2:
		*p_val= 2;	
		break;
	case 4:
		*p_val= 3;	
		break;
	case 8:
		*p_val= 4;
		break;	
	}
	
	
	return HW_ERR_OK;
}


//------------------------------------------------------------------------------
//! HWStatus_t MEMS_BMC050_SetAccRange(u8_t val)
//------------------------------------------------------------------------------
//! @brief  set range of acceleration measurement
//------------------------------------------------------------------------------
HWStatus_t MEMS_BMC050_SetAccRange(u8_t val) {
	
	if(val > 4)
		return HW_ERR_ILLPARAMVAL;
	switch(val){
	default:
	case 1:
		AccSensitivity = 1;
		break;
	case 2:
		AccSensitivity = 2;
		break;
	case 3:
		AccSensitivity = 4;
		break;
	case 4:
		AccSensitivity = 8;
		break;
	}
	
	if(ssp_init) 	// only try to write value if spi is already running
		setAccRange();
	
	return HW_ERR_OK;
}


//------------------------------------------------------------------------------
//! void MEMS_BMC050_task(void)
//------------------------------------------------------------------------------
void MEMS_BMC050_task(void)
{
	int res = 0;

	if (checkAccIRQ())
		handleAccIRQ();
	if (checkMagIRQ())
		handleMagIRQ();

	if (BMC050_Readings.data_valid == TRUE)
		return;

	switch (BMC050State) {
	case readAccelerometer:
		res = readCurrAccelerationValues(&AccDataRaw);
		if (res == 0)
			BMC050State = readMagnetometer;
		break;

	case readMagnetometer:
		res = readCurrMagValues(&CmpDataRaw);
		if (res == 0)
			BMC050State = processResults;
		break;

	case processResults:
		#ifdef DEBUG
		if (use_dbg_data){
			prcRawAccValues(&BMC050_dbg_data_acc, &BMC050_Readings);
			prcRawCmpValues(&BMC050_dbg_data_cmp, &BMC050_Readings);
		}else{
		#endif
		prcRawAccValues(&AccDataRaw, &BMC050_Readings);
		prcRawCmpValues(&CmpDataRaw, &BMC050_Readings);
		#ifdef DEBUG
		}
		#endif
		BMC050_Readings.data_valid = TRUE; // data now ready for CAN transmission
		BMC050State = GoToState;
		break;

	case slowCompensation:
		break;

	case startFastCompensation:
		BMC050_AccCompensate.data_valid = FALSE;
		if (setAccCompensationTargets() >= 0)
			BMC050State = fastCompensation_x;
		break;

	case fastCompensation_x:
		BMC050_AccCompensate.data_valid = FALSE;
		if (accFastCompensationX() >= 0)
			BMC050State = fastCompensation_y;
		break;

	case fastCompensation_y:
		BMC050_AccCompensate.data_valid = FALSE;
		if (accFastCompensationY() >= 0)
			BMC050State = fastCompensation_z;
		break;

	case fastCompensation_z:
		BMC050_AccCompensate.data_valid = FALSE;
		if (accFastCompensationZ() >= 0)
			BMC050State = readAccelerometerCompenstaionValues;
		break;

	case readAccelerometerCompenstaionValues:
		if (readAccCompenstaionValues() >= 0) {
			BMC050_AccCompensate.data_valid = TRUE;
			BMC050State = readAccelerometer;
			GoToState = readAccelerometer;
		}
		break;

	case manualAccCompensation:
		break;

	case resetAccCompensation:
		BMC050_AccCompensate.data_valid = FALSE;
		if (resetAccCompensationValues() >= 0)
			BMC050State = readAccelerometerCompenstaionValues;
		break;

	// error case:
	default:
		break;
		//-------------------------
	}

} // MEMS_BMC050_task
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
//! void initData(void)
//------------------------------------------------------------------------------
//! @ brief initializes data structures
//------------------------------------------------------------------------------
void initData(void)
{
	// no valid readings so far
	BMC050_Readings.data_valid = FALSE;

	// init local state machine
	BMC050State = readAccelerometer;

	data_init = 1;
	
	return;
}

//------------------------------------------------------------------------------
//! static HWStatus_t initSSP(void)
//------------------------------------------------------------------------------
//@ brief  initializes SSP interface for communication with sensor
//------------------------------------------------------------------------------
static HWStatus_t initSSP(void)
{
	SSPInit_t init;
	SSPStatus_t res=0;

	init.bitlen = t8_bit;
	init.clk_phase = 1;
	init.clk_pol = 1;
	init.mode = 0;
	init.frm_format = 0;
	init.loop_back=0;

//	init.ser_clk_rate = 9; 		//! 10 clocks per bit
	init.ser_clk_rate = 0; 		//! 1 clocks per bit
	init.clk_prescaler = 12;

	init.enable=1;

	res = SSP_Init(SSP_2, &init);
	if(res != SSP_ERR_OK)
		return HW_ERR_RESOURCE;

	ssp_init=1;

	return HW_ERR_OK;
}
//------------------------------------------------------------------------------

static int prcRawAccValues(MEMS_BMC050_Acc_Data_Type *pData,
		MEMS_BMC050_Result_Type *pRes) {

	pRes->AcclDevID = pData->chip_id;
	pRes->Acceleration_X = ((s16_t) (pData->Reg_0x02.acc_x_lsb & 0x03)
			| ((s16_t) pData->acc_x_msb << 2));
	if (pRes->Acceleration_X & 0x0200)
		pRes->Acceleration_X |= 0xFC00;
	pRes->Acceleration_X *= AccSensitivity;

	pRes->Acceleration_Y = ((s16_t) (pData->Reg_0x04.acc_y_lsb & 0x03)
			| ((s16_t) pData->acc_y_msb << 2));
	if (pRes->Acceleration_Y & 0x0200)
		pRes->Acceleration_Y |= 0xFC00;
	pRes->Acceleration_Y *= AccSensitivity;

	pRes->Acceleration_Z = ((s16_t) (pData->Reg_0x06.acc_z_lsb & 0x03)
			| ((s16_t) pData->acc_z_msb << 2));
	if (pRes->Acceleration_Z & 0x0200)
		pRes->Acceleration_Z |= 0xFC00;
	pRes->Acceleration_Z *= AccSensitivity;

	pRes->Temperature = pData->temp;

	return 0;
}

static int prcRawCmpValues(MEMS_BMC050_Mag_Data_Type *pData,
		MEMS_BMC050_Result_Type *pRes) {

	pRes->CompDevID = pData->chip_id;

	pRes->MagField_X = (s16_t) pData->Reg_0x42.mag_x_lsb
			| (s16_t) pData->mag_x_msb << 5;
	if (pRes->MagField_X & 0x1000)
		pRes->MagField_X |= 0xE000;

	pRes->MagField_Y = (s16_t) pData->Reg_0x44.mag_y_lsb
			| (s16_t) pData->mag_y_msb << 5;
	if (pRes->MagField_Y & 0x1000)
		pRes->MagField_Y |= 0xE000;

	pRes->MagField_Z = (s16_t) pData->Reg_0x46.mag_z_lsb
			| (s16_t) pData->mag_z_msb << 7;
	if (pRes->MagField_Z & 0x4000)
		pRes->MagField_Z |= 0x8000;

	return 0;
}

static int readCurrAccelerationValues(MEMS_BMC050_Acc_Data_Type *pBuff) {
	u8_t wBuff_Accl[bufsize] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0 };

	wBuff_Accl[0] = ((1 << 7) | 0x00); // command: read from register 40h onward
	rwACC(wBuff_Accl, (u8_t*) pBuff, 10);

	return 0;
}

static int readCurrMagValues(MEMS_BMC050_Mag_Data_Type *pBuff) {
	u8_t wBuff_Comp[bufsize] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

	wBuff_Comp[0] = ((1 << 7) | 0x40); // command: read from register 40h onward
	rwMAG(wBuff_Comp, (u8_t*) pBuff, 9);

	return 0;
}

static int readAccRange(void) {
	u8_t wBuff[bufsize];
	u8_t rBuff[bufsize];
	int ret = 0;

	wBuff[0] = ((1 << 7) | 0x0F);  // command: read from register 0Fh onward
	wBuff[1] = *((u8_t*) &(BMC050_AccCompensate.Reg_0x37));
	rwACC(wBuff, rBuff, 2);

	switch (rBuff[1] & 0xF) {
	case 0x03:
		/* +- 2g range */
		AccSensitivity = 1;
		break;
	case 0x05:
		/* +- 4g range */
		AccSensitivity = 2;
		break;
	case 0x08:
		/* +- 8g range */
		AccSensitivity = 4;
		break;
	case 0x0C:
		/* +- 16g range */
		AccSensitivity = 8;
		break;
	default:
		ret = -1;
		break;
	}

	return ret;
}

static void setAccRange(void){
	
	switch(AccSensitivity){
	default:	
	case 1:
		/* +- 2g range */
		wAccReg(0x0F, 0x03);
		break;
	case 2:
		/* +- 4g range */
		wAccReg(0x0F, 0x05);
		break;
	case 4:
		/* +- 8g range */
		wAccReg(0x0F, 0x08);
		break;
	case 8:
		/* +- 16g range */
		wAccReg(0x0F, 0x0C);
		break;
	}
	return;
}

static int readAccIRQConfig(void) {
	u8_t wBuff[26]={0,0,0,0,0,0,0,0,0,0,
					0,0,0,0,0,0,0,0,0,0,
					0,0,0,0,0,0};
	int ret = 0;

	wBuff[0] = ((1 << 7) | 0x16);  // command: read from register 16h onward
	rwACC(wBuff, (u8_t*)&AccIRQConfig, 26);

	return ret;
}

static int accFastCompensationX(void) {

	if (AccCalRdy())
		return -1;

	wAccReg(0x36, 0x20);

	return 0;
}

static int accFastCompensationY(void) {

	if (AccCalRdy())
		return -1;

	wAccReg(0x36, 0x40);

	return 0;
}

static int accFastCompensationZ(void) {
	
	if (AccCalRdy())
		return -1;

	wAccReg(0x36, 0x60);

	return 0;
}

static int setAccCompensationTargets(void) {

	if (AccCalRdy())
		return -1;

	wAccReg(0x37, *((u8_t*) &(BMC050_AccCompensate.Reg_0x37)));

	return 0;
}

static int readAccCompenstaionValues(void) {
	u8_t wBuff[bufsize];

	wBuff[0] = ((1 << 7) | 0x36);  // command: read from register 36h onward
	wBuff[1] = 0x00; // + 8 clocks for compensation configuration part I
	wBuff[2] = 0x00; // + 8 clocks for compensation configuration part II
	wBuff[3] = 0x00; // + 8 clocks for compensation value for filtered data for the x-axis
	wBuff[4] = 0x00; // + 8 clocks for compensation value for filtered data for the y-axis.
	wBuff[5] = 0x00; // + 8 clocks for compensation value for filtered data for the z-axis.
	wBuff[6] = 0x00; // + 8 clocks for compensation value for unfiltered data for the x-axis.
	wBuff[7] = 0x00; // + 8 clocks for compensation value for unfiltered data for the y-axis.
	wBuff[8] = 0x00; // + 8 clocks for compensation value for unfiltered data for the z-axis.
	rwACC(wBuff, (u8_t*)&BMC050_AccCompensate, 9);

	return 0;
}

static int resetAccCompensationValues(void) {

	if (AccCalRdy())
		return -1;

	wAccReg(0x36, 0x80);

	return 0;
}

static u8_t checkAccIRQ(void) {
	return((LPC_GPIO2->PIN>>5)&1);
}

static u8_t checkMagIRQ(void) {
	return ((LPC_GPIO2->PIN>>6)&1);
}

static void handleAccIRQ(void) {
	u8_t wBuff[bufsize];
	MEMS_BMC050_Acc_IRQ_Type irqReg;

	wBuff[0] = ((1 << 7) | 0x09);  // command: read from register 09h onward
	wBuff[1] = 0x00; // + 8 clocks for IRQ part 1
	wBuff[2] = 0x00; // + 8 clocks for IRQ part 2
	wBuff[3] = 0x00; // + 8 clocks for IRQ part 3
	wBuff[4] = 0x00; // + 8 clocks for IRQ part 4
	rwACC(wBuff, (u8_t*)&irqReg, 5);

	irqReg.Reg_0x09.reseved = 0;
	irqReg.Reg_0x0A.reseved = 0;
	/**************************/
	/* handle interrupts here */
	/**************************/
	while (*((u8_t*)&irqReg.Reg_0x0A) != 0 || *((u8_t*)&irqReg.Reg_0x09) != 0) {
		irqReg.Reg_0x09.reseved = 0;
		irqReg.Reg_0x0A.reseved = 0;

		if (irqReg.Reg_0x09.flat_int) {
			/* flat interrupt */
			if (irqReg.Reg_0x0C.orient_z){
				BMC050_Readings.orientation = flat_upside_down;
			}else{
				BMC050_Readings.orientation = flat;
			}
			vertical_axis = Z_AXIS;
			irqReg.Reg_0x09.flat_int = 0;
		}

		if (irqReg.Reg_0x09.orient_int) {
			/* orientation interrupt */
			switch (irqReg.Reg_0x0C.orient_xy) {
			case 0:
				/* portrait upright */
				BMC050_Readings.orientation = portrait_upside_down;
				vertical_axis = X_AXIS;
				break;
			case 1:
				/* portrait upside down */
				BMC050_Readings.orientation = portrait;
				vertical_axis = X_AXIS;
				break;
			case 2:
				/* landscape left */
				BMC050_Readings.orientation = landscape_left;
				vertical_axis = Y_AXIS;
				break;
			case 3:
				/* landscape right */
				BMC050_Readings.orientation = landscape_right;
				vertical_axis = Y_AXIS;
				break;
			default:
				break;
			}

			irqReg.Reg_0x09.orient_int = 0;
		}

		if (irqReg.Reg_0x09.s_tap_int) {
			/* single tap interrupt */
			irqReg.Reg_0x09.s_tap_int = 0;
		}

		if (irqReg.Reg_0x09.d_tap_int) {
			/* double tap interrupt */
			irqReg.Reg_0x09.d_tap_int = 0;
		}

		if (irqReg.Reg_0x09.slope_int) {
			/* slope interrupt */
			irqReg.Reg_0x09.slope_int = 0;
		}

		if (irqReg.Reg_0x09.high_int) {
			/* high-g interrupt */
			irqReg.Reg_0x09.high_int = 0;
		}

		if (irqReg.Reg_0x09.low_int) {
			/* low-g interrupt */
			irqReg.Reg_0x09.low_int = 0;
		}

		if (irqReg.Reg_0x0A.data_int) {
			/* new data interrupt */
			irqReg.Reg_0x0A.data_int = 0;
		}
	}

	AccIRQConfig.Reg_0x21.reset_int = 1;
	wAccReg(0x21, *((u8_t*)&AccIRQConfig.Reg_0x21));
	
	return;
}

static void handleMagIRQ(void) {

	return;
}

// write single ACC register
static u8_t wAccReg(u8_t reg, u8_t val){
	u8_t wbuff[2];
	u8_t rbuff[2];

	wbuff[0]=reg;
	wbuff[1]=val;
	return rwACC(wbuff, rbuff, 2);
}

static u8_t rwACC(u8_t *p_wBuff, u8_t *p_rBuff, u8_t len){
	//SSP_DATA_SETUP_Type SSPdata;

	//if(!p_rBuff)
	//	return 1;
	if(!p_wBuff)
		return 1;
	if(!len)
		return 1;

	HW_BMC_CsAccelerationOn();
	SSP_ReadWrite(SSP_2, p_rBuff, p_wBuff,len);
	HW_BMC_CsAccelerationOff();

	return 0;
}

// write single magnetometer register
static u8_t wMagReg(u8_t reg, u8_t val){
	u8_t wbuff[2];
	u8_t rbuff[2];

	wbuff[0]=reg;
	wbuff[1]=val;
	return rwMAG(wbuff, rbuff, 2);
}

// read single magnetometer register
static u8_t rMagReg(u8_t reg, u8_t *p_val){
	u8_t wbuff[2];
	u8_t rbuff[2];

	if(!p_val)
		return 1;

	wbuff[0]=reg | 1<<7;
	wbuff[1]=0x00;

	if(rwMAG(wbuff, rbuff, 2)!=0)
		return 1;

	*p_val=rbuff[1];

	return 0;
}

static u8_t rwMAG(u8_t *p_wBuff, u8_t *p_rBuff, u8_t len){
	//SSP_DATA_SETUP_Type SSPdata;

	//if(!p_rBuff)
	//	return 1;
	if(!p_wBuff)
		return 1;
	if(!len)
		return 1;

	HW_BMC_CsMagnetometerOn();
	SSP_ReadWrite(SSP_2, p_rBuff, p_wBuff,len);
	HW_BMC_CsMagnetometerOff();

	return 0;
}



