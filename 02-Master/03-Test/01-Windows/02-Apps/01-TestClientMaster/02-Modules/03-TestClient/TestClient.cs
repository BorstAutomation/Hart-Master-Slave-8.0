/*
 *          File: TestClient.cs (CTestClient)
 *                TestClient is a very central module through which almost 
 *                all communication processes are handled.
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
using BaTestHart.DataSyntax;
using System.Runtime.InteropServices;
using System.Text;
#endregion Namespaces

namespace BaTestHart
{
    #region Globals
    public static class CMALib
    {
        #region Global Textes
        public static class CGlobText
        {
            public const string TEST_CPP_MASTER = "C++ Hart Master Test";
            public const string VERSION = "8.0E";
        }
        #endregion Global Textes

        #region Global Constants
        public static class CConsts
        {
            public static class CTClientECodes
            {
                public const int NO_ERROR = 0;
                public const int UNKNOWN_ERROR = 1;
                public const int COMM_ERROR = 2;
                public const int EMPTY_SERVICE = 3;
                public const int INVALID_HANDLE = 4;
                public const int NO_DEVICE_RESPONSE = 5;
                public const int CONNECTION_FAILED = 6;
            }
        }
        #endregion Global Constants
    }
    #endregion Globals

    internal static class CTestClient
    {
        #region Private Data
        private static FrmTestClient? mo_parent;
        private static byte m_dev_hart_rev = 5;
        private static ushort m_cfg_ch_count = 0;
        #endregion Private Data

        #region Internal Data
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        internal static byte[] BytesOfData = new byte[255];
        internal static bool IsComPortOpen = false;
        internal static bool IsConnected = false;
        internal static HartDeviceDLL.TY_Configuration Configuration;
        internal static HartDeviceDLL.TY_Connection ConnectionInfo;
        internal static HartDeviceDLL.TY_Confirmation Confirmation;
        internal static ushort hServiceDoCmd = HartDeviceDLL.INVALID_SRV_Handle;
        internal static ushort hServiceConnect = HartDeviceDLL.INVALID_SRV_Handle;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        internal static byte[] BytesOfUniqueId = new byte[5];
        #endregion Internal Data

        #region Initialization
        internal static void Init(FrmTestClient f_)
        {
            ConnectionInfo.BytesOfUniqueID = new byte[5];
            Configuration.HartIpAddress = new byte[64];
            Configuration.HartIpHostName = new byte[64];
            Configuration.HartIpPort = new byte[64];
            Confirmation.BytesOfData = new byte[255];
            BytesOfData = new byte[255];
            mo_parent = f_;
        }
        #endregion Initialization

        #region Internal Properties	  
        internal static bool IsPrimaryMaster
        {
            get
            {
                //HartDeviceDLL.BAHAMA_GetRunTimeInfo(ref CTestClient.RunTimeInfo);
                //if (CTestClient.RunTimeInfo.ActualMaster == 1)
                //{
                    return true;
                //}

                //return false;
            }
        }
        #endregion Internal Properties	  

        #region Setup/Terminate
        internal static void Configure()
        {
            if (CDevStatus.CGeneral.ActiveComport != CSettings.ComPort)
            {
                // Close the channel and open a new one if there is a new
                // com port selected
                if (IsComPortOpen)
                {
                    HartDeviceDLL.BAHAMA_CloseChannel();
                    HartDeviceDLL.BAHAMA_StopMonitor();
                    CTestClient.hServiceConnect = HartDeviceDLL.INVALID_SRV_Handle;
                    CTestClient.hServiceDoCmd = HartDeviceDLL.INVALID_SRV_Handle;
                    IsComPortOpen = false;  
                    IsConnected = false;    
                }
                else
                {
                    // Delete service handles
                    CTestClient.hServiceConnect = HartDeviceDLL.INVALID_SRV_Handle;
                    CTestClient.hServiceDoCmd = HartDeviceDLL.INVALID_SRV_Handle;
                }

                CDevStatus.CGeneral.ActiveComport = CSettings.ComPort;
                if (HartDeviceDLL.BAHAMA_OpenChannel(CSettings.ComPort) == EN_Bool.TRUE8)
                {
                    HartDeviceDLL.BAHAMA_StartMonitor();
                    CTestClient.IsComPortOpen = true;
                }
                else
                {
                    CTestClient.IsComPortOpen = false;
                }
            }

            if (CTestClient.IsComPortOpen)
            {
                // Configure connection
                HartDeviceDLL.BAHAMA_GetConfiguration(ref CTestClient.Configuration);
                InitHartDLLconfig();
                HartDeviceDLL.BAHAMA_SetConfiguration(ref Configuration);
                mo_parent?.SetComPortStatus(FrmTestClient.CComPortStatus.OK);
            }
            else
            {
                CDevStatus.CGeneral.ActiveComport = 0;
                mo_parent?.SetComPortStatus(FrmTestClient.CComPortStatus.FAILED);
            }
        }

        internal static void Disconnect()
        {
            IsConnected = false;
        }

        internal static void Terminate()
        {
            if (CTestClient.IsComPortOpen)
            {
                HartDeviceDLL.BAHAMA_CloseChannel();
                CTestClient.IsComPortOpen = false;
            }

            CDevStatus.CGeneral.ActiveComport = 0;
            m_dev_hart_rev = 5;
            m_cfg_ch_count = 0;
        }
        #endregion

        #region HartMasterDLL Configuration
        private static void InitHartDLLconfig()
        {
            CTestClient.Configuration.Baudrate = CSettings.BaudRate switch
            {
                1 => 2400,
                2 => 4800,
                3 => 9600,
                4 => 19200,
                5 => 38400,
                6 => 57600,
                7 => 115200,
                _ => 1200,
            };

            CTestClient.Configuration.NumPreambles = (byte)(CSettings.NumPreambles);
            CTestClient.Configuration.NumRetries = CSettings.NumRetries;
            CTestClient.Configuration.InitialMaster = CSettings.InitialMasterRole;
            CTestClient.Configuration.RetryIfBusy = CSettings.RetryIfBusy;
            CTestClient.Configuration.AddressingMode = HartDeviceDLL.CAddressingMode.LONGAddress;
            byte[] host_name = Encoding.UTF8.GetBytes(CSettings.HartIpHostName);
            int len = host_name.Length;
            if (len > 63)
            {
                len = 63;
            }

            FillByteArrayFromString(ref CTestClient.Configuration.HartIpHostName, CSettings.HartIpHostName, 64);
            FillByteArrayFromString(ref CTestClient.Configuration.HartIpAddress, CSettings.HartIpAddress, 64);
            FillByteArrayFromString(ref CTestClient.Configuration.HartIpPort, CSettings.HartIpPort, 64);
            if (CSettings.HartIpUseAddress == true)
            {
                CTestClient.Configuration.HartIpUseAddress = EN_Bool.TRUE8;
            }
            else
            {
                CTestClient.Configuration.HartIpUseAddress = EN_Bool.FALSE8;
            }

            CTestClient.Configuration.HartIpSendKeepAlive = (EN_Bool)CSettings.HartIpSendKeepAlive;
        }
        #endregion

        #region Handle Services
        internal static void HandleDoCmdService()
        {
            int iError = CMALib.CConsts.CTClientECodes.NO_ERROR;

            HartDeviceDLL.BAHAMA_FetchConfirmation(CTestClient.hServiceDoCmd, ref CTestClient.Confirmation);
            CTestClient.hServiceDoCmd = HartDeviceDLL.INVALID_SRV_Handle;
            if (CTestClient.Confirmation.SrvResultCode != HartDeviceDLL.SRVsuccessful)
            {
                iError = CMALib.CConsts.CTClientECodes.UNKNOWN_ERROR;

                switch (Confirmation.SrvResultCode)
                {
                    case HartDeviceDLL.SRVcommErr:
                        iError = CMALib.CConsts.CTClientECodes.COMM_ERROR;
                        break;
                    case HartDeviceDLL.SRVempty:
                        iError = CMALib.CConsts.CTClientECodes.EMPTY_SERVICE;
                        break;
                    case HartDeviceDLL.SRVinvalidHandle:
                        iError = CMALib.CConsts.CTClientECodes.INVALID_HANDLE;
                        break;
                    case HartDeviceDLL.SRVnoDevResp:
                        iError = CMALib.CConsts.CTClientECodes.NO_DEVICE_RESPONSE;
                        IsConnected = false; 
                        break;
                }
            }
            else
            {
                if (CTestClient.Confirmation.DataLen > 0)
                {
                    CTestClient.HandleData();
                }
            }

            mo_parent?.HandleMonitorData();
            mo_parent?.UpdateServiceDisplay(iError);
        }

        internal static void HandleConnectService()
        {
            int iError = CMALib.CConsts.CTClientECodes.NO_ERROR;

            HartDeviceDLL.BAHAMA_FetchConnection(CTestClient.hServiceConnect, ref CTestClient.ConnectionInfo);
            CTestClient.hServiceConnect = HartDeviceDLL.INVALID_SRV_Handle;
            if (CTestClient.ConnectionInfo.SrvResultCode == HartDeviceDLL.SRVsuccessful)
            {
                mo_parent?.UpdateUniqueID(CTestClient.ConnectionInfo.BytesOfUniqueID);
                m_dev_hart_rev = CTestClient.ConnectionInfo.CmdRevNum;
                m_cfg_ch_count = CTestClient.ConnectionInfo.CfgChCount;
                IsConnected = true; 
            }
            else
            {
                iError = CMALib.CConsts.CTClientECodes.CONNECTION_FAILED;
                m_dev_hart_rev = 5;
                m_cfg_ch_count = 0;
                IsConnected = false;
            }

            mo_parent?.HandleMonitorData();
            mo_parent?.UpdateServiceDisplay(iError);
        }
        #endregion

        #region Send Commands
        // Generic
        internal static ushort DoCommand(byte command_, byte qos_, ref byte data_ref_, byte data_len_)
        {
            return HartDeviceDLL.BAHAMA_DoCommand(command_, qos_, ref data_ref_, data_len_, ref BytesOfUniqueId[0]);
        }

        internal static void ReqCmd0short()
        {
            if (IsRequestAllowed())
            {
                hServiceConnect = HartDeviceDLL.BAHAMA_ConnectByAddr(CSettings.PollAddress, HartDeviceDLL.NO_WAIT_For_Service, CSettings.NumRetries);
            }
        }

        internal static void ReqCmd0short_Wait()
        {
            if (IsRequestAllowed())
            {
                hServiceConnect = HartDeviceDLL.BAHAMA_ConnectByAddr(CSettings.PollAddress, HartDeviceDLL.WAIT_For_Service, CSettings.NumRetries);
            }
        }

        internal static void ReqCmd6(byte new_addr_)
        {
            if (MessageBox.Show("Do you really want to change the slave address?", "Verification", MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            if (IsRequestAllowed())
            {
                BytesOfData[0] = new_addr_;

                hServiceDoCmd = DoCommand(6, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 1);
            }
        }

        internal static void ReqCmd31()
        {
            byte byLen;
            try
            {
                byLen = CDataSyntaxHelper.PutToBuffer(CSettings.ExtCmdData, ref CTestClient.BytesOfData[0]);
            }
            catch
            {
                MessageBox.Show("Syntax error, use ';' to separate items.");
                return;
            }

            if (CTestClient.IsRequestAllowed())
            {
                CTestClient.hServiceDoCmd = HartDeviceDLL.BAHAMA_DoExtCmd(CSettings.ExtCmd,
                                                                HartDeviceDLL.NO_WAIT_For_Service,
                                                                ref CTestClient.BytesOfData[0],
                                                                byLen,
                                                                ref CTestClient.BytesOfUniqueId[0]);
            }
        }
        internal static void ReqCmd1()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(1, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd2()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(2, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd3()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(3, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd3_Wait()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(3, HartDeviceDLL.WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd12()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(12, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd12_Wait()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(12, HartDeviceDLL.WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd13()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(13, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd13_Wait()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(13, HartDeviceDLL.WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd14()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(14, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd14_Wait()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(14, HartDeviceDLL.WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd15()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(15, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqCmd15_Wait()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(15, HartDeviceDLL.WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }

        internal static void ReqCmd18(string tag_name_, string descriptor_, byte day_, byte month_, byte year_)
        {
            if (IsRequestAllowed())
            {
                StringBuilder sb = new StringBuilder(8);

                sb.Insert(0, MakeFixLength(tag_name_.ToUpper(), 8));
                HartDeviceDLL.BAHA_PutPackedASCII(sb, 8, 0, ref BytesOfData[0]);
                sb = new StringBuilder(16);
                sb.Insert(0, MakeFixLength(descriptor_.ToUpper(), 16));
                HartDeviceDLL.BAHA_PutPackedASCII(sb, 16, 6, ref BytesOfData[0]);
                BytesOfData[18] = day_;
                BytesOfData[19] = month_;
                BytesOfData[20] = year_;
                hServiceDoCmd = CTestClient.DoCommand(18, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 21);
            }
        }

        internal static void ReqCmd20_Wait()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(20, HartDeviceDLL.WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }

        internal static void ReqCmd35(byte units_code_, float upper_range_, float lower_range_)
        {
            if (IsRequestAllowed())
            {
                BytesOfData[0] = units_code_;
                HartDeviceDLL.BAHA_PutFloat(upper_range_, 1, ref BytesOfData[0], EN_Endian.MSB_First);
                HartDeviceDLL.BAHA_PutFloat(lower_range_, 5, ref BytesOfData[0], EN_Endian.MSB_First);
                hServiceDoCmd = CTestClient.DoCommand(35, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 21);
            }
        }

        internal static void ReqCmd38()
        {
            if (IsRequestAllowed())
            {
                if (m_dev_hart_rev >= 7)
                {
                    if (BytesOfData != null)
                    {
                        BytesOfData[0] = (byte)(m_cfg_ch_count >> 8);
                        BytesOfData[1] = (byte)(m_cfg_ch_count & 0xff);

                        hServiceDoCmd = CTestClient.DoCommand(38, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 2);
                    }
                }
                else
                {
                    hServiceDoCmd = CTestClient.DoCommand(38, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 0);
                }
            }
        }

        internal static void ReqCmd48()
        {
            if (IsRequestAllowed())
            {
                hServiceDoCmd = CTestClient.DoCommand(48, HartDeviceDLL.NO_WAIT_For_Service, ref BytesOfData[0], 0);
            }
        }
        internal static void ReqUserCmd()
        {
            byte len;

            try
            {
                len = CDataSyntaxHelper.PutToBuffer(CSettings.CustomCmdData, ref CTestClient.BytesOfData[0]);
            }

            catch
            {
                MessageBox.Show("Syntax error, use ';' to separate items.");
                return;
            }

            if (CTestClient.IsRequestAllowed())
            {
                CTestClient.hServiceDoCmd = CTestClient.DoCommand(CSettings.CustomCmd,
                                                                  HartDeviceDLL.NO_WAIT_For_Service,
                                                                  ref CTestClient.BytesOfData[0],
                                                                  len);
            }
        }
        #endregion

        #region Helper
        private static bool IsRequestAllowed()
        {
            if (!CTestClient.IsComPortOpen)
            {
                return false;
            }

            if (CTestClient.hServiceConnect != HartDeviceDLL.INVALIDserviceHandle)
            {
                return false;
            }

            if (CTestClient.hServiceDoCmd != HartDeviceDLL.INVALIDserviceHandle)
            {
                return false;
            }

            return true;
        }

        private static string MakeFixLength(string s_, uint len_)
        {
            int e;
            int iSpacesNeeded;

            iSpacesNeeded = (int)(len_ - s_.Length);
            if (iSpacesNeeded > 0)
            {
                for (e = 0; e < iSpacesNeeded; e++)
                {
                    s_ += " ";
                }
            }
            return s_;
        }

        internal static byte[] GetUniqueID()
        {
            return BytesOfUniqueId;
        }

        internal static void SetUniqueID(byte[] unique_id_)
        {
            Array.Copy(unique_id_, BytesOfUniqueId, 5);
        }

        internal static void FillByteArrayFromString(ref byte[] dest_, string src_, byte length_)
        {
            byte[] tmp = Encoding.UTF8.GetBytes(src_);
            int len = src_.Length;
            if (len > 63)
            {
                len = 63;
            }

            Array.Copy(tmp, dest_, len);
            dest_[len] = 0;
        }
        #endregion

        #region Hart Data Handling
        private static void HandleData()
        {
            if (CTestClient.Confirmation.Cmd == 3)
            {
                // Get current and PVs
                mo_parent?.DevDatShowCurrent(HartDeviceDLL.BAHA_PickFloat(0, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First));
                if (CTestClient.Confirmation.DataLen > 7)
                {
                    mo_parent?.DevDatShowPV(HartDeviceDLL.BAHA_PickFloat(5, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First),
                        Confirmation.BytesOfData[4]);
                }

                if (CTestClient.Confirmation.DataLen > 12)
                {
                    mo_parent?.DevDatShowSV(HartDeviceDLL.BAHA_PickFloat(10, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First),
                        Confirmation.BytesOfData[9]);
                }

                if (CTestClient.Confirmation.DataLen > 17)
                {
                    mo_parent?.DevDatShowTV(HartDeviceDLL.BAHA_PickFloat(15, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First),
                        Confirmation.BytesOfData[14]);
                }

                if (CTestClient.Confirmation.DataLen > 22)
                {
                    mo_parent?.DevDatShowQV(HartDeviceDLL.BAHA_PickFloat(20, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First),
                        Confirmation.BytesOfData[19]);
                }
            }

            if ((CTestClient.Confirmation.Cmd == 12) || (CTestClient.Confirmation.Cmd == 17))
            {
                // Read Message Command
                StringBuilder sb = new StringBuilder(new string(' ', 32));

                HartDeviceDLL.BAHA_PickPackedASCII(sb, 32, 0, ref CTestClient.Confirmation.BytesOfData[0]);
                mo_parent?.DevDatShowMessage(sb.ToString());
            }

            if ((Confirmation.Cmd == 13) || (Confirmation.Cmd == 18))
            {
                StringBuilder sb = new StringBuilder(new string(' ', 16));

                // Tag Name
                sb.Length = 8;
                HartDeviceDLL.BAHA_PickPackedASCII(sb, 8, 0, ref CTestClient.Confirmation.BytesOfData[0]);
                CSettings.TagNameShort = sb.Length == 0 ? string.Empty : sb.Length > 8 ? sb.ToString(0, 8) : sb.ToString().PadRight(8, ' ');
                mo_parent?.DetDatShowShortTag(sb.ToString());
                /* Descriptor */
                sb.Length = 16;
                HartDeviceDLL.BAHA_PickPackedASCII(sb, 16, 6, ref CTestClient.Confirmation.BytesOfData[0]);
                CSettings.Descriptor = sb.Length == 0 ? string.Empty : sb.Length > 16 ? sb.ToString(0, 16) : sb.ToString().PadRight(16, ' ');
                /* Date */
                CSettings.Day = Confirmation.BytesOfData[18];
                CSettings.Month = Confirmation.BytesOfData[19];
                CSettings.Year = Confirmation.BytesOfData[20];
            }

            if (Confirmation.Cmd == 14)
            {
                mo_parent?.DevDatShowSpan(HartDeviceDLL.BAHA_PickFloat(12, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First),
                    Confirmation.BytesOfData[3]);
            }

            if(Confirmation.Cmd == 15)
            {
                // Range values
                CSettings.RangeUnitIdx = Confirmation.BytesOfData[2];
                CSettings.UpperRange = HartDeviceDLL.BAHA_PickFloat(3, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First);
                CSettings.LowerRange = HartDeviceDLL.BAHA_PickFloat(7, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First);
                mo_parent?.DevDatShowUpperRange(CSettings.UpperRange, CSettings.RangeUnitIdx);
                mo_parent?.DevDatShowLowerRange(CSettings.LowerRange, CSettings.RangeUnitIdx);
            }

            if(Confirmation.Cmd == 35)
            {
                // Range values
                CSettings.RangeUnitIdx = Confirmation.BytesOfData[0];
                CSettings.UpperRange = HartDeviceDLL.BAHA_PickFloat(1, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First);
                CSettings.LowerRange = HartDeviceDLL.BAHA_PickFloat(5, ref Confirmation.BytesOfData[0], EN_Endian.MSB_First);
                mo_parent?.DevDatShowUpperRange(CSettings.UpperRange, CSettings.RangeUnitIdx);
                mo_parent?.DevDatShowLowerRange(CSettings.LowerRange, CSettings.RangeUnitIdx);
            }

            if(Confirmation.Cmd == 20)
            {
                StringBuilder sb = new StringBuilder(new string(' ', 32));

                HartDeviceDLL.BAHA_PickString(sb, 32, 0, ref CTestClient.Confirmation.BytesOfData[0]);
                CSettings.TagNameLong = sb.ToString();
                mo_parent?.DevDatShowLongTag(CSettings.TagNameLong);
            }
        }
        #endregion

        #region Hart Ip Handling
        internal static bool HartIpConnect()
        {
            if (IsRequestAllowed())
            {
                hServiceConnect = HartDeviceDLL.BAHAMA_ConnectByAddr(CSettings.PollAddress, HartDeviceDLL.WAIT_For_Service, CSettings.NumRetries);
            }

            return false;
        }
        #endregion Hart Ip Handling
    }
}
