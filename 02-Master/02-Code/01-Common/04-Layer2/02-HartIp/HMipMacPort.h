/*
 *          File: HMipMacPort.h 
 *                The interface to the MAC port is relatively small
 *                and can be defined generically. However, the implementation
 *                depends on the hardware and software environment.
 *                That's why there is only a header at this point, while
 *                the file HIPMMacPort.cpp can be found in the specific branch.
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
#ifndef __hmipmacport_h__
#define __hmipmacport_h__

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

class CHMipMacPort
{
public:
    // Enum classes
    enum class EN_Msg_Type : TY_Byte
    {
        REQUEST = 0u,
        RESPONSE = 1u,
        BURST = 2u,
        NAK = 15u,
        // Note: This code is only for internal use
        RECEIVED = 0xff
    };

    enum class EN_Msg_ID : TY_Byte
    {
        INITIATE = 0u,
        CLOSE = 1u,
        KEEP_ALIVE = 2u,
        HART_PDU = 3u
    };

    enum class EN_HartIP_Info : TY_Byte
    {
        KEEP_ALIVE_REQUEST = 0,
        KEEP_ALIVE_RESPONSE = 1,
        COMMAND_REQUEST = 2,
        COMMAND_RESPONSE = 3,
        INITIATE_REQUEST = 4,
        INITIATE_RESPONSE = 5,
        CLOSE_REQUEST = 6,
        CLOSE_RESPONSE = 7,
        BURST = 8,
        NO_HART_IP = 9,
        NOT_FOR_ME = 10,
        NET_ERR = 11,
        NO_TRAFFIC = 12,
        NAK_RESPONSE = 13
    };

    enum class EN_Status : TY_Byte
    {
        IDLE = 0,
        INITIALIZING = 1,
        WAIT_CONNECT = 2,
        WAIT_INITIATE_RESPONSE = 3,
        CLIENT_READY = 4,
        WAIT_ALIVE_RESPONSE = 5,
        WAIT_COMMAND_RESPONSE = 6,
        WAIT_CLOSE_RESPONSE = 7,
        SHUTTING_DOWN = 8
    };

    enum class EN_ToDo : TY_Byte
    {
        NOTHING = 0,
        CONNECT = 1,
        DISCONNECT = 2,
        SEND_REQUEST = 3,
        RECEIVE_ENABLE = 4, // ??
        RECEIVE_DISABLE = 5 // ??
    };

    enum class EN_LastError : TY_Byte
    {
        NONE = 0,
        INITIALIZING = 1,
        GET_ADDR_INFO = 2,
        CREATE_SOCKET = 3,
        NO_SERVER = 4,
        TX_FAILED = 5,
        SHUTDOWN = 6,
        RECEIVING = 7,
        NETWORK = 8,
        SET_TIMEOUT = 9,
        KEEP_ALIVE = 10
    };

    static void                 Execute(TY_Word time_ms_);
    static EN_Bool                 Open(TY_Byte* host_name_, TY_Byte* port_, EN_CommType type_);
    static void                   Close();
    static void                    Init();
    static TY_Word            GetStatus();
    static TY_Word       GetMagicNumber();
    static TY_Word    GetSequenceNumber(TY_Byte msg_type_);
    static void       SetSequenceNumber(TY_Byte msg_type_);
    static void    GetIpFrameForMonitor(TY_Byte* dst_, TY_Byte* dst_len_, TY_Byte* src_, TY_Byte src_len, TY_Byte msg_type_);
    static TY_Word       GetPayloadData(TY_Byte* data_);

private:
    // Data
    static TY_Byte        m_rcv_buf[MAX_IP_TXRX_SIZE];
    static int            m_rcv_len;
    static TY_Byte        m_tx_buf[MAX_IP_TXRX_SIZE];
    static int            m_tx_len;
    static TY_Byte        m_hart_ip_data[MAX_IP_TXRX_SIZE];
    static TY_Word        m_hart_ip_len;
    static EN_LastError   m_last_error;
    static TY_Byte        m_rx_err;
    static EN_HartIP_Info m_last_hart_ip_info;
    static EN_ToDo        m_to_do;
    static EN_Bool        m_close_request;
    static TY_Byte        m_hart_rx_data[MAX_TXRX_SIZE];
    static TY_Byte        m_hart_rx_len;
    static TY_Byte        m_hart_ip_version;
    static TY_Byte        m_hart_ip_message_type;
    static TY_Byte        m_hart_ip_message_id;
    static TY_Byte        m_hart_ip_comm_status;
    static TY_Word        m_hart_ip_request_seq_number;
    static TY_Word        m_hart_ip_response_seq_number;
    static TY_Word        m_hart_ip_burst_seq_number;
    static TY_Word        m_hart_ip_nak_seq_number;
    static TY_Word        m_hart_ip_received_seq_number;
    static TY_Word        m_hart_ip_byte_count;
    static TY_Word        m_magic_number;
    static TY_DWord       m_ms_counter;
    static TY_Byte        m_initiate_req_data[5];
    // Methods
    static EN_Bool        InitializeSocketHandler();
    static EN_Bool                ConnectToServer();
    static EN_Status          SendInitiateRequest();
    static void            AcceptInitiateResponse();
    static void            EncodeKeepAliveRequest();
    static EN_Status         SendKeepAliveRequest();
    static void           AcceptKeepAliveResponse();
    static EN_Status           SendCommandRequest();
    static void             AcceptCommandResponse();
    static void             RejectCommandResponse();
    static void                   AcceptHartBurst();
    static EN_Status             SendCloseRequest();
    static void               AcceptCloseResponse();
    static EN_Bool        HandleConnectionClosing();
    // Receive from the network
    static EN_HartIP_Info   ReceiveNetworkMessage();
    // Signal to the Hart protocol
    static EN_ToDo         SignalHartPDUreceiving();
    static EN_ToDo              SignalHartSilence();
    static void                  SignalHartTxDone();
    static void                SignalNetworkError();
    // Helper functions
    static EN_Status          TerminateConnection(EN_LastError last_err_);
    static void                      SaveNextToDo(EN_ToDo to_do_);
    static EN_ToDo                  FetchNextToDo();

public:
    static EN_Status Status;
};
#endif // __hmipmacport_h__