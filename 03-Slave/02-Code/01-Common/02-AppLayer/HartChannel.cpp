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

#include "WbHartSlave.h"
#include "HartChannel.h"
#include "HartService.h"
#include "HSuartMacPort.h"
#include "HSipMacPort.h"
#include "HartDevice.h"

// Data
// Public
EN_Bool CChannel::HartEnabledChanged = EN_Bool::TRUE8;
EN_Bool CChannel::BurstModeChanged = EN_Bool::FALSE8;
CService CChannel::PrimMasterService;
CService CChannel::ScndMasterService;

// Private
EN_Bool     CChannel::m_is_open = EN_Bool::FALSE8;
EN_CommType CChannel::m_comm_type = EN_CommType::UART;

// Methods
EN_Bool CChannel::Open(TY_Word port_number_, EN_CommType type_)
{
    // Close channel if already open by any port
    if (m_is_open == EN_Bool::TRUE8)
    {
        Close();
        m_is_open = EN_Bool::FALSE8;
    }

    Init();
    CHartData::CStat.ComPort = (TY_Byte)port_number_ - 1;
    m_comm_type = type_;
    if (type_ == EN_CommType::UART)
    {
        if (CHSuartMacPort::Open(CHartData::CStat.ComPort, CHartData::CStat.BaudRate, m_comm_type) == EN_Bool::TRUE8)
        {
            m_is_open = EN_Bool::TRUE8;
            // Reset the Hart state machine
            HartEnabledChanged = EN_Bool::TRUE8;
            return EN_Bool::TRUE8;
        }
    }
    else if (type_ == EN_CommType::HART_IP)
    {
        if (CHSipMacPort::Open(CHartData::CStat.HartIpHostName, CHartData::CStat.HartIpPort, m_comm_type) == EN_Bool::TRUE8)
        {
            m_is_open = EN_Bool::TRUE8;
            HartEnabledChanged = EN_Bool::TRUE8;
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
            CHSuartMacPort::Close();
            m_is_open = EN_Bool::FALSE8;
        }
        else if (m_comm_type == EN_CommType::HART_IP)
        {
            CHSipMacPort::Close();
            m_is_open = EN_Bool::FALSE8;
        }
    }
}

void CChannel::Init()
{
    CDevice::Init();
    if (m_comm_type == EN_CommType::HART_IP)
    {
        CHSipMacPort::Init();
    }
    else
    {
        CHSuartMacPort::Init();
    }
}

// Test Information
TY_Word CChannel::GetHartIpStatus()
{
    return CHSipMacPort::GetStatus();
}

// Service handling
SRV_Handle CChannel::WasCommandReceived()
{
    SRV_Handle result = INVALID_SRV_HANDLE;

    if (CService::ActiveService != NULL)
    {
        if (CService::ActiveService->Owner == EN_Owner::USER)
        {
            if (CService::ActiveService->Status == CService::EN_Status::REQUESTED)
            {
                if (CService::ActiveService->Master == EN_Master::PRIMARY)
                {
                    result = (SRV_Handle)(0);
                }
                else
                {
                    result = (SRV_Handle)(1);
                }

                CService::ActiveService->Status = CService::EN_Status::BUSY;
            }
        }
    }

    return result;
}

// Get Configuration
TY_DWord CChannel::GetBaudrate(void)
{
    return CHartData::CStat.BaudRate;
}

EN_Bool CChannel::IsHartEnabled(void)
{
    return CHartData::CStat.HartEnabled;
}

EN_Bool CChannel::IsBurstMode(void)
{
    return CHartData::CStat.BurstMode;
}

EN_CommType CChannel::GetCommType(void)
{
    return m_comm_type;
}
