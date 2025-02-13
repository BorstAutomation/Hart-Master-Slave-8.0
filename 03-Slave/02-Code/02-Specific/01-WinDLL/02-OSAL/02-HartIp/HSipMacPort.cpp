/*
 *          File: HIPSMacPort.cpp
 *                The Execute method is called directly by the fast cyclic
 *                handler. This basically drives all status machines in
 *                the Hart implementation. Here too, the method is divided
 *                into an Event handler and a ToDo handler.
 *                This version is especially dedicated to a Hart IP server.
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

 // Winsockets
#include <winsock2.h>
#include <ws2tcpip.h>
// Need to link with Ws2_32.lib, Mswsock.lib, and Advapi32.lib
#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")
#define DEFAULT_BUFLEN 512
// End Winsockets
#include <Windows.h>

#include "WbHartSlave.h"
#include "HSipMacPort.h"
#include "WinSystem.h"
#include "HSipProtocol.h"
#include "HSipLayer2.h"
#include "HartChannel.h"
#include "HartData.h"
#include "Monitor.h"

// Data
CHSipMacPort::EN_Status      CHSipMacPort::m_status = CHSipMacPort::EN_Status::IDLE;
TY_Byte                      CHSipMacPort::m_rcv_buf[MAX_IP_TXRX_SIZE];
TY_Word                      CHSipMacPort::m_rcv_len = MAX_IP_TXRX_SIZE;
TY_Byte                      CHSipMacPort::m_tx_buf[MAX_IP_TXRX_SIZE];
TY_Word                      CHSipMacPort::m_tx_len = MAX_IP_TXRX_SIZE;
TY_Byte                      CHSipMacPort::m_rx_err = 0;
CHSipMacPort::EN_LastError   CHSipMacPort::m_last_error;
CHSipMacPort::EN_HartIP_Info CHSipMacPort::m_hart_ip_msg_info;
CHSipMacPort::EN_ToDo        CHSipMacPort::m_to_do;
EN_Bool                      CHSipMacPort::m_close_request = EN_Bool::FALSE8;
TY_Byte                      CHSipMacPort::m_hart_rx_data[MAX_TXRX_SIZE];
TY_Byte                      CHSipMacPort::m_hart_rx_len;
TY_Byte                      CHSipMacPort::m_hart_tx_data[MAX_TXRX_SIZE];
TY_Byte                      CHSipMacPort::m_hart_tx_len;
TY_Byte                      CHSipMacPort::m_hart_ip_version = 0;
TY_Byte                      CHSipMacPort::m_hart_ip_message_type = 0;
TY_Byte                      CHSipMacPort::m_hart_ip_message_id = 0;
TY_Byte                      CHSipMacPort::m_hart_ip_comm_status = 0;
TY_Word                      CHSipMacPort::m_hart_ip_sequence_number = 0;
TY_Word                      CHSipMacPort::m_hart_ip_sq_num_request = 0;
TY_Word                      CHSipMacPort::m_hart_ip_sq_num_burst = 0;
TY_Word                      CHSipMacPort::m_hart_ip_byte_count = 0;
TY_Word                      CHSipMacPort::m_magic_number = 0xe0a3;
TY_Byte                      CHSipMacPort::m_initiate_req_data[5] = { 0, 0, 0, 0, 0 };

// WinSockets
static WSADATA so_wsa_data;
static addrinfo *so_result = NULL;
static addrinfo *so_ptr = NULL;
static addrinfo so_addrinfo;
static SOCKET so_connection_socket = INVALID_SOCKET;
static SOCKET so_listen_socket = INVALID_SOCKET;
static SOCKET so_client_socket = INVALID_SOCKET;

// Hart Ip
static TY_Byte        s_intiate_rsp_header[] = { MAX_VER, MSGTY_RSP, MSGID_INI, 0x00, 0x00, 0x00, 0x00, 13 };
static int       s_initiate_rsp_header_len = 8;
static TY_Byte          s_close_rsp_header[] = { MAX_VER, MSGTY_RSP, MSGID_CLS, 0x00, 0x00, 0x00, 0x00,  8 };
static int          s_close_rsp_header_len = 8;
static TY_Byte     s_keep_alive_rsp_header[] = { MAX_VER, MSGTY_RSP, MSGID_ALI, 0x00, 0x00, 0x00, 0x00,  8 };
static int     s_keep_alive_rsp_header_len = 8;
static TY_Byte                s_rsp_header[] = { MAX_VER, MSGTY_RSP, MSGID_PDU, 0x00, 0x00, 0x00, 0x00,  8 };
static int                s_rsp_header_len = 8;
static TY_Byte              s_burst_header[] = { MAX_VER, MSGTY_BST, MSGID_PDU, 0x00, 0x00, 0x00, 0x00,  8 };
static int              s_burst_header_len = 8;
static TY_Byte                s_nak_header[] = { MAX_VER, MSGTY_NAK, MSGID_PDU, 0x00, 0x00, 0x00, 0x00,  8 };
static int                s_nak_header_len = 8;

// Methods

void CHSipMacPort::Execute(TY_Word time_ms_)
{
    // Note: This procedure is called every ms as long as the channel is open
    int     result = 0;
    TY_Word test_val = 0;
    EN_ToDo to_do;

    if (time_ms_ > 20)
    {
        test_val = time_ms_;
    }

    COSAL::CTimer::UpdateTime(time_ms_);

    if (CHartData::CStat.HartIpDataChanged == EN_Bool::TRUE8)
    {
        CHartData::CStat.HartIpDataChanged = EN_Bool::FALSE8;
        if (m_status != EN_Status::IDLE)
        {
            m_status = EN_Status::INITIALIZING;
        }
    }

    switch (m_status)
    {
    case EN_Status::IDLE:
        // Do nothing
        break;
    case EN_Status::INITIALIZING:
        if (InitializeSocketHandler() == EN_Bool::TRUE8)
        {
            m_status = EN_Status::WAIT_CONNECT;
        }

        break;
    case EN_Status::WAIT_CONNECT:
        if (ConnectToClient() == EN_Bool::TRUE8)
        {
            m_status = EN_Status::WAIT_INITIATE;
        }
        else
        {
            // Try again to connect
            m_status = EN_Status::INITIALIZING;
        }

        break;
    case EN_Status::WAIT_INITIATE:
        m_hart_ip_msg_info = ReceiveNetworkMessage();
        if (m_hart_ip_msg_info == EN_HartIP_Info::INITIATE_REQUEST)
        {
            AcceptHartIpInitiateRequest();
            EncodeInitiateResponse();
            m_status = SendHartIpMessage();
            // Put the Hart state machine into receive mode
            SignalHartSilence();
        }

        break;
    case EN_Status::SERVER_READY:
        m_hart_ip_msg_info = ReceiveNetworkMessage();
        switch (m_hart_ip_msg_info)
        {
        case EN_HartIP_Info::KEEP_ALIVE_REQUEST:
            AcceptHartIpKeepAliveRequest();
            EncodeKeepAliveResponse();
            m_status = SendHartIpMessage();
            break;
        case EN_HartIP_Info::CLOSE_REQUEST:
            RespondToHartIpCloseRequest();
            break;
        case EN_HartIP_Info::NO_TRAFFIC:
            SaveNextToDo(SignalHartSilence());
            to_do = FetchNextToDo();
            if (to_do == EN_ToDo::SEND_BURST)
            {
                EncodeBurst();
                SignalHartTxDone();
                m_status = SendHartIpMessage();
            }

            break;
        case EN_HartIP_Info::REQUEST_PDU:
            // Accept is checking the address
            if (AcceptHartIpRequestPDU() == EN_Bool::TRUE8)
            {
                SaveNextToDo(SignalHartPDU_Received());
                m_status = EN_Status::WAIT_RESPONSE;
            }
            else
            {
                m_status = EN_Status::SERVER_READY;
            }

            break;
        case EN_HartIP_Info::NET_ERR:
            SignalNetworkError();
            m_status = EN_Status::INITIALIZING;
            break;
        }

        break;
    case EN_Status::WAIT_RESPONSE:
        SaveNextToDo(SignalWaiting());
        to_do = FetchNextToDo();
        if (to_do == EN_ToDo::SEND_RESPONSE)
        {
            EncodeResponse();
            SignalHartTxDone();
            m_status = SendHartIpMessage();
        }

        break;
    }
}

EN_Bool CHSipMacPort::Open(TY_Byte* host_name_, TY_Byte* port_, EN_CommType type_)
{
    m_status = EN_Status::INITIALIZING;
    m_last_error = EN_LastError::NONE;
    CWinSys::CyclicTaskStart();
    return EN_Bool::TRUE8;
}

void CHSipMacPort::Close()
{
    // Try to tell the cyclic thread to close
    // the connection
    m_close_request = EN_Bool::TRUE8;
    // Wait for the thread
    COSAL::Wait(50);
    if (m_status != EN_Status::IDLE)
    {
        // Thread stucked, kill it
        CWinSys::CyclicTaskKill();
    }
    else
    {
        // Terminate thraed
        CWinSys::CyclicTaskTerminate();
    }

    // Get rid of the leftover mess
    if (so_connection_socket != INVALID_SOCKET)
    {
        closesocket(so_connection_socket);
        so_connection_socket = INVALID_SOCKET;
    }

    WSACleanup();
    m_close_request = EN_Bool::FALSE8;
    m_status = EN_Status::IDLE;
}

void CHSipMacPort::Init()
{
    CHSipL2SM::Init();
}

TY_Word CHSipMacPort::GetStatus()
{
    // .15 .14 .13 .12 .11 .10 .09 .08 .07 .06 .05 .04 .03 .02 .01 .00
    //  +   +   +   +--- Status ----+   +-------- Last Error -------+
    //  |   |   +-- tbd
    //  |   +------ tbd
    //  +---------- Data available

    return (TY_Word)((((TY_Byte)m_status & 0x1f) << 8) + (TY_Byte)m_last_error);
}

TY_Word CHSipMacPort::GetMagicNumber()
{
    return m_magic_number;
}

TY_Byte CHSipMacPort::GetMessageType()
{
    return m_hart_ip_message_type;
}

void CHSipMacPort::SetMessageType(TY_Byte hart_ip_msg_type_)
{
    m_hart_ip_message_type = hart_ip_msg_type_;
    if (hart_ip_msg_type_ == 2)
    {
        m_hart_ip_sequence_number = m_hart_ip_sq_num_burst;
    }
}

TY_Word CHSipMacPort::GetSequenceNumber()
{
    return m_hart_ip_sequence_number;
}

void CHSipMacPort::GetIpFrameForMonitor(TY_Byte* dst_, TY_Byte* dst_len_, TY_Byte* src_, TY_Byte src_len_)
{
    TY_Word del_pos;
    TY_Word word;

    COSAL::CMem::Set(dst_, 0, MAX_TXRX_SIZE);
    word = CHSipMacPort::GetMagicNumber();
    dst_[0] = (TY_Byte)(word >> 8);
    dst_[1] = (TY_Byte)(word);
    dst_[2] = CHSipMacPort::GetMessageType();
    word = CHSipMacPort::GetSequenceNumber();
    dst_[3] = (TY_Byte)(word >> 8);
    dst_[4] = (TY_Byte)(word);
    // Find delimiter pos
    for (TY_Byte i = 0; i < src_len_; i++)
    {
        if (src_[i] != 0xff)
        {
            del_pos = i;
            break;
        }
    }

    *dst_len_ = src_len_ - del_pos;
    // Copy starting from delimiter
    COSAL::CMem::Copy(&dst_[5], &src_[del_pos], *dst_len_);
    *dst_len_ = *dst_len_ + 5;
}

EN_Bool CHSipMacPort::InitializeSocketHandler()
{
    int result;

    WSACleanup();
    so_connection_socket = INVALID_SOCKET;
    so_listen_socket = INVALID_SOCKET;
    so_client_socket = INVALID_SOCKET;
    so_result = NULL;
    so_ptr = NULL;

    result = WSAStartup(MAKEWORD(2, 2), &so_wsa_data);
    if (result != 0)
    {
        m_last_error = EN_LastError::INITIALIZING;
        return EN_Bool::FALSE8;
    }
    else
    {
        ZeroMemory(&so_addrinfo, sizeof(so_addrinfo));
        so_addrinfo.ai_family = AF_UNSPEC;
        so_addrinfo.ai_socktype = SOCK_STREAM;
        so_addrinfo.ai_protocol = IPPROTO_TCP;

        // Resolve the server address and port
        if (CHartData::CStat.HartIpUseAddress == EN_Bool::TRUE8)
        {
            result = getaddrinfo((const char*)CHartData::CStat.HartIpAddress, (const char*)CHartData::CStat.HartIpPort, &so_addrinfo, &so_result);
        }
        else
        {
            result = getaddrinfo((const char*)CHartData::CStat.HartIpHostName, (const char*)CHartData::CStat.HartIpPort, &so_addrinfo, &so_result);
        }

        if (result != 0)
        {
            m_last_error = EN_LastError::GET_ADDR_INFO;
            WSACleanup();
            return EN_Bool::FALSE8;
        }
    }

    return EN_Bool::TRUE8;
}

EN_Bool CHSipMacPort::ConnectToClient()
{
    int result;
    int error_code = 0;
    // Set time_out to 50 ms
    timeval rcv_to = { 1, 50000 }; 

    // Create a SOCKET for the server to listen for client connections.
    so_listen_socket = socket(so_result->ai_family, so_result->ai_socktype, so_result->ai_protocol);
    if (so_listen_socket == INVALID_SOCKET)
    {
        // Socket create failed with error.
        freeaddrinfo(so_result);
        WSACleanup();
        m_last_error = EN_LastError::CREATE_SOCKET;
        return EN_Bool::FALSE8;
    }

    // Setup the TCP listening socket
    result = bind(so_listen_socket, so_result->ai_addr, so_result->ai_addrlen);
    if (result == SOCKET_ERROR) {
        // Bind failed with error.
        freeaddrinfo(so_result);
        closesocket(so_listen_socket);
        so_listen_socket = INVALID_SOCKET;
        WSACleanup();
        m_last_error = EN_LastError::BIND;
        return EN_Bool::FALSE8;
    }

    freeaddrinfo(so_result);

    result = listen(so_listen_socket, SOMAXCONN);
    if (result == SOCKET_ERROR) {
        // Listen failed with error.
        closesocket(so_listen_socket);
        so_listen_socket = INVALID_SOCKET;
        WSACleanup();
        m_last_error = EN_LastError::LISTEN;
        return EN_Bool::FALSE8;
    }

    // Accept a client socket
    so_client_socket = accept(so_listen_socket, NULL, NULL);
    if (so_client_socket == INVALID_SOCKET) {
        // Accept failed with error.
        closesocket(so_listen_socket);
        so_listen_socket = INVALID_SOCKET;
        WSACleanup();
        m_last_error = EN_LastError::ACCEPT;
        return EN_Bool::FALSE8;
    }

    // No longer need listen socket
    closesocket(so_listen_socket);
    so_listen_socket = INVALID_SOCKET;

    // Settimeout of the clientsocket
    if (setsockopt(so_client_socket, SOL_SOCKET, SO_RCVTIMEO, (char *)&rcv_to, sizeof(rcv_to)) == SOCKET_ERROR)
    {
        error_code = WSAGetLastError();
        m_last_error = EN_LastError::SET_TIMEOUT;
        return EN_Bool::FALSE8;
    }

    return EN_Bool::TRUE8;
}

CHSipMacPort::EN_HartIP_Info CHSipMacPort::ReceiveNetworkMessage()
{
    int result;
    int error_code = 0;
    EN_HartIP_Info msg_info = EN_HartIP_Info::NO_HART_IP;

    m_rcv_len = MAX_IP_TXRX_SIZE;
    result = recv(so_client_socket, (char*)&m_rcv_buf, m_rcv_len, MSG_PEEK);
    if (result > 0)
    {
        // Take the message out of the buffer in the sockets
        result = recv(so_client_socket, (char*)&m_rcv_buf, m_rcv_len, 0);
    }

    if (result == SOCKET_ERROR)
    {
        error_code = WSAGetLastError();
        if (error_code == WSAETIMEDOUT)
        {
            // Time out (50 ms) bursts may be sent
            return EN_HartIP_Info::NO_TRAFFIC;
        }
    }

    if ((result >= 8) &&
        (result <= MAX_IP_TXRX_SIZE))
    {
        m_rcv_len = (TY_Byte)result;

        // Possibly a Hart IP frame
        if ((m_rcv_buf[IDX_VER] != 0) &&
            (m_rcv_buf[IDX_VER] <= MAX_VER))
        {
            // Hart IP, look for details
            if (m_rcv_buf[IDX_TYPE] == (TY_Byte)EN_Msg_Type::REQUEST)
            {
                // What kind of a request?
                if ((m_rcv_buf[IDX_ID] == (TY_Byte)EN_Msg_ID::INITIATE) &&
                    (m_rcv_len == 13))
                {
                    COSAL::CMem::Copy(m_initiate_req_data, &m_rcv_buf[IDX_PDU], 5);
                    msg_info = EN_HartIP_Info::INITIATE_REQUEST;
                }
                else if ((m_rcv_buf[IDX_ID] == (TY_Byte)EN_Msg_ID::CLOSE) &&
                    (m_rcv_len == 8))
                {
                    msg_info = EN_HartIP_Info::CLOSE_REQUEST;
                }
                else if ((m_rcv_buf[IDX_ID] == (TY_Byte)EN_Msg_ID::HART_PDU) &&
                         (m_rcv_len > 8))
                {
                    msg_info = EN_HartIP_Info::REQUEST_PDU;
                }
                else if ((m_rcv_buf[IDX_ID] == (TY_Byte)EN_Msg_ID::KEEP_ALIVE) &&
                         (m_rcv_len == 8))
                {
                    msg_info = EN_HartIP_Info::KEEP_ALIVE_REQUEST;
                }
                else
                {
                    SaveNextToDo(SignalHartSilence());
                }
            }
        }
    }
    
    return msg_info;
}

CHSipMacPort::EN_Status CHSipMacPort::SendHartIpMessage()
{
    int result = send(so_client_socket, (const char*)m_tx_buf, m_tx_len, 0);
    if (result == m_tx_len)
    {
        return EN_Status::SERVER_READY;
    }
    else
    {
        SignalNetworkError();
        return EN_Status::INITIALIZING;
    }
}

void CHSipMacPort::SendHartIpNAK(TY_Byte* ip_data_, TY_Byte ip_data_len_)
{
    TY_Word payload_len = s_nak_header_len + ip_data_len_;

    // Begin with the header
    COSAL::CMem::Copy(m_tx_buf, s_nak_header, s_nak_header_len);
    // Finalze the packet
    m_tx_buf[4] = (TY_Byte)(m_hart_ip_sequence_number >> 8);
    m_tx_buf[5] = (TY_Byte)(m_hart_ip_sequence_number);
    m_tx_buf[6] = (TY_Byte)(payload_len >> 8);
    m_tx_buf[7] = (TY_Byte)(payload_len);
    COSAL::CMem::Copy(&m_tx_buf[s_nak_header_len], ip_data_, ip_data_len_);
    m_tx_len = (TY_Byte)payload_len;
    SendHartIpMessage();
}


CHSipMacPort::EN_ToDo CHSipMacPort::SignalWaiting()
{
    CHSipMacPort::EN_ToDo todo = CHSipProtocol::EventHandler(CHSipProtocol::EN_Event::NONE, NULL, 0, 0);
    return todo;
}

CHSipMacPort::EN_ToDo CHSipMacPort::SignalHartPDU_Received()
{
    CHSipMacPort::EN_ToDo todo = CHSipProtocol::EventHandler(CHSipProtocol::EN_Event::HART_IP_DATA_RECEIVED, m_hart_rx_data, m_hart_rx_len, 0);
    return todo;
}

CHSipMacPort::EN_ToDo CHSipMacPort::SignalHartSilence()
{
    CHSipMacPort::EN_ToDo todo = CHSipProtocol::EventHandler(CHSipProtocol::EN_Event::HART_IP_NO_DATA_RECEIVED, m_hart_rx_data, m_hart_rx_len, 0);
    return todo;
}

void CHSipMacPort::SignalHartTxDone()
{
    CHSipProtocol::EventHandler(CHSipProtocol::EN_Event::HART_IP_TX_DONE, NULL, NULL, 0);
}

void CHSipMacPort::SignalNetworkError()
{
    CHSipProtocol::EventHandler(CHSipProtocol::EN_Event::NETWORK_ERROR, NULL, NULL, 0);
}

void CHSipMacPort::EncodeResponse()
{
    // Prepare the hart ip payload
    TY_Word del_pos = 0;
    TY_Word tx_len;
    TY_Word idx = 0;
    TY_Word payload_len = 0;
    TY_Byte* dst_ = CHSipProtocol::GetTxData(&tx_len);
    // Search delimiter
    for (TY_Word i = 0; i < tx_len; i++)
    {
        if (dst_[i] != 0xff)
        {
            del_pos = i;
            break;
        }
    }

    // Begin with the header
    COSAL::CMem::Copy(m_tx_buf, s_rsp_header, s_rsp_header_len);
    idx = s_rsp_header_len;
    // Add the hart ip payload
    COSAL::CMem::Copy(&m_tx_buf[idx], &dst_[del_pos], tx_len - del_pos);
    payload_len = tx_len - del_pos;
    m_tx_len = s_rsp_header_len + tx_len - del_pos;
    // Finalze the packet
    m_tx_buf[4] = (TY_Byte)(m_hart_ip_sq_num_request >> 8);
    m_tx_buf[5] = (TY_Byte)(m_hart_ip_sq_num_request);
    payload_len = payload_len + 8;
    m_tx_buf[6] = (TY_Byte)(payload_len >> 8);
    m_tx_buf[7] = (TY_Byte)(payload_len);
    m_hart_ip_message_type = 1;
    CMonitor::SetAdditionalData(m_tx_buf, m_tx_len);
}

void CHSipMacPort::EncodeBurst()
{
    // Prepare the hart ip payload
    TY_Word del_pos;
    TY_Word tx_len;
    TY_Word idx = 0;
    TY_Word payload_len = 0;
    TY_Byte* dst_ = CHSipProtocol::GetTxData(&tx_len);
    // Search delimiter
    for (TY_Word i = 0; i < tx_len; i++)
    {
        if (dst_[i] != 0xff)
        {
            del_pos = i;
            break;
        }
    }

    // Begin with the header
    COSAL::CMem::Copy(m_tx_buf, s_burst_header, s_burst_header_len);
    idx = s_burst_header_len;
    // Add the hart ip payload
    COSAL::CMem::Copy(&m_tx_buf[idx], &dst_[del_pos], tx_len - del_pos);
    payload_len = tx_len - del_pos;
    m_tx_len = s_burst_header_len + tx_len - del_pos;
    // Finalize the paket
    m_tx_buf[4] = (TY_Byte)(m_hart_ip_sq_num_burst >> 8);
    m_tx_buf[5] = (TY_Byte)(m_hart_ip_sq_num_burst);
    payload_len = payload_len + 8;
    m_tx_buf[6] = (TY_Byte)(payload_len >> 8);
    m_tx_buf[7] = (TY_Byte)(payload_len);
    m_hart_ip_message_type = 2;
    CMonitor::SetAdditionalData(m_tx_buf, m_tx_len);
}

void CHSipMacPort::EncodeInitiateResponse()
{
    // Begin with the header
    COSAL::CMem::Copy(m_tx_buf, s_intiate_rsp_header, s_initiate_rsp_header_len);
    // Add payload data
    COSAL::CMem::Copy(&m_tx_buf[s_initiate_rsp_header_len], m_initiate_req_data, 5);
    m_tx_len = 13;
    // Insert sequence number
    m_tx_buf[4] = (TY_Byte)(m_hart_ip_sequence_number >> 8);
    m_tx_buf[5] = (TY_Byte)(m_hart_ip_sequence_number);
}

void CHSipMacPort::EncodeKeepAliveResponse()
{
    // Begin with the header
    COSAL::CMem::Copy(m_tx_buf, s_keep_alive_rsp_header, s_keep_alive_rsp_header_len);
    m_tx_len = 8;
    // Insert sequence number
    m_tx_buf[4] = (TY_Byte)(m_hart_ip_sequence_number >> 8);
    m_tx_buf[5] = (TY_Byte)(m_hart_ip_sequence_number);
}

void CHSipMacPort::AcceptHartIpHeader()
{
    // Get the hart ip header details
    m_hart_ip_version = m_rcv_buf[0];
    m_hart_ip_message_type = m_rcv_buf[1];
    m_hart_ip_message_id = m_rcv_buf[2];
    m_hart_ip_comm_status = m_rcv_buf[3];
    m_hart_ip_sequence_number = (TY_Word)((m_rcv_buf[4] << 8) + m_rcv_buf[5]);
    m_hart_ip_byte_count = (TY_Word)((m_rcv_buf[6] << 8) + m_rcv_buf[7]);
}

EN_Bool CHSipMacPort::AcceptHartIpRequestPDU()
{
    AcceptHartIpHeader();

    m_hart_ip_sq_num_request = m_hart_ip_sequence_number;
    m_hart_ip_sq_num_burst = m_hart_ip_sequence_number;

    // Copy frame  to the hart context
    COSAL::CMem::Set(m_hart_rx_data, 0, MAX_TXRX_SIZE);
    m_hart_rx_len = (TY_Byte)(m_rcv_len - HART_IP_HEADER_LEN);
    COSAL::CMem::Copy(m_hart_rx_data, &m_rcv_buf[HART_IP_HEADER_LEN], m_hart_rx_len);

    // Check at least short addresses
    if ((m_hart_rx_data[0] & 0x80) == 0)
    {
        // Is short address
        if ((m_hart_rx_data[1] & 0x3f) != CHartData::CStat.PollAddress)
        {
            // Wrong short address
            // Slave will not send a response
            SendHartIpNAK(m_hart_rx_data, m_hart_rx_len);
            return EN_Bool::FALSE8;
        }
    }

    return EN_Bool::TRUE8;
}

void CHSipMacPort::AcceptHartIpInitiateRequest()
{
    AcceptHartIpHeader();

    m_hart_ip_sq_num_request = m_hart_ip_sequence_number;
    m_hart_ip_sq_num_burst = m_hart_ip_sequence_number;
}

void CHSipMacPort::RespondToHartIpCloseRequest()
{
    AcceptHartIpHeader();

    // Send a response right away
    // Begin with the header
    COSAL::CMem::Copy(m_tx_buf, s_close_rsp_header, s_close_rsp_header_len);
    m_tx_len = s_close_rsp_header_len;
    // Insert sequence number
    m_tx_buf[4] = (TY_Byte)(m_hart_ip_sequence_number >> 8);
    m_tx_buf[5] = (TY_Byte)(m_hart_ip_sequence_number);
    int result = send(so_client_socket, (const char*)m_tx_buf, m_tx_len, 0);
    m_status = TerminateConnection();
}

void CHSipMacPort::AcceptHartIpKeepAliveRequest()
{
    AcceptHartIpHeader();
}

void CHSipMacPort::SaveNextToDo(EN_ToDo to_do_)
{
    if (to_do_ == EN_ToDo::SEND_RESPONSE)
    {
        // Highest priority
        m_to_do = to_do_;
        return;
    }

    if (to_do_ == EN_ToDo::SEND_BURST)
    {
        if (m_to_do != EN_ToDo::SEND_RESPONSE)
        {
            // Send burst is next priority
            m_to_do = to_do_;
        }
        return;
    }
}

CHSipMacPort::EN_ToDo CHSipMacPort::FetchNextToDo()
{
    EN_ToDo tmp;
    tmp = m_to_do;
    m_to_do = EN_ToDo::NOTHING;
    return tmp;
}

CHSipMacPort::EN_Status CHSipMacPort::TerminateConnection()
{
    SignalNetworkError();
    closesocket(so_client_socket);
    WSACleanup();
    return EN_Status::INITIALIZING;
}


