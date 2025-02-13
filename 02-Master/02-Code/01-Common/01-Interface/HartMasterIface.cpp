/*
 *          File: HartMasterIface.cpp (CHartMaster)
 *                This is where the actual interface of the master implementation
 *                is located, which would also have to be integrated into an
 *                embedded system. The version with the DLL is only intended for
 *                testing under Windows.
 *                You can find a detailed description of the realized functions
 *                in the PDF documentation of this software package.
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

#include "HartMasterIface.h"
#include "HartCoding.h"

// Channel handling
EN_Bool CHartMaster::OpenChannel(TY_Word port_number_, EN_CommType type_)
{
    EN_Bool result;

    COSAL::Lock();
    result = CChannel::Open(port_number_, type_);
    COSAL::Unlock();
    return result;
}

void CHartMaster::CloseChannel()
{
    COSAL::Lock();
    CChannel::Close();
    COSAL::Unlock();
}

// Configuration
void CHartMaster::GetConfiguration(TY_Configuration* config_)
{
    COSAL::Lock();

    config_->BaudRate = CChannel::GetBaudrate();
    config_->NumPreambles = CChannel::GetNumPreambles();
    config_->NumRetries = CChannel::GetNumRetries();
    config_->RetryIfBusy = CChannel::GetRetryIfBusy();
    config_->MasterType = CChannel::GetMasterType();
    config_->AddressingMode = CChannel::GetAddressingMode();
    CChannel::FetchHartIpHostName(config_->HartIpHostName);
    CChannel::FetchHartIpAddress(config_->HartIpAddress);
    CChannel::FetchHartIpPort(config_->HartIpPort);
    config_->HartIpUseAddress = CChannel::GetHartIpUseAddress();
    config_->HartIpSendKeepAlive = CChannel::HartIpSendKeepAlive;
    COSAL::Unlock();
}

void CHartMaster::SetConfiguration(TY_Configuration* config_)
{
    COSAL::Lock();

    CChannel::SetNumPreambles(config_->NumPreambles);
    CChannel::SetNumRetries(config_->NumRetries);
    CChannel::SetRetryIfBusy(config_->RetryIfBusy);
    CChannel::SetMasterType(config_->MasterType);
    CChannel::SetAddressingMode(config_->AddressingMode);
    CChannel::SetBaudrate(config_->BaudRate);
    CChannel::SetHartIpHostName(config_->HartIpHostName);
    CChannel::SetHartIpAddress(config_->HartIpAddress);
    CChannel::SetHartIpPort(config_->HartIpPort);
    CChannel::SetHartIpUseAddress(config_->HartIpUseAddress);
    CChannel::HartIpSendKeepAlive = config_->HartIpSendKeepAlive;

    COSAL::Unlock();
}

// Information
TY_Word CHartMaster::GetHartIpStatus()
{
    return CChannel::GetHartIpStatus();
}


// Connection
SRV_Handle CHartMaster::ConnectByAddr(TY_Byte address_, EN_Wait qos_, TY_Byte num_retries_)
{
    SRV_Handle h_service = INVALID_SRV_HANDLE;
    CService* p_service = NULL;
    EN_Bool    is_valid_service = EN_Bool::FALSE8;

    COSAL::Lock();

    if (CChannel::IsOpen() == EN_Bool::FALSE8)
    {
        COSAL::Unlock();
        return INVALID_SRV_HANDLE;
    }

    h_service = CChannel::GetNewService();
    if (h_service != INVALID_SRV_HANDLE)
    {
        p_service = CChannel::GetServicePtr(h_service);
        if (p_service != NULL)
        {
            is_valid_service = EN_Bool::TRUE8;
            p_service->SetCommand(0);
            p_service->SetAddrMode(CHart::CAddrMode::POLLING);
            p_service->SetShortAddr(address_);
            p_service->SetData(NULL, 0);
            p_service->SetNumRetries(num_retries_);
            p_service->SetNumPreambles(CChannel::GetNumPreambles());
            p_service->SetRetryIfBusy(CChannel::GetRetryIfBusy());
            p_service->SetHandle(h_service);
            p_service->Launch();
            // Pass the access of the service to the protocol kernel
            CChannel::SetServiceOwner(h_service, EN_Owner::PROTOCOL);
        }
    }

    COSAL::Unlock();

    /// Debug
    TY_Word wait_count = 500;
    /// End debug

    if ((qos_ == EN_Wait::WAIT) && (is_valid_service == EN_Bool::TRUE8))
    {
        /* Wait for service completion */
        while (CChannel::IsServiceCompleted(h_service) == EN_Bool::FALSE8)
        {
            COSAL::Wait(10);

            /// Debug
            if (wait_count-- == 0)
            {
                break;
            }
            /// End debug
        }
    }

    return h_service;
}

