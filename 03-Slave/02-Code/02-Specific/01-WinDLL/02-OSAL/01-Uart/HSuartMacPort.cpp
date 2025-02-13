/*
 *          File: HSMacPort.cpp
 *                The Execute method is called directly by the fast cyclic
 *                handler. This basically drives all status machines in
 *                the Hart implementation. Here too, the method is divided
 *                into an Event handler and a ToDo handler.
 *                This class is very close to the physics. It is the holder
 *                of the receive buffer and it manages the CD handshake.
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

#include <Windows.h>

#include "WbHartSlave.h"
#include "HSuartMacPort.h"
#include "WinSystem.h"
#include "HSuartProtocol.h"
#include "HSuartLayer2.h"

// Data

CHSuartMacPort::EN_Status CHSuartMacPort::m_status = CHSuartMacPort::EN_Status::IDLE;
TY_Byte            CHSuartMacPort::m_rx_buffer[MAX_TXRX_SIZE];
TY_Byte               CHSuartMacPort::m_rx_len = 0;
TY_Byte               CHSuartMacPort::m_rx_err = 0;

// Methods

void CHSuartMacPort::Execute(TY_Word time_ms_)
{
    // Note: This procedure is called every ms as long as the channel is open

    CHSuartMacPort::EN_ToDo to_do = CHSuartMacPort::EN_ToDo::NOTHING;
    TY_Len                len = 0;

    COSAL::CTimer::UpdateTime(time_ms_);

    switch (m_status)
    {
    case EN_Status::IDLE:
        to_do = CHSuartProtocol::EventHandler(CHSuartProtocol::EN_Event::NONE, m_rx_buffer, m_rx_len, m_rx_err);
        break;
    case EN_Status::RECEIVING:

        if (CWinSys::CUart::Rx(m_rx_buffer, &m_rx_len, &m_rx_err) == EN_Bool::TRUE8)
        {
            to_do = CHSuartProtocol::EventHandler(CHSuartProtocol::EN_Event::RX_BYTE_RECEIVED, m_rx_buffer, m_rx_len, m_rx_err);
        }
        else
        {
            to_do = CHSuartProtocol::EventHandler(CHSuartProtocol::EN_Event::NONE, m_rx_buffer, m_rx_len, m_rx_err);
        }
        break;
    case EN_Status::TRANSMITTING:
        to_do = CHSuartProtocol::EventHandler(CHSuartProtocol::EN_Event::NONE, m_rx_buffer, m_rx_len, m_rx_err);
        break;
    }

    switch (to_do)
    {
    case EN_ToDo::NOTHING:
        break;
    case EN_ToDo::CARRIER_ON:
        if (CWinSys::CUart::IsCarrierOn() == EN_Bool::FALSE8)
        {
            CWinSys::CUart::SetCarrierOn();
            m_status = EN_Status::TRANSMITTING;
            COSAL::Wait(1);
        }
        break;
    case EN_ToDo::CARRIER_OFF:
        if (CWinSys::CUart::IsCarrierOn() == EN_Bool::TRUE8)
        {
            CWinSys::CUart::SetCarrierOff();
        }

        m_status = EN_Status::IDLE;
        break;
    case EN_ToDo::SEND_DATA:
        {
            CWinSys::CUart::Tx(CFrame::TxBufferBytes, CFrame::TxBufferLen);
        }
        break;
    case EN_ToDo::RECEIVE_ENABLE:
        if (m_status != EN_Status::RECEIVING)
        {
            if (CWinSys::CUart::IsCarrierOn() == EN_Bool::TRUE8)
            {
                CWinSys::CUart::SetCarrierOff();
            }

            m_rx_len = 0;
            m_rx_err = 0;
            m_status = EN_Status::RECEIVING;
        }
        break;
    case EN_ToDo::RESET_RECEIVER:
        if (CWinSys::CUart::IsCarrierOn() == EN_Bool::TRUE8)
        {
            CWinSys::CUart::SetCarrierOff();
        }

        m_rx_len = 0;
        m_rx_err = 0;
        m_status = EN_Status::RECEIVING;
        break;
    case EN_ToDo::RECEIVE_DISABLE:
        m_status = EN_Status::IDLE;
        break;
    }
}

EN_Bool CHSuartMacPort::Open(TY_Word port_, TY_DWord baudrate_, EN_CommType type_)
{
    if ((port_ == 0) || (port_ > 254))
    {
        // Only port numbers in the range 1..254 are supported
        return EN_Bool::FALSE8;
    }

    if (CWinSys::CUart::Open((TY_Byte)port_, baudrate_) == EN_Bool::TRUE8)
    {
        // Start the thread for the cyclic handler
        CWinSys::CyclicTaskStart();
        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}

void CHSuartMacPort::Close()
{
    CWinSys::CyclicTaskTerminate();
    CWinSys::CUart::Close();
}

void CHSuartMacPort::Init()
{
    CHSuartL2SM::Init();
}

