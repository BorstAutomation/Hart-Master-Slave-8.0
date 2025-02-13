/*
 *          File: HMipLayer2.cpp (CHartIpSM, CRxIpSM and CTxIpSM)
 *                This module implements the entire state machine of the
 *                Hart communication protocol (CHartIpSM) including the state
 *                machines for sending (CTxIpSM) and receiving (CRxIpSM) bytes.
 *                After the call, the status machine returns a ToDo code
 *                that tells the caller what to do next.
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

#include "WbHartM_Structures.h"
#include "HMipLayer2.h"
#include "HMipProtocol.h"
#include "HartService.h"
#include "HartChannel.h"
#include "Monitor.h"
#include "HMipMacPort.h"

// CHartIpSM

// Data 
CHMipL2SM::EN_Status           CHMipL2SM::Status = EN_Status::IDLE;
CHMipL2SM::EN_StatusInWATCHING CHMipL2SM::m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
CHMipL2SM::EN_StatusInIDLE     CHMipL2SM::m_status_in_IDLE = EN_StatusInIDLE::IDLE;
CHMipL2SM::EN_StatusInUSING    CHMipL2SM::m_status_in_USING = EN_StatusInUSING::NOT_SET;
EN_Bool                        CHMipL2SM::m_hart_enabled = EN_Bool::FALSE8;
EN_Bool                        CHMipL2SM::m_burst_mode = EN_Bool::FALSE8;
EN_Bool                        CHMipL2SM::m_msg_pending = EN_Bool::FALSE8;
CService*                      CHMipL2SM::m_active_CService;
COSAL::CTimer                  CHMipL2SM::m_timer;

// = Methods =
 
// Initialization

void CHMipL2SM::Init()
{
    Status = EN_Status::IDLE;
    m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
    m_status_in_IDLE = EN_StatusInIDLE::IDLE;
    m_status_in_USING = EN_StatusInUSING::NOT_SET;
    m_hart_enabled = EN_Bool::FALSE8;
    m_burst_mode = EN_Bool::FALSE8;
    m_msg_pending = EN_Bool::FALSE8;
    m_timer.InitNoneStatic();
    CHMipL2RxSM::Init();
    CHMipL2TxSM::Init();
}

// Operation

CHMipProtocol::EN_ToDo CHMipL2SM::EventHandler(EN_Event event_, CFrame* frame_)
{
    // This is the implementation of the Hart protocol
    // state machine

    CHMipProtocol::EN_ToDo to_do = CHMipProtocol::EN_ToDo::NOTHING;

    switch (Status)
    {
    case EN_Status::IDLE:
        Status = HandleStatus_IDLE(event_, frame_, &to_do);
        break;
    case EN_Status::WATCHING:
        Status = HandleStatus_WATCHING(event_, frame_, &to_do);
        break;
    case EN_Status::ENABLED:
        Status = HandleStatus_ENABLED(event_, frame_, &to_do);
        break;
    case EN_Status::USING:
        Status = HandleStatus_USING(event_, frame_, &to_do);
        break;
    }

    return to_do;
}

TY_Byte* CHMipL2SM::GetTxData(TY_Word* len_)
{
    if (m_active_CService != NULL)
    {
        TY_Word len;
        TY_Byte* tx_data = m_active_CService->GetTxData(&len);

        if (tx_data != NULL)
        {
            if (len > 0)
            {
                *len_ = len;
                return tx_data;
            }
        }
    }
    return NULL;
}

void CHMipL2SM::SetActiveServiceFailed()
{
    if (m_active_CService != NULL)
    {
        m_active_CService->SetCompletionCode(EN_SRV_Result::RESOURCE_ERROR);
        m_active_CService->SetStatus(CService::EN_Status::WAITING);
        CChannel::FireServiceEvent(CChannel::CServiceEvent::CONFIRMATION,
            m_active_CService->GetHandle(),
            0
        );
        m_active_CService = NULL;
    }
}

void CHMipL2SM::Enable()
{
    m_hart_enabled = EN_Bool::TRUE8;
}

void CHMipL2SM::Disable()
{
    m_hart_enabled = EN_Bool::FALSE8;
}

// State handling

CHMipL2SM::EN_Status CHMipL2SM::HandleStatus_IDLE(EN_Event event_, CFrame* frame_, CHMipProtocol::EN_ToDo* to_do_)
{
    switch (m_status_in_IDLE)
    {
    case EN_StatusInIDLE::IDLE:
        if (CChannel::GetHartEnabled() == EN_Bool::TRUE8)
        {
            SetRT1();
            return Enter_WATCHING(to_do_);
        }
        break;
    case EN_StatusInIDLE::SENDING:
        if (m_active_CService->GetSubStatus() == CService::EN_SubStat::SENDING)
        {
            if (event_ == EN_Event::TX_DONE)
            {
                m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
                m_active_CService->SetCompletionCode(EN_SRV_Result::NO_DEV_RESP);
                m_active_CService->SetStatus(CService::EN_Status::WAITING);
                CChannel::FireServiceEvent(CChannel::CServiceEvent::CONFIRMATION,
                    m_active_CService->GetHandle(),
                    0
                );
                m_active_CService = NULL;
                *to_do_ = CHMipProtocol::EN_ToDo::RECEIVE_DISABLE;
                m_status_in_IDLE = EN_StatusInIDLE::IDLE;
            }
        }
        break;
    case EN_StatusInIDLE::RECEIVING:
        *to_do_ = CHMipProtocol::EN_ToDo::NOTHING;
        break;
    default:
        *to_do_ = CHMipProtocol::EN_ToDo::NOTHING;
        break;
    }

    return EN_Status::IDLE;
}

CHMipL2SM::EN_Status CHMipL2SM::HandleStatus_WATCHING(EN_Event event_, CFrame* frame_, CHMipProtocol::EN_ToDo* to_do_)
{
    // Delete timer internal values
    if (m_timer.IsActive() == EN_Bool::FALSE8)
    {
        m_timer.Stop();
    }

    // Handle time out
    if (m_timer.IsExpired() == EN_Bool::TRUE8)
    {
        SetBURST(EN_Bool::FALSE8);
        SetHOLD();
        return Enter_ENABLED(to_do_);
    }

    // Check for pending request
    if (IsMsgPending() == EN_Bool::FALSE8)
    {
        CheckForPendingMsg();
    }

    // Handle events
    switch (event_)
    {
    case EN_Event::NONE:
        break;

    // Rx data detected
    case EN_Event::RX_DATA_DETECTED:
        m_timer.Stop();
        m_status_in_WATCHING = EN_StatusInWATCHING::RECEIVING;
        break;

    // Request completed
    case EN_Event::RX_COMPLETED_REQ:
        SetRT1();
        m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
        return Enter_WATCHING(to_do_);

    // Response completed
    case EN_Event::RX_COMPLETED_RSP:
        m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
        if (frame_ != NULL)
        {
            UpdateBurstMode(frame_);
            if (IsBURST() == EN_Bool::TRUE8)
            {
                // burst to be expected anyway
                SetRT1();
                return Enter_WATCHING(to_do_);
            }
            else
            {
                if (frame_->IsOACK() == EN_Bool::TRUE8)
                {
                    if (IsMsgPending() == EN_Bool::TRUE8)
                    {
                        return StartTransmit_EnterUSING(to_do_);
                    }
                    else
                    {
                        SetHOLD();
                        return Enter_ENABLED(to_do_);
                    }
                }
                else
                {
                    SetRT2();
                    return Enter_WATCHING(to_do_);
                }
            }
        }
        break;

    // Burst message conpleted   
    case EN_Event::RX_COMPLETED_BST:
        m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
        if (frame_ != NULL)
        {
            SignalBurstIndication(frame_);
            UpdateBurstMode(frame_);
            if (frame_->IsBACK() == EN_Bool::TRUE8)
            {
                //SetRT1();
                SetHOLD();
                return Enter_WATCHING(to_do_);
            }
            else
            {
                if (IsMsgPending() == EN_Bool::TRUE8)
                {
                    m_timer.Stop();
                    CHMipL2RxSM::Reset();
                    return StartTransmit_EnterUSING(to_do_);
                }
                else
                {
                    SetHOLD();
                    return Enter_WATCHING(to_do_);
                }
            }
        }
        else
        {
            SetHOLD();
            return Enter_WATCHING(to_do_);
        }
        break;

    // Rx error detected 
    case EN_Event::RX_COMPLETED_ERR:
        m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
        SetRT1();
        return Enter_WATCHING(to_do_);

    // Tx completed
    case EN_Event::TX_DONE:
        break;
    } // switch event_


    // Check for pending service if totally idle
    if (m_status_in_WATCHING == EN_StatusInWATCHING::IDLE)
    {
        if (m_timer.IsActive() == EN_Bool::FALSE8)
        {
            if (IsMsgPending() == EN_Bool::FALSE8)
            {
                CheckForPendingMsg();
            }
            if (IsMsgPending() == EN_Bool::TRUE8)
            {
                return StartTransmit_EnterUSING(to_do_);
            }
        }
    }

    *to_do_ = CHMipProtocol::EN_ToDo::NOTHING;
    return Status;
}

CHMipL2SM::EN_Status CHMipL2SM::HandleStatus_ENABLED(CHMipL2SM::EN_Event event_, CFrame* frame_, CHMipProtocol::EN_ToDo* to_do_)
{
    if (IsMsgPending() == EN_Bool::FALSE8)
    {
        CheckForPendingMsg();
    }

    if (IsMsgPending() == EN_Bool::TRUE8)
    {
        m_timer.Stop();
        return StartTransmit_EnterUSING(to_do_);
    }

    if (m_timer.IsExpired() == EN_Bool::TRUE8)
    {
        if (IsBURST() == EN_Bool::TRUE8)
        {
            SetTwoRT1();
        }
        else
        {
            SetRT2();
        }
        return Enter_WATCHING(to_do_);
    }

    *to_do_ = CHMipProtocol::EN_ToDo::NOTHING;
    return Status;
}

CHMipL2SM::EN_Status CHMipL2SM::HandleStatus_USING(CHMipL2SM::EN_Event event_, CFrame* frame_, CHMipProtocol::EN_ToDo* to_do_)
{
    switch (m_status_in_USING)
    {
        // Tx in progress
    case EN_StatusInUSING::SENDING:
        if (event_ == EN_Event::TX_DONE)
        {
            if (m_active_CService->GetMode() == CService::EN_Mode::SEND_BURST)
            {
                TRANSMITcnfDone();
                SetRT1();
                return Enter_WATCHING(to_do_);
            }
            else
            {
                m_active_CService->SetSubStatus(CService::EN_SubStat::RECEIVING);
                *to_do_ = CHMipProtocol::EN_ToDo::RECEIVE_ENABLE;
                m_status_in_USING = EN_StatusInUSING::RECEIVING;
                SetRT1prim();
                return EN_Status::USING;
            }
        }
        else
        {
            if (m_timer.IsActive() == EN_Bool::FALSE8)
            {
                SetRT1();
            }
            else
            {
                if (m_timer.IsExpired() == EN_Bool::TRUE8)
                {
                    SetRT1();
                    return Enter_WATCHING(to_do_);
                }
            }
        }
        break;

        // Receiving data
    case EN_StatusInUSING::RECEIVING:

        // Handle Rx Events
        switch (event_)
        {
            // Rx data detected
        case EN_Event::RX_DATA_DETECTED:
            SetRT1();
            break;

            // Ghost tx started
        case EN_Event::GHOST_TX_STARTED:
            m_timer.Stop();
            break;

            // Response received
        case EN_Event::RX_COMPLETED_RSP:
            m_timer.Stop();
            if (IsBURST() == EN_Bool::TRUE8)
            {
                SetRT1();
            }
            else
            {
                SetRT2();
            }
            // Handle retry if busy
            if ((frame_->GetRspCode1() == 32) &&
                (m_active_CService->RetryIfBusy() == EN_Bool::TRUE8)
                )
            {
                TRANSMITcnfRetry();
            }
            else
            {
                TRANSMITcnfSuccess(frame_);
            }

            return Enter_WATCHING(to_do_);

            // Burst received
        case EN_Event::RX_COMPLETED_BST:
            SignalBurstIndication(frame_);
            m_timer.Stop();
            return HandleSrvFailed(to_do_);

            // Request received
        case EN_Event::RX_COMPLETED_REQ:
            m_timer.Stop();
            return HandleSrvFailed(to_do_);

            // Junk received
        case EN_Event::RX_COMPLETED_ERR:
            m_timer.Stop();
            return HandleSrvFailed(to_do_);

            // Tx completed
        case EN_Event::TX_DONE:
            m_timer.Stop();
            break;

            // Ghost tx done
        case EN_Event::GHOST_TX_DONE:
            m_timer.Stop();
            break;

        case EN_Event::NONE:
            break;

            // Any other event
        default:
            SetRT1();
            return Enter_WATCHING(to_do_);
        }

        // Handle Time Out
        if (m_timer.IsExpired() == EN_Bool::TRUE8)
        {
            return HandleSrvFailed(to_do_);
        }

        break;

    default:
        break;
    }

    * to_do_ = CHMipProtocol::EN_ToDo::NOTHING;
    return Status;
}

// Helpers

void CHMipL2SM::SetRT1()
{
    m_timer.Start(GetRT1());
}

void CHMipL2SM::SetTwoRT1()
{
    m_timer.Start(2 * GetRT1());
}

void CHMipL2SM::SetRT1prim()
{
    m_timer.Start(GetRT1());
}

EN_Bool CHMipL2SM::SetRT1diff()
{
    TY_DWord      rt1 = GetRT1();
    TY_DWord rt1_prim = GetRT1prim();

    if (rt1 > rt1_prim)
    {
        m_timer.Start(rt1 - rt1_prim);
        return EN_Bool::TRUE8;
    }
    return EN_Bool::FALSE8;
}

void CHMipL2SM::SetRT2()
{
    switch (CChannel::GetBaudrate())
    {
    case COSAL::CBitRates::CBR_Win::BR_1200:
        m_timer.Start(CUsedTimeOuts::RT2_1200);
        break;
    case COSAL::CBitRates::CBR_Win::BR_2400:
        m_timer.Start(CUsedTimeOuts::RT2_2400);
        break;
    case COSAL::CBitRates::CBR_Win::BR_4800:
        m_timer.Start(CUsedTimeOuts::RT2_4800);
        break;
    default:
        m_timer.Start(CUsedTimeOuts::SHORT_RT2 + COSAL::CTimer::GetDelay(6, CChannel::GetBaudrate()));
        break;
    }
    if (CChannel::GetBaudrate() == 1200)
    {
    }
    else
    {
    }
}

void CHMipL2SM::SetHOLD()
{
    switch (CChannel::GetBaudrate())
    {
    case COSAL::CBitRates::CBR_Win::BR_1200:
        m_timer.Start(CUsedTimeOuts::HOLD_1200);
        break;
    case COSAL::CBitRates::CBR_Win::BR_2400:
        m_timer.Start(CUsedTimeOuts::HOLD_2400);
        break;
    case COSAL::CBitRates::CBR_Win::BR_4800:
        m_timer.Start(CUsedTimeOuts::HOLD_4800);
        break;
    default:
        m_timer.Start(CUsedTimeOuts::SHORT_HOLD);
        break;
    }
}

TY_DWord CHMipL2SM::GetRT1()
{
    if (CChannel::IsPrimaryMaster() == EN_Bool::TRUE8)
    {
        // Primary master
        switch (CChannel::GetBaudrate())
        {
        case COSAL::CBitRates::CBR_Win::BR_1200:
            return CUsedTimeOuts::PRIM_RT1_1200;
            break;
        case COSAL::CBitRates::CBR_Win::BR_2400:
            return CUsedTimeOuts::PRIM_RT1_2400;
            break;
        case COSAL::CBitRates::CBR_Win::BR_4800:
            return CUsedTimeOuts::PRIM_RT1_4800;
            break;
        default:
            return CUsedTimeOuts::SHORT_PRIM_RT1 + COSAL::CTimer::GetDelay(5, CChannel::GetBaudrate());
            break;
        }
    }
    else
    {
        // Secondary master
        switch (CChannel::GetBaudrate())
        {
        case COSAL::CBitRates::CBR_Win::BR_1200:
            return CUsedTimeOuts::SCND_RT1_1200;
            break;
        case COSAL::CBitRates::CBR_Win::BR_2400:
            return CUsedTimeOuts::SCND_RT1_2400;
            break;
        case COSAL::CBitRates::CBR_Win::BR_4800:
            return CUsedTimeOuts::SCND_RT1_4800;
            break;
        default:
            return CUsedTimeOuts::SHORT_SCND_RT1 + COSAL::CTimer::GetDelay(11, CChannel::GetBaudrate());
            break;
        }
    }
}

TY_DWord CHMipL2SM::GetRT1prim()
{
    switch (CChannel::GetBaudrate())
    {
    case COSAL::CBitRates::CBR_Win::BR_1200:
        return CUsedTimeOuts::PRIM_RT1_1200;
        break;
    case COSAL::CBitRates::CBR_Win::BR_2400:
        return CUsedTimeOuts::PRIM_RT1_2400;
        break;
    case COSAL::CBitRates::CBR_Win::BR_4800:
        return CUsedTimeOuts::PRIM_RT1_4800;
        break;
    default:
        return CUsedTimeOuts::SHORT_PRIM_RT1 + COSAL::CTimer::GetDelay(5, CChannel::GetBaudrate());
        break;
    }
}

EN_Bool CHMipL2SM::IsBURST()
{
    return m_burst_mode;
}

void CHMipL2SM::SetBURST(EN_Bool value)
{
    m_burst_mode = value;
}

void CHMipL2SM::SetMsgPending(EN_Bool value)
{
    m_msg_pending = value;
}

EN_Bool CHMipL2SM::IsMsgPending()
{
    return m_msg_pending;
}

void CHMipL2SM::UpdateBurstMode(CFrame* frame_)
{
    if (frame_ != NULL)
    {
        if (frame_->IsBurstModeDevice() == EN_Bool::TRUE8)
        {
            m_burst_mode = EN_Bool::TRUE8;
        }
        else
        {
            m_burst_mode = EN_Bool::FALSE8;
        }
    }
}

EN_Bool CHMipL2SM::CheckForPendingMsg()
{
    if (m_msg_pending == EN_Bool::FALSE8)
    {
        if (m_active_CService == NULL)
        {
            m_active_CService = CChannel::GetServicePtr(CChannel::GetRequestedService());
            if (m_active_CService != NULL)
            {
                m_active_CService->ClearRetryCount();
                m_msg_pending = EN_Bool::TRUE8;
                if (m_active_CService->GetMode() == CService::EN_Mode::SEND_BURST)
                {
                    SetRT2();
                }

                return EN_Bool::TRUE8;
            }
        }
    }
    return EN_Bool::FALSE8;
}

CHMipL2SM::EN_Status CHMipL2SM::Enter_WATCHING(CHMipProtocol::EN_ToDo* to_do_)
{
    *to_do_ = CHMipProtocol::EN_ToDo::RECEIVE_ENABLE;
    return EN_Status::WATCHING;
}

CHMipL2SM::EN_Status CHMipL2SM::Enter_ENABLED(CHMipProtocol::EN_ToDo* to_do_)
{
    *to_do_ = CHMipProtocol::EN_ToDo::RECEIVE_DISABLE;
    return EN_Status::ENABLED;
}

CHMipL2SM::EN_Status CHMipL2SM::StartTransmit_EnterUSING(CHMipProtocol::EN_ToDo* to_do_)
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    *to_do_ = CHMipProtocol::EN_ToDo::START_TRANSMIT;
    m_status_in_USING = EN_StatusInUSING::SENDING;
    return EN_Status::USING;
}

CHMipL2SM::EN_Status CHMipL2SM::XmtMsg_ENABLE(CHMipProtocol::EN_ToDo* to_do_)
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    return EN_Status::ENABLED;
}

CHMipL2SM::EN_Status CHMipL2SM::XmtMsg_WATCH(CHMipProtocol::EN_ToDo* to_do_)
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
    return EN_Status::WATCHING;
}

void CHMipL2SM::TRANSMITcnfRetry()
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    m_active_CService->SetCompletionCode(EN_SRV_Result::IN_PROGRESS);
    m_active_CService->SetStatus(CService::EN_Status::REQUESTED);
    m_active_CService = NULL;
    SetMsgPending(EN_Bool::FALSE8);
}

void CHMipL2SM::TRANSMITcnfFail()
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    m_active_CService->SetCompletionCode(EN_SRV_Result::NO_DEV_RESP);
    m_active_CService->SetStatus(CService::EN_Status::WAITING);
    CChannel::FireServiceEvent(CChannel::CServiceEvent::CONFIRMATION,
        m_active_CService->GetHandle(),
        0
    );
    m_active_CService = NULL;
    SetMsgPending(EN_Bool::FALSE8);
}

void CHMipL2SM::TRANSMITcnfSuccess(CFrame* frame_)
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    m_active_CService->SetCompletionCode(EN_SRV_Result::SUCCESSFUL);
    m_active_CService->SetStatus(CService::EN_Status::WAITING);
    m_active_CService->SetResponse(frame_);
    CChannel::FireServiceEvent(CChannel::CServiceEvent::CONFIRMATION,
        m_active_CService->GetHandle(),
        0
    );
    m_active_CService = NULL;
    SetMsgPending(EN_Bool::FALSE8);
}

void CHMipL2SM::SignalBurstIndication(CFrame* frame_)
{
    CChannel::BurstIndicate(frame_);
}

void CHMipL2SM::TRANSMITcnfDone()
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    m_active_CService->SetCompletionCode(EN_SRV_Result::OBSOLETE);
    m_active_CService->SetStatus(CService::EN_Status::WAITING);
    CChannel::FireServiceEvent(CChannel::CServiceEvent::CONFIRMATION,
        m_active_CService->GetHandle(),
        0
    );
    m_active_CService = NULL;
    SetMsgPending(EN_Bool::FALSE8);
}

CHMipL2SM::EN_Status CHMipL2SM::HandleSrvFailed(CHMipProtocol::EN_ToDo* to_do_)
{
    if (m_active_CService->IsRetryPermitted() == EN_Bool::FALSE8)
    {
        // No more retries possible: service failed
        TRANSMITcnfFail();
        if (IsBURST() == EN_Bool::TRUE8)
        {
            SetRT1();
            return Enter_WATCHING(to_do_);
        }
        else
        {
            SetHOLD();
            return Enter_ENABLED(to_do_);
        }
    }
    else
    {
        m_active_CService->IncRetryCount();
        if (IsBURST() == EN_Bool::TRUE8)
        {
            SetRT2();
            return XmtMsg_WATCH(to_do_);
        }
        else
        {
            if (SetRT1diff() == EN_Bool::TRUE8)
            {
                return XmtMsg_WATCH(to_do_);
            }
            else
            {
                SetHOLD();
                return StartTransmit_EnterUSING(to_do_);
            }
        }
    }
}

// CRxIpSM

// Data
CHMipL2RxSM::EN_Status CHMipL2RxSM::Status;
TY_Byte            CHMipL2RxSM::m_pend_rcv_bytes[MAX_TXRX_SIZE];
TY_Word            CHMipL2RxSM::m_num_pend_bytes;
COSAL::CTimer      CHMipL2RxSM::m_timer;
TY_Word            CHMipL2RxSM::m_next_char_gap;
TY_DWord           CHMipL2RxSM::m_last_rcv_event_time;
TY_Byte            CHMipL2RxSM::m_expected_rcv_size;

// = Methods =

// Initialization

void CHMipL2RxSM::Init()
{
    Status = EN_Status::IDLE;
    m_num_pend_bytes = 0;
    m_timer.InitNoneStatic();
    m_next_char_gap = 0;
    m_expected_rcv_size = 14;
}

// Operation

CHMipProtocol::EN_ToDo CHMipL2RxSM::EventHandler(CHMipProtocol::EN_Event event_, CFrame* frame_, TY_Byte* rcv_bytes_, TY_Word len_,
    CFrame* junk_frame_, CFrame* request_frame_, CFrame* response_frame_, CFrame* burst_frame_)
{
    TY_Byte          au8_LocRcvBytes[MAX_TXRX_SIZE];
    TY_Word               u16_LocLen = 0;
    TY_Word       u16_RemainingBytes = 0;

    if (len_ > MAX_TXRX_SIZE)
    {
        len_ = 0;
    }

    COSAL::CMem::Copy(au8_LocRcvBytes, rcv_bytes_, len_);
    u16_LocLen = len_;

    if (u16_LocLen > 0)
    {
        len_ = u16_LocLen;
        u16_RemainingBytes = HandleData(event_, frame_, au8_LocRcvBytes, u16_LocLen,
            junk_frame_, request_frame_, response_frame_, burst_frame_);
    }

    return CHMipProtocol::EN_ToDo::NOTHING;
}

void CHMipL2RxSM::Reset()
{
    m_timer.Stop();
    Status = EN_Status::IDLE;
    m_num_pend_bytes = 0;
}

TY_Byte CHMipL2RxSM::GetBlockSize()
{
    return m_expected_rcv_size;
}

// Helpers

TY_Word CHMipL2RxSM::HandleData(CHMipProtocol::EN_Event event_, CFrame* frame_, TY_Byte* rcv_bytes_, TY_Word new_data_len_,
    CFrame* junk_frame_, CFrame* request_frame_, CFrame* response_frame_, CFrame* burst_frame_)
{
    TY_Word         bytes_parsed = 0;
    TY_Byte             new_data[MAX_TXRX_SIZE];
    TY_Byte              new_err[MAX_TXRX_SIZE];

    COSAL::CMem::Copy(new_data, rcv_bytes_, new_data_len_);
    COSAL::CMem::Set(new_err, 0, MAX_TXRX_SIZE);

    if (Status == EN_Status::IDLE)
    {
        Status = EN_Status::RECEIVING;
        frame_->Init();

        if (new_data_len_ > 0)
        {
            CMonitor::StartReceive(COSAL::CTimer::GetTime());
            frame_->SetStartTime(COSAL::CTimer::GetTime());
        }
    }


    // store the end time just for the case
    if (new_data_len_ > 0)
    {
        frame_->SetEndTime(COSAL::CTimer::GetTime() + 1);
        m_last_rcv_event_time = COSAL::CTimer::GetTime() + 1;
    }

    frame_->NoPreamb = EN_Bool::TRUE8;
    if (frame_->TryParse(&bytes_parsed, new_data, new_err, new_data_len_, EN_Bool::FALSE8) == EN_Bool::TRUE8)
    {
        TY_Byte mon_data[MAX_TXRX_SIZE];
        TY_Byte mon_data_len = 0;

        COSAL::CMem::Set(mon_data, 0, MAX_TXRX_SIZE);

        // set the end of the frame anew
        if ((new_data_len_ > 0) && (bytes_parsed > 0))
        {
            frame_->SetEndTime(COSAL::CTimer::GetTime());
        }

        if (new_data_len_ > bytes_parsed)
        {
            TY_Word pending = new_data_len_ - bytes_parsed;
            CMonitor::RemoveData(pending);
        }

        switch (frame_->Type)
        {
        case CFrame::EN_Type::JUNK:
            CHMipMacPort::GetIpFrameForMonitor(mon_data, &mon_data_len, new_data, (TY_Byte)new_data_len_, (TY_Byte)CHMipMacPort::EN_Msg_Type::RECEIVED);
            CMonitor::StoreData(mon_data, mon_data_len);
            if (CMonitor::GetDataLen() > 0)
            {
                CMonitor::EndRcvGapTO(COSAL::CTimer::GetTime());
            }
            else
            {
                CMonitor::AbortReceive();
            }

            *junk_frame_ = *frame_;
            junk_frame_->SetActive();
            Status = EN_Status::IDLE;
            break;
        case CFrame::EN_Type::REQUEST:
            CHMipMacPort::GetIpFrameForMonitor(mon_data, &mon_data_len, new_data, (TY_Byte)new_data_len_, (TY_Byte)CHMipMacPort::EN_Msg_Type::REQUEST);
            CMonitor::StoreData(mon_data, mon_data_len);
            CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
            *request_frame_ = *frame_;
            request_frame_->SetActive();
            Status = EN_Status::IDLE;
            break;
        case CFrame::EN_Type::RESPONSE:
            CHMipMacPort::GetIpFrameForMonitor(mon_data, &mon_data_len, new_data, (TY_Byte)new_data_len_, (TY_Byte)CHMipMacPort::EN_Msg_Type::RESPONSE);
            CMonitor::StoreData(mon_data, mon_data_len);
            CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
            *response_frame_ = *frame_;
            response_frame_->SetActive();
            Status = EN_Status::IDLE;
            break;
        case CFrame::EN_Type::BURST:
            CHMipMacPort::GetIpFrameForMonitor(mon_data, &mon_data_len, new_data, (TY_Byte)new_data_len_, (TY_Byte)CHMipMacPort::EN_Msg_Type::BURST);
            CMonitor::StoreData(mon_data, mon_data_len);
            CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
            *burst_frame_ = *frame_;
            burst_frame_->SetActive();
            Status = EN_Status::IDLE;
            break;
        }
    }

    if (bytes_parsed != new_data_len_)
    {
        int e;
        // Copy the rest of the bytes to the beginning of the buffer
        for (e = 0; e < (new_data_len_ - bytes_parsed); e++)
        {
            rcv_bytes_[e] = rcv_bytes_[bytes_parsed + e];
        }
    }

    return (TY_Word)(new_data_len_ - bytes_parsed);
}

void CHMipL2RxSM::SetGapTimer(TY_Word u16_NumCharacters)
{
    if (u16_NumCharacters > 0)
    {
        TY_DWord u32_Gap = (TY_DWord)((u16_NumCharacters + 1) * COSAL::CTimer::GetByteTime(CChannel::GetBaudrate()));

        if (u32_Gap < 50)
        {
            u32_Gap = 50;
        }

        m_timer.Start(u32_Gap);
    }
    else
    {
        TY_DWord u32_Gap = (TY_DWord)(3 * COSAL::CTimer::GetByteTime(CChannel::GetBaudrate()));

        m_timer.Start(u32_Gap);
    }
}

// CTxIpSM

// Data
CHMipL2TxSM::EN_Status CHMipL2TxSM::Status = EN_Status::IDLE;
TY_Word          CHMipL2TxSM::m_len = 0;
COSAL::CTimer    CHMipL2TxSM::m_timer;

// Methods 

// Initialization

void CHMipL2TxSM::Init()
{
    m_timer.InitNoneStatic();
    Status = EN_Status::IDLE;
}

// Operation

CHMipProtocol::EN_ToDo CHMipL2TxSM::EventHandler()
{
    switch (Status)
    {
    case EN_Status::START_TX:
        Status = EN_Status::WAIT_TX_END;
        return CHMipProtocol::EN_ToDo::SEND_REQUEST;
        break;
    case EN_Status::WAIT_TX_END:
        Status = EN_Status::IDLE;
        return CHMipProtocol::EN_ToDo::END_TRANSMIT;
        break;
    }

    return CHMipProtocol::EN_ToDo::NOTHING;
}

CHMipL2TxSM::EN_Status CHMipL2TxSM::GetStatus()
{
    return Status;
}

void CHMipL2TxSM::SetStatus(CHMipL2TxSM::EN_Status status_)
{
    Status = status_;
}

void CHMipL2TxSM::SetTxLen(TY_Word len_)
{
    m_len = len_;
}

