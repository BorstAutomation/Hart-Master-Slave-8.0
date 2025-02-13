/*
 *          File: HartCoding.cpp (CCoding)
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

#include "OSAL.h"
#include "WbHartUser.h"
#include "HartCoding.h"
#include "HartConsts.h"
#include "math.h"
#include <stdio.h>
#include <string.h>

// Encoding
void CCoding::PutWord(TY_Word data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    if (endian_ == EN_Endian::LSB_First)
    {
        data_ref_[offset_] = (TY_Byte)(data_ & 0xff);
        data_ref_[offset_ + 1] = (TY_Byte)((data_ >> 8) & 0xff);
    }
    else
    {
        data_ref_[offset_ + 1] = (TY_Byte)(data_ & 0xff);
        data_ref_[offset_] = (TY_Byte)((data_ >> 8) & 0xff);
    }
}

void CCoding::PutInt24(TY_DWord data_, TY_Byte offset_,
    TY_Byte* data_ref_, EN_Endian endian_)
{
    if (endian_ == EN_Endian::LSB_First)
    {
        data_ref_[offset_] = (TY_Byte)(data_ & 0xff);
        data_ref_[offset_ + 1] = (TY_Byte)((data_ >> 8) & 0xff);
        data_ref_[offset_ + 2] = (TY_Byte)((data_ >> 16) & 0xff);
    }
    else
    {
        data_ref_[offset_ + 2] = (TY_Byte)(data_ & 0xff);
        data_ref_[offset_ + 1] = (TY_Byte)((data_ >> 8) & 0xff);
        data_ref_[offset_] = (TY_Byte)((data_ >> 16) & 0xff);
    }
}

void CCoding::PutDWord(TY_DWord data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    if (endian_ == EN_Endian::LSB_First)
    {
        data_ref_[offset_] = (TY_Byte)(data_ & 0xff);
        data_ref_[offset_ + 1] = (TY_Byte)((data_ >> 8) & 0xff);
        data_ref_[offset_ + 2] = (TY_Byte)((data_ >> 16) & 0xff);
        data_ref_[offset_ + 3] = (TY_Byte)((data_ >> 24) & 0xff);
    }
    else
    {
        data_ref_[offset_ + 3] = (TY_Byte)(data_ & 0xff);
        data_ref_[offset_ + 2] = (TY_Byte)((data_ >> 8) & 0xff);
        data_ref_[offset_ + 1] = (TY_Byte)((data_ >> 16) & 0xff);
        data_ref_[offset_] = (TY_Byte)((data_ >> 24) & 0xff);
    }
}

void CCoding::PutInt64(TY_UInt64 data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    if (endian_ == EN_Endian::LSB_First)
    {
        // Copy as is
        memcpy(data_ref_, &data_, 8);
    }
    else
    {
        data_ref_[offset_] = ((TY_Byte*)&data_)[7];
        data_ref_[offset_ + 1] = ((TY_Byte*)&data_)[6];
        data_ref_[offset_ + 2] = ((TY_Byte*)&data_)[5];
        data_ref_[offset_ + 3] = ((TY_Byte*)&data_)[4];
        data_ref_[offset_ + 4] = ((TY_Byte*)&data_)[3];
        data_ref_[offset_ + 5] = ((TY_Byte*)&data_)[2];
        data_ref_[offset_ + 6] = ((TY_Byte*)&data_)[1];
        data_ref_[offset_ + 7] = ((TY_Byte*)&data_)[0];
    }
}

void CCoding::PutFloat(TY_Float data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    TY_DWord u32;

    // Copy the pattern
    // Note: Operationg system has to code as IEEE754
    *((TY_Float*)&u32) = data_;

    // Handle NaN
    if ((u32 & 0x7F800000) == 0x7F800000)
    {
        // Set none signalling standard code
        u32 = 0x7FA00000;
    }

    PutDWord(u32, offset_, data_ref_, endian_);
}

void CCoding::PutDFloat(TY_DFloat data_, TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    TY_UInt64 u64;

    // Copy the pattern
    *((TY_DFloat*)&u64) = data_;

    // Handle NaN
    if ((u64 & 0x7FF0000000000000) == 0x7FF0000000000000)
    {
        // Set none signalling standard code
        u64 = 0x7FF4000000000000;
    }

    PutInt64(u64, offset_, data_ref_, endian_);
}

void CCoding::PutPackedASCII(
    TY_Byte* asc_string_ref_, TY_Byte asc_string_len_,
    TY_Byte offset_, TY_Byte* data_ref_)
{
    TY_Byte source_len = asc_string_len_;
    TY_Word   data_idx = offset_;
    TY_Word source_idx = 0;
    TY_Word   data_len;
    TY_Word          e;

    data_len = asc_string_len_ / 4;
    if (asc_string_len_ % 4)
    {
        //String len no ordinal multiple of 4
        data_len++;
    }
    data_len *= 3;
    // initialize destination with default values
    for (e = 0; e < data_len; e++)
    {
        switch (e % 3)
        {
        case 0:
            data_ref_[offset_ + e] = 0x7d;
            break;
        case 1:
            data_ref_[offset_ + e] = 0xf7;
            break;
        case 2:
            data_ref_[offset_ + e] = 0xdf;
            break;
        }
    }
    for (e = 0; e < data_len; e++)
    {
        // first octet in triple
        if (source_len == 0)
        {
            return;
        }
        data_ref_[data_idx] &= 0x03;
        data_ref_[data_idx] |= ((asc_string_ref_[source_idx] & 0x3f) << 2);
        source_len -= 1;
        // second octet in triple
        if (source_len == 0)
        {
            return;
        }
        data_ref_[data_idx] &= 0xFC;
        data_ref_[data_idx] |= ((asc_string_ref_[source_idx + 1] & 0x30) >> 4);
        data_ref_[data_idx + 1] &= 0xF;
        data_ref_[data_idx + 1] |= (asc_string_ref_[source_idx + 1] & 0x0f) << 4;
        source_len -= 1;
        // third octet in triple
        if (source_len == 0)
        {
            return;
        }
        data_ref_[data_idx + 1] &= 0xF0;
        data_ref_[data_idx + 1] |= ((asc_string_ref_[source_idx + 2] & 0x3c) >> 2);
        data_ref_[data_idx + 2] &= 0x3F;
        data_ref_[data_idx + 2] |= asc_string_ref_[source_idx + 2] << 6;
        source_len -= 1;
        if (source_len > 0)
        {
            data_ref_[data_idx + 2] &= 0xc0;
            data_ref_[data_idx + 2] |= asc_string_ref_[source_idx + 3] & 0x3f;
            source_len -= 1;
        }
        data_idx += 3;
        source_idx += 4;
    }
}

void CCoding::PutOctets(TY_Byte* stream_ref_, TY_Byte stream_len_, TY_Byte offset_, TY_Byte* data_ref_)
{
    COSAL::CMem::Copy(&data_ref_[offset_], stream_ref_, stream_len_);
}

void CCoding::PutString(TY_Byte* string_ref_, TY_Byte string_max_len_, TY_Byte offset_, TY_Byte* data_ref_)
{
    TY_Byte len;
    TY_Word idx = offset_;

    for (len = 0; len < string_max_len_; len++)
    {
        data_ref_[idx++] = string_ref_[len];
        if ((idx >= string_max_len_) || (string_ref_[len] == 0))
        {
            break;
        }
    }
}

// Decoding
TY_Word CCoding::PickWord(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    TY_Word result = 0;

    if (data_ref_ != NULL)
    {
        if (endian_ == EN_Endian::LSB_First)
        {
            result = (TY_Word)(((TY_Word)data_ref_[offset_ + 1] << 8) +
                ((TY_Word)data_ref_[offset_] & 0xff)
                );
        }
        else
        {
            result = (TY_Word)(((TY_Word)data_ref_[offset_] << 8) +
                ((TY_Word)data_ref_[offset_ + 1] & 0xff)
                );
        }
    }
    return result;
}

TY_DWord CCoding::PickInt24(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    TY_DWord result = 0;

    if (data_ref_ != NULL)
    {
        if (endian_ == EN_Endian::LSB_First)
        {
            result = (TY_DWord)(((TY_DWord)data_ref_[offset_ + 2] << 16) +
                (((TY_DWord)data_ref_[offset_ + 1] << 8) & 0xff00) +
                ((TY_DWord)data_ref_[offset_] & 0x00ff)
                );
        }
        else
        {
            result = (TY_DWord)(((TY_DWord)data_ref_[offset_] << 16) +
                (((TY_DWord)data_ref_[offset_ + 1] << 8) & 0xff00) +
                ((TY_DWord)data_ref_[offset_ + 2] & 0x00ff)
                );
        }
    }
    return result;
}

TY_DWord CCoding::PickDWord(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    TY_DWord result = 0;

    if (data_ref_ != NULL)
    {
        if (endian_ == EN_Endian::LSB_First)
        {
            result = (TY_DWord)(((TY_DWord)data_ref_[offset_ + 3] << 24) +
                (((TY_DWord)data_ref_[offset_ + 2] << 16) & 0xff0000) +
                (((TY_DWord)data_ref_[offset_ + 1] << 8) & 0x00ff00) +
                ((TY_DWord)data_ref_[offset_] & 0x0000ff)
                );
        }
        else
        {
            result = (TY_DWord)(((TY_DWord)data_ref_[offset_] << 24) +
                (((TY_DWord)data_ref_[offset_ + 1] << 16) & 0xff0000) +
                (((TY_DWord)data_ref_[offset_ + 2] << 8) & 0x00ff00) +
                ((TY_DWord)data_ref_[offset_ + 3] & 0x0000ff)
                );
        }
    }
    return result;
}

TY_UInt64 CCoding::PickInt64(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    TY_UInt64 result = 0;

    if (data_ref_ != NULL)
    {
        if (endian_ == EN_Endian::LSB_First)
        {
            // Copy as is
            memcpy(&result, data_ref_, 8);
        }
        else
        {
            ((TY_Byte*)&result)[7] = data_ref_[offset_];
            ((TY_Byte*)&result)[6] = data_ref_[offset_ + 1];
            ((TY_Byte*)&result)[5] = data_ref_[offset_ + 2];
            ((TY_Byte*)&result)[4] = data_ref_[offset_ + 3];
            ((TY_Byte*)&result)[3] = data_ref_[offset_ + 4];
            ((TY_Byte*)&result)[2] = data_ref_[offset_ + 5];
            ((TY_Byte*)&result)[1] = data_ref_[offset_ + 6];
            ((TY_Byte*)&result)[0] = data_ref_[offset_ + 7];
        }
    }

    return result;
}

TY_Float CCoding::PickFloat(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    TY_DWord u32;
    TY_Float result;

    // Get stream
    u32 = PickDWord(offset_, data_ref_, endian_);

    // Handle NaN
    if ((u32 & 0x7F800000) == 0x7F800000)
    {
        // Set none signalling standard code
        u32 = 0x7FA00000;
    }

    // Copy the pattern to the float value
    // Note: Operationg system has to code as IEEE754
    result = *((TY_Float*)&u32);

    return result;
}

TY_DFloat CCoding::PickDouble(TY_Byte offset_, TY_Byte* data_ref_, EN_Endian endian_)
{
    TY_UInt64 u64;
    TY_DFloat result;

    // Get stream
    u64 = PickInt64(offset_, data_ref_, endian_);

    // Handle NaN
    if ((u64 & 0x7FF0000000000000) == 0x7FF0000000000000)
    {
        // Set none signalling standard code
        u64 = 0x7FF4000000000000;
    }

    // Copy the pattern to the float value
    // Note: Operationg system has to code as IEEE754
    result = *((TY_DFloat*)&u64);

    return result;
}

void CCoding::PickPackedASCII(TY_Byte* string_ref_, TY_Byte string_max_len_, TY_Byte offset_, TY_Byte* data_ref_)
{
    TY_DWord  data_len = (string_max_len_ / 4) * 3;
    TY_DWord  data_idx = 0;
    TY_Byte       uchr;

    if (data_len > 0)
    {
        for (TY_Word e = offset_; e < (offset_ + data_len - 2); e += 3)
        {
            uchr = ((data_ref_[e] >> 2) & 0x3F);
            if (uchr < 0x20)
            {
                uchr += 0x40;
            }

            string_ref_[data_idx++] = uchr;
            uchr = ((data_ref_[e] << 4) & 0x3F) |
                ((data_ref_[e + 1] >> 4) & 0x3F);
            if (uchr < 0x20)
            {
                uchr += 0x40;
            }

            string_ref_[data_idx++] = uchr;
            uchr = ((data_ref_[e + 1] << 2) & 0x3F) |
                ((data_ref_[e + 2] >> 6) & 0x3F);
            if (uchr < 0x20)
            {
                uchr += 0x40;
            }

            string_ref_[data_idx++] = uchr;
            uchr = data_ref_[e + 2] & 0x3F;
            if (uchr < 0x20)
            {
                uchr += 0x40;
            }

            string_ref_[data_idx++] = uchr;
        }
    }
}

void CCoding::PickOctets(TY_Byte* stream_ref_, TY_Byte stream_len_, TY_Byte offset_, TY_Byte* data_ref_)
{
    COSAL::CMem::Copy(stream_ref_, &data_ref_[offset_], stream_len_);
}

void CCoding::PickString(TY_Byte* string_ref_, TY_Byte string_max_len_, TY_Byte offset_, TY_Byte* data_ref_)
{
    if (string_max_len_ > 0)
    {
        for (TY_Word e = 0; e < string_max_len_; e++)
        {
            string_ref_[e] = data_ref_[offset_ + e];
            if (string_ref_[e] == 0)
            {
                break;
            }
        }
    }
}