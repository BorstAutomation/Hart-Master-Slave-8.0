/*
 *          File: HMuartProtocol.cpp (CHMuartProtocol)
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
#include "WbHartM_Structures.h"
#include "HMuartProtocol.h"
#include "HMuartMacPort.h"
#include "HMuartLayer2.h"
#include "Monitor.h"

/* CHMUartProtocol */
CHMuartProtocol::EN_Status CHMuartProtocol::Status;
CFrame    CHMuartProtocol::m_work_frame;
CFrame    CHMuartProtocol::m_junk_frame;
CFrame    CHMuartProtocol::m_request_frame;
CFrame    CHMuartProtocol::m_response_frame;
CFrame    CHMuartProtocol::m_burst_frame;
TY_Byte*  CHMuartProtocol::mpu8_TxData;
TY_Len    CHMuartProtocol::mu16_TxLen;

// Methods
CHMuartMacPort::EN_ToDo CHMuartProtocol::EventHandler(EN_Event event_, ST_RcvByte* rx_bytes_, TY_Word len_)
{
    TY_DWord time = COSAL::CTimer::GetTime();

    CHMuartMacPort::EN_ToDo parent_to_do = CHMuartMacPort::EN_ToDo::NOTHING;
    EN_ToDo                    to_do = EN_ToDo::NOTHING;

    if (len_ > 0)
    {
        to_do = EN_ToDo::NOTHING;
    }

    switch (Status)
    {
    case EN_Status::IDLE:
        to_do = CHMuartL2SM::EventHandler(CHMuartL2SM::EN_Event::NONE, NULL);
        break;
    case EN_Status::RECEIVING:
        if (event_ == EN_Event::NEW_RCV_DATA)
        {
            to_do = CHMuartL2RxSM::EventHandler(event_, &m_work_frame, rx_bytes_, len_, &m_junk_frame, &m_request_frame, &m_response_frame, &m_burst_frame);
            if (m_work_frame.NumPreambles > 1)
            {
                to_do = CHMuartL2SM::EventHandler(CHMuartL2SM::EN_Event::RX_DATA_DETECTED, &m_work_frame);
            }
        }
        else
        {
            to_do = CHMuartL2RxSM::EventHandler(event_, &m_work_frame, NULL, 0, &m_junk_frame, &m_request_frame, &m_response_frame, &m_burst_frame);
        }

        if (m_request_frame.IsActive() == EN_Bool::TRUE8)
        {
            to_do = CHMuartL2SM::EventHandler(CHMuartL2SM::EN_Event::RX_COMPLETED_REQ, &m_request_frame);
            m_request_frame.Uninit();
        }

        if (m_response_frame.IsActive() == EN_Bool::TRUE8)
        {
            to_do = CHMuartL2SM::EventHandler(CHMuartL2SM::EN_Event::RX_COMPLETED_RSP, &m_response_frame);
            m_response_frame.Uninit();
        }

        if (m_burst_frame.IsActive() == EN_Bool::TRUE8)
        {
            to_do = CHMuartL2SM::EventHandler(CHMuartL2SM::EN_Event::RX_COMPLETED_BST, &m_burst_frame);
            m_burst_frame.Uninit();
        }

        if (m_junk_frame.IsActive() == EN_Bool::TRUE8)
        {
            to_do = CHMuartL2SM::EventHandler(CHMuartL2SM::EN_Event::RX_COMPLETED_ERR, &m_junk_frame);
            m_junk_frame.Uninit();
        }

        if (to_do == EN_ToDo::START_TRANSMIT)
        {
            CHMuartL2RxSM::Reset();
        }
        else
        {
            to_do = CHMuartL2SM::EventHandler(CHMuartL2SM::EN_Event::NONE, NULL);
        }
        break;

    case EN_Status::TRANSMITTING:
        to_do = CHMuartL2TxSM::EventHandler();
        break;
    }

    while (to_do != EN_ToDo::NOTHING)
    {
        switch (to_do)
        {
        case EN_ToDo::NOTHING:
            break;
        case EN_ToDo::RECEIVE_ENABLE:
            to_do = EN_ToDo::NOTHING;
            parent_to_do = CHMuartMacPort::EN_ToDo::RECEIVE_ENABLE;
            if (Status != EN_Status::RECEIVING)
            {
                Status = EN_Status::RECEIVING;
            }
            break;
        case EN_ToDo::RECEIVE_DISABLE:
            to_do = EN_ToDo::NOTHING;
            parent_to_do = CHMuartMacPort::EN_ToDo::RECEIVE_DISABLE;
            if (Status != EN_Status::IDLE)
            {
                Status = EN_Status::IDLE;
            }
            break;
        case EN_ToDo::START_TRANSMIT:
            to_do = EN_ToDo::NOTHING;
            mpu8_TxData = CHMuartL2SM::GetTxData(&mu16_TxLen);
            if ((mpu8_TxData != 0) && (mu16_TxLen > 0))
            {
                CHMuartL2TxSM::SetTxLen(mu16_TxLen);
                CHMuartL2TxSM::SetStatus(CHMuartL2TxSM::EN_Status::START_TX);
                Status = EN_Status::TRANSMITTING;
                parent_to_do = CHMuartMacPort::EN_ToDo::CARRIER_ON;
            }
            else
            {
                CHMuartL2SM::SetActiveServiceFailed();
                Status = EN_Status::RECEIVING;
                parent_to_do = CHMuartMacPort::EN_ToDo::CARRIER_OFF;
                to_do = EN_ToDo::NOTHING;
            }
            break;
        case EN_ToDo::SEND_REQUEST:
            to_do = EN_ToDo::NOTHING;
            parent_to_do = CHMuartMacPort::EN_ToDo::SEND_REQUEST;
            if (mu16_TxLen > 0)
            {
                CMonitor::StartTransmit(COSAL::CTimer::GetTime() + 1);
                CMonitor::StoreData(mpu8_TxData, mu16_TxLen);
            }
            break;
        case EN_ToDo::END_TRANSMIT:
            CMonitor::EndTransmit(COSAL::CTimer::GetTime() - 1);
            to_do = CHMuartL2SM::EventHandler(CHMuartL2SM::EN_Event::TX_DONE, NULL);
            if (to_do == EN_ToDo::RECEIVE_ENABLE)
            {
                parent_to_do = CHMuartMacPort::EN_ToDo::RECEIVE_ENABLE;
                Status = EN_Status::RECEIVING;
                to_do = EN_ToDo::NOTHING;
            }
            else
            {
                parent_to_do = CHMuartMacPort::EN_ToDo::CARRIER_OFF;
                Status = EN_Status::IDLE;
            }
            break;
        default:
            to_do = EN_ToDo::NOTHING;
            break;
        }
    }
    return parent_to_do;
}


