/*******************************************************************************
 *
 * Project  :	PCAN-GPS
 * Module   :
 * Filename :	UBLOX_MAX7W.c
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
 * 2014 June 04/StM	0.0.2	cleaned code, changed evaluation of substrings,
 * 							added HDOP, VDOP and PDOP
 * 							removed some unnecessary buffers
 * 2013 Sep xx/StS	0.0.1	Initial Version
 ******************************************************************************/

/*******************************************************************************
 include files
 ******************************************************************************/
//
// System header files
//
#include <stdio.h>      /* printf, NULL */
#include <stdlib.h>     /* strtof */
#include <string.h>
#include <lpc407x_8x_177x_8x.h>

//
// Library header files
//
#include "typedefs.h"
#include "hardware.h"
#include "serial.h"

//
// Source code header files
//
#include "MAX-7W.h"

/*******************************************************************************
 global definitions
 ******************************************************************************/

/*******************************************************************************
 local definitions
 ******************************************************************************/

#undef DEBUG_RAW_POS
//#define DEBUG_RAW_POS 1

//! size of uart reception buffer (data from gps-receiver, should be >=83)
#define GPS_BUF_SZ 128
#define SER_TX_FIFO_SIZE  64 	//!< size of TX soft-fifo
#define SER_RX_FIFO_SIZE 128 	//!< size of RX soft-fifo
#define	SER_BAUD_GPS	9600	//!< baudrate GPS-UART

#define TXT_ELEM_CNT 5		//!< number of elements in $GPTXT message buffer
#define RMC_ELEM_CNT 14		//!< number of elements in $GPRMC message buffer
#define VTG_ELEM_CNT 11		//!< number of elements in $GPVTG message buffer
#define GGA_ELEM_CNT 16		//!< number of elements in $GPGGA message buffer
#define GSA_ELEM_CNT 19		//!< number of elements in $GPGSA message buffer
#define GLL_ELEM_CNT 9		//!< number of elements in $GPGLL message buffer

UBLOX_MAX7W_Result_Type MAX7W_Readings; //!< received data of the MAX7W module

//! flag set if received NMEA string is ready to be evaluated
static u8_t nmea_str_complete = 0;

//! TX Fifo (soft-fifo read by TX complete interrupt)
static u8_t TxFifo[SER_TX_FIFO_SIZE];
//! RX Fifo (soft-fifo write by RX complete interrupt)
static u8_t RxFifo[SER_RX_FIFO_SIZE];

/*******************************************************************************
 local function prototypes
 ******************************************************************************/
static void evaluate_$GPTXT(char *p_msg, u16_t msg_len);
static void evaluate_$GPRMC(char *p_msg, u16_t msg_len);
static void evaluate_$GPVTG(char *p_msg, u16_t msg_len);
static void evaluate_$GPGGA(char *p_msg, u16_t msg_len);
static void evaluate_$GPGSA(char *p_msg, u16_t msg_len);
static void evaluate_$GPGLL(char *p_msg, u16_t msg_len);
static int validate_csum(char *p_msg, unsigned int msg_len);
static int spltStr(char *p_str, u16_t sz, char delim, char **pp_res, u8_t cnt);
static u8_t hex2ascii(u8_t chr);
static u8_t ascii2hex(u8_t chr);

/*******************************************************************************
 global functions
 ******************************************************************************/

//------------------------------------------------------------------------------
//! void Init_UART2(void)
//------------------------------------------------------------------------------
//! @brief	initializes UART for usage with GPS module
//------------------------------------------------------------------------------
void Init_UART2(void){

	SERInit_t params;

	//params.prescaler = (PCLK + 8 * SER_BAUD_3) / ( 16 * SER_BAUD_3);
	params.mulval = 4;
	params.divadd = 1;
	params.dlval = ((u32_t) ((double) PCLK / SER_BAUD_GPS / 16
			/ (1 + (double) params.divadd / params.mulval)));

	params.databits = 8;
	params.stopbits = 1;
	params.parity = SER_PARITY_NONE;

	params.pTxFifo = &TxFifo;
	params.pRxFifo = &RxFifo;

	params.TxFifoSize = SER_TX_FIFO_SIZE;
	params.RxFifoSize = SER_RX_FIFO_SIZE;

	SER_Initialize(SER_PORT3, &params);

	return;
}



