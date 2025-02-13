/*
 *          File: HMuartProtocol.h (CHMuartProtocol)
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

// Once
#ifndef __hmuartprotocol_h__
#define __hmuartprotocol_h__

#include "OSAL.h"
#include "WbHartUser.h"
#include "HartFrame.h"
#include "HMuartMacPort.h"

class CHMuartProtocol
{
public:
    enum class EN_Event : TY_Byte
    {
        NONE = 0,
        NEW_RCV_DATA = 1
    };

    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        RECEIVING = 2,
        TRANSMITTING = 3
    };

    enum class EN_ToDo : TY_Byte
    {
        NOTHING = 0,
        RECEIVE_ENABLE = 2,
        RECEIVE_DISABLE = 3,
        START_TRANSMIT = 4,
        SEND_REQUEST = 5,
        END_TRANSMIT = 6
    };

    static CHMuartMacPort::EN_ToDo EventHandler(CHMuartProtocol::EN_Event event_, ST_RcvByte* rx_bytes_, TY_Word len_);

private:
    static EN_Status Status;
    static CFrame    m_work_frame;
    static CFrame    m_junk_frame;
    static CFrame    m_request_frame;
    static CFrame    m_response_frame;
    static CFrame    m_burst_frame;
    static TY_Byte*  mpu8_TxData;
    static TY_Len    mu16_TxLen;
};
#endif // __hmuartprotocol_h__