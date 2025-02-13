/*
 *          File: UniCommandIntp.cpp (CUniCommandIntp)
 *                Universal commands interpreter
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
#include "UniCommandIntp.h"
#include "HartService.h"

// Data
TY_Byte CUniCommandIntp::Command009::m_num_slots = 0;
TY_Byte CUniCommandIntp::Command009::m_slots[8] = { 0, 0, 0, 0, 0, 0, 0, 0 };

// Methods
EN_Bool CUniCommandIntp::Execute()
{
    if (CService::ActiveService == NULL)
    {
        return EN_Bool::FALSE8;
    }

    TY_Word command = CService::ActiveService->Command;
    EN_Bool result = EN_Bool::TRUE8;

    switch (command)
    {
    case 0:
        Command000::ToResponse();
        break;
    case 1:
        Command001::ToResponse();
        break;
    case 2:
        Command002::ToResponse();
        break;
    case 3:
        Command003::ToResponse();
        break;
    case 6:
        if (Command006::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command006::ToResponse();
        }

        break;
    case 7:
        Command007::ToResponse();
        break;
    case 8:
        Command008::ToResponse();
        break;
    case 9:
        if (Command009::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command009::ToResponse();
        }

        break;
    case 11:
        Command012::ToResponse();
        break;
    case 12:
        Command012::ToResponse();
        break;
    case 13:
        Command013::ToResponse();
        break;
    case 14:
        Command014::ToResponse();
        break;
    case 15:
        Command015::ToResponse();
        break;
    case 16:
        Command016::ToResponse();
        break;
    case 17:
        if (Command017::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command017::ToResponse();
        }

        break;
    case 18:
        if (Command018::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command018::ToResponse();
        }

        break;
    case 19:
        if (Command019::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command019::ToResponse();
        }

        break;
    case 20:
        Command020::ToResponse();
        break;
    case 21:
        Command021::ToResponse();
        break;
    case 22:
        if (Command022::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command022::ToResponse();
        }

        break;
    case 38:
        if (Command038::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command038::ToResponse();
        }

        break;

    case 48:
        if (Command048::FromRequest(CService::ActiveService->Request.PayloadData,
            (TY_Byte)CService::ActiveService->Request.PayloadCount) == EN_Error::NONE)
        {
            Command048::ToResponse();
        }

        break;

    default:
        // Command not found
        result = EN_Bool::FALSE8;
    }

    return result;
}

// Prolog
EN_Error CUniCommandIntp::PrecheckCommand(TY_Byte min_payload_, TY_Byte payload_size_)
{
    EN_Error result = EN_Error::NONE;

    if (CHartData::CStat.WriteProtected == EN_Bool::TRUE8)
    {
        CService::SendRespErrCode(CHart::CRespCode::WRITE_PROT);
        result = EN_Error::ERR;
    }
    if (payload_size_ < min_payload_)
    {
        CService::SendRespErrCode(CHart::CRespCode::TOO_FEW_DATA);
        result = EN_Error::ERR;
    }

    return result;
}

// Command handling
void CUniCommandIntp::Command000::ToResponse()
{
    // Read Unique Identifier
    SendUniqueIdResponse();
}

void CUniCommandIntp::Command001::ToResponse()
{
    // Read Primary Variable
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.DevVarPV1.UnitsCode;
    CCoding::PutFloat(CHartData::CDyn.PV1value, 1, data, EN_Endian::MSB_First);
    CService::ActiveService->Response.PayloadSize = 5;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command002::ToResponse()
{
    // Read Loop Current And Percent Of Range
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    CCoding::PutFloat(CHartData::CDyn.CurrValue, 0, data, EN_Endian::MSB_First);
    CCoding::PutFloat(CHartData::CDyn.PercValue, 4, data, EN_Endian::MSB_First);
    CService::ActiveService->Response.PayloadSize = 8;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command003::ToResponse()
{
    // Read Dynamic Variables And Loop Current
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    CCoding::PutFloat(CHartData::CDyn.CurrValue, 0, data, EN_Endian::MSB_First);
    data[4] = CHartData::CStat.DevVarPV1.UnitsCode;
    CCoding::PutFloat(CHartData::CDyn.PV1value, 5, data, EN_Endian::MSB_First);
    data[9] = CHartData::CStat.DevVarPV2.UnitsCode;
    CCoding::PutFloat(CHartData::CDyn.PV2value, 10, data, EN_Endian::MSB_First);
    data[14] = CHartData::CStat.DevVarPV3.UnitsCode;
    CCoding::PutFloat(CHartData::CDyn.PV3value, 15, data, EN_Endian::MSB_First);
    data[19] = CHartData::CStat.DevVarPV4.UnitsCode;
    CCoding::PutFloat(CHartData::CDyn.PV4value, 20, data, EN_Endian::MSB_First);
    CService::ActiveService->Response.PayloadSize = 24;
    CService::SendHartIpMessage();
}

EN_Error CUniCommandIntp::Command006::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Write Polling Address
    EN_Error result = EN_Error::NONE;

    result = PrecheckCommand( 1, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    if (payload_data_[0] > 63)
    {
        CService::SendRespErrCode(CHart::CRespCode::INVALID_SEL);
        result = EN_Error::ERR;
    }
    else
    {
        CHartData::CStat.PollAddress = payload_data_[0];
        CService::SetConfigChangedFlag();
    }

    return result;
}

void CUniCommandIntp::Command006::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.PollAddress;
    data[1] = 11;
    CService::ActiveService->Response.PayloadSize = 2;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command007::ToResponse()
{
    // Read Loop Configuration
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.PollAddress;
    data[1] = 1;
    CService::ActiveService->Response.PayloadSize = 2;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command008::ToResponse()
{
    // Read Dynamic Variable Classifications
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = (TY_Byte)CHartData::CStat.DevVarPV1.Class;
    data[1] = (TY_Byte)CHartData::CStat.DevVarPV2.Class;
    data[2] = (TY_Byte)CHartData::CStat.DevVarPV3.Class;
    data[3] = (TY_Byte)CHartData::CStat.DevVarPV4.Class;
    CService::ActiveService->Response.PayloadSize = 4;
    CService::SendHartIpMessage();
}

EN_Error CUniCommandIntp::Command009::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Read Device Variables with Status
    EN_Error result = EN_Error::NONE;

    result = PrecheckCommand(1, payload_size_);
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

void CUniCommandIntp::Command009::ToResponse()
{
    TY_Byte index = 1;
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CDyn.ExtendedDevStatus;

    for (TY_Byte e = 0; e < m_num_slots; e++)
    {
        // Dev var alert may be  generated
        data[0] |= InsertDevVar((EN_DevVarCode)m_slots[e], &data[index]);
        index += 8;
    }

    // Append time stamp
    CCoding::PutDWord(CHartData::CDyn.TimeStamp, index, data, EN_Endian::MSB_First);

    CService::ActiveService->Response.PayloadSize = index + 4;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command011::ToResponse()
{
    // Read Unique Identifier Associated With Tag
    // Note: The request is already handled in the Hart protocol
    SendUniqueIdResponse();
}

void CUniCommandIntp::Command012::ToResponse()
{
    // Read Message
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    CCoding::PutOctets(CHartData::CStat.Message, 24, 0, data);
    CService::ActiveService->Response.PayloadSize = 24;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command013::ToResponse()
{
    // Read Tag, Descriptor, Date
    SendTagDescrDateResponse();
}

void CUniCommandIntp::Command014::ToResponse()
{
    // Read Primary Variable Transducer Information
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.TransdSerNum[0];
    data[1] = CHartData::CStat.TransdSerNum[1];
    data[2] = CHartData::CStat.TransdSerNum[2];
    data[3] = CHartData::CConst.TransdUnit;
    CCoding::PutFloat(CHartData::CConst.TransdUpperLimit, 0, &data[4], EN_Endian::MSB_First);
    CCoding::PutFloat(CHartData::CConst.TransdLowerLimit, 0, &data[8], EN_Endian::MSB_First);
    CCoding::PutFloat(CHartData::CConst.TransdMinSpan, 0, &data[12], EN_Endian::MSB_First);

    CService::ActiveService->Response.PayloadSize = 16;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command015::ToResponse()
{
    // Read Device Information
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.AlmSelTable6;
    data[1] = CHartData::CStat.TfuncTable3;
    data[2] = CHartData::CStat.RangeUnit;
    CCoding::PutFloat(CHartData::CStat.UpperRange, 0, &data[3], EN_Endian::MSB_First);
    CCoding::PutFloat(CHartData::CStat.LowerRange, 0, &data[7], EN_Endian::MSB_First);
    CCoding::PutFloat(CHartData::CStat.Damping, 0, &data[11], EN_Endian::MSB_First);
    data[15] = CHartData::CStat.WrProtCode;
    data[16] = 250;
    data[17] = CHartData::CStat.ChanFlagsTable26;

    CService::ActiveService->Response.PayloadSize = 18;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command016::ToResponse()
{
    // Read Final Assembly Number
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = CHartData::CStat.FinAssNum[0];
    data[1] = CHartData::CStat.FinAssNum[1];
    data[2] = CHartData::CStat.FinAssNum[2];

    CService::ActiveService->Response.PayloadSize = 3;
    CService::SendHartIpMessage();
}

EN_Error CUniCommandIntp::Command017::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Write Message
    EN_Error result = EN_Error::NONE;

    result = PrecheckCommand(24, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    // Write the message
    COSAL::CMem::Copy(CHartData::CStat.Message, payload_data_, 24);

    return result;
}

void CUniCommandIntp::Command017::ToResponse()
{
    // Write Tag, Descriptor, Date
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    CCoding::PutOctets(CHartData::CStat.Message, 24, 0, data);
    CService::ActiveService->Response.PayloadSize = 24;
    CService::SendHartIpMessage();
}

EN_Error CUniCommandIntp::Command018::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Write Final Assembly Number
    EN_Error result = EN_Error::NONE;

    result = PrecheckCommand(21, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    COSAL::CMem::Copy(CHartData::CStat.ShortTag, payload_data_, 6);
    COSAL::CMem::Copy(CHartData::CStat.Descriptor, &payload_data_[6], 12);
    CHartData::CStat.Day = payload_data_[18];
    CHartData::CStat.Month = payload_data_[19];
    CHartData::CStat.Year = payload_data_[20];
    return result;
}

void CUniCommandIntp::Command018::ToResponse()
{
    SendTagDescrDateResponse();
}

EN_Error CUniCommandIntp::Command019::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Write Final Assembly Number
    EN_Error result = EN_Error::NONE;

    result = PrecheckCommand(3, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    COSAL::CMem::Copy(CHartData::CStat.FinAssNum, payload_data_, 3);
    return result;
}

void CUniCommandIntp::Command019::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    COSAL::CMem::Copy(data, CHartData::CStat.FinAssNum, 3);
    CService::ActiveService->Response.PayloadSize = 3;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command020::ToResponse()
{
    // Read Long Tag
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    CCoding::PutString(CHartData::CStat.LongTag, 32, 0, data);
    CService::ActiveService->Response.PayloadSize = 32;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::Command021::ToResponse()
{
    // Read Unique Identifier Associated With Long Tag
    // Note: The request is already handled in the Hart protocol
    SendUniqueIdResponse();
}

EN_Error CUniCommandIntp::Command022::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Write Long Tag Name
    EN_Error result = EN_Error::NONE;

    result = PrecheckCommand(32, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    COSAL::CMem::Copy(CHartData::CStat.LongTag, payload_data_, 32);
    return result;
}

void CUniCommandIntp::Command022::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    COSAL::CMem::Copy(data, CHartData::CStat.LongTag, 32);
    CService::ActiveService->Response.PayloadSize = 32;
    CService::SendHartIpMessage();
}

EN_Error CUniCommandIntp::Command038::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Reset Configuration Changed Flag
    EN_Error result = EN_Error::NONE;

    result = PrecheckCommand(0, payload_size_);
    if (result == EN_Error::ERR)
    {
        return result;
    }

    CService::ClearConfigChangedFlag(CService::ActiveService->Request.Host);

    return result;
}

void CUniCommandIntp::Command038::ToResponse()
{
    //TY_Byte* data = CService::ActiveService->Response.PayloadData;

    CService::ActiveService->Response.PayloadSize = 0;
    CService::SendHartIpMessage();
}

EN_Error CUniCommandIntp::Command048::FromRequest(TY_Byte* payload_data_, TY_Byte payload_size_)
{
    // Read Additional Device Status
    EN_Error result = EN_Error::NONE;

    if (payload_size_ > 0)
    {
        result = PrecheckCommand(CHart::CSize::ADD_STAT_LEN, payload_size_);
        if (result == EN_Error::ERR)
        {
            return result;
        }

        if (CService::ActiveService->Request.Host == EN_Master::PRIMARY)
        {
            if (COSAL::CMem::IsEqual(payload_data_,
                &CHartData::CDyn.AddStatus[0], CHart::CSize::ADD_STAT_LEN) == EN_Bool::TRUE8)
            {
                // Delete more status flag
                CHartData::CDyn.DeviceStatusPrimary &= ~CHart::CDevStatus::MORE_STATUS;
            }
            else
            {
                // The new status will be sent. Make a backup copy for verification.
                COSAL::CMem::Copy(CHartData::CDyn.PrimaryAddStatus, CHartData::CDyn.AddStatus, CHart::CSize::ADD_STAT_LEN);
            }
        }
        else
        {
            if (COSAL::CMem::IsEqual(payload_data_,
                CHartData::CDyn.AddStatus, CHart::CSize::ADD_STAT_LEN) == EN_Bool::TRUE8)
            {
                // Delete more status flag
                CHartData::CDyn.DeviceStatusSecondary &= ~CHart::CDevStatus::MORE_STATUS;
            }
            else
            {
                // The new status will be sent. Make a backup copy for verification.
                COSAL::CMem::Copy(CHartData::CDyn.SecondarydAddStatus, CHartData::CDyn.AddStatus, CHart::CSize::ADD_STAT_LEN);
            }
        }
    }
    else
    {
        if (CService::ActiveService->Request.Host == EN_Master::PRIMARY)
        {
            // Delete primary more status flag
            CHartData::CDyn.DeviceStatusPrimary &= ~CHart::CDevStatus::MORE_STATUS;
            // The new status will be sent. Make a backup copy for verification.
            COSAL::CMem::Copy(CHartData::CDyn.PrimaryAddStatus, CHartData::CDyn.AddStatus, CHart::CSize::ADD_STAT_LEN);
        }
        else
        {
            // Delete secondary more status flag
            CHartData::CDyn.DeviceStatusSecondary &= ~CHart::CDevStatus::MORE_STATUS;
            // The new status will be sent. Make a backup copy for verification.
            COSAL::CMem::Copy(CHartData::CDyn.SecondarydAddStatus, CHartData::CDyn.AddStatus, CHart::CSize::ADD_STAT_LEN);
        }
    }

    return result;
}

void CUniCommandIntp::Command048::ToResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    COSAL::CMem::Copy(data, CHartData::CDyn.AddStatus, CHart::CSize::ADD_STAT_LEN);

    CService::ActiveService->Response.PayloadSize = 25;
    CService::SendHartIpMessage();
}

// Helpers
void CUniCommandIntp::SendUniqueIdResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    data[0] = 254;
    CCoding::PutWord(CHartData::CConst.ExpandedDeviceType, 1, data, EN_Endian::MSB_First);
    data[3] = CHartData::CStat.NumRequestPreambles;
    data[4] = CHartData::CConst.HartRevision;
    data[5] = CHartData::CConst.DeviceRevision;
    data[6] = CHartData::CConst.SoftwRevision;
    data[7] = CHartData::CConst.HwRevAndSigCode;
    data[8] = CHartData::CConst.Flags;
    data[9] = CHartData::CConst.DevUniqueID[0];
    data[10] = CHartData::CConst.DevUniqueID[1];
    data[11] = CHartData::CConst.DevUniqueID[2];
    data[12] = CHartData::CStat.NumResponsePreambles;
    data[13] = CHartData::CConst.LastDevVarCode;
    CCoding::PutWord(CHartData::CDyn.ConfigChangeCounter, 14, data, EN_Endian::MSB_First);
    data[16] = CHartData::CDyn.ExtendedDevStatus;
    CCoding::PutWord(CHartData::CConst.ExtendedManuCode, 17, data, EN_Endian::MSB_First);
    CCoding::PutWord(CHartData::CConst.ExtendedLabelCode, 19, data, EN_Endian::MSB_First);
    data[21] = CHartData::CConst.DeviceProfile;
    CService::ActiveService->Response.PayloadSize = 22;
    CService::SendHartIpMessage();
}

void CUniCommandIntp::SendTagDescrDateResponse()
{
    TY_Byte* data = CService::ActiveService->Response.PayloadData;

    CCoding::PutOctets(CHartData::CStat.ShortTag, 6, 0, data);
    CCoding::PutOctets(CHartData::CStat.Descriptor, 12, 6, data);
    data[18] = CHartData::CStat.Day;
    data[19] = CHartData::CStat.Month;
    data[20] = CHartData::CStat.Year;
    CService::ActiveService->Response.PayloadSize = 21;
    CService::SendHartIpMessage();
}

TY_Byte CUniCommandIntp::Command009::InsertDevVar(EN_DevVarCode code_, TY_Byte* buffer_)
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
    default:
        buffer_[1] = 0;
        buffer_[2] = 250;
        buffer_[3] = 0x07f;
        buffer_[4] = 0xa0;
        buffer_[5] = 0x0;
        buffer_[6] = 0x0;
        buffer_[7] = 0x30;
        break;
    }
    
    return 0;
}

