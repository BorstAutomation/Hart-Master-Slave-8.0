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
#include <winsock2.h>
#include <ws2tcpip.h>
// Need to link with Ws2_32.lib, Mswsock.lib, and Advapi32.lib
#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")
#define DEFAULT_BUFLEN 512
#define DEFAULT_PORT "27015"
// End Winsockets

#include <Windows.h>
#include "OSAL.h"
#include "WbHartUser.h"
#include "WbHartM_Structures.h"
#include "WinSystem.h"
#include "HartMasterIface.h"
#include <stdlib.h>
#include <stdio.h>
#include <timeapi.h>
#include <minwindef.h>
#include "HMuartMacPort.h"
#include "HMipMacPort.h"
#include "Monitor.h"

static ST_UartPortData uart_port_data = { NULL, 0, EN_Bool::FALSE8 };
static CWinSys::CThread::ST_ThreadContr cyclic_thread_control;
static CWinSys::CCyclicTask CyclicTask;
void*  CWinSys::CThread::m_handle = NULL;
void*  CWinSys::CUart::m_data;

EN_Bool CWinSys::CUart::Open(TY_Byte com_port_, TY_DWord baudrate_)
{
    if (com_port_ == 2)
    {
        WSADATA wsaData;
        SOCKET ConnectSocket = INVALID_SOCKET;
        struct addrinfo* result = NULL,
            * ptr = NULL,
            hints;
        const char* sendbuf = "this is a test";
        char recvbuf[DEFAULT_BUFLEN];
        int iResult;
        int recvbuflen = DEFAULT_BUFLEN;

        // Initialize Winsock
        iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
        if (iResult != 0) {
            printf("WSAStartup failed with error: %d\n", iResult);
            return EN_Bool::FALSE8;
        }

        ZeroMemory(&hints, sizeof(hints));
        hints.ai_family = AF_UNSPEC;
        hints.ai_socktype = SOCK_STREAM;
        hints.ai_protocol = IPPROTO_TCP;

        // Resolve the server address and port
        iResult = getaddrinfo(NULL, DEFAULT_PORT, &hints, &result);
        if (iResult != 0) {
            printf("getaddrinfo failed with error: %d\n", iResult);
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        // Attempt to connect to an address until one succeeds
        for (ptr = result; ptr != NULL; ptr = ptr->ai_next) {

            // Create a SOCKET for connecting to server
            ConnectSocket = socket(ptr->ai_family, ptr->ai_socktype,
                ptr->ai_protocol);
            if (ConnectSocket == INVALID_SOCKET) {
                printf("socket failed with error: %ld\n", WSAGetLastError());
                WSACleanup();
                return EN_Bool::FALSE8;
            }

            // Connect to server.
            iResult = connect(ConnectSocket, ptr->ai_addr, (int)ptr->ai_addrlen);
            if (iResult == SOCKET_ERROR) {
                closesocket(ConnectSocket);
                ConnectSocket = INVALID_SOCKET;
                continue;
            }
            break;
        }

        freeaddrinfo(result);

        if (ConnectSocket == INVALID_SOCKET) {
            printf("Unable to connect to server!\n");
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        // Send an initial buffer
        iResult = send(ConnectSocket, sendbuf, (int)strlen(sendbuf), 0);
        if (iResult == SOCKET_ERROR) {
            printf("send failed with error: %d\n", WSAGetLastError());
            closesocket(ConnectSocket);
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        printf("Bytes Sent: %ld\n", iResult);

        // shutdown the connection since no more data will be sent
        iResult = shutdown(ConnectSocket, SD_SEND);
        if (iResult == SOCKET_ERROR) {
            printf("shutdown failed with error: %d\n", WSAGetLastError());
            closesocket(ConnectSocket);
            WSACleanup();
            return EN_Bool::FALSE8;
        }

        // Receive until the peer closes the connection
        do {

            iResult = recv(ConnectSocket, recvbuf, recvbuflen, 0);
            if (iResult > 0)
                printf("Bytes received: %d\n", iResult);
            else if (iResult == 0)
                printf("Connection closed\n");
            else
                printf("recv failed with error: %d\n", WSAGetLastError());

        } while (iResult > 0);

        // cleanup
        closesocket(ConnectSocket);
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
        SetupComm(uart_port_data.Handle, 256, 256);
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
        COSAL::Lock();
    }
}

TY_Len CWinSys::CUart::Rx(TY_Word max_len_, ST_RcvByte* rcv_bytes_)
{
    COMSTAT                com_stat;
    int                   read_stat = FALSE;
    DWORD               error_flags = 0;
    DWORD                    length = 0;
    TY_Byte                     err = 0;

    // only try to read number of bytes in queue
    ClearCommError(uart_port_data.Handle, &error_flags, &com_stat);
    if (error_flags)
    {
        if (error_flags & CE_FRAME)
            err |= COSAL::CRcvErr::ERR_Frame;
        if (error_flags & CE_OVERRUN)
            err |= COSAL::CRcvErr::ERR_Over;
        if (error_flags & CE_RXPARITY)
            err |= COSAL::CRcvErr::ERR_Parity;
    }

    length = min(max_len_, com_stat.cbInQue);
    if (length > 0)
    {
        TY_Byte  data[MAX_TXRX_SIZE];
        TY_Word        e;
        TY_DWord time = COSAL::CTimer::GetTime() - (length * uart_port_data.ByteTime);
        DWORD    in_len = length;

        read_stat = ReadFile(uart_port_data.Handle,
            data,
            length,
            &length,
            NULL);
        if (!read_stat)
        {
            // I/O error ???
            return 0;
        }

        // Handle the received data bytes
        for (e = 0; e < length; e++)
        {
            rcv_bytes_[e].Data = data[e];
            rcv_bytes_[e].Error = err;
            rcv_bytes_[e].Time = time;
            time += uart_port_data.ByteTime;
        }
    }

    return (TY_Len)length;
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
    CyclicTask.Start(CHartMaster::FastCyclicHandler);
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
        TerminateThread(m_handle, 33333);
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
    cyclic_thread_control.Handler = CHartMaster::FastCyclicHandler;
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

void CHartMaster::FastCyclicHandler(TY_Word time_ms_)
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
        CHMipMacPort::Execute(time_ms_);
    }
    else
    {
        CHMuartMacPort::Execute(time_ms_);
    }
    /// Test
    //if (time_ms_ > 3)
    //{
    //    time_ms_ = 2;
    //}
}
