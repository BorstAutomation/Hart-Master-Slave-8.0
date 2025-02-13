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
using BaTestHart.Properties;
#endregion Namespaces

namespace BaTestHart
{
    internal static partial class CSettings
    {
        #region Internal Data

        internal static bool ViewFrameNumbers = false;
        internal static bool ViewPreambles = false;
        internal static bool ViewAddressing = false;
        internal static bool ViewTiming = false;
        internal static bool ViewStatusBinary = false;
        internal static bool ViewDecodedData = false;
        internal static bool ViewShortBursts = false;
        internal static byte ComPort = 0;
        internal static byte PollAddress = 0;
        internal static byte BaudRate = 1;

        // Options
        internal static bool OptionsTopMost = false;

        // Options (Colors)
        internal static Color MonColLinNum = Color.Black;
        internal static Color MonColTime = Color.SaddleBrown;
        internal static Color MonColHeader = Color.Blue;
        internal static Color MonColData = Color.Black;
        internal static Color MonColJabber = Color.DarkViolet;
        internal static Color MonColGarbage = Color.Purple;
        internal static Color MonColError = Color.Red;
        internal static Color MonColDataSel = Color.DimGray;
        internal static Color MonColBack = Color.Bisque;
        internal static Color MonColPrimMaster = Color.DarkBlue;
        internal static Color MonColScndMaster = Color.SaddleBrown;
        internal static Color MonColWrongData = Color.DarkRed;
        internal static Color MonColRespIndication = Color.DarkKhaki;
        internal static Color StatBarColBkNotRec = Color.LightGreen;
        internal static Color StatBarColBkRecording = Color.LightCoral;
        internal static int ColorSet = 0;

        // Run time control
        internal static bool ModeMonitorFrames = false;

        // Main Form Appearance
        internal static int MainFormTop = 0;
        internal static int MainFormLeft = 0;
        internal static int MainFormHeight = 400;
        internal static int MainFormWidth = 600;

        // Hart Ip
        internal static string HartIpHostName = string.Empty;
        internal static string HartIpAddress = string.Empty;
        internal static string HartIpPort = "5094";
        internal static bool HartIpUseAddress = false;
        internal static bool IsHartIpEnabled = false;
        internal static bool IsHartIpSendKeepAlive = false;
        #endregion Internal Data  

        #region User Colors Data
        private static Color MonColLinNumUser;
        private static Color MonColTimeUser;
        private static Color MonColHeaderUser;
        private static Color MonColDataUser;
        private static Color MonColJabberUser;
        private static Color MonColGarbageUser;
        private static Color MonColErrorUser;
        private static Color MonColDataSelUser;
        private static Color MonColBackUser;
        private static Color MonColPrimMasterUser;
        private static Color MonColScndMasterUser;
        private static Color MonColWrongDataUser;
        private static Color MonColBusyIndicationUser;
        private static Color StatBarColBkNotRecUser;
        private static Color StatBarColBkRecordingUser;
        #endregion User Colors Data

        #region Internal Methods
        internal static void SaveUserColors()
        {
            MonColLinNumUser = MonColLinNum;
            MonColTimeUser = MonColTime;
            MonColHeaderUser = MonColHeader;
            MonColDataUser = MonColData;
            MonColJabberUser = MonColJabber;
            MonColGarbageUser = MonColGarbage;
            MonColErrorUser = MonColError;
            MonColDataSelUser = MonColDataSel;
            MonColBackUser = MonColBack;
            MonColPrimMasterUser = MonColPrimMaster;
            MonColScndMasterUser = MonColScndMaster;
            MonColWrongDataUser = MonColWrongData;
            MonColBusyIndicationUser = MonColRespIndication;
            StatBarColBkNotRecUser = StatBarColBkNotRec;
            StatBarColBkRecordingUser = StatBarColBkRecording;
        }

        internal static void GetColorSet1()
        {
            MonColBack = Color.FromArgb(255, 255, 255);
            MonColPrimMaster = Color.FromArgb(0, 0, 255);
            MonColScndMaster = Color.FromArgb(204, 51, 0);
            MonColData = Color.FromArgb(0, 0, 192);
            MonColWrongData = Color.FromArgb(139, 0, 0);
            MonColHeader = Color.FromArgb(0, 0, 0);
            MonColTime = Color.FromArgb(102, 51, 0);
            MonColLinNum = Color.FromArgb(153, 153, 153);
            MonColJabber = Color.FromArgb(148, 0, 211);
            MonColGarbage = Color.FromArgb(185, 92, 0);
            MonColError = Color.FromArgb(192, 0, 0);
            MonColRespIndication = Color.FromArgb(192, 0, 0);
            MonColDataSel = Color.FromArgb(105, 105, 105);
            StatBarColBkNotRec = Color.FromArgb(220, 220, 220);
            StatBarColBkRecording = Color.FromArgb(208, 181, 159);
        }

        internal static void GetColorSet2()
        {
            MonColBack = Color.FromArgb(0, 0, 0);
            MonColPrimMaster = Color.FromArgb(0, 255, 255);
            MonColScndMaster = Color.FromArgb(128, 255, 128);
            MonColData = Color.FromArgb(255, 255, 255);
            MonColWrongData = Color.FromArgb(255, 128, 64);
            MonColHeader = Color.FromArgb(255, 128, 255);
            MonColTime = Color.FromArgb(255, 255, 0);
            MonColLinNum = Color.FromArgb(224, 224, 224);
            MonColJabber = Color.FromArgb(128, 255, 255); ;
            MonColGarbage = Color.FromArgb(255, 255, 128);
            MonColError = Color.FromArgb(255, 196, 196);
            MonColRespIndication = Color.FromArgb(128, 255, 128);
            MonColDataSel = Color.FromArgb(176, 176, 176);
            StatBarColBkNotRec = Color.FromArgb(144, 238, 144);
            StatBarColBkRecording = Color.FromArgb(240, 128, 128);
        }

