/*
 *          File: HMuartMacPort.h (CHMuartMacPort)
 *                The interface to the MAC port is relatively small
 *                and can be defined generically. However, the implementation
 *                depends on the hardware and software environment.
 *                That's why there is only a header at this point, while
 *                the file HMuartMacPort.cpp can be found in the specific branch.
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
#ifndef __hmuartmacport_h__
#define __hmuartmacport_h__

#include "OSAL.h"
#include "WbHartUser.h"

class CHMuartMacPort
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
        SEND_REQUEST = 3,
        RECEIVE_ENABLE = 4,
        RECEIVE_DISABLE = 5
    };

    static EN_Bool   Open(TY_Word port_, TY_DWord baudrate_, EN_CommType type_);
    static void     Close();
    static void   Execute(TY_Word time_ms_);
    static void      Init();

private:
    static ST_RcvByte m_loc_rcv_buf[MAX_TXRX_SIZE];

public:
    static EN_Status  Status;
};
#endif // __hmuartmacport_h__