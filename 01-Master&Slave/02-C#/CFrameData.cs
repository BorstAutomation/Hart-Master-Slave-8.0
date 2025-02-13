/*
 *          File: CFrameData.cs (CFrameData)
 *                The two most important functions of this class (CFrameData) 
 *                are CatchFrame() and GetDisplayString(). CatchFrame() 
 *                reads all information from a binary byte stream that can 
 *                be interpreted (parsed), while GetDispayString() formats 
 *                a text from the information available, which is ultimately 
 *                displayed in a control that understands RTX formatting.
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

#region Name Spaces
using BaTestHart.CmdProperties;
#endregion Name Spaces

namespace BaTestHart
{
    class CFrameData
    {
        #region Private Data
            private byte[]? m_data_bytes = new byte[255];
        #endregion Private Data

        #region Internal Data
        internal uint TotalLen = 0;
        internal uint StartTime = 0;
        internal uint EndTime = 0;
        internal uint RelStartTime = 0xffffffff;
        internal uint RelEndTime = 0;
        internal bool GapTimeOut = false;
        internal bool ValidFrame = false;
        internal string ErrInfo = string.Empty;
        internal string PreGarbage = "|";
        internal uint PreGarbLen = 0;
        internal string Preambles = "|";
        internal uint NumPreambs = 0;
        internal uint NumPreambFFs = 0;
        internal bool PreambleValid = false;
        internal string FrameType = string.Empty;
        internal byte DelByte = 0;
        internal byte ChkStart = 0;
        internal bool LongAddress = false;
        internal bool PrimaryMaster = false;
        internal byte ExpBytes = 0;
        internal byte PhyType = 0;
        internal bool DelValid = false;
        internal string DelString = "|";
        internal bool Primary = false;
        internal bool Burst = false;
        internal bool Request = false;
        internal bool Response = false;
        internal bool ValidAddress = false;
        internal byte[] BytesOfLongAddr = new byte[5];
        internal byte ShortAddr = 0;
        internal string Addr = "|";
        internal byte[] BytesOfExpBytes = new byte[3];
        internal string ExpBytesString = "|";
        internal bool ValidExp = false;
        internal byte CommandByte = 0;
        internal ushort usCommand = 0;
        internal string CommandString = "|";
        internal string ExtCommandString = "|";
        internal bool ValidCmd = false;
        internal bool ValidExtCmd = false;
        internal byte ByteCountByte = 0;
        internal string ByteCountString = "|";
        internal bool ValidBC = false;
        internal bool RspNeeded = false;
        internal byte Rsp1Byte = 0;
        internal string Rsp1String = "|";
        internal bool ValidRsp1 = false;
        internal byte Rsp2Byte = 0;
        internal string Rsp2String = "|";
        internal bool ValidRsp2 = false;
        internal byte DataLen = 0;
        internal byte[]? BytesOfData;
        internal string DataString = "|";
        internal string SymbData = "|";
        internal bool ValidData = false;
        internal byte ChkSumByte = 0;
        internal string ChkSumString = "|";
        internal bool ValidChkSum = false;
        internal byte[]? BytesOfGarbage;
        internal string GarbageString = "|";
        internal bool GarbageAvail = false;
        internal byte GarbageLen = 0;
        internal bool PreGarbageAvail = false;
        internal byte PreGarbageLen = 0;
        internal bool FrameError = false;
        internal bool MinorError = false;
        internal bool ClientTx = false;
        internal int FrameNumber = 0;
        internal string TailingCommentString = string.Empty;
        internal Color TailingCommentColor = Color.Black;
        internal string HeadingCommentString = string.Empty;
        internal Color HeadingCommentColor = Color.Black;
        //------------ Coloring -------------
        internal int OffsSelEndTime = 0;
        internal int LengthEndTime = 0;
        internal int OffsSelType = 0;
        internal int LengthType = 0;
        internal int OffsSelPreGarbage = 0;
        internal int LengthPreGarbage = 0;
        internal int OffsSelFraming = 0;
        internal int LengthFraming = 0;
        internal int OffsSelExtCmd = 0;
        internal int LengthExtCmd = 0;
        internal int OffsSelData = 0;
        internal int LengthData = 0;
        internal int OffsSelError = 0;
        internal int LengthError = 0;
        internal int OffsSelGarbage = 0;
        internal int LengthGarbage = 0;
        internal int OffsSelResp = 0;
        internal int LengthResp = 0;
        internal int OffsSelChk = 0;
        internal int LengthChk = 0;
        internal string ResponseString = string.Empty;
        internal string StatusString = string.Empty;
        internal Color CommentColor = Color.DarkGreen;
        #endregion Internal Data

        #region Constructors
        internal CFrameData()
        {
            // Just to prevent warnings
            m_data_bytes = new byte[255];
        }

        internal CFrameData(ref HartDeviceDLL.TY_MonFrame mon_frame_)
        {
            ushort e;

            m_data_bytes = new byte[mon_frame_.Len];

            #region Get Frame Data
            /* Get the data bytes */
            for (e = 0; e < mon_frame_.Len; e++)
            {
                m_data_bytes[e] = mon_frame_.BytesOfData[e];
            }
            TotalLen = mon_frame_.Len;
            StartTime = mon_frame_.StartTime;
            EndTime = mon_frame_.EndTime;
            if ((mon_frame_.Detail & HartDeviceDLL.CFrameDetail.GAPtimeOutBit) != 0)
            {
                GapTimeOut = true;
            }
            if ((mon_frame_.Detail & HartDeviceDLL.CFrameDetail.CLIENTtxBit) != 0)
            {
                ClientTx = true;
            }
            if (mon_frame_.byValidFrame != 0)
            {
                ValidFrame = true;
            }
            #endregion

            CatchFrame(m_data_bytes);
        }
        #endregion Constructors

        #region Internal Methods
        internal string GetDisplayString(int frame_num_)
        {
            string s = string.Empty;
            string tmp = string.Empty;
            string add_info = string.Empty;
            string indent_first = string.Empty;
            int rsp_indent = 0;
            bool short_display = false;

            #region Initialize
            OffsSelEndTime = 0;
            LengthEndTime = 0;
            OffsSelType = 0;
            LengthType = 0;
            OffsSelPreGarbage = 0;
            LengthPreGarbage = 0;
            OffsSelFraming = 0;
            LengthFraming = 0;
            OffsSelData = 0;
            LengthData = 0;
            OffsSelError = 0;
            LengthError = 0;
            OffsSelResp = 0;
            LengthResp = 0;
            OffsSelGarbage = 0;
            LengthGarbage = 0;
            OffsSelChk = 0;
            LengthChk = 0;
            if ((CSettings.ViewShortBursts) && (Burst))
            {
                short_display = true;
            }
            #endregion Initialize

            #region Display Line Number
            if (CSettings.ViewFrameNumbers)
            {
                if (frame_num_ > 0)
                {
                    tmp += frame_num_.ToString("0").PadLeft(4, ' ') + ":";
                }
                else
                {
                    tmp += "----:";
                }
            }
            #endregion

            #region Display Timing Start
            if (CSettings.ViewTiming)
            {
                if (RelStartTime == 0xffffffff)
                {
                    tmp += "  --->";
                }
                else
                {
                    if ((RelStartTime > 99999))
                    {
                        tmp += "99999>";
                    }
                    else
                    {
                        tmp += RelStartTime.ToString("0").PadLeft(5, ' ') + ">";
                    }
                }
            }
            #endregion

            #region Frame Type
            OffsSelType = tmp.Length;
            tmp += FrameType;
            LengthType = tmp.Length - OffsSelType;
            #endregion

            #region Pre Garbage
            if (PreGarbageAvail && CSettings.ViewPreambles)
            {
                OffsSelPreGarbage = tmp.Length;
                tmp += PreGarbage;
                LengthPreGarbage = tmp.Length - OffsSelPreGarbage;
            }
            #endregion

            #region Display Preambles
            if ((CSettings.ViewPreambles) && (NumPreambs > 0))
            {
                OffsSelFraming = tmp.Length;
                tmp += Preambles;
            }
            #endregion

            #region Display Addressing
            if ((CSettings.ViewAddressing) && (short_display == false))
            {
                if (OffsSelFraming == 0)
                {
                    OffsSelFraming = tmp.Length;
                }
                if (DelValid)
                {
                    tmp += DelString;
                }
                else
                {
                    LengthFraming = tmp.Length - OffsSelFraming;
                }
                if (ValidAddress)
                {
                    tmp += Addr;
                }
                else
                {
                    LengthFraming = tmp.Length - OffsSelFraming;
                }
            }
            #endregion

            #region Display Command
            if (ValidCmd)
            {
                if (OffsSelFraming == 0)
                {
                    OffsSelFraming = tmp.Length;
                }
                tmp += CommandString;
            }
            else
            {
                LengthFraming = tmp.Length - OffsSelFraming;
            }
            #endregion

            if (short_display)
            {
                LengthFraming = tmp.Length - OffsSelFraming;
                AppendEndTime(ref tmp, ref rsp_indent);
                return tmp;
            }

            #region Display Byte Count
            if (ValidBC)
            {
                tmp += ByteCountString;
            }
            else
            {
                LengthFraming = tmp.Length - OffsSelFraming;
            }
            #endregion

            #region Display Responses
            if (RspNeeded)
            {
                if (ByteCountByte > 0)
                {
                    rsp_indent = tmp.Length + 1;
                    tmp += Rsp1String;
                }
                if (ByteCountByte > 1)
                {
                    if (CSettings.ViewStatusBinary)
                    {
                        Rsp2String = "|" + CFrameHelper.GetBin(Rsp2Byte);
                    }
                    else
                    {
                        Rsp2String = "|" + CFrameHelper.GetHex(Rsp2Byte);
                    }
                    tmp += Rsp2String;
                }
            }
            else
            {
                if (ValidFrame)
                {
                    /* Replace the responses by an empty field */
                    tmp += Rsp1String;
                    if (CSettings.ViewStatusBinary)
                    {
                        tmp += "        ";
                    }
                    else
                    {
                        tmp += "   ";
                    }
                }
            }
            #endregion

            if (OffsSelFraming > 0)
            {
                LengthFraming = tmp.Length - OffsSelFraming;
            }

            #region Display extended command
            OffsSelExtCmd = tmp.Length;
            if (ValidExtCmd)
            {
                tmp += ExtCommandString;
            }
            LengthExtCmd = tmp.Length - OffsSelExtCmd;
            #endregion

            #region Display Data
            OffsSelData = tmp.Length;
            if (CSettings.ViewDecodedData)
            {
                if (ByteCountByte > 0)
                {
                    if ((CommandByte == 48) || (CommandByte == 9) || (CommandByte == 0) || (CommandByte == 11) || (CommandByte == 21) || (CommandByte == 78) ||
                        (CommandByte == 3) || (CommandByte == 33) || (CommandByte == 54))
                    {
                        tmp += ReplaceNewLine(tmp.Length, SymbData);
                    }
                    else
                    {
                        tmp += SymbData;
                    }
                }
            }
            else
            {
                if (ByteCountByte > 0)
                {
                    tmp += DataString;
                }
            }

            LengthData = tmp.Length - OffsSelData;
            #endregion

            #region Display Check Sum
            if (ValidChkSum)
            {
                OffsSelChk = tmp.Length + 1;
                tmp += ChkSumString;
                LengthChk = tmp.Length - OffsSelChk;
            }
            #endregion

            #region Display Garbage
            if (PreambleValid)
            {
                if (tmp.Length > 0)
                {
                    if (tmp.Substring(tmp.Length - 1, 1) != "|")
                    {
                        tmp += "|";
                    }
                }
            }

            if (GarbageAvail)
            {
                OffsSelGarbage = tmp.Length;
                tmp += GarbageString;
                LengthGarbage = GarbageString.Length;
            }
            #endregion

            #region Display Timing End
            if (CSettings.ViewTiming)
            {
                if (CSettings.ViewFrameNumbers)
                {
                    tmp += "\n     ";
                    if (rsp_indent > 5)
                        rsp_indent -= 5;
                }
                else
                {
                    tmp += "\n";
                }
                OffsSelEndTime = tmp.Length + 1;
                tmp += RelEndTime.ToString("0").PadLeft(5, ' ') + "<";
                LengthEndTime = tmp.Length - OffsSelEndTime;
                if (rsp_indent > (LengthEndTime + 1))
                {
                    rsp_indent -= (LengthEndTime + 1);
                }
            }
            else
            {
                if (((CSettings.ViewDecodedData && (ResponseString != string.Empty)) ||
                       (CSettings.ViewStatusBinary && (StatusString != string.Empty))
                    ) && (rsp_indent > 0))
                {
                    //tmp += "\n";            
                }
            }
            #endregion

            #region Display Response
            if (CSettings.ViewDecodedData && (ResponseString != string.Empty))
            {
                s = ResponseString;
            }

            if (CSettings.ViewStatusBinary && (StatusString != string.Empty))
            {
                s = CFrameHelper.AppendTextSlash(s, StatusString);
            }

            if ((s != string.Empty) && rsp_indent > 0)
            {
                tmp += new string(' ', rsp_indent);
                OffsSelResp = tmp.Length;
                LengthResp = s.Length;
                tmp += s;
            }
            #endregion

            #region Analysis
            string indent_following = "\n ";
            if (CSettings.ViewTiming)
            {
                indent_first += " ";
                indent_following += "      ";
            }
            else
            {
                if (CSettings.ViewFrameNumbers)
                {
                    indent_first += "      ";
                }
                else
                {
                    indent_first += " ";
                }
            }

            if (CSettings.ViewFrameNumbers)
            {
                indent_following += "     ";
            }

            if (ErrInfo != string.Empty)
            {
                OffsSelError = add_info.Length;
                if (add_info == string.Empty)
                {
                    add_info += indent_first + ErrInfo;
                }
                else
                {
                    add_info += indent_following + ErrInfo;
                }
                LengthError = add_info.Length - OffsSelError;
                OffsSelError += tmp.Length;
            }

            if (add_info != string.Empty)
            {
                if (CSettings.ViewTiming)
                {
                    tmp += add_info;
                }
                else
                {
                    tmp += "\n" + add_info;
                    if (LengthError > 0)
                    {
                        OffsSelError++;
                    }
                }
            }
            #endregion

            return tmp;
        }

        private void AppendEndTime(ref string tmp_, ref int rsp_indent_)
        {
            string result = string.Empty;

            if (CSettings.ViewTiming)
            {
                if (CSettings.ViewFrameNumbers)
                {
                    tmp_ += "\n     ";
                    if (rsp_indent_ > 5)
                        rsp_indent_ -= 5;
                }
                else
                {
                    tmp_ += "\n";
                }
                OffsSelEndTime = tmp_.Length + 1;
                tmp_ += RelEndTime.ToString("0").PadLeft(5, ' ') + "<";
                LengthEndTime = tmp_.Length - OffsSelEndTime;
                if (rsp_indent_ > (LengthEndTime + 1))
                {
                    rsp_indent_ -= (LengthEndTime + 1);
                }
            }
            else
            {
                if (((CSettings.ViewDecodedData && (ResponseString != string.Empty)) ||
                       (CSettings.ViewStatusBinary && (StatusString != string.Empty))
                    ) && (rsp_indent_ > 0))
                {
                    //tmp += "\n";            
                }
            }
        }

        private static string ReplaceNewLine(int indent_, string input_string_)
        {
            string[] astr = input_string_.Split('\n');
            string s;
            string offset = new string(' ', indent_);

            if (astr.Length > 1)
            {
                s = astr[0];
                for (int i = 1; i < astr.Length; i++)
                {
                    s += "\n" + offset + astr[i];
                }
            }
            else
            {
                s = astr[0];
            }
            return s;
        }

        private void CatchFrame(byte[] data_bytes_)
        {
            uint pa_count = 0;
            int del_idx = -1;
            int pa_start_idx = -1;
            bool hart_ip_mode = false;

            #region Timing
            RelEndTime = EndTime - StartTime;
            if (RelEndTime == 0)
            {
                RelEndTime = 1; 
            }
            if (StartTime > EndTime)
            {
                RelEndTime = 0;
            }
            #endregion

            #region Preamble / pre garbage
            if (TotalLen > 5)
            {
                if ((data_bytes_[0] == 0xe0) &&
                    (data_bytes_[1] == 0xa3))
                {
                    hart_ip_mode = true;   
                }
            }

            if (hart_ip_mode)
            {
                byte type = data_bytes_[2];
                ushort sequence_number = (ushort)((data_bytes_[3] << 8) + (data_bytes_[4]));
                if (type == 0)
                {
                    Preambles = "|[Req][S:"; 
                }
                else if (type == 1)
                {
                    Preambles = "|[Rsp][S:"; 
                }
                else if (type == 2)
                {
                    Preambles = "|[Pub][S:"; 
                }
                else if (type == 15)
                {
                    Preambles = "|[NAK][S:"; 
                }
                else
                {
                    Preambles = "|[???][S:"; 
                }

                Preambles = Preambles + sequence_number.ToString("00000");
                Preambles = Preambles + "]";
                NumPreambs = 5;
                pa_count = 5;
                del_idx = 5;
                PreambleValid = true;
            }
            else
            {
                // Preamble Detection
                // search delimiter position
                for (int i = 2; i < TotalLen; i++)
                {
                        if ((data_bytes_[i] != 0xff) &&
                             (data_bytes_[i - 1] == 0xff) &&
                             (data_bytes_[i - 2] == 0xff)
                          )
                        {
                            // Set delimiter position 
                            del_idx = i;
                            PreambleValid = true;
                            break;
                        }
                }

                // search preamble start
                if (del_idx > 1)
                {
                    for (int i = del_idx - 1; i >= 0; i--)
                    {
                        if (data_bytes_[i] != 0xff)
                        {
                            pa_start_idx = i + 1;
                            break;
                        }
                        else
                        {
                            if (i == 0)
                            {
                                pa_start_idx = 0;
                            }
                        }
                    }
                }

                // store the pre garbage
                if (pa_start_idx > 0)
                {
                    PreGarbageAvail = true;
                    PreGarbage = "|?" + CFrameHelper.GetHex(data_bytes_[0]);
                    PreGarbageLen = 1;
                    for (int i = 1; i < pa_start_idx; i++)
                    {
                        PreGarbage += " " + CFrameHelper.GetHex(data_bytes_[i]);
                        PreGarbageLen++;
                    }
                }

                // store the preamble
                if (pa_start_idx != -1)
                {
                    for (int i = pa_start_idx; i < del_idx; i++)
                    {
                        if (i == pa_start_idx)
                        {
                            Preambles = "|FF";
                        }
                        else
                        {
                            Preambles += " FF";
                        }
                        pa_count++;
                    }
                }

                NumPreambs = pa_count;
            }

            int idx = del_idx;

            //Continue Frame
            if (TotalLen == 1)
            {
                ErrInfo = "Jabber Byte?";
                FrameError = true;
                MinorError = true;
            }
            else if (TotalLen < 3)
            {
                ErrInfo = "Too few bytes";
                FrameError = true;
                MinorError = true;
            }
            else if (pa_count == 0)
            {
                ErrInfo = "No preamble detected";
                FrameError = true;
            }
            else if (pa_count < 2)
            {
                ErrInfo = "Too few preamble bytes";
                FrameError = true;
            }
            else if (pa_count > 20)
            {
                ErrInfo = "Too many preamble bytes";
                FrameError = true;
            }
            #endregion

            #region Delimiter
            if (PreambleValid)
            {
                if ((TotalLen - idx) > 0)
                {
                    if ((data_bytes_[idx] & 0x80) != 0)
                    {
                        LongAddress = true;
                        FrameType = "L";
                    }
                    else
                    {
                        FrameType = "S";
                    }
                    switch (data_bytes_[idx] & 0x07)
                    {
                        case 0x06:
                            FrameType += "ACK";
                            Response = true;
                            break;
                        case 0x02:
                            FrameType += "STX";
                            Request = true;
                            break;
                        case 0x01:
                            FrameType += "BCK";
                            Burst = true;
                            break;
                        default:
                            FrameType += "???";
                            break;
                    }
                    ExpBytes = (byte)((data_bytes_[idx] & 0x60) >> 5);
                    PhyType = (byte)((data_bytes_[idx] & 0x18) >> 3);
                    switch (data_bytes_[idx] & 0x07)
                    {
                        case 0x06:
                        case 0x01:
                        case 0x02:
                            DelString += CFrameHelper.GetHex(data_bytes_[idx]);
                            ChkStart = (byte)idx;
                            DelByte = (byte)(data_bytes_[idx++] & 0x07);
                            DelValid = true;
                            if (DelByte != 2)
                            {
                                RspNeeded = true;
                            }
                            break;
                        default:
                            ErrInfo = "Invalid delemiter!";
                            FrameError = true;
                            break;
                    }
                }
                else
                {
                    ErrInfo = "No delimiter!";
                    FrameError = true;
                }
            }
            ushort e;
            #endregion

            #region Address
            if (DelValid)
            {
                if ((TotalLen - idx) > 0)
                {
                    if ((data_bytes_[idx]
                        & 0x80) > 0)
                    {
                        PrimaryMaster = true;
                    }
                }
                if (PrimaryMaster)
                {
                    FrameType += "P";
                }
                else
                {
                    FrameType += "S";
                }
                if (LongAddress)
                {
                    if ((TotalLen - idx) > 4)
                    {
                        for (e = 0; e < 5; e++)
                        {
                            BytesOfLongAddr[e] = data_bytes_[idx];
                            if (e == 0)
                            {
                                Addr += CFrameHelper.GetHex(data_bytes_[idx++]);
                            }
                            else
                            {
                                Addr += " " + CFrameHelper.GetHex(data_bytes_[idx++]);
                            }
                        }
                        ValidAddress = true;
                    }
                    else
                    {
                        ErrInfo = "Too few address bytes!";
                        FrameError = true;
                    }
                }
                else
                {
                    if ((TotalLen - idx) > 0)
                    {
                        ShortAddr = data_bytes_[idx];
                        Addr += CFrameHelper.GetHex(data_bytes_[idx++]) + "            ";
                        ValidAddress = true;
                    }
                    else
                    {
                        ErrInfo = "No address byte!";
                        FrameError = true;
                    }
                }
            }
            #endregion

            #region Expansion Bytes
            if (ValidAddress)
            {
                if (ExpBytes > 0)
                {
                    if ((TotalLen - idx) >= ExpBytes)
                    {
                        for (e = 0; e < ExpBytes; e++)
                        {
                            BytesOfExpBytes[e] = data_bytes_[idx];
                            if (e == 0)
                            {
                                ExpBytesString += CFrameHelper.GetHex(data_bytes_[idx++]);
                            }
                            else
                            {
                                ExpBytesString += " " + CFrameHelper.GetHex(data_bytes_[idx++]);
                            }
                        }
                        ValidExp = true;
                    }
                    else
                    {
                        ErrInfo = "Too few expansion bytes!";
                        FrameError = true;
                    }
                }
                else
                {
                    ValidExp = false;
                }
            }
            #endregion

            #region Command
            if ((ValidAddress) && (ValidExp || (ExpBytes == 0)))
            {
                if ((TotalLen - idx) > 0)
                {
                    string s;

                    CommandByte = data_bytes_[idx];
                    s = CommandByte.ToString("0");
                    if (s.Length < 2)
                    {
                        s = s.PadLeft(2, ' ');
                    }
                    CommandString += "Cmd" + s;
                    idx++;
                    ValidCmd = true;
                }
                else
                {
                    ErrInfo = "Missing command!";
                    FrameError = true;
                }
            }
            #endregion

            #region Byte Count
            if (ValidCmd)
            {
                if ((TotalLen - idx) > 0)
                {
                    ByteCountByte = data_bytes_[idx];
                    DataLen = ByteCountByte;
                    ByteCountString += CFrameHelper.GetDec(data_bytes_[idx++]);
                    ValidBC = true;
                }
                else
                {
                    ErrInfo = "Missing byte count!";
                    FrameError = true;
                }
            }
            #endregion

            #region Response Code 1
            if (!RspNeeded)
            {
                if (ValidCmd)
                {
                    ValidRsp1 = true;
                }
                Rsp1String += "   ";
            }
            if ((ValidCmd) && (RspNeeded))
            {
                if ((TotalLen - idx) > 0)
                {
                    Rsp1Byte = data_bytes_[idx];
                    Rsp1String += CFrameHelper.GetDec(data_bytes_[idx++]);
                    ValidRsp1 = true;
                    DataLen--;
                    ResponseString = CCmdProps.GetResponseText(CommandByte, Rsp1Byte);
                }
                else
                {
                    ErrInfo = "Missing response 1!";
                    FrameError = true;
                }
            }
            #endregion

            #region Response Code 2
            if (!RspNeeded)
            {
                if (ValidCmd)
                {
                    ValidRsp2 = true;
                }
                Rsp2String += "   ";
            }
            if ((ValidRsp1) && (RspNeeded))
            {
                if ((TotalLen - idx) > 0)
                {
                    Rsp2Byte = data_bytes_[idx];
                    Rsp2String += CFrameHelper.GetBin(data_bytes_[idx++]);
                    ValidRsp2 = true;
                    DataLen--;
                    StatusString = CCmdProps.GetStatusText(Rsp2Byte);
                    if (ResponseString == string.Empty)
                    {
                        StatusString = "   " + StatusString;
                    }
                }
                else
                {
                    ErrInfo = "Missing response 2!";
                    FrameError = true;
                }
            }
            #endregion

            #region Hart Data
            if (ValidRsp2)
            {
                if (DataLen > 0)
                {
                    if ((TotalLen - idx) < DataLen)
                    {
                        ErrInfo = "Too few data bytes!";
                        FrameError = true;
                    }
                    else
                    {
                        if (CommandByte == 31)
                        {
                            if (DataLen < 2)
                            {
                                ErrInfo = "Extended command format error!";
                                FrameError = true;
                            }
                            else
                            {
                                // fetch the extended command value
                                usCommand = (ushort)((ushort)(data_bytes_[idx++] << 8) + (ushort)(data_bytes_[idx++]));
                                ExtCommandString += "ExtCmd(" + usCommand.ToString("0") + ")";
                                // correct the data length, idx already corrected
                                DataLen -= 2;
                                ValidExtCmd = true;
                            }
                        }
                        if (DataLen > 0)
                        {
                            BytesOfData = new byte[DataLen];
                            for (e = 0; e < DataLen; e++)
                            {
                                BytesOfData[e] = data_bytes_[idx + e];
                            }
                            for (e = 0; e < DataLen; e++)
                            {
                                if (e == 0)
                                {
                                    DataString += CFrameHelper.GetHex(data_bytes_[idx++]);
                                }
                                else
                                {
                                    DataString += " " + CFrameHelper.GetHex(data_bytes_[idx++]);
                                }
                            }
                            ValidData = true;
                        }
                    }
                }
                else
                {
                    ValidData = false;
                }
                if (ValidData && ValidCmd)
                {
                    if (BytesOfData !=  null)
                    { 
                        if (DecodeCmdData.WasCmdDataDecoded(out SymbData, CommandByte, BytesOfData, Request))
                        {
                            SymbData = "|" + SymbData;
                        }
                        else
                        {
                            SymbData = DataString;
                        }
                    }
                }
                else
                {
                    if (DataLen == 0)
                    {
                        SymbData = "|No Data";
                    }
                    else
                    {
                        SymbData = "|";
                    }
                }
            }
            #endregion

            #region Checksum
            if (ValidData || ((DataLen == 0) && ValidRsp2))
            {
                if ((TotalLen - idx) > 0)
                {
                    byte byIdx = ChkStart;
                    byte byEnd = (byte)idx;
                    byte byChk = 0;

                    ChkSumByte = data_bytes_[idx];
                    ChkSumString += CFrameHelper.GetHex(data_bytes_[idx++]);
                    ValidChkSum = true;
                    while (byIdx < byEnd)
                    {
                        byChk ^= data_bytes_[byIdx++];
                    }
                    if (byChk != ChkSumByte)
                    {
                        ErrInfo = "Checksum Error!";
                        FrameError = true;
                    }
                }
                else
                {
                    if (ErrInfo == string.Empty)
                    {
                        ErrInfo = "Missing checksum!";
                        FrameError = true;
                    }
                }
            }
            #endregion

            #region Garbage
            if (idx == -1)
            {
                idx = 0;
            }
            if (TotalLen > idx)
            {
                GarbageAvail = true;
                GarbageLen = (byte)(TotalLen - idx);
                BytesOfGarbage = new byte[GarbageLen];
                GarbageString = "?";
                for (e = 0; e < GarbageLen; e++)
                {
                    BytesOfGarbage[e] = data_bytes_[idx];
                    if (e == 0)
                    {
                        GarbageString += CFrameHelper.GetHex(data_bytes_[idx++]);
                    }
                    else
                    {
                        GarbageString += " " + CFrameHelper.GetHex(data_bytes_[idx++]);
                    }
                }
            }
            #endregion
        }
        #endregion Internal Methods

    }
}
