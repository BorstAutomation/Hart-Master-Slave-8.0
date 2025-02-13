/*
 *          File: HartData.cpp (CHartData)
 *                Data defined for the Hart commands
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

// Data
TY_ConstDataHart CHartData::CConst;
TY_DynDataHart   CHartData::CDyn;
TY_StatDataHart  CHartData::CStat;
const TY_Byte    CHartData::NOT_A_NUMBER[4] = { 0x7f, 0xa0, 0, 0 };


void CHartData::SetDefaults()
{
    // 'Constant' Data
    // Cmd 0 
    // Generic Fieldcomm Group
    CHartData::CConst.Magic = 254;
    CHartData::CConst.ExpandedDeviceType = 0x00F9;
    CHartData::CConst.HartRevision = 7;
    CHartData::CConst.DeviceRevision = 1;
    CHartData::CConst.SoftwRevision = 2;
    CHartData::CConst.HwRevAndSigCode = (3 << 3) + 0;
    CHartData::CConst.Flags = 0;
    CHartData::CConst.DevUniqueID[0] = 3;
    CHartData::CConst.DevUniqueID[1] = 4;
    CHartData::CConst.DevUniqueID[2] = 5;
    CHartData::CConst.LastDevVarCode = 0;
    CHartData::CConst.ExtendedManuCode = 0x00E0;
    CHartData::CConst.ExtendedLabelCode = 0x00E1;
    CHartData::CConst.DeviceProfile = 0;

    // Dynamic Data
    CHartData::CDyn.ConfigChangeCounter = 0;
    CHartData::CDyn.ExtendedDevStatus = 0;
    CHartData::CDyn.PV1value = 7.5f;
    CHartData::CDyn.PV2value = 1250.0f;
    CHartData::CDyn.PV3value = 1.0f;
    CHartData::CDyn.PV4value = 20.0f;
    CHartData::CDyn.PercValue = 75.0f;
    CHartData::CDyn.CurrValue = 16.0f;
    // Work around setting cold start + more status
    CHartData::CDyn.DeviceStatusPrimary = CHart::CDevStatus::MORE_STATUS | CHart::CDevStatus::COLD_START;
    CHartData::CDyn.DeviceStatusSecondary = CHart::CDevStatus::MORE_STATUS | CHart::CDevStatus::COLD_START;
    CHartData::CDyn.ConfigChangeCounter = 0;
    COSAL::CMem::Set(CHartData::CDyn.AddStatus, 0, CHart::CSize::ADD_STAT_LEN);
    COSAL::CMem::Set(CHartData::CDyn.PrimaryAddStatus, 0, CHart::CSize::ADD_STAT_LEN);
    COSAL::CMem::Set(CHartData::CDyn.SecondarydAddStatus, 0, CHart::CSize::ADD_STAT_LEN);
    COSAL::CMem::Set(CHartData::CDyn.AddStatusMask, 0, CHart::CSize::ADD_STAT_LEN);
    CHartData::CDyn.TimeStamp = COSAL::CTimer::GetTime() * 32;

    // Static Data
    // Interface
    CHartData::CStat.BaudRate = 1200;
    CHartData::CStat.ComPort = 6;
    CHartData::CStat.PollAddress = 0;
    CHartData::CStat.LongAddress[0] = (TY_Byte)((CHartData::CConst.ExpandedDeviceType & 0x3f00) >> 8);
    CHartData::CStat.LongAddress[1] = (TY_Byte)(CHartData::CConst.ExpandedDeviceType & 0xff);
    CHartData::CStat.LongAddress[2] = CHartData::CConst.DevUniqueID[0];
    CHartData::CStat.LongAddress[3] = CHartData::CConst.DevUniqueID[1];
    CHartData::CStat.LongAddress[4] = CHartData::CConst.DevUniqueID[2];
    CHartData::CStat.NumRequestPreambles = 4;
    CHartData::CStat.NumResponsePreambles = 6;
    CHartData::CStat.HartEnabled = EN_Bool::TRUE8;
    CHartData::CStat.BurstMode = EN_Bool::FALSE8;
    CHartData::CStat.WriteProtected = EN_Bool::FALSE8;
    CHartData::CStat.DevVarPercent = {EN_DevVarCode::PERCENT, EN_DevVarClass::NONE, 57, 0 };  // %
    CHartData::CStat.DevVarCurrent = { EN_DevVarCode::CURRENT, EN_DevVarClass::CURRENT, 39, 0 }; // mA
    CHartData::CStat.DevVarPV1 = { EN_DevVarCode::PV1, EN_DevVarClass::LENGTH, 45, 0 };       // meter
    CHartData::CStat.DevVarPV2 = { EN_DevVarCode::PV2, EN_DevVarClass::PRESSURE,  8, 0 };     // mbar
    CHartData::CStat.DevVarPV3 = { EN_DevVarCode::PV3, EN_DevVarClass::DENSITY, 96, 0 };      // kg/l
    CHartData::CStat.DevVarPV4 = { EN_DevVarCode::PV4, EN_DevVarClass::TEMPERATURE, 32, 0 };  // °C
    CHartData::CStat.AlmSelTable6 = 250; // Not used
    CHartData::CStat.TfuncTable3 = 0;    // Linear 
    CHartData::CStat.RangeUnit = 8;      // mBar
    CHartData::CStat.UpperRange = 10.0f;
    CHartData::CStat.LowerRange = 0.0f;
    CHartData::CStat.Damping = 1.0f;
    CHartData::CStat.WrProtCode = 0;     // Off
    CHartData::CStat.ChanFlagsTable26 = 0x81;
    CHartData::CStat.FinAssNum[0] = 0xb;
    CHartData::CStat.FinAssNum[1] = 0xde;
    CHartData::CStat.FinAssNum[2] = 0x31;
    CHartData::CStat.TransdSerNum[0] = 0x12;
    CHartData::CStat.TransdSerNum[1] = 0xd6;
    CHartData::CStat.TransdSerNum[2] = 0x87;
    CHartData::CStat.CountryCode[0] = 'd';
    CHartData::CStat.CountryCode[1] = 'e';
    CHartData::CStat.SiUnitsOnly = 0;
}


