/*
 *          File: HartFrame.h (CFrame)
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

#ifndef __hartframe_h__
#define __hartframe_h__

#include "OSAL.h"
#include "WbHartUser.h"

class CFrame
{
public:
    // Enum classes
    enum class EN_Status : TY_Byte
    {
        // Parsing status
        PARSE_PREAMBLE = 0,
        PARSING_PREAMBLES = 1,
        PARSE_DELIMITER = 2,
        PARSE_ADDRESS = 3,
        PARSING_UNIQUE_ADDR = 4,
        PARSE_EXP_BYTES = 5,
        PARSING_EXP_BYTES = 6,
        PARSE_COMMAND = 7,
        PARSE_DATA_LEN = 8,
        PARSE_RESP1 = 9,
        PARSE_RESP2 = 10,
        PARSE_PAYLOAD = 11,
        PARSING_PAYLOAD = 12,
        PARSE_CHECKSUM = 13,
        PARSE_GARBAGE = 14,
        PARSING_GARBAGE = 15,
        FRAME_COMPLETED = 16,
        INVALID_FRAME = 17,
        CHECKSUM_ERR = 18
    };

    enum class EN_Type : TY_Byte
    {
        NO_HART_IP = 0,
        JUNK = 1,
        REQUEST = 2,
        RESPONSE = 3,
        BURST = 4
    };

    // Static Data
    static TY_Byte       TxBufferBytes[MAX_TXRX_SIZE];
    static TY_Word       TxBufferLen;
    static EN_Master     Host;

    // Public Members
    EN_Type       Type;
    TY_Byte       CmdRespCode;      // Response 1
    TY_Byte       Command;          // 8 bit command
    TY_Byte       AddrMode;         // 0 = Polling, 1 = Unique
    TY_Byte       PayloadData[MAX_PAYLOAD_SIZE];
    TY_Word       PayloadSize;      // Used for encoding
    TY_Word       PayloadCount;     // Used by the command interpreter
    TY_Byte       NumPreambles;     // Used for the responses
    EN_Bool       NoPreamb;         // Send without preamble
    EN_Bool       DoNotCopyPayload;

    // Initialization/Construction
                          CFrame(EN_Bool local_master_);
                          CFrame();
                         ~CFrame();
               CFrame& operator=(const CFrame& other_);
    void                    Init();
    void                  Uninit();

    // Operation
    EN_Bool             IsActive();
    void               SetActive();
    void             ClearActive();
    EN_Bool             TryParse(TY_Word* bytes_parsed_, TY_Byte* new_data_, TY_Byte* new_err_, TY_Word new_data_len_, EN_Bool gap_time_out_);
    static void        ParseData(CFrame* frame_);
    EN_Bool            ParseByte(TY_Byte new_byte_, TY_Byte new_err_, EN_Bool gap_time_out_);
    void     SetRequestPrimitive(TY_Byte* data_, TY_Word len_);
    void                  Encode();

    // Setting Frame Data
    void                 SetData(TY_Byte* data_, TY_Word len_);
    void           SetUniqueAddr(TY_Byte* bytes_of_unique_id_);
    void             SetPollAddr(TY_Byte short_addr_);
    void             SetShortTag(TY_Byte* short_tag_);
    void              SetLongTag(TY_Byte* long_tag_);
    void           SetBurstFrame(EN_Bool burst_frame_);
    void          SetLocalMaster(EN_Bool primary_master_);

    // Getting Frame Data
    TY_Byte*           GetTxData(TY_Word* len_);
    EN_Status          GetStatus();
    void                 GetData(TY_Byte* data_, TY_Word* len_);
    void             GetUniqueID(TY_Byte* bytes_of_unique_id_);
    void        GetOtherUniqueID(TY_Byte* bytes_of_unique_id_);
    TY_Byte         GetShortAddr();
    EN_Bool      GetRemoteMaster();
    void             GetShortTag(TY_Byte* short_tag_);
    void              GetLongTag(TY_Byte* long_tag_);
    EN_Bool        GetBurstFrame();
    EN_Bool          GetAnyFrame();
    EN_Bool       GetLocalMaster();
    TY_Byte*       GetDataBuffer();
    TY_Byte          GetDataSize();
    EN_Bool              IsHart6();
    EN_Bool              IsHart7();
    EN_Bool    IsBurstModeDevice();
    TY_Byte          GetRspCode1();
    TY_Byte          GetRspCode2();
    TY_Byte          GetJabOctet();
    EN_Bool      GetJabOctetFlag();
    EN_Bool                IsSTX();
    EN_Bool                IsACK();
    EN_Bool               IsOACK();
    EN_Bool               IsBACK();
    EN_Bool              IsOBACK();

    // RxSM query functions
    TY_Word      GetRcvByteCount();
    TY_Word          GetTotalLen(TY_Word new_len_);

    // Handle time stamps
    TY_DWord        GetStartTime();
    TY_DWord          GetEndTime();
    void            SetStartTime(TY_DWord time_);
    void              SetEndTime(TY_DWord time_);

private:
    // Parsing status handling
    EN_Status        ParsePreamble(TY_Byte data_, TY_Byte error_);
    EN_Status     ParsingPreambles(TY_Byte data_, TY_Byte error_);
    EN_Status       ParseDelimiter(TY_Byte data_, TY_Byte error_);
    EN_Status         ParseAddress(TY_Byte data_, TY_Byte error_);
    EN_Status ParsingUniqueAddress(TY_Byte data_, TY_Byte error_);
    EN_Status        ParseExpBytes(TY_Byte data_, TY_Byte error_);
    EN_Status      ParsingExpBytes(TY_Byte data_, TY_Byte error_);
    EN_Status         ParseCommand(TY_Byte data_, TY_Byte error_);
    EN_Status         ParseDataLen(TY_Byte data_, TY_Byte error_);
    EN_Status       ParseResponse1(TY_Byte data_, TY_Byte error_);
    EN_Status       ParseResponse2(TY_Byte data_, TY_Byte error_);
    EN_Status         ParsePayload(TY_Byte data_, TY_Byte error_);
    EN_Status     ParsingPayload(TY_Byte data_, TY_Byte error_);
    EN_Status        ParseCheckSum(TY_Byte data_, TY_Byte error_);
    EN_Status         ParseGarbage(TY_Byte data_, TY_Byte error_);
    EN_Status     ParsingGarbage(TY_Byte data_, TY_Byte error_);
    
    // Query Functions
    static EN_Bool     IsDelimiter(TY_Byte byte_);
    static EN_Bool      IsPreamble(TY_Byte byte_);
    static EN_Bool IsPrimaryMaster(TY_Byte address_);
    static EN_Bool IsBurstModeFlag(TY_Byte address_);
    static EN_Bool   IsLongAddress(TY_Byte delimiter_);
    TY_Word             ParseDataLen(CFrame* frame_, TY_Byte byte_);
    
    // Special functions
    static TY_Byte GetDeviceStatus(EN_Master master_, CFrame::EN_Type frame_type_);

    // Helper functions
    void              ClearBuffers();

    // Control
    EN_Status Status;

    // Frame info
    TY_Byte   m_delimiter;
    TY_Byte   m_address[5];
    TY_Byte   m_addr_byte_count;
    TY_Byte   m_num_exp_bytes;
    TY_Byte   m_exp_bytes_count;
    TY_Byte   m_exp_bytes[3];
    EN_Bool   m_local_master;  // TRUE8 = primary
    EN_Bool   m_remote_master; // TRUE8 = primary
    TY_Byte   m_rsp1;
    TY_Byte   m_rsp2;
    TY_Byte   m_target_chk;
    TY_Byte   m_actual_chk;
    TY_DWord  m_start_time;
    TY_DWord  m_end_time;
    TY_Byte   m_short_tag[6];
    TY_Byte   m_long_tag[24];
    TY_Byte   m_req_primitive[MAX_TXRX_SIZE];
    TY_Word   m_req_prim_len;
    EN_Bool   m_burst_frame;
    EN_Bool   m_burst_mode;
    TY_Word   m_rcv_byte_count;
    TY_Byte   m_device_status;
    TY_Byte   m_jab_octet;
    EN_Bool   m_jab_octet_flag;
    EN_Bool   m_active;
};

#endif // __hartframe_h__