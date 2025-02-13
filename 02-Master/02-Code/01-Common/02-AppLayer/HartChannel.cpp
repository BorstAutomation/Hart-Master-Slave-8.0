/*
 *          File: HartChannel.cpp (CChannel)
 *                The channel manages a communication interface and the
 *                associated propperties. The channel also uses services
 *                to conduct Hart commands.
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

#include "OSAL.h"
#include "WbHartUser.h"
#include "HartFrame.h"
#include "HartService.h"
#include "HartChannel.h"
#include "HMuartMacPort.h"
#include "HMipMacPort.h"

// Data
EN_Bool     CChannel::m_is_open = EN_Bool::FALSE8;
EN_CommType CChannel::m_comm_type = EN_CommType::UART;
TY_Word     CChannel::m_port_number = 0;
TY_Word     CChannel::m_num_active_services = 0;
CService    CChannel::m_CService_pool[MAX_NUM_SERVICES];
TY_DWord    CChannel::m_baudrate = 1200;
TY_Byte     CChannel::m_num_preambles = 5;
TY_Byte     CChannel::m_num_retries = 2;
EN_Bool     CChannel::m_retry_if_busy = EN_Bool::TRUE8;
EN_Master   CChannel::m_master_type = EN_Master::SECONDARY;
TY_Byte     CChannel::m_addressing_mode = 0;
EN_Bool     CChannel::m_hart_enabled = EN_Bool::TRUE8;
TY_Byte     CChannel::m_hart_version = 7;
TY_Word     CChannel::m_next_srv_search_idx = 0;
TY_Byte     CChannel::HartIpHostName[MAX_STRING_LEN] = { 0 };
TY_Byte     CChannel::HartIpAddress[MAX_STRING_LEN] = { 0 };
TY_Byte     CChannel::HartIpPort[MAX_STRING_LEN] = { 0 };
EN_Bool     CChannel::HartIpUseAddress = EN_Bool::FALSE8;
EN_Bool     CChannel::HartIpSendKeepAlive = EN_Bool::FALSE8;

// Methods
EN_Bool CChannel::Open(TY_Word port_number_, EN_CommType type_)
{
    // Close channel if already open by any port
    if (m_is_open == EN_Bool::TRUE8)
    {
        Close();
        m_is_open = EN_Bool::FALSE8;
    }

    m_comm_type = type_;
    if (type_ == EN_CommType::UART)
    {
        Init();
        m_port_number = port_number_ - 1;
        m_comm_type = type_;
        if (CHMuartMacPort::Open(m_port_number, m_baudrate, m_comm_type) == EN_Bool::TRUE8)
        {
            m_is_open = EN_Bool::TRUE8;
            return EN_Bool::TRUE8;
        }
    }
    else if (type_ == EN_CommType::HART_IP)
    {
        Init();
        m_port_number = port_number_ - 1;
        m_comm_type = type_;
        if (CHMipMacPort::Open(HartIpHostName, HartIpPort, m_comm_type) == EN_Bool::TRUE8)
        {
            m_is_open = EN_Bool::TRUE8;
            return EN_Bool::TRUE8;
        }
    }

    return EN_Bool::FALSE8;
}

void CChannel::Close()
{
    if (m_is_open == EN_Bool::TRUE8)
    {
        if (m_comm_type == EN_CommType::UART)
        {
            CHMuartMacPort::Close();
            m_is_open = EN_Bool::FALSE8;
        }
        else if (m_comm_type == EN_CommType::HART_IP)
        {
            CHMipMacPort::Close();
            m_is_open = EN_Bool::FALSE8;
        }
    }
}

EN_Bool CChannel::IsOpen()
{
    return m_is_open;
}

void CChannel::Init()
{
    if (m_comm_type == EN_CommType::HART_IP)
    {
        CHMipMacPort::Init();
    }
    else
    {
        CHMuartMacPort::Init();
    }
}


/* Service handling */
SRV_Handle CChannel::GetNewService()
{
    SRV_Handle handle = INVALID_SRV_HANDLE;

    if (m_num_active_services < MAX_NUM_SERVICES)
    {
        for (TY_Word e = 0; e < MAX_NUM_SERVICES; e++)
        {
            if (m_CService_pool[e].IsActive() == EN_Bool::FALSE8)
            {
                handle = e;
                m_CService_pool[e].Init();
                m_num_active_services++;
                break;
            }
        }
    }

    return handle;
}

