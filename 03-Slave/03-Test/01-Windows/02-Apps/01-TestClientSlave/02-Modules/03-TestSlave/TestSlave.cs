/*
 *          File: TestClient.cs (CTestSlave)
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
using BaTestHart.CommonHelpers;
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
            public const string TEST_CPP_SLAVE = "C++ Hart Slave Test";
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

    internal static class CTestSlave
    {
        #region Private Data
        private static FrmTestClient? mo_parent;
        #endregion Private Data

        #region Internal Data
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
        internal static byte[] BytesOfData = new byte[255];
        internal static bool IsComPortOpen = false;
        internal static HartDeviceDLL.TY_ConstDataHart ConstDataHart = new HartDeviceDLL.TY_ConstDataHart();
        internal static HartDeviceDLL.TY_DynDataHart DynDataHart = new HartDeviceDLL.TY_DynDataHart();
        internal static HartDeviceDLL.TY_StatDataHart StatDataHart = new HartDeviceDLL.TY_StatDataHart();
        internal static HartDeviceDLL.TY_ConstDataHart ConstDataBackup = new HartDeviceDLL.TY_ConstDataHart();
        internal static HartDeviceDLL.TY_DynDataHart DynDataBackup = new HartDeviceDLL.TY_DynDataHart();
        internal static HartDeviceDLL.TY_StatDataHart StatDataBackup = new HartDeviceDLL.TY_StatDataHart();
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        internal static byte[] BytesOfUniqueId = new byte[5];
        #endregion Internal Data

        #region Initialization
        internal static void Init(FrmTestClient f_)
        {
            CTestSlave.ConstDataHart.DevUniqueID = new byte[3];
            CTestSlave.StatDataHart.LongAddress = new byte[5];
            CTestSlave.StatDataHart.ShortTag = new byte[6];
            CTestSlave.StatDataHart.LongTag = new byte[32];
            CTestSlave.StatDataHart.Descriptor = new byte[12];
            CTestSlave.StatDataHart.Message = new byte[24];
            CTestSlave.StatDataHart.TransdSerNum = new byte[3];
            CTestSlave.StatDataHart.FinAssNum = new byte[3];
            CTestSlave.DynDataHart.AddStatus = new byte[HartDeviceDLL.CSizes.ADD_STATUS_LEN];
            CTestSlave.DynDataHart.PrimaryAddStatus = new byte[HartDeviceDLL.CSizes.ADD_STATUS_LEN];
            CTestSlave.DynDataHart.SecondarydAddStatus = new byte[HartDeviceDLL.CSizes.ADD_STATUS_LEN];
            CTestSlave.DynDataHart.AddStatusMask = new byte[HartDeviceDLL.CSizes.ADD_STATUS_LEN];
            CTestSlave.StatDataHart.CountryCode = new byte[2];
            CTestSlave.StatDataHart.HartIpHostName = new byte[64];
            CTestSlave.StatDataHart.HartIpAddress = new byte[64];
            CTestSlave.StatDataHart.HartIpPort = new byte[64];

            BytesOfData = new byte[255];
            mo_parent = f_;
            UpdateSlaveData();
        }
        #endregion Initialization

        #region Internal Properties	  
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
                    HartDeviceDLL.BAHASL_CloseChannel();
                    HartDeviceDLL.BAHASL_StopMonitor();
                    IsComPortOpen = false;
                }

                CDevStatus.CGeneral.ActiveComport = CSettings.ComPort;
                if (HartDeviceDLL.BAHASL_OpenChannel(CSettings.ComPort) == EN_Bool.TRUE8)
                {
                    HartDeviceDLL.BAHASL_StartMonitor();
                    CTestSlave.IsComPortOpen = true;
                }
                else
                {
                    CTestSlave.IsComPortOpen = false;
                }
            }

            if (CTestSlave.IsComPortOpen)
            {
                // Configure connection
                GetSlaveDataHart();
                DLLinterfaceConfig();
                SetSlaveDataHart();
                mo_parent?.SetComPortStatus(FrmTestClient.CComPortStatus.OK);
            }
            else
            {
                CDevStatus.CGeneral.ActiveComport = 0;
                mo_parent?.SetComPortStatus(FrmTestClient.CComPortStatus.FAILED);
            }
        }

        internal static void Terminate()
        {
            if (CTestSlave.IsComPortOpen)
            {
                HartDeviceDLL.BAHASL_CloseChannel();
                CTestSlave.IsComPortOpen = false;
            }

            CDevStatus.CGeneral.ActiveComport = 0;
        }
        #endregion

        #region HartSlaveDLL Data Exchange
        #region Direct Helpers
        private static void DLLinterfaceConfig()
        {
            StatDataHart.BaudRate = CSettings.BaudRate switch
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

            StatDataHart.ComPort = CDevStatus.CGeneral.ActiveComport;
            StatDataHart.PollAddress = CSettings.PollAddress;
            StatDataHart.NumRequestPreambles = CSettings.NumReqPreambles;
            StatDataHart.NumResponsePreambles = CSettings.NumRspPreambles;
        }

        internal static void SetBurstMode(EN_Bool mode_)
        {
            HartDeviceDLL.BAHASL_GetStatDataHart(ref StatDataHart);
            StatDataHart.BurstMode = mode_;
            HartDeviceDLL.BAHASL_SetStatDataHart(ref StatDataHart);
        }

        internal static void SetHartEnabled(EN_Bool enabled_)
        {
            HartDeviceDLL.BAHASL_GetStatDataHart(ref StatDataHart);
            StatDataHart.HartEnabled = enabled_;
            HartDeviceDLL.BAHASL_SetStatDataHart(ref StatDataHart);
        }

        internal static void SetWriteProtected(EN_Bool protected_)
        {
            HartDeviceDLL.BAHASL_GetStatDataHart(ref StatDataHart);
            StatDataHart.WriteProtected = protected_;
            HartDeviceDLL.BAHASL_SetStatDataHart(ref StatDataHart);
        }

        internal static void SetMalFunction(EN_Bool mal_function_)
        {
            HartDeviceDLL.BAHASL_GetDynDataHart(ref DynDataHart);
            if (mal_function_ == EN_Bool.TRUE8)
            {
                // Set the flag
                DynDataHart.DeviceStatusPrimary = (byte)(DynDataHart.DeviceStatusPrimary | 0x80);
                DynDataHart.DeviceStatusSecondary = (byte)(DynDataHart.DeviceStatusSecondary | 0x80);
            }
            else
            {
                // Clear the flag
                DynDataHart.DeviceStatusPrimary = (byte)(DynDataHart.DeviceStatusPrimary & ~0x80);
                DynDataHart.DeviceStatusSecondary = (byte)(DynDataHart.DeviceStatusSecondary & ~0x80);
            }

            HartDeviceDLL.BAHASL_SetDynDataHart(ref DynDataHart);
        }

        internal static void SetMoreStatus(EN_Bool more_status_)
        {
            HartDeviceDLL.BAHASL_GetDynDataHart(ref DynDataHart);
            if (more_status_ == EN_Bool.TRUE8)
            {
                // Set the flag
                DynDataHart.DeviceStatusPrimary = (byte)(DynDataHart.DeviceStatusPrimary | 0x10);
                DynDataHart.DeviceStatusSecondary = (byte)(DynDataHart.DeviceStatusSecondary | 0x10);
            }
            else
            {
                // Clear the flag
                DynDataHart.DeviceStatusPrimary = (byte)(DynDataHart.DeviceStatusPrimary & ~0x10);
                DynDataHart.DeviceStatusSecondary = (byte)(DynDataHart.DeviceStatusSecondary & ~0x10);
            }

            HartDeviceDLL.BAHASL_SetDynDataHart(ref DynDataHart);
        }

        internal static void GetSlaveDataHart()
        {
            HartDeviceDLL.BAHASL_GetConstDataHart(ref ConstDataHart);
            HartDeviceDLL.BAHASL_GetDynDataHart(ref DynDataHart);
            HartDeviceDLL.BAHASL_GetStatDataHart(ref StatDataHart);
        }

        internal static void SetSlaveDataHart()
        {
            HartDeviceDLL.BAHASL_SetConstDataHart(ref ConstDataHart);
            HartDeviceDLL.BAHASL_SetDynDataHart(ref DynDataHart);
            HartDeviceDLL.BAHASL_SetStatDataHart(ref StatDataHart);
            // Make a backup copy
            ConstDataBackup = ConstDataHart;
            StatDataBackup = StatDataHart;
            DynDataBackup = DynDataHart;
        }
        #endregion Direct Helpers

        #region Update Slave Data
        internal static void UpdateSlaveData()
        {
            if (mo_parent == null)
            {
                return;
            }

            // Get data
            GetSlaveDataHart();

            // Insert values from test parent
            // Identifier
            ConstDataHart.ExpandedDeviceType = WordFromTxtBox(mo_parent.TxtDevTypeTable1);
            ConstDataHart.DeviceRevision = ByteFromTxtBox(mo_parent.TxtDevRev);
            ConstDataHart.SoftwRevision = ByteFromTxtBox(mo_parent.TxtSwRev);
            ConstDataHart.HwRevAndSigCode = Byte5and3fromTxtBoxes(mo_parent.TxtHwRev, mo_parent.TxtSignalTable10);
            ConstDataHart.Flags = ByteFromTxtBox(mo_parent.TxtFlagsTable11);
            ThreeBytesFromTxtBox(mo_parent.TxtDevUniqueID, ConstDataHart.DevUniqueID);
            ConstDataHart.ExtendedManuCode = WordFromTxtBox(mo_parent.TxtManufacturerTable8);
            ConstDataHart.ExtendedLabelCode = WordFromTxtBox(mo_parent.TxtDistributorTable8);
            ConstDataHart.DeviceProfile = ByteFromTxtBox(mo_parent.TxtProfileTable57);
            // Device
            TextBytesFromTxtBox(mo_parent.TxtLongTag, StatDataHart.LongTag, 32);
            PackedASCIIbytesFromTxtBox(mo_parent.TxtShortTag, StatDataHart.ShortTag, 8);
            PackedASCIIbytesFromTxtBox(mo_parent.TxtDescriptor, StatDataHart.Descriptor, 16);
            StatDataHart.Day = ByteFromTxtBox(mo_parent.TxtDay);
            StatDataHart.Month = ByteFromTxtBox(mo_parent.TxtMonth);
            StatDataHart.Year = (byte)(WordFromTxtBox(mo_parent.TxtYear) - 1900);
            PackedASCIIbytesFromTxtBox(mo_parent.TxtMessage, StatDataHart.Message, 32);
            ThreeBytesFromTxtBox(mo_parent.TxtFinAssNum, StatDataHart.FinAssNum);
            TextBytesFromTxtBox(mo_parent.TxtCountryLetters, StatDataHart.CountryCode, 2);
            if (mo_parent.ChkSIunitsOnly.Checked)
            {
                StatDataHart.SiUnitsOnly = 1;
            }
            else
            {
                StatDataHart.SiUnitsOnly = 0;
            }
            mo_parent.ChkSIunitsOnly.BackColor = SystemColors.Window;
            // Transducer
            ThreeBytesFromTxtBox(mo_parent.TxtTransdSerNum, StatDataHart.TransdSerNum);
            ConstDataHart.TransdUnit = ByteFromCmbBox(mo_parent.CmbTransdUnit);
            ConstDataHart.TransdUpperLimit = FloatFromTxtBox(mo_parent.TxtUpperLimit);
            ConstDataHart.TransdLowerLimit = FloatFromTxtBox(mo_parent.TxtLowerLimit);
            ConstDataHart.TransdMinSpan = FloatFromTxtBox(mo_parent.TxtMinSpan);
            switch (mo_parent.CmbAlmSelect.SelectedIndex)
            {
                case 0: // Hi
                    StatDataHart.AlmSelTable6 = 0;
                    break;
                case 1: // Lo
                    StatDataHart.AlmSelTable6 = 1;
                    break;
                case 2: // Last = 239
                    StatDataHart.AlmSelTable6 = 239;
                    break;
                case 3: // None = 250
                    StatDataHart.AlmSelTable6 = 250;
                    break;
            }
            mo_parent.CmbAlmSelect.BackColor = SystemColors.Window;
            switch (mo_parent.CmbTFunction.SelectedIndex)
            {
                case 0: // Linear
                    StatDataHart.TfuncTable3 = 0;
                    break;
                case 1: // Sqr
                    StatDataHart.TfuncTable3 = 1;
                    break;
            }
            mo_parent.CmbTFunction.BackColor = SystemColors.Window;
            StatDataHart.RangeUnit = ByteFromCmbBox(mo_parent.CmbRangeUnit);
            StatDataHart.UpperRange = FloatFromTxtBox(mo_parent.TxtUpperRange);
            StatDataHart.LowerRange = FloatFromTxtBox(mo_parent.TxtLowerRange);
            StatDataHart.Damping = FloatFromTxtBox(mo_parent.TxtDamping);
            switch (mo_parent.CmbWrProt.SelectedIndex)
            {
                case 0: // Off
                    StatDataHart.WrProtCode = 0;
                    StatDataHart.WriteProtected = EN_Bool.FALSE8;
                    break;
                case 1: // On
                    StatDataHart.WrProtCode = 1;
                    StatDataHart.WriteProtected = EN_Bool.TRUE8;
                    break;
                case 2: // None = 250
                    StatDataHart.WrProtCode = 250;
                    StatDataHart.WriteProtected = EN_Bool.FALSE8;
                    break;
            }
            mo_parent.CmbWrProt.BackColor = SystemColors.Window;
            StatDataHart.ChanFlagsTable26 = BinaryFromTxtBox(mo_parent.TxtChanFlags);
            // Device Variables
            // - Percent
            StatDataHart.DevVarPercent.Class = ByteFromTxtBox(mo_parent.TxtPerClass);
            DynDataHart.PercValue = FloatFromTxtBox(mo_parent.TxtPerValue);
            DynDataHart.PercValueStatus = DevVarStatusFromRadButtons(mo_parent.RadPerGood, mo_parent.RadPercBad);
            // - Current
            StatDataHart.DevVarCurrent.Class = ByteFromTxtBox(mo_parent.TxtCurClass);
            DynDataHart.CurrValue = FloatFromTxtBox(mo_parent.TxtCurValue);
            DynDataHart.CurrValueStatus = DevVarStatusFromRadButtons(mo_parent.RadCurGood, mo_parent.RadCurBad);
            // PV1
            StatDataHart.DevVarPV1.Class = ByteFromTxtBox(mo_parent.TxtPV1class);
            DynDataHart.PV1value = FloatFromTxtBox(mo_parent.TxtPV1value);
            StatDataHart.DevVarPV1.UnitsCode = ByteFromCmbBox(mo_parent.CmbPV1unit);
            DynDataHart.PV1valueStatus = DevVarStatusFromRadButtons(mo_parent.RadPV1good, mo_parent.RadPV1bad);
            // PV2
            StatDataHart.DevVarPV2.Class = ByteFromTxtBox(mo_parent.TxtPV2class);
            DynDataHart.PV2value = FloatFromTxtBox(mo_parent.TxtPV2value);
            StatDataHart.DevVarPV2.UnitsCode = ByteFromCmbBox(mo_parent.CmbPV2unit);
            DynDataHart.PV2valueStatus = DevVarStatusFromRadButtons(mo_parent.RadPV2good, mo_parent.RadPV2bad);
            // PV3
            StatDataHart.DevVarPV3.Class = ByteFromTxtBox(mo_parent.TxtPV3class);
            DynDataHart.PV3value = FloatFromTxtBox(mo_parent.TxtPV3value);
            StatDataHart.DevVarPV3.UnitsCode = ByteFromCmbBox(mo_parent.CmbPV3unit);
            DynDataHart.PV3valueStatus = DevVarStatusFromRadButtons(mo_parent.RadPV3good, mo_parent.RadPV3bad);
            // PV4
            StatDataHart.DevVarPV4.Class = ByteFromTxtBox(mo_parent.TxtPV4class);
            DynDataHart.PV4value = FloatFromTxtBox(mo_parent.TxtPV4value);
            StatDataHart.DevVarPV4.UnitsCode = ByteFromCmbBox(mo_parent.CmbPV4unit);
            DynDataHart.PV4valueStatus = DevVarStatusFromRadButtons(mo_parent.RadPV4good, mo_parent.RadPV4bad);
            // Additional status
            DynDataHart.AddStatus[0] = BinaryFromTxtBox(mo_parent.TxtAddDev0);
            DynDataHart.AddStatus[1] = BinaryFromTxtBox(mo_parent.TxtAddDev1);
            DynDataHart.AddStatus[2] = BinaryFromTxtBox(mo_parent.TxtAddDev2);
            DynDataHart.AddStatus[3] = BinaryFromTxtBox(mo_parent.TxtAddDev3);
            DynDataHart.AddStatus[4] = BinaryFromTxtBox(mo_parent.TxtAddDev4);
            DynDataHart.AddStatus[5] = BinaryFromTxtBox(mo_parent.TxtAddDev5);
            DynDataHart.AddStatus[6] = BinaryFromTxtBox(mo_parent.TxtExtDevStatus);
            DynDataHart.AddStatus[7] = 0;
            DynDataHart.AddStatus[8] = BinaryFromTxtBox(mo_parent.TxtAddDevStandard0);
            DynDataHart.AddStatus[9] = BinaryFromTxtBox(mo_parent.TxtAddDevStandard1);
            DynDataHart.AddStatus[10] = BinaryFromTxtBox(mo_parent.TxtAddDevStaturated);
            DynDataHart.AddStatus[11] = BinaryFromTxtBox(mo_parent.TxtAddDevStandard2);
            DynDataHart.AddStatus[12] = BinaryFromTxtBox(mo_parent.TxtAddDevStandard3);
            DynDataHart.AddStatus[13] = BinaryFromTxtBox(mo_parent.TxtAddDevFixed);
            for (int i = 14; i < HartDeviceDLL.CSizes.ADD_STATUS_LEN; i++)
            {
                DynDataHart.AddStatus[i] = 0;
            }

            FillByteArrayFromString(ref StatDataHart.HartIpHostName, mo_parent.TxtHartIpHostName.Text, 64);
            mo_parent.TxtHartIpHostName.BackColor = SystemColors.Window;
            FillByteArrayFromString(ref StatDataHart.HartIpAddress, mo_parent.TxtHartIpAddress.Text, 64);
            mo_parent.TxtHartIpAddress.BackColor = SystemColors.Window;
            FillByteArrayFromString(ref StatDataHart.HartIpPort, mo_parent.TxtHartIpPort.Text, 64);
            mo_parent.TxtHartIpPort.BackColor = SystemColors.Window;
            if (mo_parent.ChkHartIpUseAddress.Checked )
            {
                StatDataHart.HartIpUseAddress = EN_Bool.TRUE8;
            }
            else
            {
                StatDataHart.HartIpUseAddress = EN_Bool.FALSE8;
            }

            StatDataHart.PollAddress = CSettings.PollAddress;

            // Set data
            SetSlaveDataHart();
            mo_parent.butMnuUpdateData.BackColor = Color.Transparent;
        }

        internal static byte DevVarStatusFromRadButtons(RadioButton rad_good_, RadioButton rad_bad_)
        {
            rad_good_.BackColor = SystemColors.Window;
            rad_bad_.BackColor = SystemColors.Window;
            if (rad_good_.Checked)
            {
                return 0xc0;
            }

            return 0x30;
        }

        internal static void PackedASCIIbytesFromTxtBox(TextBox text_box_, byte[] buffer_, byte len_)
        {
            text_box_.BackColor = SystemColors.Window;
            CHelpers.CFromString.GetPackedASCIIbytes(ref buffer_[0], text_box_.Text, len_);
        }

        internal static void TextBytesFromTxtBox(TextBox text_box_, byte[] buffer_, byte len_)
        {
            text_box_.BackColor = SystemColors.Window;
            CHelpers.CFromString.GetTextBytes(ref buffer_[0], text_box_.Text, len_);
        }

        internal static void ThreeBytesFromTxtBox(TextBox text_box_, byte[] buffer_)
        {
            text_box_.BackColor = SystemColors.Window;
            Array.Copy(CHelpers.CFromString.ThreeBytes(text_box_.Text), buffer_, 3);
        }

        internal static byte Byte5and3fromTxtBoxes(TextBox text_box_5bit_, TextBox text_box_3bit_)
        {
            text_box_5bit_.BackColor = SystemColors.Window;
            text_box_3bit_.BackColor = SystemColors.Window;
            byte left_5_bit = (byte)(CHelpers.CFromString.Byte(text_box_5bit_.Text) << 3);
            return (byte)(left_5_bit | (byte)(CHelpers.CFromString.Byte(text_box_3bit_.Text) & 0x07));
        }

        internal static float FloatFromTxtBox(TextBox text_box_)
        {
            text_box_.BackColor = SystemColors.Window;
            return CHelpers.CFromString.Float(text_box_.Text);
        }

        internal static ushort WordFromTxtBox(TextBox text_box_)
        {
            text_box_.BackColor = SystemColors.Window;
            return CHelpers.CFromString.Word(text_box_.Text);
        }

        internal static byte ByteFromCmbBox(ComboBox combo_box_)
        {
            combo_box_.BackColor = SystemColors.Window;
            return (byte)combo_box_.SelectedIndex;
        }

        internal static byte ByteFromTxtBox(TextBox text_box_)
        {
            text_box_.BackColor = SystemColors.Window;
            return CHelpers.CFromString.Byte(text_box_.Text);
        }

        internal static byte BinaryFromTxtBox(TextBox text_box_)
        {
            text_box_.BackColor = SystemColors.Window;
            return CHelpers.CFromString.Binary(text_box_.Text);
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

        internal static ulong Get64bitFromFormattedAddrString(string addr_)
        {
            ulong u = 0;
            string[] addr = addr_.Split(new char[] { '.' });
            foreach (string s in addr)
            {
                u *= 256;
                u += Convert.ToByte(s);
            }

            return u;
        }

        #endregion Update Slave Data

        #region Dynamic Data Handling
        internal static void UpdateSlaveDynamicData()
        {
            HartDeviceDLL.BAHASL_GetDynDataHart(ref DynDataHart);
            // Do something ?
            HartDeviceDLL.BAHASL_SetDynDataHart(ref DynDataHart);
        }
        #endregion Dynamic Data Handling

        #region Verify for Slave Data Changes
        internal static void VerifySlaveData(ushort command_)
        {
            if (mo_parent == null)
            {
                return;
            }

            // Get data
            GetSlaveDataHart();

            switch (command_)
            {
                case 17:
                    VerifyCmd17(mo_parent);
                    break;
                case 18:
                    VerifyCmd18(mo_parent);
                    break;
                case 19:
                    VerifyCmd19(mo_parent);
                    break;
                case 22:
                    VerifyCmd22(mo_parent);
                    break;
                case 34:
                    VerifyCmd34(mo_parent);
                    break;
                case 35:
                    VerifyCmd35(mo_parent);
                    break;
                case 49:
                    VerifyCmd49(mo_parent);
                    break;
                case 513:
                    VerifyCmd513(mo_parent);
                    break;
            }
        }

        private static void VerifyCmd17(FrmTestClient parent_)
        {
            if (!IsEqual(StatDataHart.Message, StatDataBackup.Message, 24))
            {
                PackedASCIIbytesToTxtBox(parent_.TxtMessage, StatDataHart.Message, 24, parent_);
            }
        }

        private static void VerifyCmd18(FrmTestClient parent_)
        {
            if (!IsEqual(StatDataHart.ShortTag, StatDataBackup.ShortTag, 6))
            {
                PackedASCIIbytesToTxtBox(parent_.TxtShortTag, StatDataHart.ShortTag, 6, parent_);
            }

            if (!IsEqual(StatDataHart.Descriptor, StatDataBackup.Descriptor, 12))
            {
                PackedASCIIbytesToTxtBox(parent_.TxtDescriptor, StatDataHart.Descriptor, 12, parent_);
            }

            if (StatDataHart.Day != StatDataBackup.Day)
            {
                ByteToTxtBox(parent_.TxtDay, StatDataHart.Day, parent_);
            }

            if (StatDataHart.Month != StatDataBackup.Month)
            {
                ByteToTxtBox(parent_.TxtMonth, StatDataHart.Month, parent_);
            }

            if (StatDataHart.Year != StatDataBackup.Year)
            {
                ushort year = (ushort)(StatDataHart.Year + 1900);
                WordToTxtBox(parent_.TxtYear, year, parent_);
            }
        }

        private static void VerifyCmd19(FrmTestClient parent_)
        {
            if (!IsEqual(StatDataHart.FinAssNum, StatDataBackup.FinAssNum, 3))
            {
                ThreeBytesToTxtBox(parent_.TxtFinAssNum, StatDataHart.FinAssNum, parent_);
            }
        }

        private static void VerifyCmd22(FrmTestClient parent_)
        {
            if (!IsEqual(StatDataHart.LongTag, StatDataBackup.LongTag, 32))
            {
                TextToTxtBox(parent_.TxtLongTag, StatDataHart.LongTag, parent_, 32);
            }
        }

        private static void VerifyCmd34(FrmTestClient parent_)
        {
            if (StatDataHart.Damping != StatDataBackup.Damping)
            {
                FloatToTxtBox(parent_.TxtDamping, StatDataHart.Damping, parent_);
            }
        }

        private static void VerifyCmd35(FrmTestClient parent_)
        {
            if (StatDataHart.RangeUnit != StatDataBackup.RangeUnit)
            {
                UnitToComboBox(parent_.CmbRangeUnit, StatDataHart.RangeUnit, parent_);
            }

            if (StatDataHart.UpperRange != StatDataBackup.UpperRange)
            {
                FloatToTxtBox(parent_.TxtUpperRange, StatDataHart.UpperRange, parent_);
            }

            if (StatDataHart.LowerRange != StatDataBackup.LowerRange)
            {
                FloatToTxtBox(parent_.TxtLowerRange, StatDataHart.LowerRange, parent_);
            }
        }

        private static void VerifyCmd49(FrmTestClient parent_)
        {
            if (!IsEqual(StatDataHart.TransdSerNum, StatDataBackup.TransdSerNum, 3))
            {
                ThreeBytesToTxtBox(parent_.TxtTransdSerNum, StatDataHart.TransdSerNum, parent_);
            }
        }

        private static void VerifyCmd513(FrmTestClient parent_)
        {
            if (!IsEqual(StatDataHart.CountryCode, StatDataBackup.CountryCode, 2))
            {
                TextToTxtBox(parent_.TxtCountryLetters, StatDataHart.CountryCode, parent_, 2);
            }

            if (StatDataHart.SiUnitsOnly != StatDataBackup.SiUnitsOnly)
            {
                if (StatDataHart.SiUnitsOnly == 1)
                {
                    parent_.ChkSIunitsOnly.Checked = true;
                }
                else
                {
                    parent_.ChkSIunitsOnly.Checked = false;
                }

                parent_.ChkSIunitsOnly.BackColor = Color.AntiqueWhite;
                parent_.butMnuUpdateData.BackColor = Color.Transparent;
            }
        }


        // Helpers

        internal static void PackedASCIIbytesToTxtBox(TextBox text_box_, byte[] buffer_, byte len_, FrmTestClient parent_)
        {
            text_box_.Text = CHelpers.CFromBytes.GetPackedASCIItext(buffer_, len_);
            text_box_.BackColor = Color.AntiqueWhite;
            parent_.butMnuUpdateData.BackColor = Color.Transparent;
        }

        internal static void ByteToTxtBox(TextBox text_box_, byte byte_, FrmTestClient parent_)
        {
            text_box_.Text = byte_.ToString();
            text_box_.BackColor = Color.AntiqueWhite;
            parent_.butMnuUpdateData.BackColor = Color.Transparent;
        }

        internal static void WordToTxtBox(TextBox text_box_, ushort word_, FrmTestClient parent_)
        {
            text_box_.Text = word_.ToString();
            text_box_.BackColor = Color.AntiqueWhite;
            parent_.butMnuUpdateData.BackColor = Color.Transparent;
        }

        internal static void ThreeBytesToTxtBox(TextBox text_box_, byte[] bytes_, FrmTestClient parent_)
        {
            ulong u = (ulong)((bytes_[0] << 16) + (bytes_[1] << 8) + bytes_[2]);
            text_box_.Text = u.ToString();
            text_box_.BackColor = Color.AntiqueWhite;
            parent_.butMnuUpdateData.BackColor = Color.Transparent;
        }

        internal static void FloatToTxtBox(TextBox text_box_, float value_, FrmTestClient parent_)
        {
            text_box_.Text = CHelpers.CFormat.Number(5, value_);
            text_box_.BackColor = Color.AntiqueWhite;
            parent_.butMnuUpdateData.BackColor = Color.Transparent;
        }

        internal static void UnitToComboBox(ComboBox combo_box_, byte byte_, FrmTestClient parent_)
        {
            combo_box_.SelectedIndex = byte_;
            combo_box_.BackColor = Color.AntiqueWhite;
            parent_.butMnuUpdateData.BackColor = Color.Transparent;
        }

        internal static void TextToTxtBox(TextBox text_box_, byte[] bytes_, FrmTestClient parent_, byte len_)
        {
            StringBuilder sb = new StringBuilder(len_);
            sb.Length = len_;

            HartDeviceDLL.BAHA_PickString(sb, len_, 0, ref bytes_[0]);
            text_box_.Text = sb.ToString();
            text_box_.BackColor = Color.AntiqueWhite;
            parent_.butMnuUpdateData.BackColor = Color.Transparent;
        }
        private static bool IsEqual(byte[] arr1_, byte[] arr2_, ushort len_)
        {
            for (int i = 0; i < len_; i++)
            {
                if (arr1_[i] != arr2_[i])
                    return false;
            }

            return true;
        }
        #endregion Verify for Slave Data Changes
        #endregion HartSlaveDLL Data Exchange

        #region Helper
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

        #endregion
    }
}
