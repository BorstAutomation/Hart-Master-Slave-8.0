/*
 *          File: BaHartMaster.cpp
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

#include "BaHartMaster.h"
#include "HartMasterIface.h"

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

HARTDLL_API EN_Bool WINAPI BAHAMA_OpenChannel(TY_Word port_number_)
{
    if (port_number_ == 0)
    {
        CHartMaster::CloseChannel();
        return EN_Bool::FALSE8;
    }
    else if (port_number_ == 1)
    {
        return CHartMaster::OpenChannel(port_number_, EN_CommType::HART_IP);
    }
    else
    {
        return CHartMaster::OpenChannel(port_number_, EN_CommType::UART);
    }
}

HARTDLL_API void WINAPI BAHAMA_CloseChannel()
{
    CHartMaster::CloseChannel();
}

// Configuration

HARTDLL_API void WINAPI BAHAMA_GetConfiguration(TY_Configuration* config_)
{
    CHartMaster::GetConfiguration(config_);
}

HARTDLL_API void WINAPI BAHAMA_SetConfiguration(TY_Configuration* config_)
{
    CHartMaster::SetConfiguration(config_);
}

// Hart Ip Test Information
HARTDLL_API TY_Word WINAPI BAHAMA_GetHartIpStatus()
{
    return CHartMaster::GetHartIpStatus();
}

// Connection

HARTDLL_API SRV_Handle WINAPI BAHAMA_ConnectByAddr(
    TY_Byte address_,
    EN_Wait qos_,
    TY_Byte num_retries_)
{
    return CHartMaster::ConnectByAddr(address_, qos_, num_retries_);
}

HARTDLL_API void WINAPI BAHAMA_FetchConnection(SRV_Handle service_, TY_Connection* connection_)
{
    CHartMaster::FetchConnection(service_, connection_);
}

// Commands

HARTDLL_API SRV_Handle WINAPI BAHAMA_DoCommand(
    TY_Byte  command_,
    EN_Wait  qos_,
    TY_Byte* data_ref_,
    TY_Byte  data_len_,
    TY_Byte* bytes_of_unique_id_)
{
    return CHartMaster::LaunchCommand(command_, qos_, data_ref_, data_len_, bytes_of_unique_id_);
}

HARTDLL_API WRD_Handle WINAPI BAHAMA_DoExtCmd(
    TY_Word  command_,
    EN_Wait  qos_,
    TY_Byte* data_ref_,
    TY_Byte  data_len_,
    TY_Byte* bytes_of_unique_id_)
{
    return CHartMaster::LaunchExtCommand(command_, qos_, data_ref_, data_len_, bytes_of_unique_id_);
}

// Service handling

HARTDLL_API EN_Bool WINAPI BAHAMA_IsServiceCompleted(SRV_Handle service_)
{
    return CHartMaster::IsServiceCompleted(service_);
}

HARTDLL_API void WINAPI BAHAMA_FetchConfirmation(SRV_Handle service_,
    TY_Confirmation* conf_data_)
{
    CHartMaster::FetchConfirmation(service_, conf_data_);
}

// Encoding
HARTDLL_API void WINAPI BAHA_PutByte(TY_Byte data_, TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartMaster::PutInt8(data_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHA_PutWord(TY_Word data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    CHartMaster::PutInt16(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutInt24(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    CHartMaster::PutInt24(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutDWord(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    CHartMaster::PutInt32(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutInt64(TY_UInt64 data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian  endian_)
{
    CHartMaster::PutInt64(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutFloat(TY_Float data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian  endian_)
{
    CHartMaster::PutFloat(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutDouble(TY_DFloat data_, TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian  endian_)
{
    CHartMaster::PutDFloat(data_, offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PutPackedASCII(TY_Byte* sb_,
    TY_Byte       len_,
    TY_Byte    offset_,
    TY_Byte* data_ref_)
{
    CHartMaster::PutPackedASCII(sb_, len_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHA_PutOctets(TY_Byte* stream_ref_,
    TY_Byte stream_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartMaster::PutOctets(stream_ref_, stream_len_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHA_PutString(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartMaster::PutString(string_ref_, string_max_len_, offset_, data_ref_);
}


HARTDLL_API TY_Byte WINAPI BAHA_PickByte(TY_Byte offset_,
    TY_Byte* data_ref_)
{
    return CHartMaster::PickInt8(offset_, data_ref_);
}

HARTDLL_API TY_Word WINAPI BAHA_PickWord(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartMaster::PickInt16(offset_, data_ref_, endian_);
}

HARTDLL_API TY_DWord WINAPI BAHA_PickInt24(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartMaster::PickInt24(offset_, data_ref_, endian_);
}

HARTDLL_API TY_DWord WINAPI BAHA_PickDWord(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartMaster::PickInt32(offset_, data_ref_, endian_);
}

HARTDLL_API TY_UInt64 WINAPI BAHA_PickInt64(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartMaster::PickInt64(offset_, data_ref_, endian_);
}

HARTDLL_API TY_Float WINAPI BAHA_PickFloat(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartMaster::PickFloat(offset_, data_ref_, endian_);
}

HARTDLL_API TY_DFloat WINAPI BAHA_PickDouble(TY_Byte offset_,
    TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CHartMaster::PickDFloat(offset_, data_ref_, endian_);
}

HARTDLL_API void WINAPI BAHA_PickPackedASCII(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartMaster::PickPackedASCII(string_ref_, string_max_len_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHA_PickOctets(TY_Byte* stream_ref_,
    TY_Byte stream_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartMaster::PickOctets(stream_ref_, stream_len_, offset_, data_ref_);
}

HARTDLL_API void WINAPI BAHA_PickString(TY_Byte* string_ref_,
    TY_Byte string_max_len_,
    TY_Byte offset_,
    TY_Byte* data_ref_)
{
    CHartMaster::PickString(string_ref_, string_max_len_, offset_, data_ref_);
}
