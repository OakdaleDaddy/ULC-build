//
//	ssp.h
//
//	-------------------------------------------------------------------
//! @addtogroup SSP
//! <h3> API Functions for synchronous serial port access </h3>
//!
//! Functions for synchronous serial port communication. The functions will
//! target to the main() level. When used
//! on interrupts, the user must provide own wrapper functions for interrupt
//! safety.
//!
//! <h3> Targets: </h3>
//! - PCAN-GPS
//!
//	-------------------------------------------------------------------
//
//	Copyright (C) 1999-2014  PEAK-System Technik GmbH, Darmstadt
//	more Info at http://www.peak-system.com
//
//! @{

#ifndef SSP_H_
#define SSP_H_

////////////////////////////////////////////////////////////////////////////////
//! @name port-handles
//! Use one of these port handles to access a synchronous serial port
//! @{
#define	SSP_1			0u		//!< Synchronous Serial Port 1 - connected to L3GD20
#define	SSP_2			1u		//!< Synchronous Serial Port 2
#define	SSP_3			2u		//!< Synchronous Serial Port 3
#define	SSPORT_MIN	 SSP_1		//!< Lowest Synchronous Serial Port
#define	SSPORT_MAX	 SSP_3		//!< Highest Synchronous Serial Port
#define SSP_USED_CNT 	3u		//!< number of synchronous serial ports used
#define SSP_HW_CNT 		3u		//!< number of synchronous serial ports in total
/*! @}*/

////////////////////////////////////////////////////////////////////////////////
//! @name errors
//! A function returns one of these errors:
//! @{
#define	SSP_ERR_OK				0		//!< OK, no error
#define	SSP_ERR_FAIL			1		//!< error
#define	SSP_ERR_ILLHANDLE		2		//!< invalid port handle
#define	SSP_ERR_ILLPARAMVAL		3		//!< invalid parameter
#define	SSP_ERR_TRX_PENDING		4		//!< recent data exchange is has not finished yet
/*! @}*/

////////////////////////////////////////////////////////////////////////////////
//! @name status bits
//! bit definitions for status register:
//! @{
#define	SSP_TX_EMPTY		1u<<0	//!< Transmit FIFO Empty
#define	SSP_TX_NOT_FULL		1u<<1	//!< Transmit FIFO Not Full
#define	SSP_RX_NOT_EMPTY	1u<<2	//!< Receive FIFO Not Empty
#define	SSP_RX_FULL			1u<<3	//!< Receive FIFO Full
#define	SSP_BUSY			1u<<4	//!< 0 if the SSP controller is idle, or 1
									//!< if it is sending/receiving a frame and
									//!< /or the Tx FIFO is not empty.
/*! @}*/

////////////////////////////////////////////////////////////////////////////////
//! @name parity
//! options for initialization used by SSP_Init()
//! @{
/*! @}*/

////////////////////////////////////////////////////////////////////////////////
//! @name types
//! Basetypes for serial functions.
//! @{
#define	SSPStatus_t		u32_t			//!< status type for API functions
#define	SSPHandle_t		u8_t			//!< SER handle type
/*! @}*/

typedef enum SSP_DATA_LEN{
	na1 = 0,
	na2 = 1,
	na3 = 2,
	t4_bit = 3,
	t5_bit = 4,
	t6_bit = 5,
	t7_bit = 6,
	t8_bit = 7,
	t9_bit = 8,
	t10_bit = 9,
	t11_bit = 10,
	t12_bit = 11,
	t13_bit = 12,
	t14_bit = 13,
	t15_bit = 14,
	t16_bit = 15,
} E_SSP_DATA_LEN;

////////////////////////////////////////////////////////////////////////////////
//! @name structures
//! structures for serial interaction
//! @{

//! @brief
//! structure for serial initialization
typedef struct {
	u8_t bitlen; 		//!< number of data bits to use
	u8_t clk_phase;	//!< clock phase
	u8_t clk_pol;		//!< clock polarity
	u8_t frm_format;	//!< frame format:
	u8_t ser_clk_rate;//!< serial clock rate. Number of (pre-scaled) clocks per bit on the bus -1
	u8_t loop_back;	//!< enable loop back mode
	u8_t enable;		//!< enable port
	u8_t mode;			//!< SSP port mode
	u8_t clk_prescaler;	//!< Clock prescaler. Only even values read -> LSB is ignored
} SSPInit_t;

#ifdef __cplusplus
extern "C" {
#endif

////////////////////////////////////////////////////////////////////////////////
//! @name functions
//! This section will describe the API functions. The functions will target
//! to the main() level.
//! @{

//! @brief
//! Initialize a synchronous serial port
//!
//! @param		hPort		port to initialize
//! @param		setup		structure with setup values
//!
//! @return		one error of SSP_ERR_...
	SSPStatus_t SSP_Init(SSPHandle_t hPort, SSPInit_t *pSetup);

//! @brief
//! Read 16 bit data from given synchronous serial port
//!
//! @param		hPort		port to read data from
//! @param		*pBuffer	pointer to 16 bit wide user buffer
//!
//! @return		one error of SSP_ERR_...
	SSPStatus_t SSP_SingleRead(SSPHandle_t hPort, u16_t *pBuffer);

//! @brief
//! Writes 16 bit data to given synchronous serial port
//!
//! @param		hPort		port to send data to
//! @param		data		data to send
//!
//! @return		one error of SSP_ERR_...
	SSPStatus_t SSP_SingleWrite(SSPHandle_t hPort, u16_t data);

//! @brief
//! Read and write data from specific synchronous serial port.
//!
//! @param		hPort		port for data exchange
//! @param		pRead		buffer to store read data (u16_t or u8_t depending on 'bitlen' used for current transfer)
//! @param		pWrite		buffer with data to send (u16_t or u8_t depending on 'bitlen' used for current transfer)
//! @param		len			number of elements in read / write buffer (means numer of bytes for 'bitlen'<=8 and number of words is 'bitlen'>8)
//!
//! @return		one error of SER_ERR_...
	SSPStatus_t SSP_ReadWrite(SSPHandle_t hPort, void *pRead, void *pWrite,
			u8_t len);

	/*! @}*/

#ifdef __cplusplus
}
#endif

/*! @}*/

#endif /* SSP_H_ */
