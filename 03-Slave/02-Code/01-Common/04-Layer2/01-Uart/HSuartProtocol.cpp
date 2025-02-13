/*
 *          File: HSuartProtocol.cpp (CHSuartProtocol)
 *                This protocol layer controls the UART interface on the
 *                lower level and calls the higher status machines when
 *                necessary (events). After this call, a ToDo Part occurs,
 *                which in turn affects the Uart interface.
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
#include "HSuartMacPort.h"
#include "HSuartProtocol.h"
#include "HSuartLayer2.h"
#include "Monitor.h"
#include "HartChannel.h"

// Data
CHSuartProtocol::EN_Status CHSuartProtocol::m_status;
CFrame                     CHSuartProtocol::WorkFrame;
TY_Byte*                   CHSuartProtocol::m_tx_data_ref;
TY_Len                     CHSuartProtocol::m_tx_len;
COSAL::CTimer              CHSuartProtocol::m_gap_timer;

static TY_DWord debug_start;
static TY_DWord debug_now;

void CHSuartProtocol::Init()
{
    m_gap_timer.InitNoneStatic();
}

// Methods
CHSuartMacPort::EN_ToDo CHSuartProtocol::EventHandler(EN_Event event_, TY_Byte* rx_tx_bytes_, TY_Byte rx_tx_len_, TY_Byte rx_tx_err_)
{
    TY_DWord time = COSAL::CTimer::GetTime();

    CHSuartMacPort::EN_ToDo    mac_to_do = CHSuartMacPort::EN_ToDo::NOTHING;
    EN_ToDo                     me_to_do = EN_ToDo::NOTHING;
    CFrame::EN_Type           frame_type = CFrame::EN_Type::NO_HART_IP;
    // Debug
    TY_DWord                        diff = 0;

    if (rx_tx_len_ > 0)
    {
        me_to_do = EN_ToDo::NOTHING;
    }

    switch (m_status)
    {
    case EN_Status::IDLE:
        me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::NONE);
        break;
    case EN_Status::RECEIVING:
        if (event_ == EN_Event::RX_BYTE_RECEIVED)
        {
            if (rx_tx_len_ == 1)
            {
                CMonitor::StartReceive(COSAL::CTimer::GetTime());
                // Just to be sure
                CHSuartProtocol::WorkFrame.Init();
                // Just inform Hart layer 2 that the first byte was received
                me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::ACTIVITY_DETECTED);
            }
            
            if (CHSuartProtocol::WorkFrame.ParseByte(rx_tx_bytes_[rx_tx_len_ - 1], rx_tx_err_, EN_Bool::FALSE8) == EN_Bool::TRUE8)
            {
                m_status = EN_Status::WAIT_GAP;
            }

            StartGapTimer(3);
            debug_start = COSAL::CTimer::GetTime();
        }
        else
        {
            if (m_gap_timer.IsActive() == EN_Bool::FALSE8)
            {
                StartGapTimer(3);
            }

            if (m_gap_timer.IsExpired() == EN_Bool::TRUE8)
            {
                if (rx_tx_len_ > 0)
                {
                    CHSuartProtocol::WorkFrame.ParseByte(rx_tx_bytes_[rx_tx_len_ - 1], rx_tx_err_, EN_Bool::TRUE8);
                    m_status = EN_Status::FINALIZE_RECEIVER;
                }

                me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::SILENCE_DETECTED);
            }
        }
        break;
    case EN_Status::WAIT_GAP:
        if(event_ == EN_Event::RX_BYTE_RECEIVED)
        {
            // Continue receiver
            CHSuartProtocol::WorkFrame.ParseByte(rx_tx_bytes_[rx_tx_len_ - 1], rx_tx_err_, EN_Bool::FALSE8);
            StartGapTimer(3);
            me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::SILENCE_DETECTED);
        }

        if (m_gap_timer.IsExpired() == EN_Bool::TRUE8)
        {
            m_status = EN_Status::FINALIZE_RECEIVER;
        }

        break;
    case EN_Status::FINALIZE_RECEIVER:
        debug_now = COSAL::CTimer::GetTime();
        diff = debug_now - debug_start;

        frame_type = CHSuartProtocol::WorkFrame.Type;
        CMonitor::StoreData(rx_tx_bytes_, rx_tx_len_);

        switch (frame_type)
        {
        case CFrame::EN_Type::REQUEST:
            CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
            me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::RCV_MSG_STX);
            break;
        case CFrame::EN_Type::RESPONSE:
            CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
            me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::RCV_MSG_ACK);
            break;
        case CFrame::EN_Type::BURST:
            CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
            me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::RCV_MSG_BACK);
            break;
        case CFrame::EN_Type::JUNK:
            CMonitor::EndRcvGapTO(COSAL::CTimer::GetTime());
            me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::RCV_MSG_ERR);
            CHSuartProtocol::WorkFrame.Init();
            break;
        default:
            // Enable receiver anew
            me_to_do = EN_ToDo::RECEIVE_ENABLE;
            break;
        }

        break;
    case EN_Status::TRANSMITTING:
        me_to_do = CHSuartL2TxSM::EventHandler();
        break;
    }

    // Nothings to do? Call the Hart SM to ask
    // if something is pending
    if (me_to_do == EN_ToDo::NOTHING)
    {
        me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::NONE);
    }

    while (me_to_do != EN_ToDo::NOTHING)
    {
        switch (me_to_do)
        {
        case EN_ToDo::NOTHING:
            break;
        case EN_ToDo::RECEIVE_ENABLE:
            me_to_do = EN_ToDo::NOTHING;
            mac_to_do = CHSuartMacPort::EN_ToDo::RECEIVE_ENABLE;
            if (m_status != EN_Status::RECEIVING)
            {
                m_status = EN_Status::RECEIVING;
            }
            break;
        case EN_ToDo::RESET_RECEIVER:
            me_to_do = EN_ToDo::NOTHING;
            mac_to_do = CHSuartMacPort::EN_ToDo::RESET_RECEIVER;
            if (m_status != EN_Status::RECEIVING)
            {
                m_status = EN_Status::RECEIVING;
            }
            break;
        case EN_ToDo::RECEIVE_DISABLE:
            me_to_do = EN_ToDo::NOTHING;
            mac_to_do = CHSuartMacPort::EN_ToDo::RECEIVE_DISABLE;
            if (m_status != EN_Status::IDLE)
            {
                m_status = EN_Status::IDLE;
            }
            break;
        case EN_ToDo::START_TRANSMIT:
            me_to_do = EN_ToDo::NOTHING;
            m_tx_data_ref = CFrame::TxBufferBytes;
            m_tx_len = CFrame::TxBufferLen;
            if ((m_tx_data_ref != 0) && (m_tx_len > 0))
            {
                CHSuartL2TxSM::SetTxLen(m_tx_len);
                CHSuartL2TxSM::SetStatus(CHSuartL2TxSM::EN_Status::START_TX);
                m_status = EN_Status::TRANSMITTING;
                mac_to_do = CHSuartMacPort::EN_ToDo::CARRIER_ON;
            }
            else
            {
                //CHartSM::SetActiveServiceFailed();
                m_status = EN_Status::RECEIVING;
                mac_to_do = CHSuartMacPort::EN_ToDo::CARRIER_OFF;
                me_to_do = EN_ToDo::NOTHING;
            }
            break;
        case EN_ToDo::SEND_DATA:
            me_to_do = EN_ToDo::NOTHING;
            mac_to_do = CHSuartMacPort::EN_ToDo::SEND_DATA;
            if (m_tx_len > 0)
            {
                CMonitor::StartTransmit(COSAL::CTimer::GetTime() + 1);
                CMonitor::StoreData(m_tx_data_ref, m_tx_len);
            }
            break;
        case EN_ToDo::END_TRANSMIT:
            CMonitor::EndTransmit(COSAL::CTimer::GetTime() - 1);
            me_to_do = CHSuartL2SM::EventHandler(CHSuartL2SM::EN_Event::XMT_MSG_done);
            if (me_to_do == EN_ToDo::RECEIVE_ENABLE)
            {
                mac_to_do = CHSuartMacPort::EN_ToDo::RECEIVE_ENABLE;
                m_status = EN_Status::RECEIVING;
                me_to_do = EN_ToDo::NOTHING;
            }
            else
            {
                mac_to_do = CHSuartMacPort::EN_ToDo::CARRIER_OFF;
                m_status = EN_Status::IDLE;
            }
            break;
        default:
            me_to_do = EN_ToDo::NOTHING;
            break;
        }
    }
    return mac_to_do;
}

// Helpers
void CHSuartProtocol::StartGapTimer(TY_Byte num_characters)
{
    if (num_characters > 0)
    {
        TY_DWord gap_time = (TY_DWord)((num_characters + 1) * COSAL::CTimer::GetByteTime(CChannel::GetBaudrate()));

        m_gap_timer.Start(gap_time);
    }
    else
    {
        TY_DWord gap_time = (TY_DWord)(3 * COSAL::CTimer::GetByteTime(CChannel::GetBaudrate()));

        m_gap_timer.Start(gap_time);
    }
}

