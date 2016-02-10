/*******************************************************************************
 *
 * Project  :	PCAN-GPS
 * Module   :
 * Filename :	MEMS_L3GD20.c
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
 * 2014 June 04/StM	0.0.2	clean-up
 * 2013 Sep xx/StS	0.0.1	Initial Version
 ******************************************************************************/

/*******************************************************************************
 include files
 ******************************************************************************/
//
// System header files
//
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
#include "MEMS_L3GD20.h"

/*******************************************************************************
 global definitions
 ******************************************************************************/

// holds results of the L3GD20
MEMS_L3GD20_Result_Type L3GD20_Readings;

/*******************************************************************************
 local definitions
 ******************************************************************************/
typedef enum _L3GD20States {
	readGyroDevID,
	readGyroData,
	processResults,
	debugRegisters
} MEMS_L3GD20_States_Type;

// holds current state of state machine
static MEMS_L3GD20_States_Type L3GD20State = readGyroDevID;

static MEMS_L3GD20_Config_Register_Type ConfRegs;
static MEMS_L3GD20_Data_Register_Type DataRegs;

#define bufsize 16

#define L3GD20_ADDR_INC (1<<6)
#define L3GD20_WR		(0<<7)
#define L3GD20_RD		(1<<7)

//! multiplier for sensor values to have always the same scale in data
//! structure independent from sensors measurement range
const static float sens[4] = { 0.00875F, 0.0175F, 0.070F, 0.070F };

static u8_t wBuff_GyroData[bufsize];

static u8_t ssp_init=0;			//<! ssp initialized yet?
static u8_t data_init=0;		//<! data structure initialized yet?


/*******************************************************************************
 local function prototypes
 ******************************************************************************/
static void initData(void);
static HWStatus_t initSSP(void);
static void prcRawValues(MEMS_L3GD20_Data_Register_Type *pData,
		MEMS_L3GD20_Config_Register_Type *pConf, MEMS_L3GD20_Result_Type *pRes);
static HWStatus_t rwL3GD20(u8_t *p_wBuff, u8_t *p_rBuff, u8_t len);
static HWStatus_t wL3GD20Reg(u8_t reg, u8_t val);
static HWStatus_t rL3GD20Reg(u8_t reg, u8_t *p_val);


/*******************************************************************************
 global functions
 ******************************************************************************/


//------------------------------------------------------------------------------
//! HWStatus_t MEMS_L3GD20_init(void)
//------------------------------------------------------------------------------
//! @brief	initializes L3GD20 sensor with default settings. If not done yet SSP
//!			port is also initialized
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
HWStatus_t MEMS_L3GD20_init(void){

	if(!ssp_init){
		if(initSSP() != HW_ERR_OK)
			return HW_ERR_RESOURCE;
	}

	if(!data_init)
		initData();

	// prepare write command to write from register 20h onward
	ConfRegs.dummy = L3GD20_WR | L3GD20_ADDR_INC | 0x20;

	// enable all axis, set ~power down, set cut-off 50 output data rate 380Hz
	ConfRegs.Reg_0x20.Xen = 1;
	ConfRegs.Reg_0x20.Yen = 1;
	ConfRegs.Reg_0x20.Zen = 1;
	ConfRegs.Reg_0x20.pd = 1;
	ConfRegs.Reg_0x20.bw = 2;
	ConfRegs.Reg_0x20.dr = 2;

	// disable high pass filter
	ConfRegs.Reg_0x21.hpcf = 0;
	ConfRegs.Reg_0x21.hpm = 0;
	ConfRegs.Reg_0x21.reserved = 0;

	// disable interrupts
	ConfRegs.Reg_0x22.h_lactive = 0;
	ConfRegs.Reg_0x22.i1_boot = 0;
	ConfRegs.Reg_0x22.i1_int1 = 0;
	ConfRegs.Reg_0x22.i2_drdy = 0;
	ConfRegs.Reg_0x22.i2_empty = 0;
	ConfRegs.Reg_0x22.i2_orun = 0;
	ConfRegs.Reg_0x22.i2_wtm = 0;
	ConfRegs.Reg_0x22.pp_od = 0;

	// set block data update
	ConfRegs.Reg_0x23.bdu = 1;
	ConfRegs.Reg_0x23.ble = 0;
	ConfRegs.Reg_0x23.fs = 0;
	ConfRegs.Reg_0x23.reserved = 0;
	ConfRegs.Reg_0x23.sim = 0;

	// enable fifo, set boot = 1
	ConfRegs.Reg_0x24.boot = 1;
	ConfRegs.Reg_0x24.fifo_en = 0;
	ConfRegs.Reg_0x24.hpen = 0;
	ConfRegs.Reg_0x24.int_sel = 0;
	ConfRegs.Reg_0x24.out_sel = 0;
	ConfRegs.Reg_0x24.reserved = 0;

	rwL3GD20((u8_t*)&ConfRegs, NULL, 6);

	// write FIFO register
	wL3GD20Reg(0x2E, 0);

	Wait_Usec(2000);

	return HW_ERR_OK;
}




