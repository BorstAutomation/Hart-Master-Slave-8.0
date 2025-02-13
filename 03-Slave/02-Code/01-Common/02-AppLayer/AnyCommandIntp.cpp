/*
 *          File: AnyCommandIntp.cpp (CAnyCommandIntp)
 *                Any command interpreter for
 *                common practice and user commands
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
#include "AnyCommandIntp.h"
#include "UniCommandIntp.h"
#include "HartService.h"
#include "HartChannel.h"

 // Data
TY_Byte       CAnyCommandIntp::Command033::m_num_slots = 0;
TY_Byte       CAnyCommandIntp::Command033::m_slots[4] = { 0, 0, 0, 0 };
EN_DevVarCode CAnyCommandIntp::Command054::m_code = (EN_DevVarCode)0;

// Methods
TY_Word CAnyCommandIntp::Execute()
{
    TY_Word command;
    EN_Bool result = EN_Bool::TRUE8;

    if (CService::ActiveService == NULL)
    {
        // Something went wrong
        return 0xffff;
    }

    VerifyDeviceStatus();

    command = CService::ActiveService->Command;

    if (CUniCommandIntp::Execute() == EN_Bool::TRUE8)
    {
        // Command already handled
        return command;
    }

    switch (command)
    {
    case 33:
        if (Command033::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command033::ToResponse();
        }

        break;
    case 34:
        if (Command034::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command034::ToResponse();
        }

        break;
    case 35:
        if (Command035::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command035::ToResponse();
        }

        break;
    case 49:
        if (Command049::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command049::ToResponse();
        }

        break;
    case 54:
        if (Command054::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command054::ToResponse();
        }

        break;
    case 108:
        if (Command108::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command108::ToResponse();
        }

        break;
    case 109:
        if (Command109::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command109::ToResponse();
        }

        break;
    case 512:
        Command512::ToResponse();

        break;
    case 513:
        if (Command513::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command513::ToResponse();
        }

        break;
    default:
        // Command not found
        CService::SendRespErrCode(CHart::CRespCode::CMD_NOT_IMP);
    }

    return command;
}

void CAnyCommandIntp::VerifyDeviceStatus()
{
    if (CService::ActiveService->Request.Host == EN_Master::PRIMARY)
    {
        if (COSAL::CMem::IsEqual(CHartData::CDyn.AddStatus, CHartData::CDyn.PrimaryAddStatus, CHart::CSize::ADD_STAT_LEN) == EN_Bool::FALSE8)
        {
            // Set more status flag
            CHartData::CDyn.DeviceStatusPrimary |= CHart::CDevStatus::MORE_STATUS;
        }
    }
    else
    {
        if (COSAL::CMem::IsEqual(CHartData::CDyn.AddStatus, CHartData::CDyn.SecondarydAddStatus, CHart::CSize::ADD_STAT_LEN) == EN_Bool::FALSE8)
        {
            // Set more status flag
            CHartData::CDyn.DeviceStatusSecondary |= CHart::CDevStatus::MORE_STATUS;
        }
    }
}

// Commands

EN_Error CAnyCommandIntp::Command033::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Read Device Variables
    EN_Error result = EN_Error::NONE;

    result = CUniCommandIntp::PrecheckCommand(1, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    m_num_slots = payload_size_;
    if (m_num_slots > 8)
    {
        m_num_slots = 8;
    }

    // Save the request
    COSAL::CMem::Copy(m_slots, payload_data_, m_num_slots);

    return result;
}

void CAnyCommandIntp::Command033::ToResponse()
{
    TY_Byte index = 0;
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    for (TY_Byte e = 0; e < m_num_slots; e++)
    {
        InsertDevVar((EN_DevVarCode)m_slots[e], &data[index]);
        index += 6;
    }

    CService::ActiveService->Response.PayloadSize = index;
    CService::SendHartIpMessage();
}

EN_Error CAnyCommandIntp::Command034::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Write Primary Variable Damping Value
    EN_Error result = EN_Error::NONE;

    result = CUniCommandIntp::PrecheckCommand(4, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    // Write the value
    CHartData::CStat.Damping = CCoding::PickFloat(0, payload_data_, EN_Endian::MSB_First);

    return result;
}

void CAnyCommandIntp::Command034::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    CCoding::PutFloat(CHartData::CStat.Damping, 0, data, EN_Endian::MSB_First);

    CService::ActiveService->Response.PayloadSize = 4;
    CService::SendHartIpMessage();
}

EN_Error CAnyCommandIntp::Command035::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // 35 Write Primary Variable Range Values
    EN_Error result = EN_Error::NONE;

    result = CUniCommandIntp::PrecheckCommand(9, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    // Write the values
    CHartData::CStat.RangeUnit = payload_data_[0];
    CHartData::CStat.UpperRange = CCoding::PickFloat(1, payload_data_, EN_Endian::MSB_First);
    CHartData::CStat.LowerRange = CCoding::PickFloat(5, payload_data_, EN_Endian::MSB_First);

    return result;
}

void CAnyCommandIntp::Command035::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.RangeUnit;
    CCoding::PutFloat(CHartData::CStat.UpperRange, 1, data, EN_Endian::MSB_First);
    CCoding::PutFloat(CHartData::CStat.LowerRange, 5, data, EN_Endian::MSB_First);

    CService::ActiveService->Response.PayloadSize = 9;
    CService::SendHartIpMessage();
}

EN_Error CAnyCommandIntp::Command049::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Write Primary Variable Transducer Serial Number
    EN_Error result = EN_Error::NONE;

    result = CUniCommandIntp::PrecheckCommand(3, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    // Write the value(s)
    CHartData::CStat.TransdSerNum[0] = payload_data_[0];
    CHartData::CStat.TransdSerNum[1] = payload_data_[1];
    CHartData::CStat.TransdSerNum[2] = payload_data_[2];

    return result;
}

void CAnyCommandIntp::Command049::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.TransdSerNum[0];
    data[1] = CHartData::CStat.TransdSerNum[1];
    data[2] = CHartData::CStat.TransdSerNum[2];

    CService::ActiveService->Response.PayloadSize = 3;
    CService::SendHartIpMessage();
}

EN_Error CAnyCommandIntp::Command054::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Read Device Variable Information
    EN_Error result = EN_Error::NONE;

    result = CUniCommandIntp::PrecheckCommand(1, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    m_code = (EN_DevVarCode)payload_data_[0];

    return result;
}

void CAnyCommandIntp::Command054::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    InsertDevVar(m_code, data);

    CService::ActiveService->Response.PayloadSize = 28;
    CService::SendHartIpMessage();
}

EN_Error CAnyCommandIntp::Command108::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Write Burst Mode Command Number
    EN_Error result = EN_Error::NONE;

    if (payload_size_ < 1)
    {
        CService::SendRespErrCode(CHart::CRespCode::TOO_FEW_DATA);
        result = EN_Error::ERR;
    }
    else
    {
        if ((payload_data_[0] == 1) ||
            (payload_data_[0] == 2) ||
            (payload_data_[0] == 3) ||
            (payload_data_[0] == 9))
        {
            CService::Burst.Command = payload_data_[0];
            CBurst::Config(payload_data_[0]);
        }
        else
        {
            CService::SendRespErrCode(CHart::CRespCode::INVALID_SEL);
            result = EN_Error::ERR;
        }
    }

    return result;
}

void CAnyCommandIntp::Command108::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CService::Burst.Command;
    CService::ActiveService->Response.PayloadSize = 1;
    CService::SendHartIpMessage();
}

EN_Error CAnyCommandIntp::Command109::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Burst Mode Control
    EN_Error result = EN_Error::NONE;

    if (payload_size_ < 1)
    {
        CService::SendRespErrCode(CHart::CRespCode::TOO_FEW_DATA);
        result = EN_Error::ERR;
    }
    else
    {
        if (payload_data_[0] > 1)
        {
            CService::SendRespErrCode(CHart::CRespCode::TOO_FEW_DATA);
            result = EN_Error::ERR;
        }
        else
        {
            if ((payload_data_[0] == 0) && (CHartData::CStat.BurstMode != EN_Bool::FALSE8))
            {
                CHartData::CStat.BurstMode = EN_Bool::FALSE8;
                CChannel::BurstModeChanged = EN_Bool::TRUE8;
            }
            
            if ((payload_data_[0] == 1) && (CHartData::CStat.BurstMode != EN_Bool::TRUE8))
            {
                CHartData::CStat.BurstMode = EN_Bool::TRUE8;
                CChannel::BurstModeChanged = EN_Bool::TRUE8;
            }
        }
    }

    return result;
}

void CAnyCommandIntp::Command109::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = (TY_Byte)CHartData::CStat.BurstMode;
    CService::ActiveService->Response.PayloadSize = 1;
    CService::SendHartIpMessage();
}

void CAnyCommandIntp::Command512::ToResponse()
{
    // Read Dynamic Variable Classifications
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.CountryCode[0];
    data[1] = CHartData::CStat.CountryCode[1];
    data[2] = CHartData::CStat.SiUnitsOnly;

    CService::ActiveService->Response.PayloadSize = 3;
    CService::SendHartIpMessage();
}

EN_Error CAnyCommandIntp::Command513::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    EN_Error result = EN_Error::NONE;

    result = CUniCommandIntp::PrecheckCommand(3, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    if (payload_data_[2] > 1)
    {
        CService::SendRespErrCode(CHart::CRespCode::INVALID_SEL);
        result = EN_Error::ERR;
    }
    else
    {
        CHartData::CStat.CountryCode[0] = payload_data_[0];
        CHartData::CStat.CountryCode[1] = payload_data_[1];
        CHartData::CStat.SiUnitsOnly = payload_data_[2];
    }

    return result;
}

void CAnyCommandIntp::Command513::ToResponse()
{
    // Read Dynamic Variable Classifications
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.CountryCode[0];
    data[1] = CHartData::CStat.CountryCode[1];
    data[2] = CHartData::CStat.SiUnitsOnly;

    CService::ActiveService->Response.PayloadSize = 3;
    CService::SendHartIpMessage();
}


// Command helpers

void CAnyCommandIntp::Command033::InsertDevVar(EN_DevVarCode code_, TY_Byte* buffer_)
{
    buffer_[0] = (TY_Byte)code_;

    switch (code_)
    {
    case EN_DevVarCode::PV1:
    case EN_DevVarCode::PV1_0:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPV1.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV1value, 2, buffer_, EN_Endian::MSB_First);
        break;
    case EN_DevVarCode::PV2:
    case EN_DevVarCode::PV2_1:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPV2.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV2value, 2, buffer_, EN_Endian::MSB_First);
        break;
    case EN_DevVarCode::PV3:
    case EN_DevVarCode::PV3_2:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPV3.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV3value, 2, buffer_, EN_Endian::MSB_First);
        break;
    case EN_DevVarCode::PV4:
    case EN_DevVarCode::PV4_3:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPV4.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PV4value, 2, buffer_, EN_Endian::MSB_First);
        break;
    case EN_DevVarCode::PERCENT:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarPercent.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.PercValue, 2, buffer_, EN_Endian::MSB_First);
        break;
    case EN_DevVarCode::CURRENT:
        buffer_[1] = (TY_Byte)CHartData::CStat.DevVarCurrent.UnitsCode;
        CCoding::PutFloat(CHartData::CDyn.CurrValue, 2, buffer_, EN_Endian::MSB_First);
        break;
    default:
        buffer_[1] = 250;
        buffer_[2] = 0x07f;
        buffer_[3] = 0xa0;
        buffer_[4] = 0x0;
        buffer_[5] = 0x0;
        break;
    }
}

void CAnyCommandIntp::Command054::InsertDevVar(EN_DevVarCode code_, TY_Byte* buffer_)
{
    buffer_[0] = (TY_Byte)code_;

    switch (code_)
    {
    case EN_DevVarCode::PV1:
    case EN_DevVarCode::PV1_0:
        // Transducer serial number
        COSAL::CMem::Set(&buffer_[1], 0, 3);
        buffer_[4] = (TY_Byte)CHartData::CStat.DevVarPV1.UnitsCode;
        // Upper limit
        CCoding::PutFloat(20.0f, 5, buffer_, EN_Endian::MSB_First);
        // Lower limit
        CCoding::PutFloat(0.0f, 9, buffer_, EN_Endian::MSB_First);
        // Damping
        CCoding::PutFloat(CHartData::CStat.Damping, 13, buffer_, EN_Endian::MSB_First);
        // Min span
        CCoding::PutFloat(0.2f, 17, buffer_, EN_Endian::MSB_First);
        buffer_[21] = (TY_Byte)CHartData::CStat.DevVarPV1.Class;
        // Family
        buffer_[22] = 11; // Level
        // Acquisition period
        CCoding::PutDWord(32000, 23, buffer_, EN_Endian::MSB_First);
        // Properties
        buffer_[27] = 0;
        break;
    case EN_DevVarCode::PV2:
    case EN_DevVarCode::PV2_1:
        // Transducer serial number
        COSAL::CMem::Copy(&buffer_[1], CHartData::CStat.TransdSerNum, 3);
        buffer_[4] = (TY_Byte)CHartData::CStat.DevVarPV2.UnitsCode;
        // Upper limit
        CCoding::PutFloat(CHartData::CConst.TransdUpperLimit, 5, buffer_, EN_Endian::MSB_First);
        // Lower limit
        CCoding::PutFloat(CHartData::CConst.TransdLowerLimit, 9, buffer_, EN_Endian::MSB_First);
        // Damping
        CCoding::PutFloat(CHartData::CStat.Damping, 13, buffer_, EN_Endian::MSB_First);
        // Min span
        CCoding::PutFloat(CHartData::CConst.TransdMinSpan, 17, buffer_, EN_Endian::MSB_First);
        buffer_[21] = (TY_Byte)CHartData::CStat.DevVarPV2.Class;
        // Family
        buffer_[22] = 11; // Level
        // Acquisition period
        CCoding::PutDWord(32000, 23, buffer_, EN_Endian::MSB_First);
        // Properties
        buffer_[27] = 0;
        break;
    case EN_DevVarCode::PV3:
    case EN_DevVarCode::PV3_2:
        // Transducer serial number
        COSAL::CMem::Set(&buffer_[1], 0, 3);
        buffer_[4] = (TY_Byte)CHartData::CStat.DevVarPV3.UnitsCode;
        // Upper limit
        CCoding::PutFloat(10.0f, 5, buffer_, EN_Endian::MSB_First);
        // Lower limit
        CCoding::PutFloat(0.0f, 9, buffer_, EN_Endian::MSB_First);
        // Damping
        CCoding::PutFloat(CHartData::CStat.Damping, 13, buffer_, EN_Endian::MSB_First);
        // Min span
        CCoding::PutFloat(0.2f, 17, buffer_, EN_Endian::MSB_First);
        buffer_[21] = (TY_Byte)CHartData::CStat.DevVarPV3.Class;
        // Family
        buffer_[22] = 11; // Level
        // Acquisition period
        CCoding::PutDWord(32000, 23, buffer_, EN_Endian::MSB_First);
        // Properties
        buffer_[27] = 0;
        break;
    case EN_DevVarCode::PV4:
    case EN_DevVarCode::PV4_3:
        // Transducer serial number
        COSAL::CMem::Set(&buffer_[1], 0, 3);
        buffer_[4] = (TY_Byte)CHartData::CStat.DevVarPV4.UnitsCode;
        // Upper limit
        CCoding::PutFloat(100.0f, 5, buffer_, EN_Endian::MSB_First);
        // Lower limit
        CCoding::PutFloat(-20.0f, 9, buffer_, EN_Endian::MSB_First);
        // Damping
        CCoding::PutFloat(CHartData::CStat.Damping, 13, buffer_, EN_Endian::MSB_First);
        // Min span
        CCoding::PutFloat(1.5f, 17, buffer_, EN_Endian::MSB_First);
        buffer_[21] = (TY_Byte)CHartData::CStat.DevVarPV4.Class;
        // Family
        buffer_[22] = 11; // Level
        // Acquisition period
        CCoding::PutDWord(32000, 23, buffer_, EN_Endian::MSB_First);
        // Properties
        buffer_[27] = 0;
        break;
    case EN_DevVarCode::PERCENT:
        // Transducer serial number
        COSAL::CMem::Set(&buffer_[1], 0, 3);
        buffer_[4] = (TY_Byte)CHartData::CStat.DevVarPercent.UnitsCode;
        // Upper limit
        CCoding::PutFloat(120.0f, 5, buffer_, EN_Endian::MSB_First);
        // Lower limit
        CCoding::PutFloat(0.0f, 9, buffer_, EN_Endian::MSB_First);
        // Damping
        CCoding::PutFloat(CHartData::CStat.Damping, 13, buffer_, EN_Endian::MSB_First);
        // Min span
        CCoding::PutFloat(0.5f, 17, buffer_, EN_Endian::MSB_First);
        buffer_[21] = (TY_Byte)CHartData::CStat.DevVarPercent.Class;
        // Family
        buffer_[22] = 11; // Level
        // Acquisition period
        CCoding::PutDWord(32000, 23, buffer_, EN_Endian::MSB_First);
        // Properties
        buffer_[27] = 0;
        break;
    case EN_DevVarCode::CURRENT:
        // Transducer serial number
        COSAL::CMem::Set(&buffer_[1], 0, 3);
        buffer_[4] = (TY_Byte)CHartData::CStat.DevVarCurrent.UnitsCode;
        // Upper limit
        CCoding::PutFloat(22.8f, 5, buffer_, EN_Endian::MSB_First);
        // Lower limit
        CCoding::PutFloat(3.8f, 9, buffer_, EN_Endian::MSB_First);
        // Damping
        CCoding::PutFloat(CHartData::CStat.Damping, 13, buffer_, EN_Endian::MSB_First);
        // Min span
        CCoding::PutFloat(0.1f, 17, buffer_, EN_Endian::MSB_First);
        buffer_[21] = (TY_Byte)CHartData::CStat.DevVarCurrent.Class;
        // Family
        buffer_[22] = 11; // Level
        // Acquisition period
        CCoding::PutDWord(32000, 23, buffer_, EN_Endian::MSB_First);
        // Properties
        buffer_[27] = 0;
        break;
    default:
        // Transducer serial number
        COSAL::CMem::Set(&buffer_[1], 0, 3);
        buffer_[4] = 250;
        // Upper limit
        COSAL::CMem::Copy(&buffer_[5], CHartData::NOT_A_NUMBER, 4);
        // Lower limit
        COSAL::CMem::Copy(&buffer_[9], CHartData::NOT_A_NUMBER, 4);
        // Damping
        COSAL::CMem::Copy(&buffer_[13], CHartData::NOT_A_NUMBER, 4);
        // Min span
        COSAL::CMem::Copy(&buffer_[17], CHartData::NOT_A_NUMBER, 4);
        buffer_[21] = 0;
        // Family
        buffer_[22] = 0;
        // Acquisition period
        CCoding::PutDWord(0xffffffff, 23, buffer_, EN_Endian::MSB_First);
        // Properties
        buffer_[27] = 0;
        break;
    }
}