//------------------------------------------------------------------------------
//! s32_t UBLOX_MAX7W_init(void)
//------------------------------------------------------------------------------
//! @brief	initializes UART and variables, activates power supply of GPS module
//------------------------------------------------------------------------------
s32_t UBLOX_MAX7W_init(void){
	s32_t res = 0;
	Init_UART2();

	HW_GPS_PowerOn();

	nmea_str_complete = 0;

	return res;
}



//------------------------------------------------------------------------------
//! void UBLOX_MAX7W_task(void)
//------------------------------------------------------------------------------
//! @brief	reads chars from GPS UART, stores them and extracts data from
//!			received messages
//!			should be called from main loop as often as possible
//------------------------------------------------------------------------------
void UBLOX_MAX7W_task(void){
	static u8_t rx_buff_ptr = 0; 		//<! index of current buffer element
	static char rx_buff[GPS_BUF_SZ];	//<! uart reception buffer
	u8_t msg_len = 0; 	//<! length of received message-3 for CS calculation
	u8_t cur_chr, rd;
	s32_t res;

	res = SER_Read(SER_PORT3, &cur_chr, 1, &rd);
	if ((res != SER_ERR_OK) || (rd < 1))
		return;

#if DEBUG_RAW_POS>0
	SER_Write(SER_PORT1, &cur_chr, rd);
#endif

	// store char and increase pointer up to GPS_BUF_SZ
	// overwrite uppermost entry with all subsequent chars
	rx_buff[rx_buff_ptr] = cur_chr;
	if (rx_buff_ptr < GPS_BUF_SZ)
		rx_buff_ptr++;

	if (cur_chr == 0x24){
		// $ => start of new message => reset buffer index
		rx_buff[0] = cur_chr;
		rx_buff_ptr = 1; // reset buffer pointer
	} else if (cur_chr == 0x0A) {
		// at this point received message is complete !!!
		rx_buff[rx_buff_ptr] = 0x00;

		if (nmea_str_complete == 0) {
			msg_len = rx_buff_ptr;
			nmea_str_complete = 1;
		}
		rx_buff_ptr = 0; // reset buffer index
	}

	if (nmea_str_complete != 1)
		return;

	nmea_str_complete = 0; // reset NMEA string complete flag

	if(!validate_csum((char *)&rx_buff[0], msg_len)){
		// checksum mismatch
		// ToDo report error if needed
		return;
	}

	if (strncmp((const char*) &rx_buff[0], "$GPRMC", 6) == 0) {
		evaluate_$GPRMC(&rx_buff[0], msg_len);
	} else if (strncmp((const char*) &rx_buff[0], "$GPVTG", 6) == 0) {
		evaluate_$GPVTG(&rx_buff[0], msg_len);
	} else if (strncmp((const char*) &rx_buff[0], "$GPGGA", 6) == 0) {
		evaluate_$GPGGA(&rx_buff[0], msg_len);
	} else if (strncmp((const char*) &rx_buff[0], "$GPGSA", 6) == 0) {
		evaluate_$GPGSA(&rx_buff[0], msg_len);
	} else if (strncmp((const char*) &rx_buff[0], "$GPGLL", 6) == 0) {
		evaluate_$GPGLL(&rx_buff[0], msg_len);
	} else if (strncmp((const char*) &rx_buff[0], "$GPTXT", 6) == 0) {
		evaluate_$GPTXT(&rx_buff[0], msg_len);
	} else {
		; // any other NMEA string: do nothing
	}

	return;
} // UBLOX_MAX7W_task



