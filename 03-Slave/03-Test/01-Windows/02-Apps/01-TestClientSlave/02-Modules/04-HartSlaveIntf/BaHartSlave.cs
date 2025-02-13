/*
 *          File: BaHartSlave.cs (HartDeviceDLL)
 *                The module contains all declarations that are required to 
 *                interface to the test DLL (BaHartSlave.dll).
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

#region NameSpaces
using System.Runtime.InteropServices;
using System.Text;
#endregion

namespace BaTestHart
{
    // Interface to the HartDeviceDLL
    internal static class HartDeviceDLL
    {
        #region Internal Constants
        internal const ushort INVALID_SRV_Handle = 0xffff;
        internal const ushort INVALIDserviceHandle = 0xffff;
        internal const byte BABOOLtrue = 1;
        internal const byte BABOOLfalse = 0;
        internal const byte BABITset = 1;
        internal const byte BABITclear = 0;

        // Primary master identifier (address_)
        internal const byte SLVprim = 0;

        // Secondary master identifier (address_)
        internal const byte SLVscnd = 1;
        internal const uint BAUDrate1200 = 1200;
        internal const uint BAUDrate2400 = 2400;
        internal const uint BAUDrate4800 = 4800;
        internal const uint BAUDrate9600 = 9600;
        internal const uint BAUDrate19200 = 19200;
        internal const uint BAUDrate38400 = 38400;

        // Service completion code: Empty service
        internal const byte SRVempty = 0;

        // Service completion code: Device did not respond
        internal const byte SRVnoDevResp = 1;

        // Service completion code: Communication error
        internal const byte SRVcommErr = 2;

        // Service completion code: Invalid handle value
        internal const byte SRVinvalidHandle = 3;

        // Service completion code: Service in progress
        internal const byte SRVinProgress = 4;

        // Service completion code: Successfully finished
        internal const byte SRVsuccessful = 5;

        // Do not wait for completion of service
        internal const byte NO_WAIT_For_Service = 0;

        // Wait for completion of service
        internal const byte WAIT_For_Service = 1;

        // Most significant byte first
        internal const byte MSBfirst = 0;

        // Least significant byte first
        internal const byte LSBfirst = 1;
        internal const byte SLAVEdisabled = 0;
        internal const byte SLAVEenabled = 1;

        // Response code: command_ not implemented
        internal const byte HRTrspCodeCmdNotImpl = 0x40;

        // Cyclic data_ available
        internal const byte CYCDATok = 0;

        // No cyclic data_ available
        internal const byte CYCDATnoData = 1;

        // No of bytes of cyclic data_
        internal const byte CYCLIC_DATA_SIZE = 200;
        #endregion Public Constants

        #region Functions of the DLL
        #region Channel handling
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern EN_Bool BAHASL_OpenChannel(ushort com_port_);
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHASL_CloseChannel();
        #endregion

        #region Configuration
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHASL_GetConstDataHart(ref TY_ConstDataHart const_data_hart_);
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHASL_SetConstDataHart(ref TY_ConstDataHart const_data_hart_);
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHASL_GetDynDataHart(ref TY_DynDataHart dyn_data_hart_);
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHASL_SetDynDataHart(ref TY_DynDataHart dyn_data_hart_);
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHASL_GetStatDataHart(ref TY_StatDataHart stat_data_hart_);
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHASL_SetStatDataHart(ref TY_StatDataHart stat_data_hart_);
        #endregion

        #region Information
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHASL_GetHartIpStatus();
        #endregion

        #region Service Handling
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern byte BAHASL_WasCommandReceived();
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHASL_ExecuteCommandInterpreter();
        #endregion Service Handling

        #region Coding/Decoding

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutByte(byte data_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutWord(ushort data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutInt24(uint data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutDWord(uint data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutFloat(float data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutDouble(double data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutPackedASCII(StringBuilder sb_, byte len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutOctets(ref byte data_source_, byte len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutString(StringBuilder sb_, byte len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern byte BAHA_PickByte(byte offset_, ref byte data_ref_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHA_PickWord
            (byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern uint BAHA_PickInt24(byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern uint BAHA_PickDWord(byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern float BAHA_PickFloat(byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern double BAHA_PickDouble(byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PickPackedASCII(StringBuilder sb_, byte string_len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PickString(StringBuilder sb_, byte string_len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PickOctets(ref byte octets_ref_, byte max_num_octets_, byte offset_, ref byte data_ref_);
        #endregion Coding/Decoding

        #region Monitoring
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHASL_StartMonitor();
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHASL_StopMonitor();
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern EN_Bool BAHASL_GetMonitorData(ref TY_MonFrame mon_frame_);
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHASL_GetMonitorStatus();
        [DllImport("BaHartSlave.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHASL_GetMonitorAddData(ref byte data_);
        #endregion Monitoring 
        #endregion

        #region Structure Definitions
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // Const Data
        internal struct TY_DevVarData
        {
            // Cmd 0
            internal byte Code;
            internal byte Class;
            internal byte UnitsCode;
            internal byte Status;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // Const Data
        internal struct TY_ConstDataHart
        {
            // Cmd 0
            internal byte Magic;
            internal ushort ExpandedDeviceType;
            internal byte HartRevision;
            internal byte DeviceRevision;
            internal byte SoftwRevision;
            internal byte HwRevAndSigCode;
            internal byte Flags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            internal byte[] DevUniqueID;
            internal byte MaxNumDevVars;
            internal ushort ExtendedManuCode;
            internal ushort ExtendedLabelCode;
            internal byte DeviceProfile;
            internal byte TransdUnit;
            internal float TransdUpperLimit;
            internal float TransdLowerLimit;
            internal float TransdMinSpan;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // Dynamic Data
        internal struct TY_DynDataHart
        {
            internal ushort ConfigChangeCounter;
            internal byte ExtendedDevStatus;
            internal float PV1value;
            internal float PV2value;
            internal float PV3value;
            internal float PV4value;
            internal float PercValue;
            internal float CurrValue;
            internal byte PV1valueStatus;
            internal byte PV2valueStatus;
            internal byte PV3valueStatus;
            internal byte PV4valueStatus;
            internal byte PercValueStatus;
            internal byte CurrValueStatus;
            internal byte DeviceStatusPrimary;
            internal byte DeviceStatusSecondary;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = HartDeviceDLL.CSizes.ADD_STATUS_LEN)]
            internal byte[] AddStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = HartDeviceDLL.CSizes.ADD_STATUS_LEN)]
            internal byte[] PrimaryAddStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = HartDeviceDLL.CSizes.ADD_STATUS_LEN)]
            internal byte[] SecondarydAddStatus;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = HartDeviceDLL.CSizes.ADD_STATUS_LEN)]
            internal byte[] AddStatusMask;
            internal ulong TimeStamp;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // Used to store the result data_ for a connection
        internal struct TY_StatDataHart
        {
            internal UInt32 BaudRate;
            internal byte ComPort;
            internal byte PollAddress;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            internal byte[] LongAddress;
            internal byte NumRequestPreambles;
            internal byte NumResponsePreambles;
            internal EN_Bool HartEnabled;
            internal EN_Bool BurstMode;
            internal EN_Bool WriteProtected; // Signalling bit
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            internal byte[] ShortTag; // As packed ascii
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal byte[] LongTag;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            internal byte[] Descriptor;
            internal byte Day;
            internal byte Month;
            internal byte Year;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
            internal byte[] Message;
            internal TY_DevVarData DevVarPercent; // 244
            internal TY_DevVarData DevVarCurrent; // 245
            internal TY_DevVarData DevVarPV1;     // 246
            internal TY_DevVarData DevVarPV2;     // 247
            internal TY_DevVarData DevVarPV3;     // 248
            internal TY_DevVarData DevVarPV4;     // 249
            internal TY_DevVarData DevVarNone;    // 250
            internal byte AlmSelTable6;
            internal byte TfuncTable3;
            internal byte RangeUnit;
            internal float UpperRange;
            internal float LowerRange;
            internal float Damping;
            internal byte WrProtCode;
            internal byte ChanFlagsTable26;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            internal byte[] FinAssNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            internal byte[] TransdSerNum;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            internal byte[] CountryCode;
            internal byte SiUnitsOnly;
            // Hart Ip
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal byte[] HartIpHostName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal byte[] HartIpAddress;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal byte[] HartIpPort;
            internal EN_Bool HartIpUseAddress;
            internal EN_Bool HartIpDataChanged;
        }

        #region Monitor
        internal delegate void delHandleMasterMonitorEvent();

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TY_MonFrame
        {
            internal uint StartTime;
            internal uint EndTime;
            internal ushort Len;
            internal EN_Bool FrameStarted;
            // .0: Gap TP, .1: Client Tx, .2: Slave Tx 
            internal byte Detail;
            internal EN_Bool byValidFrame;
            internal EN_Bool byReceiveReady;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal byte[] BytesOfData;
        };

        internal static TY_MonFrame MonFrame;
        #endregion Monitor
        #endregion

        #region Classes
        // Constants for Frame details
        internal static class CSizes
        {
            internal const byte ADD_STATUS_LEN = 25;
        }

        internal static class CFrameDetail
        {
            // Gap time out flag
            internal const byte GAPtimeOutBit = 0x01;

            // Client tx flag
            internal const byte CLIENTtxBit = 0x02;
        }

        // Constants for Addressing Mode
        internal static class CAddressingMode
        {
            // Use long address_
            internal const byte LONGAddress = 0x00;

            // Use short address_
            internal const byte SHORTAddress = 0x02;
        }

        // Frequently used helpers functions
        internal static class BaHelpers
        {
            // Transform the comma in the string to a point
            //     string_: Input string
            //  return: Resulting string
            internal static string CommaToPoint(string string_)
            {
                string v = string_.Replace(",", ".");
                return v;
            }
        }

        // Access of Windows API functions
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "timeGetTime() is a kernel function with fixed m_name")]
        internal static class CWinAPI
        {
            // Windows kernel function
            // return: System time in milliseconds
            [DllImport("winmm.dll", CharSet = CharSet.Ansi)]
            internal static extern uint timeGetTime();
        }
        #endregion
    }
}
