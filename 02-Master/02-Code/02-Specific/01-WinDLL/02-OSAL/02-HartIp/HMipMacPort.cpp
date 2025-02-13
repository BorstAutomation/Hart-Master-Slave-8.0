/*
 *          File: HMipMacPort.cpp (CHMipMacPort)
 *                The Execute method is called directly by the fast cyclic
 *                handler. This basically drives all status machines in
 *                the Hart implementation. Here too, the method is divided
 *                into an Event handler and a ToDo handler.
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
#include "WinSystem.h"
#include "WbHartUser.h"
#include "HMipMacPort.h"
#include "HMipProtocol.h"
#include "HMipLayer2.h"
#include "WbHartM_Structures.h"
#include "Monitor.h"
#include "HartChannel.h"

// Data
CHMipMacPort::EN_Status      CHMipMacPort::Status = CHMipMacPort::EN_Status::IDLE;
TY_Byte                      CHMipMacPort::m_rcv_buf[MAX_IP_TXRX_SIZE];
int                          CHMipMacPort::m_rcv_len = 0;
TY_Byte                      CHMipMacPort::m_tx_buf[MAX_IP_TXRX_SIZE];
int                          CHMipMacPort::m_tx_len = 0;
TY_Byte                      CHMipMacPort::m_hart_ip_data[MAX_IP_TXRX_SIZE];
TY_Word                      CHMipMacPort::m_hart_ip_len;
CHMipMacPort::EN_LastError   CHMipMacPort::m_last_error = CHMipMacPort::EN_LastError::NONE;
TY_Byte                      CHMipMacPort::m_rx_err = 0;
CHMipMacPort::EN_HartIP_Info CHMipMacPort::m_last_hart_ip_info;
CHMipMacPort::EN_ToDo        CHMipMacPort::m_to_do;
EN_Bool                      CHMipMacPort::m_close_request = EN_Bool::FALSE8;
TY_Byte                      CHMipMacPort::m_hart_rx_data[MAX_TXRX_SIZE];
TY_Byte                      CHMipMacPort::m_hart_rx_len = 0;
TY_Byte                      CHMipMacPort::m_hart_ip_version = 0;
TY_Byte                      CHMipMacPort::m_hart_ip_message_type = 0;
TY_Byte                      CHMipMacPort::m_hart_ip_message_id = 0;
TY_Byte                      CHMipMacPort::m_hart_ip_comm_status = 0;
TY_Word                      CHMipMacPort::m_hart_ip_request_seq_number;
TY_Word                      CHMipMacPort::m_hart_ip_response_seq_number;
TY_Word                      CHMipMacPort::m_hart_ip_burst_seq_number;
TY_Word                      CHMipMacPort::m_hart_ip_nak_seq_number;
TY_Word                      CHMipMacPort::m_hart_ip_received_seq_number;
TY_Word                      CHMipMacPort::m_hart_ip_byte_count = 0;
TY_Word                      CHMipMacPort::m_magic_number = 0xe0a3;
TY_DWord                     CHMipMacPort::m_ms_counter = 0;
                                                                    //   1, 60000
TY_Byte                      CHMipMacPort::m_initiate_req_data[5] = { 0x01, 0x00, 0x09, 0x27, 0xc7 };


// WinSockets
// Data
static WSADATA so_wsa_data;
static addrinfo *so_result = NULL;
static addrinfo *so_ptr = NULL;
static addrinfo so_addrinfo;
static SOCKET so_server_socket = INVALID_SOCKET;
static SOCKET so_listen_socket = INVALID_SOCKET;

// Hart Ip
                                              // Ver      Type       ID         Stat  Sequence    ByteCount
static TY_Byte        s_intiate_req_header[] = { MAX_VER, MSGTY_REQ, MSGID_INI, 0x00, 0x00, 0x00, 0x00, 13 };
static int       s_initiate_req_header_len = 8;
static TY_Byte          s_close_req_header[] = { MAX_VER, MSGTY_REQ, MSGID_CLS, 0x00, 0x00, 0x00, 0x00,  8 };
static int          s_close_req_header_len = 8;
static TY_Byte     s_keep_alive_req_header[] = { MAX_VER, MSGTY_REQ, MSGID_ALI, 0x00, 0x00, 0x00, 0x00,  8 };
static int     s_keep_alive_req_header_len = 8;
static TY_Byte                s_req_header[] = { MAX_VER, MSGTY_REQ, MSGID_PDU, 0x00, 0x00, 0x00, 0x00,  8 };
static int                s_req_header_len = 8;

// Public Methods
void CHMipMacPort::Init()
{
    CHMipL2SM::Init();
}
EN_Bool CHMipMacPort::Open(TY_Byte* host_name_, TY_Byte* port_, EN_CommType type_)
{
    // Start the thread for the cyclic handler
    Status = EN_Status::INITIALIZING;
    m_last_error = EN_LastError::NONE;
    CWinSys::CyclicTaskStart();
    return EN_Bool::TRUE8;
}
void CHMipMacPort::Close()
{
    // Try to tell the cyclic thread to close
    // the connection
    m_close_request = EN_Bool::TRUE8;
    // Wait for the thread
    COSAL::Wait(50);
    if (Status != EN_Status::IDLE)
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
    if (so_server_socket != INVALID_SOCKET)
    {
        closesocket(so_server_socket);
        so_server_socket = INVALID_SOCKET;
    }

    WSACleanup();
    m_close_request = EN_Bool::FALSE8;
    Status = EN_Status::IDLE;
}
void CHMipMacPort::Execute(TY_Word time_ms_)
{
    // Note: This procedure is called every ms as long as the channel is open

    EN_HartIP_Info hart_ip_info = EN_HartIP_Info::NO_TRAFFIC;

    COSAL::CTimer::UpdateTime(time_ms_);
    m_ms_counter += time_ms_;

    switch (Status)
    {
    case EN_Status::IDLE:
        // Do nothing
        break;
    case EN_Status::INITIALIZING:
        CHMipL2SM::Init();
        if (InitializeSocketHandler() == EN_Bool::TRUE8)
        {
            Status = EN_Status::WAIT_CONNECT;
        }

        break;
    case EN_Status::WAIT_CONNECT:
        ConnectToServer();
        break;
    case EN_Status::WAIT_INITIATE_RESPONSE:
        if (ReceiveNetworkMessage() == EN_HartIP_Info::INITIATE_RESPONSE)
        {
            Status = EN_Status::CLIENT_READY;
        }

        break;
    case EN_Status::CLIENT_READY:
        if (m_close_request == EN_Bool::TRUE8)
        {
            Status = SendCloseRequest();
            m_close_request = EN_Bool::FALSE8;
            break;
        }

        hart_ip_info = ReceiveNetworkMessage();
        if (hart_ip_info == EN_HartIP_Info::BURST)
        {
            // Keep alive not necessary
            m_ms_counter = 0;
            AcceptHartBurst();
        }
        else if (hart_ip_info == EN_HartIP_Info::NO_TRAFFIC)
        {
            EN_ToDo to_do = FetchNextToDo();

            if (to_do == EN_ToDo::SEND_REQUEST)
            {
                Status = SendCommandRequest();
            }
            else if (m_ms_counter >= 5000)
            {
                m_ms_counter = 0;
                if (CChannel::HartIpSendKeepAlive == EN_Bool::TRUE8)
                {
                    Status = SendKeepAliveRequest();
                }
            }
        }

        break;
    case EN_Status::WAIT_COMMAND_RESPONSE:
        hart_ip_info = ReceiveNetworkMessage();
        if (hart_ip_info == EN_HartIP_Info::COMMAND_RESPONSE)
        {
            AcceptCommandResponse();
            Status = EN_Status::CLIENT_READY;
        }
        else if (hart_ip_info == EN_HartIP_Info::NAK_RESPONSE)
        {
            RejectCommandResponse();
            Status = EN_Status::CLIENT_READY;
        }

        break;
    case EN_Status::WAIT_ALIVE_RESPONSE:
        hart_ip_info = ReceiveNetworkMessage();
        if (hart_ip_info == EN_HartIP_Info::KEEP_ALIVE_RESPONSE)
        {
            AcceptKeepAliveResponse();
            Status = EN_Status::CLIENT_READY;
        }

        if (m_ms_counter > 1000)
        {
            m_last_error = EN_LastError::KEEP_ALIVE;

        }

        break;
    case EN_Status::SHUTTING_DOWN:
        // HandleConnectionClosing();
        closesocket(so_server_socket);
        so_server_socket = INVALID_SOCKET;
        WSACleanup();

        CWinSys::CyclicTaskTerminate();
        Status = EN_Status::IDLE;
    }
}
TY_Word CHMipMacPort::GetStatus()
{
    // .15 .14 .13 .12 .11 .10 .09 .08 .07 .06 .05 .04 .03 .02 .01 .00
    //  +   +   +   +--- Status ----+   +-------- Last Error -------+
    //  |   |   +-- tbd
    //  |   +------ tbd
    //  +---------- tbd

    return (TY_Word)((((TY_Byte)Status & 0x1f) << 8) + (TY_Byte)m_last_error);
}
TY_Word CHMipMacPort::GetMagicNumber()
{
    return m_magic_number;
}
TY_Word CHMipMacPort::GetSequenceNumber(TY_Byte msg_type_)
{
    switch (msg_type_)
    {
    case (TY_Byte)EN_Msg_Type::REQUEST:
        return m_hart_ip_request_seq_number;
        break;
    case (TY_Byte)EN_Msg_Type::RESPONSE:
        return m_hart_ip_response_seq_number;
        break;
    case (TY_Byte)EN_Msg_Type::BURST:
        return m_hart_ip_burst_seq_number;
        break;
    case (TY_Byte)EN_Msg_Type::NAK:
        return m_hart_ip_nak_seq_number;
        break;
    default:
        return m_hart_ip_received_seq_number;
        break;
    }
}
void CHMipMacPort::SetSequenceNumber(TY_Byte msg_type_)
{
    switch (msg_type_)
    {
    case (TY_Byte)EN_Msg_Type::REQUEST:
        m_hart_ip_request_seq_number++;
        m_hart_ip_response_seq_number = m_hart_ip_request_seq_number;
        break;
    case (TY_Byte)EN_Msg_Type::RESPONSE:
        m_hart_ip_response_seq_number = m_hart_ip_response_seq_number;
        break;
    case (TY_Byte)EN_Msg_Type::BURST:
        m_hart_ip_burst_seq_number = m_hart_ip_received_seq_number;
        break;
    case (TY_Byte)EN_Msg_Type::NAK:
        m_hart_ip_nak_seq_number = m_hart_ip_response_seq_number;
        break;
    default:
        break;
    }
}
void CHMipMacPort::GetIpFrameForMonitor(TY_Byte* dst_, TY_Byte* dst_len_, TY_Byte* src_, TY_Byte src_len_, TY_Byte msg_type_)
{
    TY_Word del_pos = 0;
    TY_Word word;
    TY_Word sequence_number = CHMipMacPort::GetSequenceNumber(msg_type_);

    COSAL::CMem::Set(dst_, 0, MAX_TXRX_SIZE);

    word = CHMipMacPort::GetMagicNumber();
    dst_[0] = (TY_Byte)(word >> 8);
    dst_[1] = (TY_Byte)(word);
    dst_[2] = msg_type_;
    word = GetSequenceNumber(msg_type_);
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
TY_Word CHMipMacPort::GetPayloadData(TY_Byte* data_)
{
    COSAL::CMem::Copy(data_, m_hart_ip_data, m_hart_ip_len);
    return m_hart_ip_len;
}
// Private Methods
EN_Bool CHMipMacPort::InitializeSocketHandler()
{
    int result;

    WSACleanup();
    so_listen_socket = INVALID_SOCKET;
    so_server_socket = INVALID_SOCKET;
    so_result = NULL;
    so_ptr = NULL;

    result = WSAStartup(MAKEWORD(2, 2), &so_wsa_data);
    if (result != 0) {
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
        if (CChannel::HartIpUseAddress == EN_Bool::TRUE8)
        {
            result = getaddrinfo((const char*)CChannel::HartIpAddress, (const char*)CChannel::HartIpPort, &so_addrinfo, &so_result);
        }
        else
        {
            result = getaddrinfo((const char*)CChannel::HartIpHostName, (const char*)CChannel::HartIpPort, &so_addrinfo, &so_result);
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
EN_Bool CHMipMacPort::ConnectToServer()
{
    int result;
    int error_code = 0;
    // Set time_out to 50 ms
    timeval rcv_to = { 2, 50000 };

    // Attempt to connect to an address until one succeeds
    for (so_ptr = so_result; so_ptr != NULL; so_ptr = so_ptr->ai_next) {

        // Create a SOCKET for connecting to server
        so_server_socket = socket(so_ptr->ai_family, so_ptr->ai_socktype,
            so_ptr->ai_protocol);
        if (so_server_socket == INVALID_SOCKET)
        {
            // Socket failed with error.
            Status = TerminateConnection(EN_LastError::CREATE_SOCKET);
            return EN_Bool::FALSE8;
        }

        // Try connect to server.
        result = connect(so_server_socket, so_ptr->ai_addr, (int)so_ptr->ai_addrlen);
        if (result == SOCKET_ERROR) {
            closesocket(so_server_socket);
            so_server_socket = INVALID_SOCKET;
            continue;
        }

        break;
    }

    freeaddrinfo(so_result);

    if (so_server_socket == INVALID_SOCKET)
    {
        Status = TerminateConnection(EN_LastError::NO_SERVER);
        return EN_Bool::FALSE8;
    }
    else
    {
        // Set timeout of the server socket
        if (setsockopt(so_server_socket, SOL_SOCKET, SO_RCVTIMEO, (char*)&rcv_to, sizeof(rcv_to)) == SOCKET_ERROR)
        {
            Status = TerminateConnection(EN_LastError::SET_TIMEOUT);
            return EN_Bool::FALSE8;
        }

        // Send an initiate request
        Status = SendInitiateRequest();
    }

    return EN_Bool::TRUE8;
}
CHMipMacPort::EN_Status CHMipMacPort::SendInitiateRequest()
{
    SetSequenceNumber(MSGTY_REQ);
    TY_Word sequence_number = GetSequenceNumber(MSGTY_REQ);

    // Begin with the header
    COSAL::CMem::Copy(m_tx_buf, s_intiate_req_header, s_initiate_req_header_len);
    // Add payload data
    COSAL::CMem::Copy(&m_tx_buf[s_initiate_req_header_len], m_initiate_req_data, 5);
    m_tx_len = 13;
    // Insert sequence number
    m_tx_buf[4] = (TY_Byte)(sequence_number >> 8);
    m_tx_buf[5] = (TY_Byte)(sequence_number);

    int result = send(so_server_socket, (const char*)m_tx_buf, m_tx_len, 0);
    if (result == m_tx_len)
    {
        return EN_Status::WAIT_INITIATE_RESPONSE;
    }
    else
    {
        return TerminateConnection(EN_LastError::TX_FAILED);
    }
}
void CHMipMacPort::AcceptInitiateResponse()
{
    // tbd?
}
CHMipMacPort::EN_Status CHMipMacPort::SendKeepAliveRequest()
{
    int result;

    SetSequenceNumber(MSGTY_REQ);
    TY_Word sequence_number = GetSequenceNumber(MSGTY_REQ);
    // Encode the request
    COSAL::CMem::Copy(m_tx_buf, s_keep_alive_req_header, s_keep_alive_req_header_len);
    m_tx_len = s_keep_alive_req_header_len;
    // Update sequence number
    m_tx_buf[4] = (TY_Byte)(sequence_number >> 8);
    m_tx_buf[5] = (TY_Byte)(sequence_number);

    // Send a the keep alive request
    result = send(so_server_socket, (const char*)m_tx_buf, m_tx_len, 0);
    if (result == SOCKET_ERROR)
    {
        return TerminateConnection(EN_LastError::TX_FAILED);
    }

    return EN_Status::WAIT_ALIVE_RESPONSE;
}
void CHMipMacPort::AcceptKeepAliveResponse()
{

}
CHMipMacPort::EN_Status CHMipMacPort::SendCommandRequest()
{
    int result;

    // Prepare the hart ip payload
    TY_Word     del_pos = 0;
    TY_Word      tx_len = 0;
    TY_Word         idx = 0;
    TY_Word payload_len = 0;


    TY_Byte* tx_data = CHMipL2SM::GetTxData(&tx_len);
    // Find delimiter
    for (TY_Word i = 0; i < tx_len; i++)
    {
        if (tx_data[i] != 0xff)
        {
            del_pos = i;
            break;
        }
    }

    SetSequenceNumber(MSGTY_REQ);
    TY_Word sequence_number = GetSequenceNumber(MSGTY_REQ);
    // Begin with the header
    COSAL::CMem::Copy(m_tx_buf, s_req_header, s_req_header_len);
    idx = s_req_header_len;
    // Add the hart ip payload
    COSAL::CMem::Copy(&m_tx_buf[idx], &tx_data[del_pos], tx_len - del_pos);
    payload_len = tx_len - del_pos;
    m_tx_len = s_req_header_len + tx_len - del_pos;
    m_tx_buf[4] = (TY_Byte)(sequence_number >> 8);
    m_tx_buf[5] = (TY_Byte)(sequence_number);
    payload_len = payload_len + 8;
    m_tx_buf[6] = (TY_Byte)(payload_len >> 8);
    m_tx_buf[7] = (TY_Byte)(payload_len);
    CMonitor::SetAdditionalData(m_tx_buf, m_tx_len);
    // Send the paket
    result = send(so_server_socket, (const char*)m_tx_buf, m_tx_len, 0);
    SignalHartTxDone();
    if (result == SOCKET_ERROR)
    {
        return TerminateConnection(EN_LastError::TX_FAILED);
    }

    return EN_Status::WAIT_COMMAND_RESPONSE;
}
void CHMipMacPort::AcceptCommandResponse()
{
    m_hart_ip_version = m_rcv_buf[0];
    m_hart_ip_message_type = m_rcv_buf[1];
    m_hart_ip_message_id = m_rcv_buf[2];
    m_hart_ip_comm_status = m_rcv_buf[3];
    m_hart_ip_received_seq_number = (TY_Word)((m_rcv_buf[4] << 8) + m_rcv_buf[5]);
    m_hart_ip_byte_count = (TY_Word)((m_rcv_buf[6] << 8) + m_rcv_buf[7]);

    // Copy frame  to the hart context
    COSAL::CMem::Set(m_hart_rx_data, 0, MAX_TXRX_SIZE);
    m_hart_rx_len = (TY_Byte)(m_rcv_len - HART_IP_HEADER_LEN);
    COSAL::CMem::Copy(m_hart_rx_data, &m_rcv_buf[HART_IP_HEADER_LEN], m_hart_rx_len);
    // Call the protocol state machine
    CHMipMacPort::EN_ToDo todo = CHMipProtocol::EventHandler(CHMipProtocol::EN_Event::HART_IP_DATA_RECEIVED, m_hart_rx_data, m_hart_rx_len);
}
void CHMipMacPort::RejectCommandResponse()
{
    m_hart_ip_version = m_rcv_buf[0];
    m_hart_ip_message_type = m_rcv_buf[1];
    m_hart_ip_message_id = m_rcv_buf[2];
    m_hart_ip_comm_status = m_rcv_buf[3];
    m_hart_ip_received_seq_number = (TY_Word)((m_rcv_buf[4] << 8) + m_rcv_buf[5]);
    m_hart_ip_byte_count = (TY_Word)((m_rcv_buf[6] << 8) + m_rcv_buf[7]);

    // Copy frame  to the hart context
    COSAL::CMem::Set(m_hart_rx_data, 0, MAX_TXRX_SIZE);
    m_hart_rx_len = (TY_Byte)(m_rcv_len - HART_IP_HEADER_LEN);
    COSAL::CMem::Copy(m_hart_rx_data, &m_rcv_buf[HART_IP_HEADER_LEN], m_hart_rx_len);
    // Call the protocol state machine
    CHMipMacPort::EN_ToDo todo = CHMipProtocol::EventHandler(CHMipProtocol::EN_Event::REQUEST_REJECTED, m_hart_rx_data, m_hart_rx_len);
}
void CHMipMacPort::AcceptHartBurst()
{
    if (m_rcv_len < 9)
    {
        return;
    }

    // Get the hart ip header details
    m_hart_ip_version = m_rcv_buf[0];
    m_hart_ip_message_type = m_rcv_buf[1];
    m_hart_ip_message_id = m_rcv_buf[2];
    m_hart_ip_comm_status = m_rcv_buf[3];
    m_hart_ip_burst_seq_number = (TY_Word)((m_rcv_buf[4] << 8) + m_rcv_buf[5]);
    m_hart_ip_byte_count = (TY_Word)((m_rcv_buf[6] << 8) + m_rcv_buf[7]);

    SetSequenceNumber((TY_Byte)EN_Msg_Type::BURST);
    // Copy frame  to the hart context
    COSAL::CMem::Set(m_hart_rx_data, 0, MAX_TXRX_SIZE);
    m_hart_rx_len = (TY_Byte)(m_rcv_len - HART_IP_HEADER_LEN);
    COSAL::CMem::Copy(m_hart_rx_data, &m_rcv_buf[HART_IP_HEADER_LEN], m_hart_rx_len);
    // Call the protocol state machine
    CHMipMacPort::EN_ToDo todo = CHMipProtocol::EventHandler(CHMipProtocol::EN_Event::HART_IP_DATA_RECEIVED, m_hart_rx_data, m_hart_rx_len);

    return;
}
CHMipMacPort::EN_Status CHMipMacPort::SendCloseRequest()
{
    int result;

    SetSequenceNumber((TY_Byte)EN_Msg_Type::REQUEST);
    TY_Word sequence_number = GetSequenceNumber(MSGTY_REQ);

    // Encode the request
    COSAL::CMem::Copy(m_tx_buf, s_close_req_header, s_close_req_header_len);
    m_tx_len = s_close_req_header_len;
    // Update sequence number
    m_tx_buf[4] = (TY_Byte)(sequence_number >> 8);
    m_tx_buf[5] = (TY_Byte)(sequence_number);
    // Send a the close request
    result = send(so_server_socket, (const char*)m_tx_buf, m_tx_len, 0);

    return EN_Status::SHUTTING_DOWN;
}
void CHMipMacPort::AcceptCloseResponse()
{
    // tbd ?
}
EN_Bool CHMipMacPort::HandleConnectionClosing()
{
    int result;

    // Receive until the peer closes the connection
    do {
        m_rcv_len = MAX_IP_TXRX_SIZE;
        result = recv(so_server_socket, (char*)(&m_rcv_buf), m_rcv_len, 0);
        if (result > 0)
            // Bytes received
            m_rcv_len = result;
        else if (result == 0)
            // Connection closed
            m_rcv_len = 0;
        else
            m_last_error = EN_LastError::SHUTDOWN;

    } while (result > 0);

    if (result == 0)
    {
        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}
// Receive from the network
CHMipMacPort::EN_HartIP_Info CHMipMacPort::ReceiveNetworkMessage()
{
    int result;
    int error_code = 0;
    EN_HartIP_Info hart_ip_info = EN_HartIP_Info::NO_HART_IP;

    m_rcv_len = MAX_IP_TXRX_SIZE;
    result = recv(so_server_socket, (char*)&m_rcv_buf, m_rcv_len, MSG_PEEK);
    if (result > 0)
    {
        // Take the message out of the buffer in the sockets
        result = recv(so_server_socket, (char*)&m_rcv_buf, m_rcv_len, 0);
    }

    if (result == SOCKET_ERROR)
    {
        error_code = WSAGetLastError();
        if (error_code == WSAETIMEDOUT)
        {
            // Time out (50 ms) bursts may be sent
            // (published) by the slave
            if (Status == EN_Status::CLIENT_READY)
            {
                SaveNextToDo(SignalHartSilence());
            }

            return EN_HartIP_Info::NO_TRAFFIC;
        }
        else
        {
            Status = TerminateConnection(EN_LastError::RECEIVING);
            return EN_HartIP_Info::NET_ERR;
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
            if (m_rcv_buf[IDX_TYPE] == (TY_Byte)EN_Msg_Type::BURST)
            {
                if ((m_rcv_buf[IDX_ID] == (TY_Byte)EN_Msg_ID::HART_PDU) &&
                    (m_rcv_len > 8))
                {
                    hart_ip_info = EN_HartIP_Info::BURST;
                }
            }
            else if (m_rcv_buf[IDX_TYPE] == (TY_Byte)EN_Msg_Type::RESPONSE)
            {
                // What kind of a response?
                if ((m_rcv_buf[IDX_ID] == (TY_Byte)EN_Msg_ID::INITIATE) &&
                    (m_rcv_len == 13))
                {
                    COSAL::CMem::Copy(m_initiate_req_data, &m_rcv_buf[IDX_PDU], 5);
                    hart_ip_info = EN_HartIP_Info::INITIATE_RESPONSE;
                }
                else if ((m_rcv_buf[IDX_ID] == (TY_Byte)EN_Msg_ID::CLOSE) &&
                    (m_rcv_len == 8))
                {
                    hart_ip_info = EN_HartIP_Info::CLOSE_RESPONSE;
                }
                else if ((m_rcv_buf[IDX_ID] == (TY_Byte)EN_Msg_ID::HART_PDU) &&
                    (m_rcv_len > 8))
                {
                    hart_ip_info = EN_HartIP_Info::COMMAND_RESPONSE;
                }
                else if ((m_rcv_buf[IDX_ID] == (TY_Byte)EN_Msg_ID::KEEP_ALIVE) &&
                    (m_rcv_len == 8))
                {
                    hart_ip_info = EN_HartIP_Info::KEEP_ALIVE_RESPONSE;
                }
            }
            else if (m_rcv_buf[IDX_TYPE] == (TY_Byte)EN_Msg_Type::NAK)
            {
                hart_ip_info = EN_HartIP_Info::NAK_RESPONSE;
            }
        }
    }

    return hart_ip_info;
}
// Signal to the Hart protocol
CHMipMacPort::EN_ToDo CHMipMacPort::SignalHartPDUreceiving()
{
    AcceptCommandResponse();
    // Call the protocol state machine
    CHMipMacPort::EN_ToDo todo = CHMipProtocol::EventHandler(CHMipProtocol::EN_Event::HART_IP_DATA_RECEIVED, m_hart_rx_data, m_hart_rx_len);
    return todo;
}
CHMipMacPort::EN_ToDo CHMipMacPort::SignalHartSilence()
{
    CHMipMacPort::EN_ToDo todo = CHMipProtocol::EventHandler(CHMipProtocol::EN_Event::SILENCE_DETECTED, m_hart_rx_data, m_hart_rx_len);

    return todo;
}
void CHMipMacPort::SignalHartTxDone()
{
    CHMipProtocol::EventHandler(CHMipProtocol::EN_Event::HART_IP_TX_DONE, NULL, NULL);
}
void CHMipMacPort::SignalNetworkError()
{
    CHMipProtocol::EventHandler(CHMipProtocol::EN_Event::NETWORK_ERROR, NULL, NULL);
}
CHMipMacPort::EN_Status CHMipMacPort::TerminateConnection(EN_LastError last_err_)
{
    SignalNetworkError();
    closesocket(so_server_socket);
    WSACleanup();
    m_last_error = last_err_;
    return EN_Status::INITIALIZING;
}
void CHMipMacPort::SaveNextToDo(EN_ToDo to_do_)
{
    if (to_do_ == EN_ToDo::SEND_REQUEST)
    {
        m_to_do = to_do_;
    }

    // Test
    if (m_to_do == EN_ToDo::NOTHING)
    {
        m_to_do = to_do_;
    }
}
CHMipMacPort::EN_ToDo CHMipMacPort::FetchNextToDo()
{
    EN_ToDo tmp;
    tmp = m_to_do;
    m_to_do = EN_ToDo::NOTHING;
    return tmp;
}
