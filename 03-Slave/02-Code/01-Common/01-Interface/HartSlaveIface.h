/*
 *          File: HartSLaveIface.h (CHartSlave)
 *                This is where the actual interface of the slave implementation
 *                is located, which would also have to be integrated into an
 *                embedded system. The version with the DLL is only intended for
 *                testing under Windows.
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

 // Once
#ifndef __hart_slave_iface_h__
#define __hart_slave_iface_h__

class CHartSlave
{
public:
    // Channel handling
    static EN_Bool        OpenChannel(TY_Word port_number_, EN_CommType type_);
    static void          CloseChannel();

    // Configuration
    static void          GetConstDataHart(TY_ConstDataHart* const_data_);
    static void          SetConstDataHart(TY_ConstDataHart* const_data_);
    static void            GetDynDataHart(TY_DynDataHart* dyn_data_);
    static void            SetDynDataHart(TY_DynDataHart* dyn_data_);
    static void           GetStatDataHart(TY_StatDataHart* stat_data_);
    static void           SetStatDataHart(TY_StatDataHart* stat_data_);

    // Command interpreter
    static EN_Bool        WasCommandReceived();
    static TY_Word ExecuteCommandInterpreter();

    // Information
    static TY_Word GetHartIpStatus();

    // Encoding
    static void PutInt8(TY_Byte data_, TY_Byte offset_,
        TY_Byte* data_ref_);
    static void PutInt16(TY_Word data_, TY_Byte offset_,
        TY_Byte* data_ref_, EN_Endian endian_);
    static void PutInt24(TY_DWord data_, TY_Byte offset_,
        TY_Byte* data_ref_, EN_Endian endian_);
    static void PutInt32(TY_DWord data_, TY_Byte offset_,
        TY_Byte* data_ref_, EN_Endian endian_);
    static void PutInt64(TY_UInt64 data_, TY_Byte offset_,
        TY_Byte* data_ref_, EN_Endian endian_);
    static void PutFloat(TY_Float data_, TY_Byte offset_,
        TY_Byte* data_ref_, EN_Endian endian_);
    static void PutDFloat(TY_DFloat data_, TY_Byte offset_,
        TY_Byte* data_ref_, EN_Endian endian_);
    static void PutPackedASCII(
        TY_Byte* asc_string_ref_, TY_Byte asc_string_len_,
        TY_Byte offset_, TY_Byte* data_ref_);
    static void PutOctets(TY_Byte* stream_ref_, TY_Byte stream_len_,
        TY_Byte offset_, TY_Byte* data_ref_);
    static void PutString(TY_Byte* string_ref_, TY_Byte string_max_len_,
        TY_Byte offset_, TY_Byte* data_ref_);

    // Decoding
    static TY_Byte PickInt8(TY_Byte offset_, TY_Byte* data_ref_);
    static TY_Word PickInt16(TY_Byte offset_, TY_Byte* data_ref_, 
        EN_Endian endian_);
    static TY_DWord PickInt24(TY_Byte offset_, TY_Byte* data_ref_,
        EN_Endian endian_);
    static TY_DWord PickInt32(TY_Byte offset_, TY_Byte* data_ref_,
        EN_Endian endian_);
    static TY_UInt64 PickInt64(TY_Byte offset_, TY_Byte* data_ref_,
        EN_Endian endian_);
    static TY_Float PickFloat(TY_Byte offset_, TY_Byte* data_ref_,
        EN_Endian endian_);
    static TY_DFloat PickDFloat(TY_Byte offset_, TY_Byte* data_ref_,
        EN_Endian endian_);
    static void PickPackedASCII(TY_Byte* string_ref_, TY_Byte string_len_,
        TY_Byte offset_, TY_Byte* data_ref_);
    static void PickOctets(TY_Byte* stream_ref_, TY_Byte stream_len_,
        TY_Byte offset_, TY_Byte* data_ref_);
    static void PickString(TY_Byte* string_ref_, TY_Byte string_max_len_,
        TY_Byte offset_, TY_Byte* data_ref_);

    // Internal handling
    static void FastCyclicHandler(TY_Word time_ms_);

};

#endif // __hart_slave_iface_h__