/**************************************************************************
MODULE:    MCOP_INC.h
CONTAINS:  MicroCANopen Plus, all includes
COPYRIGHT: Embedded Systems Academy, Inc. 2002-2015.
           All rights reserved. www.microcanopen.com
DISCLAIM:  Read and understand our disclaimer before using this code!
           www.esacademy.com/disclaim.htm
           This software was written in accordance to the guidelines at
           www.esacademy.com/software/softwarestyleguide.pdf
LICENSE:   THIS IS THE COMMERCIAL VERSION OF MICROCANOPEN PLUS
           ONLY USERS WHO PURCHASED A LICENSE MAY USE THIS SOFTWARE
           See file license_commercial_plus.txt or
           www.microcanopen.com/license_commercial_plus.txt
VERSION:   6.20, ESA 11-MAY-15
           $LastChangedDate: 2015-05-09 19:41:45 -0300 (Sat, 09 May 2015) $
           $LastChangedRevision: 3390 $
***************************************************************************/ 

#ifndef _MCOP_INC_H
#define _MCOP_INC_H

#ifdef __cplusplus
extern "C" {
#endif

#include "mco_types.h"
#include "mcohw.h"
#include "mco.h"
#include "mcohw_cfg.h"
#include "mcohw_LEDs.h"
#include "canfifo.h"
#include "mcop.h"
#include "xsdo.h"
#include "profile.h"
#include "lss.h"
#include "mlss.h"
#include "svninfo.h"

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

#if USE_LEDS
#include "mcohw_LEDs.h"
#endif

#ifdef __SIMULATION__
  #include "mcohwPCSIM.h"
  #include "simdriver.h"
#endif

#ifdef __cplusplus
}
#endif

#endif // _MCOP_INC_H
/**************************************************************************
END OF FILE
**************************************************************************/