        internal static void GetColorSetUser()
        {
            MonColLinNum = MonColLinNumUser;
            MonColTime = MonColTimeUser;
            MonColHeader = MonColHeaderUser;
            MonColData = MonColDataUser;
            MonColJabber = MonColJabberUser;
            MonColGarbage = MonColGarbageUser;
            MonColError = MonColErrorUser;
            MonColDataSel = MonColDataSelUser;
            MonColBack = MonColBackUser;
            MonColPrimMaster = MonColPrimMasterUser;
            MonColScndMaster = MonColScndMasterUser;
            MonColWrongData = MonColWrongDataUser;
            MonColRespIndication = MonColBusyIndicationUser;
            StatBarColBkNotRec = StatBarColBkNotRecUser;
            StatBarColBkRecording = StatBarColBkRecordingUser;
        }

        internal static void Fetch()
        {
            FetchSpecific();

            // Hart
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

            // Options
            OptionsTopMost = Settings.Default.OptionsTopMost;

            // Tab Options (Colors)
            MonColLinNum = Settings.Default.MonColLinNum;
            MonColTime = Settings.Default.MonColTime;
            MonColHeader = Settings.Default.MonColHeader;
            MonColData = Settings.Default.MonColData;
            MonColJabber = Settings.Default.MonColJabber;
            MonColGarbage = Settings.Default.MonColGarbage;
            MonColError = Settings.Default.MonColError;
            MonColDataSel = Settings.Default.MonColDataSel;
            MonColBack = Settings.Default.MonColBack;
            MonColPrimMaster = Settings.Default.MonColPrimMaster;
            MonColScndMaster = Settings.Default.MonColScndMaster;
            MonColWrongData = Settings.Default.MonColWrongData;
            MonColRespIndication = Settings.Default.MonColBusyIndication;
            StatBarColBkNotRec = Settings.Default.StatBarColBkNotRec;
            StatBarColBkRecording = Settings.Default.StatBarColBkRecording;
            ColorSet = Settings.Default.ColorSet;

            // Run time control
            ModeMonitorFrames = Settings.Default.ModeMonitorFrames;

            // Main Form Appearance
            MainFormTop = Settings.Default.MainFormTop;
            MainFormLeft = Settings.Default.MainFormLeft;
            MainFormHeight = Settings.Default.MainFormHeight;
            MainFormWidth = Settings.Default.MainFormWidth;

            // Hart Ip
            HartIpHostName = Settings.Default.HartIpHostName;
            HartIpAddress = Settings.Default.HartIpAddress;
            HartIpPort = Settings.Default.HartIpPort;
            HartIpUseAddress = Settings.Default.HartIpUseAddress;
        }

        internal static void Put()
        {
            // Start
            Settings.Default.ViewFrameNumbers = ViewFrameNumbers;
            Settings.Default.ViewPreambles = ViewPreambles;
            Settings.Default.ViewAddressing = ViewAddressing;
            Settings.Default.ViewTiming = ViewTiming;
            Settings.Default.ViewStatusBinary = ViewStatusBinary;
            Settings.Default.ViewDecodedData = ViewDecodedData;
            Settings.Default.ViewShortBursts = ViewShortBursts;
            Settings.Default.ComPort = ComPort;
            Settings.Default.PollAddress = PollAddress;
            Settings.Default.BaudRate = BaudRate;

            // Options
            Settings.Default.OptionsTopMost = OptionsTopMost;

            // Tab Options (Colors)
            Settings.Default.MonColLinNum = MonColLinNum;
            Settings.Default.MonColTime = MonColTime;
            Settings.Default.MonColHeader = MonColHeader;
            Settings.Default.MonColData = MonColData;
            Settings.Default.MonColJabber = MonColJabber;
            Settings.Default.MonColGarbage = MonColGarbage;
            Settings.Default.MonColError = MonColError;
            Settings.Default.MonColDataSel = MonColDataSel;
            Settings.Default.MonColBack = MonColBack;
            Settings.Default.MonColPrimMaster = MonColPrimMaster;
            Settings.Default.MonColScndMaster = MonColScndMaster;
            Settings.Default.MonColWrongData = MonColWrongData;
            Settings.Default.MonColBusyIndication = MonColRespIndication;
            Settings.Default.StatBarColBkNotRec = StatBarColBkNotRec;
            Settings.Default.StatBarColBkRecording = StatBarColBkRecording;
            Settings.Default.ColorSet = ColorSet;

            // Run time control
            Settings.Default.ModeMonitorFrames = ModeMonitorFrames;

            // Main Form Appearance
            Settings.Default.MainFormTop = MainFormTop;
            Settings.Default.MainFormLeft = MainFormLeft;
            Settings.Default.MainFormHeight = MainFormHeight;
            Settings.Default.MainFormWidth = MainFormWidth;

            // Hart Ip
            Settings.Default.HartIpHostName = HartIpHostName;
            Settings.Default.HartIpAddress = HartIpAddress;
            Settings.Default.HartIpPort = HartIpPort;
            Settings.Default.HartIpUseAddress = HartIpUseAddress;
        }
        #endregion Public Methods
    }
}

