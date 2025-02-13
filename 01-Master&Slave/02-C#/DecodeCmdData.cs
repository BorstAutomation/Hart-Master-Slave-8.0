/*
 *          File: DecodeCmdData.cs (DecodeCmdData)
 *                The module provides functions to decode some commands. 
 *                The input is a response as a byte array and the output 
 *                consists of a string.
 *                The following commands are decoded:
 *                0, 1, 2, 3, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 17, 18, 20, 
 *                21, 22, 33, 34, 35.38, 48, 78 and 109.
 *                Here is an example for command 0:
 *                 In: FE 11 2B 05 07 01 01 08 00 9C 9F 4D 05 0B 00 94 00 00 11 00 11 01|65|
 *                Out: 254/Man17/Dev43/5 PAs/Hart7/Tx1/Sw1/Hw8/FL00000000/
 *                     ID 0x9C 0x9F 0x4D/MinPArsp:5/MaxNumDVs:11/CfgChCnt:148/
 *                     ExtDevStat:00000000/ManuID:0x0011/LabDistID:0011/Profile:1|65|
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
using System.Text;
#endregion Namespaces

namespace BaTestHart
{
    internal static class DecodeCmdData
    {
        internal static bool WasCmdDataDecoded(out string decoded_data_, byte cmd_, byte[] bytes_of_data_, bool is_req_)
        {
            switch (cmd_)
            {
                case 0:
                    decoded_data_ = DecodeCmd0(bytes_of_data_);
                    return true;
                case 1:
                    decoded_data_ = DecodeCmd1(bytes_of_data_);
                    return true;
                case 2:
                    decoded_data_ = DecodeCmd2(bytes_of_data_);
                    return true;
                case 3:
                    decoded_data_ = DecodeCmd3(bytes_of_data_);
                    return true;
                case 6:
                    decoded_data_ = DecodeCmd6(bytes_of_data_);
                    return true;
                case 7:
                    decoded_data_ = DecodeCmd7(bytes_of_data_);
                    return true;
                case 8:
                    decoded_data_ = DecodeCmd8(bytes_of_data_);
                    return true;
                case 09:
                    decoded_data_ = DecodeCmd09(bytes_of_data_, is_req_);
                    return true;
                case 11:
                    decoded_data_ = DecodeCmd11(bytes_of_data_, is_req_);
                    return true;
                case 12:
                    decoded_data_ = DecodeCmd12(bytes_of_data_);
                    return true;
                case 13:
                    decoded_data_ = DecodeCmd13(bytes_of_data_);
                    return true;
                case 14:
                    decoded_data_ = DecodeCmd14(bytes_of_data_);
                    return true;
                case 15:
                    decoded_data_ = DecodeCmd15(bytes_of_data_);
                    return true;
                case 16:
                    decoded_data_ = DecodeCmd16(bytes_of_data_);
                    return true;
                case 17:
                    decoded_data_ = DecodeCmd17(bytes_of_data_);
                    return true;
                case 18:
                    decoded_data_ = DecodeCmd18(bytes_of_data_);
                    return true;
                case 20:
                    decoded_data_ = DecodeCmd20(bytes_of_data_);
                    return true;
                case 21:
                    decoded_data_ = DecodeCmd21(bytes_of_data_, is_req_);
                    return true;
                case 22:
                    decoded_data_ = DecodeCmd20(bytes_of_data_);
                    return true;
                case 33:
                    decoded_data_ = DecodeCmd33(bytes_of_data_);
                    return true;
                case 34:
                    decoded_data_ = DecodeCmd34(bytes_of_data_);
                    return true;
                case 35:
                    decoded_data_ = DecodeCmd35(bytes_of_data_);
                    return true;
                case 38:
                    decoded_data_ = DecodeCmd38(bytes_of_data_);
                    return true;
                case 48:
                    decoded_data_ = DecodeCmd48(bytes_of_data_);
                    return true;
                case 54:
                    decoded_data_ = DecodeCmd54(bytes_of_data_);
                    return true;
                case 78:
                    decoded_data_ = DecodeCmd78(bytes_of_data_, is_req_);
                    return true;
                case 109:
                    decoded_data_ = DecodeCmd109(bytes_of_data_);
                    return true;
            }
            decoded_data_ = string.Empty;
            return false;
        }

        #region Universal Commands
        private static string DecodeCmd0(byte[] bytes_of_data_)
        {
            string sResult;

            sResult = GetConnectionInfo(bytes_of_data_);

            return sResult;
        }

        private static string DecodeCmd1(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 5)
            {
                s = "Length Err";
            }
            else
            {
                s = "PV 1:" + decodeFloat(bytes_of_data_, 1) + " " + GetHartUnit(bytes_of_data_[0]);
            }
            return s;
        }

        private static string DecodeCmd2(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 8)
            {
                s = "Length Err";
            }
            else
            {
                s = decodeFloat(bytes_of_data_, 0) + " mA";
                s += "/" + decodeFloat(bytes_of_data_, 4) + " %";
            }
            return s;
        }

        private static string DecodeCmd3(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 9)
            {
                s = "Length Err";
            }
            else
            {
                s = "Curr:" + decodeFloat(bytes_of_data_, 0) + " mA";
                s += "\n PV 1: " + decodeFloat(bytes_of_data_, 5) + " " + GetHartUnit(bytes_of_data_[4]);
            }
            if (bytes_of_data_.Length > 13)
            {
                s += " / PV 2: " + decodeFloat(bytes_of_data_, 10) + " " + GetHartUnit(bytes_of_data_[9]);
            }
            if (bytes_of_data_.Length > 18)
            {
                s += "\n PV 3: " + decodeFloat(bytes_of_data_, 15) + " " + GetHartUnit(bytes_of_data_[14]);
            }
            if (bytes_of_data_.Length > 23)
            {
                s += " / PV 4: " + decodeFloat(bytes_of_data_, 20) + " " + GetHartUnit(bytes_of_data_[19]);
            }

            return s;
        }

        private static string DecodeCmd6(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 1)
            {
                s = "Length Err";
            }
            else
            {
                s = "Polling Address: " + bytes_of_data_[0].ToString("0");
                if (bytes_of_data_.Length >= 2)
                {
                    s += " / ";
                    switch (bytes_of_data_[1])
                    {
                        case 0:
                            s += "Loop Current: Disabled";
                            break;
                        default:
                            s += "Loop Current: Enabled";
                            break;
                    }
                }
            }
            return s;
        }

        private static string DecodeCmd7(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 2)
            {
                s = "Length Err";
            }
            else
            {
                s = "Polling Address: " + bytes_of_data_[0].ToString("0");
                s += " / ";
                switch (bytes_of_data_[1])
                {
                    case 0:
                        s += "Loop Current: Disabled";
                        break;
                    default:
                        s += "Loop Current: Enabled";
                        break;
                }
            }
            return s;
        }

        private static string DecodeCmd8(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 4)
            {
                s = "Length Err";
            }
            else
            {
                s = "/Class DV1:" + bytes_of_data_[0].ToString("0");
                s += "/Class DV2:" + bytes_of_data_[1].ToString("0");
                s += "/Class DV3:" + bytes_of_data_[2].ToString("0");
                s += "/Class DV4:" + bytes_of_data_[3].ToString("0");
            }
            return s;
        }

        private static string DecodeCmd20(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 32)
            {
                s = "Length Err";
            }
            else
            {
                s = "Long Tag:" + decodeString(bytes_of_data_, 0, 32);
            }
            return s;
        }

        private static string DecodeCmd09(byte[] bytes_of_data_, bool isReq)
        {
            int Len = bytes_of_data_.Length;
            string sResult = string.Empty;
            int remaining = Len - 9;
            byte idx = 0;

            if (isReq)
            {
                if (Len > 0)
                {
                    sResult += "DV: " + bytes_of_data_[0].ToString("0");
                    if (Len > 1)
                    {
                        for (int e = 1; e < Len; e++)
                        {
                            sResult += "," + bytes_of_data_[e].ToString("0");
                        }
                    }
                }

                return sResult;
            }

            if (Len < 9)
            {
                return "Data Length Error";
            }

            sResult = "Extended Device Status: " + CFrameHelper.GetBin(bytes_of_data_[0]);
            sResult += "\n DV" + bytes_of_data_[1].ToString("0").PadLeft(4, ' ');
            sResult += " / Class:" + bytes_of_data_[2].ToString("0").PadLeft(4, ' ');
            sResult += " /" + decodeFloat(bytes_of_data_, 4).PadLeft(10, ' ') + "  " + GetHartUnit(bytes_of_data_[3]).PadRight(9, ' ');
            sResult += " / Status:" + CFrameHelper.GetBin(bytes_of_data_[8]);
            idx = 9;
            if (Len >= 17)
            {
                sResult += "\n DV" + bytes_of_data_[9].ToString("0").PadLeft(4, ' ');
                sResult += " / Class:" + bytes_of_data_[10].ToString("0").PadLeft(4, ' ');
                sResult += " /" + decodeFloat(bytes_of_data_, 12).PadLeft(10, ' ') + "  " + GetHartUnit(bytes_of_data_[11]).PadRight(9, ' ');
                sResult += " / Status:" + CFrameHelper.GetBin(bytes_of_data_[16]);
                remaining -= 8;
                idx = 17;
            }

            if (Len >= 25)
            {
                sResult += "\n DV" + bytes_of_data_[17].ToString("0").PadLeft(4, ' ');
                sResult += " / Class:" + bytes_of_data_[18].ToString("0").PadLeft(4, ' ');
                sResult += " /" + decodeFloat(bytes_of_data_, 20).PadLeft(10, ' ') + "  " + GetHartUnit(bytes_of_data_[19]).PadRight(9, ' ');
                sResult += " / Status:" + CFrameHelper.GetBin(bytes_of_data_[24]);
                remaining -= 8;
                idx = 25;
            }

            if (Len >= 33)
            {
                sResult += "\n DV" + bytes_of_data_[25].ToString("0").PadLeft(4, ' ');
                sResult += " / Class:" + bytes_of_data_[26].ToString("0").PadLeft(4, ' ');
                sResult += " /" + decodeFloat(bytes_of_data_, 28).PadLeft(10, ' ') + "  " + GetHartUnit(bytes_of_data_[27]).PadRight(9, ' ');
                sResult += " / Status:" + CFrameHelper.GetBin(bytes_of_data_[32]);
                remaining -= 8;
                idx = 33;
            }

            if (Len >= 41)
            {
                sResult += "\n DV" + bytes_of_data_[33].ToString("0").PadLeft(4, ' ');
                sResult += " / Class:" + bytes_of_data_[34].ToString("0").PadLeft(4, ' ');
                sResult += " /" + decodeFloat(bytes_of_data_, 36).PadLeft(10, ' ') + "  " + GetHartUnit(bytes_of_data_[35]).PadRight(9, ' ');
                sResult += " / Status:" + CFrameHelper.GetBin(bytes_of_data_[40]);
                remaining -= 8;
                idx = 41;
            }

            if (Len >= 49)
            {
                sResult += "\n DV" + bytes_of_data_[41].ToString("0").PadLeft(4, ' ');
                sResult += " / Class:" + bytes_of_data_[42].ToString("0").PadLeft(4, ' ');
                sResult += " /" + decodeFloat(bytes_of_data_, 44).PadLeft(10, ' ') + "  " + GetHartUnit(bytes_of_data_[43]).PadRight(9, ' ');
                sResult += " / Status:" + CFrameHelper.GetBin(bytes_of_data_[48]);
                remaining -= 8;
                idx = 49;
            }

            if (Len >= 57)
            {
                sResult += "\n DV" + bytes_of_data_[49].ToString("0").PadLeft(4, ' ');
                sResult += " / Class:" + bytes_of_data_[50].ToString("0").PadLeft(4, ' ');
                sResult += " /" + decodeFloat(bytes_of_data_, 52).PadLeft(10, ' ') + "  " + GetHartUnit(bytes_of_data_[51]).PadRight(9, ' ');
                sResult += " / Status:" + CFrameHelper.GetBin(bytes_of_data_[56]);
                remaining -= 8;
                idx = 57;
            }

            if (Len >= 65)
            {
                sResult += "\n DV" + bytes_of_data_[57].ToString("0").PadLeft(4, ' ');
                sResult += " / Class:" + bytes_of_data_[58].ToString("0").PadLeft(4, ' ');
                sResult += " /" + decodeFloat(bytes_of_data_, 60).PadLeft(10, ' ') + "  " + GetHartUnit(bytes_of_data_[59]).PadRight(9, ' ');
                sResult += " / Status:" + CFrameHelper.GetBin(bytes_of_data_[64]);
                remaining -= 8;
                idx = 65;
            }

            if (remaining >= 4)
            {
                sResult += "\n Time Stamp: " + decodeInt32(bytes_of_data_, idx) + " 1/32 ms";
            }

            return sResult;
        }

        private static string DecodeCmd11(byte[] bytes_of_data_, bool is_req_)
        {
            string sResult;

            if (is_req_)
            {
                if (bytes_of_data_.Length < 6)
                {
                    sResult = "Length Err";
                }
                else
                {
                    sResult = "Short Tag:" + decodePackedASCII(bytes_of_data_, 0, 6);
                }
            }
            else
            {
                sResult = GetConnectionInfo(bytes_of_data_);
            }
            return sResult;
        }

        private static string DecodeCmd21(byte[] bytesOfData, bool isReq)
        {
            string sResult;

            if (isReq)
            {
                if (bytesOfData.Length < 32)
                {
                    sResult = "Length Err";
                }
                else
                {
                    sResult = "Long Tag:" + decodeString(bytesOfData, 0, 32);
                }
            }
            else
            {
                sResult = GetConnectionInfo(bytesOfData);
            }
            return sResult;
        }

        private static string DecodeCmd12(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 18)
            {
                s = "Length Err";
            }
            else
            {
                s = decodePackedASCII(bytes_of_data_, 0, 24);
            }
            return s;
        }

        private static string DecodeCmd13(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 21)
            {
                s = "Length Err";
            }
            else
            {
                s = decodePackedASCII(bytes_of_data_, 0, 6);
                s += "/" + decodePackedASCII(bytes_of_data_, 6, 12);
                s += "/Date:" + decodeDate(bytes_of_data_, 18);
            }
            return s;
        }

        private static string DecodeCmd14(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 16)
            {
                s = "Length Err";
            }
            else
            {
                s = "SensSerNum:" + decodeInt24(bytes_of_data_, 0);
                s += "/SensLim:" + decodeFloat(bytes_of_data_, 8) + " to " + decodeFloat(bytes_of_data_, 4) + " " + GetHartUnit(bytes_of_data_[3]);
                s += "/MinSpan:" + decodeFloat(bytes_of_data_, 12) + " " + GetHartUnit(bytes_of_data_[3]);
            }
            return s;
        }

        private static string DecodeCmd15(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 17)
            {
                s = "Length Err";
            }
            else
            {
                //Alarm Select Code
                switch (bytes_of_data_[0])
                {
                    case 0:
                        s = "Set High Output";
                        break;
                    case 1:
                        s = "Set Low Output";
                        break;
                    case 239:
                        s = "Hold Last Output";
                        break;
                    case 250:
                        s = "Not Used";
                        break;
                    case 251:
                        s = "NONE";
                        break;
                    case 252:
                        s = "Unknown";
                        break;
                    case 253:
                        s = "Special";
                        break;
                    default:
                        s = "Undefined Select Code";
                        break;
                }
                //Transfer Function
                switch (bytes_of_data_[1])
                {
                    case 0:
                        s += "/Linear";
                        break;
                    case 1:
                        s += "/SQR";
                        break;
                    case 2:
                        s += "/SQR pwr 3";
                        break;
                    case 3:
                        s += "/SQR pwr 5";
                        break;
                    case 4:
                        s += "/Special";
                        break;
                    case 5:
                        s += "/SQ";
                        break;
                    case 231:
                        s += "/SQR + Special";
                        break;
                    case 232:
                        s += "/SQR pwr 3 + Special";
                        break;
                    case 233:
                        s += "/SQR pwr 5 + Special";
                        break;
                    case 250:
                        s += "/Not Used";
                        break;
                    case 251:
                        s += "/NONE";
                        break;
                    case 252:
                        s += "/Unknown";
                        break;
                    case 253:
                        s += "/Special";
                        break;
                    default:
                        s += "/Undefined Transfer Function";
                        break;
                }
                s += "/Range:" + decodeFloat(bytes_of_data_, 7) + " to " + decodeFloat(bytes_of_data_, 3) + " " + GetHartUnit(bytes_of_data_[2]);
                s += "/Damping:" + decodeFloat(bytes_of_data_, 11) + "s";
                //Write Protect Code
                switch (bytes_of_data_[15])
                {
                    case 0:
                        s += "/WrProt: No";
                        break;
                    case 1:
                        s += "/WrProt: Yes";
                        break;
                    case 250:
                        s += "/Not Used";
                        break;
                    case 251:
                        s += "/NONE";
                        break;
                    case 252:
                        s += "/Unknown";
                        break;
                    case 253:
                        s += "/Special";
                        break;
                    default:
                        s += "/Undefined Write Protect Code";
                        break;
                }
                s += "/" + bytes_of_data_[16].ToString("0");
                s += "/" + bytes_of_data_[17].ToString("0");
            }
            return s;
        }

        private static string DecodeCmd16(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 3)
            {
                s = "Length Err";
            }
            else
            {
                s = "FinalAssemblyNumber:" + decodeInt24(bytes_of_data_, 0);
            }
            return s;
        }

        private static string DecodeCmd17(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 18)
            {
                s = "Length Err";
            }
            else
            {
                s = "Message:" + decodePackedASCII(bytes_of_data_, 0, 24);
            }
            return s;
        }

        private static string DecodeCmd18(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 21)
            {
                s = "Length Err";
            }
            else
            {
                s = "Tag:" + decodePackedASCII(bytes_of_data_, 0, 6);
                s += "/Descr:" + decodePackedASCII(bytes_of_data_, 6, 12);
                s += "/Date:" + decodeDate(bytes_of_data_, 18);
            }
            return s;
        }

        private static string DecodeCmd33(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 6)
            {
                s = "Length Err";
            }
            else
            {
                s = "Code " + bytes_of_data_[0].ToString("000") + ": " + decodeFloat(bytes_of_data_, 2) + " " + GetHartUnit(bytes_of_data_[1]);
            }
            if (bytes_of_data_.Length > 11)
            {
                s += "\n Code " + bytes_of_data_[6].ToString("000") + ": " + decodeFloat(bytes_of_data_, 8) + " " + GetHartUnit(bytes_of_data_[7]);
            }
            if (bytes_of_data_.Length > 17)
            {
                s += "\n Code " + bytes_of_data_[12].ToString("000") + ": " + decodeFloat(bytes_of_data_, 14) + " " + GetHartUnit(bytes_of_data_[13]);
            }
            if (bytes_of_data_.Length > 23)
            {
                s += "\n Code " + bytes_of_data_[18].ToString("000") + ": " + decodeFloat(bytes_of_data_, 20) + " " + GetHartUnit(bytes_of_data_[19]);
            }

            return s;
        }

        private static string DecodeCmd34(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 4)
            {
                s = "Length Err";
            }
            else
            {
                s = "Damping:" + decodeFloat(bytes_of_data_, 0) + " s";
            }
            return s;
        }

        private static string DecodeCmd35(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 9)
            {
                s = "Length Err";
            }
            else
            {
                s = "Range:" + decodeFloat(bytes_of_data_, 5) + " to " + decodeFloat(bytes_of_data_, 1) + " " + GetHartUnit(bytes_of_data_[0]);
            }
            return s;
        }

        private static string DecodeCmd38(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 2)
            {
                s = "Length Err";
            }
            else
            {
                s = "CfgChCount:" + ((ushort)((bytes_of_data_[0] << 8) + bytes_of_data_[1])).ToString("0");
            }
            return s;
        }

        private static string DecodeCmd48(byte[] bytes_of_data_)
        {
            string s = string.Empty;

            for (int i = 0; i < bytes_of_data_.Length; i++)
            {
                if (i == 0)
                {
                    s = "[" + (i % 10).ToString("0") + "]:" + DecodeBin8(bytes_of_data_[i]) + " ";
                }
                else if ((i % 4) == 0)
                {
                    s += "\n [" + (i % 10).ToString("0") + "]:" + DecodeBin8(bytes_of_data_[i]) + " ";
                }
                else
                {
                    s += "[" + (i % 10).ToString("0") + "]:" + DecodeBin8(bytes_of_data_[i]) + " ";
                }
            }
            return s;
        }

        private static string DecodeCmd54(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 28)
            {
                s = "Length Err";
            }
            else
            {
                s = "Dev Var Code: " + bytes_of_data_[0].ToString("000");
                s += ", Serial Number: " + HartDeviceDLL.BAHA_PickInt24(0, ref bytes_of_data_[1], EN_Endian.MSB_First).ToString("000");
                s += "\n Upper Limit: " + decodeFloat(bytes_of_data_, 5);
                s += ", Lower Limit: " + decodeFloat(bytes_of_data_, 9);
                s += ", Min Span: " + decodeFloat(bytes_of_data_, 17) + " " + GetHartUnit(bytes_of_data_[4]);
                s += "\n Damping: " + decodeFloat(bytes_of_data_, 13) + " s";
                s += "\n Class: " + bytes_of_data_[21].ToString("000") + ", Family: " + bytes_of_data_[22].ToString("000");
                s += "\n Acquisition Period: " + HartDeviceDLL.BAHA_PickDWord(0, ref bytes_of_data_[23], EN_Endian.MSB_First).ToString("000") + ", Prpperties: " + bytes_of_data_[27].ToString("000");
            }

            return s;
        }
        private static string DecodeCmd78(byte[] bytes_of_data_, bool is_req_)
        {
            string s = string.Empty;
            int idx = 0;

            if (is_req_)
            {
                s += "Num requested cmds: " + bytes_of_data_[0].ToString("0");
                if (bytes_of_data_[0] != 0)
                {
                    idx = 1;
                    for (int e = 0; e < bytes_of_data_[0]; e++)
                    {
                        ushort cmd = HartDeviceDLL.BAHA_PickWord(0, ref bytes_of_data_[idx], EN_Endian.MSB_First);

                        s += "\n Cmd: " + cmd.ToString("0") + " / ReqLen: " + bytes_of_data_[idx + 2].ToString("0");
                        if (bytes_of_data_[idx + 2] > 0)
                        {
                            s += "\n ReqData: " + CFrameHelper.HexDump(bytes_of_data_, (byte)(idx + 3), bytes_of_data_[idx + 2]);
                        }
                        idx += 3 + bytes_of_data_[idx + 2];
                    }
                }
            }
            else
            {
                s += "Ext Dev Status: " + DecodeBin8(bytes_of_data_[0]) + " / Num response cmds: " + bytes_of_data_[1].ToString("0");
                if (bytes_of_data_[1] != 0)
                {
                    idx = 2;
                    for (int e = 0; e < bytes_of_data_[1]; e++)
                    {
                        ushort cmd = HartDeviceDLL.BAHA_PickWord(0, ref bytes_of_data_[idx], EN_Endian.MSB_First);

                        s += "\n Cmd: " + cmd.ToString("0") + " / RspLen: " + bytes_of_data_[idx + 2].ToString("0") + " / RspCode: " + bytes_of_data_[idx + 3].ToString("0");
                        if (bytes_of_data_[idx + 2] > 1)
                        {
                            s += "\n RspData: " + CFrameHelper.HexDump(bytes_of_data_, (byte)(idx + 4), (byte)(bytes_of_data_[idx + 2] - 1));
                        }
                        idx += (3 + bytes_of_data_[idx + 2]);
                    }
                }
            }
            return s;
        }

        private static string DecodeCmd109(byte[] bytes_of_data_)
        {
            string s;

            if (bytes_of_data_.Length < 1)
            {
                s = "Length Err";
            }
            else
            {
                if (bytes_of_data_[0] == 0)
                {
                    s = " Burst Off ";
                }
                else
                {
                    s = " Burst On ";
                }
                if (bytes_of_data_.Length >= 2)
                {
                    s += " / Burst Message:" + bytes_of_data_[1].ToString("0");
                }
            }
            return s;
        }
        #endregion

        #region Decoding Functions
        private static string GetConnectionInfo(byte[] bytes_of_data_)
        {
            string sResult;

            if (bytes_of_data_.Length < 12)
            {
                return "Data Length Error";
            }

            sResult = bytes_of_data_[0].ToString("0");
            sResult += "/Man" + bytes_of_data_[1].ToString("0");
            sResult += "/Dev" + bytes_of_data_[2].ToString("0");
            sResult += "/" + bytes_of_data_[3].ToString("0") + " PAs";
            sResult += "/Hart" + bytes_of_data_[4].ToString("0");
            sResult += "/Tx" + bytes_of_data_[5].ToString("0");
            sResult += "/Sw" + bytes_of_data_[6].ToString("0");
            sResult += "/Hw" + bytes_of_data_[7].ToString("0");
            sResult += "/FL" + GetBin(bytes_of_data_[8]);
            sResult += "/ID 0x" + GetHex(bytes_of_data_[9]) + " 0x" + GetHex(bytes_of_data_[10]) + " 0x" + GetHex(bytes_of_data_[11]);
            if (bytes_of_data_.Length >= 17)
            {
                sResult += "\n MinPArsp:" + bytes_of_data_[12].ToString("0");
                sResult += "/MaxNumDVs:" + bytes_of_data_[13].ToString("0");
                sResult += "/CfgChCnt:" + decodeInt16(bytes_of_data_, 14);
                sResult += "/ExtDevStat:" + CFrameHelper.GetBin(bytes_of_data_[16]);
            }
            if (bytes_of_data_.Length >= 22)
            {
                sResult += "\n ManuID:0x" + bytes_of_data_[17].ToString("X2") + bytes_of_data_[18].ToString("X2");
                sResult += "/LabDistID:" + bytes_of_data_[19].ToString("X2") + bytes_of_data_[20].ToString("X2");
                sResult += "/Profile:" + bytes_of_data_[21].ToString("0");
            }
            return sResult;
        }

        private static string decodeFloat(byte[] bytes_of_data_, byte idx_)
        {
            if ((bytes_of_data_[0] == 0x7f) &&
               (bytes_of_data_[1] == 0xA0) &&
               (bytes_of_data_[2] == 0x00) &&
               (bytes_of_data_[3] == 0x00)
             )
            {
                return "Not Used";
            }
            float f = HartDeviceDLL.BAHA_PickFloat(0, ref bytes_of_data_[idx_], 0);
            return f.ToString();
        }

        private static string decodePackedASCII(byte[] bytes_of_data_, byte idx_, byte pa_len_)
        {
            StringBuilder sb = new StringBuilder((pa_len_ * 4) / 3);
            sb.Length = (pa_len_ * 4) / 3;
            HartDeviceDLL.BAHA_PickPackedASCII(sb, (byte)((pa_len_ * 4) / 3), idx_, ref bytes_of_data_[0]);
            return sb.ToString();
        }

        private static string decodeString(byte[] bytes_of_data_, byte idx_, byte len_)
        {
            StringBuilder sb = new StringBuilder(len_);
            sb.Length = len_;
            HartDeviceDLL.BAHA_PickString(sb, len_, idx_, ref bytes_of_data_[0]);
            return sb.ToString();
        }

        private static string decodeInt16(byte[] bytes_of_data_, byte idx_)
        {
            int i = bytes_of_data_[idx_] << 8;

            i += bytes_of_data_[idx_ + 1];

            return i.ToString("0");
        }

        private static string decodeInt24(byte[] bytes_of_data_, byte idx_)
        {
            int i = bytes_of_data_[idx_] << 16;

            i += bytes_of_data_[idx_ + 1] << 8;
            i += bytes_of_data_[idx_ + 2];

            return i.ToString("0");
        }

        private static string decodeInt32(byte[] bytes_of_data_, byte idx_)
        {
            int i = bytes_of_data_[idx_] << 24;

            i += bytes_of_data_[idx_ + 1] << 16;
            i += bytes_of_data_[idx_ + 2] << 8;
            i += bytes_of_data_[idx_ + 3];

            return i.ToString("0");
        }

        private static string decodeDate(byte[] bytes_of_data_, byte idx_)
        {
            int iYear = 1900;
            string s = bytes_of_data_[idx_].ToString("0") + ".";

            s += bytes_of_data_[idx_ + 1].ToString("0") + ".";
            iYear += bytes_of_data_[idx_ + 2];
            s += iYear.ToString("0");
            return s;
        }

        private static string DecodeBin8(byte byte_)
        {
            char[] ach = new char[8];
            byte byMask = 0x80;

            for (int i = 0; i < 8; i++)
            {
                if ((byte_ & byMask) != 0)
                {
                    ach[i] = '1';
                }
                else
                {
                    ach[i] = '0';
                }
                byMask >>= 1;
            }
            return new string(ach);
        }
        #endregion

        #region Private Primitive Functions
        private static string GetHex(byte byte_)
        {
            string s = string.Empty + hexDigit((byte)((byte_ & 0xF0) >> 4));
            return s + hexDigit((byte)(byte_ & 0x0F));
        }

        private static string GetDec(byte byte_)
        {
            return byte_.ToString("##0");
        }

        private static string GetBin(byte byte_)
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

        private static char decDigit(byte byte_)
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

        private static char hexDigit(byte byte_)
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

        #region Hart Unit
        private static string GetHartUnit(byte unit_code_)
        {
            switch (unit_code_)
            {
                case 1: return "InH2O";
                case 2: return "InHg";
                case 3: return "FtH2O";
                case 4: return "mmH2O";
                case 5: return "mmHg";
                case 6: return "psi";
                case 7: return "bar";
                case 8: return "mbar";
                case 9: return "g_SqCm";
                case 10: return "kg_SqCm";
                case 11: return "PA";
                case 12: return "kPA";
                case 13: return "torr";
                case 14: return "ATM";
                case 15: return "ft³/minute";
                case 16: return "USgal/minute";
                case 17: return "l/minute";
                case 18: return "ImpGal/minute";
                case 19: return "m³/hour";
                case 20: return "ft/s";
                case 21: return "m/s";
                case 22: return "USgal/s";
                case 23: return "MilUSgal/day";
                case 24: return "l/s";
                case 25: return "MilL/day";
                case 26: return "ft³/s";
                case 27: return "ft³/day";
                case 28: return "m³/s";
                case 29: return "m³/day";
                case 30: return "ImpGal/h";
                case 31: return "ImpGal/day";
                case 32: return "°C";
                case 33: return "°F";
                case 34: return "Rk";
                case 35: return "K";
                case 36: return "mV";
                case 37: return "Ohm";
                case 38: return "Hz";
                case 39: return "mA";
                case 40: return "gal";
                case 41: return "liter";
                case 42: return "impGal";
                case 43: return "m³";
                case 44: return "ft";
                case 45: return "meter";
                case 46: return "Barrels";
                case 47: return "in";
                case 48: return "cm";
                case 49: return "mm";
                case 50: return "min";
                case 51: return "sec";
                case 52: return "hr";
                case 53: return "day";
                case 54: return "centi_st.";
                case 55: return "cpoise";
                case 56: return "uMhol";
                case 57: return "%";
                case 58: return "V";
                case 59: return "pH";
                case 60: return "grams";
                case 61: return "kg";
                case 62: return "MetTon";
                case 63: return "lbs";
                case 64: return "ShTon";
                case 65: return "LTon";
                case 70: return "g/s";
                case 71: return "g/minute";
                case 72: return "g/h";
                case 73: return "kg/s";
                case 74: return "kg/m";
                case 75: return "kg/h";
                case 76: return "kg/day";
                case 77: return "MetTons/m";
                case 78: return "MetTons/h";
                case 79: return "MetTons/day";
                case 80: return "lbs/s";
                case 81: return "lbs/m";
                case 82: return "lbs/h";
                case 83: return "lbs/day";
                case 84: return "ShTons/m";
                case 85: return "ShTons/h";
                case 86: return "ShTons/day";
                case 87: return "LTons/h";
                case 88: return "LTons/day";
                case 90: return "SGU";
                case 91: return "g/cm³";
                case 92: return "kg/m³";
                case 93: return "pounds/UsGal";
                case 94: return "pounds/ft³";
                case 95: return "g/ml";
                case 96: return "kg/l";
                case 97: return "g/l";
                case 98: return "lb/In³";
                case 99: return "ShTons/Yd³";
                case 100: return "degTwad";
                case 101: return "degBrix";
                case 102: return "degBaum/hv";
                case 103: return "degBaum/lt";
                case 104: return "degAPI";
                case 105: return "Percent_sol_wt";
                case 106: return "Percent_sol_vol";
                case 107: return "degBall";
                case 108: return "proof_vol";
                case 109: return "proof_mass";
                case 110: return "bush";
                case 111: return "Yd³";
                case 112: return "Ft³";
                case 113: return "In³";
                case 120: return "m/h";
                case 130: return "Ft³/h";
                case 131: return "m³/m";
                case 132: return "barrels/s";
                case 133: return "barrels/m";
                case 134: return "barrels/h";
                case 135: return "barrels/day";
                case 136: return "USgal/h";
                case 137: return "ImpGal/s";
                case 138: return "l/h";
                case 139: return "%/StmQual";
                case 151: return "Ftin16";
                case 152: return "Ft³/lb";
                case 153: return "pico_farads";
                case 160: return "Percent_plato";
                case 235: return "gallons/day";
                case 236: return "hl";
                case 237: return "mega_pascals";
                case 238: return "in_H2O_4_degrees_C";
                case 239: return "mm_H2O_4_degrees_C";
                case 240: return "°";
                case 250: return "not used";
                case 251: return "none";
                case 253: return "-/-";
                default: return "unknown";
            }
        }
        #endregion
    }
}
