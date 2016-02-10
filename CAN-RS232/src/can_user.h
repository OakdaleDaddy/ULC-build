
#ifndef  _CAN_USER_H_
#define  _CAN_USER_H_

// defines
#define  CAN1_TX_QUEUE_SIZE	8
#define  CAN1_RX_QUEUE_SIZE	16



// Baudrates
// VPB clock 60 MHz, 15 Tsegs, sample point 80 %
//											-- SJW --	- Tseg1 -	- Tseg2 -	- BRP -
#define		CAN_BAUD_1M			(	0 << 14 |	10 << 16 |	2 << 20 |	3)
#define		CAN_BAUD_800K		(	0 << 14 |	10 << 16 |	2 << 20 |	4)
#define		CAN_BAUD_500K		(	0 << 14 |	10 << 16 |	2 << 20 |	7)
#define		CAN_BAUD_250K		(	0 << 14 |	10 << 16 |	2 << 20 |	15)
#define		CAN_BAUD_200K		(	0 << 14 |	10 << 16 |	2 << 20 |	19)
#define		CAN_BAUD_125K		(	0 << 14 |	10 << 16 |	2 << 20 |	31)
#define		CAN_BAUD_100K		(	0 << 14 |	10 << 16 |	2 << 20 |	39)
#define		CAN_BAUD_95K2		(	0 << 14 |	10 << 16 |	2 << 20 |	41)
#define		CAN_BAUD_83K3		(	0 << 14 |	10 << 16 |	2 << 20 |	47)
#define		CAN_BAUD_50K		(	0 << 14 |	10 << 16 |	2 << 20 |	79)
#define		CAN_BAUD_47K6		(	0 << 14 |	10 << 16 |	2 << 20 |	83)
#define		CAN_BAUD_33K3		(	0 << 14 |	10 << 16 |	2 << 20 |	119)
#define		CAN_BAUD_20K		(	0 << 14 |	10 << 16 |	2 << 20 |	199)
#define		CAN_BAUD_10K		(	0 << 14 |	10 << 16 |	2 << 20 |	399)

#define CAN_COB(type,id) ((type<<7)|id)
#define CAN_TYPE(cob) ((cob>>7)&0xF)
#define CAN_ID(cob) (cob&0x7F)

typedef enum
{
   CAN_NMT_T = 0,
   CAN_SYNC_T = 1,
   CAN_EMGY_T = 1,
   CAN_TS_T = 2,
   CAN_TPDO1_T = 3,
   CAN_RPDO1_T,
   CAN_TPDO2_T,
   CAN_RPDO2_T,
   CAN_TPDO3_T,
   CAN_RPDO3_T,
   CAN_TPDO4_T,
   CAN_RPDO4_T,
   CAN_TSDO_T,
   CAN_RSDO_T,
   CAN_ERROR_T = 14,
}CAN_FrameType;

u32_t CAN_UserInit(u32_t bitRateCode);
u32_t  CAN_UserRead ( CANHandle_t  hBus, CANMsg_t  *pBuff);
CANStatus_t  CAN_UserWrite ( CANHandle_t  hBus, CANMsg_t  *pBuff);

#endif
