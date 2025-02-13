/*
 *          File: MonitorSlave.cpp (CMonitor)
 *                The module is providing the DLL functions
 *                which are required for access of the monitor
 *                functionality.
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
#include <stdio.h>
#include <timeapi.h>
#include <minwindef.h>

#include "WbHartSlave.h"
#include "WinSystem.h"
#include "HSuartMacPort.h"
#include "Monitor.h"

#include "BaHartSlave.h"

// Windows specific part
HARTDLL_API void WINAPI BAHASL_InitMonitor()
{
    CMonitor::Init();
}

HARTDLL_API void WINAPI BAHASL_TerminateMonitor()
{
    CMonitor::Terminate();
}

HARTDLL_API void WINAPI BAHASL_StartMonitor()
{
    CMonitor::Start();
}

HARTDLL_API void WINAPI BAHASL_StopMonitor()
{
    CMonitor::Stop();
}

HARTDLL_API EN_Bool WINAPI BAHASL_GetMonitorData(TY_MonFrame* mon_frame_)
{
    return CMonitor::GetData(mon_frame_);
}

HARTDLL_API EN_Bit WINAPI BAHASL_GetMonitorStatus(void)
{
    return CMonitor::GetStatus();
}

HARTDLL_API TY_Word WINAPI BAHASL_GetMonitorAddData(TY_Byte* data_)
{
    return CMonitor::GetPayloadData(data_);
}

