/*
 *          File: HMuartMacPort.cpp (CHMuartMacPort)
 *                The Execute method is called directly by the fast cyclic
 *                handler. This basically drives all status machines in
 *                the Hart implementation. Here too, the method is divided
 *                into an Event handler and a ToDo handler.
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

#include "HMuartMacPort.h"
#include "WinSystem.h"
#include "HMuartProtocol.h"
#include "HMuartLayer2.h"

// Data

CHMuartMacPort::EN_Status CHMuartMacPort::Status = CHMuartMacPort::EN_Status::IDLE;
ST_RcvByte            CHMuartMacPort::m_loc_rcv_buf[MAX_TXRX_SIZE];

// Methods

void CHMuartMacPort::Execute(TY_Word time_ms_)
{
    // Note: This procedure is called every ms as long as the channel is open

    CHMuartMacPort::EN_ToDo to_do = CHMuartMacPort::EN_ToDo::NOTHING;
    TY_Len                 len = 0;

    COSAL::CTimer::UpdateTime(time_ms_);

    switch (Status)
    {
    case EN_Status::IDLE:
        to_do = CHMuartProtocol::EventHandler(CHMuartProtocol::EN_Event::NONE, NULL, 0);
        break;
    case EN_Status::RECEIVING:
        len = CWinSys::CUart::Rx(MAX_TXRX_SIZE, m_loc_rcv_buf);
        if (len > 0)
        {
            to_do = CHMuartProtocol::EventHandler(CHMuartProtocol::EN_Event::NEW_RCV_DATA, m_loc_rcv_buf, len);
        }
        else
        {
            to_do = CHMuartProtocol::EventHandler(CHMuartProtocol::EN_Event::NONE, NULL, 0);
        }
        break;
    case EN_Status::TRANSMITTING:
        to_do = CHMuartProtocol::EventHandler(CHMuartProtocol::EN_Event::NONE, NULL, 0);
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
            Status = EN_Status::TRANSMITTING;
            COSAL::Wait(1);
        }
        break;
    case EN_ToDo::CARRIER_OFF:
        if (CWinSys::CUart::IsCarrierOn() == EN_Bool::TRUE8)
        {
            CWinSys::CUart::SetCarrierOff();
        }

        Status = EN_Status::IDLE;
        break;
    case EN_ToDo::SEND_REQUEST:
        {
        TY_Word tx_len;
        TY_Byte* tx_data = CHMuartL2SM::GetTxData(&tx_len);
        CWinSys::CUart::Tx(tx_data, tx_len);
        }
    break;
    case EN_ToDo::RECEIVE_ENABLE:
        if (Status != EN_Status::RECEIVING)
        {
            if (CWinSys::CUart::IsCarrierOn() == EN_Bool::TRUE8)
            {
                CWinSys::CUart::SetCarrierOff();
            }

            Status = EN_Status::RECEIVING;
        }
        break;
    case EN_ToDo::RECEIVE_DISABLE:
        Status = EN_Status::IDLE;
        break;
    }
}

EN_Bool CHMuartMacPort::Open(TY_Word port_, TY_DWord baudrate_, EN_CommType type_)
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

void CHMuartMacPort::Close()
{
    CWinSys::CyclicTaskTerminate();
    CWinSys::CUart::Close();
}

void CHMuartMacPort::Init()
{
    CHMuartL2SM::Init();
}

