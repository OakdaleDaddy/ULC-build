///////////////////////////////////////////////////////////////////////////////
//  Based on:
//  PCAN_ISA.cs
//  PCAN_2ISA.cs
//  PCAN_PCI.cs
//  PCAN_2PCI.cs
//  PCAN_PCC.cs
//  PCAN_2PCC.cs
//  PCAN_DNG.cs
//  PCAN_DNP.cs
//  PCAN_USB.cs
//  PCAN_2USB.cs
//
//  Version 2.0
//
//  ~~~~~~~~~~~~
//
//  Idea:
//
//  ~~~~~~~~~~
//
//  PCANLight is a namespace used to make the managing of the different PCAN Hardware using the 
//  PCANLight Dlls: pcan_isa,pcan_2isa,pcan_pci,pcan_2pci,pcan_pcc,pcan_2pcc,pcan_dng,pcan_dnp,
//  pcan_usb, pcan_2usb
//
//  In order  to offer  a simple  interface, some constant valuest were converted  to enumerate 
//  types.  The class CANLight  make use of all Dlls and gives  an unique interface for all the 
//  hardware types.  A TCLightMsg class is implemented too.  This class will  be used to make a 
//  bridge between the definition of the TCANMsg structure in every *.cs file.  In this way, in 
//  this high level,  will  exists  only  one  kind of CAN  message and  one occurrence of each 
//  PCANLIGHT function.
//
//  ~~~~~~~~~~~~
//
//  PCAN-Light -API
//
//  ~~~~~~~~~~~~
//
//   Init() (Two versions, for P&P and Non P&P)
//   Close() 
//   Status()
//   Write() 
//   Read()  
//   ReadEx()
//   VersionInfo() 
//   DLLVersionInfo()
//   ResetClient()
//   MsgFilter()
//	 ResetFilter()
//	 SetUSBDeviceNr()
//   GetUSBDeviceNr()
//   SetRecvEvent()
//
//  ------------------------------------------------------------------
//
//  Author  : Keneth Wagner
//  Language: C# 1.0
//  Last Modified: Wagner (28.09.2009)
//
//  ------------------------------------------------------------------
//  Copyright (C) 2006-2009  PEAK-System Technik GmbH, Darmstadt
//
using System;
using System.Text;
using System.Threading;

using DYNO.Utilities;
using Peak.Can.Light;

namespace DYNO.PCANLight
{
   #region Types definition
   /// <summary>
   /// Kind of Frame - Message Type
   /// </summary>
   public enum FramesType : int
   {
      INIT_TYPE_ST = 0x00,	//Standart Frame 
      INIT_TYPE_EX = 0x01,	//Extended Frame
   }

   /// <summary>
   /// Maximal values for the ID of a CAN Message
   /// </summary>
   public enum MaxIDValues : int
   {
      MAX_STANDARD_ID = 0x7FF,
      MAX_EXTENDED_ID = 0x1FFFFFFF,
   }

   /// <summary>
   /// Kind of CAN Message
   /// </summary>
   [Flags]
   public enum MsgTypes : int
   {
      MSGTYPE_STANDARD = 0x00,		// Standard Frame (11 bit ID)
      MSGTYPE_RTR = 0x01,		// Remote request
      MSGTYPE_EXTENDED = 0x02,		// CAN 2.0 B Frame (29 Bit ID)
      MSGTYPE_STATUS = 0x80,		// Status Message
   }

   /// <summary>
   /// PCAN Hardware enumeration
   /// </summary>
   public enum Hardware : int
   {
      HW_ISA = 1,
      HW_DONGLE_SJA = 5,
      HW_DONGLE_SJA_EPP = 6,
      HW_DONGLE_PRO = 7,
      HW_DONGLE_PRO_EPP = 8,
      HW_ISA_SJA = 9,
      HW_PCI = 10,
   }

   /// <summary>
   /// Hardware type corresponding to the different PCAN Light Dlls
   /// </summary>
   public enum HardwareType : int
   {
      ISA_1CH = 0,		// ISA 1st Channel
      ISA_2CH = 1,		// ISA 2nd Channel
      PCI_1CH = 2,		// PCI 1st Channel
      PCI_2CH = 3,		// PCI 2nd Channel
      PCC_1CH = 4,		// PCC 1st Channel
      PCC_2CH = 5,		// PCC 2nd Channel
      USB_1CH = 6,		// USB 1st Channel
      USB_2CH = 7,		// USB 2nd Channel
      DNP = 8,		// DONGLE PRO
      DNG = 9,		// DONGLE
   }

   /// <summary>
   /// CAN Baudrates
   /// </summary>
   public enum Baudrates : ushort
   {
      BAUD_1M = 0x0014,  //   1 MBit/s
      BAUD_500K = 0x001C,  // 500 kBit/s 
      BAUD_250K = 0x011C,  // 250 kBit/s
      BAUD_125K = 0x031C,  // 125 kBit/s
      BAUD_100K = 0x432F,  // 100 kBit/s
      BAUD_50K = 0x472F,  //  50 kBit/s
      BAUD_20K = 0x532F,  //  20 kBit/s
      BAUD_10K = 0x672F,  //  10 kBit/s
      BAUD_5K = 0x7F7F,  //   5 kBit/s
   }

   /// <summary>
   /// CAN Error and status values
   /// </summary>
   [Flags]
   public enum CANResult : uint
   {
      ERR_OK = 0x0000,
      ERR_XMTFULL = 0x0001,   // Send buffer of the Controller ist full
      ERR_OVERRUN = 0x0002,   // CAN-Controller was read to late 
      ERR_BUSLIGHT = 0x0004,   // Bus error: an Error count reached the limit
      ERR_BUSHEAVY = 0x0008,   // Bus error: an Error count reached the limit
      ERR_BUSOFF = 0x0010,   // Bus error: CAN_Controller went to 'Bus-Off'
      ERR_QRCVEMPTY = 0x0020,   // RcvQueue is empty
      ERR_QOVERRUN = 0x0040,   // RcvQueue was read to late
      ERR_QXMTFULL = 0x0080,   // Send queue is full
      ERR_REGTEST = 0x0100,   // RegisterTest of the 82C200/SJA1000 failed
      ERR_NOVXD = 0x0200,   // Problem with Localization of the VxD
      ERRMASK_ILLHANDLE = 0x1C00,   // Mask for all Handle errors
      ERR_HWINUSE = 0x0400,   // Hardware is occupied by a net
      ERR_NETINUSE = 0x0800,   // The Net is attached to a Client
      ERR_ILLHW = 0x1400,   // Invalid Hardware handle
      ERR_ILLNET = 0x1800,   // Invalid Net handle 
      ERR_ILLCLIENT = 0x1C00,   // Invalid Client handle 
      ERR_RESOURCE = 0x2000,   // Not generatably Resource (FIFO, Client, Timeout)
      ERR_PARMTYP = 0x4000,   // Parameter not permitted
      ERR_PARMVAL = 0x8000,   // Invalid Parameter value
      ERR_ANYBUSERR = ERR_BUSLIGHT | ERR_BUSHEAVY | ERR_BUSOFF, // All others error status <> 0 please ask by PEAK ......intern Driver errors..... 
      ERR_NO_DLL = 0xFFFFFFFF// A Dll could not be loaded or a function was not found into the Dll
   }
   #endregion

   #region Classes definition
   /// <summary>
   /// Class to managing the multiple definition of a TPCANMsg structure 
   /// between the different PCANLIGHT Classes/Dlls
   /// </summary>
   public class TCLightMsg
   {
      #region Properties
      /// <summary>
      /// 11/29-Bit CAN-ID
      /// </summary>
      public uint ID;
      /// <summary>
      /// Kind of Message
      /// </summary>
      public MsgTypes MsgType;
      /// <summary>
      /// Lenght of the Message
      /// </summary>
      public byte Len;
      /// <summary>
      /// Data Bytes (0...7)
      /// </summary>
      public byte[] Data;
      #endregion

