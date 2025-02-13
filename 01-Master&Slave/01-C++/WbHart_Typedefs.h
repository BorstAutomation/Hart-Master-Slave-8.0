/*
 *          File: WbHart_Typedefs.h
 *                This file contains type definitions which are used
 *                in all modules in the Hart master kernel.
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

// Once
#ifndef __wbhart_typedefs_h__
#define __wbhart_typedefs_h__

// Type definitions
typedef unsigned char      TY_Byte;
typedef unsigned short     TY_Word;
typedef unsigned int       TY_DWord;
typedef int                TY_Int32;
typedef unsigned long long TY_UInt64;
typedef float              TY_Float;
typedef double             TY_DFloat;
typedef TY_Word            WRD_Handle;
typedef TY_Word            SRV_Handle;
typedef char               TY_Char;
typedef TY_Word            TY_Len;
typedef unsigned long      TY_ULong;
typedef void*              PTR_Handle;

// Enum classes 
enum class EN_SRV_Result : TY_Byte {
    EMPTY = 0,
    NO_DEV_RESP = 1,
    COMM_ERR = 2,
    INVALID_HANDLE = 3,
    IN_PROGRESS = 4,
    SUCCESSFUL = 5,
    RESOURCE_ERROR = 6,
    TOO_FEW_DATA_BYTES = 7,
    OBSOLETE = 8
};

enum class EN_Endian : TY_Byte
{
    MSB_First = 0, // Big endian (Hart standard)
    LSB_First = 1  // Little endian
};

enum class EN_Bool : TY_Byte
{
    FALSE8 = 0,
    TRUE8 = 1
};

enum class EN_Bit : TY_Byte
{
    CLEAR8 = 0,
    SET8 = 1
};

enum class EN_Error : TY_Byte
{
    NONE = 0,
    ERR = 1
};

enum class EN_Wait : TY_Byte
{
    NO_WAIT = 0,
    WAIT = 1
};

enum class EN_CommType : TY_Byte
{
    NONE = 0,
    UART = 1,
    HART_IP = 2
};

enum class EN_Master : TY_Byte
{
    PRIMARY = 1,
    SECONDARY = 0
};

enum class EN_AdMode : TY_Byte
{
    SHORT = 0,
    LONG = 1
};

enum class EN_DevVarClass : TY_Byte
{
    // Parts of Hart table 21
    NONE = 0,
    TEMPERATURE = 64,
    PRESSURE = 65,
    VOLUMEFLOW = 66,
    VELOCITY = 67,
    VOLUME = 68,
    LENGTH = 69,
    MASS = 71,
    MASS_FLOW = 72,
    DENSITY = 73,
    CURRENT = 84
};

enum class EN_DevVarCode : TY_Byte
{
    // See table 34
    PV1_0 = 0,
    PV2_1 = 1,
    PV3_2 = 2,
    PV4_3 = 3,
    PERCENT = 244,
    CURRENT = 245,
    PV1 = 246,
    PV2 = 247,
    PV3 = 248,
    PV4 = 249,
    NOT_USED = 250
};

enum class EN_ExtDevStatusBits : TY_Byte
{
    // Parts of Hart table 21
    NONE = 0,
    MAINTENANCE = 0x01,
    DEV_VAR_ALERT = 0x02,
    POWER_FAILURE = 0x04,
    FAILURE = 0x08,
    OUT_OF_SPEC = 0x10,
    FUNCTION_CHECK = 0x20
};

// Constants
static const TY_Word INVALID_WRD_HANDLE = (TY_Word)0xFFFF;
static const TY_Word INVALID_SRV_HANDLE = (TY_Word)0xFFFF;

#endif // __wbhart_typedefs_h__