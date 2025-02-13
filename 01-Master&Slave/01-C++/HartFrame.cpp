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

// Initialization/Construction
CFrame::CFrame()
{
    // Public members
    Type = EN_Type::NO_HART_IP;
    CmdRespCode = 0;
    Command = 0xff;
    AddrMode = 0;
    COSAL::CMem::Set(PayloadData, 0, MAX_PAYLOAD_SIZE);
    PayloadSize = 0;
    PayloadCount = 0;
    NumPreambles = 0;
    NoPreamb = EN_Bool::FALSE8;
    DoNotCopyPayload = EN_Bool::FALSE8;

    Status = EN_Status::PARSE_PREAMBLE;
    m_delimiter = 0;
    COSAL::CMem::Set(m_address, 0, 5);
    m_addr_byte_count = 0;
    m_num_exp_bytes = 0;
    m_exp_bytes_count = 0;
    COSAL::CMem::Set(m_exp_bytes, 0, 3);
    m_local_master = EN_Bool::FALSE8;
    m_remote_master = EN_Bool::FALSE8;
    m_rsp1 = 0;
    m_rsp2 = 0;
    m_target_chk = 0;
    m_actual_chk = 0;
    m_start_time = 0;
    m_end_time = 0;
    COSAL::CMem::Set(m_short_tag, 0, 6);
    COSAL::CMem::Set(m_long_tag, 0, 24);
    COSAL::CMem::Set(m_req_primitive, 0, MAX_TXRX_SIZE);
    m_req_prim_len = 0;
    m_burst_frame = EN_Bool::FALSE8;
    m_burst_mode = EN_Bool::FALSE8;
    m_rcv_byte_count = 0;
    m_device_status = 0;
    m_jab_octet = 0;
    m_rcv_byte_count = 0;
    m_jab_octet_flag = EN_Bool::FALSE8;
    m_active = EN_Bool::FALSE8;
}

CFrame::CFrame(EN_Bool local_master_)
{
    CFrame();
    m_local_master = local_master_;
}

CFrame::~CFrame()
{
}

CFrame& CFrame::operator=(const CFrame& other_)
{
    if (this != &other_)
    {
        // Public members
        Type = other_.Type;
        CmdRespCode = other_.CmdRespCode;
        Command = other_.Command;
        AddrMode = other_.AddrMode;
        if (DoNotCopyPayload == EN_Bool::FALSE8)
        {
            PayloadSize = other_.PayloadSize;
            COSAL::CMem::Copy(PayloadData, other_.PayloadData, PayloadSize);
        }
        else
        {
            DoNotCopyPayload = EN_Bool::FALSE8;
        }
        PayloadCount = other_.PayloadCount;
        NumPreambles = other_.NumPreambles;
        NoPreamb = other_.NoPreamb;

        // Private members
        Status = other_.Status;
        m_delimiter = other_.m_delimiter;
        COSAL::CMem::Copy(m_address, other_.m_address, 5);
        m_addr_byte_count = other_.m_addr_byte_count;
        m_num_exp_bytes = other_.m_num_exp_bytes;
        m_exp_bytes_count = other_.m_exp_bytes_count;
        COSAL::CMem::Copy(m_exp_bytes, other_.m_exp_bytes, 3);
        m_local_master = other_.m_local_master;
        m_remote_master = other_.m_remote_master;
        m_rsp1 = other_.m_rsp1;
        m_rsp2 = other_.m_rsp2;
        m_target_chk = other_.m_target_chk;
        m_actual_chk = other_.m_actual_chk;
        m_start_time = other_.m_start_time;
        m_end_time = other_.m_end_time;
        COSAL::CMem::Copy(m_short_tag, other_.m_short_tag, 6);
        COSAL::CMem::Copy(m_long_tag, other_.m_long_tag, 24);
        m_req_prim_len = other_.m_req_prim_len;
        COSAL::CMem::Copy(m_req_primitive, other_.m_req_primitive, m_req_prim_len);
        m_burst_frame = other_.m_burst_frame;
        m_burst_mode = other_.m_burst_mode;
        m_rcv_byte_count = other_.m_rcv_byte_count;
        m_device_status = other_.m_device_status;
        m_jab_octet = other_.m_jab_octet;
        m_jab_octet_flag = other_.m_jab_octet_flag;
        m_active = other_.m_active;
    }

    return *this;
}

