/*
 *          File: HSuartProtocol.h (CHSuartProtocol)
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
#ifndef __hsuartprotocol_h__
#define __hsuartprotocol_h__

class CHSuartProtocol
{
public:
    enum class EN_Event : TY_Byte
    {
        NONE = 0,
        RX_BYTE_RECEIVED = 1,
        SILENCE_DETECTED = 2
    };

    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        RECEIVING = 2,
        WAIT_GAP = 3,
        FINALIZE_RECEIVER = 4,
        TRANSMITTING = 5
    };

    enum class EN_ToDo : TY_Byte
    {
        NOTHING = 0,
        RECEIVE_ENABLE = 1,
        RESET_RECEIVER = 2,
        RECEIVE_DISABLE = 3,
        START_TRANSMIT = 4,
        SEND_DATA = 5,
        END_TRANSMIT = 6,
        WAIT_TX_END = 7,
        SIGNAL_ACTIVITY = 8,
        SIGNAL_SIELENCE = 9
    };

    // Data
    static CFrame    WorkFrame;
 
    // Methods
    static void                        Init();
    static CHSuartMacPort::EN_ToDo EventHandler(CHSuartProtocol::EN_Event event_, TY_Byte* rx_tx_bytes_, TY_Byte rx_tx_len, TY_Byte rx_tx_err);

private:
    // Data
    static EN_Status     m_status;
    static TY_Byte*      m_tx_data_ref;
    static TY_Len        m_tx_len;
    static COSAL::CTimer m_gap_timer;

    // Methods
    static void          StartGapTimer(TY_Byte num_characters_);
};
#endif // __hsuartprotocol_h__