//------------------------------------------------------------------------------
//! static void evaluate_$GPTXT(char *p_msg, u16_t msg_le)
//------------------------------------------------------------------------------
//! @brief	extracts date from GPTXT message
//------------------------------------------------------------------------------
//! @param	p_msg		pointer to message buffer
//! @param	msg_len		length of message
//------------------------------------------------------------------------------
static void evaluate_$GPTXT(char *p_msg, u16_t msg_len){
	int res;
	char *p_token[TXT_ELEM_CNT] = { 0, 0, 0, 0, 0 };

	if(!p_msg)
		return;

	/***************************************************************************
	 *	$GPTXT,numMsg,msgNum,msgType,text*cs<CR><LF>
	 *  [0] -> "$GPTXT"
	 *  [1] -> total number of messages in this transmission
	 *  [2] -> number of this message in current transmission
	 *  [3] -> type of this message 	'00' (error) | '01' (warning) |
	 *  								'02' (notice) | '07' (user)
	 *  [4] -> text + checksum
	 **************************************************************************/
	res = spltStr(p_msg, msg_len, ',', &p_token[0], TXT_ELEM_CNT);

	if (res < TXT_ELEM_CNT - 1)
		return;

	if (!p_token[4])
		return;

	if (strncmp(p_token[4], "ANTSTATUS=", 10) == 0) {
		if (strncmp(p_token[4] + 10, "INIT", 4) == 0)
			MAX7W_Readings.Gps_AntennaStatus = 0;
		else if (strncmp(p_token[4] + 10, "DONTKNOW", 8) == 0)
			MAX7W_Readings.Gps_AntennaStatus = 1;
		else if (strncmp(p_token[4] + 10, "OK", 2) == 0)
			MAX7W_Readings.Gps_AntennaStatus = 2;
		else if (strncmp(p_token[4] + 10, "SHORT", 5) == 0)
			MAX7W_Readings.Gps_AntennaStatus = 3;
		else if (strncmp(p_token[4] + 10, "OPEN", 4) == 0)
			MAX7W_Readings.Gps_AntennaStatus = 4;
		else
			// any other string (undefined...)
			MAX7W_Readings.Gps_AntennaStatus = 0;
	}
}



