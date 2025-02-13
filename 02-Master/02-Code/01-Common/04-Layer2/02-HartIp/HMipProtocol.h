/*
 *          File: HMipProtocol.h (CHMipProtocol)
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
#ifndef __hmipprotocol_h__
#define __hmipprotocol_h__

#include "OSAL.h"
#include "WbHartUser.h"
#include "HartFrame.h"
#include "HMipMacPort.h"

class CHMipProtocol
{
public:
    enum class EN_Event : TY_Byte
    {
        NONE = 0,
        HART_IP_DATA_RECEIVED = 1,
        HART_IP_TX_DONE = 2,
        SILENCE_DETECTED = 3,
        NETWORK_ERROR = 4,
        REQUEST_REJECTED = 5
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

    static CHMipMacPort::EN_ToDo              EventHandler(CHMipProtocol::EN_Event event_, TY_Byte* rx_bytes_, TY_Word len_);
    static TY_Byte*                              GetTxData(TY_Word* tx_len_);
    static EN_ToDo               HandleHartIpPayloadPacket(TY_Byte* rx_tx_bytes_, TY_Word rx_tx_len_);

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
#endif // __hmipprotocol_h__