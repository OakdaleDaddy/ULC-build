
#include "datatypes.h"
#include "lpc21xx.h"
#include "can.h"
#include "can_user.h"


// Queues for CAN1
CANMsg_t  TxQueueCAN1[CAN1_TX_QUEUE_SIZE];
CANMsg_t  RxQueueCAN1[CAN1_RX_QUEUE_SIZE];


u32_t CAN_UserInit(u32_t bitRateCode)
{
   u32_t result = 1;
   u32_t bitTiming = CAN_BAUD_500K;

   if ( (0 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_10K;
   }
   else if ( (1 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_20K;
   }
   else if ( (3 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_50K;
   }
   else if ( (4 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_125K;
   }
   else if ( (5 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_250K;
   }
   else if ( (6 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_500K;
   }
   else if ( (7 == bitRateCode) )
   {
      bitTiming = CAN_BAUD_1M;
   }

   // init CAN1

   result &= CAN_ReferenceTxQueue ( CAN_BUS1, &TxQueueCAN1[0], CAN1_TX_QUEUE_SIZE);          // Reference above Arrays as Queues
   result &= CAN_ReferenceRxQueue ( CAN_BUS1, &RxQueueCAN1[0], CAN1_RX_QUEUE_SIZE);

   result &= CAN_SetTimestampHandler ( CAN_BUS1, NULL);

   VICVectAddr1 = (u32_t) CAN_GetIsrVector ( CAN1_TX_INTSOURCE);
   VICVectAddr3 = (u32_t) CAN_GetIsrVector ( CAN1_RX_INTSOURCE);

   VICVectCntl1 = 1 << 5 | CAN1_TX_INTSOURCE;                                 // Setup VIC
   VICVectCntl3 = 1 << 5 | CAN1_RX_INTSOURCE;

   VICIntEnable = 1 << CAN1_TX_INTSOURCE | 1 << CAN1_RX_INTSOURCE;

   result &= CAN_SetErrorLimit ( CAN_BUS1, STD_TX_ERRORLIMIT);

   result &= CAN_SetTxErrorCallback ( CAN_BUS1, NULL);                                 // Set ErrorLimit & Callbacks
   result &= CAN_SetRxCallback ( CAN_BUS1, NULL);

   result &= CAN_SetChannelInfo ( CAN_BUS1, NULL);                                     // Textinfo is NULL


   // Set Error Handler

   VICVectAddr0 = (u32_t) CAN_GetIsrVector ( GLOBAL_CAN_INTSOURCE);
   VICVectCntl0 = 1 << 5 | GLOBAL_CAN_INTSOURCE;
   VICIntEnable = 1 << GLOBAL_CAN_INTSOURCE;


   // Setup Filters

   result &= CAN_InitFilters();                             // Clear Filter LUT
   result &= CAN_SetFilterMode ( AF_ON_BYPASS_ON);          // No Filters ( Bypassed)


   // init CAN1
   result &= CAN_InitChannel ( CAN_BUS1, bitTiming);
   
   
   //
   result &= CAN_SetTransceiverMode ( CAN_BUS1, CAN_TRANSCEIVER_MODE_NORMAL);


   // Buses on

   result &= CAN_SetBusMode ( CAN_BUS1, BUS_ON);               // CAN Bus On

   return (result);
}


// CAN_UserWrite()
// Send a message on CAN_BUSx
CANStatus_t  CAN_UserWrite ( CANHandle_t  hBus, CANMsg_t  *pBuff)
{
	CANStatus_t  ret;
	CANMsg_t  *pMsg;
	
	
	ret = CAN_ERR_OK;
	
	pMsg = CAN_TxQueueGetNext ( hBus);

	if ( pMsg != NULL)
	{
		pMsg->Id   = pBuff->Id;
		pMsg->Len  = pBuff->Len;
		pMsg->Type = pBuff->Type;
		
		pMsg->Data32[0] = pBuff->Data32[0];
		pMsg->Data32[1] = pBuff->Data32[1];
		
		// Send Msg
		ret = CAN_TxQueueWriteNext ( hBus);
	}
	
	else
	{
		// Tx Queue FULL
		ret = CAN_ERR_FAIL;
	}
	
	return ret;
}




// CAN_UserRead()
// read message from CAN_BUSx
u32_t  CAN_UserRead ( CANHandle_t  hBus, CANMsg_t  *pBuff)
{
	u32_t  ret;
	CANMsg_t  *pMsg;
	
	
	ret = 0;
	
	pMsg = CAN_RxQueueGetNext ( hBus);

	if ( pMsg != NULL)
	{
		pBuff->Id   = pMsg->Id;
		pBuff->Len  = pMsg->Len;
		pBuff->Type = pMsg->Type;
		
		pBuff->Data32[0] = pMsg->Data32[0];
		pBuff->Data32[1] = pMsg->Data32[1];
		
		CAN_RxQueueReadNext ( hBus);
		ret = 1;
	}
	
	return ret;
}