//------------------------------------------------------------------------------
//! static void evaluate_$GPRMC(char *p_msg, u16_t msg_len)
//------------------------------------------------------------------------------
//! @brief	extracts date from GPRMC message
//------------------------------------------------------------------------------
//! @param	p_msg		pointer to message buffer
//! @param	msg_len		length of message
//------------------------------------------------------------------------------
static void evaluate_$GPRMC(char *p_msg, u16_t msg_len)
{
	int res;
	char *p_token[RMC_ELEM_CNT] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

	if(!p_msg)
		return;

	/***************************************************************************
	 *  should send NMEA-PGNs: 126992, 127258, 129025, 129026, 129029, 129033
	 *  +	127250
	 *	$GPRMC,time,status,lat,NS,long,EW,spd,cog,date,mv,mvEW,
	 *	posMode*cs<CR><LF>
	 *  [0] -> "$GPRMC"
	 *  [1] -> UTC time hhmmss.ss
	 *  [2] -> status 'V' (warning) | 'A' (valid data)
	 *  [3] -> latitude ddmm.mmmmm
	 *  [4] -> north / south indicator 'N'|'S'
	 *  [5] -> longitude dddmm.mmmmm
	 *  [6] -> east / west indicator 'E'|'W'
	 *  [7] -> speed over ground [kn]
	 *  [8] -> course over ground [degree]
	 *  [9] -> date ddmmyy
	 *  [10] -> magnetic variation (not used!)
	 *  [11] -> magnetic variation E/W east / west indicator (not used!)
	 *  [12] -> positioning mode	'N' (no fix) |
	 *  							'E' (estimated / dead reckoning fix) |
	 *  							'A' (autonomous GNSS fix) |
	 *  							'D' (differential GNSS fix)
	 *  [13] -> checksum
	 **************************************************************************/
	res = spltStr(p_msg, msg_len, ',', &p_token[0], RMC_ELEM_CNT);

	if (res < RMC_ELEM_CNT - 1)
		return;

	// make sure that length of token is okay
	if (p_token[1] && (p_token[2] == p_token[1] + 10)) {
		char *p_chr;
		p_chr = p_token[1];
		MAX7W_Readings.Time_Hrs = ascii2hex(*(p_chr++)) * 10;
		MAX7W_Readings.Time_Hrs += ascii2hex(*(p_chr++));
		MAX7W_Readings.Time_Min = ascii2hex(*(p_chr++)) * 10;
		MAX7W_Readings.Time_Min += ascii2hex(*(p_chr++));
		MAX7W_Readings.Time_Sec = ascii2hex(*(p_chr++)) * 10;
		MAX7W_Readings.Time_Sec += ascii2hex(*(p_chr++));
		MAX7W_Readings.Validity |= GPS_TIME_VALID;
	} else {
		MAX7W_Readings.Time_Hrs = 0;
		MAX7W_Readings.Time_Min = 0;
		MAX7W_Readings.Time_Sec = 0;
		MAX7W_Readings.Validity &= ~GPS_TIME_VALID;
	}

	// make sure that length of token is okay
	if (p_token[9] && (p_token[10] == p_token[9] + 7)) {
		char *p_chr;
		p_chr = p_token[9];
		MAX7W_Readings.Date_DayOfMonth = ascii2hex(*(p_chr++)) * 10;
		MAX7W_Readings.Date_DayOfMonth += ascii2hex(*(p_chr++));
		MAX7W_Readings.Date_Month = ascii2hex(*(p_chr++)) * 10;
		MAX7W_Readings.Date_Month += ascii2hex(*(p_chr++));
		MAX7W_Readings.Date_Year = ascii2hex(*(p_chr++)) * 10;
		MAX7W_Readings.Date_Year += ascii2hex(*(p_chr++));
		MAX7W_Readings.Validity |= GPS_DATE_VALID;
	} else {
		MAX7W_Readings.Date_Year = 0;
		MAX7W_Readings.Date_Month = 0;
		MAX7W_Readings.Date_DayOfMonth = 0;
		MAX7W_Readings.Validity &= ~GPS_DATE_VALID;
	}

	// first check status filed to know if position data is valid!
	if (p_token[2] && *p_token[2] == 'V')
		return;

	if (p_token[3])
		MAX7W_Readings.Pos_Latitude = strtof(p_token[3], NULL);
	if (p_token[4])
		MAX7W_Readings.Pos_LatitudeIndNS = *p_token[4];
	if (p_token[5])
		MAX7W_Readings.Pos_Longitude = strtof(p_token[5], NULL);
	if (p_token[6])
		MAX7W_Readings.Pos_LongitudeIndEW = *p_token[6];
//	if (p_token[8])
//		MAX7W_Readings.Nav_CourseOverGround = strtof(p_token[8],NULL);

}



//------------------------------------------------------------------------------
//! static void evaluate_$GPVTG(char *p_msg, u16_t msg_len)
//------------------------------------------------------------------------------
//! @brief	extracts date from GPVTG message
//------------------------------------------------------------------------------
//! @param	p_msg		pointer to message buffer
//! @param	msg_len		length of message
//------------------------------------------------------------------------------
static void evaluate_$GPVTG(char *p_msg, u16_t msg_len){
	int res;
	char *p_token[VTG_ELEM_CNT] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

	if(!p_msg)
		return;

	/***************************************************************************
	 *  should contain NMEA-PGNs: 129026
	 *	$GPVTG,cogt,T,cogm,M,knots,N,kph,K,posMode*cs<CR><LF>
	 *  [0] -> "$GPVTG"
	 *  [1] -> course over ground (true) [degree]
	 *  [2] -> fix 'T'
	 *  [3] -> course over ground magnetic (not used!)
	 *  [4] -> fix 'M'
	 *  [5] -> speed over ground [kn]
	 *  [6] -> fix 'N'
	 *  [7] -> speed over ground [km/h]
	 *  [8] -> fix 'K'
	 *  [9] -> positioning mode	'N' (no fix) |
	 *  						'E' (estimated / dead reckoning fix) |
	 *  						'A' (autonomous GNSS fix) |
	 *  						'D' (differential GNSS fix)
	 *  [10] -> checksum
	 **************************************************************************/
	res = spltStr(p_msg, msg_len, ',', &p_token[0], VTG_ELEM_CNT);

	if (res < VTG_ELEM_CNT - 1)
		return;

	if (p_token[1])
		MAX7W_Readings.Nav_CourseOverGround = strtof(p_token[1], NULL);
	if (p_token[7])
		MAX7W_Readings.Nav_SpeedOverGroundKmh = strtof(p_token[7], NULL);
}