void CHartMaster::FetchConnection(SRV_Handle service_, TY_Connection* connection_)
{
    CService* srv = NULL;

    COSAL::Lock();

    if (CChannel::IsValidService(service_) == EN_Bool::TRUE8)
    {
        TY_Byte len;

        srv = CChannel::GetServicePtr(service_);
        if (srv->IsInProgress() == EN_Bool::TRUE8)
        {
            connection_->SrvResultCode = (TY_Byte)EN_SRV_Result::IN_PROGRESS;
            COSAL::Unlock();
            return;
        }
        if (srv->Failed() == EN_Bool::TRUE8)
        {
            connection_->SrvResultCode = (TY_Byte)EN_SRV_Result::NO_DEV_RESP;
            connection_->UsedRetries = srv->GetUsedRetries();
            CChannel::FreeService(service_);
            COSAL::Unlock();
            return;
        }
        len = srv->GetRespLen();
        if (len < 12)
        {
            connection_->SrvResultCode = (TY_Byte)EN_SRV_Result::NO_DEV_RESP;
            connection_->UsedRetries = srv->GetUsedRetries();
            CChannel::FreeService(service_);
            COSAL::Unlock();
            return;
        }

        connection_->SrvResultCode = (TY_Byte)EN_SRV_Result::SUCCESSFUL;
        connection_->RespCode1 = srv->GetRespCode1();
        connection_->RespCode2 = srv->GetRespCode2();
        connection_->ManuId = srv->GetRespDataByte(1);
        connection_->DevId = srv->GetRespDataByte(2);
        connection_->NumPreambles = srv->GetRespDataByte(3);
        connection_->CmdRevNum = srv->GetRespDataByte(4);
        connection_->SpecRevCode = srv->GetRespDataByte(5);
        connection_->SwRev = srv->GetRespDataByte(6);
        connection_->HwRev = srv->GetRespDataByte(7);
        connection_->HartFlags = srv->GetRespDataByte(8);
        connection_->BytesOfUniqueID[0] = srv->GetRespDataByte(1);
        connection_->BytesOfUniqueID[1] = srv->GetRespDataByte(2);
        connection_->BytesOfUniqueID[2] = srv->GetRespDataByte(9);
        connection_->BytesOfUniqueID[3] = srv->GetRespDataByte(10);
        connection_->BytesOfUniqueID[4] = srv->GetRespDataByte(11);
        connection_->DeviceInBurstMode = (TY_Byte)srv->GetDeviceInBurstMode();
        connection_->UsedRetries = srv->GetUsedRetries();
        if (len >= 17)
        {
            connection_->MinNumPreambs = srv->GetRespDataByte(12);
            connection_->MaxNumDVs = srv->GetRespDataByte(13);
            connection_->CfgChCount = (TY_Word)((srv->GetRespDataByte(14) << 8) + srv->GetRespDataByte(15));
            connection_->ExtDevStatus = srv->GetRespDataByte(16);
        }
        else
        {
            connection_->MinNumPreambs = 0;
            connection_->MaxNumDVs = 0;
            connection_->CfgChCount = 0;
            connection_->ExtDevStatus = 0;
        }
        if (len >= 22)
        {
            connection_->ExtManuID = (TY_Word)((srv->GetRespDataByte(17) << 8) + srv->GetRespDataByte(18));
            connection_->ExtLabDistID = (TY_Word)((srv->GetRespDataByte(19) << 8) + srv->GetRespDataByte(20));
            connection_->EDevProfile = srv->GetRespDataByte(21);
        }
        else
        {
            connection_->ExtManuID = 0;
            connection_->ExtLabDistID = 0;
            connection_->EDevProfile = 0;
        }

        CChannel::FreeService(service_);
    }
    else
    {
        connection_->SrvResultCode = (TY_Byte)EN_SRV_Result::EMPTY;
    }

    COSAL::Unlock();
}

