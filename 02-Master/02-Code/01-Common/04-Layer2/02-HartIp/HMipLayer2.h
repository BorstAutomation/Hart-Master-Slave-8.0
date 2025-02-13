/*
 *          File: HMipLayer2.h (CHMipL2SM, CHMipL2RxSM and CHMipL2TxSM)
 *                This module implements the entire state machine of the
 *                Hart communication protocol (CHartSM) including the state
 *                machines for sending (CTxSM) and receiving (CRxSM) bytes.
 *                After the call, the status machine returns a ToDo code
 *                that tells the caller what to do next.
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
#ifndef __hmiplayer2_h__
#define __hmiplayer2_h__

#include "OSAL.h"
#include "WbHartUser.h"
#include "HartFrame.h"
#include "HartService.h"
#include "HMipProtocol.h"

class CHMipL2SM
{
public:
    // Enum classes
    enum class EN_Event : TY_Byte
    {
        NONE = 0,
        RX_DATA_DETECTED = 1,
        RX_COMPLETED_REQ = 2,
        RX_COMPLETED_RSP = 3,
        RX_COMPLETED_BST = 4,
        RX_COMPLETED_ERR = 5,
        TX_DONE = 6,
        GHOST_TX_STARTED = 7,
        GHOST_TX_DONE = 8
    };

    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        WATCHING = 1,
        ENABLED = 2,
        USING = 3
    };

    enum class EN_StatusInWATCHING : TY_Byte
    {
        IDLE = 0,
        RECEIVING = 1
    };

    enum class EN_StatusInIDLE : TY_Byte
    {
        IDLE = 0,
        SENDING = 1,
        RECEIVING = 2
    };

    enum class EN_StatusInUSING : TY_Byte
    {
        NOT_SET = 0,
        SENDING = 1,
        RECEIVING = 2
    };

    // Initialization
    static void                          Init();
    // Operation
    static CHMipProtocol::EN_ToDo  EventHandler(EN_Event event_, CFrame* frame_);
    static TY_Byte*                 GetTxData(TY_Word* pu16_Len);
    static void        SetActiveServiceFailed();
    static void                        Enable();
    static void                       Disable();
    // State handling
    static EN_Status        HandleStatus_IDLE(EN_Event event_, CFrame* frame_, CHMipProtocol::EN_ToDo* to_do_);
    static EN_Status    HandleStatus_WATCHING(EN_Event event_, CFrame* frame_, CHMipProtocol::EN_ToDo* to_do_);
    static EN_Status     HandleStatus_ENABLED(EN_Event event_, CFrame* frame_, CHMipProtocol::EN_ToDo* to_do_);
    static EN_Status       HandleStatus_USING(EN_Event event_, CFrame* frame_, CHMipProtocol::EN_ToDo* to_do_);
    // Helpers
    static void                        SetRT1();
    static EN_Bool                 SetRT1diff();
    static void                     SetTwoRT1();
    static void                    SetRT1prim();
    static void                 SetTwoRT1prim();
    static void                        SetRT2();
    static void                       SetHOLD();
    static TY_DWord                    GetRT1();
    static TY_DWord                GetRT1prim();
    static EN_Bool                    IsBURST();
    static void                      SetBURST(EN_Bool value);
    static void                 SetMsgPending(EN_Bool value);
    static EN_Bool               IsMsgPending();
    static void                   UpdateBurstMode(CFrame* frame_);
    static EN_Bool         CheckForPendingMsg();
    static EN_Status           Enter_WATCHING(CHMipProtocol::EN_ToDo* to_do_);
    static EN_Status            Enter_ENABLED(CHMipProtocol::EN_ToDo* to_do_);
    static EN_Status               StartTransmit_EnterUSING(CHMipProtocol::EN_ToDo* to_do_);
    static EN_Status            XmtMsg_ENABLE(CHMipProtocol::EN_ToDo* to_do_);
    static EN_Status             XmtMsg_WATCH(CHMipProtocol::EN_ToDo* to_do_);
    static void               TRANSMITcnfFail();
    static void              TRANSMITcnfRetry();
    static void            TRANSMITcnfSuccess(CFrame* frame_);
    static void              SignalBurstIndication(CFrame* frame_);
    static void               TRANSMITcnfDone();
    static EN_Status          HandleSrvFailed(CHMipProtocol::EN_ToDo* to_do_);

    /* Nested classes */
    class CUsedTimeOuts
    {
    public:
        static const TY_DWord            LONG_STO = 270;
        static const TY_DWord           SHORT_STO = 32;
        static const TY_DWord       PRIM_RT1_1200 = 305;
        static const TY_DWord       SCND_RT1_1200 = 380;
        static const TY_DWord       PRIM_RT1_2400 = 305;
        static const TY_DWord       SCND_RT1_2400 = 380;
        static const TY_DWord       PRIM_RT1_4800 = 305;
        static const TY_DWord       SCND_RT1_4800 = 380;
        //static const TY_DWord       PRIM_RT1_2400 = 152;
        //static const TY_DWord       SCND_RT1_2400 = 190;
        //static const TY_DWord       PRIM_RT1_4800 = 75;
        //static const TY_DWord       SCND_RT1_4800 = 95;
        static const TY_DWord      SHORT_PRIM_RT1 = 32;
        static const TY_DWord      SHORT_SCND_RT1 = 35;
        static const TY_DWord            RT2_1200 = 75;
        static const TY_DWord            RT2_2400 = 75;
        static const TY_DWord            RT2_4800 = 75;
        //static const TY_DWord            RT2_2400 = 37;
        //static const TY_DWord            RT2_4800 = 18;
        static const TY_DWord           SHORT_RT2 = 3;
        static const TY_DWord           HOLD_1200 = 20;
        static const TY_DWord           HOLD_2400 = 20;
        static const TY_DWord           HOLD_4800 = 20;
        //static const TY_DWord           HOLD_2400 = 10;
        //static const TY_DWord           HOLD_4800 = 5;
        static const TY_DWord          SHORT_HOLD = 3;
    };
