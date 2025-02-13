/*
 *          File: CHelpers.cs (CHelpers)
 *                A number of small functions are implemented in the 
 *                helpers that do nothing other than convert numbers into 
 *                formatted text in a certain way. The functions generally 
 *                have nothing to do with Hart.
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

namespace BaTestHart.CommonHelpers
{
    #region Namespaces
    using System;
    using System.Globalization;
    using System.Text;
    using System.Windows.Forms;
    #endregion Namespaces

    internal static class CHelpers
    {
        #region Fields
        private static string m_dec_sep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.ToString();
        #endregion Fields

        #region Delegates
        // A general delegate
        internal delegate void DelSignalSomething();
        #endregion Delegates

        #region Public Methods
        #region String Handling
        // Add an array of strings to a rich text_ box
        internal static void AddMoreStringsToRtxBox(string[] str_, RichTextBox rtx_ctr_)
        {
            if (str_.GetLength(0) == 0)
            {
                return;
            }

            for (int n = 0; n < str_.GetLength(0); n++)
            {
                rtx_ctr_.AppendText("\n" + str_[n]);
            }
        }
        #endregion String Handling

        #region Wait and Others

        // Wait for a number of milli seconds
        //     wait_time_: The number of milli seconds to wait
        internal static void Wait(int wait_time_)
        {
            System.Threading.Thread.Sleep(wait_time_);
        }

        #endregion Wait and Others
        #endregion Public Methods

        #region Nested Classes
        #region Static Classes
        // The class contains special formatting methods
        internal static class CFormat
        {
        internal static bool GetFloatFromString(string text, out float fOut)
        {
            string s = text;

                fOut = 0.0f;
                if ((text == "-/-") || (text == string.Empty))
            {
                return false;
            }

            if (m_dec_sep == ",")
            {
                s = s.Replace('.', ',');
            }

            if (m_dec_sep == ".")
            {
                s = s.Replace(',', '.');
            }

            if (!float.TryParse(s, out float f))
            {
                return false;
            }

            fOut = f;
            return true;
        }

        internal static bool GetDoubleFromString(string text, out double dOut)
        {
            string s = text;

                dOut = 0.0;
                if ((text == "-/-") || (text == string.Empty))
            {
                return false;
            }

            if (m_dec_sep == ",")
            {
                s = s.Replace('.', ',');
            }

            if (m_dec_sep == ".")
            {
                s = s.Replace(',', '.');
            }

            if (!double.TryParse(s, out double d))
            {
                return false;
            }

            dOut = d;
            return true;
        }
            // Format a float number using 5 digits to display
            internal static string Float5digit(float f_)
            {
                if (Math.Abs(f_) >= 10000.0f)
                {
                    return f_.ToString("####0");
                }

                if (Math.Abs(f_) >= 1000.0f)
                {
                    return f_.ToString("###0.0");
                }

                if (Math.Abs(f_) >= 100.0f)
                {
                    return f_.ToString("##0.00");
                }

                if (Math.Abs(f_) >= 10.0f)
                {
                    return f_.ToString("#0.000");
                }

                if (Math.Abs(f_) >= 1.0f)
                {
                    return f_.ToString("0.0000");
                }

                if (Math.Abs(f_) < 0.00001f)
                {
                    return f_.ToString("0.0");
                }

                return f_.ToString("0.00000");
            }

            // Format a float number
            internal static string Number(int chars_, float f_)
            {
                string s;

                s = f_.ToString("0.0");
                return FitIntoField(chars_, s);
            }

            // Format an unsigned integer
            internal static string Number(int chars_, uint dword_)
            {
                string s;

                s = dword_.ToString("0");
                return FitIntoField(chars_, s);
            }

            // Format an integer
            internal static string Number(int chars_, int int32_)
            {
                string s;

                s = int32_.ToString("0");
                return FitIntoField(chars_, s);
            }

            // Format an unsigned short
            internal static string Number(int chars_, ushort word_)
            {
                string s;

                s = word_.ToString("0");
                return FitIntoField(chars_, s);
            }

            // Format a byte
            internal static string Number(int chars_, byte byte_)
            {
                string s;

                s = byte_.ToString("0");
                return FitIntoField(chars_, s);
            }

            // Pad a string left and add one space on both sides
            internal static string FitIntoField(int chars_, string string_)
            {
                return " " + string_.PadLeft(chars_) + " ";
            }

            // Convert a byte to a binary representation
            internal static string Bin(byte byte_)
            {
                string s = string.Empty;
                byte e;
                byte mask = 0x80;

                for (e = 0; e < 8; e++)
                {
                    if ((byte_ & mask) > 0)
                    {
                        s += "1";
                    }
                    else
                    {
                        s += "0";
                    }

                    mask = (byte)(mask >> 1);
                }

                return s;
            }
        }

        // The class is used for methods which are checking numeric string expressions
        internal static class IsValid
        {
            // Check if the expression allows to get a byte
            internal static bool Byte(ref byte byte_, string string_)
            {
                try
                {
                    byte_ = Convert.ToByte(string_);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            // Check if the expression allows to get a unsinged integer 16
            internal static bool Ushort(ref ushort word_, string string_)
            {
                try
                {
                    word_ = Convert.ToUInt16(string_);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        // For easier programming this class is providing standard
        // methods to convert a string into a numeric byte
        // or (in case of a text into an 8 bit characters array
        internal static class CFromString
        {
            // Convert a string to a float byte_
            // Resolve any conflicts arising from the decimal separation
            // character.
            //       text_: Text containing the expression
            //     return: The resulting float byte_
            internal static float Float(string text_)
            {
                string s = text_;
                if ((text_ == "-/-") || (text_ == string.Empty))
                {
                    return 0.0f;
                }

                if (m_dec_sep == ",")
                {
                    s = s.Replace('.', ',');
                }

                if (m_dec_sep == ".")
                {
                    s = s.Replace(',', '.');
                }

                if (!float.TryParse(s, out float f))
                {
                    f = 0.0f;
                }

                return f;
            }

            // Convert a string to a double
            // Resolve any conflicts arising from the decimal separation
            // character.
            internal static double Double(string text_)
            {
                string s = text_;
                if ((text_ == "-/-") || (text_ == string.Empty))
                {
                    return 0.0f;
                }

                if (m_dec_sep == ",")
                {
                    s = s.Replace('.', ',');
                }

                if (m_dec_sep == ".")
                {
                    s = s.Replace(',', '.');
                }

                if (!double.TryParse(s, out double d))
                {
                    d = 0.0f;
                }

                return d;
            }

            // Retrieve an unsigned integer 8 from a string
            internal static byte Byte(string text_)
            {
                string s = text_;

                return Convert.ToByte(s);
            }

            internal static byte Binary(string text_)
            {
                if (text_.Length > 8)
                {
                    return 0xff;
                }

                if (text_.Length == 0)
                {
                    return 0;
                }

                byte result = 0;

                char[] binArr = text_.ToCharArray();
                for (int i = 0; i < binArr.Length; i++)
                {
                    result = (byte)(result << 1);

                    if ((binArr[i] == '1'))
                    {
                        result++;
                    }
                }

                return result;
            }

            // Retrieve an unsigned integer 16 from a string
            internal static ushort Word(string text_)
            {
                string s = text_;

                ulong val = DWord(s);

                return (ushort) val;
            }

            internal static byte[] ThreeBytes(string text_)
            {
                ulong u = DWord(text_);
                byte[] result = new byte[3];

                result[2] = (byte)u;
                result[1] = (byte)(u >> 8);
                result[0] = (byte)(u >> 16);

                return result;
            }

            // Retrieve an unsigned integer 32 from a string
            internal static ulong DWord(string text_)
            {
                string text = text_.Trim().ToLower();
                ulong u = 0;
                bool done = false;

                if (text.Length > 2)
                {
                    if (text.Substring(0, 2) == "0x")
                    {
                        // Marked as hex number
                        text = text.Substring(2);
                        // Try convert from hex
                        if (ulong.TryParse(text, NumberStyles.HexNumber, null, out u))
                        {
                            done = true;
                        }
                    }
                }

                if (!done)
                {
                    // Decimal?
                    if (ulong.TryParse(text, out u))
                    {
                        done = true;
                    }
                }

                if (!done)
                {
                    if (ulong.TryParse(text, NumberStyles.HexNumber, null, out u))
                    {
                        done = true;
                    }
                }

                if (!done)
                {
                    MessageBox.Show("Item '" + text_ + "' was not decoded correctly!");
                }

                return u;
            }

            // Get the boolean byte_ from a text_
            internal static bool Bool(string text_)
            {
                string s = text_.Trim().ToLower();

                if (s == "true")
                {
                    return true;
                }

                return false;
            }

            // Gets a byte array from hexadecimal expressions separated byte_ a comma such as
            // 01,B3,C4
            internal static byte[] GetHexBytes(string byte_stream_)
            {
                int num = 0;
                string[] astr = byte_stream_.Split(',');
                byte[] bytes = new byte[num];

                if (astr.GetLength(0) > 0)
                {
                    num = (int)astr.GetLength(0);
                    bytes = new byte[num];

                    for (int e = 0; e < num; e++)
                    {
                        bytes[e] = CFromString.Byte(astr[e]);
                    }
                }

                return bytes;
            }
            internal static void GetTextBytes(ref byte bytes_, string text_, byte length_)
            {
                StringBuilder sb = new StringBuilder(length_);
                if (text_.Length < length_)
                {
                    text_ = text_.PadRight(length_, ' ');
                }
                sb.Insert(0, text_);
                HartDeviceDLL.BAHA_PutString(sb, length_, 0, ref bytes_);
            }
            internal static void GetPackedASCIIbytes(ref byte bytes_, string text_, byte length_)
            {
                StringBuilder sb = new StringBuilder(length_);
                if (text_.Length < length_)
                {
                    text_ = text_.PadRight(length_, ' ');
                }
                sb.Insert(0, text_);
                HartDeviceDLL.BAHA_PutPackedASCII(sb, length_, 0, ref bytes_);
            }
        }

        internal static class CFromBytes
        {
            internal static string GetPackedASCIItext(byte[] bytes_, byte length_)
            {
                byte string_len = (byte)((length_ / 3) * 4);

                StringBuilder sb = new StringBuilder(string_len);
                sb.Length = string_len;
                HartDeviceDLL.BAHA_PickPackedASCII(sb, string_len, 0, ref bytes_[0]);
                return sb.ToString();
            }
        }

        // The class is containing methods targeted to generate hex dumps
        internal static class CHex
        {
            // Converts a stream of bytes to an hexadecimal representation
            //        BytesOfData: The stream of bytes
            //                len_: Length of the byte stream
            //     bytes_per_string_: Number of bytes to be formatted into one string
            //             return: Array of strings containing the hexadecimal representation
            internal static string[] SimpleDump(byte[] bytes_of_data_, byte len_, byte bytes_per_string_)
            {
                string[] result_ = [];
                int idx = 0;
                string s = string.Empty;

                if (len_ == 0)
                {
                    return result_;
                }

                while (idx < len_)
                {
                    for (int n = 0; (n < bytes_per_string_) && (idx < len_); n++)
                    {
                        if ((n < (bytes_per_string_ - 1)) && (idx < (len_ - 1)))
                        {
                            s += bytes_of_data_[idx++].ToString("X2") + " ";
                        }
                        else
                        {
                            s += bytes_of_data_[idx++].ToString("X2");
                        }
                    }

                    if (idx < len_)
                    {
                        s += ";";
                    }
                }

                return s.Split(';');
            }

            // Create the hexadecimal representation of a byte stream in one string
            internal static string DumpBytes(byte[] bytes_of_data_)
            {
                string s = string.Empty;
                byte dataLen = Convert.ToByte(bytes_of_data_.GetLength(0));

                if (bytes_of_data_.GetLength(0) == 0)
                {
                    s = "-/-";
                }
                else
                {
                    for (int e = 0; e < dataLen; e++)
                    {
                        if (e == 0)
                        {
                            s += GetHex(bytes_of_data_[e]);
                        }
                        else
                        {
                            s += " " + GetHex(bytes_of_data_[e]);
                        }
                    }
                }

                return s;
            }

            // Create the hexadecimal representation of a byte stream in one string
            internal static string DataDump(byte[] bytes_of_data_, byte len_)
            {
                string s = string.Empty;
                int e;

                if (len_ == 0)
                {
                    s = "-/-";
                }
                else
                {
                    for (e = 0; e < len_; e++)
                    {
                        if (e == 0)
                        {
                            s += GetHex(bytes_of_data_[e]);
                        }
                        else
                        {
                            s += " " + GetHex(bytes_of_data_[e]);
                        }
                    }
                }

                return s;
            }

            // Create the hexadecimal representation of a byte stream in one string
            // Use a format useful for initializer such as 0x01, 0x02 ...
            internal static string DumpAsInitializer(byte[] bytes_of_data_, byte len_)
            {
                string s = string.Empty;
                int e;

                if (len_ == 0)
                {
                    s = "-/-";
                }
                else
                {
                    for (e = 0; e < len_; e++)
                    {
                        if (e == 0)
                        {
                            s += "0x" + GetHex(bytes_of_data_[e]);
                        }
                        else
                        {
                            s += "," + "0x" + GetHex(bytes_of_data_[e]);
                        }
                    }
                }

                return s;
            }

            #region Hex Dump Helpers

            // Format a byte to a hexadecimal representation
            internal static string GetHex(byte byte_)
            {
                string s = string.Empty + HexDigit((byte)((byte_ & 0xF0) >> 4));
                return s + HexDigit((byte)(byte_ & 0x0F));
            }

            // Convert the values 0..15 of a byte to the characters '0'..'F'
            internal static char HexDigit(byte byte_)
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
                    default:
                        break;
                }

                return '?';
            }

            // Convert two hexadecimal characters to a byte byte_
            internal static byte GetByteFromString(string string_)
            {
                byte by;

                string_ = string_.ToUpper();
                by = Convert.ToByte(GetNibbleFromChar(string_.ToCharArray()[0]) * 16);
                by = Convert.ToByte(GetNibbleFromChar(string_.ToCharArray()[1]) + by);
                return by;
            }

            // Convert a character in the range from '0' to 'F'
            // to a byte byte_
            internal static byte GetNibbleFromChar(char char_)
            {
                return char_ switch
                {
                    '0' => 0,
                    '1' => 1,
                    '2' => 2,
                    '3' => 3,
                    '4' => 4,
                    '5' => 5,
                    '6' => 6,
                    '7' => 7,
                    '8' => 8,
                    '9' => 9,
                    'A' => 10,
                    'B' => 11,
                    'C' => 12,
                    'D' => 13,
                    'E' => 14,
                    'F' => 15,
                    _ => 0,
                };
            }

            #endregion Hex Dump Helpers
        }

        #endregion Static Classes

        #region Normal Classes
        // CItem is a simple construct providing strings for a m_name and a byte_(text_)
        internal class CItem(string name_, string text_)
        {
            #region Fields
            // The m_name of the item
            private string m_name = name_;

            // The byte_ of the item (as text_)
            private string m_text = text_;
            #endregion Fields

            #region Public Properties
            // Gets the string for the m_name of the item
            internal string Name
            {
                get
                {
                    return this.m_name;
                }
            }

            // Gets the string for the byte_ of the item
            internal string Text
            {
                get
                {
                    return this.m_text;
                }
            }
            #endregion Public Properties
        }

        #endregion Normal Classes
        #endregion Nested Classes
    }
}
