/*
 *          File: BaHartSlave.cpp
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

#include <Windows.h>

#include "WbHartSlave.h"
#include "BaHartSlave.h"
#include "HartSlaveIface.h"

// Main entry point (Windows standard)
BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

// Channel handling

HARTDLL_API EN_Bool WINAPI BAHASL_OpenChannel(TY_Word port_number_)
{
    if (port_number_ == 0)
    {
        CHartSlave::CloseChannel();
        return EN_Bool::FALSE8;
    }
    else if (port_number_ == 1)
    {
        return CHartSlave::OpenChannel(port_number_, EN_CommType::HART_IP);
    }
    else
    {
        return CHartSlave::OpenChannel(port_number_, EN_CommType::UART);
    }
}

HARTDLL_API void WINAPI BAHASL_CloseChannel()
{
    CHartSlave::CloseChannel();
}

// Configuration

HARTDLL_API void WINAPI BAHASL_GetConstDataHart(TY_ConstDataHart* const_data_hart_)
{
    CHartSlave::GetConstDataHart(const_data_hart_);
}

HARTDLL_API void WINAPI BAHASL_SetConstDataHart(TY_ConstDataHart* const_data_hart_)
{
    CHartSlave::SetConstDataHart(const_data_hart_);
}

HARTDLL_API void WINAPI BAHASL_GetDynDataHart(TY_DynDataHart* dyn_data_hart_)
{
    CHartSlave::GetDynDataHart(dyn_data_hart_);
}

HARTDLL_API void WINAPI BAHASL_SetDynDataHart(TY_DynDataHart* dyn_data_hart_)
{
    CHartSlave::SetDynDataHart(dyn_data_hart_);
}

HARTDLL_API void WINAPI BAHASL_GetStatDataHart(TY_StatDataHart* stat_data_hart_)
{
    CHartSlave::GetStatDataHart(stat_data_hart_);
}

HARTDLL_API void WINAPI BAHASL_SetStatDataHart(TY_StatDataHart* stat_data_hart_)
{
    CHartSlave::SetStatDataHart(stat_data_hart_);
}

// Hart Ip Test Information
HARTDLL_API TY_Word WINAPI BAHASL_GetHartIpStatus()
{
    return CHartSlave::GetHartIpStatus();
}

// Service handling
HARTDLL_API EN_Bool WINAPI BAHASL_WasCommandReceived()
{
    return CHartSlave::WasCommandReceived();
}

HARTDLL_API TY_Word WINAPI BAHASL_ExecuteCommandInterpreter()
{
    return CHartSlave::ExecuteCommandInterpreter();
}

// Encoding
HARTDLL_API void WINAPI BAHA_PutByte(TY_Byte data_, TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartSlave::PutInt8(data_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHASL_PutWord(TY_Word data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    CHartSlave::PutInt16(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutInt24(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    CHartSlave::PutInt24(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutWord(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    CHartSlave::PutInt32(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutInt64(TY_UInt64 data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian  endian_)
{
    CHartSlave::PutInt64(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutFloat(TY_Float data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian  endian_)
{
    CHartSlave::PutFloat(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutDouble(TY_DFloat data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian  endian_)
{
    CHartSlave::PutDFloat(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutPackedASCII(TY_Byte* sb_,
    TY_Byte       len_,
    TY_Byte    offset_,
    TY_Byte* data_ref_)
{
    CHartSlave::PutPackedASCII(sb_, len_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHA_PutOctets(TY_Byte* stream_ref_,
    TY_Byte stream_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartSlave::PutOctets(stream_ref_, stream_len_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHA_PutString(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartSlave::PutString(string_ref_, string_max_len_, offset_, data_ref_);
}


HARTDLL_API TY_Byte WINAPI BAHA_PickByte(TY_Byte offset_,
    TY_Byte* data_ref_)
{
    return CHartSlave::PickInt8(offset_, data_ref_);
}

HARTDLL_API TY_Word WINAPI BAHA_PickWord(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartSlave::PickInt16(offset_, data_ref_, endian_);
}

HARTDLL_API TY_DWord WINAPI BAHA_PickInt24(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartSlave::PickInt24(offset_, data_ref_, endian_);
}

HARTDLL_API TY_DWord WINAPI BAHA_PickDWord(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartSlave::PickInt32(offset_, data_ref_, endian_);
}

HARTDLL_API TY_UInt64 WINAPI BAHA_PickInt64(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartSlave::PickInt64(offset_, data_ref_, endian_);
}

HARTDLL_API TY_Float WINAPI BAHA_PickFloat(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartSlave::PickFloat(offset_, data_ref_, endian_);
}

HARTDLL_API TY_DFloat WINAPI BAHA_PickDouble(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartSlave::PickDFloat(offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PickPackedASCII(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartSlave::PickPackedASCII(string_ref_, string_max_len_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHA_PickOctets(TY_Byte* stream_ref_,
    TY_Byte stream_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartSlave::PickOctets(stream_ref_, stream_len_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHA_PickString(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartSlave::PickString(string_ref_, string_max_len_, offset_, data_ref_);
}
