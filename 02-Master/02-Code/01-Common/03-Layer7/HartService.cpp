/*
 *          File: CHartService.cpp (CService)
 *                In simple terms, a service executes a Hart command by
 *                passing a request to Layer2 of the Hart protocol.
 *                In doing so, it returns a handle to the caller, with
 *                which the calling program can check the status.
 *                A service is only considered completed when the caller
 *                has read the response (e.g. FetchConfirmation).
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
#include "HartFrame.h"
#include "HartService.h"
#include "HartChannel.h"
#include "HartCoding.h"

 /* Initialization and Termination */
void CService::Init()
{
    Clear();
    m_is_active = EN_Bool::TRUE8;
    m_owner = EN_Owner::USER;
}

void CService::Release()
{
    m_is_active = EN_Bool::FALSE8;
}

EN_Bool CService::IsActive()
{
    return m_is_active;
}

void CService::Clear()
{
    m_request.Init();
    m_response.Init();
    Status = EN_Status::IDLE;
    m_sub_status = EN_SubStat::IDLE;
    m_type = EN_Type::SEND_RECEIVE;
    m_mode = EN_Mode::NORMAL;
    m_duration = 0;
    m_retry_count = 0;
    m_req_cmd = 0;
}

/* Handling of properties */
SRV_Handle CService::GetHandle()
{
    return m_handle;
}

void CService::SetHandle(SRV_Handle service_)
{
    m_handle = service_;
}

void CService::Launch()
{
    m_request.SetLocalMaster(CChannel::IsPrimaryMaster());
    m_response.SetLocalMaster(CChannel::IsPrimaryMaster());
    if (GetMode() == EN_Mode::SEND_BURST)
    {
        m_request.SetBurstFrame(EN_Bool::TRUE8);
    }

    m_request.Encode();
    Status = EN_Status::REQUESTED;
}

void CService::SetLastEvent(TY_Byte event_)
{
    m_last_event = event_;
}

TY_Byte CService::GetLastEvent()
{
    return m_last_event;
}

CService::EN_Status CService::GetStatus()
{
    return Status;
}

void CService::SetStatus(EN_Status status_)
{
    Status = status_;
    if (status_ == EN_Status::BUSY)
    {
        m_start_time = COSAL::CTimer::GetTime();
    }

    if (status_ == EN_Status::WAITING)
    {
        m_duration = (TY_Word)(COSAL::CTimer::GetTime() - m_start_time);
    }
}

CService::EN_SubStat CService::GetSubStatus()
{
    return m_sub_status;
}

void CService::SetSubStatus(EN_SubStat status_)
{
    m_sub_status = status_;
}

CService::EN_Type CService::GetType()
{
    return m_type;
}

void CService::SetType(EN_Type type_)
{
    m_type = type_;
}

CService::EN_Mode CService::GetMode()
{
    return m_mode;
}

void CService::SetMode(EN_Mode mode_)
{
    m_mode = mode_;
}

EN_SRV_Result CService::GetCompletionCode()
{
    return m_completion_code;
}

void CService::SetCompletionCode(EN_SRV_Result code_)
{
    m_completion_code = code_;
}

EN_Owner CService::GetOwner()
{
    return m_owner;
}

void CService::SetOwner(EN_Owner owner_)
{
    m_owner = owner_;
}

TY_Byte* CService::GetTxData(TY_Word* len_)
{
    TY_Word len;
    TY_Byte* tx_data;

    tx_data = m_request.GetTxData(&len);
    if (tx_data != NULL)
    {
        if (len > 0)
        {
            *len_ = len;
            return tx_data;
        }
    }

    return NULL;
}

EN_Bool CService::IsRetryPermitted()
{
    if (m_retry_count < CService::m_num_retries)
    {
        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}

void CService::ClearRetryCount()
{
    m_retry_count = 0;
}

void CService::IncRetryCount()
{
    m_retry_count++;
}
#pragma endregion
//
/* Set Request Data */
void CService::SetCommand(TY_Byte command_)
{
    m_request.Command = command_;
    m_req_cmd = command_;
}

void CService::SetAddrMode(TY_Byte addr_mode_)
{
    m_request.AddrMode = addr_mode_;
}

void CService::SetRetryIfBusy(EN_Bool retry_if_busy_)
{
    m_retry_if_busy = retry_if_busy_;
}

void CService::SetNumRetries(TY_Byte num_retries_)
{
    m_num_retries = num_retries_;
}

void CService::SetNumPreambles(TY_Byte num_preambles_)
{
    m_request.NumPreambles = num_preambles_;
}

void CService::SetData(TY_Byte* data_, TY_Byte len_)
{
    m_request.SetData(data_, len_);
}

void CService::SetUniqueID(TY_Byte* unique_id_)
{
    m_request.SetUniqueAddr(unique_id_);
}

void CService::SetShortAddr(TY_Byte short_addr_)
{
    m_request.SetPollAddr(short_addr_);
}

/* Setup up request */
void CService::SetResponse(CFrame* frame_)
{
    m_response.Init();
    m_response = *frame_;
}

EN_Bool CService::IsInProgress()
{
    if (Status == EN_Status::REQUESTED)
    {
        return EN_Bool::TRUE8;
    }
    else if (Status == EN_Status::BUSY)
    {
        return EN_Bool::TRUE8;
    }
    else if (Status == EN_Status::KILL)
    {
        return EN_Bool::TRUE8;
    }
    else if (Status == EN_Status::IDLE)
    {
        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}

EN_Bool CService::RetryIfBusy()
{
    return m_retry_if_busy;
}

EN_Bool CService::Failed()
{
    switch (m_completion_code)
    {
    case EN_SRV_Result::NO_DEV_RESP:
    case EN_SRV_Result::COMM_ERR:
    case EN_SRV_Result::RESOURCE_ERROR:
        return EN_Bool::TRUE8;
    default:
        break;
    }
    return EN_Bool::FALSE8;
}

TY_Byte CService::GetRespLen()
{
    return m_response.GetDataSize();
}

TY_Byte CService::GetRespCode1()
{
    return m_response.GetRspCode1();
}

TY_Byte CService::GetRespCode2()
{
    return m_response.GetRspCode2();
}

TY_Byte CService::GetRespDataByte(TY_Byte idx_)
{
    TY_Byte* data;
    TY_Byte  len;

    data = m_response.GetDataBuffer();
    if (data != NULL)
    {
        len = m_response.GetDataSize();
        if (len > idx_)
        {
            return data[idx_];
        }
    }

    return 0;
}

EN_Bool CService::GetDeviceInBurstMode()
{
    return m_response.IsBurstModeDevice();
}

TY_Byte CService::GetUsedRetries()
{
    return 0;
}

TY_Word CService::GetDuration()
{
    return m_duration;
}

TY_Byte CService::GetRespData(TY_Byte* data_bytes_)
{
    TY_Byte len;

    len = m_response.GetDataSize();
    if (len > 0)
    {
        TY_Byte* data = m_response.GetDataBuffer();
        if (data != 0)
        {
            COSAL::CMem::Copy(data_bytes_, data, len);
            return len;
        }
    }

    return 0;
}

TY_Byte CService::GetRespCmd()
{
    return m_response.Command;
}