//------------------------------------------------------------------------------
//! static void evaluate_$GPGGA(char *p_msg, u16_t msg_len)
//------------------------------------------------------------------------------
//! @brief	extracts date from GPGGA message
//------------------------------------------------------------------------------
//! @param	p_msg		pointer to message buffer
//! @param	msg_len		length of message
//------------------------------------------------------------------------------
static void evaluate_$GPGGA(char *p_msg, u16_t msg_len)
{
	int res;
	char *p_token[GGA_ELEM_CNT] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0 };

	if(!p_msg)
		return;

	/***************************************************************************
	 *  should contain NMEA-PGNs: 126992, 129025, 129029, 129033, 129539
	 *	$GPGGA,time,lat,NS,long,EW,quality,numSV,HDOP,alt,M,sep,M,diffAge,
	 *	diffStation*cs<CR><LF>
	 *  [0] -> "$GPGGA"
	 *  [1] -> UTC time hhmmss.ss
	 *  [2] -> latitude ddmm.mmmmm
	 *  [3] -> north / south indicator 'N'|'S'
	 *  [4] -> longitude dddmm.mmmmm
	 *  [5] -> east / west indicator 'E'|'W'
	 *  [6] -> quality of fix 	'0' (no fix) | '1' (standard GPS (2D/3D)) |
	 *  						'2' Differential GPS | '6' (estimated DR fix)
	 *  [7] -> number of used satellites
	 *  [8] -> horizontal dilution of precision
	 *  [9] -> altitude above mean see level
	 *  [10] -> altitude unit (fix) 'M' (meters)
	 *  [11] -> geoid separation
	 *  [12] -> geoid separation unit (fix) 'M' (meters)
	 *  [13] -> age of differential correction (DGPS only)
	 *  [14] -> ID of differantial correction station (DGPS only)
	 *  [15] -> checksum
	 **************************************************************************/
	res = spltStr(p_msg, msg_len, ',', &p_token[0], GGA_ELEM_CNT);

	if (res < GGA_ELEM_CNT - 1)
		return;

	if (p_token[7])
		MAX7W_Readings.Nav_NumSatellites = (u8_t) strtoul(p_token[7], NULL, 10);
	if (p_token[8])
		MAX7W_Readings.HDOP = strtof(p_token[8], NULL);
	if (p_token[9])
		MAX7W_Readings.Pos_AltitudeOverSea = strtof(p_token[9], NULL);

	return;
}



//------------------------------------------------------------------------------
//! static void evaluate_$GPGSA(char *p_msg, u16_t msg_len)
//------------------------------------------------------------------------------
//! @brief	extracts date from GPGSA message
//------------------------------------------------------------------------------
//! @param	p_msg		pointer to message buffer
//! @param	msg_len		length of message
//------------------------------------------------------------------------------
static void evaluate_$GPGSA(char *p_msg, u16_t msg_len){
	int res;
	char *p_token[GSA_ELEM_CNT] = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0, 0 };

	if(!p_msg)
		return;

	/***************************************************************************
	 *	$GPGSA,opMode,navMode{,sv},PDOP,HDOP,VDOP*cs<CR><LF>
	 *  [0] -> "$GPGSA"
	 *  [1] -> operation mode 'M' (manually set to 2D/3D mode)
	 *						  'A' (automatically choosing 2D/3D mode)
	 *  [2] -> navigation mode '1' (no fix) | '2' (2D fix) | '3' (3D fix)
	 *  [3-14] -> satellite number
	 *  [15] -> position dilution of precision
	 *  [16] -> horizontal dilution of precision
	 *  [17] -> vertical dilution of precision
	 *  [18] -> checksum
	 **************************************************************************/
	res = spltStr(p_msg, msg_len, ',', &p_token[0], GSA_ELEM_CNT);

	if (res < GSA_ELEM_CNT - 1)
		return;

	if (p_token[2])
		MAX7W_Readings.Nav_Method = ascii2hex(*p_token[2]);
	if (p_token[15])
		MAX7W_Readings.PDOP = strtof((const char*) p_token[15], NULL);
	if (p_token[16])
		MAX7W_Readings.HDOP = strtof((const char*) p_token[16], NULL);
	if (p_token[17])
		MAX7W_Readings.VDOP = strtof((const char*) p_token[17], NULL);

	return;
}




