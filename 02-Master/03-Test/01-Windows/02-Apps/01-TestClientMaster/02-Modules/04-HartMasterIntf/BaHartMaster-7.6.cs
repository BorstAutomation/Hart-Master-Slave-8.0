/*
 *          File: BaHartMasterIntf.cs (HartDeviceDLL)
 *                The module contains all declarations that are required to 
 *                interface to the test DLL (BaHartMaster.dll).
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
using System;
using System.Numerics;
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
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern EN_Bool BAHAMA_OpenChannel(ushort com_port_);
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHAMA_CloseChannel();
        #endregion

        #region Configuration
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHAMA_GetConfiguration(ref TY_Configuration config_);
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHAMA_SetConfiguration(ref TY_Configuration config_);
        #endregion

        #region Information
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHAMA_GetHartIpStatus();
        #endregion

        #region Connection
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHAMA_ConnectByAddr(byte address_, byte qos_, byte num_retries_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHAMA_ConnectByShortTag(ref byte data_ref_, byte qos_, byte num_retries_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHAMA_ConnectByLongTag(ref byte data_ref_, byte qos_, byte num_retries_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern int BAHAMA_ConnectByUniqueID(int channel, ref byte data_ref_, byte qos_, byte num_retries_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHAMA_FetchConnection(int service_handle_, ref TY_Connection conn_data_);
        #endregion

        #region Commands
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHAMA_DoCommand(byte command_, byte qos_, ref byte data_ref_, byte data_len_, ref byte bytes_unique_id_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHAMA_DoExtCmd(ushort ext_cmd_, byte qos_, ref byte data_ref_, byte data_len_, ref byte bytes_unique_id_);
        #endregion

        #region Service handling
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern EN_Bool BAHAMA_IsServiceCompleted(int service_handle_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHAMA_FetchConfirmation(int service_handle_, ref TY_Confirmation conf_data_);
        #endregion

        #region Coding/Decoding

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutByte(byte data_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutWord(ushort data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutInt24(uint data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutDWord(uint data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutFloat(float data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutDouble(double data_, byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutPackedASCII(StringBuilder sb_, byte len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutOctets(ref byte data_source_, byte len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PutString(StringBuilder sb_, byte len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern byte BAHA_PickByte(byte offset_, ref byte data_ref_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHA_PickWord(byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern uint BAHA_PickInt24(byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern uint BAHA_PickDWord(byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern float BAHA_PickFloat(byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern double BAHA_PickDouble(byte offset_, ref byte data_ref_, EN_Endian endian_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PickPackedASCII(StringBuilder sb_, byte string_len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PickString(StringBuilder sb_, byte string_len_, byte offset_, ref byte data_ref_);

        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHA_PickOctets(ref byte octets_ref_, byte max_num_octets_, byte offset_, ref byte data_ref_);
        #endregion Coding/Decoding

        #region Monitoring
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHAMA_StartMonitor();
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern void BAHAMA_StopMonitor();
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern EN_Bool BAHAMA_GetMonitorData(ref TY_MonFrame mon_frame_);
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHAMA_GetMonitorStatus();
        [DllImport("BaHartMaster.dll", CharSet = CharSet.Ansi)]
        internal static extern ushort BAHAMA_GetMonitorAddData(ref byte data_);
        #endregion Monitoring 
        #endregion

        #region Structure Definitions
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // Flat structure for the configuration of the HartDeviceDLL
        internal struct TY_Configuration
        {
            // Bitrate as defined in Windows, default: 1200
            internal int Baudrate;

            // Number of preambles used for a request (0..22), default: 5
            internal byte NumPreambles;

            // Number of retries if device response is erroneous, default: 2
            internal byte NumRetries;

            //     0: Do not retry if device is responding with busy code
            // other: Retry the command_ if device is responding with busy code.
            //        The number of retries is reflected in the confirmation as <c>ucUsedRetries</c>
            internal byte RetryIfBusy;

            // 0: Primary master, 1: Secondary master, Default: 0
            internal byte InitialMaster;

            // Addressing mode, 2: Use short address_, other: Use long address_
            internal byte AddressingMode;

            //Hart Ip
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal byte[]     HartIpHostName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal byte[]      HartIpAddress;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
            internal byte[]           HartIpPort;
            internal EN_Bool  HartIpUseAddress;
            internal EN_Bool HartIpDataChanged;
            internal EN_Bool HartIpSendKeepAlive;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // Mainly used by the monitor to get run time information
        internal struct TY_RunTimeInfo
        {
            // Indicates actual master, 0: primary, 1: secondary
            internal byte ActualMaster;

            // Indicates buffering: 0: single characters, 1: behavior like buffered
            internal byte FifoDetected;

            // Mean size of the number of characters received in a block
            internal byte BlockSize;

            // This field is not yet used.
            internal byte Reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // Used to store the result data_ for a connection
        internal struct TY_Connection
        {
            // Manufacturer id as defined by the Hart Communication Foundation
            internal byte ManuId;

            // Vendor's device id
            internal byte DevId;

            // Number of preambles to be sent with a response
            internal byte NumPreambles;

            // command_ set revision number as defined by Hart, 5 or 6 or 7
            internal byte CmdRevNum;

            // Device specific revision code
            internal byte SpecRevCode;

            // Software revision code
            internal byte SwRev;

            // Hardware revision code
            internal byte HwRev;

            // Hart flags
            internal byte HartFlags;

            // Service completion code
            // 0: Not active, 1: No dev response, 2: Some comm err, 3: Invalid handle
            // 4: Service in progress, 5: Successfully completed, 6: Out of resource, 7: Reserved for cmd 31
            internal byte SrvResultCode;

            // command_ specific response code
            internal byte RespCode1;

            // Device status
            internal byte RespCode2;

            // Number of retries to complete service
            internal byte UsedRetries;

            // Device in burst mode indication, 1: Device in burst mode
            internal byte DeviceInBurstMode;

            // Extended device status
            internal byte ExtDevStatus;

            // Configuration changed counter
            internal ushort CfgChCount;

            // Minimum number of preambles for the receiving a request
            internal byte MinNumPreambs;

            // Maximum number of device variables
            internal byte MaxNumDVs;

            // Extended manufacturer ID
            internal ushort ExtManuID;

            // Extended label distributor code
            internal ushort ExtLabDistID;

            // Device profile
            internal byte EDevProfile;

            // The field is not used.
            internal byte Reserved;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
            // Array for the unique identifier
            internal byte[] BytesOfUniqueID;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // Result data_ of a Hart service (e.g. DoCommand)
        internal struct TY_Confirmation
        {
            // Received command_
            internal byte Cmd;

            // command_ specific response code (response byte 1)
            internal byte RespCode1;

            // Device status (response byte 2)
            internal byte RespCode2;

            // Service completion code
            // 0: Not active, 1: No dev response, 2: Some comm err, 3: Invalid handle
            // 4: Service in progress, 5: Successfully completed, 6: Out of resource,
            // 7: Reserved for cmd 31
            internal byte SrvResultCode;

            // Number of retries to complete service
            internal byte UsedRetries;

            // Device in burst mode indication, 1: Device in burst mode
            internal byte DeviceInBurstMode;

            // Duration of the service conduction in milliseconds
            internal short SrvDuration;

            // The 16 bit command_ code (if any)
            internal ushort ExtCommand;

            // This field is not used.
            internal byte Reserved;

            // Length of payload data_
            internal byte DataLen;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            // Byte array for the payload data_
            internal byte[] BytesOfData;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        // General data_ buffer
        internal struct TY_DataBuffer
        {
            // Length of data_
            internal byte DataLen;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 255)]
            // Byte array for the data_
            internal byte[] BytesOfData;
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
                string_.Replace(",", ".");
                return string_;
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
