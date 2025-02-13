/*
 *          File: Settings.cs (CSettings)
 *                A .NET component is used to store and read the user 
 *                settings. Nevertheless, the individual settings must 
 *                be assigned to specific functionalities.
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
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
using System.Globalization;
using BaTestHart.Properties;
using System.Drawing;
#endregion Namespaces

namespace BaTestHart
{
    internal static partial class CSettings
    {
        #region Internal Data
        // Start
        internal static byte NumPreambles = 5;
        internal static bool OptionsLsbFirst = false;
        internal static byte InitialMasterRole = 0;

        // Commands
        internal static byte CustomCmd = 0;
        internal static string CustomCmdData = string.Empty;
        internal static string TagNameShort = string.Empty;
        internal static string TagNameLong = string.Empty;
        internal static string Descriptor = string.Empty;
        internal static byte Day = 1;
        internal static byte Month = 1;
        internal static byte Year = 107;
        internal static ushort ExtCmd = 0;
        internal static string ExtCmdData = string.Empty;
        internal static byte RangeUnitIdx = 0;
        internal static float UpperRange = 100.0f;
        internal static float LowerRange = 0.0f;

        // Options
        internal static byte NumRetries = 2;
        internal static byte RetryIfBusy = 0;

        // Hart Ip
        internal static byte HartIpSendKeepAlive = 0;
        #endregion Internal Data  

        #region Internal Methods
        internal static void FetchSpecific()
        {
            // Start
            ViewFrameNumbers = Settings.Default.ViewFrameNumbers;
            ViewPreambles = Settings.Default.ViewPreambles;
            ViewAddressing = Settings.Default.ViewAddressing;
            ViewTiming = Settings.Default.ViewTiming;
            ViewStatusBinary = Settings.Default.ViewStatusBinary;
            ViewDecodedData = Settings.Default.ViewDecodedData;
            ViewShortBursts = Settings.Default.ViewShortBursts;
            ComPort = Settings.Default.ComPort;
            PollAddress = Settings.Default.PollAddress;
            BaudRate = Settings.Default.BaudRate;
            NumPreambles = Settings.Default.NumPreambles;
            InitialMasterRole = Settings.Default.InitialMasterRole;
            OptionsLsbFirst = Settings.Default.OptionsLsbFirst;

            // Tab Commands
            CustomCmd = Settings.Default.CustomCmd;
            CustomCmdData = Settings.Default.CustomCmdData;
            Descriptor = Settings.Default.Descriptor;
            CSettings.Day = Settings.Default.Day;
            CSettings.Month = Settings.Default.Month;
            CSettings.Year = Settings.Default.Year;
            CSettings.ExtCmd = Settings.Default.ExtCmd;
            CSettings.ExtCmdData = Settings.Default.ExtCmdData;
            CSettings.RangeUnitIdx = Settings.Default.RangeUnitIdx;
            CSettings.UpperRange = Settings.Default.UpperRange;
            CSettings.LowerRange = Settings.Default.LowerRange;

            // Tab Options
            NumRetries = Settings.Default.NumRetries;
            RetryIfBusy = Settings.Default.RetryIfBusy;

            // Hart Ip Master
            if (Settings.Default.HartIpSendKeepAlive)
            {
                HartIpSendKeepAlive = 1;
            }
            else
            {
                HartIpSendKeepAlive = 0;
            }
        }

        internal static void PutSpecific()
        {
            // Start
            Settings.Default.NumPreambles = NumPreambles;
            Settings.Default.InitialMasterRole = InitialMasterRole;
            Settings.Default.OptionsLsbFirst = OptionsLsbFirst;

            // Tab Commands
            Settings.Default.CustomCmd = CustomCmd;
            Settings.Default.CustomCmdData = CustomCmdData;
            Settings.Default.Descriptor = CSettings.Descriptor;
            Settings.Default.Day = CSettings.Day;
            Settings.Default.Month = CSettings.Month;
            Settings.Default.Year = CSettings.Year;
            Settings.Default.ExtCmd = CSettings.ExtCmd;
            Settings.Default.ExtCmdData = CSettings.ExtCmdData;
            Settings.Default.RangeUnitIdx = CSettings.RangeUnitIdx;
            Settings.Default.UpperRange = CSettings.UpperRange;
            Settings.Default.LowerRange = CSettings.LowerRange;

            // Tab Options
            Settings.Default.NumRetries = NumRetries;
            Settings.Default.RetryIfBusy = RetryIfBusy;

            // Hart Ip Master
            if (HartIpSendKeepAlive != 0)
            {
                Settings.Default.HartIpSendKeepAlive = true;
            } 
            else
            {
                Settings.Default.HartIpSendKeepAlive = false;
            }
        }
        #endregion Internal Methods
    }
}