//------------------------------------------------------------------------------
//! static void evaluate_$GPGLL(void)
//------------------------------------------------------------------------------
//! @brief	extracts date from GPGLL message
//------------------------------------------------------------------------------
//! @param	p_msg		pointer to message buffer
//! @param	msg_len		length of message
//------------------------------------------------------------------------------
static void evaluate_$GPGLL(char *p_msg, u16_t msg_len){
	int res;
	char *p_token[GLL_ELEM_CNT] = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

	if(!p_msg)
		return;

	/***************************************************************************
	 *  should contain NMEA-PGNs: 126992, 129025, 129029, 129033
	 *  $GPGLL,lat,NS,long,EW,time,status,posMode*cs<CR><LF>
	 *  [0] -> "$GPGLL"
	 *  [1] -> latitude ddmm.mmmmm
	 *  [2] -> north / south indicator 'N'|'S'
	 *  [3] -> longitude dddmm.mmmmm
	 *  [4] -> east / west indicator 'E'|'W'
	 *  [5] -> UTC time hhmmss.ss
	 *  [6] -> status 'A' (data valid) | 'V' (data invalid)
	 *  [7] -> positioning mode (optional)
	 *  [7|8] -> checksum
	 **************************************************************************/
	res = spltStr(p_msg, msg_len, ',', &p_token[0], GLL_ELEM_CNT);

	if (res < GLL_ELEM_CNT - 2) // one optional element
		return;

	// make sure that length of token is okay
	if (p_token[5] && (p_token[6] == p_token[5] + 10)) {
		char *p_chr;
		p_chr = p_token[5];
		MAX7W_Readings.Time_Hrs = ascii2hex(*(p_chr++)) * 10;
		MAX7W_Readings.Time_Hrs += ascii2hex(*(p_chr++));
		MAX7W_Readings.Time_Min = ascii2hex(*(p_chr++)) * 10;
		MAX7W_Readings.Time_Min += ascii2hex(*(p_chr++));
		MAX7W_Readings.Time_Sec = ascii2hex(*(p_chr++)) * 10;
		MAX7W_Readings.Time_Sec += ascii2hex(*(p_chr++));
		MAX7W_Readings.Validity |= GPS_TIME_VALID;
	} else {
		MAX7W_Readings.Time_Hrs = 0;
		MAX7W_Readings.Time_Min = 0;
		MAX7W_Readings.Time_Sec = 0;
		MAX7W_Readings.Validity &= ~GPS_TIME_VALID;
	}

	// first check status filed to know if position data is valid!
	if (p_token[6] && *p_token[6] == 'V')
		return;

	if (p_token[1])
		MAX7W_Readings.Pos_Latitude = strtof((const char*) p_token[1], NULL);
	if (p_token[2])
		MAX7W_Readings.Pos_LatitudeIndNS = *p_token[2]; // character 'N' or 'S'
	if (p_token[3])
		MAX7W_Readings.Pos_Longitude = strtof((const char*) p_token[3], NULL);
	if (p_token[4])
		MAX7W_Readings.Pos_LongitudeIndEW = *p_token[4]; // character 'E' or 'W'

	return;
}


