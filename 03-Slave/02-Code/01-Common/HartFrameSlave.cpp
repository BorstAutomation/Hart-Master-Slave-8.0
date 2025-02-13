/*
 *          File: HartFrame.cpp (CFrame)
 *                The hart frame is a construct used to collect all information
 *                which is needed to encode and decode data of so called
 *                service primitives like responses and requests, which are
 *                finally octet streams.
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

#include "HartFrame.h"
#include "HartConsts.h"
 // Only in case of a slave
 // for the function encode
#include "WbHartS_Structures.h"
#include "HartData.h"

// Static data
TY_Byte   CFrame::TxBufferBytes[MAX_TXRX_SIZE];
TY_Word   CFrame::TxBufferLen = 0;
EN_Master CFrame::Host;

// Operation
// Encoding a response
void CFrame::Encode()
{
    TY_Byte* data = TxBufferBytes;
    TY_Word                pos = 0;
    TY_Word         actual_pos = 0;
    TY_Word           next_pos = 0;
    TY_Word      start_chk_pos = 0;
    TY_Byte      num_preambles = 0;
    TY_Word                  e;
    TY_Byte                chk = 0;
    TY_Word        payload_len = 0;
    TY_Word           add_data_len = 0;

    if ((Type == CFrame::EN_Type::RESPONSE) ||
        (Type == CFrame::EN_Type::BURST))
    {
        num_preambles = CHartData::CStat.NumResponsePreambles;
    }
    else
    {
        num_preambles = CHartData::CStat.NumRequestPreambles;
    }

    if (num_preambles > 23)
    {
        num_preambles = 23;
    }

    // Insert preambles
    for (e = 0; e < num_preambles; e++)
    {
        data[actual_pos++] = 0xff;
    }

    // Insert Delimiter
    start_chk_pos = actual_pos;
    switch (Type)
    {
    case CFrame::EN_Type::BURST:
        data[actual_pos] = CHart::CDelimiter::BACK;
        break;
    case CFrame::EN_Type::REQUEST:
        data[actual_pos] = CHart::CDelimiter::STX;
        break;
    case CFrame::EN_Type::RESPONSE:
        data[actual_pos] = CHart::CDelimiter::ACK;
        break;
    }

    if (AddrMode == CHart::CAddrMode::UNIQUE)
    {
        // Using long address
        // Insert addressing flag into delimiter
        data[actual_pos++] |= (TY_Byte)(CHart::CAddrMode::UNIQUE << 7);
        // Copy uinque ID to the address field
        if ((Command == 11) || (Command == 21))
        {
            // These commands are using the global address
            data[actual_pos] = 0x00;
            data[actual_pos + 1] = 0x00;
            data[actual_pos + 2] = 0x00;
            data[actual_pos + 3] = 0x00;
            data[actual_pos + 4] = 0x00;
        }
        else
        {
            GetUniqueID(&data[actual_pos]);
        }

        next_pos = actual_pos + 5;
    }
    else
    {
        // Using short address
        // Leave delimiter as is
        actual_pos++;
        // Insert poll adress
        data[actual_pos] = GetShortAddr();
        next_pos = actual_pos + 1;
    }

    // Modify first adress byte
    // Mask out address flags
    data[actual_pos] &= 0x3f;
    if (Host == EN_Master::PRIMARY)
    {
        // Insert primary master flag
        data[actual_pos] |= 0x80;
    }

    if (Type == CFrame::EN_Type::BURST)
    {
        // Set burst mode flag if burst frame is coded
        data[actual_pos] |= 0x40;
    }
    // Continue to next position
    actual_pos = next_pos;
    // Note: Expansion bytes are skipped in this version.
    //       They may be added later if required.
    // Append command
    data[actual_pos++] = Command;
    // Insert response code and device status
    if ((Type == CFrame::EN_Type::BURST) ||
        (Type == CFrame::EN_Type::RESPONSE)
        )
    {
        data[actual_pos + 1] = CmdRespCode;
        data[actual_pos + 2] = GetDeviceStatus(Host, Type);
        add_data_len = 2;
    }
    // Append byte count and data
    GetData(&data[actual_pos + add_data_len + 1], &payload_len);
    data[actual_pos] = (TY_Byte)(payload_len + add_data_len);
    // Move to next position
    actual_pos = (TY_Byte)(actual_pos + payload_len + add_data_len + 1);
    // Insert Checksum
    for (e = start_chk_pos; e < actual_pos; e++)
    {
        chk ^= data[e];
    }

    data[actual_pos++] = chk;
    TxBufferLen = actual_pos;
}

// Getting Frame Data
void CFrame::GetData(TY_Byte* data_, TY_Word* len_)
{
    if ((PayloadData != 0) && (PayloadSize > 0))
    {
        COSAL::CMem::Copy(data_, PayloadData, PayloadSize);
        *len_ = PayloadSize;
    }
    else
    {
        *len_ = 0;
    }
}

void CFrame::GetUniqueID(TY_Byte* bytes_of_unique_id_)
{
    COSAL::CMem::Copy(bytes_of_unique_id_, CHartData::CStat.LongAddress, 5);
}

void CFrame::GetOtherUniqueID(TY_Byte* bytes_of_unique_id_)
{
    COSAL::CMem::Copy(bytes_of_unique_id_, m_address, 5);
}

EN_Bool CFrame::GetRemoteMaster()
{
    return m_remote_master;
}

// Special function
TY_Byte CFrame::GetDeviceStatus(EN_Master master_, CFrame::EN_Type frame_type_)
{
    TY_Byte device_status;

    if (master_ == EN_Master::PRIMARY)
    {
        device_status = CHartData::CDyn.DeviceStatusPrimary;
        if (frame_type_ == CFrame::EN_Type::RESPONSE)
        {
            // Delete master specific cold start flag
            CHartData::CDyn.DeviceStatusPrimary = device_status & ~(CHart::CDevStatus::COLD_START);
        }
    }
    else
    {
        device_status = CHartData::CDyn.DeviceStatusSecondary;
        if (frame_type_ == CFrame::EN_Type::RESPONSE)
        {
            // Delete master specific cold start flag
            CHartData::CDyn.DeviceStatusSecondary = device_status & ~(CHart::CDevStatus::COLD_START);
        }
    }

    return device_status;
}
