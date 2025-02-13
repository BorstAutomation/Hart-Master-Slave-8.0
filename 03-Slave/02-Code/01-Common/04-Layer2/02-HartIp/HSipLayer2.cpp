/*
 *          File: HSipLayer2.cpp (CHSipL2SM, CHSipL2RxSM and CHSipL2TxSM)
 *                This module implements the state machine of the
 *                Hart communication protocol (CHSipL2SM) including the state
 *                machines for sending (CHSipL2TxSM) and receiving
 *                (CHSipL2RxSM) bytes.
 *                After the call, the status machine returns a ToDo code
 *                that tells the caller what to do next.
 *                Note: This is the implementation of a slave. 
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

// Slave / Burst-Mode Device State Machine
// see: https://library.fieldcommgroup.org/20081/TS20081/9.1/#page=34
//
//                             +-----(06)-----+
//                             |    ENABLE_BURST_MODE 
//          +------(01)-----+  |    -----------------
//     ENABLE.indicate      |  |    Set BURST = TRUE
//     ---------------    +-+--+-+  Set BT = 0
//     RCV_MSG            |      |  HOST = master 
//          +------------>|      |<-----------+
//                        |      |
//          +------(02)---+ WAIT +---(07)-----+
// DISABLE_BURST_MODE     |      |  RCV_MSG == STX && ~AddressMatch
// ------------------     |      |  -------------------------------
// Set BURST = FALSE      |      |  Set BT = RT1 (Primary)
//          +------------>|      |<-----------+                                  +----------+
//                        |      |                                               |  RCV_MSG = No Comm Error
//          +------(03)---+      |  RCV_MSG == STX && AddressMatch               |  -----------------------
//     RCV_MSG == ERR     |      |  -------------------------------              |  TRANSMIT.indicate
//          +------------>|      |  Set BT = 0                              +----+----+     |
//                        |      |  Set Tx_Timeout = STO                    | PROCESS |     |
//          +------(04)---+      +---(08)---------------------------------->|         |<----+
//  RCV_MSG == ACK        |      |                                          |         |
//  --------------        |      |  TRANSMIT.response                       |         |
//  Set BT = 0            |      |  -----------------                       |         |
//  Set Tx_Timeout = STO  |      |  XMT_MSG(ACK)                            |         |
//          +------------>|      |<-----------------------------------------+         |
//                        |      |                                          |         |
//          +------(05)---+      |  RCV_MSG == Comm Error                   |         |
//  BURST && (BT == 0)    |      |  --------------------                    |         |
//  ------------------    |      |  XMT_MSG (Comm Error)                    |         |
//  XMT_MSG(BACK)         |      |<-----------------------------------------+         |
//  Set BT = RT2          |      |                                          |         |
//  HOST = ~HOST          |      |  Tx_Timeout == 0                         |         |
//          +------------>|      |  ---------------                         |         |
//                        |      |<-----------------------------------------+         |
//                        |      |                                          |         |
//                        +------+                                          +---------+
// ------------------------------------------------------------------------------------
// BACK = Burst ACKnowlegde, BT = Burst Timer
//

#include "WbHartSlave.h"
#include "HSipMacPort.h"
#include "HSipProtocol.h"
#include "HSipLayer2.h"
#include "HartService.h"
#include "HartChannel.h"

// CHSipL2SM
// Private data
CHSipL2SM::EN_Status           CHSipL2SM::m_status = EN_Status::WAIT;
CHSipL2SM::EN_StatusInWAIT     CHSipL2SM::m_status_in_WAIT = EN_StatusInWAIT::IDLE;
CHSipL2SM::EN_StatusInPROCESS  CHSipL2SM::m_status_in_PROCESS = EN_StatusInPROCESS::NOT_SET;
EN_Bool                        CHSipL2SM::m_burst = EN_Bool::FALSE8;
COSAL::CTimer                  CHSipL2SM::mo_BT_timer;
COSAL::CTimer                  CHSipL2SM::mo_Tx_timer;

// Initialization
void CHSipL2SM::Init()
{
    m_status = EN_Status::WAIT;
    m_status_in_WAIT = EN_StatusInWAIT::IDLE;
    m_status_in_PROCESS = EN_StatusInPROCESS::NOT_SET;
    m_burst = EN_Bool::FALSE8;
    mo_BT_timer.InitNoneStatic();
    mo_Tx_timer.InitNoneStatic();
    CHSipL2RxSM::Init();
    CHSipL2TxSM::Init();
}

// Operation
// State machine handler
CHSipProtocol::EN_ToDo CHSipL2SM::EventHandler(EN_Event event_)
{
    // Note that CHMUartProt is calling this state machine
    // and needs back a todo
    CHSipProtocol::EN_ToDo to_do = CHSipProtocol::EN_ToDo::NOTHING;

    switch (m_status)
    {
    case EN_Status::WAIT:
        m_status = HandleStatus_WAIT(event_, &to_do);
        break;
    case EN_Status::PROCESS:
        m_status = HandleStatus_PROCESS(event_, &to_do);
        break;
    }

    return to_do;
}

CHSipL2SM::EN_Status CHSipL2SM::HandleStatus_WAIT(EN_Event event_, CHSipProtocol::EN_ToDo* to_do_)
{
    // Handle events

    // Set default to_do
    *to_do_ = CHSipProtocol::EN_ToDo::NOTHING;

    EN_Event event = GetSoftEvent(m_status_in_WAIT, event_);

    // Event handling depends on the internal state
    switch (m_status_in_WAIT)
    {
    case EN_StatusInWAIT::IDLE:
        if (event == EN_Event::ENABLE_indicate)
        {
            // --- (01) ---
            // Reset receiver etc.
            // Next status in wait == receiving
            *to_do_ = CHSipProtocol::EN_ToDo::RECEIVE_ENABLE;
            m_status_in_WAIT = EN_StatusInWAIT::RECEIVING;
            if (CHartData::CStat.BurstMode == EN_Bool::TRUE8)
            {
                EnableBurst();
            }

            return m_status;
        }
        break;
    case EN_StatusInWAIT::RECEIVING:
        m_status = HandleReceivingInWAIT(event, to_do_);
        break;
    case EN_StatusInWAIT::WAIT_STO:
        if (mo_Tx_timer.IsExpired() == EN_Bool::TRUE8)
        {
            if (CHartData::CStat.BurstMode == EN_Bool::TRUE8)
            {
                mo_BT_timer.Start(GetRT2());
            }

            *to_do_ = CHSipProtocol::EN_ToDo::RESET_RECEIVER;
            m_status_in_WAIT = EN_StatusInWAIT::RECEIVING;
        }
        break;
    case EN_StatusInWAIT::WAIT_XMIT_BACK:
        if (event_ == CHSipL2SM::EN_Event::XMT_MSG_done)
        {
            // Toggle burst master
            CService::Burst.Host = (CService::Burst.Host == EN_Master::PRIMARY) ? EN_Master::SECONDARY : EN_Master::PRIMARY;
            mo_BT_timer.Start(GetRT2());
            *to_do_ = CHSipProtocol::EN_ToDo::RECEIVE_ENABLE;
            m_status_in_WAIT = EN_StatusInWAIT::RECEIVING;
            m_status = CHSipL2SM::EN_Status::WAIT;
        }
        break;
    }

    return m_status;
}

CHSipL2SM::EN_Status CHSipL2SM::HandleReceivingInWAIT(EN_Event event_, CHSipProtocol::EN_ToDo* to_do_)
{
    EN_Status next_status = m_status;

    switch (event_)
    {
    case EN_Event::DISABLE_BURST_MODE:
        // --- (02) ---
        DisableBurst();
        break;

        //  --- (03) ---
        // Rx error detected 
    case EN_Event::RCV_MSG_ERR:
        *to_do_ = CHSipProtocol::EN_ToDo::RESET_RECEIVER;
        m_status_in_WAIT = EN_StatusInWAIT::RECEIVING;
        break;

        //  --- (04) ---
        // Response or burst received
    case EN_Event::RCV_MSG_ACK:
    case EN_Event::RCV_MSG_BACK:
        mo_BT_timer.Stop();
        mo_Tx_timer.Start(GetSTO());
        m_status_in_WAIT = EN_StatusInWAIT::WAIT_STO;
        break;

        // --- (05) ---
        // Burst timer expired in burst mode
    case EN_Event::BURST_BT_EXPIRED:
        mo_BT_timer.Stop();
        CBurst::Launch();
        *to_do_ = CHSipProtocol::EN_ToDo::START_TX_BURST;
        m_status_in_WAIT = EN_StatusInWAIT::WAIT_XMIT_BACK;
        break;

        // --- (06) ---
        // Enable burst mode
    case EN_Event::ENABLE_BURST_MODE:
        EnableBurst();
        break;

    case EN_Event::RCV_MSG_STX_NoMatch:
        mo_BT_timer.Start(GetRT1prim());
        *to_do_ = CHSipProtocol::EN_ToDo::RESET_RECEIVER;
        break;

    case EN_Event::RCV_MSG_STX_Match:
        mo_BT_timer.Stop();
        mo_Tx_timer.Start(GetSTO());
        CService::PassToUser(&CHSipProtocol::WorkFrame);
        m_status_in_PROCESS = EN_StatusInPROCESS::WAIT_STO;
        next_status = EN_Status::PROCESS;
        // This frame has to be processed by higher layers
        // before doing anything else.
        *to_do_ = CHSipProtocol::EN_ToDo::RECEIVE_DISABLE;
        break;

    case EN_Event::ACTIVITY_DETECTED:
        mo_BT_timer.Stop();
        break;

        // --- (??) ---
        // Disable Hart communication
    case EN_Event::DISABLE_indicate:
        mo_BT_timer.Stop();
        mo_Tx_timer.Stop();
        *to_do_ = CHSipProtocol::EN_ToDo::RECEIVE_DISABLE;
        m_status_in_WAIT = EN_StatusInWAIT::IDLE;
        next_status = EN_Status::WAIT;
        if (CHartData::CStat.BurstMode == EN_Bool::TRUE8)
        {
            CHartData::CStat.BurstMode = EN_Bool::FALSE8;
            DisableBurst();
        }

        break;
    } // switch event_

    return next_status;
}

CHSipL2SM::EN_Status CHSipL2SM::HandleStatus_PROCESS(CHSipL2SM::EN_Event event_, CHSipProtocol::EN_ToDo* to_do_)
{
    switch (m_status_in_PROCESS)
    {
    case EN_StatusInPROCESS::NOT_SET:
        m_status_in_WAIT = EN_StatusInWAIT::RECEIVING;
        m_status = EN_Status::WAIT;

        *to_do_ = CHSipProtocol::EN_ToDo::NOTHING;
        break;

    case EN_StatusInPROCESS::WAIT_STO:
        if (mo_Tx_timer.IsExpired() == EN_Bool::TRUE8)
        {
            mo_Tx_timer.Stop();
            CService::MakeErrorResponse(CHart::CRespCode::CMD_ERR);
            *to_do_ = CHSipProtocol::EN_ToDo::START_TX_RESPONSE;
            m_status_in_PROCESS = EN_StatusInPROCESS::WAIT_TX;
        }

        if (CService::ActiveService->Owner == EN_Owner::PROTOCOL)
        {
            if (CService::ActiveService->Status == CService::EN_Status::XMT_RESPONSE)
            {
                CService::MakeResponse();
                *to_do_ = CHSipProtocol::EN_ToDo::START_TX_RESPONSE;
                m_status_in_PROCESS = EN_StatusInPROCESS::WAIT_TX;
            }

            if (CService::ActiveService->Status == CService::EN_Status::XMT_ERR)
            {
                CService::MakeErrorResponse(CService::ActiveService->ErrRespCode);
                *to_do_ = CHSipProtocol::EN_ToDo::START_TX_RESPONSE;
                m_status_in_PROCESS = EN_StatusInPROCESS::WAIT_TX;
            }
        }
        break;

    case EN_StatusInPROCESS::WAIT_TX:
        if (event_ == CHSipL2SM::EN_Event::XMT_MSG_done)
        {
            EN_Bool send_burst = EN_Bool::FALSE8;

            if (CHartData::CStat.BurstMode == EN_Bool::TRUE8)
            {
                if (mo_BT_timer.IsActive() == EN_Bool::TRUE8)
                {
                    if (mo_BT_timer.IsExpired() == EN_Bool::TRUE8)
                    {
                        mo_BT_timer.Stop();
                        send_burst = EN_Bool::TRUE8;
                    }
                }

                if (send_burst == EN_Bool::FALSE8)
                {
                    // Toggle burst and restart
                    CService::Burst.Host = (CService::Burst.Host == EN_Master::PRIMARY) ? EN_Master::SECONDARY : EN_Master::PRIMARY;
                    mo_BT_timer.Start(GetRT2());
                }
            }

            if (send_burst == EN_Bool::TRUE8)
            {
                CBurst::Launch();
                *to_do_ = CHSipProtocol::EN_ToDo::START_TX_BURST;
                m_status_in_PROCESS = EN_StatusInPROCESS::WAIT_XMIT_BACK;
            }
            else
            {
                *to_do_ = CHSipProtocol::EN_ToDo::RECEIVE_ENABLE;
                m_status_in_WAIT = EN_StatusInWAIT::RECEIVING;
                m_status = CHSipL2SM::EN_Status::WAIT;
            }

            CService::ReleaseActiveService();
        }

        break;

    case EN_StatusInPROCESS::WAIT_XMIT_BACK:
        *to_do_ = CHSipProtocol::EN_ToDo::RECEIVE_ENABLE;
        m_status_in_WAIT = EN_StatusInWAIT::RECEIVING;
        m_status = CHSipL2SM::EN_Status::WAIT;
        mo_BT_timer.Start(GetRT2());
        break;
    }

    return m_status;
}

// Event handling
CHSipL2SM::EN_Event CHSipL2SM::GetSoftEvent(EN_StatusInWAIT status_, EN_Event event_)
{
    EN_Event result = event_;

    // However the event RCV_MSG_STX is splitted
    // into two events
    if (event_ == EN_Event::RCV_MSG_STX)
    {
        EN_Bool is_match = EN_Bool::TRUE8;
        EN_Bool is_global_addr = EN_Bool::TRUE8;

        if (CHSipProtocol::WorkFrame.AddrMode == CHart::CAddrMode::POLLING)
        {
            if (CHSipProtocol::WorkFrame.GetShortAddr() !=
                CHartData::CStat.PollAddress)
            {
                is_match = EN_Bool::FALSE8;
            }
        }
        else
        {
            // Long address type
            TY_Byte req_addr[5];
            CHSipProtocol::WorkFrame.GetOtherUniqueID(req_addr);
            // Take out master and burst mode flag
            req_addr[0] &= 0x3f;

            for (TY_Byte e = 0; e < 5; e++)
            {
                if (req_addr[e] != CHartData::CStat.LongAddress[e])
                {
                    is_match = EN_Bool::FALSE8;
                    break;
                }
            }

            if (is_match == EN_Bool::FALSE8)
            {
                // Global address?
                for (TY_Byte e = 0; e < 5; e++)
                {
                    if (req_addr[e] != 0)
                    {
                        is_global_addr = EN_Bool::FALSE8;
                        break;
                    }
                }

                if (is_global_addr == EN_Bool::TRUE8)
                {
                    if (CHSipProtocol::WorkFrame.Command == 11)
                    {
                        if (CHSipProtocol::WorkFrame.PayloadCount == 6)
                        {
                            TY_Byte* data = CHSipProtocol::WorkFrame.PayloadData;
                            // Does short tag match?
                            for (TY_Byte e = 0; e < 6; e++)
                            {
                                is_match = EN_Bool::TRUE8;
                                if (data[e] != CHartData::CStat.ShortTag[e])
                                {
                                    is_match = EN_Bool::FALSE8;
                                }
                            }
                        }
                    }
                    else if (CHSipProtocol::WorkFrame.Command == 21)
                    {
                        if (CHSipProtocol::WorkFrame.PayloadCount == 32)
                        {
                            TY_Byte* data = CHSipProtocol::WorkFrame.PayloadData;
                            // Does long tag match?
                            for (TY_Byte e = 0; e < 32; e++)
                            {
                                is_match = EN_Bool::TRUE8;
                                if (data[e] != CHartData::CStat.LongTag[e])
                                {
                                    is_match = EN_Bool::FALSE8;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (is_match == EN_Bool::TRUE8)
        {
            return CHSipL2SM::EN_Event::RCV_MSG_STX_Match;
        }
        else
        {
            return CHSipL2SM::EN_Event::RCV_MSG_STX_NoMatch;
        }
    }

    if (event_ == EN_Event::SILENCE_DETECTED)
    {
        if (m_burst == EN_Bool::TRUE8)
        {
            if (mo_BT_timer.IsExpired() == EN_Bool::TRUE8)
            {
                mo_BT_timer.Stop();

                if (CService::ActiveService != NULL)
                {
                    if (CService::ActiveService->Status == CService::EN_Status::IDLE)
                    {
                        return EN_Event::BURST_BT_EXPIRED;
                    }
                }
                else
                {
                    return EN_Event::BURST_BT_EXPIRED;
                }
            }
        }
    }

    // The following events can only be processed
    // if no action is running (e.g. transmitting)
    if ((event_ == EN_Event::NONE) ||
        (event_ == EN_Event::SILENCE_DETECTED))
    {
        switch (status_)
        {
        case EN_StatusInWAIT::IDLE:
            if (CChannel::HartEnabledChanged == EN_Bool::TRUE8)
            {
                if (CChannel::IsHartEnabled() == EN_Bool::TRUE8)
                {
                    // Confirm
                    CChannel::HartEnabledChanged = EN_Bool::FALSE8;
                    return EN_Event::ENABLE_indicate;
                }
            }

            break;

        case EN_StatusInWAIT::RECEIVING:
            if (CChannel::HartEnabledChanged == EN_Bool::TRUE8)
            {
                if (CChannel::IsHartEnabled() == EN_Bool::FALSE8)
                {
                    // Confirm
                    CChannel::HartEnabledChanged = EN_Bool::FALSE8;
                    return EN_Event::DISABLE_indicate;
                }
            }

            if (CChannel::BurstModeChanged == EN_Bool::TRUE8)
            {
                if (CChannel::IsBurstMode() == EN_Bool::FALSE8)
                {
                    // Confirm
                    CChannel::BurstModeChanged = EN_Bool::FALSE8;
                    return EN_Event::DISABLE_BURST_MODE;
                }
                else
                {
                    // Confirm
                    CChannel::BurstModeChanged = EN_Bool::FALSE8;
                    return EN_Event::ENABLE_BURST_MODE;
                }
            }

            break;

        case EN_StatusInWAIT::WAIT_STO:
            // tbd
            break;

        case EN_StatusInWAIT::WAIT_XMIT_BACK:
            // tbd
            break;
        }
    }

    return result;
}

// Helpers
void CHSipL2SM::EnableBurst()
{
    if (CService::ActiveService != NULL)
    {
        if (CService::ActiveService->Request.Host != CService::Burst.Host)
        {
            CService::Burst.Host = CService::ActiveService->Request.Host;
        }
    }

    m_burst = EN_Bool::TRUE8;
    mo_BT_timer.Start(GetRT1prim());
    CBurst::Launch();
    // The toggle here is required because the first
    // burst is directly appended to the command service
    // which enabled the burst mode
    CService::Burst.Host = (CService::Burst.Host == EN_Master::PRIMARY) ? EN_Master::SECONDARY : EN_Master::PRIMARY;
}

void CHSipL2SM::DisableBurst()
{
    m_burst = EN_Bool::FALSE8;
    mo_BT_timer.Stop();
}

// Time out values
TY_DWord CHSipL2SM::GetSTO()
{
    if (CChannel::GetBaudrate() > COSAL::CBitRates::CBR_Win::BR_4800)
    {
        return CUsedTimeOuts::STO_SHORT;
    }
    
    return CUsedTimeOuts::STO_NORMAL - CUsedTimeOuts::STO_MARGIN;
}

TY_DWord CHSipL2SM::GetRT1prim()
{
    if (CChannel::GetBaudrate() > COSAL::CBitRates::CBR_Win::BR_4800)
    {
        return CUsedTimeOuts::PRIM_RT1_SHORT;
    }

    return CUsedTimeOuts::PRIM_RT1_NORMAL;
}

TY_DWord CHSipL2SM::GetRT1(EN_Master master_)
{
    if (master_ == EN_Master::PRIMARY)
    {
        // Primary master
        if (CChannel::GetBaudrate() > COSAL::CBitRates::CBR_Win::BR_4800)
        {
            return CUsedTimeOuts::PRIM_RT1_SHORT;
        }

        return CUsedTimeOuts::PRIM_RT1_NORMAL;
    }
    else
    {
        // Secondary master
        if (CChannel::GetBaudrate() > COSAL::CBitRates::CBR_Win::BR_4800)
        {
            return CUsedTimeOuts::SCND_RT1_SHORT;
        }

        return CUsedTimeOuts::SCND_RT1_NORMAL;
    }
}

TY_DWord CHSipL2SM::GetRT2()
{
    if (CChannel::GetBaudrate() > COSAL::CBitRates::CBR_Win::BR_4800)
    {
        return CUsedTimeOuts::RT2_SHORT;
    }

    return CUsedTimeOuts::RT2_NORMAL;
}

TY_DWord CHSipL2SM::GetHold()
{
    if (CChannel::GetBaudrate() > COSAL::CBitRates::CBR_Win::BR_4800)
    {
        return CUsedTimeOuts::HOLD_SHORT;
    }

    return CUsedTimeOuts::HOLD_NORMAL;
}

// CRxIpSM
// RCV_MSG Receive state machine
// see: https://library.fieldcommgroup.org/20081/TS20081/9.1/#page=32
//
//                          ---------------  
//                          Set Gap Timeout   
//                                  |
//                         +--------V-------+  Gap Timeout |~ENABLE.indicate
//                         |                |  -----------------------------
//                         |  Wait For SOM  |  RCV_MSG = ERR
//                         |                +----------------->
//                         +--------+-------+
//                                  |  Valid SOM
//                                  |  -----------------
//                                  |  Set Header Length
//                                  |  Set RCV_MSG type
//  Byte Count Comm Error  +--------V-------+
//  ---------------------  |                |
//           +-------------+    Rx Header   +-------------+
//           |             |                |             |
//           |             +--------+-------+             |
//           |                      |                     |
//  +--------V-------+              |            +--------V-------+
//  |                |              |            |   Rx Data and  |
//  |  Wait For End  +------------->|<-----------+   Check Byte   |
//  |                |              |            |                |
//  +----------------+              |            +--------+-------+
//                                  |                     |
//                                  |                     V
//                                  |            Received Check Byte
//                                  |            -------------------
//                                  V
//                       Gap Timeout |~ENABLE.indicate
//                       -----------------------------
//                       RCV_MSG = ERR
//  ----------------------------------------------------------------
//  SOM = Start Of Message

// Data
CHSipL2RxSM::EN_Status CHSipL2RxSM::m_status;

// Initialization
void CHSipL2RxSM::Init()
{
    m_status = EN_Status::IDLE;
    CHSipProtocol::Init();
}

// The receiver does not provide an explicit state machine (event handler)
// The required operations are realized in the module
// HSUartProtocol.cpp.

// CTxIpSM
// XMT_MSG Transmit State Machine
// see: https://library.fieldcommgroup.org/20081/TS20081/9.1/#page=34
//
//                                  ENABLE.request
//                                  -------------- 
//                                        |
//                               +--------V---------+
//                               |                  |
//                               |    Initialize    |
//                               |  Physical Layer  |
//                               |                  |
//                               +------------------+
//                                        |  ENABLE.confirm
//                                        |  --------------
//                                        |  DATA.request
//                               +--------V---------+        ERROR.indicate
//                               |                  |        --------------
//              +--------------->|      Write       +----->  XMT_MSG = ERR
//              |                |                  |        ~ENABLE.request
//  DATA.confirm && ~LastByte    +--+------------+--+
//  -------------------------       |            |
//  DATA.request                    |            V
//              |                   |     DATA.confirm && LastByte 
//              +-------------------+     ------------------------
//                                        XMT_MSG = OK
//                                        ~ENABLE.request
//  ------------------------------------------------------------------------
//  Note, Walter Borst:
//  Normally it is not possible to detect an error in the hardware of a
//  transmitter. That's why my implementation works time-dependently.
//  However, the actual writing of the bytes can be found in the MAC (Medium
//  Access Control), HSMacPort.cpp:
//  CWinSys::CUart::Tx(tx_data, tx_len);

// Data
CHSipL2TxSM::EN_Status CHSipL2TxSM::m_status = EN_Status::IDLE;
TY_Word          CHSipL2TxSM::m_len = 0;
COSAL::CTimer    CHSipL2TxSM::m_timer;

// Methods 
// Initialization
void CHSipL2TxSM::Init()
{
    m_timer.InitNoneStatic();
    m_status = EN_Status::IDLE;
}

// Operation
CHSipProtocol::EN_ToDo CHSipL2TxSM::EventHandler()
{
    switch (m_status)
    {
    case EN_Status::START_TX_RESPONSE:
        m_status = EN_Status::WAIT_TX_END;
        return CHSipProtocol::EN_ToDo::SEND_RESPONSE;
        break;
    case EN_Status::START_TX_BURST:
        m_status = EN_Status::WAIT_TX_END;
        return CHSipProtocol::EN_ToDo::SEND_BURST;
        break;
    case EN_Status::WAIT_TX_END:
        m_status = EN_Status::IDLE;
        return CHSipProtocol::EN_ToDo::END_TRANSMIT;
        break;
    }
    return CHSipProtocol::EN_ToDo::NOTHING;
}

CHSipL2TxSM::EN_Status CHSipL2TxSM::GetStatus()
{
    return m_status;
}

void CHSipL2TxSM::SetStatus(CHSipL2TxSM::EN_Status status_)
{
    m_status = status_;
}

void CHSipL2TxSM::SetTxLen(TY_Word len_)
{
    m_len = len_;
}