private:
    static EN_Status           Status;
    static EN_StatusInWATCHING m_status_in_WATCHING;
    static EN_StatusInIDLE     m_status_in_IDLE;
    static EN_StatusInUSING    m_status_in_USING;
    static EN_Bool             m_hart_enabled;
    static EN_Bool             m_burst_mode;
    static EN_Bool             m_msg_pending;
    static CService*           m_active_CService;
    static COSAL::CTimer       m_timer;
};

class CHMipL2RxSM
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
    static void                 Init();
     // Operation
    static CHMipProtocol::EN_ToDo      EventHandler(CHMipProtocol::EN_Event event_, CFrame* frame_, TY_Byte* rcv_bytes_, TY_Word len_,
                                                  CFrame* junk_, CFrame* request_, CFrame* response_, CFrame* burst_);
    static void                Reset();
    static TY_Byte      GetBlockSize();

private:
    static EN_Status       Status;
    static TY_Byte         m_pend_rcv_bytes[MAX_TXRX_SIZE];
    static TY_Word         m_num_pend_bytes;
    static COSAL::CTimer   m_timer;
    static TY_Word         m_next_char_gap;
    static TY_DWord        m_last_rcv_event_time;
    static TY_Byte         m_expected_rcv_size;
    static TY_DWord        m_debug;

    // Helpers
    static TY_Word        HandleData(CHMipProtocol::EN_Event event_, CFrame* frame_, TY_Byte* pst_RcvBytes, TY_Word u16_Len,
                                     CFrame* junk_, CFrame* request_, CFrame* response_, CFrame* burst_);
    static void     SavePendingBytes(TY_Byte* rcv_bytes_, TY_Word len_);
    static TY_Word   GetPendingBytes(TY_Byte* rcv_bytes_);
    static void          SetGapTimer(TY_Word num_characters_);
};

class CHMipL2TxSM
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
    /* Operation */
    static CHMipProtocol::EN_ToDo EventHandler();
    static EN_Status               GetStatus();
    static void                    SetStatus(EN_Status status_);
    static void                     SetTxLen(TY_Word len_);

private:
    static EN_Status     Status;
    static TY_Word       m_len;
    static COSAL::CTimer m_timer;
};

#endif // __hmiplayer2_h__