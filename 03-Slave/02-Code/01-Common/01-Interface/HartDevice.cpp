/*
 *          File: HartDevice.cpp (CDevice)
 *                Module used for some settings in the scope of the device.
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

#include "WbHartSlave.h"
#include "HartDevice.h"
#include "HartService.h"

void CDevice::Init()
{
    CHartData::SetDefaults();
    CService::Burst.Type = CFrame::EN_Type::BURST;
    CService::Burst.AddrMode = 1;
    CService::Burst.Command = 1;
    CService::Burst.Host = EN_Master::PRIMARY;
    CBurst::Config(1);
}

void CDevice::UpdateTimeStamp()
{
    CHartData::CDyn.TimeStamp = COSAL::CTimer::GetTime() * 32;
}

