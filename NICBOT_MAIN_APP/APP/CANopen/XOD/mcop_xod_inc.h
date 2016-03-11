/**************************************************************************
MODULE:    MCOP_XOD_INC.h
CONTAINS:  MicroCANopen Plus Extended PDO, all includes
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2016
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL VERSION OF MICROCANOPEN PLUS
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
           See file license_commercial_plus.txt or
           www.microcanopen.com/license_commercial_plus.txt
VERSION:   6.21, ESA 16-JUN-16
           $LastChangedDate: 2016-02-16 22:14:13 +0100 (Tue, 16 Feb 2016) $
           $LastChangedRevision: 3568 $
***************************************************************************/ 

#ifndef _MCOP_XOD_H
#define _MCOP_XOD_H

#ifdef __cplusplus
extern "C" {
#endif

#include "mco_types.h"

#include "mco.h"
#include "mcohw.h"
#include "mcohw_cfg.h"
#include "mcohw_LEDs.h"
#include "canfifo.h"
#include "mcop.h"
#include "xsdo.h"

#include "nodecfg.h"
#include "procimg.h"

#if (USE_DYNAMIC_PDO_MAPPING == 1)
 #include "xpdo.h"
#endif

#if (USE_XOD_ACCESS == 1)
 #include "xod.h"
#endif

#if (USE_REMOTE_ACCESS == 1)
 #include "mcohw_com.h"
 #include "raccess.h"
 #include "raserial.h"
 #include "racrc.h"
#endif


#ifdef __cplusplus
}
#endif

#endif // _MCOP_XOD_H
/**************************************************************************
END OF FILE
**************************************************************************/