CService* CChannel::GetServicePtr(SRV_Handle handle_)
{
    if (handle_ != INVALID_SRV_HANDLE)
    {
        return &(m_CService_pool[handle_]);
    }

    return NULL;
}

void CChannel::ReleaseService(SRV_Handle handle_)
{
    if (IsValidService(handle_) == EN_Bool::TRUE8)
    {
        m_CService_pool[handle_].SetOwner(EN_Owner::USER);
        m_CService_pool[handle_].Release();
        m_num_active_services--;
    }
}

EN_Bool CChannel::IsValidService(SRV_Handle handle_)
{
    EN_Bool result = EN_Bool::FALSE8;

    if (handle_ < MAX_NUM_SERVICES)
    {
        if (GetServicePtr(handle_)->IsActive() == EN_Bool::TRUE8)
        {
            result = EN_Bool::TRUE8;
        }
    }

    return result;
}

EN_Bool CChannel::IsServiceCompleted(SRV_Handle handle_)
{
    if (IsValidService(handle_) == EN_Bool::TRUE8)
    {
        if (m_CService_pool[handle_].GetOwner() == EN_Owner::USER)
        {
            if (m_CService_pool[handle_].GetStatus() == CService::EN_Status::WAITING)
            {
                return EN_Bool::TRUE8;
            }
        }
    }

    return EN_Bool::FALSE8;
}

void CChannel::SetServiceOwner(SRV_Handle handle_, EN_Owner owner_)
{
    if (handle_ < MAX_NUM_SERVICES)
    {
        m_CService_pool[handle_].SetOwner(owner_);
    }
}

EN_Owner CChannel::GetServiceOwner(SRV_Handle handle_)
{
    if (handle_ < MAX_NUM_SERVICES)
    {
        return m_CService_pool[handle_].GetOwner();
    }

    return EN_Owner::USER;
}

void CChannel::FreeService(SRV_Handle handle_)
{
    ReleaseService(handle_);
}

SRV_Handle CChannel::GetRequestedService()
{
    TY_Word   e;
    SRV_Handle idx = m_next_srv_search_idx;

    for (e = 0; e < MAX_NUM_SERVICES; e++)
    {
        if (idx >= MAX_NUM_SERVICES)
        {
            idx = 0;
        }

        if (m_CService_pool[idx].GetOwner() == EN_Owner::PROTOCOL)
        {
            if (m_CService_pool[idx].GetStatus() == CService::EN_Status::REQUESTED)
            {
                m_CService_pool[idx].SetStatus(CService::EN_Status::BUSY);
                m_next_srv_search_idx = idx;
                return idx;
            }
        }

        idx++;
    }

    return INVALID_SRV_HANDLE;
}

/* Get Configuration */
TY_DWord CChannel::GetBaudrate(void)
{
    return m_baudrate;
}

TY_Byte CChannel::GetNumPreambles(void)
{
    return m_num_preambles;
}

TY_Byte CChannel::GetNumRetries(void)
{
    return m_num_retries;
}

EN_Bool CChannel::GetRetryIfBusy(void)
{
    return m_retry_if_busy;
}

EN_Master CChannel::GetMasterType(void)
{
    return m_master_type;
}

TY_Byte CChannel::GetAddressingMode(void)
{
    return m_addressing_mode;
}

EN_Bool CChannel::GetHartEnabled(void)
{
    return m_hart_enabled;
}

TY_Byte CChannel::GetHartVersion(void)
{
    return m_hart_version;
}