      #region Methodes
      #region Constructors, destructor, initialations
      /// <summary>
      /// TCLightMsg standard constructor
      /// </summary>
      public TCLightMsg()
      {
         Data = new byte[8];
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_ISA Class</param>
      public TCLightMsg(PCAN_ISA.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_2ISA Class</param>
      public TCLightMsg(PCAN_2ISA.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_PCI Class</param>
      public TCLightMsg(PCAN_PCI.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_2PCI Class</param>
      public TCLightMsg(PCAN_2PCI.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_PCC Class</param>
      public TCLightMsg(PCAN_PCC.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_2PCC Class</param>
      public TCLightMsg(PCAN_2PCC.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_USB Class</param>
      public TCLightMsg(PCAN_USB.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_2USB Class</param>
      public TCLightMsg(PCAN_2USB.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_DNG Class</param>
      public TCLightMsg(PCAN_DNG.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }

      /// <summary>
      /// TCLightMsg constructor
      /// </summary>
      /// <param name="Msg">A TCANMsg structure defined in the PCAN_DNP Class</param>
      public TCLightMsg(PCAN_DNP.TPCANMsg Msg)
      {
         ID = Msg.ID;
         MsgType = (MsgTypes)Msg.MSGTYPE;
         Len = Msg.LEN;
         Data = Msg.DATA;
      }
      #endregion

      #region Casting functions
      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_ISA class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_ISA Class</returns>
      public static implicit operator PCAN_ISA.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_ISA.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_2ISA class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_2ISA Class</returns>
      public static implicit operator PCAN_2ISA.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_2ISA.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_PCI class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_PCI Class</returns>
      public static implicit operator PCAN_PCI.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_PCI.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_2PCI class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_2PCI Class</returns>
      public static implicit operator PCAN_2PCI.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_2PCI.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_PCC class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_PCC Class</returns>
      public static implicit operator PCAN_PCC.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_PCC.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_2PCC class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_2PCC Class</returns>
      public static implicit operator PCAN_2PCC.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_2PCC.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_USB class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_USB Class</returns>
      public static implicit operator PCAN_USB.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_USB.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_USB class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_USB Class</returns>
      public static implicit operator PCAN_2USB.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_2USB.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_DNG class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_DNG Class</returns>
      public static implicit operator PCAN_DNG.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_DNG.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TCANMsg structure defined in the PCAN_DNP class
      /// </summary>
      /// <param name="Msg">Instance of the TCLightMsg Class to cast</param>
      /// <returns>A corresponding TCANMsg structure, defined in PCAN_DNP Class</returns>
      public static implicit operator PCAN_DNP.TPCANMsg(TCLightMsg Msg)
      {
         PCAN_DNP.TPCANMsg toReturn;

         toReturn.ID = Msg.ID;
         toReturn.LEN = Msg.Len;
         toReturn.MSGTYPE = (byte)Msg.MsgType;
         toReturn.DATA = Msg.Data;

         return toReturn;
      }
      #endregion
      #endregion
   }

   /// <summary>
   /// Class to managing the multiple definition of a TPCANTimestamp structure 
   /// between the different PCANLIGHT Classes/Dlls
   /// </summary>
   public class TCLightTimestamp
   {
      #region Properties
      /// <summary>
      /// Base-value: milliseconds: 0.. 2^32-1
      /// </summary>
      public uint millis;
      /// <summary>
      /// Roll-arounds of milliseconds
      /// </summary>
      public ushort millis_overflow;
      /// <summary>
      /// Microseconds: 0..999
      /// </summary>
      public ushort micros;
      #endregion

      #region Methodes
      #region Constructors, destructor, initializations
      /// <summary>
      ///  TCLightTimestamp standard constructor
      /// </summary>
      public TCLightTimestamp()
      {
         millis = 0;
         micros = 0;
         millis_overflow = 0;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_ISA Class</param>
      public TCLightTimestamp(PCAN_ISA.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_2ISA Class</param>
      public TCLightTimestamp(PCAN_2ISA.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_PCI Class</param>
      public TCLightTimestamp(PCAN_PCI.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_2PCI Class</param>
      public TCLightTimestamp(PCAN_2PCI.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_PCC Class</param>
      public TCLightTimestamp(PCAN_PCC.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_2PCC Class</param>
      public TCLightTimestamp(PCAN_2PCC.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_USB Class</param>
      public TCLightTimestamp(PCAN_USB.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_2USB Class</param>
      public TCLightTimestamp(PCAN_2USB.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_DNG Class</param>
      public TCLightTimestamp(PCAN_DNG.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }

      /// <summary>
      /// TCLightTimestamp constructor
      /// </summary>
      /// <param name="RcvTime">A TPCANTimestamp structure defined in the PCAN_DNP Class</param>
      public TCLightTimestamp(PCAN_DNP.TPCANTimestamp RcvTime)
      {
         millis = RcvTime.millis;
         millis_overflow = RcvTime.millis_overflow;
         micros = RcvTime.micros;
      }
      #endregion

      #region Casting functions
      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_ISA class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_ISA Class</returns>
      public static implicit operator PCAN_ISA.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_ISA.TPCANTimestamp toReturn;

         toReturn = new PCAN_ISA.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_2ISA class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_2ISA Class</returns>
      public static implicit operator PCAN_2ISA.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_2ISA.TPCANTimestamp toReturn;

         toReturn = new PCAN_2ISA.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_PCI class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_PCI Class</returns>
      public static implicit operator PCAN_PCI.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_PCI.TPCANTimestamp toReturn;

         toReturn = new PCAN_PCI.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_2PCI class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_2PCI Class</returns>
      public static implicit operator PCAN_2PCI.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_2PCI.TPCANTimestamp toReturn;

         toReturn = new PCAN_2PCI.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_PCC class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_PCC Class</returns>
      public static implicit operator PCAN_PCC.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_PCC.TPCANTimestamp toReturn;

         toReturn = new PCAN_PCC.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_2PCC class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_2PCC Class</returns>
      public static implicit operator PCAN_2PCC.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_2PCC.TPCANTimestamp toReturn;

         toReturn = new PCAN_2PCC.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_USB class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_USB Class</returns>
      public static implicit operator PCAN_USB.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_USB.TPCANTimestamp toReturn;

         toReturn = new PCAN_USB.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_2USB class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_2USB Class</returns>
      public static implicit operator PCAN_2USB.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_2USB.TPCANTimestamp toReturn;

         toReturn = new PCAN_2USB.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_DNG class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_DNG Class</returns>
      public static implicit operator PCAN_DNG.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_DNG.TPCANTimestamp toReturn;

         toReturn = new PCAN_DNG.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }

      /// <summary>
      /// Overloaded Type Casting to a TPCANTimestamp structure defined in the PCAN_DNP class
      /// </summary>
      /// <param name="RcvTime">Instance of the TCLightTimestamp Class to cast</param>
      /// <returns>A corresponding TPCANTimestamp structure, defined in PCAN_DNP Class</returns>
      public static implicit operator PCAN_DNP.TPCANTimestamp(TCLightTimestamp RcvTime)
      {
         PCAN_DNP.TPCANTimestamp toReturn;

         toReturn = new PCAN_DNP.TPCANTimestamp();
         toReturn.millis = RcvTime.millis;
         toReturn.millis_overflow = RcvTime.millis_overflow;
         toReturn.micros = RcvTime.micros;

         return toReturn;
      }
      #endregion
      #endregion
   }

   /// <summary>
   /// Interfacing class to the PCAN Light Dlls
   /// </summary>
   public class PCANLight
   {
      #region Methodes

      #region PCANLight Functions
      /// <summary>
      /// PCANLight Init function for non Plug and Play Hardware.  
      /// This function make the following:
      ///		- Activate a Hardware
      ///		- Make a Register Test of 82C200/SJA1000
      ///		- Allocate a Send buffer and a Hardware handle
      ///		- Programs the configuration of the transmit/receive driver
      ///		- Set the Baudrate register
      ///		- Set the Controller in RESET condition
      /// </summary>
      /// <param name="HWType">Which hardware should be initialized</param>
      /// <param name="BTR0BTR1">BTR0-BTR1 baudrate register</param>
      /// <param name="MsgType">If the frame type is standard or extended</param>
      /// <param name="IO_Port">Input/output Port Address of the hardware</param>
      /// <param name="Interrupt">Interrupt number</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult Init(HardwareType HWType, Baudrates BTR0BTR1, FramesType MsgType, uint IO_Port, ushort Interrupt)
      {
         try
         {
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  return (CANResult)PCAN_ISA.Init((ushort)BTR0BTR1, (int)MsgType, (int)Hardware.HW_ISA_SJA, IO_Port, Interrupt);

               case HardwareType.ISA_2CH:
                  return (CANResult)PCAN_2ISA.Init((ushort)BTR0BTR1, (int)MsgType, (int)Hardware.HW_ISA_SJA, IO_Port, Interrupt);

               case HardwareType.DNG:
                  return (CANResult)PCAN_DNG.Init((ushort)BTR0BTR1, (int)MsgType, (int)Hardware.HW_DONGLE_SJA, IO_Port, Interrupt);

               case HardwareType.DNP:
                  return (CANResult)PCAN_DNP.Init((ushort)BTR0BTR1, (int)MsgType, (int)Hardware.HW_DONGLE_PRO, IO_Port, Interrupt);

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "ppInit {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight Init function for Plug and Play Hardware.
      /// This function make the following:
      ///		- Activate a Hardware
      ///		- Make a Register Test of 82C200/SJA1000
      ///		- Allocate a Send buffer and a Hardware handle
      ///		- Programs the configuration of the transmit/receive driver
      ///		- Set the Baudrate register
      ///		- Set the Controller in RESET condition
      /// </summary>
      /// <param name="HWType">Which hardware should be initialized</param>
      /// <param name="BTR0BTR1">BTR0-BTR1 baudrate register</param>
      /// <param name="MsgType">f the frame type is standard or extended</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult Init(HardwareType HWType, Baudrates BTR0BTR1, FramesType MsgType)
      {
         try
         {
            switch (HWType)
            {
               case HardwareType.PCI_1CH:
                  return (CANResult)PCAN_PCI.Init((ushort)BTR0BTR1, (int)MsgType);

               case HardwareType.PCI_2CH:
                  return (CANResult)PCAN_2PCI.Init((ushort)BTR0BTR1, (int)MsgType);

               case HardwareType.PCC_1CH:
                  return (CANResult)PCAN_PCC.Init((ushort)BTR0BTR1, (int)MsgType);

               case HardwareType.PCC_2CH:
                  return (CANResult)PCAN_2PCC.Init((ushort)BTR0BTR1, (int)MsgType);

               case HardwareType.USB_1CH:
                  return (CANResult)PCAN_USB.Init((ushort)BTR0BTR1, (int)MsgType);

               case HardwareType.USB_2CH:
                  return (CANResult)PCAN_2USB.Init((ushort)BTR0BTR1, (int)MsgType);

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "Init {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight Close function.
      /// This function terminate and release all resources and the configured hardware:
      /// </summary>
      /// <param name="HWType">Which hardware should be finished</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult Close(HardwareType HWType)
      {
         try
         {
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  return (CANResult)PCAN_ISA.Close();

               case HardwareType.ISA_2CH:
                  return (CANResult)PCAN_2ISA.Close();

               case HardwareType.PCI_1CH:
                  return (CANResult)PCAN_PCI.Close();

               case HardwareType.PCI_2CH:
                  return (CANResult)PCAN_2PCI.Close();

               case HardwareType.PCC_1CH:
                  return (CANResult)PCAN_PCC.Close();

               case HardwareType.PCC_2CH:
                  return (CANResult)PCAN_2PCC.Close();

               case HardwareType.USB_1CH:
                  return (CANResult)PCAN_USB.Close();

               case HardwareType.USB_2CH:
                  return (CANResult)PCAN_2USB.Close();

               case HardwareType.DNP:
                  return (CANResult)PCAN_DNP.Close();

               case HardwareType.DNG:
                  return (CANResult)PCAN_DNG.Close();

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "Close {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight Status Function
      /// This function request the current status of the hardware (b.e. BUS-OFF)
      /// </summary>
      /// <param name="HWType">Which hardware should be asked for it Status</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult Status(HardwareType HWType)
      {
         try
         {
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  return (CANResult)PCAN_ISA.Status();

               case HardwareType.ISA_2CH:
                  return (CANResult)PCAN_2ISA.Status();

               case HardwareType.PCI_1CH:
                  return (CANResult)PCAN_PCI.Status();

               case HardwareType.PCI_2CH:
                  return (CANResult)PCAN_2PCI.Status();

               case HardwareType.PCC_1CH:
                  return (CANResult)PCAN_PCC.Status();

               case HardwareType.PCC_2CH:
                  return (CANResult)PCAN_2PCC.Status();

               case HardwareType.USB_1CH:
                  return (CANResult)PCAN_USB.Status();

               case HardwareType.USB_2CH:
                  return (CANResult)PCAN_2USB.Status();

               case HardwareType.DNP:
                  return (CANResult)PCAN_DNP.Status();

               case HardwareType.DNG:
                  return (CANResult)PCAN_DNG.Status();

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "Status {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight Write function
      /// This function Place a CAN message into the Transmit Queue of the CAN Hardware
      /// </summary>
      /// <param name="HWType">In which hardware should be written the CAN Message</param>3
      /// <param name="MsgToSend">The TCLightMsg message to be written</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult Write(HardwareType HWType, TCLightMsg MsgToSend)
      {
         PCAN_ISA.TPCANMsg MsgIsa;
         PCAN_2ISA.TPCANMsg MsgIsa2;
         PCAN_PCI.TPCANMsg MsgPci;
         PCAN_2PCI.TPCANMsg MsgPci2;
         PCAN_PCC.TPCANMsg MsgPcc;
         PCAN_2PCC.TPCANMsg MsgPcc2;
         PCAN_USB.TPCANMsg MsgUsb;
         PCAN_2USB.TPCANMsg MsgUsb2;
         PCAN_DNP.TPCANMsg MsgDnp;
         PCAN_DNG.TPCANMsg MsgDng;

         try
         {
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  MsgIsa = MsgToSend;
                  return (CANResult)PCAN_ISA.Write(ref MsgIsa);

               case HardwareType.ISA_2CH:
                  MsgIsa2 = MsgToSend;
                  return (CANResult)PCAN_2ISA.Write(ref MsgIsa2);

               case HardwareType.PCI_1CH:
                  MsgPci = MsgToSend;
                  return (CANResult)PCAN_PCI.Write(ref MsgPci);

               case HardwareType.PCI_2CH:
                  MsgPci2 = MsgToSend;
                  return (CANResult)PCAN_2PCI.Write(ref MsgPci2);

               case HardwareType.PCC_1CH:
                  MsgPcc = MsgToSend;
                  return (CANResult)PCAN_PCC.Write(ref MsgPcc);

               case HardwareType.PCC_2CH:
                  MsgPcc2 = MsgToSend;
                  return (CANResult)PCAN_2PCC.Write(ref MsgPcc2);

               case HardwareType.USB_1CH:
                  MsgUsb = MsgToSend;
                  return (CANResult)PCAN_USB.Write(ref MsgUsb);

               case HardwareType.USB_2CH:
                  MsgUsb2 = MsgToSend;
                  return (CANResult)PCAN_2USB.Write(ref MsgUsb2);

               case HardwareType.DNP:
                  MsgDnp = MsgToSend;
                  return (CANResult)PCAN_DNP.Write(ref MsgDnp);

               case HardwareType.DNG:
                  MsgDng = MsgToSend;
                  return (CANResult)PCAN_DNG.Write(ref MsgDng);

               // Hardware is not valid for this function
               //					
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "Write {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight Read function
      /// This function get the next message or the next error from the Receive Queue of 
      /// the CAN Hardware.  
      /// REMARK:
      ///		- Check always the type of the received Message (MSGTYPE_STANDARD,MSGTYPE_RTR,
      ///		  MSGTYPE_EXTENDED,MSGTYPE_STATUS)
      ///		- The function will return ERR_OK always that you receive a CAN message successfully 
      ///		  although if the messages is a MSGTYPE_STATUS message.  
      ///		- When a MSGTYPE_STATUS mesasge is got, the ID and Length information of the message 
      ///		  will be treated as indefined values. Actually information of the received message
      ///		  should be interpreted using the first 4 data bytes as follow:
      ///			*	Data0	Data1	Data2	Data3	Kind of Error
      ///				0x00	0x00	0x00	0x02	CAN_ERR_OVERRUN		0x0002	CAN Controller was read to late
      ///				0x00	0x00	0x00	0x04	CAN_ERR_BUSLIGHT	0x0004  Bus Error: An error counter limit reached (96)
      ///				0x00	0x00	0x00	0x08	CAN_ERR_BUSHEAVY	0x0008	Bus Error: An error counter limit reached (128)
      ///				0x00	0x00	0x00	0x10	CAN_ERR_BUSOFF		0x0010	Bus Error: Can Controller went "Bus-Off"
      ///		- If a CAN_ERR_BUSOFF status message is received, the CAN Controller must to be 
      ///		  initialized again using the Init() function.  Otherwise, will be not possible 
      ///		  to send/receive more messages.
      /// </summary>
      /// <param name="HWType">From which hardware should be read a CAN Message</param>
      /// <param name="Msg">The TCLightMsg structure to store the CAN message</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult Read(HardwareType HWType, out TCLightMsg Msg)
      {
         PCAN_ISA.TPCANMsg MsgIsa;
         PCAN_2ISA.TPCANMsg MsgIsa2;
         PCAN_PCI.TPCANMsg MsgPci;
         PCAN_2PCI.TPCANMsg MsgPci2;
         PCAN_PCC.TPCANMsg MsgPcc;
         PCAN_2PCC.TPCANMsg MsgPcc2;
         PCAN_USB.TPCANMsg MsgUsb;
         PCAN_2USB.TPCANMsg MsgUsb2;
         PCAN_DNP.TPCANMsg MsgDnp;
         PCAN_DNG.TPCANMsg MsgDng;
         CANResult resTemp;

         Msg = null;

         try
         {
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  resTemp = (CANResult)PCAN_ISA.Read(out MsgIsa);
                  Msg = new TCLightMsg(MsgIsa);
                  return resTemp;

               case HardwareType.ISA_2CH:
                  resTemp = (CANResult)PCAN_2ISA.Read(out MsgIsa2);
                  Msg = new TCLightMsg(MsgIsa2);
                  return resTemp;

               case HardwareType.PCI_1CH:
                  MsgPci = new PCAN_PCI.TPCANMsg();
                  MsgPci.DATA = new byte[8];
                  resTemp = (CANResult)PCAN_PCI.Read(out MsgPci);
                  Msg = new TCLightMsg(MsgPci);
                  return resTemp;

               case HardwareType.PCI_2CH:
                  resTemp = (CANResult)PCAN_2PCI.Read(out MsgPci2);
                  Msg = new TCLightMsg(MsgPci2);
                  return resTemp;

               case HardwareType.PCC_1CH:
                  resTemp = (CANResult)PCAN_PCC.Read(out MsgPcc);
                  Msg = new TCLightMsg(MsgPcc);
                  return resTemp;

               case HardwareType.PCC_2CH:
                  resTemp = (CANResult)PCAN_2PCC.Read(out MsgPcc2);
                  Msg = new TCLightMsg(MsgPcc2);
                  return resTemp;

               case HardwareType.USB_1CH:
                  resTemp = (CANResult)PCAN_USB.Read(out MsgUsb);
                  Msg = new TCLightMsg(MsgUsb);
                  return resTemp;

               case HardwareType.USB_2CH:
                  resTemp = (CANResult)PCAN_2USB.Read(out MsgUsb2);
                  Msg = new TCLightMsg(MsgUsb2);
                  return resTemp;


               case HardwareType.DNP:
                  resTemp = (CANResult)PCAN_DNP.Read(out MsgDnp);
                  Msg = new TCLightMsg(MsgDnp);
                  return resTemp;

               case HardwareType.DNG:
                  resTemp = (CANResult)PCAN_DNG.Read(out MsgDng);
                  Msg = new TCLightMsg(MsgDng);
                  return resTemp;

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //			
            Tracer.WriteError(TraceGroup.CANBUS, null, "Read {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight ReadEx function
      /// This function get the next message or the next error from the Receive Queue of 
      /// the CAN Hardware and the time when the message arrived.  
      /// REMARK:
      ///		- Check always the type of the received Message (MSGTYPE_STANDARD,MSGTYPE_RTR,
      ///		  MSGTYPE_EXTENDED,MSGTYPE_STATUS)
      ///		- The function will return ERR_OK always that you receive a CAN message successfully 
      ///		  although if the messages is a MSGTYPE_STATUS message.  
      ///		- When a MSGTYPE_STATUS mesasge is got, the ID and Length information of the message 
      ///		  will be treated as indefined values. Actually information of the received message
      ///		  should be interpreted using the first 4 data bytes as follow:
      ///			*	Data0	Data1	Data2	Data3	Kind of Error
      ///				0x00	0x00	0x00	0x02	CAN_ERR_OVERRUN		0x0002	CAN Controller was read to late
      ///				0x00	0x00	0x00	0x04	CAN_ERR_BUSLIGHT	0x0004  Bus Error: An error counter limit reached (96)
      ///				0x00	0x00	0x00	0x08	CAN_ERR_BUSHEAVY	0x0008	Bus Error: An error counter limit reached (128)
      ///				0x00	0x00	0x00	0x10	CAN_ERR_BUSOFF		0x0010	Bus Error: Can Controller went "Bus-Off"
      ///		- If a CAN_ERR_BUSOFF status message is received, the CAN Controller must to be 
      ///		  initialized again using the Init() function.  Otherwise, will be not possible 
      ///		  to send/receive more messages.
      /// </summary>
      /// <param name="HWType">From which hardware should be read a CAN Message</param>
      /// <param name="Msg">The TCLightMsg structure to store the CAN message</param>
      /// <param name="RcvTime">The TCLightTimestamp structure to store the timestamp of the CAN message</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult ReadEx(HardwareType HWType, out TCLightMsg Msg, out TCLightTimestamp RcvTime)
      {
         PCAN_ISA.TPCANMsg MsgIsa;
         PCAN_2ISA.TPCANMsg MsgIsa2;
         PCAN_PCI.TPCANMsg MsgPci;
         PCAN_2PCI.TPCANMsg MsgPci2;
         PCAN_PCC.TPCANMsg MsgPcc;
         PCAN_2PCC.TPCANMsg MsgPcc2;
         PCAN_USB.TPCANMsg MsgUsb;
         PCAN_2USB.TPCANMsg MsgUsb2;
         PCAN_DNP.TPCANMsg MsgDnp;
         PCAN_DNG.TPCANMsg MsgDng;

         PCAN_ISA.TPCANTimestamp RcvTimeIsa;
         PCAN_2ISA.TPCANTimestamp RcvTimeIsa2;
         PCAN_PCI.TPCANTimestamp RcvTimePci;
         PCAN_2PCI.TPCANTimestamp RcvTimePci2;
         PCAN_PCC.TPCANTimestamp RcvTimePcc;
         PCAN_2PCC.TPCANTimestamp RcvTimePcc2;
         PCAN_USB.TPCANTimestamp RcvTimeUsb;
         PCAN_2USB.TPCANTimestamp RcvTimeUsb2;
         PCAN_DNP.TPCANTimestamp RcvTimeDnp;
         PCAN_DNG.TPCANTimestamp RcvTimeDng;

         CANResult resTemp;

         Msg = null;
         RcvTime = null;

         try
         {
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  resTemp = (CANResult)PCAN_ISA.ReadEx(out MsgIsa, out RcvTimeIsa);
                  Msg = new TCLightMsg(MsgIsa);
                  RcvTime = new TCLightTimestamp(RcvTimeIsa);
                  return resTemp;

               case HardwareType.ISA_2CH:
                  resTemp = (CANResult)PCAN_2ISA.ReadEx(out MsgIsa2, out RcvTimeIsa2);
                  Msg = new TCLightMsg(MsgIsa2);
                  RcvTime = new TCLightTimestamp(RcvTimeIsa2);
                  return resTemp;

               case HardwareType.PCI_1CH:
                  resTemp = (CANResult)PCAN_PCI.ReadEx(out MsgPci, out RcvTimePci);
                  Msg = new TCLightMsg(MsgPci);
                  RcvTime = new TCLightTimestamp(RcvTimePci);
                  return resTemp;

               case HardwareType.PCI_2CH:
                  resTemp = (CANResult)PCAN_2PCI.ReadEx(out MsgPci2, out RcvTimePci2);
                  Msg = new TCLightMsg(MsgPci2);
                  RcvTime = new TCLightTimestamp(RcvTimePci2);
                  return resTemp;

               case HardwareType.PCC_1CH:
                  resTemp = (CANResult)PCAN_PCC.ReadEx(out MsgPcc, out RcvTimePcc);
                  Msg = new TCLightMsg(MsgPcc);
                  RcvTime = new TCLightTimestamp(RcvTimePcc);
                  return resTemp;

               case HardwareType.PCC_2CH:
                  resTemp = (CANResult)PCAN_2PCC.ReadEx(out MsgPcc2, out RcvTimePcc2);
                  Msg = new TCLightMsg(MsgPcc2);
                  RcvTime = new TCLightTimestamp(RcvTimePcc2);
                  return resTemp;

               case HardwareType.USB_1CH:
                  resTemp = (CANResult)PCAN_USB.ReadEx(out MsgUsb, out RcvTimeUsb);
                  Msg = new TCLightMsg(MsgUsb);
                  RcvTime = new TCLightTimestamp(RcvTimeUsb);
                  return resTemp;

               case HardwareType.USB_2CH:
                  resTemp = (CANResult)PCAN_2USB.ReadEx(out MsgUsb2, out RcvTimeUsb2);
                  Msg = new TCLightMsg(MsgUsb2);
                  RcvTime = new TCLightTimestamp(RcvTimeUsb2);
                  return resTemp;

               case HardwareType.DNP:
                  resTemp = (CANResult)PCAN_DNP.ReadEx(out MsgDnp, out RcvTimeDnp);
                  Msg = new TCLightMsg(MsgDnp);
                  RcvTime = new TCLightTimestamp(RcvTimeDnp);
                  return resTemp;

               case HardwareType.DNG:
                  resTemp = (CANResult)PCAN_DNG.ReadEx(out MsgDng, out RcvTimeDng);
                  Msg = new TCLightMsg(MsgDng);
                  RcvTime = new TCLightTimestamp(RcvTimeDng);
                  return resTemp;

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (EntryPointNotFoundException Ex)
         {
            // Function is not available in the loaded Dll
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "ReadEx wrong version {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "ReadEx {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight VersionInfo function
      /// This function get the Version and copyright of the hardware as text 
      /// (max. 255 characters)
      /// </summary>
      /// <param name="HWType">Which hardware should be asked for its Version information</param>
      /// <param name="strInfo">String variable to return the hardware information</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult VersionInfo(HardwareType HWType, out string strInfo)
      {
         StringBuilder stbTemp;
         CANResult resTemp;

         strInfo = "";

         try
         {
            stbTemp = new StringBuilder(256);
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  resTemp = (CANResult)PCAN_ISA.VersionInfo(stbTemp);
                  break;

               case HardwareType.ISA_2CH:
                  resTemp = (CANResult)PCAN_2ISA.VersionInfo(stbTemp);
                  break;

               case HardwareType.PCI_1CH:
                  resTemp = (CANResult)PCAN_PCI.VersionInfo(stbTemp);
                  break;

               case HardwareType.PCI_2CH:
                  resTemp = (CANResult)PCAN_2PCI.VersionInfo(stbTemp);
                  break;

               case HardwareType.PCC_1CH:
                  resTemp = (CANResult)PCAN_PCC.VersionInfo(stbTemp);
                  break;

               case HardwareType.PCC_2CH:
                  resTemp = (CANResult)PCAN_2PCC.VersionInfo(stbTemp);
                  break;

               case HardwareType.USB_1CH:
                  resTemp = (CANResult)PCAN_USB.VersionInfo(stbTemp);
                  break;

               case HardwareType.USB_2CH:
                  resTemp = (CANResult)PCAN_2USB.VersionInfo(stbTemp);
                  break;

               case HardwareType.DNP:
                  resTemp = (CANResult)PCAN_DNP.VersionInfo(stbTemp);
                  break;

               case HardwareType.DNG:
                  resTemp = (CANResult)PCAN_DNG.VersionInfo(stbTemp);
                  break;

               // Hardware is not valid for this function
               //
               default:
                  stbTemp = new StringBuilder("");
                  resTemp = CANResult.ERR_ILLHW;
                  break;
            }
            strInfo = stbTemp.ToString();
            return resTemp;
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "VersionInfo {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight DllVersionInfo function
      /// This function get the Version information of the used PCAN-Light DLL. (max. 255 characters)
      /// </summary>
      /// <param name="HWType">Which DLL (Hardware implementation) should be asked for its 
      /// Version information</param>
      /// <param name="strInfo">String buffer to return the DLL information</param>
      /// <returns>A CANResult value generated after execute the function</returns>
      public static CANResult DllVersionInfo(HardwareType HWType, out string strInfo)
      {
         StringBuilder stbTemp;
         CANResult resTemp;

         strInfo = "";

         try
         {
            stbTemp = new StringBuilder(256);
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  resTemp = (CANResult)PCAN_ISA.DLLVersionInfo(stbTemp);
                  break;

               case HardwareType.ISA_2CH:
                  resTemp = (CANResult)PCAN_2ISA.DLLVersionInfo(stbTemp);
                  break;

               case HardwareType.PCI_1CH:
                  resTemp = (CANResult)PCAN_PCI.DLLVersionInfo(stbTemp);
                  break;

               case HardwareType.PCI_2CH:
                  resTemp = (CANResult)PCAN_2PCI.DLLVersionInfo(stbTemp);
                  break;

               case HardwareType.PCC_1CH:
                  resTemp = (CANResult)PCAN_PCC.DLLVersionInfo(stbTemp);
                  break;

               case HardwareType.PCC_2CH:
                  resTemp = (CANResult)PCAN_2PCC.DLLVersionInfo(stbTemp);
                  break;

               case HardwareType.USB_1CH:
                  resTemp = (CANResult)PCAN_USB.DLLVersionInfo(stbTemp);
                  break;

               case HardwareType.USB_2CH:
                  resTemp = (CANResult)PCAN_2USB.DLLVersionInfo(stbTemp);
                  break;

               case HardwareType.DNP:
                  resTemp = (CANResult)PCAN_DNP.DLLVersionInfo(stbTemp);
                  break;

               case HardwareType.DNG:
                  resTemp = (CANResult)PCAN_DNG.DLLVersionInfo(stbTemp);
                  break;

               // Hardware is not valid for this function
               //
               default:
                  stbTemp = new StringBuilder("");
                  resTemp = CANResult.ERR_ILLHW;
                  break;
            }
            strInfo = stbTemp.ToString();
            return resTemp;
         }
         catch (EntryPointNotFoundException Ex)
         {
            // Function is not available in the loaded Dll
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "DllVersionInfo wrong version {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "DllVersionInfo {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight ResetClient function
      /// This function delete the both queues (Transmit,Receive) of the CAN Controller 
      /// using a RESET
      /// </summary>
      /// <param name="HWType">Hardware to reset</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult ResetClient(HardwareType HWType)
      {
         try
         {
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  return (CANResult)PCAN_ISA.ResetClient();

               case HardwareType.ISA_2CH:
                  return (CANResult)PCAN_2ISA.ResetClient();

               case HardwareType.PCI_1CH:
                  return (CANResult)PCAN_PCI.ResetClient();

               case HardwareType.PCI_2CH:
                  return (CANResult)PCAN_2PCI.ResetClient();

               case HardwareType.PCC_1CH:
                  return (CANResult)PCAN_PCC.ResetClient();

               case HardwareType.PCC_2CH:
                  return (CANResult)PCAN_2PCC.ResetClient();

               case HardwareType.USB_1CH:
                  return (CANResult)PCAN_USB.ResetClient();

               case HardwareType.USB_2CH:
                  return (CANResult)PCAN_2USB.ResetClient();

               case HardwareType.DNP:
                  return (CANResult)PCAN_DNP.ResetClient();

               case HardwareType.DNG:
                  return (CANResult)PCAN_DNG.ResetClient();

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "ResetClient {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLigth MsgFilter function
      /// This function set the receive message filter of the CAN Controller.
      /// REMARK:
      ///		- A quick register of all messages is possible using the parameters From and To as 0
      ///		- Every call of this function maybe cause an extention of the receive filter of the 
      ///		  CAN controller, which one can go briefly to RESET
      ///		- New in Ver 2.x:
      ///			* Standard frames will be put it down in the acc_mask/code as Bits 28..13
      ///			* Hardware driver for 82C200 must to be moved to Bits 10..0 again!
      ///	WARNING: 
      ///		It is not guaranteed to receive ONLY the registered messages.
      /// </summary>
      /// <param name="HWType">Hardware which applay the filter to</param>
      /// <param name="From">First/Start Message ID - It muss be smaller than the "To" parameter</param>
      /// <param name="To">Last/Finish Message ID - It muss be bigger than the "From" parameter</param>
      /// <param name="MsgType">Kind of Frame - Standard or Extended</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult MsgFilter(HardwareType HWType, uint From, uint To, MsgTypes MsgType)
      {
         try
         {
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  return (CANResult)PCAN_ISA.MsgFilter(From, To, (int)MsgType);

               case HardwareType.ISA_2CH:
                  return (CANResult)PCAN_2ISA.MsgFilter(From, To, (int)MsgType);

               case HardwareType.PCI_1CH:
                  return (CANResult)PCAN_PCI.MsgFilter(From, To, (int)MsgType);

               case HardwareType.PCI_2CH:
                  return (CANResult)PCAN_2PCI.MsgFilter(From, To, (int)MsgType);

               case HardwareType.PCC_1CH:
                  return (CANResult)PCAN_PCC.MsgFilter(From, To, (int)MsgType);

               case HardwareType.PCC_2CH:
                  return (CANResult)PCAN_2PCC.MsgFilter(From, To, (int)MsgType);

               case HardwareType.USB_1CH:
                  return (CANResult)PCAN_USB.MsgFilter(From, To, (int)MsgType);

               case HardwareType.USB_2CH:
                  return (CANResult)PCAN_2USB.MsgFilter(From, To, (int)MsgType);

               case HardwareType.DNP:
                  return (CANResult)PCAN_DNP.MsgFilter(From, To, (int)MsgType);

               case HardwareType.DNG:
                  return (CANResult)PCAN_DNG.MsgFilter(From, To, (int)MsgType);

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "MsgFilter {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLigth ResetFilter function
      /// This function close completely the Message Filter of the Hardware.
      /// They will be no more messages received.
      /// </summary>
      /// <param name="HWType">Hardware to reset its filter</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult ResetFilter(HardwareType HWType)
      {
         try
         {
            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  return (CANResult)PCAN_ISA.ResetFilter();

               case HardwareType.ISA_2CH:
                  return (CANResult)PCAN_2ISA.ResetFilter();

               case HardwareType.PCI_1CH:
                  return (CANResult)PCAN_PCI.ResetFilter();

               case HardwareType.PCI_2CH:
                  return (CANResult)PCAN_2PCI.ResetFilter();

               case HardwareType.PCC_1CH:
                  return (CANResult)PCAN_PCC.ResetFilter();

               case HardwareType.PCC_2CH:
                  return (CANResult)PCAN_2PCC.ResetFilter();

               case HardwareType.USB_1CH:
                  return (CANResult)PCAN_USB.ResetFilter();

               case HardwareType.USB_2CH:
                  return (CANResult)PCAN_2USB.ResetFilter();

               case HardwareType.DNP:
                  return (CANResult)PCAN_DNP.ResetFilter();

               case HardwareType.DNG:
                  return (CANResult)PCAN_DNG.ResetFilter();

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "ResetFilter {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight SetUSBDeviceNr function 
      /// This function set an identification number to the USB CAN hardware 
      /// </summary>
      /// <param name="HWType">Hardware to set its Device Number</param>
      /// <param name="DeviceNumber">Value to be set as Device Number</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult SetUSBDeviceNr(HardwareType HWType, uint DeviceNumber)
      {
         try
         {
            switch (HWType)
            {
               case HardwareType.USB_1CH:
                  return (CANResult)PCAN_USB.SetUSBDeviceNr(DeviceNumber);

               case HardwareType.USB_2CH:
                  return (CANResult)PCAN_2USB.SetUSBDeviceNr(DeviceNumber);

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }

         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "SetUSBDeviceNr {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight GetUSBDeviceNr function
      /// This function read the device number of a USB CAN Hardware
      /// </summary>
      /// <param name="HWType">Hardware to get the Device Number</param>
      /// <param name="DeviceNumber">Variable to return the Device Number value</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult GetUSBDeviceNr(HardwareType HWType, out uint DeviceNumber)
      {
         DeviceNumber = uint.MaxValue;

         try
         {
            switch (HWType)
            {
               case HardwareType.USB_1CH:
                  return (CANResult)PCAN_USB.GetUSBDeviceNr(out DeviceNumber);

               case HardwareType.USB_2CH:
                  return (CANResult)PCAN_2USB.GetUSBDeviceNr(out DeviceNumber);

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "GetUSBDeviceNr {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }

      /// <summary>
      /// PCANLight SetRcvEvent function
      /// This function read the device number of a USB CAN Hardware
      /// </summary>
      /// <param name="HWType">Hardware that will set the Event</param>
      /// <param name="EventHandle">The handle (ID) of the event to be set</param>
      /// <returns>A CANResult value - Error/status of the hardware after execute the function</returns>
      public static CANResult SetRcvEvent(HardwareType HWType, System.Threading.EventWaitHandle EventHandle)
      {
         IntPtr hHandle;

         try
         {
            // If the Event parameter is null, a value of IntPtr.Zero is set in order to clear the 
            // Event on the driver. Otherwise we get the internal Handle value representing 
            // the Receive-Event
            //
            hHandle = (EventHandle == null) ? IntPtr.Zero : EventHandle.SafeWaitHandle.DangerousGetHandle();

            switch (HWType)
            {
               case HardwareType.ISA_1CH:
                  return (CANResult)PCAN_ISA.SetRcvEvent(hHandle);

               case HardwareType.ISA_2CH:
                  return (CANResult)PCAN_2ISA.SetRcvEvent(hHandle);

               case HardwareType.PCI_1CH:
                  return (CANResult)PCAN_PCI.SetRcvEvent(hHandle);

               case HardwareType.PCI_2CH:
                  return (CANResult)PCAN_2PCI.SetRcvEvent(hHandle);

               case HardwareType.PCC_1CH:
                  return (CANResult)PCAN_PCC.SetRcvEvent(hHandle);

               case HardwareType.PCC_2CH:
                  return (CANResult)PCAN_2PCC.SetRcvEvent(hHandle);

               case HardwareType.USB_1CH:
                  return (CANResult)PCAN_USB.SetRcvEvent(hHandle);

               case HardwareType.USB_2CH:
                  return (CANResult)PCAN_2USB.SetRcvEvent(hHandle);

               case HardwareType.DNP:
                  return (CANResult)PCAN_DNP.SetRcvEvent(hHandle);

               case HardwareType.DNG:
                  return (CANResult)PCAN_DNG.SetRcvEvent(hHandle);

               // Hardware is not valid for this function
               //
               default:
                  return CANResult.ERR_ILLHW;
            }
         }
         catch (EntryPointNotFoundException Ex)
         {
            // Function is not available in the loaded Dll
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "SetRcvEvent wrong version {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
         catch (Exception Ex)
         {
            // Error: Dll does not exists or the function is not available
            //
            Tracer.WriteError(TraceGroup.CANBUS, null, "SetRcvEvent {0}", Ex.Message + "\"");
            return CANResult.ERR_NO_DLL;
         }
      }
      #endregion
      #endregion

      #region Definition

      public delegate void ReceiveDelegateHandler(CanFrame frame);

      #endregion

      #region Fields

      private class BusContext
      {
         public bool active;
         public HardwareType activeHardware;
         public AutoResetEvent receiveEvent;
         public Thread receiveThread;
         public bool receiveThreadExecute;
         public ReceiveDelegateHandler receiverHandler;
         public TraceGroup traceGroup;

         public BusContext()
         {
            this.active = false;
         }
      }

      private static BusContext[] context = new BusContext[Enum.GetValues(BusInterfaces.USBA.GetType()).Length];

      #endregion

      #region Process Functions

      private static void ReceiveProcess(BusContext busContext)
      {
         for (;busContext.receiveThreadExecute; )
         {
            bool moreMsgs;
            TCLightMsg msg;
            TCLightTimestamp timeStamp;

            busContext.receiveEvent.WaitOne();

            do
            {
               CANResult receiveResult = CANResult.ERR_OK;
               receiveResult = PCANLight.ReadEx(busContext.activeHardware, out msg, out timeStamp);
               moreMsgs = !Convert.ToBoolean(receiveResult & CANResult.ERR_QRCVEMPTY);

               if ((CANResult.ERR_OK == receiveResult) && (null != busContext.receiverHandler))
               {
                  CanFrame frame = new CanFrame((int)msg.ID, msg.Data, (int)msg.Len);
                  busContext.receiverHandler(frame);
               }
            }
            while (false != moreMsgs);
         }
      }
      
      #endregion

      #region Access Functions

      private static HardwareType GetHardwareType(BusInterfaces busInterface)
      {
         HardwareType result = HardwareType.DNG;

         if (BusInterfaces.PCIA == busInterface)
         {
            result = HardwareType.PCI_1CH;
         }
         else if (BusInterfaces.PCIB == busInterface)
         {
            result = HardwareType.PCI_2CH;
         }
         else if (BusInterfaces.USBA == busInterface)
         {
            result = HardwareType.USB_1CH;
         }
         else if (BusInterfaces.USBB == busInterface)
         {
            result = HardwareType.USB_2CH;
         }

         return (result);
      }

      public static Baudrates GetInterfaceBaudRate(int bitRate)
      {
         Baudrates interfaceBaudRate = Baudrates.BAUD_500K;

         if (1000000 == bitRate)
         {
            interfaceBaudRate = Baudrates.BAUD_1M;
         }
         else if (500000 == bitRate)
         {
            interfaceBaudRate = Baudrates.BAUD_500K;
         }
         else if (250000 == bitRate)
         {
            interfaceBaudRate = Baudrates.BAUD_250K;
         }
         else if (125000 == bitRate)
         {
            interfaceBaudRate = Baudrates.BAUD_125K;
         }
         else if (100000 == bitRate)
         {
            interfaceBaudRate = Baudrates.BAUD_100K;
         }
         else if (50000 == bitRate)
         {
            interfaceBaudRate = Baudrates.BAUD_50K;
         }
         else if (20000 == bitRate)
         {
            interfaceBaudRate = Baudrates.BAUD_20K;
         }
         else if (10000 == bitRate)
         {
            interfaceBaudRate = Baudrates.BAUD_10K;
         }

         return (interfaceBaudRate);
      }

      public static CANResult Start(BusInterfaces busInterface, int bitRate, FramesType messageType, TraceGroup traceGroup, ReceiveDelegateHandler receiveHandler, int ioPort = -1, short interrupt = -1)
      {
         int busIndex = (int)busInterface;

         if (null == context[busIndex])
         {
            context[busIndex] = new BusContext();
         }

         HardwareType hardwareType = GetHardwareType(busInterface);
         Baudrates interfaceBaudRate = GetInterfaceBaudRate(bitRate);
			CANResult result = CANResult.ERR_NETINUSE;

         if (false == context[busIndex].active)
         {
            result = CANResult.ERR_OK;
            string dllVersionString = "";
            uint nodeId = 0;

            if (CANResult.ERR_OK == result)
            {
               CANResult dllVersionResult = PCANLight.DllVersionInfo(hardwareType, out dllVersionString);

               if (CANResult.ERR_OK == dllVersionResult)
               {
                  int majorVersion = 0;

                  String[] versionTabInfo = dllVersionString.Split('.');

                  if (versionTabInfo.Length > 0)
                  {
                     Int32.TryParse(versionTabInfo[0].ToString(), out majorVersion);
                  }

                  if (majorVersion < 2)
                  {
                     Tracer.WriteError(traceGroup, "", "DLL version 2.x or higher needed, version \"{0}\" invalid", dllVersionString);
                  }
               }
               else
               {
                  Tracer.WriteError(traceGroup, "", "DLL version error {0}", dllVersionResult.ToString());
                  result = dllVersionResult;
               }
            }

            if (CANResult.ERR_OK == result)
            {
               CANResult initResult = CANResult.ERR_OK;

               if ((-1 != ioPort) && (-1 != interrupt))
               {
                  initResult = PCANLight.Init(hardwareType, interfaceBaudRate, messageType, (uint)ioPort, (ushort)interrupt);
               }
               else
               {
                  initResult = PCANLight.Init(hardwareType, interfaceBaudRate, messageType);
               }

               if (CANResult.ERR_OK != initResult)
               {
                  Tracer.WriteError(traceGroup, "", "DLL init error {0}", initResult.ToString());
                  result = initResult;
               }
            }

            if (CANResult.ERR_OK == result)
            {
               CANResult resetResult = PCANLight.ResetClient(hardwareType);

               if (CANResult.ERR_OK != resetResult)
               {
                  Tracer.WriteError(traceGroup, "", "DLL reset error {0}", resetResult.ToString());
                  result = resetResult;
               }
            }

            if ((HardwareType.USB_1CH == hardwareType) || (HardwareType.USB_2CH == hardwareType))
            {
               if (CANResult.ERR_OK == result)
               {
                  CANResult deviceNumberResult = CANResult.ERR_OK;
                  deviceNumberResult = PCANLight.GetUSBDeviceNr(hardwareType, out nodeId);

                  if (CANResult.ERR_OK != deviceNumberResult)
                  {
                     Tracer.WriteError(traceGroup, "", "DLL device number error {0}", deviceNumberResult.ToString());
                     result = deviceNumberResult;
                  }
               }
            }

            if (CANResult.ERR_OK == result)
            {
               CANResult setEventResult = CANResult.ERR_OK;
               context[busIndex].receiveEvent = new AutoResetEvent(false);
               setEventResult = PCANLight.SetRcvEvent(hardwareType, context[busIndex].receiveEvent);

               if (CANResult.ERR_OK != setEventResult)
               {
                  Tracer.WriteError(traceGroup, "", "DLL receive event error {0}", setEventResult.ToString());
                  result = setEventResult;
               }
            }

            if (CANResult.ERR_OK == result)
            {
               context[busIndex].traceGroup = traceGroup;
               context[busIndex].receiverHandler = new ReceiveDelegateHandler(receiveHandler);
               context[busIndex].receiveThread = new Thread(() => ReceiveProcess(context[busIndex]));
               context[busIndex].receiveThread.IsBackground = true;
               context[busIndex].receiveThread.Name = "CAN " + busInterface.ToString() + " reader";

               context[busIndex].receiveThreadExecute = true;
               context[busIndex].receiveThread.Start();
            }

            if (CANResult.ERR_OK == result)
            {
               context[busIndex].activeHardware = hardwareType;
               context[busIndex].active = true;
               Tracer.WriteHigh(traceGroup, "", "started, version {0}, node id {1}", dllVersionString, nodeId);
            }
         }

         return (result);
      }

      /// <summary>
      /// Function to stop interface.
      /// </summary>
      /// <remarks>
      /// PCAN returns error when another application is monitoring bus.
      /// </remarks>
      /// <param name="busInterface">interface to access</param>
      public static void Stop(BusInterfaces busInterface)
      {
         int busIndex = (int)busInterface;

         if (null == context[busIndex])
         {
            context[busIndex] = new BusContext();
         }

         if (null != context[busIndex].receiveThread)
         {
            context[busIndex].receiveThreadExecute = false;
            context[busIndex].receiveEvent.Set();
            context[busIndex].receiveThread.Join(3000);
            context[busIndex].receiveThread = null;
         }

         if (null != context[busIndex].receiveEvent)
         {
            context[busIndex].receiveEvent.Close();
            context[busIndex].receiveEvent = null;
         }

         if (false != context[busIndex].active)
         {
            CANResult closeResult = CANResult.ERR_OK;
            closeResult = PCANLight.Close(context[busIndex].activeHardware);

            if (CANResult.ERR_OK == closeResult)
            {
               Tracer.WriteHigh(context[busIndex].traceGroup, "", "stopped");
            }
            else
            {
               Tracer.WriteError(context[busIndex].traceGroup, "", "stop error {0}", closeResult.ToString());
            }

            context[busIndex].active = false;
         }
      }

      public static CANResult Send(BusInterfaces busInterface, int id, byte[] data)
      {
         CANResult result = CANResult.ERR_BUSOFF;

         int busIndex = (int)busInterface;

         if (null == context[busIndex])
         {
            context[busIndex] = new BusContext();
         }

         if (false != context[busIndex].active)
         {
            TCLightMsg msg;

            msg = new TCLightMsg();

            msg.ID = (uint)id;
            msg.Len = (byte)data.Length;
            msg.MsgType = MsgTypes.MSGTYPE_STANDARD;

            for (int i = 0; i < data.Length; i++)
            {
               msg.Data[i] = data[i];
            }

            result = PCANLight.Write(context[busIndex].activeHardware, msg);

            if (CANResult.ERR_OK != result)
            {
               StringBuilder sb = new StringBuilder();

               for (int i = 0; i < data.Length; i++)
               {
                  sb.AppendFormat("{0:X2}", data[i]);
               }

               Tracer.WriteError(context[busIndex].traceGroup, "", "send failure {0} data {1}", result, sb.ToString());
            }
         }

         return (result);
      }

      // on BT failure reset bus
      public static CANResult ResetBus(BusInterfaces busInterface)
      {
         CANResult result = CANResult.ERR_BUSOFF;

         int busIndex = (int)busInterface;

         if (null == context[busIndex])
         {
            context[busIndex] = new BusContext();
         }

         if (false != context[busIndex].active)
         {
            TCLightMsg msg;

            msg = new TCLightMsg();

            msg.ID = (uint)0x000;
            msg.Len = (byte)2;
            msg.MsgType = MsgTypes.MSGTYPE_STANDARD;

            msg.Data[0] = (byte)0x81;
            msg.Data[1] = (byte)0x00;

            result = PCANLight.Write(context[busIndex].activeHardware, msg);

            if (CANResult.ERR_OK == result)
            {
               Tracer.WriteError(context[busIndex].traceGroup, "", "bus reset");
            }
            else
            {
               Tracer.WriteError(context[busIndex].traceGroup, "", "reset bus send failure {0}", result.ToString());
            }
         }

         return (result);
      }

      public static CANResult SendSync(BusInterfaces busInterface)
      {
         CANResult result = CANResult.ERR_BUSOFF;

         int busIndex = (int)busInterface;

         if (null == context[busIndex])
         {
            context[busIndex] = new BusContext();
         } 
         
         if (false != context[busIndex].active)
         {
            TCLightMsg msg;

            msg = new TCLightMsg();

            msg.ID = (uint)0x080;
            msg.Len = (byte)0;
            msg.MsgType = MsgTypes.MSGTYPE_STANDARD;

            result = PCANLight.Write(context[busIndex].activeHardware, msg);

            if (CANResult.ERR_OK != result)
            {
               Tracer.WriteError(context[busIndex].traceGroup, "", "sync send failure {0}", result.ToString());
            }
            else
            {
               Tracer.WriteError(context[busIndex].traceGroup, "", "sync");
            }
         }

         return (result);
      }

      public static CANResult SendTimestamp(BusInterfaces busInterface, UInt32 timeStamp)
      {
         CANResult result = CANResult.ERR_BUSOFF;

         int busIndex = (int)busInterface;

         if (null == context[busIndex])
         {
            context[busIndex] = new BusContext();
         } 
         
         if (false != context[busIndex].active)
         {
            TCLightMsg msg;

            msg = new TCLightMsg();

            msg.ID = (uint)0x100;
            msg.Len = (byte)4;
            msg.MsgType = MsgTypes.MSGTYPE_STANDARD;

            msg.Data[0] = (byte)((timeStamp >> 0) & 0xFF);
            msg.Data[1] = (byte)((timeStamp >> 8) & 0xFF);
            msg.Data[2] = (byte)((timeStamp >> 16) & 0xFF);
            msg.Data[3] = (byte)((timeStamp >> 24) & 0xFF);

            result = PCANLight.Write(context[busIndex].activeHardware, msg);

            if (CANResult.ERR_OK != result)
            {
               Tracer.WriteError(context[busIndex].traceGroup, "", "timestamp send failure {0}", timeStamp);
            }
            else
            {
               Tracer.WriteError(context[busIndex].traceGroup, "", "timestamp");
            }
         }

         return (result);
      }

      #endregion

   }
   #endregion
}