// Commands
SRV_Handle CHartMaster::LaunchCommand(TY_Byte command_,
    EN_Wait qos_,
    TY_Byte* data_ref_,
    TY_Byte data_len_,
    TY_Byte* bytes_of_unique_id_)
{
    CService*  p_service = NULL;
    SRV_Handle h_service = INVALID_SRV_HANDLE;
    EN_Bool    is_valid_service = EN_Bool::FALSE8;

    COSAL::Lock();

    if (CChannel::IsOpen() == EN_Bool::FALSE8)
    {
        COSAL::Unlock();
        return INVALID_SRV_HANDLE;
    }

    h_service = CChannel::GetNewService();
    if (h_service != INVALID_SRV_HANDLE)
    {
        p_service = CChannel::GetServicePtr(h_service);
        if (p_service != NULL)
        {
            is_valid_service = EN_Bool::TRUE8;
            p_service->SetCommand(command_);
            if (CChannel::GetAddressingMode() == 2)
            {
                p_service->SetAddrMode(CHart::CAddrMode::POLLING);
            }
            else
            {
                p_service->SetAddrMode(CHart::CAddrMode::UNIQUE);
            }
            p_service->SetUniqueID(bytes_of_unique_id_);
            p_service->SetData(data_ref_, data_len_);
            p_service->SetRetryIfBusy(CChannel::GetRetryIfBusy());
            p_service->SetNumPreambles(CChannel::GetNumPreambles());
            p_service->SetNumRetries(CChannel::GetNumRetries());
            p_service->SetMode(CService::EN_Mode::NORMAL);
            p_service->SetHandle(h_service);
            p_service->Launch();
            // Pass the access of the service to the protocol kernel
            CChannel::SetServiceOwner(h_service, EN_Owner::PROTOCOL);
        }
        else
        {
            CChannel::FreeService(h_service);
            h_service = INVALID_SRV_HANDLE;
        }
    }

    COSAL::Unlock();

    /// Debug
    TY_Word wait_count = 500;
    /// End debug

    if ((qos_ == EN_Wait::WAIT) && (is_valid_service == EN_Bool::TRUE8))
    {
        /* Wait for service completion */
        while (CChannel::IsServiceCompleted(h_service) == EN_Bool::FALSE8)
        {
            COSAL::Wait(10);

            /// Debug
            if (wait_count-- == 0)
            {
                break;
            }
            /// End debug
        }
    }

    return h_service;
}

SRV_Handle CHartMaster::LaunchExtCommand(
    TY_Word  command_,
    EN_Wait  qos_,
    TY_Byte* data_ref_,
    TY_Byte  data_len_,
    TY_Byte* bytes_of_unique_id_)
{
    TY_Byte data_bytes[256];

    if (data_len_ > 254)
    {
        return INVALID_SRV_HANDLE;
    }

    // Pack the 16 bit command at the beginning of the request data 
    PutInt16(command_, 0, data_bytes, EN_Endian::MSB_First);
    if (data_len_ > 0)
    {
        // Only if payload exists
        PutOctets(data_ref_, data_len_, 0, &data_bytes[2]);
    }

    return LaunchCommand(31, qos_, data_bytes, (TY_Byte)(data_len_ + 2), bytes_of_unique_id_);
}


// Service handling
void CHartMaster::FetchConfirmation(SRV_Handle service_,
    TY_Confirmation* conf_data_)
{
    CService* p_service = NULL;

    COSAL::Lock();

    if (CChannel::IsValidService(service_) == EN_Bool::TRUE8)
    {
        p_service = CChannel::GetServicePtr(service_);
        if (p_service->IsInProgress() == EN_Bool::TRUE8)
        {
            conf_data_->SrvResultCode = EN_SRV_Result::IN_PROGRESS;
            conf_data_->DataLen = 0;
            COSAL::Unlock();
            return;
        }
        
        if (p_service->Failed() == EN_Bool::TRUE8)
        {
            conf_data_->SrvResultCode = EN_SRV_Result::NO_DEV_RESP;
            conf_data_->UsedRetries = p_service->GetUsedRetries();
            conf_data_->DataLen = 0;
            CChannel::FreeService(service_);
            COSAL::Unlock();
            return;
        }

        conf_data_->SrvResultCode = EN_SRV_Result::SUCCESSFUL;
        conf_data_->RespCode1 = p_service->GetRespCode1();
        conf_data_->RespCode2 = p_service->GetRespCode2();
        conf_data_->Cmd = p_service->GetRespCmd();
        conf_data_->DeviceInBurstMode = p_service->GetDeviceInBurstMode();
        conf_data_->UsedRetries = p_service->GetUsedRetries();
        conf_data_->SrvDuration = p_service->GetDuration();
        conf_data_->DataLen = p_service->GetRespData(conf_data_->BytesOfData);
        if (p_service->GetRespCmd() == 31)
        {
            // Handle extended command
            if (conf_data_->RespCode1 != 64)
            {
                if (conf_data_->DataLen < 2)
                {
                    // Extended command format error
                    conf_data_->SrvResultCode = EN_SRV_Result::TOO_FEW_DATA_BYTES;
                    conf_data_->ExtCommand = 0xffff;
                }
                else
                {
                    TY_Byte u8_DataLen = (TY_Byte)(conf_data_->DataLen - 2);

                    // Extended command: correct data and data length
                    conf_data_->ExtCommand = CCoding::PickWord(0, conf_data_->BytesOfData, EN_Endian::MSB_First);
                    if (u8_DataLen > 0)
                    {
                        // Pull the data two octets foreward
                        COSAL::CMem::Copy(conf_data_->BytesOfData, &conf_data_->BytesOfData[2], u8_DataLen);
                    }

                    conf_data_->DataLen = u8_DataLen;
                }
            }
        }
        
        CChannel::FreeService(service_);
    }
    else
    {
        conf_data_->SrvResultCode = EN_SRV_Result::EMPTY;
        conf_data_->DataLen = 0;
    }

    COSAL::Unlock();
}

