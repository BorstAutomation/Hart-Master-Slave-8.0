/*
 *          File: Monitor.cpp (CMonitor)
 *                On the one hand, there are methods that are mapped to the
 *                interface of the Windows DLL. In addition, there are a
 *                number of functions that are included with the kernel
 *                functions. Since this module is so small overall, the
 *                methods were not implemented in two different files.
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
#include "OSAL.h"
#include "WbHartUser.h"
#include "WinSystem.h"
#include "HartMasterIface.h"
#include <stdio.h>
#include <timeapi.h>
#include <minwindef.h>
#include "BaHartMaster.h"
#include "Monitor.h"

// Windows specific part
HARTDLL_API void WINAPI BAHAMA_InitMonitor()
{
    CMonitor::Init();
}

HARTDLL_API void WINAPI BAHAMA_TerminateMonitor()
{
    CMonitor::Terminate();
}

HARTDLL_API void WINAPI BAHAMA_StartMonitor()
{
    CMonitor::Start();
}

HARTDLL_API void WINAPI BAHAMA_StopMonitor()
{
    CMonitor::Stop();
}

HARTDLL_API EN_Bool WINAPI BAHAMA_GetMonitorData(TY_MonFrame* mon_frame_)
{
    return CMonitor::GetData(mon_frame_);
}

HARTDLL_API EN_Bit WINAPI BAHAMA_GetMonitorStatus(void)
{
    return CMonitor::GetStatus();
}

HARTDLL_API TY_Word WINAPI BAHAMA_GetMonitorAddData(TY_Byte* data_)
{
    return CMonitor::GetPayloadData(data_);
}

