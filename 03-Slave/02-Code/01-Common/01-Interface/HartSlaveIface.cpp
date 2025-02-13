/*
 *          File: HartSlaveIface.cpp (CHartSlave)
 *                This is where the actual interface of the slave implementation
 *                is located, which would also have to be integrated into an
 *                embedded system. The version with the DLL is only intended for
 *                testing under Windows.
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

#include "WbHartSlave.h"
#include "HartSlaveIface.h"
#include "HartChannel.h"
#include "AnyCommandIntp.h"
#include "HartDevice.h"

// Channel handling
EN_Bool CHartSlave::OpenChannel(TY_Word port_number_, EN_CommType type_)
{
    EN_Bool result;

    COSAL::Lock();
    result = CChannel::Open(port_number_, type_);
    COSAL::Unlock();
    return result;
}

void CHartSlave::CloseChannel()
{
    COSAL::Lock();
    CChannel::Close();
    COSAL::Unlock();
}

// Configuration
void CHartSlave::GetConstDataHart(TY_ConstDataHart* const_data_hart_)
{
    COSAL::Lock();

    // Copy data
    *const_data_hart_ = CHartData::CConst;

    COSAL::Unlock();
}

void CHartSlave::SetConstDataHart(TY_ConstDataHart* const_data_hart_)
{
    COSAL::Lock();

    // Copy data
    CHartData::CConst = *const_data_hart_;

    // Long address in static always overwritten
    CHartData::CStat.LongAddress[0] = (TY_Byte)((CHartData::CConst.ExpandedDeviceType & 0x3f00) >> 8);
    CHartData::CStat.LongAddress[1] = (TY_Byte)(CHartData::CConst.ExpandedDeviceType & 0xff);
    CHartData::CStat.LongAddress[2] = CHartData::CConst.DevUniqueID[0];
    CHartData::CStat.LongAddress[3] = CHartData::CConst.DevUniqueID[1];
    CHartData::CStat.LongAddress[4] = CHartData::CConst.DevUniqueID[2];

    COSAL::Unlock();
}

void CHartSlave::GetDynDataHart(TY_DynDataHart* dyn_data_hart_)
{
    COSAL::Lock();

    // Copy data
    *dyn_data_hart_ = CHartData::CDyn;

    COSAL::Unlock();
}

void CHartSlave::SetDynDataHart(TY_DynDataHart* dyn_data_hart_)
{
    COSAL::Lock();

    // Copy data
    CHartData::CDyn = *dyn_data_hart_;

    COSAL::Unlock();
}

void CHartSlave::GetStatDataHart(TY_StatDataHart* stat_data_hart_)
{
    COSAL::Lock();

    // Copy data
    *stat_data_hart_ = CHartData::CStat;

    COSAL::Unlock();
}

void CHartSlave::SetStatDataHart(TY_StatDataHart* stat_data_hart_)
{
    COSAL::Lock();

    if (CHartData::CStat.BurstMode != stat_data_hart_->BurstMode)
    {
        CChannel::BurstModeChanged = EN_Bool::TRUE8;
    }

    if (CHartData::CStat.HartEnabled != stat_data_hart_->HartEnabled)
    {
        CChannel::HartEnabledChanged = EN_Bool::TRUE8;
    }

    // Copy data
    CHartData::CStat = *stat_data_hart_;

    // Long address always overwritten by hart const settings
    CHartData::CStat.LongAddress[0] = (TY_Byte)((CHartData::CConst.ExpandedDeviceType & 0x3f00) >> 8);
    CHartData::CStat.LongAddress[1] = (TY_Byte)(CHartData::CConst.ExpandedDeviceType & 0xff);
    CHartData::CStat.LongAddress[2] = CHartData::CConst.DevUniqueID[0];
    CHartData::CStat.LongAddress[3] = CHartData::CConst.DevUniqueID[1];
    CHartData::CStat.LongAddress[4] = CHartData::CConst.DevUniqueID[2];

    COSAL::Unlock();
}

// Test Information
TY_Word CHartSlave::GetHartIpStatus()
{
    return CChannel::GetHartIpStatus();
}

// Service handling
EN_Bool CHartSlave::WasCommandReceived()
{
    SRV_Handle result;

    COSAL::Lock();

    result = CChannel::WasCommandReceived();

    COSAL::Unlock();

    if (result == INVALID_SRV_HANDLE)
    {
        return EN_Bool::FALSE8;
    }

    return EN_Bool::TRUE8;
}

TY_Word CHartSlave::ExecuteCommandInterpreter()
{
    // This should run in another context
    // Because this is the level of the application
    // That's why lock and unlock are not used
    CDevice::UpdateTimeStamp();
    return CAnyCommandIntp::Execute();
}


// Encoding
void CHartSlave::PutInt8(TY_Byte data_, TY_Byte offset_,
    TY_Byte* data_ref_)
{
    data_ref_[offset_] = data_;
}

void CHartSlave::PutInt16(TY_Word data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutWord(data_, offset_, data_ref_, endian_);
}

void CHartSlave::PutInt24(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutInt24(data_, offset_, data_ref_, endian_);
}

void CHartSlave::PutInt32(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutDWord(data_, offset_, data_ref_, endian_);
}

void CHartSlave::PutInt64(TY_UInt64 data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutInt64(data_, offset_, data_ref_, endian_);
}

void CHartSlave::PutFloat(TY_Float data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutFloat(data_, offset_, data_ref_, endian_);
}

void CHartSlave::PutDFloat(TY_DFloat data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutDFloat(data_, offset_, data_ref_, endian_);
}

void CHartSlave::PutPackedASCII(
    TY_Byte* asc_string_ref_, TY_Byte asc_string_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PutPackedASCII(asc_string_ref_, asc_string_len_, offset_, data_ref_);
}

void CHartSlave::PutOctets(
    TY_Byte* stream_ref_, TY_Byte stream_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PutOctets(stream_ref_, stream_len_, offset_, data_ref_);
}

void CHartSlave::PutString(TY_Byte* string_ref_, TY_Byte string_max_len_, TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PutString(string_ref_, string_max_len_, offset_, data_ref_);
}

// Decoding
TY_Byte CHartSlave::PickInt8(TY_Byte offset_, TY_Byte* data_ref_)
{
    return data_ref_[offset_];
}

TY_Word CHartSlave::PickInt16(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickWord(offset_, data_ref_, endian_);
}

TY_DWord CHartSlave::PickInt24(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickInt24(offset_, data_ref_, endian_);
}

TY_DWord CHartSlave::PickInt32(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickDWord(offset_, data_ref_, endian_);
}

TY_UInt64 CHartSlave::PickInt64(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickInt64(offset_, data_ref_, endian_);
}

TY_Float CHartSlave::PickFloat(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickFloat(offset_, data_ref_, endian_);
}

TY_DFloat CHartSlave::PickDFloat(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickDouble(offset_, data_ref_, endian_);
}

void CHartSlave::PickPackedASCII(TY_Byte* string_ref_, TY_Byte string_max_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PickPackedASCII(string_ref_, string_max_len_, offset_, data_ref_);
}

void CHartSlave::PickOctets(TY_Byte* stream_ref_, TY_Byte stream_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PickOctets(stream_ref_, stream_len_, offset_, data_ref_);
}

void CHartSlave::PickString(TY_Byte* string_ref_, TY_Byte string_max_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PickString(string_ref_, string_max_len_, offset_, data_ref_);
}