void CFrame::Init()
{
    ClearBuffers();
    NumPreambles = 0;
    Status = EN_Status::PARSE_PREAMBLE;
    m_start_time = 0;
    m_burst_frame = EN_Bool::FALSE8;
    m_rcv_byte_count = 0;
    PayloadCount = 0;
    m_exp_bytes_count = 0;
    PayloadSize = 0;
    m_active = EN_Bool::TRUE8;
    NoPreamb = EN_Bool::FALSE8;
}

void CFrame::Uninit()
{
    Init();
    m_active = EN_Bool::FALSE8;
}

// Operation
EN_Bool CFrame::IsActive()
{
    return m_active;
}

void CFrame::SetActive()
{
    m_active = EN_Bool::TRUE8;
}

void CFrame::ClearActive()
{
    m_active = EN_Bool::FALSE8;
}

EN_Bool CFrame::TryParse(TY_Word* bytes_parsed_, TY_Byte* new_data_, TY_Byte* new_err_, TY_Word new_data_len_, EN_Bool gap_time_out_)
{
    // Online parsing
    TY_Word bytes_parsed = 0;
    //static TY_Byte  test[256];

    // Just for testing copy data to local buffer
    //COSAL::CMem::Set(test, 0 , 256);
    //COSAL::CMem::Copy(test, new_data_, new_data_len_ );

    while ((bytes_parsed < new_data_len_) &&
        (Status != EN_Status::FRAME_COMPLETED) &&
        (Status != EN_Status::INVALID_FRAME) &&
        (Status != EN_Status::CHECKSUM_ERR)
        )
    {
        switch (Status)
        {
            // State Machine Cases
        case EN_Status::PARSE_PREAMBLE:
            if (NoPreamb == EN_Bool::TRUE8)
            {
                // Start with the delimiter;
                Status = ParseDelimiter(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            }
            else
            {
                Status = ParsePreamble(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            }

            break;
        case EN_Status::PARSING_PREAMBLES:
            Status = ParsingPreambles(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_DELIMITER:
            Status = ParseDelimiter(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_ADDRESS:
            Status = ParseAddress(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSING_UNIQUE_ADDR:
            Status = ParsingUniqueAddress(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_EXP_BYTES:
            Status = ParseExpBytes(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSING_EXP_BYTES:
            Status = ParsingExpBytes(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_COMMAND:
            Status = ParseCommand(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_DATA_LEN:
            Status = ParseDataLen(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_RESP1:
            Status = ParseResponse1(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_RESP2:
            Status = ParseResponse2(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_PAYLOAD:
            Status = ParsePayload(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSING_PAYLOAD:
            Status = ParsingPayload(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_CHECKSUM:
            Status = ParseCheckSum(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSE_GARBAGE:
            Status = ParseGarbage(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        case EN_Status::PARSING_GARBAGE:
            Status = ParsingGarbage(new_data_[bytes_parsed], new_err_[bytes_parsed]);
            break;
        default:
            break;
        }
        bytes_parsed++;
    }
    if (gap_time_out_ == EN_Bool::TRUE8)
    {
        if (Status != EN_Status::PARSE_PREAMBLE)
        {
            // handle gap time out
            bytes_parsed = new_data_len_;
            Status = EN_Status::INVALID_FRAME;
        }
        else
        {
            if (m_rcv_byte_count > 0)
            {
                bytes_parsed = new_data_len_;
                Status = EN_Status::INVALID_FRAME;
            }
        }
    }

    *bytes_parsed_ = bytes_parsed;
    m_rcv_byte_count += bytes_parsed;

    if ((Status == EN_Status::CHECKSUM_ERR) ||
        (Status == EN_Status::INVALID_FRAME) ||
        (Status == EN_Status::FRAME_COMPLETED)
        )
    {
        switch (Status)
        {
        case EN_Status::CHECKSUM_ERR:
        case EN_Status::INVALID_FRAME:
            Type = EN_Type::JUNK;
            break;
        case EN_Status::FRAME_COMPLETED:
            if (m_delimiter == CHart::CDelimiter::STX)
            {
                Type = EN_Type::REQUEST;
            }
            else if (m_delimiter == CHart::CDelimiter::ACK)
            {
                Type = EN_Type::RESPONSE;
            }
            else if (m_delimiter == CHart::CDelimiter::BACK)
            {
                Type = EN_Type::BURST;
            }
            else
            {
                Type = EN_Type::JUNK;
            }
            break;
        default:
            Type = EN_Type::NO_HART_IP;
            break;
        }

        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}

void CFrame::ParseData(CFrame* frame_)
{
    // Offline Parsing
    // Not yet used
}

EN_Bool CFrame::ParseByte(TY_Byte new_byte_, TY_Byte new_err_, EN_Bool gap_time_out_)
{
    if (gap_time_out_ == EN_Bool::FALSE8)
    {
        switch (Status)
        {
            // State Machine Cases
        case EN_Status::PARSE_PREAMBLE:
            if (NoPreamb == EN_Bool::TRUE8)
            {
                // Start with the delimiter;
                Status = ParseDelimiter(new_byte_, new_err_);
            }
            else
            {
                Status = ParsePreamble(new_byte_, new_err_);
            }

            break;
        case EN_Status::PARSING_PREAMBLES:
            Status = ParsingPreambles(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_DELIMITER:
            Status = ParseDelimiter(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_ADDRESS:
            Status = ParseAddress(new_byte_, new_err_);
            break;
        case EN_Status::PARSING_UNIQUE_ADDR:
            Status = ParsingUniqueAddress(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_EXP_BYTES:
            Status = ParseExpBytes(new_byte_, new_err_);
            break;
        case EN_Status::PARSING_EXP_BYTES:
            Status = ParsingExpBytes(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_COMMAND:
            Status = ParseCommand(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_DATA_LEN:
            Status = ParseDataLen(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_RESP1:
            Status = ParseResponse1(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_RESP2:
            Status = ParseResponse2(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_PAYLOAD:
            Status = ParsePayload(new_byte_, new_err_);
            break;
        case EN_Status::PARSING_PAYLOAD:
            Status = ParsingPayload(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_CHECKSUM:
            Status = ParseCheckSum(new_byte_, new_err_);
            break;
        case EN_Status::PARSE_GARBAGE:
            Status = ParseGarbage(new_byte_, new_err_);
            break;
        case EN_Status::PARSING_GARBAGE:
            Status = ParsingGarbage(new_byte_, new_err_);
            break;
        case EN_Status::CHECKSUM_ERR:
            // To do?
            break;
        case EN_Status::INVALID_FRAME:
            // To do?
            break;
        case EN_Status::FRAME_COMPLETED:
            // To do?
            break;
        default:
            break;
        }

        if (new_err_ != 0)
        {
            Status = EN_Status::PARSE_GARBAGE;
        }

        m_rcv_byte_count++;
    }
    else
    {
        if (Status != EN_Status::PARSE_PREAMBLE)
        {
            // handle gap time out
            Status = EN_Status::INVALID_FRAME;
        }
        else
        {
            if (m_rcv_byte_count > 0)
            {
                Status = EN_Status::INVALID_FRAME;
            }
        }
    }

    if ((Status == EN_Status::CHECKSUM_ERR) ||
        (Status == EN_Status::INVALID_FRAME) ||
        (Status == EN_Status::FRAME_COMPLETED)
        )
    {
        switch (Status)
        {
        case EN_Status::CHECKSUM_ERR:
        case EN_Status::INVALID_FRAME:
            Type = EN_Type::JUNK;
            break;
        case EN_Status::FRAME_COMPLETED:
            if (m_delimiter == CHart::CDelimiter::STX)
            {
                Type = EN_Type::REQUEST;
            }
            else if (m_delimiter == CHart::CDelimiter::ACK)
            {
                Type = EN_Type::RESPONSE;
            }
            else if (m_delimiter == CHart::CDelimiter::BACK)
            {
                Type = EN_Type::BURST;
            }
            else
            {
                Type = EN_Type::JUNK;
            }
            break;
        default:
            Type = EN_Type::NO_HART_IP;
            break;
        }

        return EN_Bool::TRUE8;
    }

    return EN_Bool::FALSE8;
}

void CFrame::SetRequestPrimitive(TY_Byte* data_, TY_Word len_)
{
    if (len_ > 0)
    {
        COSAL::CMem::Copy(m_req_primitive, data_, len_);
        m_req_prim_len = len_;
    }
}

// Setting Frame Data
void CFrame::SetData(TY_Byte* data_, TY_Word len_)
{
    if (len_ > 0)
    {
        PayloadSize = len_;
        COSAL::CMem::Copy(PayloadData, data_, len_);
    }
}

void CFrame::SetUniqueAddr(TY_Byte* bytes_of_unique_id_)
{
    COSAL::CMem::Copy(m_address, bytes_of_unique_id_, 5);
}

void CFrame::SetPollAddr(TY_Byte poll_addr_)
{
    m_address[0] = poll_addr_;
}

void CFrame::SetShortTag(TY_Byte* short_tag_)
{
    COSAL::CMem::Copy(m_short_tag, short_tag_, 6);
}

void CFrame::SetLongTag(TY_Byte* long_tag_)
{
    COSAL::CMem::Copy(m_long_tag, long_tag_, 24);
}

void CFrame::SetBurstFrame(EN_Bool burst_frame_)
{
    m_burst_frame = burst_frame_;
}

void CFrame::SetLocalMaster(EN_Bool primary_master_)
{
    m_local_master = primary_master_;
}

// Getting Frame data
TY_Byte* CFrame::GetTxData(TY_Word* len_)
{
    if (m_req_primitive != NULL)
    {
        if (m_req_prim_len > 0)
        {
            *len_ = m_req_prim_len;
            return m_req_primitive;
        }
    }
    return NULL;
}

TY_Byte CFrame::GetShortAddr()
{
    return m_address[0];
}

void CFrame::GetShortTag(TY_Byte* short_tag_)
{
    COSAL::CMem::Copy(short_tag_, m_short_tag, 6);
}

void CFrame::GetLongTag(TY_Byte* long_tag_)
{
    COSAL::CMem::Copy(long_tag_, m_long_tag, 24);
}

EN_Bool CFrame::GetBurstFrame()
{
    return m_burst_frame;
}

EN_Bool CFrame::GetLocalMaster()
{
    return m_local_master;
}

CFrame::EN_Status CFrame::GetStatus()
{
    return Status;
}

TY_Byte* CFrame::GetDataBuffer()
{
    return PayloadData;
}

TY_Byte CFrame::GetDataSize()
{
    return (TY_Byte)PayloadSize;
}

EN_Bool CFrame::IsBurstModeDevice()
{
    return m_burst_mode;
}

TY_Byte CFrame::GetRspCode1()
{
    return m_rsp1;
}

TY_Byte CFrame::GetRspCode2()
{
    return m_rsp2;
}

TY_Byte CFrame::GetJabOctet()
{
    return m_jab_octet;
}

EN_Bool CFrame::GetJabOctetFlag()
{
    return m_jab_octet_flag;
}

EN_Bool CFrame::IsSTX()
{
    return EN_Bool::FALSE8;
}

EN_Bool CFrame::IsACK()
{
    if (Type == EN_Type::REQUEST)
    {
        if (m_local_master == m_remote_master)
        {
            return EN_Bool::TRUE8;
        }
    }
    return EN_Bool::FALSE8;
}

EN_Bool CFrame::IsOACK()
{
    if (Type == EN_Type::RESPONSE)
    {
        if (m_local_master != m_remote_master)
        {
            return EN_Bool::TRUE8;
        }
    }
    return EN_Bool::FALSE8;
}

EN_Bool CFrame::IsBACK()
{
    if (Type == EN_Type::BURST)
    {
        if (m_local_master == m_remote_master)
        {
            return EN_Bool::TRUE8;
        }
    }
    return EN_Bool::FALSE8;
}

EN_Bool CFrame::IsOBACK()
{
    if (Type == EN_Type::BURST)
    {
        if (m_local_master != m_remote_master)
        {
            return EN_Bool::TRUE8;
        }
    }
    return EN_Bool::FALSE8;
}

// RxSM query functions
TY_Word CFrame::GetRcvByteCount()
{
    return m_rcv_byte_count;
}

TY_Word CFrame::GetTotalLen(TY_Word u16_NewLen)
{
    return (TY_Word)(u16_NewLen + m_rcv_byte_count);
}

// Handle time stamps
TY_DWord CFrame::GetStartTime()
{
    return m_start_time;
}

TY_DWord CFrame::GetEndTime()
{
    return m_end_time;
}

void CFrame::SetStartTime(TY_DWord time_)
{
    m_start_time = time_;
}

void CFrame::SetEndTime(TY_DWord time_)
{
    m_end_time = time_;
}

// *******************
// * Private Section *
// *******************
// Parsing items
CFrame::EN_Status CFrame::ParsePreamble(TY_Byte data_, TY_Byte error_)
{
    if (IsPreamble(data_) == EN_Bool::TRUE8)
    {
        NumPreambles = 1;
        return EN_Status::PARSING_PREAMBLES;
    }
    return EN_Status::PARSE_PREAMBLE;
}

CFrame::EN_Status CFrame::ParsingPreambles(TY_Byte data_, TY_Byte error_)
{
    if (IsPreamble(data_) == EN_Bool::TRUE8)
    {
        NumPreambles = 2;
        return EN_Status::PARSE_DELIMITER;
    }
    return EN_Status::PARSE_PREAMBLE;
}

CFrame::EN_Status CFrame::ParseDelimiter(TY_Byte data_, TY_Byte error_)
{
    if (IsPreamble(data_) == EN_Bool::TRUE8)
    {
        NumPreambles++;
        if (NumPreambles > CHart::CLimit::MAX_NUM_PREAMBLES)
        {
            return EN_Status::PARSE_GARBAGE;
        }
        else
        {
            return EN_Status::PARSE_DELIMITER;
        }
    }
    else if (IsDelimiter(data_) == EN_Bool::TRUE8)
    {
        m_num_exp_bytes = (TY_Byte)((data_ & CHart::CMask::NUM_EXP_BYTES) >> 5);
        m_delimiter = data_ & CHart::CMask::DELIMITER;
        m_target_chk = data_; // Start checksum
        if (IsLongAddress(data_) == EN_Bool::TRUE8)
        {
            AddrMode = CHart::CAddrMode::UNIQUE;
        }
        else
        {
            AddrMode = CHart::CAddrMode::POLLING;
        }
        return EN_Status::PARSE_ADDRESS;
    }
    return EN_Status::PARSE_PREAMBLE;
}

CFrame::EN_Status CFrame::ParseAddress(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    m_address[0] = data_ & CHart::CMask::HIGH_ADDR;
    if (IsPrimaryMaster(data_) == EN_Bool::TRUE8)
    {
        m_remote_master = EN_Bool::TRUE8;
    }
    else
    {
        m_remote_master = EN_Bool::FALSE8;
    }
    if (IsBurstModeFlag(data_) == EN_Bool::TRUE8)
    {
        m_burst_mode = EN_Bool::TRUE8;
    }
    else
    {
        m_burst_mode = EN_Bool::FALSE8;
    }
    m_addr_byte_count = 1;
    if (AddrMode == CHart::CAddrMode::UNIQUE)
    {
        return EN_Status::PARSING_UNIQUE_ADDR;
    }
    if (m_num_exp_bytes > 0)
    {
        return EN_Status::PARSE_EXP_BYTES;
    }
    return EN_Status::PARSE_COMMAND;
}

CFrame::EN_Status CFrame::ParsingUniqueAddress(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    m_address[m_addr_byte_count] = data_;
    m_addr_byte_count++;
    if (m_addr_byte_count == CHart::CSize::NUM_ADDR_BYTES)
    {
        if (m_num_exp_bytes > 0)
        {
            return EN_Status::PARSE_EXP_BYTES;
        }
        return EN_Status::PARSE_COMMAND;
    }
    return EN_Status::PARSING_UNIQUE_ADDR;
}

CFrame::EN_Status CFrame::ParseExpBytes(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    m_exp_bytes[0] = data_;
    m_exp_bytes_count = 1;
    if (m_num_exp_bytes == 1)
    {
        return EN_Status::PARSE_COMMAND;
    }
    return EN_Status::PARSING_EXP_BYTES;
}

CFrame::EN_Status CFrame::ParsingExpBytes(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    m_exp_bytes[m_exp_bytes_count] = data_;
    m_exp_bytes_count++;
    if (m_exp_bytes_count == m_num_exp_bytes)
    {
        return EN_Status::PARSE_COMMAND;
    }
    return EN_Status::PARSING_EXP_BYTES;
}

CFrame::EN_Status CFrame::ParseCommand(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    Command = data_;
    return EN_Status::PARSE_DATA_LEN;
}

CFrame::EN_Status CFrame::ParseDataLen(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    if (m_delimiter == CHart::CDelimiter::STX)
    {
        PayloadSize = data_;
        if (PayloadSize > 0)
        {
            return EN_Status::PARSE_PAYLOAD;
        }
        else
        {
            return EN_Status::PARSE_CHECKSUM;
        }
    }
    else
    {
        if (data_ >= 2)
        {
            PayloadSize = data_ - 2;
            return EN_Status::PARSE_RESP1;
        }
    }
    return EN_Status::INVALID_FRAME;
}

CFrame::EN_Status CFrame::ParseResponse1(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    m_rsp1 = data_;
    return EN_Status::PARSE_RESP2;
}

CFrame::EN_Status CFrame::ParseResponse2(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    m_rsp2 = data_;
    if (PayloadSize > 0)
    {
        return EN_Status::PARSE_PAYLOAD;
    }
    else
    {
        return EN_Status::PARSE_CHECKSUM;
    }
}

CFrame::EN_Status CFrame::ParsePayload(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    PayloadData[0] = data_;
    PayloadCount = 1;
    if (PayloadSize == 1)
    {
        return EN_Status::PARSE_CHECKSUM;
    }
    return EN_Status::PARSING_PAYLOAD;
}

CFrame::EN_Status CFrame::ParsingPayload(TY_Byte data_, TY_Byte error_)
{
    m_target_chk ^= data_;
    PayloadData[PayloadCount] = data_;
    PayloadCount++;
    if (PayloadCount == PayloadSize)
    {
        return EN_Status::PARSE_CHECKSUM;
    }
    return EN_Status::PARSING_PAYLOAD;
}

CFrame::EN_Status CFrame::ParseCheckSum(TY_Byte data_, TY_Byte error_)
{
    m_actual_chk = data_;
    if (m_actual_chk == m_target_chk)
    {
        return EN_Status::FRAME_COMPLETED;
    }
    return EN_Status::CHECKSUM_ERR;
}

CFrame::EN_Status CFrame::ParseGarbage(TY_Byte data_, TY_Byte error_)
{
    return EN_Status::PARSING_GARBAGE;
}

CFrame::EN_Status CFrame::ParsingGarbage(TY_Byte data_, TY_Byte error_)
{
    return EN_Status::PARSING_GARBAGE;
}

// Query Functions
EN_Bool CFrame::IsDelimiter(TY_Byte byte_)
{
    TY_Byte u8_Del = byte_ & 0x07;

    switch (u8_Del)
    {
    case CHart::CDelimiter::ACK:
        return EN_Bool::TRUE8;
    case CHart::CDelimiter::STX:
        return EN_Bool::TRUE8;
    case CHart::CDelimiter::BACK:
        return EN_Bool::TRUE8;
    }
    return EN_Bool::FALSE8;
}

EN_Bool CFrame::IsPreamble(TY_Byte byte_)
{
    if (byte_ == 0xff)
    {
        return EN_Bool::TRUE8;
    }
    return EN_Bool::FALSE8;
}

EN_Bool CFrame::IsPrimaryMaster(TY_Byte address_)
{
    if ((address_ & 0x80) != NULL)
    {
        return EN_Bool::TRUE8;
    }
    return EN_Bool::FALSE8;
}

EN_Bool CFrame::IsBurstModeFlag(TY_Byte address_)
{
    if ((address_ & 0x40) != NULL)
    {
        return EN_Bool::TRUE8;
    }
    return EN_Bool::FALSE8;
}

EN_Bool CFrame::IsLongAddress(TY_Byte delimiter_)
{
    if ((delimiter_ & CHart::CMask::ADDR_TYPE) != NULL)
    {
        return EN_Bool::TRUE8;
    }
    return EN_Bool::FALSE8;
}

// Special functions

// Helper functions
void CFrame::ClearBuffers()
{
    PayloadSize = 0;
}