//------------------------------------------------------------------------------
//! void MEMS_L3GD20_task(void)
//------------------------------------------------------------------------------
//! @brief	Gyroscope main task. Reads data from sensor, processes it and writes
//!			results to the according data structure.
//!			Should be called as often as possible.
//------------------------------------------------------------------------------
void MEMS_L3GD20_task(void){

	switch (L3GD20State) {
	case readGyroDevID:
		rL3GD20Reg(0x0F, &L3GD20_Readings.GyroDevID);
		L3GD20State = readGyroData;
		break;
	case readGyroData:
		rwL3GD20(wBuff_GyroData, (u8_t*)&DataRegs, 10);
		L3GD20State = processResults;
		break;
	case processResults:
		prcRawValues(&DataRegs, &ConfRegs ,&L3GD20_Readings);
		//! switch to next state after whole data reading/processing cycle
		L3GD20State = readGyroData;
		break;
	default:
		break;
		//-------------------------
	}

} // MEMS_L3GD20_task



//------------------------------------------------------------------------------
//! HWStatus_t MEMS_L3GD20_SetRange(u8_t range)
//------------------------------------------------------------------------------
//! @brief	Sets measurement range of gyroscope
//------------------------------------------------------------------------------
//! @param	range	range to use (0=250[dps]; 1=500[dps]; 2=2000[dps];
//!					3=2000[dps])
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
HWStatus_t MEMS_L3GD20_SetRange(u8_t range){
	u8_t wBuff[2];
	u8_t rBuff[2];

	if(range>3)
		return HW_ERR_ILLPARAMVAL;

	ConfRegs.Reg_0x23.fs = range;

	wBuff[0] = L3GD20_WR | 0x23; // write register 23h
	wBuff[1] = *((u8_t*)&ConfRegs.Reg_0x23);

	rwL3GD20(wBuff, rBuff, 2);
		
	return HW_ERR_OK;
}


//------------------------------------------------------------------------------
//! HWStatus_t MEMS_L3GD20_GetRange(u8_t *p_val)
//------------------------------------------------------------------------------
//! @brief	Reads the current measurement range of the sensor
//------------------------------------------------------------------------------
//! @param	p_val	pointer to buffer for the result
//!					(0=250[dps]; 1=500[dps]; 2=2000[dps]; 3=2000[dps])
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
HWStatus_t MEMS_L3GD20_GetRange(u8_t *p_val){
	HWStatus_t res;

	if(!p_val)
		return HW_ERR_ILLPARAMVAL;

	res = rL3GD20Reg(0x23, (u8_t*)&ConfRegs.Reg_0x23);
	if(res != HW_ERR_OK)
		return res;

	*p_val = ConfRegs.Reg_0x23.fs;

	return HW_ERR_OK;
}



//------------------------------------------------------------------------------
//! static HWStatus_t initSSP(void)
//------------------------------------------------------------------------------
//! @brief	initializes SSP for Gyroscope usage
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
static HWStatus_t initSSP(void){
	SSPInit_t init;
	SSPStatus_t res=0;

	init.bitlen = t8_bit;
	init.clk_phase = 1;
	init.clk_pol = 1;
	init.mode = 0;
	init.frm_format = 0;
	init.loop_back=0;

	init.ser_clk_rate = 0; 		//! 1 clocks per bit
	init.clk_prescaler = 12;

	init.enable=1;

	res = SSP_Init(SSP_1, &init);

	if(res != SSP_ERR_OK)
		return HW_ERR_RESOURCE;

	ssp_init=1;

	return HW_ERR_OK;
}



