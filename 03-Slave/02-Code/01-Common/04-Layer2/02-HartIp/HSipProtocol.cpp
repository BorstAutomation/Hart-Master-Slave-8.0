/*
 *          File: HSipProtocol.cpp (CHSipProtocol)
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
#include "HSipMacPort.h"
#include "HSipProtocol.h"
#include "HSipLayer2.h"
#include "Monitor.h"
#include "HartChannel.h"
#include "HartData.h"

// Data
CHSipProtocol::EN_Status  CHSipProtocol::m_status;
CFrame                    CHSipProtocol::WorkFrame;
TY_Byte*                  CHSipProtocol::m_tx_data;
TY_Len                    CHSipProtocol::m_tx_len;

static TY_DWord debug_start;
static TY_DWord debug_now;

void CHSipProtocol::Init()
{
}

// Methods
CHSipMacPort::EN_ToDo CHSipProtocol::EventHandler(EN_Event event_, TY_Byte* rx_tx_bytes_, TY_Word rx_tx_len_, TY_Byte rx_tx_err_)
{
    TY_DWord time = COSAL::CTimer::GetTime();

    CHSipMacPort::EN_ToDo    mac_to_do = CHSipMacPort::EN_ToDo::NOTHING;
    EN_ToDo                   me_to_do = EN_ToDo::NOTHING;
    // Debug
    TY_DWord                      diff = 0;

    if (rx_tx_len_ > 0)
    {
        me_to_do = EN_ToDo::NOTHING;
    }

    switch (m_status)
    {
    case EN_Status::IDLE:
        me_to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::NONE);
        break;
    case EN_Status::RECEIVING:
        if (event_ == EN_Event::HART_IP_DATA_RECEIVED)
        {
            me_to_do = HandleHartIpPayloadPacket(rx_tx_bytes_, rx_tx_len_);
        }
        else if (event_ == EN_Event::HART_IP_NO_DATA_RECEIVED)
        {
            me_to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::SILENCE_DETECTED);
        }
        else
        {
            me_to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::NONE);
        }

        break;
    case EN_Status::WAIT_RESPONSE:
        break;
    case EN_Status::TRANSMITTING:
        me_to_do = CHSipL2TxSM::EventHandler();
        break;
    }

    // Nothings to do? Call the Hart SM to ask
    // if something is pending
    if (me_to_do == EN_ToDo::NOTHING)
    {
        me_to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::NONE);
    }

    while (me_to_do != EN_ToDo::NOTHING)
    {
        switch (me_to_do)
        {
        case EN_ToDo::NOTHING:
            break;
        case EN_ToDo::RECEIVE_ENABLE:
            me_to_do = EN_ToDo::NOTHING;
            mac_to_do = CHSipMacPort::EN_ToDo::RECEIVE_ENABLE;
            if (m_status != EN_Status::RECEIVING)
            {
                m_status = EN_Status::RECEIVING;
            }
            break;
        case EN_ToDo::RESET_RECEIVER:
            me_to_do = EN_ToDo::NOTHING;
            //mac_to_do = CHIPSMacPort::EN_ToDo::RESET_RECEIVER;
            if (m_status != EN_Status::RECEIVING)
            {
                m_status = EN_Status::RECEIVING;
            }
            break;
        case EN_ToDo::RECEIVE_DISABLE:
            me_to_do = EN_ToDo::NOTHING;
            mac_to_do = CHSipMacPort::EN_ToDo::RECEIVE_DISABLE;
            if (m_status != EN_Status::IDLE)
            {
                m_status = EN_Status::IDLE;
            }
            break;
        case EN_ToDo::START_TX_RESPONSE:
            me_to_do = EN_ToDo::NOTHING;
            m_tx_data = CFrame::TxBufferBytes;
            m_tx_len = CFrame::TxBufferLen;
            if ((m_tx_data != 0) && (m_tx_len > 0))
            {
                CHSipL2TxSM::SetTxLen(m_tx_len);
                CHSipL2TxSM::SetStatus(CHSipL2TxSM::EN_Status::START_TX_RESPONSE);
                m_status = EN_Status::TRANSMITTING;
            }
            else
            {
                //CHartIpSM::SetActiveServiceFailed();
                m_status = EN_Status::RECEIVING;
                me_to_do = EN_ToDo::NOTHING;
            }
            break;
        case EN_ToDo::START_TX_BURST:
            me_to_do = EN_ToDo::NOTHING;
            m_tx_data = CFrame::TxBufferBytes;
            m_tx_len = CFrame::TxBufferLen;
            if ((m_tx_data != 0) && (m_tx_len > 0))
            {
                CHSipL2TxSM::SetTxLen(m_tx_len);
                CHSipL2TxSM::SetStatus(CHSipL2TxSM::EN_Status::START_TX_BURST);
                m_status = EN_Status::TRANSMITTING;
            }
            else
            {
                //CHartIpSM::SetActiveServiceFailed();
                m_status = EN_Status::RECEIVING;
                me_to_do = EN_ToDo::NOTHING;
            }
            break;
        case EN_ToDo::SEND_RESPONSE:
            me_to_do = EN_ToDo::NOTHING;
            mac_to_do = CHSipMacPort::EN_ToDo::SEND_RESPONSE;
            if (m_tx_len > 0)
            {
                TY_Byte tx_data[MAX_TXRX_SIZE];
                TY_Byte tx_len = 0;

                CHSipMacPort::SetMessageType(1);
                CMonitor::StartTransmit(COSAL::CTimer::GetTime() + 1);
                CHSipMacPort::GetIpFrameForMonitor(tx_data, &tx_len, m_tx_data, (TY_Byte)m_tx_len);
                CMonitor::StoreData(tx_data, tx_len);
            }

            break;
        case EN_ToDo::SEND_BURST:
            me_to_do = EN_ToDo::NOTHING;
            mac_to_do = CHSipMacPort::EN_ToDo::SEND_BURST;
            if (m_tx_len > 0)
            {
                TY_Byte tx_data[MAX_TXRX_SIZE];
                TY_Byte tx_len = 0;

                CHSipMacPort::SetMessageType(2);
                CMonitor::StartTransmit(COSAL::CTimer::GetTime() + 1);
                CHSipMacPort::GetIpFrameForMonitor(tx_data, &tx_len, m_tx_data, (TY_Byte)m_tx_len);
                CMonitor::StoreData(tx_data, tx_len);
            }

            break;
        case EN_ToDo::END_TRANSMIT:
            CMonitor::EndTransmit(COSAL::CTimer::GetTime() - 1);
            me_to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::XMT_MSG_done);
            if (me_to_do == EN_ToDo::RECEIVE_ENABLE)
            {
                mac_to_do = CHSipMacPort::EN_ToDo::RECEIVE_ENABLE;
                m_status = EN_Status::RECEIVING;
                me_to_do = EN_ToDo::NOTHING;
            }
            else
            {
                //mac_to_do = CHIPSMacPort::EN_ToDo::CARRIER_OFF;
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
TY_Byte* CHSipProtocol::GetTxData(TY_Word* tx_len_)
{
    *tx_len_ = m_tx_len;
    return m_tx_data;
}

CHSipProtocol::EN_ToDo CHSipProtocol::HandleHartIpPayloadPacket(TY_Byte* rx_tx_bytes_, TY_Word rx_tx_len_)
{
    TY_Byte                    rx_data[MAX_TXRX_SIZE];
    TY_Byte                     rx_len = 0;
    CFrame::EN_Type   frame_type = CFrame::EN_Type::NO_HART_IP;


    CMonitor::StartReceive(COSAL::CTimer::GetTime());
    // Just inform Hart layer 2 that the first byte (at least) was received
    EN_ToDo to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::ACTIVITY_DETECTED);

    CHSipProtocol::WorkFrame.NoPreamb = EN_Bool::TRUE8;
    // Note: In hart ip data is completed
    for (TY_Word i = 0; i < rx_tx_len_; i++)
    {
        CHSipProtocol::WorkFrame.ParseByte(rx_tx_bytes_[i], 0, EN_Bool::FALSE8);
    }

    CHSipMacPort::SetMessageType(0);
    CHSipMacPort::GetIpFrameForMonitor(rx_data, &rx_len, rx_tx_bytes_, (TY_Byte)rx_tx_len_);
    CMonitor::StoreData(rx_data, rx_len);

    frame_type = CHSipProtocol::WorkFrame.Type;
    switch (frame_type)
    {
    case CFrame::EN_Type::REQUEST:
        CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
        to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::RCV_MSG_STX);
        break;
    case CFrame::EN_Type::RESPONSE:
        CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
        to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::RCV_MSG_ACK);
        break;
    case CFrame::EN_Type::BURST:
        CMonitor::EndRcvValidFrame(COSAL::CTimer::GetTime());
        to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::RCV_MSG_BACK);
        break;
    case CFrame::EN_Type::JUNK:
        CMonitor::EndRcvGapTO(COSAL::CTimer::GetTime());
        to_do = CHSipL2SM::EventHandler(CHSipL2SM::EN_Event::RCV_MSG_ERR);
        CHSipProtocol::WorkFrame.Init();
        break;
    default:
        // Enable receiver anew
        to_do = EN_ToDo::RECEIVE_ENABLE;
        break;
    }

    return to_do;
}