EN_Bool CHartMaster::IsServiceCompleted(SRV_Handle service_)
{
    return CChannel::IsServiceCompleted(service_);
}


// Encoding
void CHartMaster::PutInt8(TY_Byte data_, TY_Byte offset_,
    TY_Byte* data_ref_)
{
    data_ref_[offset_] = data_;
}

void CHartMaster::PutInt16(TY_Word data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutWord(data_, offset_, data_ref_, endian_);
}

void CHartMaster::PutInt24(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutInt24(data_, offset_, data_ref_, endian_);
}

void CHartMaster::PutInt32(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutDWord(data_, offset_, data_ref_, endian_);
}

void CHartMaster::PutInt64(TY_UInt64 data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutInt64(data_, offset_, data_ref_, endian_);
}

void CHartMaster::PutFloat(TY_Float data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutFloat(data_, offset_, data_ref_, endian_);
}

void CHartMaster::PutDFloat(TY_DFloat data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    CCoding::PutDFloat(data_, offset_, data_ref_, endian_);
}

void CHartMaster::PutPackedASCII(
    TY_Byte* asc_string_ref_, TY_Byte asc_string_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PutPackedASCII(asc_string_ref_, asc_string_len_, offset_, data_ref_);
}

void CHartMaster::PutOctets(
    TY_Byte* stream_ref_, TY_Byte stream_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PutOctets(stream_ref_, stream_len_, offset_, data_ref_);
}

void CHartMaster::PutString(TY_Byte* string_ref_, TY_Byte string_max_len_, TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PutString(string_ref_, string_max_len_, offset_, data_ref_);
}

// Decoding
TY_Byte CHartMaster::PickInt8(TY_Byte offset_, TY_Byte* data_ref_)
{
    return data_ref_[offset_];
}

TY_Word CHartMaster::PickInt16(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickWord(offset_, data_ref_, endian_);
}

TY_DWord CHartMaster::PickInt24(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickInt24(offset_, data_ref_, endian_);
}

TY_DWord CHartMaster::PickInt32(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickDWord(offset_, data_ref_, endian_);
}

TY_UInt64 CHartMaster::PickInt64(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickInt64(offset_, data_ref_, endian_);
}

TY_Float CHartMaster::PickFloat(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickFloat(offset_, data_ref_, endian_);
}

TY_DFloat CHartMaster::PickDFloat(TY_Byte offset_, TY_Byte* data_ref_,
    EN_Endian endian_)
{
    return CCoding::PickDouble(offset_, data_ref_, endian_);
}

void CHartMaster::PickPackedASCII(TY_Byte* string_ref_, TY_Byte string_max_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PickPackedASCII(string_ref_, string_max_len_, offset_, data_ref_);
}

void CHartMaster::PickOctets(TY_Byte* stream_ref_, TY_Byte stream_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PickOctets(stream_ref_, stream_len_, offset_, data_ref_);
}

void CHartMaster::PickString(TY_Byte* string_ref_, TY_Byte string_max_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    CCoding::PickString(string_ref_, string_max_len_, offset_, data_ref_);
}
