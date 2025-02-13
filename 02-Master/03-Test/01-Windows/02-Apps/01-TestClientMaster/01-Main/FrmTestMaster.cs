/*
 *          File: FrmTestClient.cs (FrmTestClient)
 *                Includes the operation of the user interface and 
 *                all functions necessary to coordinate the additional 
 *                modules.
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
using BaTestHart.DataSyntax;
using BaTestHart.Forms;
using BaTestHart.HartUtils;
using BaTestHart.Properties;
using System.Collections;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
#endregion

namespace BaTestHart
{
    #region Enumerations
    internal enum EN_Bool : byte
    {
        FALSE8 = 0,
        TRUE8 = 1
    }

    internal enum EN_Endian : byte
    {
        MSB_First = 0,  // Big endian (Hart standard)
        LSB_First = 1   // Little endian
    }

    internal enum EN_HartIpStatus : byte
    {
        IDLE = 0,
        INITIALIZING = 1,
        WAIT_CONNECT = 2,
        WAIT_INI_RESPONSE = 3,
        CLIENT_READY = 4,
        WAIT_ALIVE_RESPONSE = 5,
        WAIT_RESPONSE = 6,
        WAIT_CLOSE_RESPONSE = 7,
        SHUTTING_DOWN = 8
    }
    #endregion Enumerations

    internal partial class FrmTestClient : Form
    {
        #region Internal Constants
        internal class CLEDcolor
        {
            internal const int BLACK = 0;
            internal const int YELLOW = 1;
            internal const int GREEN = 2;
            internal const int RED = 3;
        }

        internal class CComPortStatus
        {
            internal const int NEUTRAL = 0;
            internal const int FAILED = 1;
            internal const int OK = 2;
            internal const int UNCERTAIN = 3;
        }

        internal class CMonitorStatus
        {
            internal const int NEUTRAL = 0;
            internal const int CONNECTED = 1;
            internal const int UNCERTAIN = 2;
        }

        internal class CLEDstatus
        {
            internal const int IDLE = 0;
            internal const int YELLOW = 1;
            internal const int RED = 2;
            internal const int BLACK = 3;
            internal const int KEEP_RED = 4;
            internal const int FIRST_BLACK = 5;
        }

        internal class CWidthValues
        {
            internal const int COM_LED = 20;
            internal const int MIN_STAT = 100;
            internal const int ERR_LED = 20;
            internal const int NUM = 60;
        }
        #endregion

        #region Private Data
        private int m_number_frames = 0;
        private int m_last_number_frames = -1;
        private bool m_last_time_valid = false;
        private uint m_last_end_time = 0;
        private int m_last_color_set = -1;
        private ArrayList mo_hart_frames = new ArrayList();
        private bool m_msg_show_yellow = false;
        private bool m_msg_yellow_busy = false;
        private bool m_msg_show_red = false;
        private bool m_msg_red_busy = false;
        private int m_msg_led_status = CLEDstatus.IDLE;
        private int m_led_cycles = 0;
        // diagnostics
        private int m_num_rcv_frames = 0;
        private int m_num_errors = 0;
        private float m_quality = 100.0f;
        private int m_last_num_rcv_frames = -1;
        private int m_last_num_errors = -1;
        private float m_last_quality = -1.0f;
        // decoding
        private ushort m_data_bytes_selected = 0;
        private string m_selected_data_text = string.Empty;
        private bool m_tool_tip_info_changed = false;
        // ip_iface_flags info
        private int m_last_comport_status = -1;
        private int m_last_monitor_status = -1;
        // Hart Ip Handling
        // .15 .14 .13 .12 .11 .10 .09 .08 .07 .06 .05 .04 .03 .02 .01 .00
        //  +   +   +   +--- Status ----+   +-------- Last Error -------+
        //  |   |   +-- tbd
        //  |   +------ tbd
        //  +---------- tbd
        private ushort m_last_ip_iface_flags = 0xffff;
        private EN_HartIpStatus m_ip_status = (EN_HartIpStatus)0xff;
        private EN_HartIpStatus m_ip_last_status = (EN_HartIpStatus)0xff;
        private byte m_ip_last_error = 0x00;
        private byte m_ip_last_last_error = 0xff;
        private bool m_ip_connection_status = false;
        private bool m_ip_last_connection_status = false;
        // Form
        private bool m_loading = true;
        private bool m_no_control_action = false;
        #endregion

        #region ControlLists
        private List<Button> mo_list_of_buttons = new List<Button>();
        private List<CheckBox> mo_list_of_checkboxes = new List<CheckBox>();
        private List<ComboBox> mo_list_of_comboboxes = new List<ComboBox>();
        private List<Button> mo_list_of_hot_buttons = new List<Button>();
        private List<CheckBox> mo_list_of_hot_checkboxes = new List<CheckBox>();

        private void SetupControlLists()
        {
            // Buttons
            mo_list_of_buttons.Clear();
            mo_list_of_buttons.Add(ButSendCmd0short);
            mo_list_of_buttons.Add(butSendCmd1);
            mo_list_of_buttons.Add(ButSendCmd2);
            mo_list_of_buttons.Add(ButSendCmd3);
            mo_list_of_buttons.Add(ButSendCmd12);
            mo_list_of_buttons.Add(ButSendCmd13);
            mo_list_of_buttons.Add(ButSendCmd14);
            mo_list_of_buttons.Add(ButSendCmd15);
            mo_list_of_buttons.Add(ButSendCmd18);
            mo_list_of_buttons.Add(ButSendCmd31);
            mo_list_of_buttons.Add(ButSendCmd35);
            mo_list_of_buttons.Add(ButSendCmd38);
            mo_list_of_buttons.Add(ButSendCmd48);
            mo_list_of_buttons.Add(ButSendUserCmd);
            // Master combo boxes
            mo_list_of_comboboxes.Clear();
            mo_list_of_comboboxes.Add(CmbPollAddress);
            mo_list_of_comboboxes.Add(CmbBaudRate);
            mo_list_of_comboboxes.Add(CmbNumPreambles);
            mo_list_of_comboboxes.Add(CmbMasterRole);
            // Master check boxes
            mo_list_of_checkboxes.Clear();
            // Master hot buttons
            mo_list_of_hot_buttons.Clear();
            mo_list_of_hot_buttons.Add(butSendCmd1);
            mo_list_of_hot_buttons.Add(ButSendCmd2);
            mo_list_of_hot_buttons.Add(ButSendCmd3);
            mo_list_of_hot_buttons.Add(ButSendCmd12);
            mo_list_of_hot_buttons.Add(ButSendCmd13);
            mo_list_of_hot_buttons.Add(ButSendCmd14);
            mo_list_of_hot_buttons.Add(ButSendCmd15);
            mo_list_of_hot_buttons.Add(ButSendCmd18);
            mo_list_of_hot_buttons.Add(ButSendCmd31);
            mo_list_of_hot_buttons.Add(ButSendCmd35);
            mo_list_of_hot_buttons.Add(ButSendCmd38);
            mo_list_of_hot_buttons.Add(ButSendCmd48);
            mo_list_of_hot_buttons.Add(ButSendUserCmd);
            // Master hot check boxes
            mo_list_of_hot_checkboxes.Clear();
        }
        #endregion

        #region Standard Component Management
        internal FrmTestClient()
        {
            InitializeComponent();
        }
        #endregion

        #region Handle Settings
        private void ApplySettings()
        {
            CDevStatus.ApplyingSettings = true;
            switch (CSettings.ColorSet)
            {
                case 1:
                    RadColorSet2.Checked = true;
                    break;
                case 2:
                    RadColorSetUser.Checked = true;
                    break;
                default:
                    RadColorSet1.Checked = true;
                    break;
            }
            if (CSettings.RetryIfBusy == 1)
            {
                ChkRetryIfBusy.Checked = true;
            }
            else
            {
                ChkRetryIfBusy.Checked = false;
            }
            ChkViewPreambles.Checked = CSettings.ViewPreambles;
            ChkViewAddress.Checked = CSettings.ViewAddressing;
            ChkViewTiming.Checked = CSettings.ViewTiming;
            ChkViewStatusBinary.Checked = CSettings.ViewStatusBinary;
            ChkViewDecodedData.Checked = CSettings.ViewDecodedData;
            ChkViewFrameNumbers.Checked = CSettings.ViewFrameNumbers;
            ChkViewShortBursts.Checked = CSettings.ViewShortBursts;
            ChkTopMost.Checked = CSettings.OptionsTopMost;
            CmbNumRetries.SelectedIndex = CSettings.NumRetries;
            CmbComPort.SelectedIndex = CSettings.ComPort;

            if ((CSettings.PollAddress >= 0) && (CSettings.PollAddress < 64))
            {
                CmbPollAddress.SelectedIndex = CSettings.PollAddress;
            }
            else
            {
                CSettings.PollAddress = 0;
                CmbPollAddress.SelectedIndex = 0;
            }

            if ((CSettings.BaudRate >= 0) && (CSettings.BaudRate < 64))
            {
                CmbBaudRate.SelectedIndex = CSettings.BaudRate;
            }
            else
            {
                CSettings.BaudRate = 0;
                CmbBaudRate.SelectedIndex = 0;
            }

            if ((CSettings.NumPreambles >= 0) && (CSettings.NumPreambles < 23))
            {
                CmbNumPreambles.SelectedIndex = CSettings.NumPreambles;
            }
            else
            {
                CSettings.NumPreambles = 5;
                CmbNumPreambles.SelectedIndex = 5;
            }

            if ((CSettings.InitialMasterRole >= 0) && (CSettings.InitialMasterRole < 2))
            {
                CmbMasterRole.SelectedIndex = CSettings.InitialMasterRole;
            }
            else
            {
                CSettings.InitialMasterRole = 0;
                CmbMasterRole.SelectedIndex = 0;
            }

            // Hart Ip
            TxtHartIpHostName.Text = CSettings.HartIpHostName;
            TxtHartIpAddress.Text = CSettings.HartIpAddress;
            TxtHartIpPort.Text = CSettings.HartIpPort.ToString();
            if (CSettings.HartIpUseAddress == true)
            {
                ChkHartIpUseAddress.Checked = true;
            }
            else
            {
                ChkHartIpUseAddress.Checked = false;
            }

            if (CSettings.HartIpSendKeepAlive != 0)
            {
                ChkHartIpSendKeepAlive.Checked = true;
            }
            else
            {
                ChkHartIpSendKeepAlive.Checked = false;
            }

            HandleMonitorMode();

            if (this.TopMost != CSettings.OptionsTopMost)
            {
                this.TopMost = CSettings.OptionsTopMost;
            }

            CDevStatus.ApplyingSettings = false;

            // Update all
            CDevStatus.Init();
        }

        private void WriteSettings()
        {
            CSettings.MainFormHeight = this.Height;
            CSettings.MainFormWidth = this.Width;
            CSettings.MainFormLeft = this.Left;
            CSettings.MainFormTop = this.Top;

            // Copy the user colors to default properties
            CSettings.GetColorSetUser();
            // Put internal values to user defaults
            CSettings.Put();
            // Save settings
            Settings.Default.Save();
        }

        private void ReadSettings()
        {
            // Load settings
            Settings.Default.Reload();
            // Read Settings to internal data
            CSettings.Fetch();
            // Adjust main form size and position
            Height = CSettings.MainFormHeight;
            Width = CSettings.MainFormWidth;
            SetMainWindowStartPosition(CSettings.MainFormLeft, CSettings.MainFormTop);
            // Be sure that user colors are not overwritten by standard sets
            CSettings.SaveUserColors();
            // Adjust control values and internal states
            ApplySettings();
        }
        #endregion

        #region Dynamic Configuration and Status Control
        private void InvalidateConfig()
        {
            CDevStatus.CMonitor.CfgChanged = true;
            CDevStatus.CGeneral.CfgChanged = true;
            CDevStatus.CMonitor.CfgChanged = true;
            CDevStatus.CFramesDisplay.bCfgChanged = true;
            CDevStatus.CColors.CfgChanged = true;
            CDevStatus.CfgChanged = true;
            CDevStatus.CGeneral.ActiveComport = 0xff;
        }
        private void CheckConfig()
        {
            CDevStatus.ConfigInProgress = true;
            if (CDevStatus.AnyCfgChanged)
            {
                if (CDevStatus.CMonitor.CfgChanged)
                {
                    CheckForMonitorClose();
                }

                if (CDevStatus.CGeneral.CfgChanged)
                {
                    CheckClientConfig();
                }

                if (CDevStatus.CMonitor.CfgChanged)
                {
                    CheckForMonitorOpen();
                }

                if (CDevStatus.CFramesDisplay.bCfgChanged)
                {
                    DisplayFrames();
                }

                if (CDevStatus.CColors.CfgChanged)
                {
                    ApplyColors();
                    CDevStatus.CfgChanged = true;
                }

                if (CDevStatus.CTopMost.CfgChanged)
                {
                    this.TopMost = CSettings.OptionsTopMost;
                }

                if (CDevStatus.CfgChanged)
                {
                    CheckControlsDisplay();
                }

                SetStatusInfo();
                CDevStatus.ClearChangeFlags();
            }
            CheckMonitorStatus();
            CDevStatus.ConfigInProgress = false;
        }

        private void CheckForMonitorClose()
        {
            if (CDevStatus.CMonitor.CfgChanged)
            {
                HartDeviceDLL.BAHAMA_StopMonitor();
                CDevStatus.CGeneral.CfgChanged = true;

                HandleMonitorMode();
            }
        }

        private void CheckForMonitorOpen()
        {
            if (CDevStatus.CMonitor.CfgChanged)
            {
                HartDeviceDLL.BAHAMA_StartMonitor();
                HandleMonitorMode();
            }
        }

        private void CheckMonitorConfig()
        {
            if (CDevStatus.CMonitor.CfgChanged)
            {
                if (CDevStatus.CMonitor.ActiveComport != CSettings.ComPort)
                {
                    if (CDevStatus.CMonitor.ActiveComport != 0)
                    {
                        HartDeviceDLL.BAHAMA_StopMonitor();
                        HartDeviceDLL.BAHAMA_CloseChannel();
                        CDevStatus.CMonitor.ActiveComport = 0;
                    }

                    if (CSettings.ComPort != 0)
                    {
                        HartDeviceDLL.BAHAMA_StartMonitor();
                        CDevStatus.CMonitor.ActiveComport = CSettings.ComPort;
                    }
                    CDevStatus.CGeneral.CfgChanged = true;
                }

                HandleMonitorMode();
            }
        }

        private static void CheckClientConfig()
        {
            if (CDevStatus.CGeneral.CfgChanged)
            {
                CTestClient.Configure();
            }
        }

        private void ActivateMonitor()
        {
            if (!CSettings.ModeMonitorFrames)
            {
                CSettings.ModeMonitorFrames = true;
                CDevStatus.CMonitor.CfgChanged = true;
                HandleControlEvent();
            }
        }

        private static bool IsServiceAllowed()
        {
            return CTestClient.IsComPortOpen;
        }

        private static bool IsMonitoringAllowed()
        {
            return true;
        }
        #endregion

        #region Event Procedures
        #region Form
        private void FrmMonitor_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WriteSettings();
            HartDeviceDLL.BAHAMA_StopMonitor();
            CTestClient.Terminate();
            HartDeviceDLL.BAHAMA_CloseChannel();
        }

        private void FrmMonitor_Load(object sender, System.EventArgs e)
        {
            m_loading = true;
            PlaceAndSizeControls();
            ChkHartIpEnabled.Checked = true;
            // Initialize static classes
            CTestClient.Init(this);
            CFrameHelper.Init();
            CDevStatus.Init();
            SetupControlLists();
            ReadSettings();
            InvalidateConfig();
            CheckConfig();
            tabMain.SelectedTab = tabMain.TabPages[0];
            DisplayUniqueID();
            SetMsgLedStatusOff();
            rtxtTestDisplay.BorderStyle = BorderStyle.None;
            tabMain.SelectedTab = tabCommands;
            m_loading = false;
            CheckHartIpVisibility();
            DllUpdateSendKeepAlive();
            // Force tab Hart Ip if selected
            if (CmbComPort.SelectedIndex == 1)
            {
                tabMain.SelectedTab = tabHartIp;
            }
            else
            {
                tabMain.SelectedTab = tabStart;
            }
        }

        private void FrmMonitor_Resize(object sender, System.EventArgs e)
        {
            PlaceAndSizeControls();
        }

        private void FrmMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            CTestClient.Terminate();
        }
        #endregion Form

        #region TabStart Controls
        private void ChkViewPreambles_CheckedChanged(object sender, EventArgs e)
        {
            ViewPreambles();
        }

        private void ChkViewAddress_CheckedChanged(object sender, EventArgs e)
        {
            ViewAddress();
        }

        private void ChkViewTiming_CheckedChanged(object sender, EventArgs e)
        {
            ViewTiming();
        }

        private void ChkViewFrameNumbers_CheckedChanged(object sender, EventArgs e)
        {
            ViewFrameNumbers();
        }

        private void ChkViewDecodedData_CheckedChanged(object sender, EventArgs e)
        {
            ViewDecodedData();
        }

        private void ChkViewStatusBinary_CheckedChanged(object sender, EventArgs e)
        {
            ViewStatusBinary();
        }

        private void ChkViewShortBursts_CheckedChanged(object sender, EventArgs e)
        {
            ViewShortBursts();
        }

        private void CmbComPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbComPort.SelectedIndex == 1)
            {
                lblPreambles.Visible = false;
                CmbNumPreambles.Visible = false;
                HartIpInit();
            }
            else
            {
                lblPreambles.Visible = true;
                CmbNumPreambles.Visible = true;
                m_no_control_action = true;
                ChkHartIpEnabled.Checked = false;
                m_no_control_action = false;
            }

            CSettings.ComPort = (byte)CmbComPort.SelectedIndex;
            CDevStatus.CMonitor.CfgChanged = true;
            CDevStatus.CGeneral.CfgChanged = true;
            CDevStatus.CfgChanged = true;
            HandleControlEvent();
            CheckHartIpVisibility();
        }

        private void CmbPollAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte NewPollAddress;

            if ((CmbPollAddress.SelectedIndex >= 0) && (CmbPollAddress.SelectedIndex < 64))
            {
                NewPollAddress = (byte)(CmbPollAddress.SelectedIndex);
            }
            else
            {
                NewPollAddress = 0;
            }
            if (CSettings.PollAddress != NewPollAddress)
            {
                CTestClient.IsConnected = false;
                CSettings.PollAddress = NewPollAddress;
                CDevStatus.CGeneral.CfgChanged = true;
                CDevStatus.CfgChanged = true;
                HandleControlEvent();
            }
        }

        private void CmbBaudRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte NewBaudRate;

            if ((CmbBaudRate.SelectedIndex >= 0) && (CmbBaudRate.SelectedIndex < 8))
            {
                NewBaudRate = (byte)(CmbBaudRate.SelectedIndex);
            }
            else
            {
                NewBaudRate = 0;
            }
            if (CSettings.BaudRate != NewBaudRate)
            {
                CSettings.BaudRate = NewBaudRate;
                CDevStatus.CGeneral.CfgChanged = true;
                HandleControlEvent();
            }
        }

        private void CmbNumPreambles_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte NewNumPreAmbs;

            if ((CmbNumPreambles.SelectedIndex >= 0) && (CmbNumPreambles.SelectedIndex < 23))
            {
                NewNumPreAmbs = (byte)(CmbNumPreambles.SelectedIndex);
            }
            else
            {
                NewNumPreAmbs = 5;
            }
            if (CSettings.NumPreambles != NewNumPreAmbs)
            {
                CSettings.NumPreambles = NewNumPreAmbs;
                CDevStatus.CGeneral.CfgChanged = true;
                HandleControlEvent();
            }
        }

        private void CmbMasterRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte NewMasterRole;

            if ((CmbMasterRole.SelectedIndex >= 0) && (CmbMasterRole.SelectedIndex < 2))
            {
                NewMasterRole = (byte)(CmbMasterRole.SelectedIndex);
            }
            else
            {
                NewMasterRole = 0;
            }
            if (CSettings.InitialMasterRole != NewMasterRole)
            {
                CSettings.InitialMasterRole = NewMasterRole;
                CDevStatus.CGeneral.CfgChanged = true;
                HandleControlEvent();
            }
        }
        #endregion TabStart Controls

        #region TabHartIp
        private void ChkHartIpUseAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkHartIpUseAddress.Checked == true)
            {
                CSettings.HartIpUseAddress = true;
            }
            else
            {
                CSettings.HartIpUseAddress = false;
            }
        }
        private void TxtHartIpHostName_TextChanged(object sender, EventArgs e)
        {
            CSettings.HartIpHostName = TxtHartIpHostName.Text;
        }

        private void TxtHartIpAddress_TextChanged(object sender, EventArgs e)
        {
            CSettings.HartIpAddress = TxtHartIpAddress.Text;
        }

        private void TxtHartIpPort_TextChanged(object sender, EventArgs e)
        {
            CSettings.HartIpPort = TxtHartIpPort.Text;
        }

        private void ChkHartIpSendKeepAlive_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_loading)
            {
                DllUpdateSendKeepAlive();
            }
        }
        #endregion TabHartIp

        #region TabCommands Controls
        private void ButSendCmd0short_Click(object sender, EventArgs e)
        {
            SendCmd0short();
        }
        private void ButSendCmd1_Click(object sender, EventArgs e)
        {
            SendCmd1();
        }
        private void ButSendCmd2_Click(object sender, EventArgs e)
        {
            SendCmd2();
        }
        private void ButSendCmd3_Click(object sender, EventArgs e)
        {
            SendCmd3();
        }
        private void ButSendCmd12_Click(object sender, EventArgs e)
        {
            SendCmd12();
        }
        private void ButSendCmd13_Click(object sender, EventArgs e)
        {
            SendCmd13();
        }
        private void ButSendCmd14_Click(object sender, EventArgs e)
        {
            SendCmd14();
        }
        private void ButSendCmd15_Click(object sender, EventArgs e)
        {
            SendCmd15();
        }
        private void ButSendCmd18_Click(object sender, EventArgs e)
        {
            SendCmd18();
        }
        private void ButEditCmd18_Click(object sender, EventArgs e)
        {
            Edit_Cmd18();
        }
        private void ButEditCmd31_Click(object sender, EventArgs e)
        {
            Edit_Cmd31();
        }
        private void ButSendCmd31_Click(object sender, EventArgs e)
        {
            SendCmd31();
        }
        private void ButEditUserCmd_Click(object sender, EventArgs e)
        {
            Edit_UserCmd();
        }
        private void ButSendCmd38_Click(object sender, EventArgs e)
        {
            SendCmd38();
        }
        private void ButSendCmd48_Click(object sender, EventArgs e)
        {
            SendCmd48();
        }
        private void ButSendUserCmd_Click(object sender, EventArgs e)
        {
            SendUserCmd();
        }
        #endregion TabCommands Controls

        #region TabOptions Controls
        private void ChkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            MnuFnc_TopMost();
        }

        private void RadColorSet1_CheckedChanged(object sender, EventArgs e)
        {
            CSettings.GetColorSet1();
            CSettings.ColorSet = 0;
            ButEditColors.Enabled = false;
            CDevStatus.CColors.CfgChanged = true;
            HandleControlEvent();
        }

        private void RadColorSet2_CheckedChanged(object sender, EventArgs e)
        {
            CSettings.GetColorSet2();
            CSettings.ColorSet = 1;
            ButEditColors.Enabled = false;
            CDevStatus.CColors.CfgChanged = true;
            HandleControlEvent();
        }

        private void RadColorSetUser_CheckedChanged(object sender, EventArgs e)
        {
            CSettings.GetColorSetUser();
            CSettings.ColorSet = 2;
            ButEditColors.Enabled = true;
            CDevStatus.CColors.CfgChanged = true;
            HandleControlEvent();
        }

        private void CmbNumRetries_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte NewNumRetries = (byte)(CmbNumRetries.SelectedIndex);

            if (CSettings.NumRetries != NewNumRetries)
            {
                CSettings.NumRetries = NewNumRetries;
                CDevStatus.CGeneral.CfgChanged = true;
                HandleControlEvent();
            }
        }

        private void ChkRetryIfBusy_CheckedChanged(object sender, EventArgs e)
        {
            byte NewRetryIfBusy;

            if (ChkRetryIfBusy.Checked == true)
            {
                NewRetryIfBusy = 1;
            }
            else
            {
                NewRetryIfBusy = 0;
            }

            if (CSettings.RetryIfBusy != NewRetryIfBusy)
            {
                CSettings.RetryIfBusy = NewRetryIfBusy;
                CDevStatus.CGeneral.CfgChanged = true;
                HandleControlEvent();
            }
        }

        #endregion TabOptions Controls

        #region Menu Buttons
        private void ButMenu_Click(object sender, EventArgs e)
        {
        }

        private void ButMnuInfo_Click(object sender, EventArgs e)
        {
            MnuFnc_InfoAbout();
        }

        private void ButMnuRecord_Click(object sender, EventArgs e)
        {
            MnuFnc_ModeMonitorFrames();
        }

        private void ButMnuClearDisplay_Click(object sender, EventArgs e)
        {
            MnuFnc_ClearDisplay(sender, e);
        }
        #endregion Menu Buttons

        #region Timer
        private void TimWatch_Tick(object sender, EventArgs e)
        {
            HandleWatchingData();
        }

        private void TimOperating_Tick(object sender, EventArgs e)
        {
            HandleWatchingStatus();
            HandleHartIpStatus();
        }
        #endregion Timer
        #endregion Event Procedures

        #region Monitor Handling
        internal void HandleMonitorData()
        {
            if (HartDeviceDLL.BAHAMA_GetMonitorData(ref HartDeviceDLL.MonFrame) != 0)
            {
                if (CSettings.ModeMonitorFrames != false)
                {
                    if (IsMonitoringAllowed())
                    {
                        CFrameData clFrameData = new CFrameData(ref HartDeviceDLL.MonFrame);
                        mo_hart_frames.Add(clFrameData);
                        if (CmbComPort.SelectedIndex == 1)
                        {
                            if (clFrameData.Request)
                            {
                                HandleMonitorAddData();
                            }
                        }
                        m_number_frames++;
                        UpdateDiagnostics(clFrameData);
                        DisplayFrame(clFrameData, m_number_frames);
                        if ((clFrameData.FrameError) && (clFrameData.MinorError == false))
                        {
                            SetMsgLedColor(CLEDcolor.RED);
                        }
                        else
                        {
                            SetMsgLedColor(CLEDcolor.YELLOW);
                        }
                    }
                }
            }

            CheckMonitorStatus();
        }

        internal void CheckMonitorStatus()
        {
            int monitorStatus = HartDeviceDLL.BAHAMA_GetMonitorStatus();

            if (monitorStatus != m_last_monitor_status)
            {
                m_last_monitor_status = monitorStatus;
                switch (monitorStatus)
                {
                    case 0:
                        SetComPortStatus(CComPortStatus.NEUTRAL);
                        break;
                    case 1:
                        SetComPortStatus(CComPortStatus.OK);
                        break;
                    case 2:
                        SetComPortStatus(CComPortStatus.UNCERTAIN);
                        break;
                }
            }
        }

        internal void HandleMonitorAddData()
        {
            byte[] data = new byte[256];
            ushort len = 0;

            len = HartDeviceDLL.BAHAMA_GetMonitorAddData(ref data[0]);
            rtxIPFrames.Clear();
            if (len == 0)
            {
                rtxIPFrames.AppendText("NoData");
            }
            else
            {
                string s = "Version: " + data[0].ToString() + ", ";
                s = s + "Type: " + data[1].ToString() + ", ";
                s = s + "Message ID: " + data[2].ToString() + ", ";
                s = s + "Status: " + data[3].ToString() + ", ";
                ushort seq_num = HartDeviceDLL.BAHA_PickWord(0, ref data[4], EN_Endian.MSB_First);
                s = s + "Sequence Number: " + seq_num.ToString() + ",  ";
                ushort byte_cnt = HartDeviceDLL.BAHA_PickWord(0, ref data[6], EN_Endian.MSB_First);
                if (byte_cnt > 8)
                {
                    byte_cnt -= 8;
                }
                s = s + "Byte count: " + byte_cnt.ToString();
                rtxIPFrames.AppendText(s);
                if (byte_cnt > 0)
                {
                    s = "\nHart IP Payload (Hex):";
                    rtxIPFrames.AppendText(s);
                    byte[] payload = new byte[256];
                    Array.Copy(data, 8, payload, 0, byte_cnt);
                    string[] simple_dump = CHelpers.CHex.SimpleDump(payload, (byte)byte_cnt, 32);
                    foreach (string dump in simple_dump)
                    {
                        s = "\n" + dump;
                        rtxIPFrames.AppendText(s);
                    }
                }
                else
                {
                    s = "No Payload";
                    rtxIPFrames.AppendText(s);
                }
            }
        }
        #endregion Monitor Handling

        #region Service Handling
        internal void UpdateServiceDisplay(int error_)
        {
            string comment = string.Empty;

            switch (error_)
            {
                case CMALib.CConsts.CTClientECodes.NO_ERROR:
                    break;
                case CMALib.CConsts.CTClientECodes.UNKNOWN_ERROR:
                    comment = "Client: Unknown Error Code!";
                    break;
                case CMALib.CConsts.CTClientECodes.COMM_ERROR:
                    comment = "Client: Communication Failure!";
                    break;
                case CMALib.CConsts.CTClientECodes.EMPTY_SERVICE:
                    comment = "Client: Empty Service Used!";
                    break;
                case CMALib.CConsts.CTClientECodes.INVALID_HANDLE:
                    comment = "Client: Invalid Service Handle!";
                    break;
                case CMALib.CConsts.CTClientECodes.NO_DEVICE_RESPONSE:
                    comment = "Client: No Device Response!";
                    SetMsgLedColor(CLEDcolor.RED);
                    break;
                case CMALib.CConsts.CTClientECodes.CONNECTION_FAILED:
                    comment = "Client: Connection Failed!";
                    SetMsgLedColor(CLEDcolor.RED);
                    break;
            }

            if (comment != string.Empty)
            {
                AddCommentToFrame(comment, CSettings.MonColError);
            }
        }
        #endregion

        #region Frame Display Handling
        private void DisplayFrames()
        {
            if (mo_hart_frames != null)
            {
                if (mo_hart_frames.Count > 0)
                {
                    int iFrameNum = 1;

                    rtxtDisplay.Clear();
                    m_last_time_valid = false;
                    foreach (CFrameData clFrmDat in mo_hart_frames)
                    {
                        DisplayFrame(clFrmDat, iFrameNum);
                        iFrameNum++;
                    }
                }
            }
        }

        private void DisplayFrame(CFrameData frm_dat_, int frame_num_)
        {
            int iSelStart = rtxtDisplay.TextLength;

            frm_dat_.FrameNumber = frame_num_;

            if (m_last_time_valid)
            {
                if (m_last_end_time == 0)
                {
                    frm_dat_.RelStartTime = 0xffffffff;
                }
                else
                {
                    if (frm_dat_.StartTime >= m_last_end_time)
                    {
                        frm_dat_.RelStartTime = frm_dat_.StartTime - m_last_end_time;
                    }
                    else
                    {
                        frm_dat_.RelStartTime = 0;
                    }
                }
            }
            else
            {
                frm_dat_.RelStartTime = 0xffffffff;
                m_last_time_valid = true;
            }

            m_last_end_time = frm_dat_.EndTime;

            // Display
            if (frm_dat_.HeadingCommentString != string.Empty)
            {
                string s = string.Empty;
                if (CSettings.ViewTiming)
                {
                    s += "      ";
                }
                if (CSettings.ViewFrameNumbers)
                {
                    s += "     ";
                }
                s += frm_dat_.HeadingCommentString + "\n";
                rtxtDisplay.AppendText(s);
                rtxtDisplay.Select(iSelStart, rtxtDisplay.TextLength - iSelStart);
                rtxtDisplay.SelectionColor = frm_dat_.CommentColor;
                rtxtDisplay.Select(rtxtDisplay.TextLength, 0);
            }

            iSelStart = rtxtDisplay.TextLength;
            rtxtDisplay.AppendText(frm_dat_.GetDisplayString(frame_num_) + "\n");

            #region Colors
            if (CSettings.ViewFrameNumbers)
            {
                // Frame numbers
                rtxtDisplay.Select(iSelStart, 5);
                rtxtDisplay.SelectionColor = CSettings.MonColLinNum;
            }

            if (CSettings.ViewTiming)
            {
                if (CSettings.ViewFrameNumbers)
                {
                    rtxtDisplay.Select(iSelStart + 5, 6);
                }
                else
                {
                    rtxtDisplay.Select(iSelStart, 6);
                }
                rtxtDisplay.SelectionColor = CSettings.MonColTime;
                if (frm_dat_.OffsSelEndTime > 0)
                {
                    rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelEndTime, frm_dat_.LengthEndTime);
                    rtxtDisplay.SelectionColor = CSettings.MonColTime;
                }
            }

            if (frm_dat_.LengthType > 0)
            {
                rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelType, frm_dat_.LengthType);
                if (frm_dat_.PrimaryMaster != false)
                {
                    rtxtDisplay.SelectionColor = CSettings.MonColPrimMaster;
                }
                else
                {
                    rtxtDisplay.SelectionColor = CSettings.MonColScndMaster;
                }
            }

            if (frm_dat_.LengthPreGarbage > 0)
            {
                rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelPreGarbage, frm_dat_.LengthPreGarbage);
                rtxtDisplay.SelectionColor = CSettings.MonColGarbage;
            }

            if (frm_dat_.OffsSelFraming > 0)
            {
                if (frm_dat_.ValidExtCmd)
                {
                    rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelFraming, frm_dat_.LengthFraming + frm_dat_.LengthExtCmd);
                }
                else
                {
                    rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelFraming, frm_dat_.LengthFraming);
                }
                if ((frm_dat_.PreambleValid) && (!frm_dat_.FrameError))
                {
                    rtxtDisplay.SelectionColor = CSettings.MonColHeader;
                    if (frm_dat_.OffsSelChk > 0)
                    {
                        rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelChk, frm_dat_.LengthChk);
                        rtxtDisplay.SelectionColor = CSettings.MonColHeader;
                    }
                }
                else
                {
                    rtxtDisplay.SelectionColor = CSettings.MonColWrongData;
                    if (frm_dat_.OffsSelChk > 0)
                    {
                        rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelChk, frm_dat_.LengthChk);
                        rtxtDisplay.SelectionColor = CSettings.MonColWrongData;
                    }
                }
            }

            if (frm_dat_.ValidExtCmd)
            {
                rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelExtCmd, frm_dat_.LengthExtCmd);
                rtxtDisplay.SelectionColor = CSettings.MonColHeader;
            }

            if (frm_dat_.LengthData > 0)
            {
                rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelData, frm_dat_.LengthData);
                rtxtDisplay.SelectionColor = CSettings.MonColData;
            }

            if (frm_dat_.LengthResp > 0)
            {
                rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelResp, frm_dat_.LengthResp);
                rtxtDisplay.SelectionColor = CSettings.MonColRespIndication;
            }

            if (frm_dat_.LengthError > 0)
            {
                rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelError, frm_dat_.LengthError);
                rtxtDisplay.SelectionColor = CSettings.MonColError;
            }

            if (frm_dat_.LengthGarbage > 0)
            {
                rtxtDisplay.Select(iSelStart + frm_dat_.OffsSelGarbage, frm_dat_.LengthGarbage);
                rtxtDisplay.SelectionColor = CSettings.MonColGarbage;
            }

            rtxtDisplay.Select(rtxtDisplay.TextLength, 0);
            rtxtDisplay.HideSelection = false;
            #endregion

            if (frm_dat_.TailingCommentString != string.Empty)
            {
                if (frm_dat_.TailingCommentString.StartsWith("Client:"))
                {
                    DisplayTailingComment(frm_dat_.TailingCommentString, CSettings.MonColError);
                }
                else
                {
                    DisplayTailingComment(frm_dat_.TailingCommentString, frm_dat_.TailingCommentColor);
                }
            }
        }

        private void DisplayTailingComment(string comment_, Color sel_col_)
        {
            string s = " ";

            if (CSettings.ViewTiming)
            {
                s += "      ";
            }

            if (CSettings.ViewFrameNumbers)
            {
                s += "     ";
            }

            s += comment_ + "\n";
            int iSelStart = rtxtDisplay.TextLength;
            rtxtDisplay.AppendText(s);
            rtxtDisplay.Select(iSelStart, rtxtDisplay.TextLength - iSelStart);
            rtxtDisplay.SelectionColor = sel_col_;
            rtxtDisplay.Select(rtxtDisplay.TextLength, 0);
        }
        #endregion

        #region Internal Methods
        internal bool IsValidComPort(ushort com_port_)
        {
            bool result;

            if (CTestClient.IsComPortOpen)
            {
                if (CSettings.ComPort == com_port_)
                {
                    // No change happened
                    HartDeviceDLL.BAHAMA_StopMonitor();
                    result = true;
                }
                else
                {
                    HartDeviceDLL.BAHAMA_StopMonitor();
                    HartDeviceDLL.BAHAMA_CloseChannel();
                    // Try to open new com port
                    if (HartDeviceDLL.BAHAMA_OpenChannel(com_port_) == EN_Bool.TRUE8)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            else
            {
                // Try to open new com port
                if (HartDeviceDLL.BAHAMA_OpenChannel(com_port_) == EN_Bool.TRUE8)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            if (result)
            {
                CTestClient.IsComPortOpen = true;
                HartDeviceDLL.BAHAMA_StartMonitor();
                SetModeMonitorFrames();
            }
            else
            {
                CTestClient.IsComPortOpen = false;
                ClearModeMonitorFrames();
            }

            return result;
        }
        #endregion

        #region Status Info Handling
        private void SetStatusInfo()
        {
            string s;
            if (CSettings.ComPort == 0)
            {
                s = "No Com ";
            }
            else if (CSettings.ComPort == 1)
            {
                s = "Hart IP ";
            }
            else
            {
                s = "COM " + (CSettings.ComPort - 1).ToString();
            }

            if (CSettings.ModeMonitorFrames != false)
            {
                s += " | Monitoring active | " + "Switch record off to stop monitoring.";
                StatusInfo.BackColor = CSettings.StatBarColBkRecording;
            }
            else
            {
                s += " | Monitoring stopped | " + "Switch record on to continue monitoring.";
                StatusInfo.BackColor = CSettings.StatBarColBkNotRec;
            }
            StatusInfo.Items[1].Text = s;
        }
        #endregion

        #region Helpers
        void DisplayTitle()
        {
            Text = CMALib.CGlobText.TEST_CPP_MASTER;
        }

        private void HandleControlEvent()
        {
            if (CDevStatus.HandleEvents)
            {
                CheckConfig();
            }
            if (CSettings.ModeMonitorFrames == false)
            {
                SetMsgLedStatusOff();
            }
        }

        private void AddCommentToFrame(string comment_, Color col_comment_)
        {
            int iLastFrameIdx = mo_hart_frames.Count - 1;

            if (iLastFrameIdx >= 0)
            {
                CFrameData? frame = (CFrameData?)mo_hart_frames[iLastFrameIdx];
                if (frame != null)
                {
                    frame.TailingCommentString = comment_;
                    frame.TailingCommentColor = col_comment_;
                    mo_hart_frames[iLastFrameIdx] = frame;
                    DisplayTailingComment(comment_, col_comment_);
                }
            }
        }
        #endregion

        #region Form Management
        private void SetWindowPosition(int left_, int top_)
        {
            int iDeskTopWidth = SystemInformation.VirtualScreen.Width;
            int iDeskTopHeight = SystemInformation.VirtualScreen.Height;

            if ((left_ + Width) > iDeskTopWidth)
            {
                left_ = iDeskTopWidth - Width;
            }
            if ((top_ + Height) > iDeskTopHeight)
            {
                top_ = iDeskTopHeight - Height;
            }
            Left = left_;
            Top = top_;
        }

        #endregion Form Management

        #region Set/Clear Mode Monitor Frames
        private void SetModeMonitorFrames()
        {
            CSettings.ModeMonitorFrames = true;
            CDevStatus.CMonitor.CfgChanged = true;
            HandleControlEvent();
        }

        private void ClearModeMonitorFrames()
        {
            CSettings.ModeMonitorFrames = false;
            CDevStatus.CMonitor.CfgChanged = true;
            HandleControlEvent();
        }
        #endregion Set/Clear Mode Monitor Frames

        #region Tab Commands Functions
        private void SendCmd0short()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd0short();
            }
        }

        private void SendCmd0short_Wait()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd0short_Wait();
            }
        }

        private void SendCmd1()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd1();
            }
        }
        private void SendCmd2()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd2();
            }
        }

        private void SendCmd3()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd3();
            }
        }
        private void SendCmd3_Wait()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd3_Wait();
            }
        }
        private void SendCmd12()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd12();
            }
        }
        private void SendCmd12_Wait()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd12_Wait();
            }
        }

        private void SendCmd13()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd13();
            }
        }
        private void SendCmd13_Wait()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd13_Wait();
            }
        }
        private void SendCmd14()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd14();
            }
        }

        private void SendCmd14_Wait()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd14_Wait();
            }
        }
        internal void SendCmd15()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd15();
            }
        }

        internal void SendCmd15_Wait()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd15_Wait();
            }
        }

        internal void SendCmd18()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd18(CSettings.TagNameShort, CSettings.Descriptor, CSettings.Day, CSettings.Month, CSettings.Year);
            }
        }

        private void SendCmd20_Wait()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd20_Wait();
            }
        }

        internal void SendCmd35()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd35(CSettings.RangeUnitIdx, CSettings.UpperRange, CSettings.LowerRange);
            }
        }

        private void SendCmd38()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd38();
            }
        }

        private void SendCmd48()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd48();
            }
        }

        internal void SendCmd31()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqCmd31();
            }
        }

        internal void SendUserCmd()
        {
            if (IsServiceAllowed())
            {
                ActivateMonitor();
                CTestClient.ReqUserCmd();
            }
        }
        #endregion Tab Commands Functions

        #region Menu Functions
        #region Start
        private void MnuFnc_ClearDisplay(object sender, System.EventArgs e)
        {
            ClearDisplay();
        }

        private void ClearDisplay()
        {
            rtxtDisplay.Clear();
            mo_hart_frames.Clear();
            // CheckMenuVisibility();
            m_last_end_time = 0;
            m_last_time_valid = false;
            m_number_frames = 0;
            SetStatusInfo();
            DisplayTitle();
            m_last_number_frames = -1;
            ClearDiagnostics();
            rtxtTestDisplay.Clear();
        }

        #endregion Start

        #region View
        private void ViewPreambles()
        {
            if (ChkViewPreambles.Checked != false)
            {
                CSettings.ViewPreambles = true;
            }
            else
            {
                CSettings.ViewPreambles = false;
            }

            CDevStatus.CFramesDisplay.bCfgChanged = true;
            HandleControlEvent();
        }

        private void ViewAddress()
        {
            if (ChkViewAddress.Checked != false)
            {
                CSettings.ViewAddressing = true;
            }
            else
            {
                CSettings.ViewAddressing = false;
            }

            CDevStatus.CFramesDisplay.bCfgChanged = true;
            HandleControlEvent();
        }

        private void ViewTiming()
        {
            if (ChkViewTiming.Checked != false)
            {
                CSettings.ViewTiming = true;
            }
            else
            {
                CSettings.ViewTiming = false;
            }

            CDevStatus.CFramesDisplay.bCfgChanged = true;
            HandleControlEvent();
        }

        private void ViewDecodedData()
        {
            if (ChkViewDecodedData.Checked != false)
            {
                CSettings.ViewDecodedData = true;
            }
            else
            {
                CSettings.ViewDecodedData = false;
            }

            CDevStatus.CFramesDisplay.bCfgChanged = true;
            HandleControlEvent();
        }

        private void ViewFrameNumbers()
        {
            if (ChkViewFrameNumbers.Checked != false)
            {
                CSettings.ViewFrameNumbers = true;
            }
            else
            {
                CSettings.ViewFrameNumbers = false;
            }

            CDevStatus.CFramesDisplay.bCfgChanged = true;
            HandleControlEvent();
        }

        private void ViewStatusBinary()
        {
            if (ChkViewStatusBinary.Checked != false)
            {
                CSettings.ViewStatusBinary = true;
            }
            else
            {
                CSettings.ViewStatusBinary = false;
            }
            CDevStatus.CFramesDisplay.bCfgChanged = true;
            HandleControlEvent();
        }

        private void ViewShortBursts()
        {
            if (ChkViewShortBursts.Checked == true)
            {
                CSettings.ViewShortBursts = true;
            }
            else
            {
                CSettings.ViewShortBursts = false;
            }

            CDevStatus.CFramesDisplay.bCfgChanged = true;
            HandleControlEvent();
        }

        #endregion

        #region Options
        private void MnuFnc_TopMost()
        {
            if (ChkTopMost.Checked != false)
            {
                CSettings.OptionsTopMost = true;
            }
            else
            {
                CSettings.OptionsTopMost = false;
            }
            CDevStatus.CTopMost.CfgChanged = true;
            HandleControlEvent();
        }
        #endregion Options

        #region Edit Commands Data
        private void Edit_Cmd18()
        {
            FrmCmd18 f = new FrmCmd18(this);

            f.ShowDialog(this);
        }

        private void Edit_Cmd31()
        {
            FrmCmd31 f = new FrmCmd31(this);

            f.ShowDialog(this);
        }

        private void Edit_Cmd35()
        {
            FrmCmd35 f = new FrmCmd35(this);

            f.ShowDialog(this);
        }

        private void Edit_UserCmd()
        {
            FrmUserCmd f = new FrmUserCmd(this);

            f.ShowDialog(this);
        }
        #endregion

        #region Monitor Mode
        private void MnuFnc_ModeMonitorFrames()
        {
            if (CSettings.ModeMonitorFrames == true)
            {
                CSettings.ModeMonitorFrames = false;
            }
            else
            {
                CSettings.ModeMonitorFrames = true;
            }

            CDevStatus.CMonitor.CfgChanged = true;
            HandleControlEvent();
        }
        #endregion Monitor Mode

        #region Help
        private void MnuFnc_InfoAbout()
        {
            FrmAbout f = new FrmAbout();

            f.lblVersion.Text = "8.0 (Rev 0.1)";
            f.lblDate.Text = "14.2.2025";
            f.ForceLeft = this.Right - f.Width - 5;
            f.ForceTop = this.Top + 70;
            f.ShowDialog();
        }
        #endregion Help
        #endregion Menu Functions

        #region Appearance Control
        private void PlaceAndSizeControls()
        {
            int leftPos;
            int iTotalWidth = StatusInfo.ClientRectangle.Width;
            int i;

            Rectangle r = ClientRectangle;
            panTop.Top = 0;
            panTop.Left = 0;
            panTop.Width = r.Width;
            tabMain.Top = panTop.Height;
            tabMain.Left = 0;
            tabMain.Width = r.Width;
            ArrangeWorkArea(panTop.Height + tabMain.Height, 0, r.Width, r.Height - panTop.Height - tabMain.Height - StatusInfo.Height);
            tabMain.Visible = true;
            // Sort out the values on the top panel
            int requiredWidth = butMnuClearDisplay.Width + 5 + butMnuRecord.Width + 5 + butMnuInfo.Width;

            if (requiredWidth < r.Width)
            {
                leftPos = r.Width - requiredWidth;
            }
            else
            {
                leftPos = 0;
            }
            butMnuClearDisplay.Left = leftPos;
            leftPos += butMnuClearDisplay.Width + 5;
            butMnuRecord.Left = leftPos;
            leftPos += butMnuRecord.Width + 5;
            butMnuInfo.Left = leftPos;
            i = CWidthValues.COM_LED + CWidthValues.ERR_LED + CWidthValues.NUM;
            if ((i + 100) <= iTotalWidth)
            {
                StatusInfo.Items[1].Width = iTotalWidth - i - 50;
            }
            else
            {
                StatusInfo.Items[1].Width = 100;
            }
            StatusInfo.Items[0].Width = CWidthValues.COM_LED;
            StatusInfo.Items[2].Width = CWidthValues.ERR_LED;
            StatusInfo.Items[3].Width = CWidthValues.NUM;
        }

        private void ArrangeWorkArea(int top_, int left_, int width_, int height_)
        {
            rtxtDisplay.Parent = this;
            rtxtDisplay.Top = top_;
            rtxtDisplay.Left = left_;
            rtxtDisplay.Height = height_;
            rtxtDisplay.Width = width_;
        }
        #endregion Appearance Control

        #region Menu Visibility and Monitor Status Handling
        internal static bool IsSendPossible()
        {
            return CTestClient.IsComPortOpen;
        }

        private void DisableControls()
        {
            foreach (Button b in mo_list_of_buttons)
            {
                b.Enabled = false;
            }
            foreach (CheckBox c in mo_list_of_checkboxes)
            {
                c.Enabled = false;
            }
            foreach (ComboBox c in mo_list_of_comboboxes)
            {
                c.Enabled = false;
            }
            foreach (CheckBox c in mo_list_of_hot_checkboxes)
            {
                c.Enabled = false;
            }
            this.Refresh();
        }

        private void CheckControlsDisplay()
        {
            if (CTestClient.IsComPortOpen)
            {
                foreach (Button b in mo_list_of_buttons)
                {
                    b.Enabled = true;
                }
                foreach (CheckBox c in mo_list_of_checkboxes)
                {
                    c.Enabled = true;
                }
                foreach (ComboBox c in mo_list_of_comboboxes)
                {
                    c.Enabled = true;
                }

                if (CTestClient.IsConnected)
                {
                    ButConnect.BackColor = SystemColors.Control;
                    ButConnect.Enabled = false;
                    ButDisconnect.BackColor = Color.PowderBlue;
                    ButDisconnect.Enabled = true;
                    foreach (Button b in mo_list_of_hot_buttons)
                    {
                        b.BackColor = Color.PowderBlue;
                        b.Enabled = true;
                    }
                    foreach (CheckBox c in mo_list_of_hot_checkboxes)
                    {
                        c.Enabled = true;
                    }
                }
                else
                {
                    ButConnect.BackColor = Color.PowderBlue;
                    ButConnect.Enabled = true;
                    ButDisconnect.BackColor = SystemColors.Control;
                    ButDisconnect.Enabled = false;
                    foreach (Button b in mo_list_of_hot_buttons)
                    {
                        b.BackColor = SystemColors.Control;
                        b.Enabled = false;
                    }
                    foreach (CheckBox c in mo_list_of_hot_checkboxes)
                    {
                        c.Enabled = false;
                    }
                }

                ButSendCmd0short.BackColor = Color.PowderBlue;
                ButSendCmd0short.Enabled = true;
                ButReadAll.BackColor = Color.PowderBlue;
                ButReadAll.Enabled = true;
            }
            else
            {
                foreach (Button b in mo_list_of_buttons)
                {
                    b.Enabled = false;
                }
                foreach (CheckBox c in mo_list_of_checkboxes)
                {
                    c.Enabled = false;
                }
                foreach (ComboBox c in mo_list_of_comboboxes)
                {
                    c.Enabled = false;
                }
                foreach (Button b in mo_list_of_hot_buttons)
                {
                    b.BackColor = SystemColors.Control;
                    b.Enabled = false;
                }
                foreach (CheckBox c in mo_list_of_hot_checkboxes)
                {
                    c.Enabled = false;
                }

                ButConnect.BackColor = SystemColors.Control;
                ButConnect.Enabled = false;
                ButDisconnect.BackColor = SystemColors.Control;
                ButDisconnect.Enabled = false;
                ButSendCmd0short.BackColor = SystemColors.Control;
                ButSendCmd0short.Enabled = false;
            }

            if (IsHartIpSelected() && !IsHartIpConnected())
            {
                TxtConStatus.Text = "No Hart Ip";
                ButConnect.Enabled = false;
                ButConnect.BackColor = SystemColors.Control;
                ButDisconnect.Enabled = false;
                ButDisconnect.BackColor = SystemColors.Control;
                ButSendCmd0short.Enabled = false;
                ButSendCmd0short.BackColor = SystemColors.Control;
                ButReadAll.Enabled = false;
                ButReadAll.BackColor = SystemColors.Control;
            }
            else if (CTestClient.IsConnected)
            {
                TxtConStatus.Text = "Connected";
            }
            else
            {
                TxtConStatus.Text = "Not Connected";
            }
        }

        #endregion

        #region Handle Monitor Mode
        private void HandleMonitorMode()
        {
            if (CSettings.ModeMonitorFrames)
            {
                butMnuRecord.Text = "Record Off";
            }
            else
            {
                butMnuRecord.Text = "Record On";
            }
        }
        #endregion Handle Monitor Mode

        #region Color Handling
        private void ButEditColors_Click(object sender, EventArgs e)
        {
            FrmSetColors f = new FrmSetColors(this);
            bool bWasTopMost = CSettings.OptionsTopMost;

            if (bWasTopMost)
            {
                this.TopMost = false;
            }

            if (f.ShowDialog() == DialogResult.OK)
            {
                CSettings.SaveUserColors();
                ApplyColorsFromForm(f);
            }
            if (bWasTopMost)
            {
                this.TopMost = true;
            }
        }

        internal void ApplyColorsFromForm(FrmSetColors f_)
        {
            if (f_.MonitorColorsCanged)
            {
                ApplyMonitorColors();
            }

            if (f_.StatusBarColorsCanged)
            {
                ApplyStatusInfoColors();
            }

            ApplyHotKeyColors();
        }

        private void ApplyColors()
        {
            if (m_last_color_set != CSettings.ColorSet)
            {
                m_last_color_set = CSettings.ColorSet;
                ApplyMonitorColors();
                ApplyHotKeyColors();
            }
        }

        private void ApplyMonitorColors()
        {
            rtxtDisplay.BackColor = CSettings.MonColBack;
            DisplayFrames();
        }

        private void ApplyStatusInfoColors()
        {
            SetStatusInfo();
        }

        private void ApplyHotKeyColors()
        {
            ButSendCmd0short.BackColor = Color.PowderBlue;
            butSendCmd1.BackColor = Color.PowderBlue;
            ButSendCmd3.BackColor = Color.PowderBlue;
        }
        #endregion Color Handling

        #region Update Data
        internal void UpdatePollAddress(byte poll_address_)
        {
            if (poll_address_ < 64)
            {
                CSettings.PollAddress = poll_address_;
                CmbPollAddress.SelectedIndex = poll_address_;
            }
        }

        internal void UpdateUniqueID(byte[] bytes_of_unique_id_)
        {
            CTestClient.SetUniqueID(bytes_of_unique_id_);
            DisplayUniqueID();
        }
        #endregion

        #region Display Items
        private void DisplayUniqueID()
        {
            byte[] abyTmp = CTestClient.GetUniqueID();

            TxtDevDatUniqueID.Text = "0x" + CFrameHelper.GetHex(abyTmp[0]);
            TxtDevDatUniqueID.Text += CFrameHelper.GetHex(abyTmp[1]);
            TxtDevDatUniqueID.Text += CFrameHelper.GetHex(abyTmp[2]);
            TxtDevDatUniqueID.Text += CFrameHelper.GetHex(abyTmp[3]);
            TxtDevDatUniqueID.Text += CFrameHelper.GetHex(abyTmp[4]);
        }
        #endregion

        #region Handle Indication LEDs
        internal void SetComPortStatus(int status_)
        {
            if (status_ == m_last_comport_status)
            {
                return;
            }

            m_last_comport_status = status_;
            StatusInfo.Items[0].Image = status_ switch
            {
                CComPortStatus.FAILED => ImgL_Leds.Images[CLEDcolor.RED],
                CComPortStatus.OK => ImgL_Leds.Images[CLEDcolor.GREEN],
                CComPortStatus.UNCERTAIN => ImgL_Leds.Images[CLEDcolor.YELLOW],
                _ => ImgL_Leds.Images[CLEDcolor.BLACK],
            };
        }

        internal void HandleMsgLedStatus()
        {
            // Take over request if not busy
            if ((!m_msg_yellow_busy) && (!m_msg_red_busy))
            {
                // YELLOW has priority
                if (!m_msg_red_busy)
                {
                    if (m_msg_show_red)
                    {
                        m_msg_red_busy = true;
                        m_msg_show_red = false;
                        m_led_cycles = 0;
                    }
                }
            }

            if ((!m_msg_yellow_busy) && (!m_msg_red_busy))
            {
                // YELLOW has priority
                if (!m_msg_yellow_busy)
                {
                    if (m_msg_show_yellow)
                    {
                        m_msg_yellow_busy = true;
                        m_msg_show_yellow = false;
                        m_led_cycles = 0;
                    }
                }
            }

            if (m_msg_yellow_busy)
            {
                switch (m_msg_led_status)
                {
                    case CLEDstatus.KEEP_RED:
                        m_led_cycles = 0;
                        // First show black
                        StatusInfo.Items[2].Image = ImgL_Leds.Images[CLEDcolor.BLACK];
                        m_msg_led_status = CLEDstatus.FIRST_BLACK;
                        break;
                    case CLEDstatus.FIRST_BLACK:
                    case CLEDstatus.IDLE:
                        if (m_led_cycles == 7)
                        {
                            m_led_cycles = 0;
                            // Show yellow image
                            StatusInfo.Items[2].Image = ImgL_Leds.Images[CLEDcolor.YELLOW];
                            m_msg_led_status = CLEDstatus.YELLOW;
                        }
                        break;
                    case CLEDstatus.YELLOW:
                        if (m_led_cycles == 1)
                        {
                            m_led_cycles = 0;
                            // Show black image
                            StatusInfo.Items[2].Image = ImgL_Leds.Images[CLEDcolor.BLACK];
                            m_msg_led_status = CLEDstatus.BLACK;
                        }
                        break;
                    case CLEDstatus.BLACK:
                        if (m_led_cycles == 7)
                        {
                            m_led_cycles = 0;
                            // Done
                            m_msg_led_status = CLEDstatus.IDLE;
                            m_msg_yellow_busy = false;
                        }
                        break;
                }
            }

            if (m_msg_red_busy)
            {
                switch (m_msg_led_status)
                {
                    case CLEDstatus.KEEP_RED:
                        m_led_cycles = 0;
                        // First show black
                        StatusInfo.Items[2].Image = ImgL_Leds.Images[CLEDcolor.BLACK];
                        m_msg_led_status = CLEDstatus.FIRST_BLACK;
                        break;
                    case CLEDstatus.FIRST_BLACK:
                    case CLEDstatus.IDLE:
                        if (m_led_cycles == 3)
                        {
                            m_led_cycles = 0;
                            // Show red image
                            StatusInfo.Items[2].Image = ImgL_Leds.Images[CLEDcolor.RED];
                            m_msg_led_status = CLEDstatus.RED;
                        }
                        break;
                    case CLEDstatus.RED:
                        if (m_led_cycles == 10)
                        {
                            m_led_cycles = 0;
                            // Show black image
                            m_msg_led_status = CLEDstatus.KEEP_RED;
                            m_msg_red_busy = false;
                        }
                        break;
                }
            }

            if (m_led_cycles < 10)
            {
                m_led_cycles++;
            }
        }

        internal void SetMsgLedColor(int status_)
        {
            if (status_ == CLEDcolor.RED)
            {
                m_msg_show_red = true;
            }
            if (status_ == CLEDcolor.YELLOW)
            {
                m_msg_show_yellow = true;
            }
        }

        internal void SetMsgLedStatusOff()
        {
            m_msg_show_yellow = false;
            m_msg_yellow_busy = false;
            m_msg_show_red = false;
            m_msg_red_busy = false;
            m_msg_led_status = CLEDstatus.IDLE;
            m_led_cycles = 0;
            StatusInfo.Items[2].Image = ImgL_Leds.Images[CLEDcolor.BLACK];
        }
        #endregion

        #region Handle Diagnostics
        private void UpdateDiagnostics(CFrameData frame_data_)
        {
            float fNumRcvs;
            float fNumErr;

            if (!frame_data_.ClientTx)
            {
                if (!frame_data_.MinorError)
                {
                    m_num_rcv_frames++;
                    if (frame_data_.FrameError)
                    {
                        m_num_errors++;
                    }
                    fNumRcvs = Convert.ToSingle(m_num_rcv_frames);
                    fNumErr = Convert.ToSingle(m_num_errors);
                    // calculate the quality
                    m_quality = 100.0f - ((fNumErr / fNumRcvs) * 100.0f);
                }
            }
        }

        private void ClearDiagnostics()
        {
            m_num_rcv_frames = 0;
            m_num_errors = 0;
            m_quality = 100.0f;
            m_last_num_rcv_frames = -1;
            m_last_num_errors = -1;
            m_last_quality = -1.0f;
        }

        private void DisplayDiagnostics()
        {
            if (m_last_num_rcv_frames != m_num_rcv_frames)
            {
                m_last_num_rcv_frames = m_num_rcv_frames;
            }
            if (m_last_num_errors != m_num_errors)
            {
                m_last_num_errors = m_num_errors;
            }
            if (m_last_quality != m_quality)
            {
                m_last_quality = m_quality;
            }
        }
        #endregion

        #region Data Decoding
        private void TsmnuDecInteger_Click(object sender, EventArgs e)
        {
            string s;

            if (!CheckForDecoding())
            {
                return;
            }
            if (m_data_bytes_selected > 0)
            {
                s = CFrameHelper.CDecode.DecodeInteger(m_data_bytes_selected);
                ShowToolTip("Integer Value(s)", s);
            }
            else
            {
                CFrameHelper.SignalInvalidSelection(m_selected_data_text);
            }
        }

        private bool CheckForDecoding()
        {
            m_selected_data_text = rtxtDisplay.SelectedText;

            if (m_selected_data_text.Length > 0)
            {
                m_data_bytes_selected = CFrameHelper.GetBytesFromFrameData(m_selected_data_text);

                if (m_data_bytes_selected > 0)
                {
                    return true;
                }
            }
            else
            {
                CFrameHelper.SignalNoSelection();
            }
            return false;
        }

        private void TsmnuDecFloat_Click(object sender, EventArgs e)
        {
            string s;

            if (!CheckForDecoding())
            {
                return;
            }
            if (m_data_bytes_selected > 3)
            {
                s = CFrameHelper.CDecode.DecodeFloat(m_data_bytes_selected);
                ShowToolTip("Float Value(s)", s);
            }
            else
            {
                CFrameHelper.SignalInvalidSelection(m_selected_data_text);
            }
        }

        private void TsmnuDecDouble_Click(object sender, EventArgs e)
        {
            string s;

            if (!CheckForDecoding())
            {
                return;
            }
            if (m_data_bytes_selected > 7)
            {
                s = CFrameHelper.CDecode.DecodeDouble(m_data_bytes_selected);
                ShowToolTip("Double Value(s)", s);
            }
            else
            {
                CFrameHelper.SignalInvalidSelection(m_selected_data_text);
            }
        }

        private void TsmnuDecHartUnit_Click(object sender, EventArgs e)
        {
            string s;

            if (!CheckForDecoding())
            {
                return;
            }
            if (m_data_bytes_selected > 0)
            {
                s = CFrameHelper.CDecode.DecodeHartUnit(m_data_bytes_selected);
                ShowToolTip("Hart Unit(s)", s);
            }
            else
            {
                CFrameHelper.SignalInvalidSelection(m_selected_data_text);
            }
        }

        private void TsmnuDecPackedASCII_Click(object sender, EventArgs e)
        {
            string s;

            if (!CheckForDecoding())
            {
                return;
            }
            if (m_data_bytes_selected > 0)
            {
                s = CFrameHelper.CDecode.DecodePackedASCII(m_data_bytes_selected);
                ShowToolTip("Packed ASCII", s);
            }
            else
            {
                CFrameHelper.SignalInvalidSelection(m_selected_data_text);
            }
        }

        private void TsmnuDecText_Click(object sender, EventArgs e)
        {
            string s;

            if (!CheckForDecoding())
            {
                return;
            }
            if (m_data_bytes_selected > 0)
            {
                s = CFrameHelper.CDecode.DecodeText(m_data_bytes_selected);
                ShowToolTip("Text", s);
            }
            else
            {
                CFrameHelper.SignalInvalidSelection(m_selected_data_text);
            }
        }

        private void TsmnuDecBinary_Click(object sender, EventArgs e)
        {
            string s;

            if (!CheckForDecoding())
            {
                return;
            }
            if (m_data_bytes_selected > 0)
            {
                s = CFrameHelper.CDecode.DecodeBinary(m_data_bytes_selected);
                ShowToolTip("Binary", s);
            }
            else
            {
                CFrameHelper.SignalInvalidSelection(m_selected_data_text);
            }
        }

        private void ShowToolTip(string sTitle, string sCaption)
        {
            ToolTipDecodedData.ToolTipTitle = sTitle;
            //toolTipDecodedData.SetToolTip(rtxtDisplay, sCaption); 
            ToolTipDecodedData.Show(sCaption, rtxtDisplay, 60000);
        }

        private void CtxMnuDecode_Opened(object sender, EventArgs e)
        {
            ToolTipDecodedData.Hide(rtxtDisplay);
        }
        #endregion

        #region Copy to Any Frame
        private void TsmnuBytesToClipBoard_Click(object sender, EventArgs e)
        {
            string s;

            if (!CheckForDecoding())
            {
                return;
            }
            if (m_data_bytes_selected > 0)
            {
                s = CFrameHelper.HexDumpByteDataToDataStx((byte)m_data_bytes_selected);
                Clipboard.SetText(s);
            }
            else
            {
                CFrameHelper.SignalInvalidSelection(m_selected_data_text);
            }
        }
        #endregion

        #region Showing tool tips
        private void ToolTipInfo_Popup(object sender, PopupEventArgs e)
        {
            if (m_tool_tip_info_changed == true)
            {
                return;
            }
            m_tool_tip_info_changed = true;
            if (e.AssociatedControl == ButSendCmd31)
            {
                ToolTipInfo.SetToolTip(ButSendCmd31, "Cmd " + CSettings.ExtCmd.ToString("0") + " | " + CSettings.ExtCmdData);
            }

            m_tool_tip_info_changed = false;
        }
        #endregion Showing tool tips

        #region Main Window Start Position
        private void SetMainWindowStartPosition(int left_, int top_)
        {
            Point initTopLeft = new Point(left_, top_);
            Screen[] screens = Screen.AllScreens;

            foreach (Screen screen in screens)
            {
                if (screen.WorkingArea.Contains(initTopLeft))
                {
                    SetWindowPosition(CSettings.MainFormLeft, CSettings.MainFormTop);
                    return;
                }
            }
            // Window is off screen, center on main desktop
            Screen? desktop = Screen.PrimaryScreen;
            if (desktop != null)
            {
                Rectangle workRect = desktop.WorkingArea;
                int newleft = (workRect.Width - this.Width) / 2;
                int newtop = (workRect.Height - this.Height) / 2;
                SetWindowPosition(newleft, newtop);
            }
        }
        #endregion

        #region Slave Device Data Handling
        private void ButConnect_Click(object sender, EventArgs e)
        {
            if (CmbComPort.SelectedIndex == 1)
            {
                CTestClient.HartIpConnect();
            }
            else
            {
                ButSendCmd0short_Click(sender, e);
            }
        }

        private void ButDisconnect_Click(object sender, EventArgs e)
        {
            CTestClient.Disconnect();
            CDevStatus.CMonitor.CfgChanged = true;
            CDevStatus.CGeneral.CfgChanged = true;
            CDevStatus.CfgChanged = true;
            HandleControlEvent();
        }

        private void ButSendCmd35_Click(object sender, EventArgs e)
        {
            SendCmd35();
        }

        private void ButEditCmd35_Click(object sender, EventArgs e)
        {
            Edit_Cmd35();
        }

        private void ButDetDatReadAll_Click(object sender, EventArgs e)
        {
            TimOperating.Enabled = false;
            TimWatch.Enabled = false;

            TxtDevDatUniqueID.Text = "-/-";
            TxtDevDatShortTag.Text = "-/-";
            TxtDevDatLongTag.Text = "-/-";
            TxtDevDatMessage.Text = "-/-";
            TxtDevDatUpperRange.Text = "-/-";
            LblDevDatRangeUnitUp.Text = "-/-";
            TxtDevDatLowerRange.Text = "-/-";
            LblDevDatRangeUnitLo.Text = "-/-";
            TxtDevDatSpan.Text = "-/-";
            LblDevDatSpanUnit.Text = "-/-";
            TxtDevDatPV.Text = "-/-";
            LblDevDatUnitPV.Text = "-/-";
            TxtDevDatSV.Text = "-/-";
            LblDevDatUnitSV.Text = "-/-";
            TxtDevDatTV.Text = "-/-";
            LblDevDatUnitTV.Text = "-/-";
            TxtDevDatQV.Text = "-/-";
            LblDevDatUnitQV.Text = "-/-";
            TxtDevDatCurrent.Text = "-/-";
            this.Refresh();

            CTestClient.ReqCmd0short();
            if (CTestClient.hServiceConnect != HartDeviceDLL.INVALID_SRV_Handle)
            {
                while ((HartDeviceDLL.BAHAMA_IsServiceCompleted(CTestClient.hServiceConnect) != EN_Bool.TRUE8) &&
                       (CTestClient.hServiceConnect != HartDeviceDLL.INVALID_SRV_Handle))
                {
                    HandleCyclics();
                    Thread.Sleep(50);
                }
            }

            HandleCyclics();

            if (CTestClient.IsConnected)
            {
                SendCmd3_Wait();
                HandleCyclics();
            }

            if (CTestClient.IsConnected)
            {
                SendCmd12_Wait();
                HandleCyclics();
            }

            if (CTestClient.IsConnected)
            {
                SendCmd13_Wait();
                HandleCyclics();
            }

            if (CTestClient.IsConnected)
            {
                SendCmd14_Wait();
                HandleCyclics();
            }

            if (CTestClient.IsConnected)
            {
                SendCmd15_Wait();
                HandleCyclics();
            }

            if (CTestClient.IsConnected)
            {
                SendCmd20_Wait();
                HandleCyclics();
            }

            TimOperating.Enabled = true;
            TimWatch.Enabled = true;
        }
        internal void DetDatShowShortTag(string tag_)
        {
            TxtDevDatShortTag.Text = tag_;
            TxtDevDatShortTag.Refresh();
        }
        internal void DevDatShowLongTag(string tag_)
        {
            TxtDevDatLongTag.Text = tag_;
            TxtDevDatLongTag.Refresh();
        }
        internal void DevDatShowMessage(string message_)
        {
            TxtDevDatMessage.Text = message_;
            TxtDevDatMessage.Refresh();
        }
        internal void DevDatShowUpperRange(float value_, byte unit_)
        {
            TxtDevDatUpperRange.Text = CHelpers.CFormat.Float5digit(value_);
            LblDevDatRangeUnitUp.Text = CHartUtils.CHartUnit.GetText(unit_);
            TxtDevDatUpperRange.Refresh();
            LblDevDatRangeUnitUp.Refresh();
        }
        internal void DevDatShowLowerRange(float value_, byte unit_)
        {
            TxtDevDatLowerRange.Text = CHelpers.CFormat.Float5digit(value_);
            LblDevDatRangeUnitLo.Text = CHartUtils.CHartUnit.GetText(unit_);
            TxtDevDatLowerRange.Refresh();
            LblDevDatRangeUnitLo.Refresh();
        }

        internal void DevDatShowSpan(float value_, byte unit_)
        {
            TxtDevDatSpan.Text = CHelpers.CFormat.Float5digit(value_);
            LblDevDatSpanUnit.Text = CHartUtils.CHartUnit.GetText(unit_);
            TxtDevDatSpan.Refresh();
            LblDevDatSpanUnit.Refresh();
        }
        internal void DevDatShowCurrent(float value_)
        {
            TxtDevDatCurrent.Text = CHelpers.CFormat.Float5digit(value_);
            TxtDevDatCurrent.Refresh();
        }
        internal void DevDatShowPV(float value_, byte unit_)
        {
            TxtDevDatPV.Text = CHelpers.CFormat.Float5digit(value_);
            LblDevDatUnitPV.Text = CHartUtils.CHartUnit.GetText(unit_);
            TxtDevDatPV.Refresh();
            LblDevDatUnitPV.Refresh();
        }
        internal void DevDatShowSV(float value_, byte unit_)
        {
            TxtDevDatSV.Text = CHelpers.CFormat.Float5digit(value_);
            LblDevDatUnitSV.Text = CHartUtils.CHartUnit.GetText(unit_);
            TxtDevDatSV.Refresh();
            LblDevDatUnitSV.Refresh();
        }
        internal void DevDatShowTV(float value_, byte unit_)
        {
            TxtDevDatTV.Text = CHelpers.CFormat.Float5digit(value_);
            LblDevDatUnitTV.Text = CHartUtils.CHartUnit.GetText(unit_);
            TxtDevDatTV.Refresh();
            LblDevDatUnitTV.Refresh();
        }
        internal void DevDatShowQV(float value_, byte unit_)
        {
            TxtDevDatQV.Text = CHelpers.CFormat.Float5digit(value_);
            LblDevDatUnitQV.Text = CHartUtils.CHartUnit.GetText(unit_);
            TxtDevDatQV.Refresh();
            LblDevDatUnitQV.Refresh();
        }
        #endregion Slave Device Data Handling

        #region Cyclic handlers
        internal void HandleWatchingData()
        {
            HandleMonitorData();

            if (CTestClient.hServiceConnect != HartDeviceDLL.INVALID_SRV_Handle)
            {
                if (HartDeviceDLL.BAHAMA_IsServiceCompleted(CTestClient.hServiceConnect) == EN_Bool.TRUE8)
                {
                    CTestClient.HandleConnectService();
                    CTestClient.hServiceConnect = HartDeviceDLL.INVALID_SRV_Handle;
                    CDevStatus.CMonitor.CfgChanged = true;
                    CDevStatus.CGeneral.CfgChanged = true;
                    CDevStatus.CfgChanged = true;
                    HandleControlEvent();
                }

                return;
            }

            if (CTestClient.hServiceDoCmd != HartDeviceDLL.INVALID_SRV_Handle)
            {
                if (HartDeviceDLL.BAHAMA_IsServiceCompleted(CTestClient.hServiceDoCmd) == EN_Bool.TRUE8)
                {
                    CTestClient.HandleDoCmdService();
                    CTestClient.hServiceDoCmd = HartDeviceDLL.INVALID_SRV_Handle;
                    if (CTestClient.IsConnected == false)
                    {
                        CDevStatus.CGeneral.CfgChanged = true;
                        CDevStatus.CfgChanged = true;
                        HandleControlEvent();
                    }
                }
            }
        }

        internal void HandleWatchingStatus()
        {
            HandleMsgLedStatus();
            if (m_last_number_frames != m_number_frames)
            {
                StatusInfo.Items[3].Text = m_number_frames.ToString("0000000");
                m_last_number_frames = m_number_frames;
            }

            DisplayDiagnostics();
        }

        internal void HandleCyclics()
        {
            HandleWatchingData();
            HandleWatchingStatus();
        }
        #endregion Cyclic handlers

        #region White Test Support
        private void TestBegin(string text_)
        {
            rtxtTestDisplay.Clear();
            rtxtTestDisplay.Text = text_;
            DisableTimers();
        }

        private void DisableTimers()
        {
            TimWatch.Enabled = false;
            TimOperating.Enabled = false;
        }

        private void EnableTimers()
        {
            TimWatch.Enabled = true;
            TimOperating.Enabled = true;
        }

        private void TestUpdate(string text_)
        {
            HandleCyclics();
            rtxtTestDisplay.AppendText("\n" + text_);
        }

        private void TestEnd(string text_)
        {
            rtxtTestDisplay.AppendText("\n" + text_);
            EnableTimers();
        }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        // Test array buffeer
        private byte[] buffer = new byte[256];

        private void ButUserTestCoding_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 256; i++)
            {
                buffer[i] = 0;
            }

            byte by = 0;
            TestBegin("User Test Coding Started.");
            TestUpdate("01: Byte(0x11) at 05");
            HartDeviceDLL.BAHA_PutByte(0x11, 5, ref buffer[0]);
            by = HartDeviceDLL.BAHA_PickByte(5, ref buffer[0]);
            if (by != 0x11)
            {
                TestEnd("Test failed, step 01.");
                return;
            }

            ushort us = 0;
            TestUpdate("02: Word(0x55AA) at 16");
            HartDeviceDLL.BAHA_PutWord(0x55AA, 16, ref buffer[0], EN_Endian.MSB_First);
            us = HartDeviceDLL.BAHA_PickWord(16, ref buffer[0], EN_Endian.LSB_First);
            if (us != 0xAA55)
            {
                TestEnd("Test failed, step 02.");
                return;
            }

            uint ui = 0;
            TestUpdate("03: Word(0x3355AACC) at 88");
            HartDeviceDLL.BAHA_PutDWord(0x3355AACC, 88, ref buffer[0], EN_Endian.LSB_First);
            ui = HartDeviceDLL.BAHA_PickDWord(88, ref buffer[0], EN_Endian.MSB_First);
            if (ui != 0xCCAA5533)
            {
                TestEnd("Test failed, step 03.");
                return;
            }

            TestUpdate("04: Word(0xCCAA) at 88?");
            us = HartDeviceDLL.BAHA_PickWord(88, ref buffer[0], EN_Endian.MSB_First);
            if (us != 0xCCAA)
            {
                TestEnd("Test failed, step 04.");
                return;
            }

            TestUpdate("05: Word(0x5533) at 90?");
            us = HartDeviceDLL.BAHA_PickWord(90, ref buffer[0], EN_Endian.MSB_First);
            if (us != 0x5533)
            {
                TestEnd("Test failed, step 05.");
                return;
            }

            TestUpdate("06: PASCII: \"HELLO \"");
            StringBuilder sb = new StringBuilder(8);
            sb.Insert(0, "HELLO ");
            HartDeviceDLL.BAHA_PutPackedASCII(sb, 6, 30, ref buffer[0]);
            HartDeviceDLL.BAHA_PickPackedASCII(sb, 6, 30, ref buffer[0]);
            string result = sb.ToString();
            if (result != "HELLO ")
            {
                TestEnd("Test failed, step 06.");
                return;
            }

            TestEnd("Test completed with no errors.");

            // Test sequence still to be completed
        }
        private void ButUserTestCommands_Click(object sender, EventArgs e)
        {
            ushort h_service;
            HartDeviceDLL.TY_Confirmation confirmation = new HartDeviceDLL.TY_Confirmation();

            for (int i = 0; i < 256; i++)
            {
                buffer[i] = 0;
            }

            TestBegin("User Test Commands Started.");
            TestUpdate("01: Is device connected?");
            CTestClient.ReqCmd0short();
            while (HartDeviceDLL.BAHAMA_IsServiceCompleted(CTestClient.hServiceConnect) != EN_Bool.TRUE8)
            {
                HandleCyclics();
                Thread.Sleep(50);
            }

            HandleCyclics();

            if (!CTestClient.IsConnected)
            {
                TestEnd("No device connected.");
                return;
            }

            // Set range to 0 to 100 %
            buffer[0] = 57;
            HartDeviceDLL.BAHA_PutFloat(100.0f, 1, ref buffer[0], EN_Endian.MSB_First);
            HartDeviceDLL.BAHA_PutFloat(0.0f, 6, ref buffer[0], EN_Endian.MSB_First);
            TestUpdate("02: Set 0 .. 100 %");
            h_service = HartDeviceDLL.BAHAMA_DoCommand(35, HartDeviceDLL.WAIT_For_Service, ref buffer[0], 9, ref CTestClient.BytesOfUniqueId[0]);
            HandleCyclics();
            HartDeviceDLL.BAHAMA_FetchConfirmation(h_service, ref confirmation);
            if (confirmation.RespCode1 != 0)
            {
                TestEnd("Command rejected.");
                return;
            }

            // Read back the range values
            TestUpdate("03: Read back range");
            h_service = HartDeviceDLL.BAHAMA_DoCommand(15, HartDeviceDLL.WAIT_For_Service, ref buffer[0], 0, ref CTestClient.BytesOfUniqueId[0]);
            HandleCyclics();
            HartDeviceDLL.BAHAMA_FetchConfirmation(h_service, ref confirmation);
            if (confirmation.RespCode1 != 0)
            {
                TestEnd("Command rejected.");
                return;
            }

            float upper_range = HartDeviceDLL.BAHA_PickFloat(3, ref confirmation.BytesOfData[0], EN_Endian.MSB_First);
            if (upper_range != 100.0f)
            {
                TestEnd("Upper range error!");
                return;
            }

            float lower_range = HartDeviceDLL.BAHA_PickFloat(7, ref confirmation.BytesOfData[0], EN_Endian.MSB_First);
            if (lower_range != 0.0f)
            {
                TestEnd("Lower range error!");
                return;
            }

            TestEnd("Test completed with no errors.");
        }

        private void ButFreeTest_Click(object sender, EventArgs e)
        {
            TestBegin("Free User Test Started.");
            TestUpdate("No test sequence has yet been ");
            TestUpdate("implemented for this button.");
            TestEnd("Test completed with no errors.");
        }
        #endregion White Test Support

        #region Hart Ip Handling

        internal void HandleHartIpStatus()
        {
            ushort ip_iface_flags = HartDeviceDLL.BAHAMA_GetHartIpStatus();
            m_ip_status = (EN_HartIpStatus)(((ip_iface_flags & 0xFF00) >> 8) & 0x1f);
            m_ip_last_error = (byte)(ip_iface_flags & 0xff);

            if (ip_iface_flags != m_last_ip_iface_flags)
            {
                string txt_status = string.Empty;
                m_last_ip_iface_flags = ip_iface_flags;

                if (m_ip_status != m_ip_last_status)
                {
                    m_ip_last_status = m_ip_status;
                    switch (m_ip_status)
                    {
                        case EN_HartIpStatus.IDLE:
                            txt_status = "IDLE";
                            m_ip_connection_status = false;
                            break;
                        case EN_HartIpStatus.INITIALIZING:
                            txt_status = "INITIALIZING";
                            m_ip_connection_status = false;
                            break;
                        case EN_HartIpStatus.WAIT_CONNECT:
                            txt_status = "WAIT CONNECT";
                            m_ip_connection_status = false;
                            break;
                        case EN_HartIpStatus.WAIT_INI_RESPONSE:
                            txt_status = "WAIT INI RESPONSE";
                            m_ip_connection_status = false;
                            break;
                        case EN_HartIpStatus.CLIENT_READY:
                            txt_status = "CLIENT READY";
                            m_ip_connection_status = true;
                            break;
                        case EN_HartIpStatus.WAIT_ALIVE_RESPONSE:
                            txt_status = "WAIT ALIVE RESPONSE";
                            m_ip_connection_status = true;
                            break;
                        case EN_HartIpStatus.WAIT_RESPONSE:
                            txt_status = "WAIT RESPONSE";
                            m_ip_connection_status = true;
                            break;
                        case EN_HartIpStatus.WAIT_CLOSE_RESPONSE:
                            txt_status = "WAIT CLOSE RESPONSE";
                            m_ip_connection_status = false;
                            break;
                        case EN_HartIpStatus.SHUTTING_DOWN:
                            txt_status = "SHUTTING DOWN";
                            m_ip_connection_status = false;
                            break;
                        default:
                            txt_status = "UNKNOWN";
                            m_ip_connection_status = false;
                            break;
                    }

                    TxtConnectionStatus.Text = txt_status;
                }


                // Last error
                string txt_last_error = string.Empty;
                if (m_ip_last_error != m_ip_last_last_error)
                {
                    m_ip_last_last_error = m_ip_last_error;
                    switch (m_ip_last_error)
                    {
                        case 0:
                            txt_last_error = "NONE";
                            break;
                        case 1:
                            txt_last_error = "INITIALIZING";
                            break;
                        case 2:
                            txt_last_error = "GET ADDR INFO";
                            break;
                        case 3:
                            txt_last_error = "CREATE SOCKET";
                            break;
                        case 4:
                            txt_last_error = "NO SERVER";
                            break;
                        case 5:
                            txt_last_error = "TX FAILED";
                            break;
                        case 6:
                            txt_last_error = "SHUTDOWN";
                            break;
                        case 7:
                            txt_last_error = "RECEIVING";
                            break;
                        case 8:
                            txt_last_error = "NETWORK";
                            break;
                        case 9:
                            txt_last_error = "SET TIMEOUT";
                            break;
                        case 10:
                            txt_last_error = "KEEP ALIVE";
                            break;
                        default:
                            txt_last_error = "UNKNOWN";
                            break;
                    }

                    TxtLastError.Text = txt_last_error;
                }


                // Kind of a workaround

                if (IsHartIpSelected())
                {
                    if (m_ip_connection_status != m_ip_last_connection_status)
                    {
                        m_ip_last_connection_status = m_ip_connection_status;
                        if (m_ip_connection_status == false)
                        {
                            CTestClient.IsConnected = false;
                        }

                        CDevStatus.CMonitor.CfgChanged = true;
                        CDevStatus.CGeneral.CfgChanged = true;
                        CDevStatus.CfgChanged = true;
                        HandleControlEvent();
                        CheckHartIpVisibility();

                    }
                }
                else
                {
                    m_ip_connection_status = false;
                    m_ip_last_connection_status = true;
                }
            }
        }
        private void ButSetDefault_Click(object sender, EventArgs e)
        {
            if (ChkHartIpEnabled.Checked == false)
            {
                TxtHartIpAddress.Text = "127.0.0.2";
                TxtHartIpHostName.Text = "localhost";
                TxtHartIpPort.Text = "5094";
                ChkHartIpUseAddress.Checked = true;
            }
        }
        private void ChkHartIpEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_no_control_action)
            {
                if (ChkHartIpEnabled.Checked)
                {
                    HartIpEnable();
                }
                else
                {
                    HartIpDisable();
                }
            }
        }
        private void HartIpEnable()
        {
            if (!m_loading)
            {
                if (CmbComPort.SelectedIndex != 1)
                {
                    // Set com port to HartIp
                    CmbComPort.SelectedIndex = 1;
                }
            }
        }
        private void HartIpDisable()
        {
            if (!m_loading)
            {
                if (CmbComPort.SelectedIndex == 1)
                {
                    // Set com port to None
                    CmbComPort.SelectedIndex = 0;
                }
            }
        }
        private void CheckHartIpVisibility()
        {
            if (CmbComPort.SelectedIndex == 1)
            {
                TxtHartIpAddress.Enabled = false;
                TxtHartIpHostName.Enabled = false;
                TxtHartIpPort.Enabled = false;
                ChkHartIpUseAddress.Enabled = false;
                ButSetDefault.Enabled = false;
            }
            else
            {
                TxtHartIpAddress.Enabled = true;
                TxtHartIpHostName.Enabled = true;
                TxtHartIpPort.Enabled = true;
                ChkHartIpUseAddress.Enabled = true;
                ButSetDefault.Enabled = true;
            }
        }
        private bool IsHartIpSelected()
        {
            if (CmbComPort.SelectedIndex == 1)
            {
                return true;
            }

            return false;
        }
        private bool IsHartIpConnected()
        {
            return m_ip_connection_status;
        }
        private bool IsHartConnected()
        {
            return CTestClient.IsConnected;
        }
        private void HartIpInit()
        {
            // .15 .14 .13 .12 .11 .10 .09 .08 .07 .06 .05 .04 .03 .02 .01 .00
            //  +   +   +   +--- Status ----+   +-------- Last Error -------+
            //  |   |   +-- tbd
            //  |   +------ tbd
            //  +---------- tbd
            m_last_ip_iface_flags = 0xffff;
            m_ip_status = (EN_HartIpStatus)0x00;
            m_ip_last_status = (EN_HartIpStatus)0xff;
            m_ip_last_error = 0x00;
            m_ip_last_last_error = 0xff;
            m_ip_connection_status = false;
            m_ip_last_connection_status = false;
        }
        #endregion Hart Ip Handling

        #region Quick DLL Updates
        private void DllUpdateSendKeepAlive()
        {
            if (ChkHartIpSendKeepAlive.Checked == true)
            {
                CSettings.HartIpSendKeepAlive = 1;
            }
            else
            {
                CSettings.HartIpSendKeepAlive = 0;
            }

            HartDeviceDLL.BAHAMA_GetConfiguration(ref CTestClient.Configuration);
            CTestClient.Configuration.HartIpSendKeepAlive = (EN_Bool)CSettings.HartIpSendKeepAlive;
            HartDeviceDLL.BAHAMA_SetConfiguration(ref CTestClient.Configuration);
        }
        #endregion Quick DLL Updates
    }
}
