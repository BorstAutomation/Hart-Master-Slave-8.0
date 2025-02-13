/*
 *          File: BaHartSlave.h
 *                The implementation for the calls to the Windows DLL is
 *                located here. In practice, it is just a shell through
 *                which the functions in the CHartSlave module are called.
 *                See also HartSlaveIface.cpp/h.
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

#ifndef __bahartslave_h__
#define __bahartslave_h__

#include <Windows.h>
#include "WbHart_Typedefs.h"
#include "WbHartS_Structures.h"
#include "Monitor.h"

// API declarations

#ifdef DLL_EXPORTS
#define HARTDLL_API extern "C" __declspec(dllexport)
#else
#define HARTDLL_API extern "C" __declspec(dllimport)
#endif

// Type Definitions
#ifndef DWORD
#define DWORD TY_DWord
#endif

#ifndef WINAPI
#define WINAPI __stdcall
#endif  

// Global Functions

// Operation

// Channel handling
HARTDLL_API EN_Bool WINAPI BAHASL_OpenChannel(TY_Word com_port_);
HARTDLL_API void WINAPI BAHASL_CloseChannel();

// Configuration
HARTDLL_API void WINAPI BAHASL_GetConstDataHart(TY_ConstDataHart* const_data_hart_);
HARTDLL_API void WINAPI BAHASL_SetConstDataHart(TY_ConstDataHart* const_data_hart_);
HARTDLL_API void WINAPI BAHASL_GetDynDataHart(TY_DynDataHart* dyn_data_hart_);
HARTDLL_API void WINAPI BAHASL_SetDynDataHart(TY_DynDataHart* dyn_data_hart_);
HARTDLL_API void WINAPI BAHASL_GetStatDataHart(TY_StatDataHart* stat_data_hart_);
HARTDLL_API void WINAPI BAHASL_SetStatDataHart(TY_StatDataHart* stat_data_hart_);

// Hart Ip Test Information
HARTDLL_API TY_Word WINAPI BAHASL_GetHartIpStatus();

// Service handling
HARTDLL_API EN_Bool WINAPI BAHASL_WasCommandReceived();
HARTDLL_API TY_Word WINAPI BAHASL_ExecuteCommandInterpreter();

// Encoding
HARTDLL_API void WINAPI BAHA_PutByte(TY_Byte data_, TY_Byte  offset_,
    TY_Byte* data_ref_);
HARTDLL_API void WINAPI BAHASL_PutWord(TY_Word data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API void WINAPI BAHA_PutInt24(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API void WINAPI BAHA_PutWord(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API void WINAPI BAHA_PutInt64(TY_UInt64 data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API void WINAPI BAHA_PutFloat(TY_Float data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API void WINAPI BAHA_PutDouble(TY_DFloat data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API void WINAPI BAHA_PutPackedASCII(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_);
HARTDLL_API void WINAPI BAHA_PutOctets(TY_Byte* stream_ref_,
    TY_Byte stream_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_);
HARTDLL_API void WINAPI BAHA_PutString(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_);

// Decoding
HARTDLL_API TY_Byte WINAPI BAHA_PickByte(TY_Byte offset,
    TY_Byte* data_ref_);
HARTDLL_API TY_Word WINAPI BAHASL_PickInt16(TY_Byte offset,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API TY_DWord WINAPI BAHA_PickInt24(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API TY_DWord WINAPI BAHA_PickDWord(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API TY_UInt64 WINAPI BAHA_PickInt64(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API TY_Float WINAPI BAHA_PickFloat(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API TY_DFloat WINAPI BAHA_PickDouble(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API void WINAPI BAHA_PickPackedASCII(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_);
HARTDLL_API void WINAPI BAHA_PickOctets(TY_Byte* stream_ref_,
    TY_Byte stream_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_);
HARTDLL_API void WINAPI BAHA_PickString(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_);

// Monitor Interface
HARTDLL_API void    WINAPI BAHASL_InitMonitor();
HARTDLL_API void    WINAPI BAHASL_TerminateMonitor();
HARTDLL_API void    WINAPI BAHASL_StartMonitor();
HARTDLL_API void    WINAPI BAHASL_StopMonitor();
HARTDLL_API EN_Bool WINAPI BAHASL_GetMonitorData(TY_MonFrame* mon_frame);
HARTDLL_API EN_Bit  WINAPI BAHASL_GetMonitorStatus(void);
HARTDLL_API TY_Word WINAPI BAHASL_GetMonitorAddData(TY_Byte* data_);

#endif // __bahartslave_h__