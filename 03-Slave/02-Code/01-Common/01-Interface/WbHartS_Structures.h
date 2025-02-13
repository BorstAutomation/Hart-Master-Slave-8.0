/*
 *          File: WbHartS_Structures.h
 *                This file contains structures which are accessed 
 *                at the outer interface as well as in some modules
 *                in the slave kernel.
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

#include "HartConsts.h"

// Once
#ifndef __wbharts_structures_h__
#define __wbharts_structures_h__

// Structure Definitions
#pragma pack(push, 1)
typedef struct ty_dev_var_data
{
    EN_DevVarCode  Code;
    EN_DevVarClass Class;
    TY_Byte        UnitsCode;
    TY_Byte        Status;
}
TY_DevVarData;
#pragma pack(pop)

// Const Data
#pragma pack(push, 1)
typedef struct ty_const_data_hart
{
    // Cmd 0
    TY_Byte  Magic;
    TY_Word  ExpandedDeviceType;
    TY_Byte  HartRevision;
    TY_Byte  DeviceRevision;
    TY_Byte  SoftwRevision;
    TY_Byte  HwRevAndSigCode;
    TY_Byte  Flags;
    TY_Byte  DevUniqueID[3];
    TY_Byte  LastDevVarCode;
    TY_Word  ExtendedManuCode;
    TY_Word  ExtendedLabelCode;
    TY_Byte  DeviceProfile;
    TY_Byte  TransdUnit;
    TY_Float TransdUpperLimit;
    TY_Float TransdLowerLimit;
    TY_Float TransdMinSpan;
}
TY_ConstDataHart;
#pragma pack(pop)

// Dynamic Data
#pragma pack(push, 1)
typedef struct ty_dyn_data_hart
{
    TY_Word  ConfigChangeCounter;
    TY_Byte  ExtendedDevStatus;
    TY_Float PV1value;
    TY_Float PV2value;
    TY_Float PV3value;
    TY_Float PV4value;
    TY_Float PercValue;
    TY_Float CurrValue;
    TY_Byte  PV1valueStatus;
    TY_Byte  PV2valueStatus;
    TY_Byte  PV3valueStatus;
    TY_Byte  PV4valueStatus;
    TY_Byte  PercValueStatus;
    TY_Byte  CurrValueStatus;
    TY_Byte  DeviceStatusPrimary;
    TY_Byte  DeviceStatusSecondary;
    TY_Byte  AddStatus[CHart::CSize::ADD_STAT_LEN];
    TY_Byte  PrimaryAddStatus[CHart::CSize::ADD_STAT_LEN];
    TY_Byte  SecondarydAddStatus[CHart::CSize::ADD_STAT_LEN];
    TY_Byte  AddStatusMask[CHart::CSize::ADD_STAT_LEN];
    TY_DWord TimeStamp;
}
TY_DynDataHart;
#pragma pack(pop)

// Static Data
#pragma pack(push, 1)
typedef struct ty_stat_data_hart
{
    // Interface
    TY_DWord BaudRate;
    TY_Byte  ComPort;
    TY_Byte  PollAddress;
    TY_Byte  LongAddress[5];
    TY_Byte  NumRequestPreambles;
    TY_Byte  NumResponsePreambles;
    EN_Bool  HartEnabled;
    EN_Bool  BurstMode;
    EN_Bool  WriteProtected;
    // Others    
    TY_Byte  ShortTag[6]; // Packed ascii
    TY_Byte  LongTag[32];
    TY_Byte  Descriptor[12]; // Packed ascii
    TY_Byte  Day;
    TY_Byte  Month;
    TY_Byte  Year;
    TY_Byte  Message[24]; // Packed ascii
    TY_DevVarData  DevVarPercent; // 244
    TY_DevVarData  DevVarCurrent; // 245
    TY_DevVarData  DevVarPV1;     // 246
    TY_DevVarData  DevVarPV2;     // 247
    TY_DevVarData  DevVarPV3;     // 248
    TY_DevVarData  DevVarPV4;     // 249
    TY_DevVarData  DevVarNone;    // 250
    TY_Byte  AlmSelTable6;
    TY_Byte  TfuncTable3;
    TY_Byte  RangeUnit;
    TY_Float UpperRange;
    TY_Float LowerRange;
    TY_Float Damping;
    TY_Byte  WrProtCode;
    TY_Byte  ChanFlagsTable26;
    TY_Byte  FinAssNum[3];
    TY_Byte  TransdSerNum[3];
    TY_Byte  CountryCode[2];
    TY_Byte  SiUnitsOnly;

    // Hart IP
    TY_Byte     HartIpHostName[MAX_STRING_LEN];
    TY_Byte      HartIpAddress[MAX_STRING_LEN];
    TY_Byte         HartIpPort[MAX_STRING_LEN];
    EN_Bool   HartIpUseAddress;
    EN_Bool  HartIpDataChanged;
}
TY_StatDataHart;
#pragma pack(pop)

#endif // __wbharts_structures_h__