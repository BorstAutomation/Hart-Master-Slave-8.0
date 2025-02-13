/*
 *          File: BaHartMaster.h
 *                The implementation for the calls to the Windows DLL is
 *                located here. In practice, it is just a shell through
 *                which the functions in the CUartMaster module are called.
 *                See also HartM_UartIface.cpp/h.
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

#ifndef __bahartmaster76_h__
#define __bahartmaster76_h__

#include <Windows.h>
#include "WbHart_Typedefs.h"
#include "WbHartM_Structures.h"
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
HARTDLL_API EN_Bool WINAPI BAHAMA_OpenChannel(TY_Word com_port_);
HARTDLL_API void WINAPI BAHAMA_CloseChannel();

// Configuration
HARTDLL_API void WINAPI BAHAMA_GetConfiguration(TY_Configuration* config_);
HARTDLL_API void WINAPI BAHAMA_SetConfiguration(TY_Configuration* config_);

// Hart Ip Test Information
HARTDLL_API TY_Word WINAPI BAHAMA_GetHartIpStatus();

// Connection
HARTDLL_API SRV_Handle WINAPI BAHAMA_ConnectByAddr(
    TY_Byte address_,
    EN_Wait qos_,
    TY_Byte numRetries_);
HARTDLL_API SRV_Handle WINAPI BAHAMA_ConnectByUniqueID(TY_Byte* data_ref_,
    TY_Byte qos_,
    TY_Byte num_retries_);
HARTDLL_API SRV_Handle WINAPI BAHAMA_ConnectByShortTag(TY_Byte* data_ref_,
    TY_Byte qos,
    TY_Byte numRetries);
HARTDLL_API SRV_Handle WINAPI BAHAMA_ConnectByLongTag(TY_Byte* data_ref_,
    TY_Byte qos_,
    TY_Byte num_retries_);
HARTDLL_API void WINAPI BAHAMA_FetchConnection(SRV_Handle service_,
    TY_Connection* conn_data_);

// Commands
HARTDLL_API SRV_Handle WINAPI BAHAMA_DoCommand(
    TY_Byte command_,
    EN_Wait qos_,
    TY_Byte* data_ref_,
    TY_Byte dataLen,
    TY_Byte* bytesUniqueId);

HARTDLL_API WRD_Handle WINAPI BAHAMA_DoExtCmd(
    TY_Word command,
    EN_Wait qos,
    TY_Byte* data_ref_,
    TY_Byte data_len_,
    TY_Byte* bytes_of_unique_id_);

// Service handling
HARTDLL_API EN_Bool WINAPI BAHAMA_IsServiceCompleted(SRV_Handle service_);

HARTDLL_API void WINAPI BAHAMA_FetchConfirmation(
    SRV_Handle service_,
    TY_Confirmation* conf_data_);

// Encoding
HARTDLL_API void WINAPI BAHA_PutByte(TY_Byte data_, TY_Byte  offset_,
    TY_Byte* data_ref_);
HARTDLL_API void WINAPI BAHAMA_PutInt16(TY_Word data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API void WINAPI BAHAMA_PutInt24(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_);
HARTDLL_API void WINAPI BAHA_PutDWord(TY_DWord data_, TY_Byte offset_,
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
HARTDLL_API TY_Word WINAPI BAHA_PickWord(TY_Byte offset,
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
HARTDLL_API void    WINAPI BAHAMA_InitMonitor();
HARTDLL_API void    WINAPI BAHAMA_TerminateMonitor();
HARTDLL_API void    WINAPI BAHAMA_StartMonitor();
HARTDLL_API void    WINAPI BAHAMA_StopMonitor();
HARTDLL_API EN_Bool WINAPI BAHAMA_GetMonitorData(TY_MonFrame* mon_frame);
HARTDLL_API EN_Bit  WINAPI BAHAMA_GetMonitorStatus(void);
HARTDLL_API TY_Word WINAPI BAHAMA_GetMonitorAddData(TY_Byte* data_);

#endif // __bahartmaster76_h__