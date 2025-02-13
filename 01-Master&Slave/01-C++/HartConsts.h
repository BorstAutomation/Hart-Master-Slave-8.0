/*
 *          File: HartLib.h (CHart)
 *                Some classes for the definition of HART constants.
 *
 *        Author: Walter Borst
 *
 *        E-Mail: info@borst-automation.de
 *          Home: https://www.borst-automation.de
 *
 * No Warranties: https://www.borst-automation.com/legal/warranty-disclaimer
 *
 * Copyright 2006-2025 Walter Borst, Cuxhaven, Germany
 */

#ifndef __hartconsts_h__
#define __hartconsts_h__

#include "OSAL.h"
#include "WbHartUser.h"

class CHart
{
public:
    class CDelimiter
    {
    public:
        static const TY_Byte BACK = 1;
        static const TY_Byte STX = 2;
        static const TY_Byte ACK = 6;
    };

    class CLimit
    {
    public:
        static const TY_Byte MAX_NUM_PREAMBLES = 20;
    };

    class CSize
    {
    public:
        static const TY_Byte NUM_ADDR_BYTES = 5;
        static const TY_Byte ADD_STAT_LEN = 25;
    };

    class CMask
    {
    public:
        static const TY_Byte     DELIMITER = 0x07;
        static const TY_Byte     ADDR_TYPE = 0x80;
        static const TY_Byte     HIGH_ADDR = 0x3f;
        static const TY_Byte NUM_EXP_BYTES = 0x60;
    };

    class CFlags
    {
    public:
        static const TY_Byte UNIQUE_ADDR = 0x80;
        static const TY_Byte PRIM_MASTER = 0x80;
        static const TY_Byte EXT_CMD_FLAG = 31;
    };

    class CAddrMode
    {
    public:
        static const TY_Byte POLLING = 0;
        static const TY_Byte UNIQUE = 1;
    };

    class CDevStatus
    {
    public:
        static const TY_Byte  MALFUNCTION = 0x80;
        static const TY_Byte  CFG_CHANGED = 0x40;
        static const TY_Byte   COLD_START = 0x20;
        static const TY_Byte  MORE_STATUS = 0x10;
        static const TY_Byte   LOOP_FIXED = 0x08;
        static const TY_Byte     LOOP_SAT = 0x04;
        static const TY_Byte NON_PRIM_LIM = 0x02;
        static const TY_Byte     PRIM_LIM = 0x01;
    };

    class CRespCode
    {
    public:
        static const TY_Byte      SUCCESS =  0;
        static const TY_Byte  INVALID_SEL =  2;
        static const TY_Byte TOO_FEW_DATA =  5;
        static const TY_Byte      CMD_ERR =  6;
        static const TY_Byte   WRITE_PROT =  7;
        static const TY_Byte ACCESS_RESTR = 16;
        static const TY_Byte         BUSY = 32;
        static const TY_Byte  CMD_NOT_IMP = 64;
    };
};
#endif // __hartconsts_h__