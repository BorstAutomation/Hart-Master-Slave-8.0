/*
 *          File: HSipMacPort.h (CHSipMacPort)
 *                The interface to the MAC port is relatively small
 *                and can be defined generically. However, the implementation
 *                depends on the hardware and software environment.
 *                That's why there is only a header at this point, while
 *                the file HSipMacPort.cpp can be found in the specific branch.
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
#ifndef __hsipmacport_h__
#define __hsipmacport_h__

#include "OSAL.h"
#include "WbHartUser.h"

// Index into hart ip header
#define IDX_VER    0
#define IDX_TYPE   1
#define IDX_ID     2
#define IDX_STAT   3
#define IDX SEQ_LO 4
#define IDX_SEQ_HI 5
#define IDX_LEN_LO 6
#define IDX_LEN_HI 7
#define IDX_PDU    8 

#define MSGTY_REQ   0
#define MSGTY_RSP   1
#define MSGTY_BST   2
#define MSGTY_NAK  15

#define MSGID_INI   0
#define MSGID_CLS   1
#define MSGID_ALI   2
#define MSGID_PDU   3

#define MAX_VER 1

class CHSipMacPort
{
public:
    // Enum classes
    enum class EN_Msg_Type : TY_Byte
    {
        REQUEST = 0,
        RESPONSE = 1,
        BURST = 2,
        NAK = 15
    };

    enum class EN_Msg_ID : TY_Byte
    {
        INITIATE = 0,
        CLOSE = 1,
        KEEP_ALIVE = 2,
        HART_PDU = 3
    };

    enum class EN_HartIP_Info : TY_Byte
    {
        KEEP_ALIVE_REQUEST = 0,
        KEEP_ALIVE_RESPONSE = 1,
        REQUEST_PDU = 2,
        RESPONSE_PDU = 3,
        INITIATE_REQUEST = 4,
        INITIATE_RESPONSE = 5,
        CLOSE_REQUEST = 6,
        CLOSE_RESPONSE = 7,
        BURST = 8,
        NO_HART_IP = 9,
        NOT_FOR_ME = 10,
        NET_ERR = 11,
        NO_TRAFFIC = 12
    };

    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        INITIALIZING = 1,
        WAIT_CONNECT = 2,
        SERVER_READY = 3,
        WAIT_INITIATE = 4,
        WAIT_RESPONSE = 5,
        SHUTTING_DOWN = 6
    };

    enum class EN_ToDo : TY_Byte
    {
        NOTHING = 0,
        CONNECT = 1,
        DISCONNECT = 2,
        SEND_RESPONSE = 3,
        SEND_INITIATE_RESPONSE = 4,
        SEND_ALIVE_RESPONSE = 5,
        SEND_BURST = 6,
        TERMINATE_CONNECTION = 7,
        RECEIVE_ENABLE = 8,
        RECEIVE_DISABLE = 9
    };

    enum class EN_LastError : TY_Byte
    {
        NONE = 0,
        INITIALIZING = 1,
        GET_ADDR_INFO = 2,
        CREATE_SOCKET = 3,
        BIND = 4,
        LISTEN = 5,
        ACCEPT = 6,
        TX_FAILED = 7,
        SHUTDOWN = 8,
        RECEIVING = 9,
        SET_TIMEOUT = 10
    };

    static EN_Bool                  Open(TY_Byte* host_name_, TY_Byte* port_, EN_CommType type_);
    static void                    Close();
    static void                  Execute(TY_Word time_ms_);
    static void                     Init();
    static TY_Word             GetStatus();
    static TY_Word        GetMagicNumber();
    static TY_Byte        GetMessageType();
    static void           SetMessageType(TY_Byte hart_ip_msg_type_);
    static TY_Word     GetSequenceNumber();
    static void     GetIpFrameForMonitor(TY_Byte* src_, TY_Byte* src_len_, TY_Byte* dst_, TY_Byte dst_len);

private:
    static TY_Byte        m_rcv_buf[MAX_IP_TXRX_SIZE];
    static TY_Word        m_rcv_len;
    static TY_Byte        m_tx_buf[MAX_IP_TXRX_SIZE];
    static TY_Word        m_tx_len;
    static EN_Status      m_status;
    static EN_LastError   m_last_error;
    static TY_Byte        m_rx_err;
    static EN_HartIP_Info m_hart_ip_msg_info;
    static EN_ToDo        m_to_do;
    static EN_Bool        m_close_request;
    static TY_Byte        m_hart_rx_data[MAX_TXRX_SIZE];
    static TY_Byte        m_hart_rx_len;
    static TY_Byte        m_hart_tx_data[MAX_TXRX_SIZE];
    static TY_Byte        m_hart_tx_len;
    static TY_Byte        m_hart_ip_version;
    static TY_Byte        m_hart_ip_message_type;
    static TY_Byte        m_hart_ip_message_id;
    static TY_Byte        m_hart_ip_comm_status;
    static TY_Word        m_hart_ip_sequence_number;
    static TY_Word        m_hart_ip_sq_num_request;
    static TY_Word        m_hart_ip_sq_num_burst;
    static TY_Word        m_hart_ip_byte_count;
    static TY_Word        m_magic_number;
    static TY_Byte        m_initiate_req_data[5];

    // Methods
    static EN_Bool        InitializeSocketHandler();
    static EN_Bool                ConnectToClient();
    static EN_Bool        HandleConnectionClosing();
    static EN_HartIP_Info   ReceiveNetworkMessage();
    static EN_Status            SendHartIpMessage();
    static void                     SendHartIpNAK(TY_Byte* ip_data_, TY_Byte ip_data_len_);
    static EN_ToDo                  SignalWaiting();
    static EN_ToDo         SignalHartPDU_Received();
    static EN_ToDo              SignalHartSilence();
    static void                  SignalHartTxDone();
    static void                SignalNetworkError();
    static void                    EncodeResponse();
    static void                       EncodeBurst();
    static void            EncodeInitiateResponse();
    static void               EncodeCloseResponse();
    static void           EncodeKeepAliveResponse();
    static void                AcceptHartIpHeader();
    static EN_Bool         AcceptHartIpRequestPDU();
    static void       AcceptHartIpInitiateRequest();
    static void          RespondToHartIpCloseRequest();
    static void      AcceptHartIpKeepAliveRequest();
    static void                      SaveNextToDo(EN_ToDo to_do_);
    static EN_ToDo                  FetchNextToDo();
    static EN_Status          TerminateConnection();

};
#endif // __hsipmacport_h__