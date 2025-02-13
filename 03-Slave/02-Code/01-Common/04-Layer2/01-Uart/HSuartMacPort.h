/*
 *          File: HSuartMacPort.h (CHSuartMacPort)
 *                The interface to the MAC port is relatively small
 *                and can be defined generically. However, the implementation
 *                depends on the hardware and software environment.
 *                That's why there is only a header at this point, while
 *                the file HSMacPort.cpp can be found in the specific branch.
 *                The present module is executed by the fast cyclic handler.
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
#ifndef __hsuartmacport_h__
#define __hsuartmacport_h__

class CHSuartMacPort
{
public:
    // Enum classes
    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        RECEIVING = 1,
        TRANSMITTING = 2
    };

    enum class EN_ToDo : TY_Byte
    {
        NOTHING = 0,
        CARRIER_ON = 1,
        CARRIER_OFF = 2,
        SEND_DATA = 3,
        RECEIVE_ENABLE = 4,
        RECEIVE_DISABLE = 5,
        RESET_RECEIVER = 6
    };

    static EN_Bool   Open(TY_Word port_, TY_DWord baudrate_, EN_CommType type_);
    static void     Close();
    static void   Execute(TY_Word time_ms_);
    static void      Init();

    static TY_Byte    m_rx_buffer[MAX_TXRX_SIZE];
    static TY_Byte    m_rx_len;
    static TY_Byte    m_rx_err;
    static EN_Status  m_status;
};
#endif // __hsuartmacport_h__