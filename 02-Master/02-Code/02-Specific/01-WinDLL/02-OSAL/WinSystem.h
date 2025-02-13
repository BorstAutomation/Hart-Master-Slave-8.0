/*
 *          File: WinSystem.h (CWinSys)
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

// Once
#ifndef __winsystem_h__
#define __winsystem_h__

#include <Windows.h>
#include "OSAL.h"

typedef struct st_UartPortData
{
    // Note: This is a Windows handle
    HANDLE            Handle;
    TY_Byte         ByteTime;
    EN_Bool           CarrOn;
} ST_UartPortData;

class CWinSys
{
public:
    static void     CyclicTaskStart();
    static void CyclicTaskTerminate();
    static void      CyclicTaskKill();

    class CThread
    {
    public:
        typedef struct st_ThreadContr
        {
            EN_Bool     RunFlag;
            TY_Byte    Priority;
            void      (*Handler)(TY_Word time_);
            EN_Bool  Terminated;
            TY_Word       Cycle;
            TY_DWord LastTimeMs;
        } ST_ThreadContr;

        class CPrio
        {
        public:
            static const TY_Byte  Low = 0;
            static const TY_Byte High = 1;
        };

        // Create cyclically running process
        static EN_Error                    Start(CWinSys::CThread::ST_ThreadContr* task_contr_);
        static void                    Terminate(CWinSys::CThread::ST_ThreadContr* task_contr_);
        static unsigned long __stdcall   Execute(void* data_);
        static void                         Kill();
    private:
        static void* m_handle;
    };

    class CUart
    {
    public:
        static EN_Bool                        Open(TY_Byte com_port_, TY_DWord baudrate_);
        static void                          Close();
        static TY_Len                           Rx(TY_Word max_len_, ST_RcvByte* rcv_bytes_);
        static EN_Error                         Tx(TY_Byte* data_, TY_Word len_);
        static void                   SetCarrierOn();
        static void                  SetCarrierOff();
        //static                  SetConfiguration(TY_DWord baudrate_);
        static EN_Bool                 IsCarrierOn(void);
    protected:
        static void*     m_data;
    };

    class CCyclicTask:public COSAL::CTask
    {
    public:
        EN_Error        Start(void (*handler_)(TY_Word time_)) override;
        EN_Bool  IsTerminated() override;
    };

private:
    // Hide Defaults
    CWinSys();
    ~CWinSys();
    CWinSys(const CWinSys& rhs);              /* Hide copy constructor */
    CWinSys& operator=(const CWinSys& rhs);   /* Hide assignment operator */
};

#endif // __winsystem_h__