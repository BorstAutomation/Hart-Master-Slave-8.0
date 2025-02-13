/*
 *          File: HMuartLayer2.cpp (CHMuartL2SM, CHMuartL2RxSM and CHMuartL2TxSM)
 *                This module implements the entire state machine of the
 *                Hart communication protocol (CHartSM) including the state
 *                machines for sending (CTxSM) and receiving (CRxSM) bytes.
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
#include "HMuartLayer2.h"
#include "HMuartProtocol.h"
#include "HartService.h"
#include "HartChannel.h"
#include "Monitor.h"

// CHMuartL2SM

// Data 
CHMuartL2SM::EN_Status           CHMuartL2SM::Status = EN_Status::IDLE;
CHMuartL2SM::EN_StatusInWATCHING CHMuartL2SM::m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
CHMuartL2SM::EN_StatusInIDLE     CHMuartL2SM::m_status_in_IDLE = EN_StatusInIDLE::IDLE;
CHMuartL2SM::EN_StatusInUSING    CHMuartL2SM::m_status_in_USING = EN_StatusInUSING::NOT_SET;
EN_Bool                          CHMuartL2SM::m_hart_enabled = EN_Bool::FALSE8;
EN_Bool                          CHMuartL2SM::m_burst_mode = EN_Bool::FALSE8;
EN_Bool                          CHMuartL2SM::m_msg_pending = EN_Bool::FALSE8;
CService*                        CHMuartL2SM::m_active_CService;
COSAL::CTimer                    CHMuartL2SM::m_timer;

//Methods
void CHMuartL2SM::Init()
{
    Status = EN_Status::IDLE;
    m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
    m_status_in_IDLE = EN_StatusInIDLE::IDLE;
    m_status_in_USING = EN_StatusInUSING::NOT_SET;
    m_hart_enabled = EN_Bool::FALSE8;
    m_burst_mode = EN_Bool::FALSE8;
    m_msg_pending = EN_Bool::FALSE8;
    m_timer.InitNoneStatic();
    CHMuartL2RxSM::Init();
    CHMuartL2TxSM::Init();
}

// Operation
CHMuartProtocol::EN_ToDo CHMuartL2SM::EventHandler(EN_Event event_, CFrame* frame_)
{
    // This is the implementation of the Hart protocol
    // state machine

    CHMuartProtocol::EN_ToDo to_do = CHMuartProtocol::EN_ToDo::NOTHING;

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

TY_Byte* CHMuartL2SM::GetTxData(TY_Word* len_)
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

void CHMuartL2SM::SetActiveServiceFailed()
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

void CHMuartL2SM::Enable()
{
    m_hart_enabled = EN_Bool::TRUE8;
}

void CHMuartL2SM::Disable()
{
    m_hart_enabled = EN_Bool::FALSE8;
}

// State handling
CHMuartL2SM::EN_Status CHMuartL2SM::HandleStatus_IDLE(EN_Event event_, CFrame* frame_, CHMuartProtocol::EN_ToDo* to_do_)
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
                *to_do_ = CHMuartProtocol::EN_ToDo::RECEIVE_DISABLE;
                m_status_in_IDLE = EN_StatusInIDLE::IDLE;
            }
        }
        break;
    case EN_StatusInIDLE::RECEIVING:
        *to_do_ = CHMuartProtocol::EN_ToDo::NOTHING;
        break;
    default:
        *to_do_ = CHMuartProtocol::EN_ToDo::NOTHING;
        break;
    }

    return EN_Status::IDLE;
}

CHMuartL2SM::EN_Status CHMuartL2SM::HandleStatus_WATCHING(EN_Event event_, CFrame* frame_, CHMuartProtocol::EN_ToDo* to_do_)
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
            UpdateBurst(frame_);
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
            TRANSMITindBurst(frame_);
            UpdateBurst(frame_);
            if (frame_->IsBACK() == EN_Bool::TRUE8)
            {
                SetRT1();
                return Enter_WATCHING(to_do_);
            }
            else
            {
                if (IsMsgPending() == EN_Bool::TRUE8)
                {
                    m_timer.Stop();
                    CHMuartL2RxSM::Reset();
                    return StartTransmit_EnterUSING(to_do_);
                }
                else
                {
                    SetHOLD();
                    return Enter_ENABLED(to_do_);
                }
            }
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

    *to_do_ = CHMuartProtocol::EN_ToDo::NOTHING;
    return Status;
}

CHMuartL2SM::EN_Status CHMuartL2SM::HandleStatus_ENABLED(CHMuartL2SM::EN_Event event_, CFrame* frame_, CHMuartProtocol::EN_ToDo* to_do_)
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

    *to_do_ = CHMuartProtocol::EN_ToDo::NOTHING;
    return Status;
}

CHMuartL2SM::EN_Status CHMuartL2SM::HandleStatus_USING(CHMuartL2SM::EN_Event event_, CFrame* frame_, CHMuartProtocol::EN_ToDo* to_do_)
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
                *to_do_ = CHMuartProtocol::EN_ToDo::RECEIVE_ENABLE;
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
            TRANSMITindBurst(frame_);
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

    * to_do_ = CHMuartProtocol::EN_ToDo::NOTHING;
    return Status;
}

// Helpers

void CHMuartL2SM::SetRT1()
{
    m_timer.Start(GetRT1());
}

void CHMuartL2SM::SetTwoRT1()
{
    m_timer.Start(2 * GetRT1());
}

void CHMuartL2SM::SetRT1prim()
{
    m_timer.Start(GetRT1());
}

EN_Bool CHMuartL2SM::SetRT1diff()
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

void CHMuartL2SM::SetRT2()
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

void CHMuartL2SM::SetHOLD()
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

TY_DWord CHMuartL2SM::GetRT1()
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

TY_DWord CHMuartL2SM::GetRT1prim()
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

EN_Bool CHMuartL2SM::IsBURST()
{
    return m_burst_mode;
}

void CHMuartL2SM::SetBURST(EN_Bool value)
{
    m_burst_mode = value;
}

void CHMuartL2SM::SetMsgPending(EN_Bool value)
{
    m_msg_pending = value;
}

EN_Bool CHMuartL2SM::IsMsgPending()
{
    return m_msg_pending;
}

void CHMuartL2SM::UpdateBurst(CFrame* frame_)
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

EN_Bool CHMuartL2SM::CheckForPendingMsg()
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

CHMuartL2SM::EN_Status CHMuartL2SM::Enter_WATCHING(CHMuartProtocol::EN_ToDo* to_do_)
{
    *to_do_ = CHMuartProtocol::EN_ToDo::RECEIVE_ENABLE;
    return EN_Status::WATCHING;
}

CHMuartL2SM::EN_Status CHMuartL2SM::Enter_ENABLED(CHMuartProtocol::EN_ToDo* to_do_)
{
    *to_do_ = CHMuartProtocol::EN_ToDo::RECEIVE_DISABLE;
    return EN_Status::ENABLED;
}

CHMuartL2SM::EN_Status CHMuartL2SM::StartTransmit_EnterUSING(CHMuartProtocol::EN_ToDo* to_do_)
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    *to_do_ = CHMuartProtocol::EN_ToDo::START_TRANSMIT;
    m_status_in_USING = EN_StatusInUSING::SENDING;
    return EN_Status::USING;
}

CHMuartL2SM::EN_Status CHMuartL2SM::XmtMsg_ENABLE(CHMuartProtocol::EN_ToDo* to_do_)
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    return EN_Status::ENABLED;
}

CHMuartL2SM::EN_Status CHMuartL2SM::XmtMsg_WATCH(CHMuartProtocol::EN_ToDo* to_do_)
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    m_status_in_WATCHING = EN_StatusInWATCHING::IDLE;
    return EN_Status::WATCHING;
}

void CHMuartL2SM::TRANSMITcnfRetry()
{
    m_active_CService->SetSubStatus(CService::EN_SubStat::IDLE);
    m_active_CService->SetCompletionCode(EN_SRV_Result::IN_PROGRESS);
    m_active_CService->SetStatus(CService::EN_Status::REQUESTED);
    m_active_CService = NULL;
    SetMsgPending(EN_Bool::FALSE8);
}

void CHMuartL2SM::TRANSMITcnfFail()
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

void CHMuartL2SM::TRANSMITcnfSuccess(CFrame* frame_)
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

void CHMuartL2SM::TRANSMITindBurst(CFrame* frame_)
{
    CChannel::BurstIndicate(frame_);
}

void CHMuartL2SM::TRANSMITcnfDone()
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

CHMuartL2SM::EN_Status CHMuartL2SM::HandleSrvFailed(CHMuartProtocol::EN_ToDo* to_do_)
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

// CRxSM

// Data
CHMuartL2RxSM::EN_Status CHMuartL2RxSM::Status;
ST_RcvByte       CHMuartL2RxSM::m_pend_rcv_bytes[MAX_TXRX_SIZE];
TY_Word          CHMuartL2RxSM::m_num_pend_bytes;
COSAL::CTimer    CHMuartL2RxSM::m_timer;
TY_Word          CHMuartL2RxSM::m_next_char_gap;
TY_DWord         CHMuartL2RxSM::m_last_rcv_event_time;
TY_Byte          CHMuartL2RxSM::m_expected_rcv_size;

// = Methods =

// Initialization

void CHMuartL2RxSM::Init()
{
    Status = EN_Status::IDLE;
    m_num_pend_bytes = 0;
    m_timer.InitNoneStatic();
    m_next_char_gap = 0;
    m_expected_rcv_size = 14;
}

// Operation

CHMuartProtocol::EN_ToDo CHMuartL2RxSM::EventHandler(CHMuartProtocol::EN_Event event_, CFrame* frame_, ST_RcvByte* rcv_bytes_, TY_Word len_,
    CFrame* junk_frame_, CFrame* request_frame_, CFrame* response_frame_, CFrame* burst_frame_)
{
    ST_RcvByte    au8_LocRcvBytes[MAX_TXRX_SIZE];
    TY_Word               u16_LocLen = 0;
    TY_Word       u16_RemainingBytes = 0;

    if (len_ > MAX_TXRX_SIZE)
    {
        len_ = 0;
    }

    if (len_ > 0)
    {
        // stop gap timer
        m_timer.Stop();
        // calculate the default rcv size (reason: a fifo may be active)
        if (m_expected_rcv_size != len_)
        {
            if (m_expected_rcv_size > len_)
            {
                if (m_expected_rcv_size > 1)
                {
                    m_expected_rcv_size--;
                }
            }
            else
            {
                if (len_ < 15)
                {
                    m_expected_rcv_size = (TY_Byte)len_;
                }
            }
        }
    }

    if ((u16_LocLen = GetPendingBytes(au8_LocRcvBytes)) > 0)
    {
        if (len_ > 0)
        {
            // Check for gap detection
            if ((rcv_bytes_[0].Time - au8_LocRcvBytes[u16_LocLen - 1].Time) > (TY_DWord)(3 * COSAL::CTimer::GetByteTime(CChannel::GetBaudrate())))
            {
                // Gap detected, handle pending bytes first
                u16_RemainingBytes = 1;
                // Eat all pending bytes
                while (u16_RemainingBytes > 0)
                {
                    u16_RemainingBytes = HandleData(EN_Bool::TRUE8, event_, frame_, au8_LocRcvBytes, u16_LocLen,
                        junk_frame_, request_frame_, response_frame_, burst_frame_);
                }
                // Copy the new data to the data buffer
                COSAL::CopyRcvBytes(au8_LocRcvBytes, rcv_bytes_, len_);
                u16_LocLen = len_;
            }
            else
            {
                // Append data to pending data, handle all these bytes
                COSAL::CopyRcvBytes(&au8_LocRcvBytes[u16_LocLen], rcv_bytes_, len_);
                u16_LocLen += len_;
            }
        }
        else
        {
            // Remaining bytes only, already copied to local buffer
        }
    }
    else
    {
        if (len_ > 0)
        {
            // No remaining bytes, use new bytes only
            COSAL::CopyRcvBytes(au8_LocRcvBytes, rcv_bytes_, len_);
            u16_LocLen = len_;
        }
    }

    if (u16_LocLen > 0)
    {
        len_ = u16_LocLen;
        u16_RemainingBytes = HandleData(EN_Bool::FALSE8, event_, frame_, au8_LocRcvBytes, u16_LocLen,
            junk_frame_, request_frame_, response_frame_, burst_frame_);
    }
    if (u16_RemainingBytes > 0)
    {
        SavePendingBytes(au8_LocRcvBytes, u16_RemainingBytes);
    }
    else
    {
        if (m_timer.IsExpired() == EN_Bool::TRUE8)
        {
            HandleData(EN_Bool::TRUE8, CHMuartProtocol::EN_Event::NONE, frame_, NULL, 0,
                junk_frame_, request_frame_, response_frame_, burst_frame_);
            return CHMuartProtocol::EN_ToDo::RECEIVE_ENABLE;
        }
        else
        {
            if ((m_next_char_gap > 0) && (len_ > 0))
            {
                SetGapTimer(m_next_char_gap);
                m_next_char_gap = 0;
            }
        }
    }
    return CHMuartProtocol::EN_ToDo::NOTHING;
}

void CHMuartL2RxSM::Reset()
{
    m_timer.Stop();
    Status = EN_Status::IDLE;
    m_num_pend_bytes = 0;
}

TY_Byte CHMuartL2RxSM::GetBlockSize()
{
    return m_expected_rcv_size;
}

// Helpers

TY_Word CHMuartL2RxSM::HandleData(EN_Bool bGapDetected, CHMuartProtocol::EN_Event event_, CFrame* frame_, ST_RcvByte* rcv_bytes_, TY_Word len_,
    CFrame* junk_frame_, CFrame* request_frame_, CFrame* response_frame_, CFrame* burst_frame_)
{
    TY_Word bytes_parsed = 0;
    TY_Byte         au8_Data[MAX_TXRX_SIZE];
    TY_Byte        au8_Error[MAX_TXRX_SIZE];

    COSAL::ExtractRcvBytes(au8_Data, rcv_bytes_, len_);
    COSAL::ExtractRcvErrors(au8_Error, rcv_bytes_, len_);

    if (bGapDetected == EN_Bool::TRUE8)
    {
        if (frame_->GetTotalLen(len_) == 1)
        {
            frame_->Init();
            Status = EN_Status::IDLE;
        }

        if (len_ == 0)
        {
            if (frame_->GetRcvByteCount() == 0)
            {
                return 0;
            }
        }
    }

    if (frame_->GetRcvByteCount() == 0)
    {
        if (len_ > 0)
        {
            frame_->SetStartTime(rcv_bytes_[0].Time - COSAL::CTimer::GetByteTime(CChannel::GetBaudrate()));
        }
    }

    if (Status == EN_Status::IDLE)
    {
        if (len_ > 0)
        {
            CMonitor::StartReceive(rcv_bytes_[0].Time - COSAL::CTimer::GetByteTime(CChannel::GetBaudrate()));
        }

        Status = EN_Status::RECEIVING;
        frame_->Init();
    }

    if (len_ > 0)
    {
        CMonitor::StoreData(au8_Data, len_);
    }

    // store the end time just for the case
    if (len_ > 0)
    {
        frame_->SetEndTime(rcv_bytes_[len_ - 1].Time);
        m_last_rcv_event_time = rcv_bytes_[len_ - 1].Time;
    }
    if (frame_->TryParse(&bytes_parsed, au8_Data, au8_Error, len_, bGapDetected) == EN_Bool::TRUE8)
    {
        // Frame done, sharp gap time out required
        if (bGapDetected == EN_Bool::FALSE8)
        {
            m_next_char_gap = 3;
            Status = EN_Status::WAIT_SIELENCE;
        }

        // set the end of the frame
        if ((len_ > 0) && (bytes_parsed > 0))
        {
            frame_->SetEndTime(rcv_bytes_[bytes_parsed - 1].Time);
        }

        if (len_ > bytes_parsed)
        {
            TY_Word pending = len_ - bytes_parsed;
            CMonitor::RemoveData(pending);
        }

        switch (frame_->Type)
        {
        case CFrame::EN_Type::JUNK:
            if (CMonitor::GetDataLen() > 0)
            {
                CMonitor::EndRcvGapTO(COSAL::CTimer::GetTime() - 1);
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
            CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
            *request_frame_ = *frame_;
            request_frame_->SetActive();
            Status = EN_Status::IDLE;
            break;
        case CFrame::EN_Type::RESPONSE:
            CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
            *response_frame_ = *frame_;
            response_frame_->SetActive();
            Status = EN_Status::IDLE;
            break;
        case CFrame::EN_Type::BURST:
            CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
            *burst_frame_ = *frame_;
            burst_frame_->SetActive();
            Status = EN_Status::IDLE;
            break;
        }
    }
    else
    {
        if (bGapDetected == EN_Bool::TRUE8)
        {
            m_next_char_gap = 0;
            if (CMonitor::GetDataLen() > 0)
            {
                CMonitor::EndRcvGapTO(m_last_rcv_event_time);
            }
            else
            {
                CMonitor::AbortReceive();
            }
            Status = EN_Status::IDLE;
        }
        else
        {
            if (((frame_->GetStatus() == CFrame::CFrame::EN_Status::PARSE_PREAMBLE) &&
                (len_ == 1)
                ) ||
                (Status == EN_Status::WAIT_SIELENCE)
                )
            {
                // Sharp gap time out at beginning of very short frame and end of the frame
                m_next_char_gap = 3;
            }
            else
            {
                // Use default gap time out
                m_next_char_gap = m_expected_rcv_size + 2;
            }
        }
    }
    if (bytes_parsed != len_)
    {
        int e;
        // Copy the rest of the bytes to the beginning of the buffer
        for (e = 0; e < (len_ - bytes_parsed); e++)
        {
            rcv_bytes_[e] = rcv_bytes_[bytes_parsed + e];
        }
    }
    return (TY_Word)(len_ - bytes_parsed);
}

void CHMuartL2RxSM::SavePendingBytes(ST_RcvByte* rcv_bytes_, TY_Word len_)
{
    m_num_pend_bytes = len_;
    if (len_ > 0)
    {
        COSAL::CopyRcvBytes(m_pend_rcv_bytes, rcv_bytes_, len_);
    }
}

TY_Word  CHMuartL2RxSM::GetPendingBytes(ST_RcvByte* rcv_bytes_)
{
    TY_Word u16_NumPendBytes = m_num_pend_bytes;
    if (u16_NumPendBytes > 0)
    {
        if (m_pend_rcv_bytes == NULL)
        {
            return 0;
        }
        COSAL::CopyRcvBytes(rcv_bytes_, m_pend_rcv_bytes, u16_NumPendBytes);
        m_num_pend_bytes = 0;
    }
    return u16_NumPendBytes;
}

void CHMuartL2RxSM::SetGapTimer(TY_Word u16_NumCharacters)
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

// CTxSM

// Data
CHMuartL2TxSM::EN_Status CHMuartL2TxSM::Status = EN_Status::IDLE;
TY_Word                  CHMuartL2TxSM::m_len = 0;
COSAL::CTimer            CHMuartL2TxSM::m_timer;

// Methods 

// Initialization

void CHMuartL2TxSM::Init()
{
    m_timer.InitNoneStatic();
    Status = EN_Status::IDLE;
}

// Operation

CHMuartProtocol::EN_ToDo CHMuartL2TxSM::EventHandler()
{
    switch (Status)
    {
    case EN_Status::START_TX:
        m_timer.Start(COSAL::CTimer::GetTxDuration(m_len, CChannel::GetBaudrate()));
        Status = EN_Status::WAIT_TX_END;
        return CHMuartProtocol::EN_ToDo::SEND_REQUEST;
        break;
    case EN_Status::WAIT_TX_END:
        if (m_timer.IsExpired() == EN_Bool::TRUE8)
        {
            Status = EN_Status::IDLE;
            return CHMuartProtocol::EN_ToDo::END_TRANSMIT;
        }
        break;
    }
    return CHMuartProtocol::EN_ToDo::NOTHING;
}

CHMuartL2TxSM::EN_Status CHMuartL2TxSM::GetStatus()
{
    return Status;
}

void CHMuartL2TxSM::SetStatus(CHMuartL2TxSM::EN_Status status_)
{
    Status = status_;
}

void CHMuartL2TxSM::SetTxLen(TY_Word len_)
{
    m_len = len_;
}

