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

// Once
#ifndef __hsipprotocol_h__
#define __hsipprotocol_h__

class CHSipProtocol
{
public:
    enum class EN_Event : TY_Byte
    {
        NONE = 0,
        HART_IP_DATA_RECEIVED = 1,
        HART_IP_NO_DATA_RECEIVED = 2,
        HART_IP_TX_DONE = 3,
        SILENCE_DETECTED = 4,
        NETWORK_ERROR = 5
    };

    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        RECEIVING = 2,
        WAIT_RESPONSE = 3,
        TRANSMITTING = 4
    };

    enum class EN_ToDo : TY_Byte
    {
        NOTHING = 0,
        RECEIVE_ENABLE = 1,
        RESET_RECEIVER = 2,
        RECEIVE_DISABLE = 3,
        START_TX_RESPONSE = 4,
        START_TX_BURST = 5,
        SEND_RESPONSE = 6,
        SEND_BURST = 7,
        END_TRANSMIT = 8,
        WAIT_TX_END = 9,
        SIGNAL_ACTIVITY = 10,
        SIGNAL_SIELENCE = 11
    };

    // Data
    static CFrame    WorkFrame;
 
    // Methods
    static void                                        Init();
    static CHSipMacPort::EN_ToDo               EventHandler(CHSipProtocol::EN_Event event_, TY_Byte* rx_tx_bytes_, TY_Word rx_tx_len, TY_Byte rx_tx_err);
    static TY_Byte*                               GetTxData(TY_Word* tx_len_);
    static EN_ToDo                HandleHartIpPayloadPacket(TY_Byte* rx_tx_bytes_, TY_Word rx_tx_len_);
private:
    // Data
    static EN_Status     m_status;
    static TY_Byte*      m_tx_data;
    static TY_Len        m_tx_len;
};
#endif // __hsipprotocol_h__