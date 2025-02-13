/*
 *          File: HartBurst.cpp (CBurst)
 *                Handling of the burst mode from the
 *                perspective of the application.

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
#include "HartBurst.h"
#include "HartService.h"
#include "HartDevice.h"

void CBurst::Config(TY_Byte command_)
{
    if (command_ == 1)
    {
        TY_Byte* data = CService::Burst.PayloadData;

        data[0] = CHartData::CStat.DevVarPV1.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV1value, 1, data, EN_Endian::MSB_First);
        CService::Burst.PayloadSize = 5;
    }
    else if (command_ == 2)
    {
        TY_Byte* data = CService::Burst.PayloadData;

        CCoding::PutFloat(CHartData::CDyn.CurrValue, 0, data, EN_Endian::MSB_First);
        CCoding::PutFloat(CHartData::CDyn.PercValue, 4, data, EN_Endian::MSB_First);
        CService::Burst.PayloadSize = 8;
    }
    else if (command_ == 3)
    {
        TY_Byte* data = CService::Burst.PayloadData;

        CCoding::PutFloat(CHartData::CDyn.CurrValue, 0, data, EN_Endian::MSB_First);
        data[4] = CHartData::CStat.DevVarPV1.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV1value, 5, data, EN_Endian::MSB_First);
        data[9] = CHartData::CStat.DevVarPV2.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV2value, 10, data, EN_Endian::MSB_First);
        data[14] = CHartData::CStat.DevVarPV3.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV3value, 15, data, EN_Endian::MSB_First);
        data[19] = CHartData::CStat.DevVarPV4.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV4value, 20, data, EN_Endian::MSB_First);
        CService::Burst.PayloadSize = 24;
    }
    else
    {
        TY_Byte* data = CService::Burst.PayloadData;

        data[0] = CHartData::CDyn.ExtendedDevStatus;

        // Dev var alert may be  generated
        data[0] |= InsertDevVar(EN_DevVarCode::PV1, &data[1]);
        data[0] |= InsertDevVar(EN_DevVarCode::PV2, &data[9]);
        data[0] |= InsertDevVar(EN_DevVarCode::PV3, &data[17]);
        data[0] |= InsertDevVar(EN_DevVarCode::PV4, &data[25]);
        data[0] |= InsertDevVar(EN_DevVarCode::CURRENT, &data[33]);
        data[0] |= InsertDevVar(EN_DevVarCode::PERCENT, &data[41]);

        // Append time stamp
        CCoding::PutDWord(CHartData::CDyn.TimeStamp, 49, data, EN_Endian::MSB_First);

        CService::Burst.PayloadSize = 53;
    }
}

void CBurst::Launch()
{
    if (CService::Burst.Command == 9)
    {
        CDevice::UpdateTimeStamp();
        //  Put the time stamp into the right position
        CCoding::PutDWord(CHartData::CDyn.TimeStamp, 49, CService::Burst.PayloadData, EN_Endian::MSB_First);
    }

    CService::Burst.Encode();
}

// Helpers

TY_Byte CBurst::InsertDevVar(EN_DevVarCode code_, TY_Byte* buffer_)
{
    buffer_[0] = (TY_Byte)code_;

    switch (code_)
    {
    case EN_DevVarCode::PV1:
    case EN_DevVarCode::PV1_0:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPV1.Class;
        buffer_[2] = (TY_Byte)CHartData::CStat.DevVarPV1.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV1value, 3, buffer_, EN_Endian::MSB_First);
        buffer_[7] = (TY_Byte)CHartData::CDyn.PV1valueStatus;
        if ((buffer_[7] & 0xC0) == 0)
        {
            return (TY_Byte)EN_ExtDevStatusBits::DEV_VAR_ALERT;
        }
        break;
    case EN_DevVarCode::PV2:
    case EN_DevVarCode::PV2_1:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPV2.Class;
        buffer_[2] = (TY_Byte)CHartData::CStat.DevVarPV2.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV2value, 3, buffer_, EN_Endian::MSB_First);
        buffer_[7] = (TY_Byte)CHartData::CDyn.PV2valueStatus;
        if ((buffer_[7] & 0xC0) == 0)
        {
            return (TY_Byte)EN_ExtDevStatusBits::DEV_VAR_ALERT;
        }
        break;
    case EN_DevVarCode::PV3:
    case EN_DevVarCode::PV3_2:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPV3.Class;
        buffer_[2] = (TY_Byte)CHartData::CStat.DevVarPV3.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV3value, 3, buffer_, EN_Endian::MSB_First);
        buffer_[7] = (TY_Byte)CHartData::CDyn.PV3valueStatus;
        if ((buffer_[7] & 0xC0) == 0)
        {
            return (TY_Byte)EN_ExtDevStatusBits::DEV_VAR_ALERT;
        }
        break;
    case EN_DevVarCode::PV4:
    case EN_DevVarCode::PV4_3:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPV4.Class;
        buffer_[2] = (TY_Byte)CHartData::CStat.DevVarPV4.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV4value, 3, buffer_, EN_Endian::MSB_First);
        buffer_[7] = (TY_Byte)CHartData::CDyn.PV4valueStatus;
        if ((buffer_[7] & 0xC0) == 0)
        {
            return (TY_Byte)EN_ExtDevStatusBits::DEV_VAR_ALERT;
        }
        break;
    case EN_DevVarCode::PERCENT:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPercent.Class;
        buffer_[2] = (TY_Byte)CHartData::CStat.DevVarPercent.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PercValue, 3, buffer_, EN_Endian::MSB_First);
        buffer_[7] = (TY_Byte)CHartData::CDyn.PercValueStatus;
        if ((buffer_[7] & 0xC0) == 0)
        {
            return (TY_Byte)EN_ExtDevStatusBits::DEV_VAR_ALERT;
        }
        break;
    case EN_DevVarCode::CURRENT:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarCurrent.Class;
        buffer_[2] = (TY_Byte)CHartData::CStat.DevVarCurrent.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.CurrValue, 3, buffer_, EN_Endian::MSB_First);
        buffer_[7] = (TY_Byte)CHartData::CDyn.CurrValueStatus;
        if ((buffer_[7] & 0xC0) == 0)
        {
            return (TY_Byte)EN_ExtDevStatusBits::DEV_VAR_ALERT;
        }
        break;
    }

    return 0;
}
