/*
 *          File: HartCoding.h (CCoding)
 *                This module combines functions that carry out the encoding
 *                and decoding of communication primitives and data objects.
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

#ifndef __ccoding_h__
#define __ccoding_h__

class CFrame;
class CCoding
{
public:
    // Encoding
    static void            PutWord(TY_Word data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static void            PutInt24(TY_DWord data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static void            PutDWord(TY_DWord data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static void            PutInt64(TY_UInt64 data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static void            PutFloat(TY_Float data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static void           PutDFloat(TY_DFloat data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static void      PutPackedASCII(TY_Byte* string_ref_, TY_Byte string_max_len_, TY_Byte offset_, TY_Byte* data_ref_);
    static void           PutOctets(TY_Byte* stream_ref_, TY_Byte stream_len_, TY_Byte offset_, TY_Byte* data_ref_);
    static void           PutString(TY_Byte* string_ref_, TY_Byte string_max_len_, TY_Byte offset_, TY_Byte* data_ref_);

    // Decoding
    static TY_Word         PickWord(TY_Byte offset_, TY_Byte* data_, EN_Endian endian_);
    static TY_DWord        PickInt24(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static TY_DWord        PickDWord(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static TY_UInt64       PickInt64(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static TY_Float        PickFloat(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static TY_DFloat      PickDouble(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_);
    static void      PickPackedASCII(TY_Byte* string_ref_, TY_Byte string_max_len_, TY_Byte offset_, TY_Byte* data_ref_);
    static void           PickOctets(TY_Byte* stream_ref_, TY_Byte stream_len_, TY_Byte offset_, TY_Byte* data_ref_);
    static void           PickString(TY_Byte* string_ref_, TY_Byte string_max_len_, TY_Byte offset_, TY_Byte* data_ref_);
};
#endif // #ifndef __ccoding_h__
