/*
 *          File: FrameHelpers.cs (CFrameHelper)
 *                A set of classes and methods needed to interact with the user. 
 *                Mainly it's_ about decoding and representation.
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

#region Namespaces
using BaTestHart.CommonHelpers;
using BaTestHart.HartUtils;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
#endregion Namespaces

namespace BaTestHart
{
    internal static class CFrameHelper
    {
        private static string m_sDecSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();

        #region Private Data
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        private static byte[] mo_bytes_of_data = Array.Empty<byte>();
        #endregion

        #region Initialization
        internal static void Init()
        {
            mo_bytes_of_data = new byte[256];
        }
        #endregion Initialization

        #region Internal Functions
        internal static byte GetByteFromTextBox(TextBox txt_)
        {
            byte value = 0;

            if (byte.TryParse(txt_.Text, out value) == false)
            { 
                return 0;
            }

            return value;
        }

        internal static ushort GetUShortFromTextBox(TextBox txt_)
        {
            ushort value = 0;

            if (ushort.TryParse(txt_.Text, out value) == true)
            {
                return value;
            }

            return 0;
        }

        internal static string GetHex(byte byte_)
        {
            string s = string.Empty + HexDigit((byte)((byte_ & 0xF0) >> 4));
            return s + HexDigit((byte)(byte_ & 0x0F));
        }

        internal static string GetDec(byte byte_)
        {
            return byte_.ToString("0").PadLeft(2, ' ');
        }

        internal static string GetBin(byte byte_)
        {
            string s = string.Empty;
            byte e;
            byte byMask = 0x80;

            for (e = 0; e < 8; e++)
            {
                if ((byte_ & byMask) > 0)
                {
                    s += "1";
                }
                else
                {
                    s += "0";
                }
                byMask = (byte)(byMask >> 1);
            }
            return s;
        }

        internal static byte GetByteFromDecHex(string s_)
        {
            byte by = 0;

            if (s_.StartsWith("0X", StringComparison.CurrentCultureIgnoreCase))
            {
                if (!byte.TryParse(s_.AsSpan(2), NumberStyles.HexNumber, CultureInfo.CurrentCulture, out by))
                {
                    by = 0;
                }
            }
            else
            {
                if (!byte.TryParse(s_, NumberStyles.Integer, CultureInfo.CurrentCulture, out by))
                {
                    by = 0;
                }
            }

            return by;
        }

        internal static byte[] GetByteArray(string list_)
        {
            char[] delimiters = new char[] { ';', ',' };
            string[] astr = list_.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (astr.Length == 0)
            {
                // try to read single element 
                astr = new string[1] { list_ };
            }
            byte[] aby = new byte[astr.Length];
            int iLen = astr.Length;

            if (iLen > 0)
            {
                for (int i = 0; i < iLen; i++)
                {
                    aby[i] = GetByteFromDecHex(astr[i]);
                }
            }
            return aby;
        }

        internal static bool IsMatchingPattern(byte value_, string pattern_)
        {
            pattern_ = pattern_.Trim();

            if (pattern_.Length > 8)
            {
                return false;
            }

            if (pattern_.Length == 0)
            {
                return false;
            }

            pattern_ = pattern_.ToUpper();

            char[] ach = pattern_.ToCharArray();

            int iLen = ach.Length;
            byte byMask = 0x01;
            bool bMatch = true;

            for (int i = iLen - 1; i > 0; i--)
            {
                if (ach[i] != 'X')
                {
                    if (ach[i] == '1')
                    {
                        if ((value_ & byMask) == 0)
                        {
                            bMatch = false;
                            break;
                        }
                    }
                    if (ach[i] == '0')
                    {
                        if ((value_ & byMask) != 0)
                        {
                            bMatch = false;
                            break;
                        }
                    }
                }
                byMask <<= 1;
            }
            return bMatch;
        }

        internal static bool IsValidPattern(string pattern_)
        {
            pattern_ = pattern_.Trim();

            if (pattern_.Length > 8)
            {
                return false;
            }

            if (pattern_.Length == 0)
            {
                return false;
            }

            pattern_ = pattern_.ToUpper();

            char[] ach = pattern_.ToCharArray();

            int iLen = ach.Length;
            bool bValid = true;

            for (int i = iLen - 1; i > 0; i--)
            {
                if ((ach[i] != 'X') && (ach[i] != '1') && (ach[i] != '0'))
                {
                    bValid = false;
                    break;
                }
            }
            return bValid;
        }

        internal static string AppendTextWithComma(string s_, string appendix_)
        {
            if (appendix_ == string.Empty)
            {
                return s_;
            }
            if (s_ != string.Empty)
            {
                s_ += ", ";
            }
            return s_ + appendix_;
        }

        internal static string AppendTextSlash(string s_, string appendix_)
        {
            if (appendix_ == string.Empty)
            {
                return s_;
            }
            if (s_ != string.Empty)
            {
                s_ += " / ";
            }
            return s_ + appendix_;
        }

        internal static string AppendTextParagraf(string s_, string appendix_)
        {
            if (s_ != string.Empty)
            {
                s_ += "§";
            }
            return s_ + appendix_;
        }

        internal static string HexDump(byte[] bytes_of_data_, byte offset_, byte len_)
        {
            string s = string.Empty;

            if (bytes_of_data_.Length >= (offset_ + len_))
            {
                for (int e = 0; e < len_; e++)
                {
                    if (e == 0)
                    {
                        s += CFrameHelper.GetHex(bytes_of_data_[offset_ + e]);
                    }
                    else
                    {
                        s += " " + CFrameHelper.GetHex(bytes_of_data_[offset_ + e]);
                    }
                }
            }
            return s;
        }

        internal static string HexDumpByteDataToDataStx(ushort len_)
        {
            string s = string.Empty;

            if (mo_bytes_of_data.Length >= (len_))
            {
                for (int e = 0; e < len_; e++)
                {
                    if (e == 0)
                    {
                        s += "0x" + CFrameHelper.GetHex(mo_bytes_of_data[e]).ToLower();
                    }
                    else
                    {
                        s += ";0x" + CFrameHelper.GetHex(mo_bytes_of_data[e]).ToLower();
                    }
                }
            }
            return s;
        }
        #endregion Internal Functions

        #region Fomatting Numbers
        internal static string FormatFloat5digit(float float_)
        {
            if (Math.Abs(float_) >= 10000.0f)
            {
                return float_.ToString("####0");
            }
            if (Math.Abs(float_) >= 1000.0f)
            {
                return float_.ToString("###0.0");
            }
            if (Math.Abs(float_) >= 100.0f)
            {
                return float_.ToString("##0.00");
            }
            if (Math.Abs(float_) >= 10.0f)
            {
                return float_.ToString("#0.000");
            }
            if (Math.Abs(float_) >= 1.0f)
            {
                return float_.ToString("0.0000");
            }
            if (Math.Abs(float_) < 0.00001f)
            {
                return float_.ToString("0.0");
            }
            return float_.ToString("0.00000");
        }

        internal static string FormatNumber(int chars_, float float_)
        {
            string s;

            s = float_.ToString("0.0");
            return FitIntoField(chars_, s);
        }

        internal static string FormatNumber(int chars_, uint dword_)
        {
            string s;

            s = dword_.ToString("0");
            return FitIntoField(chars_, s);
        }

        internal static string FormatNumber(int chars_, int sint32_)
        {
            string s;

            s = sint32_.ToString("0");
            return FitIntoField(chars_, s);
        }

        internal static string FormatNumber(int chars_, ushort word_)
        {
            string s;

            s = word_.ToString("0");
            return FitIntoField(chars_, s);
        }

        internal static string FormatNumber(int chars_, byte byte_)
        {
            string s;

            s = byte_.ToString("0");
            return FitIntoField(chars_, s);
        }

        internal static string FitIntoField(int chars_, string s_)
        {
            int e;
            int iSpacesNeeded;

            if (s_.Length < (chars_ - 2))
            {
                iSpacesNeeded = chars_ - 2 - s_.Length;
                if (iSpacesNeeded > 0)
                {
                    for (e = 0; e < iSpacesNeeded; e++)
                    {
                        s_ = " " + s_;
                    }
                }
            }
            return " " + s_ + " ";
        }

        internal static float getFloatFromString(string text_)
        {
            string s = text_;
            float f;
            if ((text_ == "-/-") || (text_ == string.Empty))
            {
                return 0.0f;
            }

            if (m_sDecSep == ",")
            {
                s = s.Replace('.', ',');
            }
            if (m_sDecSep == ".")
            {
                s = s.Replace(',', '.');
            }
            if (!float.TryParse(s, out f))
            {
                f = 0.0f;
            }
            return f;
        }

        internal static byte getInt8FromString(string text_)
        {
            string s = text_;

            return Convert.ToByte(s);
        }
        #endregion

        #region Private Functions
        private static char DecDigit(byte byte_)
        {
            switch (byte_)
            {
                case 0:
                    return '0';
                case 1:
                    return '1';
                case 2:
                    return '2';
                case 3:
                    return '3';
                case 4:
                    return '4';
                case 5:
                    return '5';
                case 6:
                    return '6';
                case 7:
                    return '7';
                case 8:
                    return '8';
                case 9:
                    return '9';
            }
            return '?';
        }

        private static char HexDigit(byte byte_)
        {
            switch (byte_ & 0x0F)
            {
                case 0:
                    return '0';
                case 1:
                    return '1';
                case 2:
                    return '2';
                case 3:
                    return '3';
                case 4:
                    return '4';
                case 5:
                    return '5';
                case 6:
                    return '6';
                case 7:
                    return '7';
                case 8:
                    return '8';
                case 9:
                    return '9';
                case 0x0a:
                    return 'A';
                case 0x0b:
                    return 'B';
                case 0x0c:
                    return 'C';
                case 0x0d:
                    return 'D';
                case 0x0e:
                    return 'E';
                case 0x0f:
                    return 'F';
            }
            return '?';
        }
        #endregion

        #region Internal Methods
        internal static ushort GetBytesFromFrameData(string s_)
        {
            string[] astr;
            //byte[]     byte_;
            ushort e;
            ushort usLen = 0;

            s_ = s_.Trim().ToUpper();
            s_ = s_.Replace("CMD", "CMD ");
            if ((astr = s_.Split(new char[] { ' ', '|', '?' }, StringSplitOptions.RemoveEmptyEntries)) != null)
            {
                for (e = 0; e < astr.Length; e++)
                {
                    if (astr[e] == "CMD")
                    {
                        bool bIsResponse = false;
                        // resolve region around the cmd
                        if (usLen > 1)
                        {
                            if (((mo_bytes_of_data[usLen - 2] & 0x07) == 6) || ((mo_bytes_of_data[usLen - 2] & 0x07) == 1))
                            {
                                bIsResponse = true;
                            }
                            if (e < astr.Length)
                            {
                                e++;
                                mo_bytes_of_data[usLen++] = GetByteFromDecHex(astr[e]); // command
                            }
                            if (e < astr.Length)
                            {
                                e++;
                                mo_bytes_of_data[usLen++] = GetByteFromDecHex(astr[e]); // count
                            }
                            if (bIsResponse)
                            {
                                if (e < astr.Length)
                                {
                                    e++;
                                    mo_bytes_of_data[usLen++] = GetByteFromDecHex(astr[e]); // rsp1
                                }
                                if (e < astr.Length)
                                {
                                    e++;
                                    mo_bytes_of_data[usLen++] = GetByteFromDecHex(astr[e]); // rsp2
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((astr[e].Length > 0) && (astr[e].Length < 3))
                        {
                            if (astr[e].Length == 1)
                            {
                                mo_bytes_of_data[usLen++] = GetByteFromDecHex("0X0" + astr[e]);
                            }
                            else
                            {
                                mo_bytes_of_data[usLen++] = GetByteFromDecHex("0X" + astr[e]);
                            }
                        }
                    }
                }
                return usLen;
            }
            return 0;
        }

        internal static void SignalNoSelection()
        {
            MessageBox.Show("You have to select the data bytes in the frame which you want to have being decoded.", "Invalid selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        internal static void SignalInvalidSelection(string s_)
        {
            MessageBox.Show("It may be the case that you selected an area of m_text which is not representing any hexadecimal data to be decoded or you selected " +
                            "not enough data bytes for the deconding. You selected: " + s_ + ".", "Invalid selection",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        #endregion Internal Methods

        #region Nested Classes
        internal static class CDecode
        {
            #region Decimal Integer
            internal static string DecodeInteger(ushort len_)
            {
                string s = Dec8(len_);
                if (len_ > 1)
                {
                    s += "\n" + Dec16(len_);
                }
                if (len_ > 2)
                {
                    s += "\n" + Dec24(len_);
                }
                if (len_ > 3)
                {
                    s += "\n" + Dec32(len_);
                }
                return s;
            }

            internal static string Dec8(ushort len_)
            {
                ushort n;
                string sTmp = string.Empty;
                byte by;

                for (n = 0; n < len_; n++)
                {
                    if (sTmp.Length > 0)
                    {
                        sTmp += " | ";
                    }
                    by = mo_bytes_of_data[n];
                    sTmp += by.ToString();
                }
                return sTmp;
            }

            internal static string Dec16(ushort len_)
            {
                ushort n;
                byte byOffset = 0;
                string sTmp = string.Empty;
                ushort us;

                n = len_;
                while (n >= 2)
                {
                    if (sTmp.Length > 0)
                    {
                        sTmp += " | ";
                    }
                    us = HartDeviceDLL.BAHA_PickWord(byOffset, ref mo_bytes_of_data[0], HartDeviceDLL.MSBfirst);
                    sTmp += us.ToString();
                    n -= 2;
                    byOffset += 2;
                }
                return sTmp;
            }

            internal static string Dec24(ushort len_)
            {
                ushort n;
                byte byOffset = 0;
                string sTmp = string.Empty;
                uint ui;

                n = len_;
                while (n >= 3)
                {
                    if (sTmp.Length > 0)
                    {
                        sTmp += " | ";
                    }
                    ui = HartDeviceDLL.BAHA_PickInt24(byOffset, ref mo_bytes_of_data[0], HartDeviceDLL.MSBfirst);
                    sTmp += ui.ToString();
                    n -= 3;
                    byOffset += 3;
                }
                return sTmp;
            }

            internal static string Dec32(ushort len_)
            {
                ushort n;
                byte byOffset = 0;
                string sTmp = string.Empty;
                uint ui;

                n = len_;
                while (n >= 4)
                {
                    if (sTmp.Length > 0)
                    {
                        sTmp += " | ";
                    }
                    ui = HartDeviceDLL.BAHA_PickDWord(byOffset, ref mo_bytes_of_data[0], HartDeviceDLL.MSBfirst);
                    sTmp += ui.ToString();
                    n -= 4;
                    byOffset += 4;
                }
                return sTmp;
            }
            #endregion

            #region Float
            internal static string DecodeFloat(ushort len_)
            {
                string s = Float(len_);
                return s;
            }

            internal static string Float(ushort len_)
            {
                ushort n;
                byte byOffset = 0;
                string sTmp = string.Empty;
                float f;

                n = len_;
                while (n >= 4)
                {
                    if (sTmp.Length > 0)
                    {
                        sTmp += " | ";
                    }
                    f = HartDeviceDLL.BAHA_PickFloat(byOffset, ref mo_bytes_of_data[0], HartDeviceDLL.MSBfirst);
                    sTmp += f.ToString();
                    n -= 4;
                    byOffset += 4;
                }
                return sTmp;
            }
            #endregion

            #region Double
            internal static string DecodeDouble(ushort len_)
            {
                string s = Double(len_);
                return s;
            }

            internal static string Double(ushort len_)
            {
                ushort n;
                byte byOffset = 0;
                string sTmp = string.Empty;
                double d;

                n = len_;
                while (n >= 8)
                {
                    if (sTmp.Length > 0)
                    {
                        sTmp += " | ";
                    }
                    d = HartDeviceDLL.BAHA_PickDouble(byOffset, ref mo_bytes_of_data[0], HartDeviceDLL.MSBfirst);
                    sTmp += d.ToString();
                    n -= 8;
                    byOffset += 8;
                }
                return sTmp;
            }
            #endregion

            #region Hart Unit
            internal static string DecodeHartUnit(ushort len_)
            {
                string s = HartUnit(len_);
                return s;
            }

            internal static string HartUnit(ushort len_)
            {
                ushort n;
                string sTmp = string.Empty;
                byte by;

                for (n = 0; n < len_; n++)
                {
                    if (sTmp.Length > 0)
                    {
                        sTmp += " | ";
                    }
                    by = mo_bytes_of_data[n];
                    sTmp += CHartUtils.CHartUnit.GetText(by);
                }
                return sTmp;
            }
            #endregion

            #region Packed ASCII
            internal static string DecodePackedASCII(ushort len_)
            {
                string s = PackedASCII(len_);
                return s;
            }

            internal static string PackedASCII(ushort len_)
            {
                StringBuilder sb;

                sb = new StringBuilder((len_ * 4) / 3);
                sb.Length = (len_ * 4) / 3;
                HartDeviceDLL.BAHA_PickPackedASCII(sb, (byte)((len_ * 4) / 3), 0, ref mo_bytes_of_data[0]);
                return sb.ToString();
            }
            #endregion

            #region Text String
            internal static string DecodeText(ushort len_)
            {
                string s = TextString(len_);
                return s;
            }

            internal static string TextString(ushort len_)
            {
                StringBuilder sb;

                sb = new StringBuilder(len_);
                HartDeviceDLL.BAHA_PickString(sb, (byte)(len_), 0, ref mo_bytes_of_data[0]);
                return sb.ToString();
            }
            #endregion

            #region Binary
            internal static string DecodeBinary(ushort len_)
            {
                string s = Bin8(len_);
                return s;
            }

            internal static string Bin8(ushort len_)
            {
                ushort n;
                string sTmp = string.Empty;
                byte by;

                for (n = 0; n < len_; n++)
                {
                    if (sTmp.Length > 0)
                    {
                        sTmp += " | ";
                    }
                    by = mo_bytes_of_data[n];
                    sTmp += CHelpers.CFormat.Bin(by);
                }
                return sTmp;
            }
            #endregion
        }

        #endregion Nested Classes
    }
}
