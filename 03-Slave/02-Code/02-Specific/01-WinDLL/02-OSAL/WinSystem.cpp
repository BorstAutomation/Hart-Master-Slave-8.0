/*
 *          File: WinSystem.cpp (CWinSys)
 *                The OSAL concept cannot be applied to all functions that
 *                are required. These functions were implemented in the code
 *                of this module.
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
#undef UNICODE
#include <winsock2.h>
#include <ws2tcpip.h>
// Need to link with Ws2_32.lib
#pragma comment (lib, "Ws2_32.lib")
// #pragma comment (lib, "Mswsock.lib")
#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT "27015"

// End Winsockets

#include <Windows.h>
#include <stdio.h>
#include <stdlib.h>
#include <timeapi.h>

#include "WbHartSlave.h"
#include "WinSystem.h"
#include "HSuartMacPort.h"
#include "HSipMacPort.h"
#include "HartSlaveIface.h"
#include "HartChannel.h"

static ST_UartPortData uart_port_data = { NULL, 0, EN_Bool::FALSE8 };
static CWinSys::CThread::ST_ThreadContr cyclic_thread_control;
static CWinSys::CCyclicTask CyclicTask;
void*  CWinSys::CThread::m_handle = NULL;
void*  CWinSys::CUart::m_data;

EN_Bool CWinSys::CUart::Open(TY_Byte com_port_, TY_DWord baudrate_)
{
    if (com_port_ == 2)
    {
        // TCP IP server

        WSADATA wsaData;
        int iResult;

        SOCKET ListenSocket = INVALID_SOCKET;
        SOCKET ClientSocket = INVALID_SOCKET;

        struct addrinfo* result = NULL;
        struct addrinfo hints;

        int iSendResult;
        char recvbuf[MAX_TXRX_SIZE];
        int recvbuflen = MAX_TXRX_SIZE;

        // Initialize Winsock
        iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
        if (iResult != 0) {
            printf("WSAStartup failed with error: %d\n", iResult);
            return EN_Bool::FALSE8;
        }

        ZeroMemory(&hints, sizeof(hints));
        hints.ai_family = AF_INET;
        hints.ai_socktype = SOCK_STREAM;
        hints.ai_protocol = IPPROTO_TCP;
        hints.ai_flags = AI_PASSIVE;

        // Resolve the server address and port
        iResult = getaddrinfo("localhost", DEFAULT_PORT, &hints, &result);
        if (iResult != 0) {
            printf("getaddrinfo failed with error: %d\n", iResult);
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        // Create a SOCKET for the server to listen for client connections.
        ListenSocket = socket(result->ai_family, result->ai_socktype, result->ai_protocol);
        if (ListenSocket == INVALID_SOCKET) {
            printf("socket failed with error: %ld\n", WSAGetLastError());
            freeaddrinfo(result);
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        // Setup the TCP listening socket
        iResult = bind(ListenSocket, result->ai_addr, (int)result->ai_addrlen);
        if (iResult == SOCKET_ERROR) {
            printf("bind failed with error: %d\n", WSAGetLastError());
            freeaddrinfo(result);
            closesocket(ListenSocket);
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        freeaddrinfo(result);

        iResult = listen(ListenSocket, SOMAXCONN);
        if (iResult == SOCKET_ERROR) {
            printf("listen failed with error: %d\n", WSAGetLastError());
            closesocket(ListenSocket);
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        // Attention!!! Test only!!!
        WSACleanup();
        return EN_Bool::FALSE8;

        // Accept a client socket
        ClientSocket = accept(ListenSocket, NULL, NULL);
        if (ClientSocket == INVALID_SOCKET) {
            printf("accept failed with error: %d\n", WSAGetLastError());
            closesocket(ListenSocket);
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        // No longer need server socket
        closesocket(ListenSocket);

        // Receive until the peer shuts down the connection
        do {

            iResult = recv(ClientSocket, recvbuf, recvbuflen, 0);
            if (iResult > 0) {
                printf("Bytes received: %d\n", iResult);

                // Echo the buffer back to the sender
                iSendResult = send(ClientSocket, recvbuf, iResult, 0);
                if (iSendResult == SOCKET_ERROR) {
                    printf("send failed with error: %d\n", WSAGetLastError());
                    closesocket(ClientSocket);
                    WSACleanup();
                    return EN_Bool::FALSE8;
                }
                printf("Bytes sent: %d\n", iSendResult);
            }
            else if (iResult == 0)
                printf("Connection closing...\n");
            else {
                printf("recv failed with error: %d\n", WSAGetLastError());
                closesocket(ClientSocket);
                WSACleanup();
                return EN_Bool::FALSE8;
            }

        } while (iResult > 0);

        // shutdown the connection since we're done
        iResult = shutdown(ClientSocket, SD_SEND);
        if (iResult == SOCKET_ERROR) {
            printf("shutdown failed with error: %d\n", WSAGetLastError());
            closesocket(ClientSocket);
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        // cleanup
        closesocket(ClientSocket);
        WSACleanup();

        return EN_Bool::FALSE8;
    }

    TY_Char szPort[15];

    //Open the port at WINAPI
    if (com_port_ > 9)
    {
        sprintf(szPort, "\\\\.\\COM%d", com_port_);
    }
    else
    {
        sprintf(szPort, "COM%d", com_port_);
    }

    uart_port_data.Handle = 
        CreateFile(szPort, GENERIC_READ | GENERIC_WRITE,
            0,                    // exclusive access
            NULL,                 // no security attrs
            OPEN_EXISTING,
            FILE_ATTRIBUTE_NORMAL,
            NULL
        );
    if (uart_port_data.Handle == INVALID_HANDLE_VALUE)
    {
        // Error stop here
        return EN_Bool::FALSE8;
    }
    else
    {
        COMMTIMEOUTS   strCommTimeOuts;
        DCB            dcb;

        //Initialize the com port interface
        uart_port_data.ByteTime = COSAL::CTimer::GetByteTime(baudrate_);
        //get any early notifications
        SetCommMask(uart_port_data.Handle, EV_RXCHAR);
        //setup device buffers
        SetupComm(uart_port_data.Handle, MAX_TXRX_SIZE, MAX_TXRX_SIZE);
        //purge any information in the buffer
        PurgeComm(uart_port_data.Handle,
            PURGE_TXABORT | PURGE_RXABORT |
            PURGE_TXCLEAR | PURGE_RXCLEAR
        );
        //set up for overlapped I/O
        strCommTimeOuts.ReadIntervalTimeout = 20;
        strCommTimeOuts.ReadTotalTimeoutMultiplier = 0;
        strCommTimeOuts.ReadTotalTimeoutConstant = 0;
        strCommTimeOuts.WriteTotalTimeoutMultiplier = 0;
        strCommTimeOuts.WriteTotalTimeoutConstant = 0;
        SetCommTimeouts(uart_port_data.Handle, &strCommTimeOuts);
        //set the com port parameter
        GetCommState(uart_port_data.Handle, &dcb);
        dcb.BaudRate = baudrate_;
        dcb.ByteSize = 8;
        dcb.Parity = ODDPARITY;
        dcb.StopBits = ONESTOPBIT;
        dcb.fBinary = TRUE;
        dcb.fParity = TRUE;
        dcb.fErrorChar = FALSE;
        dcb.ErrorChar = (char)0;
        dcb.fAbortOnError = FALSE;
        dcb.fDtrControl = DTR_CONTROL_ENABLE;
        dcb.fRtsControl = RTS_CONTROL_ENABLE;
        if (SetCommState(uart_port_data.Handle, &dcb) == FALSE)
        {
            Close();
            return EN_Bool::FALSE8;
        }
    }

    return EN_Bool::TRUE8;
}

void CWinSys::CUart::Close()
{
    if (uart_port_data.Handle != INVALID_HANDLE_VALUE)
    {
        COSAL::Unlock();
        CloseHandle(uart_port_data.Handle);
        uart_port_data.Handle = INVALID_HANDLE_VALUE;
        CWinSys::CThread::Terminate(&cyclic_thread_control);
        COSAL::Lock();
    }
}

EN_Bool CWinSys::CUart::Rx(TY_Byte* rx_tx_buffer_, TY_Byte* rx_tx_len_, TY_Byte* rx_tx_err_)
{
    COMSTAT                com_stat;
    int                   read_stat = FALSE;
    DWORD               error_flags = 0;
    // Test only
    // static TY_Byte rx_error_count = 0;

    *rx_tx_err_ = 0;

    // only try to read number of bytes in queue
    ClearCommError(uart_port_data.Handle, &error_flags, &com_stat);
    if (error_flags)
    {
        if (error_flags & CE_FRAME)
            *rx_tx_err_ |= COSAL::CRcvErr::ERR_Frame;
        if (error_flags & CE_OVERRUN)
            *rx_tx_err_ |= COSAL::CRcvErr::ERR_Over;
        if (error_flags & CE_RXPARITY)
            *rx_tx_err_ |= COSAL::CRcvErr::ERR_Parity;
    }

    if (com_stat.cbInQue > 0)
    {
        read_stat = ReadFile(uart_port_data.Handle,
            &rx_tx_buffer_[*rx_tx_len_],
            1,
            NULL,
            NULL);
        if (!read_stat)
        {
            // I/O error ???
            return EN_Bool::FALSE8;
        }

        *rx_tx_len_ += 1;

        // For testing byte errors
        //rx_error_count += 1;
        //if (rx_error_count >= 100)
        //{
        //    *rx_tx_err_ |= CE_RXPARITY;
        //    rx_error_count = 0;
        //}

        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}

EN_Error CWinSys::CUart::Tx(TY_Byte* data_, TY_Word len_)
{
    COMSTAT                com_stat;
    int                   fReadStat = FALSE;
    DWORD               error_flags = 0;
    DWORD                   written = 0;
    DWORD                    length = 0;

    ClearCommError(uart_port_data.Handle, &error_flags, &com_stat);
    WriteFile(uart_port_data.Handle,
        data_,
        len_,
        &written,
        NULL
    );

    return EN_Error::NONE;
}

void CWinSys::CUart::SetCarrierOn()
{
    uart_port_data.CarrOn = EN_Bool::TRUE8;
    EscapeCommFunction(uart_port_data.Handle, SETRTS); //Request to Send
    EscapeCommFunction(uart_port_data.Handle, CLRDTR); //Data Terminal Ready
}

void CWinSys::CUart::SetCarrierOff()
{
    uart_port_data.CarrOn = EN_Bool::FALSE8;
    EscapeCommFunction(uart_port_data.Handle, CLRRTS); //Request to Send
    EscapeCommFunction(uart_port_data.Handle, SETDTR); //Data Terminal Ready
}

EN_Bool CWinSys::CUart::IsCarrierOn(void)
{
    return uart_port_data.CarrOn;
}

void CWinSys::CyclicTaskStart()
{
    // Start the task for the cyclic handler
    CyclicTask.Start(CHartSlave::FastCyclicHandler);
}

void CWinSys::CyclicTaskTerminate()
{
    CWinSys::CThread::Terminate(&cyclic_thread_control);
}

void CWinSys::CyclicTaskKill()
{
    CWinSys::CThread::Kill();
}

EN_Error CWinSys::CThread::Start(CWinSys::CThread::ST_ThreadContr* thread_contr_)
{
    unsigned long ulThreadID;

    thread_contr_->Terminated = EN_Bool::FALSE8;

    m_handle = CreateThread((LPSECURITY_ATTRIBUTES)NULL, //No security attributes
        0,                            //Stack size = default
        CWinSys::CThread::Execute,
        thread_contr_,
        0,                            //init flags (0 for running)
        &ulThreadID
    );
    if (m_handle != NULL)
    {
        if (thread_contr_->Priority == CPrio::High)
        {
            SetThreadPriority(m_handle, THREAD_PRIORITY_TIME_CRITICAL);
        }
        
        // Initialze the central timer
        COSAL::CTimer::Init();

        return EN_Error::NONE;
    }
    else
    {
        return EN_Error::ERR;
    }

    return EN_Error::NONE;
}

void CWinSys::CThread::Terminate(CWinSys::CThread::ST_ThreadContr* task_contr_)
{
    cyclic_thread_control.RunFlag = EN_Bool::FALSE8;
}

void CWinSys::CThread::Kill()
{
    if (m_handle != NULL)
    {
        TerminateThread(m_handle, 44444);
        m_handle = NULL;
    }
}

unsigned long __stdcall CWinSys::CThread::Execute(void* data_)
{
    CWinSys::CThread::ST_ThreadContr* thread_control = (ST_ThreadContr*)data_;

    timeBeginPeriod(thread_control->Cycle);
    thread_control->LastTimeMs = timeGetTime();

    while (thread_control->RunFlag == EN_Bool::TRUE8)
    {
        TY_DWord time = timeGetTime();
        TY_DWord passed_time = time - thread_control->LastTimeMs;

        // Correct passed time in debugging sessions
        if (passed_time > (TY_DWord)(10 * thread_control->Cycle))
        {
            thread_control->LastTimeMs = time;
            continue;
        }

        if (passed_time > 0)
        {
            // Set last time to current time and
            // call the handling routine
            thread_control->LastTimeMs = time;

            if (thread_control->Handler != NULL)
            {
                thread_control->Handler(passed_time);
            }
        }
        else
        {
            passed_time = 0;
        }

        if ((passed_time == thread_control->Cycle) || (passed_time == 0))
        {
            Sleep(thread_control->Cycle);
        }
        else if (passed_time < thread_control->Cycle)
        {
            TY_DWord diff = thread_control->Cycle - passed_time;
            Sleep(thread_control->Cycle + diff);
        }
        else
        {
            TY_DWord diff = passed_time - thread_control->Cycle;
            if (thread_control->Cycle > diff)
            {
                Sleep(thread_control->Cycle - diff);
            }
        }

    }

    thread_control->Terminated = EN_Bool::TRUE8;

    timeEndPeriod(thread_control->Cycle);
    return 9136;
}

EN_Error CWinSys::CCyclicTask::Start(void (*handler_)(TY_Word time_))
{
    // Start the thread for the uart handler
    cyclic_thread_control.RunFlag = EN_Bool::TRUE8;
    cyclic_thread_control.Handler = CHartSlave::FastCyclicHandler;
    cyclic_thread_control.Terminated = EN_Bool::FALSE8;
    cyclic_thread_control.Priority = 1;
    cyclic_thread_control.Cycle = 1;

    return CWinSys::CThread::Start(&cyclic_thread_control);
}

EN_Bool CWinSys::CCyclicTask::IsTerminated()
{
    if (cyclic_thread_control.Terminated == EN_Bool::TRUE8)
    {
        m_terminated = EN_Bool::TRUE8;
    }

    return m_terminated;
}

void CHartSlave::FastCyclicHandler(TY_Word time_ms_)
{
#ifdef DEBUG_TIMING
    static TY_DWord  last_time = timeGetTime();
    static TY_DWord   time_diff[101];
    static TY_DWord  time_diff2[101];
    static TY_DWord  start_time = 0;
    static TY_Byte        count = 0;
    static TY_DWord         sum = 0;
    static TY_DWord        sum2 = 0;
    static TY_DWord        time = timeGetTime();
    static TY_DWord        diff = 0;

    time = timeGetTime();

    if (count == 0)
    {
        start_time = time;
        last_time = time;
        sum = 0;
        sum2 = 0;
        count = 1;
        for (int e = 0; e < 101; e++)
        {
            time_diff[e] = 0;
            time_diff2[e] = 0;
        }
    }
    else
    {
        diff = time - last_time;
        time_diff[count - 1] = diff;
        time_diff2[count - 1] = time_ms_;
        sum += diff;
        sum2 += time_ms_;
        count++;
    }

    last_time = time;

    if ((time >= (start_time + 100)) || (count >= 100))
    {
        TY_DWord local_time = COSAL::CTimer::GetTime();
        
        // Set a breakpoint here to check the correct timing
        count = 0;
    }

    /// Additional test in debugging region still required
#endif // DEBUG_TIMING

    // Call the Hart master protocol handler
    if (CChannel::GetCommType() == EN_CommType::HART_IP)
    {
        CHSipMacPort::Execute(time_ms_);
    }
    else
    {
        CHSuartMacPort::Execute(time_ms_);
    }
    /// Test
    //if (time_ms_ > 3)
    //{
    //    time_ms_ = 2;
    //}
}
