/*
 *          File: DataSyntax.cs (CDataSyntaxItem, CDataSyntax, CDataSyntaxHelper)
 *                DataSyntax is a simple construct that allows the user to 
 *                define the structure of a data set. It's a bit similar to 
 *                the definition of a structure in C++.
 *                Small example (data for command 18):
 *                pca6;TAG NAME;pca12;MESSAGE 16 CHARS;dec8;5;dec8;11;dec8;111
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
using System.Runtime.InteropServices;
using System.Text;
#endregion Namespaces

namespace BaTestHart.DataSyntax
{
    internal class CDataSyntaxItem
    {
        #region Enums
        internal enum StxItemType : byte
        {
            INT8 = 0,
            FLOAT32 = 1,
            BIN8 = 2,
            UNIT8 = 3,
            PASC6 = 4,
            PASC12 = 5,
            PASC24 = 6,
            STRING8 = 7,
            STRING16 = 8,
            STRING32 = 9,
            INT16 = 10,
            INT24 = 11,
            INT32 = 12,
            HEX8 = 13,
            HEX16 = 14,
            HEX24 = 15,
            HEX32 = 16,
            FLOAT64 = 17,
            BIN16 = 18,
            BIN24 = 19,
            BIN32 = 20,
            ERROR = 255
        }
        #endregion nums

        #region Private Data
        private StxItemType mo_type = StxItemType.INT8;
        // Data description
        private string m_data_descr = string.Empty;
        private byte m_num_bytes = 0;
        private bool m_type_error = false;
        private bool m_data_error = false;
        // Data items
        private byte m_byte = 0;
        private ushort m_word = 0;
        private uint m_dword = 0;
        private float m_float = 0.0f;
        private double m_dfloat = 0.0;
        private string m_string = string.Empty;
        #endregion

        #region Public Properties
        internal byte[] DataBytes
        {
            get
            {
                byte[] aby = Array.Empty<byte>();
                StringBuilder sb;

                switch (mo_type)
                {
                    case StxItemType.INT8:
                        aby = new byte[1];
                        aby[0] = m_byte;
                        break;
                    case StxItemType.FLOAT32:
                        aby = new byte[4];
                        HartDeviceDLL.BAHA_PutFloat(m_float, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 4);
                        break;
                    case StxItemType.BIN8:
                        aby = new byte[1];
                        aby[0] = m_byte;
                        break;
                    case StxItemType.UNIT8:
                        aby = new byte[1];
                        aby[0] = m_byte;
                        break;
                    case StxItemType.PASC6:
                        aby = new byte[6];
                        sb = new StringBuilder(8);
                        sb.Insert(0, m_string, 8);
                        HartDeviceDLL.BAHA_PutPackedASCII(sb, 8, 0, ref CDataSyntaxHelper.BytesArray[0]);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 6);
                        break;
                    case StxItemType.PASC12:
                        aby = new byte[12];
                        sb = new StringBuilder(16);
                        sb.Insert(0, m_string, 16);
                        HartDeviceDLL.BAHA_PutPackedASCII(sb, 16, 0, ref CDataSyntaxHelper.BytesArray[0]);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 12);
                        break;
                    case StxItemType.PASC24:
                        aby = new byte[24];
                        sb = new StringBuilder(32);
                        sb.Insert(0, m_string, 32);
                        HartDeviceDLL.BAHA_PutPackedASCII(sb, 32, 0, ref CDataSyntaxHelper.BytesArray[0]);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 24);
                        break;
                    case StxItemType.STRING8:
                        aby = new byte[8];
                        sb = new StringBuilder(8);
                        sb.Insert(0, m_string, 8);
                        HartDeviceDLL.BAHA_PutString(sb, 8, 0, ref CDataSyntaxHelper.BytesArray[0]);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 8);
                        break;
                    case StxItemType.STRING16:
                        aby = new byte[16];
                        sb = new StringBuilder(16);
                        sb.Insert(0, m_string, 16);
                        HartDeviceDLL.BAHA_PutString(sb, 16, 0, ref CDataSyntaxHelper.BytesArray[0]);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 16);
                        break;
                    case StxItemType.STRING32:
                        aby = new byte[32];
                        sb = new StringBuilder(32);
                        sb.Insert(0, m_string, 32);
                        HartDeviceDLL.BAHA_PutString(sb, 32, 0, ref CDataSyntaxHelper.BytesArray[0]);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 32);
                        break;
                    case StxItemType.INT16:
                        aby = new byte[2];
                        HartDeviceDLL.BAHA_PutWord(m_word, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 2);
                        break;
                    case StxItemType.INT24:
                        aby = new byte[3];
                        HartDeviceDLL.BAHA_PutInt24(m_dword, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 3);
                        break;
                    case StxItemType.INT32:
                        aby = new byte[4];
                        HartDeviceDLL.BAHA_PutDWord(m_dword, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 4);
                        break;
                    case StxItemType.HEX8:
                        aby = new byte[1];
                        aby[0] = m_byte;
                        break;
                    case StxItemType.HEX16:
                        aby = new byte[2];
                        HartDeviceDLL.BAHA_PutWord(m_word, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 2);
                        break;
                    case StxItemType.HEX24:
                        aby = new byte[3];
                        HartDeviceDLL.BAHA_PutInt24(m_dword, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 3);
                        break;
                    case StxItemType.HEX32:
                        aby = new byte[4];
                        HartDeviceDLL.BAHA_PutDWord(m_dword, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 4);
                        break;
                    case StxItemType.BIN16:
                        aby = new byte[2];
                        HartDeviceDLL.BAHA_PutWord(m_word, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 2);
                        break;
                    case StxItemType.BIN24:
                        aby = new byte[3];
                        HartDeviceDLL.BAHA_PutInt24(m_dword, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 3);
                        break;
                    case StxItemType.BIN32:
                        aby = new byte[4];
                        HartDeviceDLL.BAHA_PutDWord(m_dword, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 4);
                        break;
                    case StxItemType.FLOAT64:
                        aby = new byte[8];
                        HartDeviceDLL.BAHA_PutDouble(m_dfloat, 0, ref CDataSyntaxHelper.BytesArray[0], (byte)EN_Endian.MSB_First);
                        Array.Copy(CDataSyntaxHelper.BytesArray, aby, 8);
                        break;
                    case StxItemType.ERROR:
                        break;
                    default:
                        break;
                }
                return aby;
            }
        }

        internal string TypeText
        {
            get
            {
                string s = string.Empty;

                switch (mo_type)
                {
                    case StxItemType.INT8:
                        s = "Byte";
                        break;
                    case StxItemType.FLOAT32:
                        s = "Float32";
                        break;
                    case StxItemType.BIN8:
                        s = "Bin8";
                        break;
                    case StxItemType.UNIT8:
                        s = "Unit8";
                        break;
                    case StxItemType.PASC6:
                        s = "Pasc6";
                        break;
                    case StxItemType.PASC12:
                        s = "Pasc12";
                        break;
                    case StxItemType.PASC24:
                        s = "Pasc24";
                        break;
                    case StxItemType.STRING8:
                        s = "String8";
                        break;
                    case StxItemType.STRING16:
                        s = "String16";
                        break;
                    case StxItemType.STRING32:
                        s = "String32";
                        break;
                    case StxItemType.INT16:
                        s = "Word";
                        break;
                    case StxItemType.INT24:
                        s = "Int24";
                        break;
                    case StxItemType.INT32:
                        s = "DWord";
                        break;
                    case StxItemType.HEX8:
                        s = "Hex8";
                        break;
                    case StxItemType.HEX16:
                        s = "Hex16";
                        break;
                    case StxItemType.HEX24:
                        s = "Hex24";
                        break;
                    case StxItemType.HEX32:
                        s = "Hex32";
                        break;
                    case StxItemType.BIN16:
                        s = "Bin16";
                        break;
                    case StxItemType.BIN24:
                        s = "Bin24";
                        break;
                    case StxItemType.BIN32:
                        s = "Bin32";
                        break;
                    case StxItemType.FLOAT64:
                        s = "Float64";
                        break;
                    case StxItemType.ERROR:
                        s = "Error";
                        break;
                    default:
                        s = "Fatal";
                        break;
                }
                return s;
            }

            set
            {
                mo_type = CDataSyntaxHelper.GetTypeAndLength(value.ToUpper().Trim(), out m_num_bytes);
                if ((mo_type == StxItemType.ERROR) || (m_num_bytes == 0))
                {
                    m_type_error = true;
                }
                else
                {
                    m_type_error = false;
                }
            }
        }

        internal StxItemType Type
        {
            get
            {
                return mo_type;
            }

            set
            {
                mo_type = value;
            }
        }

        internal string ItemText
        {
            get
            {
                if (m_type_error || m_data_error)
                {
                    return string.Empty;
                }
                if (mo_type == StxItemType.INT8)
                {
                    // no prefix required
                    return m_data_descr;
                }
                else if (mo_type == StxItemType.HEX8)
                {
                    // no prefix required
                    return m_data_descr;
                }
                return TypeText + ";" + FormattedValue;
            }
        }

        internal string Value
        {
            get
            {
                return m_data_descr;
            }

            set
            {
                m_data_descr = value;
                #region Convert data
                switch (mo_type)
                {
                    case CDataSyntaxItem.StxItemType.BIN16:
                        m_data_error = !CDataSyntaxHelper.GetBin16(m_data_descr, out m_word);
                        break;
                    case CDataSyntaxItem.StxItemType.BIN24:
                        m_data_error = !CDataSyntaxHelper.GetBin24(m_data_descr, out m_dword);
                        break;
                    case CDataSyntaxItem.StxItemType.BIN32:
                        m_data_error = !CDataSyntaxHelper.GetBin32(m_data_descr, out m_dword);
                        break;
                    case CDataSyntaxItem.StxItemType.BIN8:
                        m_data_error = !CDataSyntaxHelper.GetBin8(m_data_descr, out m_byte);
                        break;
                    case CDataSyntaxItem.StxItemType.FLOAT32:
                        m_data_error = !CDataSyntaxHelper.GetFloat32(m_data_descr, out m_float);
                        break;
                    case CDataSyntaxItem.StxItemType.FLOAT64:
                        m_data_error = !CDataSyntaxHelper.GetFloat64(m_data_descr, out m_dfloat);
                        break;
                    case CDataSyntaxItem.StxItemType.HEX16:
                        m_data_error = !CDataSyntaxHelper.GetHex16(m_data_descr, out m_word);
                        break;
                    case CDataSyntaxItem.StxItemType.HEX24:
                        m_data_error = !CDataSyntaxHelper.GetHex24(m_data_descr, out m_dword);
                        break;
                    case StxItemType.HEX32:
                        m_data_error = !CDataSyntaxHelper.GetHex32(m_data_descr, out m_dword);
                        break;
                    case CDataSyntaxItem.StxItemType.HEX8:
                        bool v = !CDataSyntaxHelper.GetHex8(m_data_descr, out m_byte);
                        m_data_error = v;
                        break;
                    case CDataSyntaxItem.StxItemType.INT16:
                        m_data_error = !CDataSyntaxHelper.GetInt16(m_data_descr, out m_word);
                        break;
                    case CDataSyntaxItem.StxItemType.INT24:
                        m_data_error = !CDataSyntaxHelper.GetInt24(m_data_descr, out m_dword);
                        break;
                    case CDataSyntaxItem.StxItemType.INT32:
                        m_data_error = !CDataSyntaxHelper.GetInt32(m_data_descr, out m_dword);
                        break;
                    case CDataSyntaxItem.StxItemType.INT8:
                        m_data_error = !CDataSyntaxHelper.GetInt8(m_data_descr, out m_byte);
                        break;
                    case CDataSyntaxItem.StxItemType.PASC12:
                        m_data_error = !CDataSyntaxHelper.GetPasc12(m_data_descr, out m_string);
                        m_data_descr = m_string;
                        break;
                    case CDataSyntaxItem.StxItemType.PASC24:
                        m_data_error = !CDataSyntaxHelper.GetPasc24(m_data_descr, out m_string);
                        m_data_descr = m_string;
                        break;
                    case CDataSyntaxItem.StxItemType.PASC6:
                        m_data_error = !CDataSyntaxHelper.GetPasc6(m_data_descr, out m_string);
                        m_data_descr = m_string;
                        break;
                    case CDataSyntaxItem.StxItemType.STRING16:
                        m_data_error = !CDataSyntaxHelper.GetString16(m_data_descr, out m_string);
                        m_data_descr = m_string;
                        break;
                    case CDataSyntaxItem.StxItemType.STRING32:
                        m_data_error = !CDataSyntaxHelper.GetString32(m_data_descr, out m_string);
                        m_data_descr = m_string;
                        break;
                    case CDataSyntaxItem.StxItemType.STRING8:
                        m_data_error = !CDataSyntaxHelper.GetString8(m_data_descr, out m_string);
                        m_data_descr = m_string;
                        break;
                    case CDataSyntaxItem.StxItemType.UNIT8:
                        m_data_error = !CDataSyntaxHelper.GetUnit8(m_data_descr, out m_byte);
                        break;
                    default:
                        mo_type = CDataSyntaxItem.StxItemType.ERROR;
                        m_type_error = true;
                        break;
                }
                #endregion
            }
        }

        internal string FormattedValue
        {
            get
            {
                #region Format data
                switch (mo_type)
                {
                    case CDataSyntaxItem.StxItemType.BIN16:
                        m_data_descr = CDataSyntaxHelper.FormatBin16(m_word);
                        break;
                    case CDataSyntaxItem.StxItemType.BIN24:
                        m_data_descr = CDataSyntaxHelper.FormatBin24(m_dword);
                        break;
                    case CDataSyntaxItem.StxItemType.BIN32:
                        m_data_descr = CDataSyntaxHelper.FormatBin32(m_dword);
                        break;
                    case CDataSyntaxItem.StxItemType.BIN8:
                        m_data_descr = CDataSyntaxHelper.FormatBin8(m_byte);
                        break;
                    case CDataSyntaxItem.StxItemType.FLOAT32:
                        {
                            CDataSyntaxHelper.GetFloat32(m_data_descr, out float f);
                            if (f != m_float)
                            {
                                m_data_descr = m_float.ToString();
                            }
                            m_data_descr = m_data_descr.Replace(',', '.');
                        }
                        break;
                    case CDataSyntaxItem.StxItemType.FLOAT64:
                        {
                            CDataSyntaxHelper.GetFloat64(m_data_descr, out double d);
                            if (d != m_dfloat)
                            {
                                m_data_descr = m_dfloat.ToString();
                            }
                            m_data_descr = m_data_descr.Replace(',', '.');
                        }
                        break;
                    case CDataSyntaxItem.StxItemType.HEX16:
                        m_data_descr = "0x" + m_word.ToString("x4");
                        break;
                    case CDataSyntaxItem.StxItemType.HEX24:
                        m_data_descr = "0x" + m_dword.ToString("x6");
                        break;
                    case CDataSyntaxItem.StxItemType.HEX32:
                        m_data_descr = "0x" + m_dword.ToString("x8");
                        break;
                    case CDataSyntaxItem.StxItemType.HEX8:
                        m_data_descr = "0x" + m_byte.ToString("x2");
                        break;
                    case CDataSyntaxItem.StxItemType.INT16:
                        break;
                    case CDataSyntaxItem.StxItemType.INT24:
                        break;
                    case CDataSyntaxItem.StxItemType.INT32:
                        break;
                    case CDataSyntaxItem.StxItemType.INT8:
                        break;
                    case CDataSyntaxItem.StxItemType.PASC12:
                        if (m_data_descr.Length > 16)
                        {
                            m_data_descr = m_data_descr[..16];
                        }
                        else if (m_data_descr.Length < 16)
                        {
                            m_data_descr = m_data_descr.PadRight(16, ' ');
                        }
                        break;
                    case CDataSyntaxItem.StxItemType.PASC24:
                        if (m_data_descr.Length > 32)
                        {
                            m_data_descr = m_data_descr.Substring(0, 32);
                        }
                        else if (m_data_descr.Length < 32)
                        {
                            m_data_descr = m_data_descr.PadRight(32, ' ');
                        }
                        break;
                    case CDataSyntaxItem.StxItemType.PASC6:
                        if (m_data_descr.Length > 8)
                        {
                            m_data_descr = m_data_descr.Substring(0, 8);
                        }
                        else if (m_data_descr.Length < 8)
                        {
                            m_data_descr = m_data_descr.PadRight(8, ' ');
                        }
                        break;
                    case CDataSyntaxItem.StxItemType.STRING16:
                        if (m_data_descr.Length > 16)
                        {
                            m_data_descr = m_data_descr.Substring(0, 16);
                        }
                        else if (m_data_descr.Length < 16)
                        {
                            m_data_descr = m_data_descr.PadRight(16, ' ');
                        }
                        break;
                    case CDataSyntaxItem.StxItemType.STRING32:
                        if (m_data_descr.Length > 32)
                        {
                            m_data_descr = m_data_descr.Substring(0, 32);
                        }
                        else if (m_data_descr.Length < 32)
                        {
                            m_data_descr = m_data_descr.PadRight(32, ' ');
                        }
                        break;
                    case CDataSyntaxItem.StxItemType.STRING8:
                        if (m_data_descr.Length > 8)
                        {
                            m_data_descr = m_data_descr.Substring(0, 8);
                        }
                        else if (m_data_descr.Length < 8)
                        {
                            m_data_descr = m_data_descr.PadRight(8, ' ');
                        }
                        break;
                    case CDataSyntaxItem.StxItemType.UNIT8:
                        break;
                    default:
                        mo_type = CDataSyntaxItem.StxItemType.ERROR;
                        m_type_error = true;
                        break;
                }
                #endregion
                return m_data_descr;
            }
        }

        internal bool Valid
        {
            get
            {
                if (m_type_error || m_data_error)
                {
                    return false;
                }
                return true;
            }
        }

        internal bool TypeError
        {
            get
            {
                return m_type_error;
            }

            set
            {
                m_type_error = value;
            }
        }

        internal bool DataError
        {
            get
            {
                return m_data_error;
            }

            set
            {
                m_data_error = value;
            }
        }
        #endregion
    }

    internal class CDataSytax
    {
        private string ms_DataSyntax = string.Empty;
        private List<CDataSyntaxItem> mlst_DatStxItems = new List<CDataSyntaxItem>();

        #region Internal Properties
        internal string DatStxString
        {
            get
            {
                if (mlst_DatStxItems.Count < 1)
                {
                    return string.Empty;
                }
                ms_DataSyntax = string.Empty;
                foreach (CDataSyntaxItem cItem in mlst_DatStxItems)
                {
                    if (cItem.Valid)
                    {
                        if (ms_DataSyntax == string.Empty)
                        {
                            ms_DataSyntax = cItem.ItemText;
                        }
                        else
                        {
                            ms_DataSyntax += ";" + cItem.ItemText;
                        }
                    }
                    else
                    {
                        if (ms_DataSyntax == string.Empty)
                        {
                            ms_DataSyntax = "Error";
                        }
                        else
                        {
                            ms_DataSyntax += ";" + "Error";
                        }
                    }
                }
                return ms_DataSyntax;
            }

            set
            {
            }
        }
        #endregion Internal Properties

        #region Public Methods
        internal void Clear()
        {
            mlst_DatStxItems = new List<CDataSyntaxItem>();
        }

        internal void Add(string type_, string data_)
        {
            CDataSyntaxItem cDtStx = new CDataSyntaxItem();

            cDtStx.TypeText = type_;
            cDtStx.Value = data_;

            mlst_DatStxItems.Add(cDtStx);
        }

        internal bool InitFromString(string dat_stx_)
        {
            if (dat_stx_ == string.Empty)
            {
                return true;
            }
            return CDataSyntaxHelper.ParseDataSyntax(dat_stx_, out mlst_DatStxItems);
        }

        internal static byte[] ToByteArray()
        {
            byte[] aby = Array.Empty<byte>();
            return aby;
        }

        internal List<CDataSyntaxItem> GetDatStxList()
        {
            return mlst_DatStxItems;
        }

        #endregion
    }

    internal static class CDataSyntaxHelper
    {
        #region Internal Data
        // Buffers for conversions
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        internal static byte[] BytesArray = new byte[256];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        internal static byte[] BytesArrayBuffer = new byte[256];
        #endregion

        #region Private Data
        //private static CDataSyntaxItem.StxItemType enType;
        //private static byte byTypeLen;
        #endregion Private Data

        #region Public Methods
        internal static bool ParseDataSyntax(string sDataSyntax, out List<CDataSyntaxItem> lstDatStxItems)
        {
            string[] astr = sDataSyntax.Trim().Split(';');
            char[] ach = Array.Empty<char>();
            ushort usIdx = 0;
            bool bDone = false;
            bool bError = false;
            CDataSyntaxItem.StxItemType en_Type = CDataSyntaxItem.StxItemType.ERROR;
            byte DataLen = 0;

            lstDatStxItems = new List<CDataSyntaxItem>();

            if (astr.Length > 0)
            {
                CDataSyntaxItem clDataSyntaxItem;
                while (usIdx < astr.Length)
                {
                    clDataSyntaxItem = new CDataSyntaxItem();
                    astr[usIdx] = astr[usIdx].Trim();
                    ach = astr[usIdx].ToCharArray();
                    bDone = false;

                    #region Handle empty element
                    if (astr[usIdx].Length == 0)
                    {
                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.INT8;
                        clDataSyntaxItem.Value = "0";
                        bDone = true;
                    }
                    #endregion

                    #region Handle pure hex elements
                    if (!bDone)
                    {
                        if (astr[usIdx].Length > 1)
                        {
                            if ((ach[0] == '0') && ((ach[1] == 'X') || (ach[1] == 'x')))
                            {
                                uint Value;
                                string s;

                                Remove0x(astr[usIdx], out s);
                                if (GetHex32(s, out Value))
                                {
                                    if (Value < 256)
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.HEX8;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    else if (Value < 65536)
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.HEX16;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    else if (Value < 16777216)
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.HEX24;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    else
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.HEX32;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                }
                                else
                                {
                                    clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.HEX8;
                                    clDataSyntaxItem.Value = "0x0";
                                    clDataSyntaxItem.DataError = true;
                                }
                                bDone = true;
                            }
                        }
                    }
                    #endregion

                    #region Handle pure binary elements
                    if (!bDone)
                    {
                        if (astr[usIdx].Length > 1)
                        {
                            if ((ach[0] == '0') && ((ach[1] == 'B') || (ach[1] == 'b')))
                            {
                                uint Value;
                                string s;

                                Remove0b(astr[usIdx], out s);
                                if (GetBin32(s, out Value))
                                {
                                    if (Value < 256)
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.BIN8;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    else if (Value < 65536)
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.BIN16;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    else if (Value < 16777216)
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.BIN24;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    else
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.BIN32;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                }
                                else
                                {
                                    clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.BIN8;
                                    clDataSyntaxItem.Value = "0b0";
                                    clDataSyntaxItem.DataError = true;
                                }
                                bDone = true;
                            }
                        }
                    }
                    #endregion

                    #region Handle elements starting with decimal character
                    if (!bDone)
                    {
                        if (astr[usIdx].Length > 0)
                        {
                            if (IsDecimalDigit(ach[0]))
                            {
                                uint uiValue;
                                float fValue;
                                double dValue;

                                // try integer
                                if (GetInt32(astr[usIdx], out uiValue))
                                {
                                    if (uiValue < 256)
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.INT8;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    else if (uiValue < 65536)
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.INT16;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    else if (uiValue < 16777216)
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.INT24;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    else
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.INT32;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                    }
                                    bDone = true;
                                }
                                // try float
                                if (!bDone)
                                {
                                    if (GetFloat32(astr[usIdx], out fValue))
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.FLOAT32;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                        bDone = true;
                                    }
                                }
                                // try double
                                if (!bDone)
                                {
                                    if (GetFloat64(astr[usIdx], out dValue))
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.FLOAT64;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                        bDone = true;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Handle elements starting with sign character
                    if (!bDone)
                    {
                        if (astr[usIdx].Length > 0)
                        {
                            if ((ach[0] == '+') || (ach[0] == '-'))
                            {
                                float fValue;
                                double dValue;

                                if (GetFloat32(astr[usIdx], out fValue))
                                {
                                    clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.FLOAT32;
                                    clDataSyntaxItem.Value = astr[usIdx];
                                    bDone = true;
                                }
                                // try double
                                if (!bDone)
                                {
                                    if (GetFloat64(astr[usIdx], out dValue))
                                    {
                                        clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.FLOAT64;
                                        clDataSyntaxItem.Value = astr[usIdx];
                                        bDone = true;
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region Handle item with invalid identifier
                    if (!bDone)
                    {
                        en_Type = GetTypeAndLength(astr[usIdx], out DataLen);
                        if (en_Type == CDataSyntaxItem.StxItemType.ERROR)
                        {
                            string s = astr[usIdx];

                            if (s.Length > 32)
                            {
                                s = s.Substring(0, 32);
                            }
                            clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.ERROR;
                            clDataSyntaxItem.Value = s;
                            clDataSyntaxItem.TypeError = true;
                            bError = true;
                            bDone = true;
                        }
                    }
                    #endregion

                    #region Handle item with not existing data descriptor
                    if (!bDone)
                    {
                        if (usIdx >= (astr.Length - 1))
                        {
                            clDataSyntaxItem.Type = CDataSyntaxItem.StxItemType.ERROR;
                            clDataSyntaxItem.Value = "0";
                            clDataSyntaxItem.DataError = true;
                            bError = true;
                            bDone = true;
                        }
                    }
                    #endregion

                    #region Handle item with data descriptor
                    if (!bDone)
                    {
                        usIdx++;
                        clDataSyntaxItem.Type = en_Type;
                        clDataSyntaxItem.Value = astr[usIdx];
                        if (!clDataSyntaxItem.Valid)
                        {
                            bError = true;
                            bDone = true;
                        }
                    }
                    #endregion

                    lstDatStxItems.Add(clDataSyntaxItem);

                    if (usIdx < (astr.Length))
                    {
                        usIdx++;
                    }
                }
            }
            if (bError)
            {
                return false;
            }
            return true;
        }

        internal static byte PutToBuffer(string sData, ref byte bytesOfData)
        {
            CDataSytax cDtStx = new CDataSytax();
            List<CDataSyntaxItem> lstItems = new List<CDataSyntaxItem>();
            byte byCount = 0;

            cDtStx.InitFromString(sData);
            lstItems = cDtStx.GetDatStxList();
            if (lstItems.Count == 0)
            {
                return 0;
            }

            foreach (CDataSyntaxItem cItem in lstItems)
            {
                byte[] aby = cItem.DataBytes;
                Array.Copy(aby, 0, CDataSyntaxHelper.BytesArrayBuffer, byCount, aby.Length);
                byCount += Convert.ToByte(aby.Length);
            }

            HartDeviceDLL.BAHA_PutOctets(ref CDataSyntaxHelper.BytesArrayBuffer[0], byCount, 0, ref bytesOfData);

            return byCount;
        }
        #endregion Public Methods

        #region Syntax Checker
        internal static CDataSyntaxItem.StxItemType GetTypeAndLength(string item, out byte byLen)
        {
            string s = item.ToUpper();

            if (s.Length < 4)
            {
                byLen = 0;
                return CDataSyntaxItem.StxItemType.ERROR;
            }
            if ((s == "DEC8") || (s == "INT8"))
            {
                byLen = 1;
                return CDataSyntaxItem.StxItemType.INT8;
            }
            else if ((s == "DEC16") || (s == "INT16"))
            {
                byLen = 2;
                return CDataSyntaxItem.StxItemType.INT16;
            }
            else if ((s == "DEC24") || (s == "INT24"))
            {
                byLen = 3;
                return CDataSyntaxItem.StxItemType.INT24;
            }
            else if ((s == "DEC32") || (s == "INT32"))
            {
                byLen = 4;
                return CDataSyntaxItem.StxItemType.INT32;
            }
            else if (s == "BIN8")
            {
                byLen = 1;
                return CDataSyntaxItem.StxItemType.BIN8;
            }
            else if (s == "BIN16")
            {
                byLen = 2;
                return CDataSyntaxItem.StxItemType.BIN16;
            }
            else if (s == "BIN24")
            {
                byLen = 3;
                return CDataSyntaxItem.StxItemType.BIN24;
            }
            else if (s == "BIN32")
            {
                byLen = 4;
                return CDataSyntaxItem.StxItemType.BIN32;
            }
            else if (s == "HEX8")
            {
                byLen = 1;
                return CDataSyntaxItem.StxItemType.HEX8;
            }
            else if (s == "HEX16")
            {
                byLen = 2;
                return CDataSyntaxItem.StxItemType.HEX16;
            }
            else if (s == "HEX24")
            {
                byLen = 3;
                return CDataSyntaxItem.StxItemType.HEX24;
            }
            else if (s == "HEX32")
            {
                byLen = 4;
                return CDataSyntaxItem.StxItemType.HEX32;
            }
            else if ((s == "FLOAT") || (s == "FLOAT32"))
            {
                byLen = 4;
                return CDataSyntaxItem.StxItemType.FLOAT32;
            }
            else if ((s == "DOUBLE") || (s == "FLOAT64"))
            {
                byLen = 8;
                return CDataSyntaxItem.StxItemType.FLOAT64;
            }
            else if ((s == "PCA6") || (s == "PASC6"))
            {
                byLen = 6;
                return CDataSyntaxItem.StxItemType.PASC6;
            }
            else if ((s == "PCA12") || (s == "PASC12"))
            {
                byLen = 12;
                return CDataSyntaxItem.StxItemType.PASC12;
            }
            else if ((s == "PCA24") || (s == "PASC24"))
            {
                byLen = 24;
                return CDataSyntaxItem.StxItemType.PASC24;
            }
            else if ((s == "STR8") || (s == "STRING8"))
            {
                byLen = 8;
                return CDataSyntaxItem.StxItemType.STRING8;
            }
            else if ((s == "STR16") || (s == "STRING16"))
            {
                byLen = 16;
                return CDataSyntaxItem.StxItemType.STRING16;
            }
            else if ((s == "STR32") || (s == "STRING32"))
            {
                byLen = 32;
                return CDataSyntaxItem.StxItemType.STRING32;
            }
            byLen = 0;
            return CDataSyntaxItem.StxItemType.ERROR;
        }
        #endregion

        #region String Conversions
        internal static bool GetInt8(string s_, out byte out_)
        {
            s_ = s_.Trim().ToUpper();
            if (Remove0x(s_, out s_))
            {
                return GetHex8(s_, out out_);
            }

            if (byte.TryParse(s_, System.Globalization.NumberStyles.Integer, null, out out_))
            {
                return true;
            }

            return false;
        }

        internal static bool GetFloat32(string s_, out float out_)
        {
            out_ = 0.0f;
            s_ = s_.Trim().ToUpper();
            if (CHelpers.CFormat.GetFloatFromString(s_, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetUnit8(string s_, out byte out_)
        {
            out_ = 0;
            s_ = s_.Trim().ToUpper();
            if (Remove0x(s_, out s_))
            {
                return GetHex8(s_, out out_);
            }
            if (byte.TryParse(s_, System.Globalization.NumberStyles.Integer, null, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetPasc6(string s_, out string out_)
        {
            out_ = string.Empty;
            s_ = s_.ToUpper();
            if (s_.Length < 8)
            {
                out_ = s_.PadRight(8, ' ');
                return true;
            }
            else if (s_.Length > 8)
            {
                out_ = s_.Substring(0, 8);
                return false;
            }
            out_ = s_;
            return true;
        }

        internal static bool GetPasc12(string s_, out string out_)
        {
            out_ = string.Empty;
            s_ = s_.ToUpper();
            if (s_.Length < 16)
            {
                out_ = s_.PadRight(16, ' ');
                return true;
            }
            else if (s_.Length > 16)
            {
                out_ = s_.Substring(0, 16);
                return false;
            }
            out_ = s_;
            return true;
        }

        internal static bool GetPasc24(string s_, out string out_)
        {
            out_ = string.Empty;
            s_ = s_.ToUpper();
            if (s_.Length < 32)
            {
                out_ = s_.PadRight(32, ' ');
                return true;
            }
            else if (s_.Length > 32)
            {
                out_ = s_.Substring(0, 32);
                return false;
            }
            out_ = s_;
            return true;
        }

        internal static bool GetString8(string s_, out string out_)
        {
            out_ = string.Empty;
            //s_ = s_.ToUpper();
            if (s_.Length < 8)
            {
                out_ = s_.PadRight(8, (char)(0));
                return true;
            }
            else if (s_.Length > 8)
            {
                out_ = s_.Substring(0, 8);
                return false;
            }
            out_ = s_;
            return true;
        }

        internal static bool GetString16(string s_, out string out_)
        {
            out_ = string.Empty;
            //s_ = s_.ToUpper();
            if (s_.Length < 16)
            {
                out_ = s_.PadRight(16, (char)(0));
                return true;
            }
            else if (s_.Length > 16)
            {
                out_ = s_.Substring(0, 16);
                return false;
            }
            out_ = s_;
            return true;
        }

        internal static bool GetString32(string s_, out string out_)
        {
            out_ = string.Empty;
            //s_ = s_.ToUpper();
            if (s_.Length < 32)
            {
                out_ = s_.PadRight(32, (char)(0));
                return true;
            }
            else if (s_.Length > 32)
            {
                out_ = s_.Substring(0, 32);
                return false;
            }
            out_ = s_;
            return true;
        }

        internal static bool GetInt16(string s_, out ushort out_)
        {
            out_ = 0;
            s_ = s_.Trim().ToUpper();
            if (Remove0x(s_, out s_))
            {
                return GetHex16(s_, out out_);
            }
            if (ushort.TryParse(s_, System.Globalization.NumberStyles.Integer, null, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetInt24(string s_, out uint out_)
        {
            out_ = 0;
            s_ = s_.Trim().ToUpper();
            if (Remove0x(s_, out s_))
            {
                return GetHex24(s_, out out_);
            }
            if (uint.TryParse(s_, System.Globalization.NumberStyles.Integer, null, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetInt32(string s_, out uint out_)
        {
            out_ = 0;
            s_ = s_.Trim().ToUpper();
            if (Remove0x(s_, out s_))
            {
                return GetHex32(s_, out out_);
            }
            if (out_ > 16777215)
            {
                out_ = 0;
                return false;
            }
            if (uint.TryParse(s_, System.Globalization.NumberStyles.Integer, null, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetHex8(string s_, out byte out_)
        {
            out_ = 0;
            s_ = s_.Trim().ToUpper();

            Remove0x(s_, out s_);
            if (byte.TryParse(s_, System.Globalization.NumberStyles.HexNumber, null, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetHex16(string s_, out ushort out_)
        {
            out_ = 0;
            s_ = s_.Trim().ToUpper();

            Remove0x(s_, out s_);
            if (ushort.TryParse(s_, System.Globalization.NumberStyles.HexNumber, null, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetHex24(string s_, out uint out_)
        {
            out_ = 0;
            s_ = s_.Trim().ToUpper();

            Remove0x(s_, out s_);
            if (s_.Length > 6)
            {
                out_ = 0;
                return false;
            }
            if (uint.TryParse(s_, System.Globalization.NumberStyles.HexNumber, null, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetHex32(string s_, out uint out_)
        {
            out_ = 0;
            s_ = s_.Trim().ToUpper();

            Remove0x(s_, out s_);
            if (uint.TryParse(s_, System.Globalization.NumberStyles.HexNumber, null, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetFloat64(string s_, out double out_)
        {
            out_ = 0.0;
            s_ = s_.Trim().ToUpper();
            if (CHelpers.CFormat.GetDoubleFromString(s_, out out_))
            {
                return true;
            }
            return false;
        }

        internal static bool GetBin8(string s_, out byte out_)
        {
            out_ = 0;
            byte byMask = 0x80;

            s_ = s_.Trim().ToUpper();

            Remove0b(s_, out s_);
            if (s_.Length < 8)
            {
                s_ = s_.PadLeft(8, '0');
            }
            else if (s_.Length > 8)
            {
                out_ = 0;
                return false;
            }

            char[] ach = s_.ToCharArray();

            for (int i = 0; i < 8; i++)
            {
                if (ach[i] == '1')
                {
                    out_ += byMask;
                }
                else if (ach[i] == '0')
                {
                }
                else
                {
                    out_ = 0;
                    return false;
                }
                byMask >>= 1;
            }
            return true;
        }

        internal static bool GetBin16(string s_, out ushort out_)
        {
            out_ = 0;
            ushort usMask = 0x8000;

            s_ = s_.Trim().ToUpper();

            Remove0b(s_, out s_);
            if (s_.Length < 16)
            {
                s_ = s_.PadLeft(16, '0');
            }
            else if (s_.Length > 16)
            {
                out_ = 0;
                return false;
            }

            char[] ach = s_.ToCharArray();

            for (int i = 0; i < 16; i++)
            {
                if (ach[i] == '1')
                {
                    out_ += usMask;
                }
                else if (ach[i] == '0')
                {
                }
                else
                {
                    out_ = 0;
                    return false;
                }
                usMask >>= 1;
            }
            return true;
        }

        internal static bool GetBin24(string s_, out uint out_)
        {
            out_ = 0;
            uint uiMask = 0x800000;

            s_ = s_.Trim().ToUpper();

            Remove0b(s_, out s_);
            if (s_.Length < 24)
            {
                s_ = s_.PadLeft(24, '0');
            }
            else if (s_.Length > 24)
            {
                out_ = 0;
                return false;
            }

            char[] ach = s_.ToCharArray();

            for (int i = 0; i < 24; i++)
            {
                if (ach[i] == '1')
                {
                    out_ += uiMask;
                }
                else if (ach[i] == '0')
                {
                }
                else
                {
                    out_ = 0;
                    return false;
                }
                uiMask >>= 1;
            }
            return true;
        }

        internal static bool GetBin32(string s_, out uint out_)
        {
            out_ = 0;
            uint uiMask = 0x80000000;

            s_ = s_.Trim().ToUpper();

            Remove0b(s_, out s_);
            if (s_.Length < 32)
            {
                s_ = s_.PadLeft(32, '0');
            }
            else if (s_.Length > 32)
            {
                out_ = 0;
                return false;
            }

            char[] ach = s_.ToCharArray();

            for (int i = 0; i < 32; i++)
            {
                if (ach[i] == '1')
                {
                    out_ += uiMask;
                }
                else if (ach[i] == '0')
                {
                }
                else
                {
                    out_ = 0;
                    return false;
                }
                uiMask >>= 1;
            }
            return true;
        }
        #endregion

        #region Format data
        internal static string FormatBin16(ushort value_)
        {
            ushort usMask = 0x8000;
            char[] ach = new char[16];

            for (int i = 0; i < 16; i++)
            {
                if ((value_ & usMask) != 0)
                {
                    ach[i] = '1';
                }
                else
                {
                    ach[i] = '0';
                }
                usMask >>= 1;
            }
            return "0b" + new string(ach);
        }

        internal static string FormatBin32(uint value_)
        {
            uint uiMask = 0x80000000;
            char[] ach = new char[32];

            for (int i = 0; i < 32; i++)
            {
                if ((value_ & uiMask) != 0)
                {
                    ach[i] = '1';
                }
                else
                {
                    ach[i] = '0';
                }
                uiMask >>= 1;
            }
            return "0b" + new string(ach);
        }

        internal static string FormatBin24(uint value_)
        {
            uint uiMask = 0x800000;
            char[] ach = new char[24];

            for (int i = 0; i < 24; i++)
            {
                if ((value_ & uiMask) != 0)
                {
                    ach[i] = '1';
                }
                else
                {
                    ach[i] = '0';
                }
                uiMask >>= 1;
            }
            return "0b" + new string(ach);
        }

        internal static string FormatBin8(byte value_)
        {
            byte byMask = 0x80;
            char[] ach = new char[8];

            for (int i = 0; i < 8; i++)
            {
                if ((value_ & byMask) != 0)
                {
                    ach[i] = '1';
                }
                else
                {
                    ach[i] = '0';
                }
                byMask >>= 1;
            }
            return "0b" + new string(ach);
        }
        #endregion

        #region Private Helpers
        private static bool Remove0x(string s_, out string out_)
        {
            out_ = s_;
            if (s_.Length > 1)
            {
                if (s_.Substring(0, 2) == "0X")
                {
                    if (s_.Length > 2)
                    {
                        out_ = s_.Substring(2, s_.Length - 2);
                    }
                    else
                    {
                        out_ = string.Empty;
                    }
                    return true;
                }
            }
            return false;
        }

        private static void Remove0b(string s_, out string out_)
        {
            out_ = s_;
            if (s_.Length > 1)
            {
                if (s_.Substring(0, 2) == "0B")
                {
                    if (s_.Length > 2)
                    {
                        out_ = s_.Substring(2, s_.Length - 2);
                    }
                    else
                    {
                        out_ = string.Empty;
                    }
                }
            }
        }

        internal static bool IsDecimalDigit(char c_)
        {
            if ((c_ >= '0') && (c_ <= '9'))
            {
                return true;
            }
            return false;
        }

        internal static uint GetHexDigitValue(char c_)
        {
            switch (c_)
            {
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                case 'A':
                    return 10;
                case 'B':
                    return 11;
                case 'C':
                    return 12;
                case 'D':
                    return 13;
                case 'E':
                    return 14;
                case 'F':
                    return 15;
                default:
                    return 0;
            }
        }

        internal static string MakeFixLength(string s_, uint len_)
        {
            int e;
            int iSpacesNeeded;

            iSpacesNeeded = (int)(len_ - s_.Length);
            if (iSpacesNeeded > 0)
            {
                for (e = 0; e < iSpacesNeeded; e++)
                {
                    s_ += "_";
                }
            }
            return s_;
        }
        #endregion Private Helpers
    }
}