EN_Bool CChannel::IsPrimaryMaster(void)
{
    if (m_master_type == EN_Master::PRIMARY)
    {
        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}

void CChannel::FetchHartIpHostName(TY_Byte* hart_ip_host_name_)
{
    for (TY_Byte i = 0; i < MAX_STRING_LEN; i++)
    {
        hart_ip_host_name_[i] = HartIpHostName[i];
        if (hart_ip_host_name_[i] == 0)
        {
            // Don't waste time
            break;
        }
    }
}

void CChannel::FetchHartIpAddress(TY_Byte* hart_ip_address_)
{
    for (TY_Byte i = 0; i < MAX_STRING_LEN; i++)
    {
        hart_ip_address_[i] = HartIpAddress[i];
        if (hart_ip_address_[i] == 0)
        {
            // Don't waste time
            break;
        }
    }
}

void CChannel::FetchHartIpPort(TY_Byte* hart_ip_port_)
{
    for (TY_Byte i = 0; i < MAX_STRING_LEN; i++)
    {
        hart_ip_port_[i] = HartIpPort[i];
        if (hart_ip_port_[i] == 0)
        {
            // Don't waste time
            break;
        }
    }
}

EN_Bool CChannel::GetHartIpUseAddress(void)
{
    return HartIpUseAddress;
}

EN_CommType CChannel::GetCommType(void)
{
    return m_comm_type;
}

TY_Word CChannel::GetHartIpStatus()
{
    return CHMipMacPort::GetStatus();
}

/* Set Configuration */
void CChannel::SetBaudrate(TY_DWord baudrate_)
{
    if (baudrate_ != m_baudrate)
    {
        m_baudrate = baudrate_;
        if (IsOpen() == EN_Bool::TRUE8)
        {
            // Reopen com port with the new baud_rate
            Close();
            Open(m_port_number, m_comm_type);
        }
    }
}

void CChannel::SetNumPreambles(TY_Byte num_preambles_)
{
    m_num_preambles = num_preambles_;
}

void CChannel::SetNumRetries(TY_Byte num_retries_)
{
    m_num_retries = num_retries_;
}

void CChannel::SetRetryIfBusy(EN_Bool retry_if_busy_)
{
    m_retry_if_busy = retry_if_busy_;
}

void CChannel::SetMasterType(EN_Master master_type_)
{
    m_master_type = master_type_;
}

void CChannel::SetAddressingMode(TY_Byte addressing_mode_)
{
    m_addressing_mode = addressing_mode_;
}

void CChannel::SetHartEnabled(EN_Bool hart_enabled_)
{
    m_hart_enabled = hart_enabled_;
}

void CChannel::SetHartVersion(TY_Byte hart_version_)
{
    m_hart_version = hart_version_;
}

void CChannel::FireServiceEvent(TY_Byte event_, SRV_Handle handle_, TY_DWord data_)
{
    if (IsValidService(handle_) == EN_Bool::TRUE8)
    {
        CService* srv = GetServicePtr(handle_);

        srv->SetLastEvent(event_);
        // Pass back service to user
        GetServicePtr(handle_)->SetOwner(EN_Owner::USER);
    }
}

void CChannel::SetHartIpHostName(TY_Byte* hart_ip_host_name_)
{
    for (TY_Byte i = 0; i < MAX_STRING_LEN; i++)
    {
        HartIpHostName[i] = hart_ip_host_name_[i];
        if (HartIpHostName[i] == 0)
        {
            // Don't waste time
            break;
        }
    }
}

void CChannel::SetHartIpAddress(TY_Byte* hart_ip_address_)
{
    for (TY_Byte i = 0; i < MAX_STRING_LEN; i++)
    {
        HartIpAddress[i] = hart_ip_address_[i];
        if (HartIpAddress[i] == 0)
        {
            // Don't waste time
            break;
        }
    }
}

void CChannel::SetHartIpPort(TY_Byte* hart_ip_port_)
{
    for (TY_Byte i = 0; i < MAX_STRING_LEN; i++)
    {
        HartIpPort[i] = hart_ip_port_[i];
        if (HartIpPort[i] == 0)
        {
            // Don't waste time
            break;
        }
    }
}

void CChannel::SetHartIpUseAddress(EN_Bool hart_ip_use_address_)
{
    HartIpUseAddress = hart_ip_use_address_;
}

void CChannel::BurstIndicate(CFrame* frame_)
{
    // ToDo
    // mpcl_CyclicService->AddCyclicData(pcl_Frame);
}
