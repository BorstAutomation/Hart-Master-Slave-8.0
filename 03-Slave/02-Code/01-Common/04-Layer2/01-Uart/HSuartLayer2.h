/*
 *          File: HSuartLayer2.h (CHSuartL2SM, CHSuartL2RxSM, CHSuartL2TxSM)
 *                This module implements the state machine of the
 *                Hart communication protocol (CHSuartL2SM) including the state
 *                machines for sending (CHSuartL2TxSM) and receiving
 *                (CHSuartL2TxSM) bytes.
 *                After the call, the status machine returns a ToDo code
 *                that tells the caller what to do next.
 *                Note: This is the implementation of a slave.
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
#ifndef __hsuartlayer2_h__
#define __hsuartlayer2_h__

class CHSuartL2SM
{
public:
    // Enum classes
    enum class EN_Event : TY_Byte
    {
        NONE = 0,                  // Just to be replace by events
                                   // which are created implicitly.
        ENABLE_indicate = 1,
        DISABLE_BURST_MODE = 2,
        RCV_MSG_ERR = 3,
        RCV_MSG_ACK = 4,
        BURST_BT_EXPIRED = 5, 
        ENABLE_BURST_MODE = 6,
        RCV_MSG_STX = 7,
        RCV_MSG_STX_NoMatch = 8,
        RCV_MSG_STX_Match = 9,
        RCV_MSG_CommError = 10,
        RCV_MSG_BACK = 11,
        XMT_MSG_done = 12,
        DISABLE_indicate = 13,
        ACTIVITY_DETECTED = 14,
        SILENCE_DETECTED = 15
    };

    enum class EN_Status : TY_Byte
    {
        WAIT = 0,
        PROCESS = 1
    };

    enum class EN_StatusInWAIT : TY_Byte
    {
        IDLE = 0,
        RECEIVING = 1,
        WAIT_STO = 2,
        WAIT_XMIT_BACK = 3
    };

    enum class EN_StatusInPROCESS : TY_Byte
    {
        NOT_SET = 0,
        WAIT_STO = 1,
        WAIT_TX = 2,
        WAIT_XMIT_BACK = 3
    };

    // Initialization
    static void                          Init();
    // Operation
    static CHSuartProtocol::EN_ToDo  EventHandler(EN_Event event_);
    static CHSuartL2SM::EN_Event     GetSoftEvent(EN_StatusInWAIT status_, EN_Event event_);

    // State handling
    static EN_Status        HandleStatus_WAIT(EN_Event event_, CHSuartProtocol::EN_ToDo* to_do_);
    static EN_Status    HandleReceivingInWAIT(EN_Event event_, CHSuartProtocol::EN_ToDo* to_do_);
    static EN_Status     HandleStatus_PROCESS(EN_Event event_, CHSuartProtocol::EN_ToDo* to_do_);
    // Helpers
    static void                   EnableBurst();
    static void                  DisableBurst();
    // Time out values
    static TY_DWord                    GetSTO();
    static TY_DWord                GetRT1prim();
    static TY_DWord                    GetRT1(EN_Master master_);
    static TY_DWord                    GetRT2();
    static TY_DWord                   GetHold();

    // Nested classes
    class CUsedTimeOuts
    {
    public:
        // Times as milliseconds
        static const TY_DWord          STO_NORMAL = 270;
        static const TY_DWord           STO_SHORT =  32;
        static const TY_DWord          STO_MARGIN =  40;
        static const TY_DWord     PRIM_RT1_NORMAL = 305;
        static const TY_DWord     SCND_RT1_NORMAL = 380;
        static const TY_DWord      PRIM_RT1_SHORT =  32;
        static const TY_DWord      SCND_RT1_SHORT =  35;
        static const TY_DWord          RT2_NORMAL =  75;
        static const TY_DWord           RT2_SHORT =   3;
        static const TY_DWord         HOLD_NORMAL =  20;
        static const TY_DWord          HOLD_SHORT =   3;
    };
private:
    static EN_Status           m_status;
    static EN_StatusInWAIT     m_status_in_WAIT;
    static EN_StatusInPROCESS  m_status_in_PROCESS;

    //-- State machine variables
    static EN_Bool         m_burst;
    static COSAL::CTimer   mo_BT_timer;
    static COSAL::CTimer   mo_Tx_timer;
};

class CHSuartL2RxSM
{
public:
    // Enum classes
    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        RECEIVING = 1,
        WAIT_SIELENCE = 2
    };

    // Initialization
    static void                         Init();

private:
    static EN_Status       m_status;
};

class CHSuartL2TxSM
{
public:
    // Enum classes
    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        START_TX = 1,
        WAIT_TX_END = 2
    };

    // Initialization
    static void                         Init();
    // Operation
    static CHSuartProtocol::EN_ToDo EventHandler();
    static EN_Status               GetStatus();
    static void                    SetStatus(EN_Status status_);
    static void                     SetTxLen(TY_Word len_);

private:
    static EN_Status     m_status;
    static TY_Word       m_len;
    static COSAL::CTimer m_timer;
};

#endif // __hsuartlayer2_h__