//------------------------------------------------------------------------------
//! static int validate_csum(char *p_msg, unsigned int msg_len)
//------------------------------------------------------------------------------
//! @brief	calculates checksum of nmea string and compares it to the on that
//!			was contained in the message
//------------------------------------------------------------------------------
//! @param	p_msg		pointer to message buffer
//! @param	msg_len		length of message
//------------------------------------------------------------------------------
//! @return 0 if checksums is false
//! @return 1 if checksum is okay
//------------------------------------------------------------------------------
static int validate_csum(char *p_msg, unsigned int msg_len){
	u8_t cs_calc;		//!< self-calculated checksum of the incoming message
	u8_t cs_rcv;  		//!< received checksum of the incoming message
	u8_t i_csum;

	// msg_len is incl. leading $ but without trailing <0D><0A><00>
	msg_len-=3;

	// see if checksum of received NmeaCmdString is ok
	cs_rcv = (ascii2hex(p_msg[msg_len - 1]) << 4) & 0x0F0;
	cs_rcv = (cs_rcv | ((ascii2hex(p_msg[msg_len])) & 0x00F));

	// calculate checksum of anything between $ and *
	cs_calc = 0;
	for (i_csum = 1; i_csum <= msg_len-3; i_csum++) {
		cs_calc = cs_calc ^ (p_msg[i_csum]);
	}

	if (cs_calc != cs_rcv)
		return 0;

	return 1;
}



//------------------------------------------------------------------------------
//! static int spltStr(char *p_str, u16_t sz, char delim, char **pp_res, u8_t cnt)
//------------------------------------------------------------------------------
//! @brief	splits an separated string into n strings.
//!			Attention: String gets changed! every delimiter is replaced by '\0'
//------------------------------------------------------------------------------
//! @param	p_str	pointer to string
//! @param	sz		string size
//! @param	delim	delimiter that separates string parts
//! @param	pp_res	pointer to result array
//! @param	cnt		size of array
//------------------------------------------------------------------------------
//! @return <0 in case of errors
//! @return >=0 number of elements the string is spit in
//!				(never > cnt -> further elements are ignored).
//------------------------------------------------------------------------------
static int spltStr(char *p_str, u16_t sz, char delim, char **pp_res, u8_t cnt) {
	u8_t i; //!< position in string
	u8_t k; //!< number of current element

	if (!p_str)
		return -1;
	if (!pp_res)
		return -1;
	if (sz < 1)
		return -1;

	k = 0;

	pp_res[k++] = &p_str[0];

	// search max cnt elements in first sz chars
	for (i = 0; i < sz; i++) {
		if (p_str[i] == '\0') {
			return k;
		} else if (p_str[i] == delim) {
			p_str[i] = '\0';
			pp_res[k++] = &p_str[i + 1];
		}

		if (k >= cnt)
			return k;
	}

	return k;
}



//------------------------------------------------------------------------------
//! static u8_t hex2ascii(u8_t chr)
//------------------------------------------------------------------------------
//! @brief	translates hex number to character
//------------------------------------------------------------------------------
//! @param	chr	value to convert
//------------------------------------------------------------------------------
//! @return converted char
//------------------------------------------------------------------------------
static u8_t hex2ascii(u8_t chr){
	chr = chr & 0xF;
	if (chr > 9) {
		chr += ('A' - 10); //characters
	} else {
		chr += '0'; // numbers
	}
	return chr;
}



//------------------------------------------------------------------------------
//! static u8_t hex2ascii(u8_t chr)
//------------------------------------------------------------------------------
//! @brief	translates character to hex number
//------------------------------------------------------------------------------
//! @param	chr	char to convert
//------------------------------------------------------------------------------
//! @return converted number
//------------------------------------------------------------------------------
static u8_t ascii2hex(u8_t chr){
	if ((chr <= 'F') && (chr >= 'A')) // upper case characters
			{
		chr -= ('A' - 10);
		return chr & 0xF;
	} else if ((chr <= 'f') && (chr >= 'a')) // lower case characters
			{
		chr -= ('a' - 10);
		return chr & 0xF;
	}

	else if ((chr <= '9') && (chr >= '0')) // numbers
			{
		chr -= '0';
	} else {
		chr = 0;
	}
	return chr & 0xF;
}




