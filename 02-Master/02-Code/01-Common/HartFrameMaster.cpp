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

// Operation
void CFrame::Encode()
{

    TY_Word                pos = 0;
    TY_Word         actual_pos = 0;
    TY_Word           next_pos = 0;
    TY_Word      start_chk_pos = 0;
    TY_Word                  e;
    TY_Byte                chk = 0;
    TY_Word       payload_len = 0;

    if (NumPreambles > 23)
    {
        NumPreambles = 23;
    }

    /* Insert preambles */
    for (e = 0; e < NumPreambles; e++)
    {
        m_req_primitive[actual_pos++] = 0xff;
    }

    /* Insert Delimiter */
    start_chk_pos = actual_pos;
    if (GetBurstFrame() == EN_Bool::TRUE8)
    {
        m_req_primitive[actual_pos] = CHart::CDelimiter::BACK;
    }
    else
    {
        m_req_primitive[actual_pos] = CHart::CDelimiter::STX;
    }

    if (AddrMode != CHart::CAddrMode::POLLING)
    {
        /* Using unique address */
        // Insert addressing flag into delimiter
        m_req_primitive[actual_pos++] |= (TY_Byte)(CHart::CAddrMode::UNIQUE << 7);
        // Copy uinque ID to the address field
        GetUniqueID(&m_req_primitive[actual_pos]);
        next_pos = actual_pos + 5;
    }
    else
    {
        /* Using short address */
        // Leave delimiter as is
        actual_pos++;
        // Insert poll adress
        m_req_primitive[actual_pos] = GetShortAddr();
        next_pos = actual_pos + 1;
    }

    /* Modify first adress byte */
    // Mask out address flags
    m_req_primitive[actual_pos] &= 0x3f;
    if (GetLocalMaster() == EN_Bool::TRUE8)
    {
        // Insert primary master flag
        m_req_primitive[actual_pos] |= 0x80;
    }
    if (GetBurstFrame() == EN_Bool::TRUE8)
    {
        // Set burst mode flag if burst frame is simulated
        m_req_primitive[actual_pos] |= 0x40;
    }
    // Continue to next position
    actual_pos = next_pos;
    /* Note: Expansion bytes are skipped in this version.
             They may be added later if required. */
             /* Append command */
    m_req_primitive[actual_pos++] = Command;
    /* Append byte count and data */
    GetData(&m_req_primitive[actual_pos + 1], &payload_len);
    m_req_primitive[actual_pos] = (TY_Byte)payload_len;
    // Move to next position
    actual_pos = (TY_Byte)(actual_pos + payload_len + 1);
    /* Insert Checksum */
    for (e = start_chk_pos; e < actual_pos; e++)
    {
        chk ^= m_req_primitive[e];
    }

    m_req_primitive[actual_pos++] = chk;
    m_req_prim_len = actual_pos;
}

// Getting frame data
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
    COSAL::CMem::Copy(bytes_of_unique_id_, m_address, 5);
}
