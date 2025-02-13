/*
 *          File: OSAL.cpp (COSAL)
 *                The Operating System Abstraction Layer maps general
 *                functions to the operating system.
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

#include <Windows.h>
#include "WbHartSlave.h"
#include "WinSystem.h"
#include "HSuartMacPort.h"

// Local data
static HANDLE         hComPort;
static EN_Bool       wasChrRcv = EN_Bool::FALSE8;

// Local Types 
typedef struct st_ComPortData
{
    HANDLE       port_number;
    TY_Byte        byte_time;
    EN_Bool          carr_on;
} ST_ComPortData;

COSAL::CLock m_lock;

COSAL::CLock::CLock()
{
    m_lock_semaphore = malloc(sizeof(CRITICAL_SECTION));
    if (m_lock_semaphore != NULL)
    {
        memset(m_lock_semaphore, 0, sizeof(CRITICAL_SECTION));
        InitializeCriticalSection((LPCRITICAL_SECTION)m_lock_semaphore);
    }
}

COSAL::CLock::~CLock()
{
    if (m_lock_semaphore != NULL)
    {
        DeleteCriticalSection((LPCRITICAL_SECTION)m_lock_semaphore);
        free(m_lock_semaphore);
    }
}

void COSAL::CLock::Lock()
{
    if (m_lock_semaphore != NULL)
    {
        EnterCriticalSection((LPCRITICAL_SECTION)m_lock_semaphore);
    }
}

void COSAL::CLock::Unlock()
{
    if (m_lock_semaphore != NULL)
    {
        LeaveCriticalSection((LPCRITICAL_SECTION)m_lock_semaphore);
    }
}

void COSAL::Wait(TY_DWord time_)
{
    Sleep(time_);
}

void COSAL::Lock()
{
    m_lock.Lock();
}

void COSAL::Unlock()
{
    m_lock.Unlock();
}

TY_DWord COSAL::CTimer::s_time = 0;

void COSAL::CTimer::InitNoneStatic()
{
    m_locked = EN_Bool::FALSE8;
    m_active = EN_Bool::FALSE8;
    m_start_time = 0;
    m_time_limit = 0;
    m_last_time_limit = 0;
}

void COSAL::CTimer::Start(TY_DWord limit_ms_)
{
    m_locked = EN_Bool::TRUE8;
    m_start_time = COSAL::CTimer::GetTime();
    m_time_limit = limit_ms_;
    m_last_time_limit = limit_ms_;
    m_active = EN_Bool::TRUE8;
    m_locked = EN_Bool::FALSE8;
}

void COSAL::CTimer::Restart()
{
    m_locked = EN_Bool::TRUE8;
    m_start_time = COSAL::CTimer::GetTime();
    m_time_limit = m_last_time_limit;
    m_active = EN_Bool::TRUE8;
    m_locked = EN_Bool::FALSE8;
}

void COSAL::CTimer::Continue(TY_DWord limit_ms_)
{
    m_locked = EN_Bool::TRUE8;
    if (m_time_limit == 0)
    {
        m_start_time = COSAL::CTimer::GetTime();
        m_time_limit = limit_ms_;
    }
    else
    {
        m_time_limit += limit_ms_;
    }
    m_active = EN_Bool::TRUE8;
    m_locked = EN_Bool::FALSE8;
}

void COSAL::CTimer::Stop()
{
    m_active = EN_Bool::FALSE8;
}

EN_Bool COSAL::CTimer::IsExpired()
{
    TY_DWord current_time = COSAL::CTimer::GetTime();

    if (m_active == EN_Bool::FALSE8)
    {
        return EN_Bool::FALSE8;
    }

    if (m_locked == EN_Bool::TRUE8)
    {
        return EN_Bool::FALSE8;
    }

    if ((current_time - m_start_time) > m_time_limit)
    {
        m_active = EN_Bool::FALSE8;
        m_time_limit = 0;
        return EN_Bool::TRUE8;
    }
    return EN_Bool::FALSE8;
}

EN_Bool COSAL::CTimer::IsActive()
{
    return m_active;
}

void COSAL::CTimer::Init()
{
    s_time = 0;
}

TY_DWord COSAL::CTimer::GetTime()
{
    return s_time;
}

TY_DWord COSAL::CTimer::GetTxDuration(TY_Word num_bytes_, TY_DWord baudrate_)
{
    TY_Float num_bytes = (TY_Float)num_bytes_;
    TY_Float baudrate = (TY_Float)baudrate_;
    TY_Float duration = ((num_bytes / baudrate) * 11000.0f + 2.0f);
    return (TY_DWord)duration;
}

TY_DWord COSAL::CTimer::GetByteTime(TY_DWord bitrate_)
{
    switch (bitrate_)
    {
    case COSAL::CBitRates::CBR_Win::BR_1200:
        return COSAL::CBitRates::CByteTime::BT_1200;
        break;
    case COSAL::CBitRates::CBR_Win::BR_2400:
        return COSAL::CBitRates::CByteTime::BT_2400;
        break;
    case COSAL::CBitRates::CBR_Win::BR_4800:
        return COSAL::CBitRates::CByteTime::BT_4800;
        break;
    case COSAL::CBitRates::CBR_Win::BR_9600:
        return COSAL::CBitRates::CByteTime::BT_9600;
        break;
    default:
        return COSAL::CBitRates::CByteTime::BT_Default;
        break;
    }
}

void COSAL::CTimer::UpdateTime(TY_Word time_ms_)
{
    s_time += time_ms_;
}

COSAL::CTask::CTask()
{
    m_terminated = EN_Bool::FALSE8;
}

EN_Error COSAL::CTask::Start(void (*handler_)(TY_Word time_))
{
    return EN_Error::NONE;
}

EN_Bool COSAL::CTask::IsTerminated()
{
    return m_terminated;
}

void COSAL::CMem::Copy(TY_Byte* dst_, const TY_Byte* pu8_Src, TY_DWord u32_Len)
{
    memcpy(dst_, pu8_Src, u32_Len);
}

void COSAL::CMem::Set(TY_Byte* dst_, TY_Byte u8_Val, TY_DWord u32_Len)
{
    memset(dst_, u8_Val, u32_Len);
}

EN_Bool COSAL::CMem::IsEqual(TY_Byte* mem1_, TY_Byte* mem2_, TY_DWord len_)
{
    if (memcmp(mem1_, mem2_, len_) == 0)
    {
        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}