//------------------------------------------------------------------------------
//! static HWStatus_t initSSP(void)
//------------------------------------------------------------------------------
//! @brief	initializes internal data structures
//------------------------------------------------------------------------------
static void initData(void){

	// this buffer will clock out 12 bytes for reading the Gyro
	wBuff_GyroData[0] = (L3GD20_RD| L3GD20_ADDR_INC | 0x25); // command: read from register 25h onward and increment register for continuous readings
	wBuff_GyroData[1] = 0x00; // + 8 clocks for  REFERENCE
	wBuff_GyroData[2] = 0x00; // + 8 clocks for  OUT_TEMP
	wBuff_GyroData[3] = 0x00; // + 8 clocks for  STATUS_REG
	wBuff_GyroData[4] = 0x00; // + 8 clocks for  gyro out X LSB
	wBuff_GyroData[5] = 0x00; // + 8 clocks for  gyro out X MSB
	wBuff_GyroData[6] = 0x00; // + 8 clocks for  gyro out Y LSB
	wBuff_GyroData[7] = 0x00; // + 8 clocks for  gyro out Y MSB
	wBuff_GyroData[8] = 0x00; // + 8 clocks for  gyro out Z LSB
	wBuff_GyroData[9] = 0x00; // + 8 clocks for  gyro out Z MSB

	// no valid readings so far
	L3GD20_Readings.data_valid = FALSE;

	// init local state machine
	L3GD20State = readGyroDevID;
}



//------------------------------------------------------------------------------
//! static void prcRawValues(MEMS_L3GD20_Data_Register_Type *pData,
// MEMS_L3GD20_Config_Register_Type *pConf, MEMS_L3GD20_Result_Type *pRes)
//------------------------------------------------------------------------------
//! @brief	processes raw data read from gyroscopes registers and writes results
//!			to L3GD20_Readings structure
//------------------------------------------------------------------------------
static void prcRawValues(MEMS_L3GD20_Data_Register_Type *pData,
		MEMS_L3GD20_Config_Register_Type *pConf, MEMS_L3GD20_Result_Type *pRes){
	s16_t tmp;

	L3GD20_Readings.Temperature = pData->temp;

	tmp = (s16_t) (pData->x_l | (u16_t) pData->x_h << 8);
	L3GD20_Readings.Gyro_X = sens[pConf->Reg_0x23.fs] * tmp;

	tmp = (s16_t) (pData->y_l | (u16_t) pData->y_h << 8);
	L3GD20_Readings.Gyro_Y = sens[pConf->Reg_0x23.fs] * tmp;

	tmp = (s16_t) (pData->z_l | (u16_t) pData->z_h << 8);
	L3GD20_Readings.Gyro_Z = sens[pConf->Reg_0x23.fs] * tmp;

	L3GD20_Readings.data_valid = TRUE;
			
	return;
}



//------------------------------------------------------------------------------
//! static HWStatus_t rwL3GD20(u8_t *p_wBuff, u8_t *p_rBuff, u8_t len)
//------------------------------------------------------------------------------
//! @brief	reads / writes values from / to gyroscope registers.
//------------------------------------------------------------------------------
//! @param	p_wBuff		pointer to write buffer. first byte used as address,
//!						read/write bit and address increment bit
//! @param	p_rBuff		pointer to read buffer
//! @param	len			length of buffers
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
static HWStatus_t rwL3GD20(u8_t *p_wBuff, u8_t *p_rBuff, u8_t len){

	if(!p_wBuff)
		return HW_ERR_ILLPARAMVAL;
	if(!len)
		return HW_ERR_ILLPARAMVAL;

	HW_L3GD20_CsOn();
	SSP_ReadWrite(SSP_1, p_rBuff, p_wBuff, len);
	HW_L3GD20_CsOff();

	return HW_ERR_OK;
}



//------------------------------------------------------------------------------
//! static HWStatus_t wL3GD20Reg(u8_t reg, u8_t val)
//------------------------------------------------------------------------------
//! @brief	write single L3GD20 register
//------------------------------------------------------------------------------
//! @param	reg		address of register to write
//! @param	val		value that should be written into register
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
static HWStatus_t wL3GD20Reg(u8_t reg, u8_t val){
	u8_t wbuff[2];
	u8_t rbuff[2];

	wbuff[0]=reg;
	wbuff[1]=val;
	return rwL3GD20(wbuff, rbuff, 2);
}



//------------------------------------------------------------------------------
//! static HWStatus_t rL3GD20Reg(u8_t reg, u8_t *p_val)
//------------------------------------------------------------------------------
//! @brief	reads single L3GD20 register
//------------------------------------------------------------------------------
//! @param	reg		address of register to read
//! @param	p_val	pointer to buffer for register value
//------------------------------------------------------------------------------
//! @return one of the default HW_ERR codes
//------------------------------------------------------------------------------
static HWStatus_t rL3GD20Reg(u8_t reg, u8_t *p_val){
	u8_t wbuff[2];
	u8_t rbuff[2];
	HWStatus_t res;

	if(!p_val)
		return HW_ERR_ILLPARAMVAL;

	wbuff[0]=reg | L3GD20_RD;
	wbuff[1]=0x00;

	res = rwL3GD20(wbuff, rbuff, 2);
	if(res==HW_ERR_OK)
		*p_val=rbuff[1];

	return res;
}





