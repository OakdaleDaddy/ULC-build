/**************************************************************************
MODULE:    PROFILE_CiA447
CONTAINS:  MicroCANopen application profile specific extensions
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL PLUS VERSION OF MICROCANOPEN
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
VERSION:   6.20, ESA 11-MAY-15
           $LastChangedDate: 2015-04-23 09:38:15 +0200 (Do, 23 Apr 2015) $
           $LastChangedRevision: 3367 $
***************************************************************************/ 


#ifndef _PROFILE_H
#define _PROFILE_H

#ifdef __cplusplus
extern "C" {
#endif


/**************************************************************************
Functions to store and retrieve the node ID from which the last SDO request
or RPDO was received
**************************************************************************/
void PROFILE_SetSDOFromNode (UNSIGNED8 node_id);
UNSIGNED8 PROFILE_GetSDOFromNode (void);
void PROFILE_SetRPDOFromNode (UNSIGNED8 node_id);
UNSIGNED8 PROFILE_GetRPDOFromNode (void);


#if USE_PROFILE_RPDO
/**************************************************************************
DOES:    Checks if an ID is a 447 PDO
RETURNS: TRUE if yes, else FALSE
**************************************************************************/
UNSIGNED8 IS_CAN_ID_ANY_PDO (UNSIGNED16 CANID);


/**************************************************************************
DOES:    Returns the node ID of the node transmitting a PDO
RETURNS: 1 to 16, or 127 if not known
**************************************************************************/
UNSIGNED8 GET_NODE_ID_FROM_PDO (UNSIGNED16 CANID);

/**************************************************************************
DOES:    This function checks if this RPDO CAN ID is a duplicate/mirror of 
         multiple same RPDOs coming from different devices.
         CiA447: supporting same virtual device PDO coming from multiple
         nodes.
RETURNS: RPDO CAN ID of the main RPDO that can be used to handle this RPDO.
**************************************************************************/
UNSIGNED32 PROFILE_ExtHandleRPDO(
  UNSIGNED32 RPDO_canid
  );
#endif

#ifdef __cplusplus
}
#endif

#endif // _PROFILE_H
/**************************************************************************
END OF FILE
**************************************************************************/
