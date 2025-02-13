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
using BaTestHart.HartUtils;
using BaTestHart.Properties;
using System.Collections;
using System.Diagnostics;
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

        internal bool is_max_frame_number = false;
        internal const ushort MAX_NUM_FRAMES = 5000;
        #endregion

        #region Private Data
        private int m_number_frames = 0;
        private int m_last_number_frames = -1;
        private bool m_show_menu = true;
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
        //private bool m_tool_tip_info_changed = false;
        // status info
        private int m_last_comport_status = -1;
        private int m_last_monitor_status = -1;
        // Command interpreter
        private Thread CommandInterpreter;
        private ushort m_active_command = 0xffff;
        private bool m_terminate_worker = false;
        // Hart Ip Handling
        private ushort m_last_ip_status = 0xffff;
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
            // Master combo boxes
            mo_list_of_comboboxes.Clear();
            mo_list_of_comboboxes.Add(CmbPollAddress);
            mo_list_of_comboboxes.Add(CmbBaudRate);
            mo_list_of_comboboxes.Add(CmbReqPreambles);
            // Master check boxes
            mo_list_of_checkboxes.Clear();
            // Master hot buttons
            mo_list_of_hot_buttons.Clear();
            // Master hot check boxes
            mo_list_of_hot_checkboxes.Clear();
        }
        #endregion

        #region Standard Component Management
        internal FrmTestClient()
        {
            InitializeComponent();
            CommandInterpreter = new Thread(new ThreadStart(SetupControlLists));
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
            ChkViewPreambles.Checked = CSettings.ViewPreambles;
            ChkViewAddress.Checked = CSettings.ViewAddressing;
            ChkViewTiming.Checked = CSettings.ViewTiming;
            ChkViewStatusBinary.Checked = CSettings.ViewStatusBinary;
            ChkViewDecodedData.Checked = CSettings.ViewDecodedData;
            ChkViewFrameNumbers.Checked = CSettings.ViewFrameNumbers;
            ChkViewShortBursts.Checked = CSettings.ViewShortBursts;
            ChkTopMost.Checked = CSettings.OptionsTopMost;
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

            if ((CSettings.NumReqPreambles >= 0) && (CSettings.NumReqPreambles < 23))
            {
                CmbReqPreambles.SelectedIndex = CSettings.NumReqPreambles;
            }
            else
            {
                CSettings.NumReqPreambles = 5;
                CmbReqPreambles.SelectedIndex = 5;
            }

            if ((CSettings.NumRspPreambles >= 0) && (CSettings.NumRspPreambles < 23))
            {
                CmbRspPreambles.SelectedIndex = CSettings.NumRspPreambles;
            }
            else
            {
                CSettings.NumRspPreambles = 5;
                CmbRspPreambles.SelectedIndex = 5;
            }

            HandleMonitorMode();

            if (this.TopMost != CSettings.OptionsTopMost)
            {
                this.TopMost = CSettings.OptionsTopMost;
            }

            // Hart Ip
            TxtHartIpHostName.Text = CSettings.HartIpHostName;
            TxtHartIpAddress.Text = CSettings.HartIpAddress;
            TxtHartIpPort.Text = CSettings.HartIpPort;
            if (CSettings.HartIpUseAddress == true)
            {
                ChkHartIpUseAddress.Checked = true;
            }
            else
            {
                ChkHartIpUseAddress.Checked = false;
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
                    CheckSlaveConfig();
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
                HartDeviceDLL.BAHASL_StopMonitor();
                CDevStatus.CGeneral.CfgChanged = true;

                HandleMonitorMode();
            }
        }

        private void CheckForMonitorOpen()
        {
            if (CDevStatus.CMonitor.CfgChanged)
            {
                HartDeviceDLL.BAHASL_StartMonitor();
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
                        HartDeviceDLL.BAHASL_StopMonitor();
                        HartDeviceDLL.BAHASL_CloseChannel();
                        CDevStatus.CMonitor.ActiveComport = 0;
                    }

                    if (CSettings.ComPort != 0)
                    {
                        HartDeviceDLL.BAHASL_StartMonitor();
                        CDevStatus.CMonitor.ActiveComport = CSettings.ComPort;
                    }
                    CDevStatus.CGeneral.CfgChanged = true;
                }

                HandleMonitorMode();
            }
        }

        private static void CheckSlaveConfig()
        {
            if (CDevStatus.CGeneral.CfgChanged)
            {
                CTestSlave.Configure();
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
            return CTestSlave.IsComPortOpen;
        }

        private static bool IsMonitoringAllowed()
        {
            return true;
        }
        #endregion

        #region Event Procedures
        #region Form
        private void FrmTestSlave_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            m_terminate_worker = true;
            WriteSettings();
            HartDeviceDLL.BAHASL_StopMonitor();
            CTestSlave.Terminate();

            HartDeviceDLL.BAHASL_CloseChannel();
        }

        private void FrmTestSlave_Load(object sender, System.EventArgs e)
        {
            m_loading = true;
            PlaceAndSizeControls();
            ChkHartIpEnabled.Checked = true;
            // Initialize static classes
            CTestSlave.Init(this);
            CFrameHelper.Init();
            CDevStatus.Init();
            SetupControlLists();
            ReadSettings();
            tabMain.SelectedTab = tabMain.TabPages[0];
            CHartUtils.CHartUnit.InitCombo(CmbPV1unit, 45);
            CHartUtils.CHartUnit.InitCombo(CmbPV2unit, 8);
            CHartUtils.CHartUnit.InitCombo(CmbPV3unit, 96);
            CHartUtils.CHartUnit.InitCombo(CmbPV4unit, 32);
            CHartUtils.CHartUnit.InitCombo(CmbTransdUnit, 8);
            CHartUtils.CHartUnit.InitCombo(CmbRangeUnit, 8);
            CmbAlmSelect.SelectedIndex = 3;
            CmbTFunction.SelectedIndex = 0;
            CmbWrProt.SelectedIndex = 0;
            CTestSlave.StatDataHart.HartIpDataChanged = EN_Bool.TRUE8;
            CTestSlave.UpdateSlaveData();
            SetMsgLedStatusOff();
            CommandInterpreter = new Thread(ExecuteCommandInterpreter);
            CommandInterpreter.Priority = ThreadPriority.Highest;
            CommandInterpreter.Start();
            CheckConfig();
            m_loading = false;
            CheckHartIpVisibility();
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

        private void FrmTestSlave_Activated(object sender, EventArgs e)
        {
        }

        private void FrmTestSlave_Resize(object sender, System.EventArgs e)
        {
            PlaceAndSizeControls();
        }

        private void FrmTestSlave_FormClosing(object sender, FormClosingEventArgs e)
        {
            CTestSlave.Terminate();
        }
        #endregion Form

        #region Menu Buttons
        private void ButMnuUpdateData_Click(object sender, EventArgs e)
        {
            UpdateSlaveData();
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
            if (m_active_command != 0xffff)
            {
                ushort command = m_active_command;
                m_active_command = 0xffff;

                CTestSlave.VerifySlaveData(command);
            }

            HandleWatchingData();
        }

        private void TimOperating_Tick(object sender, EventArgs e)
        {
            HandleWatchingStatus();
            CTestSlave.UpdateSlaveDynamicData();
            HandleHartIpStatus();
        }
        #endregion Timer

        #region Worker
        private void ExecuteCommandInterpreter()
        {
            EN_Bool result;
            ushort command;

            while (!m_terminate_worker)
            {
                Thread.Sleep(1);

                result = (EN_Bool)HartDeviceDLL.BAHASL_WasCommandReceived();

                if (result == EN_Bool.TRUE8)
                {
                    // Simulate typical application
                    // Only if it is not Ip
                    if (CSettings.ComPort != 1)
                    {
                        Thread.Sleep(30);
                    }
                    command = HartDeviceDLL.BAHASL_ExecuteCommandInterpreter();
                    if (m_active_command == 0xffff)
                    {
                        m_active_command = command;
                    }
                }
            }
        }


        #endregion Worker

        #region Tab Interface
        private void ChkViewPreambles_CheckedChanged(object sender, EventArgs e)
        {
            ViewPreambles();
        }

        private void ChkViewFrameNumbers_CheckedChanged(object sender, EventArgs e)
        {
            ViewFrameNumbers();
        }

        private void ChkViewAddress_CheckedChanged(object sender, EventArgs e)
        {
            ViewAddress();
        }

        private void ChkViewDecodedData_CheckedChanged(object sender, EventArgs e)
        {
            ViewDecodedData();
        }

        private void ChkViewTiming_CheckedChanged(object sender, EventArgs e)
        {
            ViewTiming();
        }

        private void ChkViewStatusBinary_CheckedChanged(object sender, EventArgs e)
        {
            ViewStatusBinary();
        }

        private void ChkViewShortBursts_CheckedChanged(object sender, EventArgs e)
        {
            ViewShortBursts();
        }

        private void ChkBurstMode_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkBurstMode.Checked)
            {
                CTestSlave.SetBurstMode(EN_Bool.TRUE8);
            }
            else
            {
                CTestSlave.SetBurstMode(EN_Bool.FALSE8);
            }
        }

        private void ChkSimWriteProtect_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkSimWriteProtect.Checked)
            {
                CTestSlave.SetWriteProtected(EN_Bool.TRUE8);
            }
            else
            {
                CTestSlave.SetWriteProtected(EN_Bool.FALSE8);
            }
        }

        private void CmbComPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_no_control_action = true;
            if (CmbComPort.SelectedIndex == 1)
            {
                GrpHartFraming.Visible = false;
                ChkHartIpEnabled.Checked = true;
            }
            else
            {
                GrpHartFraming.Visible = true;
                ChkHartIpEnabled.Checked = false;
            }
            m_no_control_action = false;

            if (CSettings.ComPort != CmbComPort.SelectedIndex)
            {
                CSettings.ComPort = (byte)CmbComPort.SelectedIndex;
                CDevStatus.CMonitor.CfgChanged = true;
                CDevStatus.CGeneral.CfgChanged = true;
                CDevStatus.CfgChanged = true;
                HandleControlEvent();
                CheckHartIpVisibility();
            }
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
                CSettings.PollAddress = NewPollAddress;
                CDevStatus.CMonitor.CfgChanged = true;
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
                CDevStatus.CMonitor.CfgChanged = true;
                CDevStatus.CGeneral.CfgChanged = true;
                CDevStatus.CfgChanged = true;
                HandleControlEvent();
            }
        }

        private void CmbNumReqPreambles_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte NewNumReqPreAmbs;

            if ((CmbReqPreambles.SelectedIndex >= 0) && (CmbReqPreambles.SelectedIndex < 23))
            {
                NewNumReqPreAmbs = (byte)(CmbReqPreambles.SelectedIndex);
            }
            else
            {
                NewNumReqPreAmbs = 5;
            }
            if (CSettings.NumReqPreambles != NewNumReqPreAmbs)
            {
                CSettings.NumReqPreambles = NewNumReqPreAmbs;
                CDevStatus.CMonitor.CfgChanged = true;
                CDevStatus.CGeneral.CfgChanged = true;
                CDevStatus.CfgChanged = true;
                HandleControlEvent();
            }
        }
        private void CmbRspPreambles_SelectedIndexChanged(object sender, EventArgs e)
        {
            byte NewNumRspPreAmbs;

            if ((CmbRspPreambles.SelectedIndex >= 0) && (CmbRspPreambles.SelectedIndex < 23))
            {
                NewNumRspPreAmbs = (byte)(CmbRspPreambles.SelectedIndex);
            }
            else
            {
                NewNumRspPreAmbs = 5;
            }
            if (CSettings.NumRspPreambles != NewNumRspPreAmbs)
            {
                CSettings.NumRspPreambles = NewNumRspPreAmbs;
                CDevStatus.CMonitor.CfgChanged = true;
                CDevStatus.CGeneral.CfgChanged = true;
                CDevStatus.CfgChanged = true;
                HandleControlEvent();
            }
        }
        #endregion Tab Interface

        #region Tab Identifier
        private void LinkCmd0_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20127/TS20127/7.2/#page=8";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void LinkTable1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=10";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void LinkTable8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=102";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void LinkTable17_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=111";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void LinkTable10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=108";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void LinkTable57_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=126";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        private void linkTable26_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=114";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void TxtDevTypeTable1_TextChanged(object sender, EventArgs e)
        {
            TxtDevTypeTable1.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtDevRev_TextChanged(object sender, EventArgs e)
        {
            TxtDevRev.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtSwRev_TextChanged(object sender, EventArgs e)
        {
            TxtSwRev.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtHwRev_TextChanged(object sender, EventArgs e)
        {
            TxtHwRev.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtSignalTable10_TextChanged(object sender, EventArgs e)
        {
            TxtSignalTable10.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtFlagsTable11_TextChanged(object sender, EventArgs e)
        {
            TxtFlagsTable11.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtDevUniqueID_TextChanged(object sender, EventArgs e)
        {
            TxtDevUniqueID.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtLastDevVarCode_TextChanged(object sender, EventArgs e)
        {
            TxtLastDevVarCode.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtConfigChangedCounter_TextChanged(object sender, EventArgs e)
        {
            TxtConfigChangedCounter.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtExtDevStatus_TextChanged(object sender, EventArgs e)
        {
            TxtExtDevStatus.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtManufacturerTable8_TextChanged(object sender, EventArgs e)
        {
            TxtManufacturerTable8.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtDistributorTable8_TextChanged(object sender, EventArgs e)
        {
            TxtDistributorTable8.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtProfileTable57_TextChanged(object sender, EventArgs e)
        {
            TxtProfileTable57.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }
        #endregion Tab Identifier

        #region Tab Device
        private void TxtLongTag_TextChanged(object sender, EventArgs e)
        {
            TxtLongTag.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtShortTag_TextChanged(object sender, EventArgs e)
        {
            TxtShortTag.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtDescriptor_TextChanged(object sender, EventArgs e)
        {
            TxtDescriptor.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtDay_TextChanged(object sender, EventArgs e)
        {
            TxtDay.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtMonth_TextChanged(object sender, EventArgs e)
        {
            TxtMonth.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtYear_TextChanged(object sender, EventArgs e)
        {
            TxtYear.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtMessage_TextChanged(object sender, EventArgs e)
        {
            TxtMessage.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }
        private void TxtChanFlags_TextChanged(object sender, EventArgs e)
        {
            TxtChanFlags.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void CmbTFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbTFunction.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void CmbRangeUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbRangeUnit.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtUpperRange_TextChanged(object sender, EventArgs e)
        {
            TxtUpperRange.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtLowerRange_TextChanged(object sender, EventArgs e)
        {
            TxtLowerRange.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtDamping_TextChanged(object sender, EventArgs e)
        {
            TxtDamping.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void CmbWrProt_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbWrProt.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void CmbAlmSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbAlmSelect.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtFinAssNum_TextChanged(object sender, EventArgs e)
        {
            TxtFinAssNum.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtCountryLetters_TextChanged(object sender, EventArgs e)
        {
            TxtCountryLetters.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void ChkSIunitsOnly_CheckedChanged(object sender, EventArgs e)
        {
            ChkSIunitsOnly.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }
        #endregion Tab Device

        #region Tab Transducer
        private void linkTable26_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=114";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void TxtTransdSerNum_TextChanged(object sender, EventArgs e)
        {
            TxtTransdSerNum.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void CmbTransdUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbTransdUnit.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtUpperLimit_TextChanged(object sender, EventArgs e)
        {
            TxtUpperLimit.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtLowerLimit_TextChanged(object sender, EventArgs e)
        {
            TxtLowerLimit.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtMinSpan_TextChanged(object sender, EventArgs e)
        {
            TxtMinSpan.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }
        #endregion Tab Transducer

        #region Tab DevVars
        private void RadPV1good_CheckedChanged(object sender, EventArgs e)
        {
            RadPV1good.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPerClass_TextChanged(object sender, EventArgs e)
        {
            TxtPerClass.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPerValue_TextChanged(object sender, EventArgs e)
        {
            TxtPerValue.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadPerGood_CheckedChanged(object sender, EventArgs e)
        {
            RadPerGood.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadPercBad_CheckedChanged(object sender, EventArgs e)
        {
            RadPercBad.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtCurClass_TextChanged(object sender, EventArgs e)
        {
            TxtCurClass.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtCurValue_TextChanged(object sender, EventArgs e)
        {
            TxtCurValue.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadCurGood_CheckedChanged(object sender, EventArgs e)
        {
            RadCurGood.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadCurBad_CheckedChanged(object sender, EventArgs e)
        {
            RadCurBad.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPV1class_TextChanged(object sender, EventArgs e)
        {
            TxtPV1class.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPV1value_TextChanged(object sender, EventArgs e)
        {
            TxtPV1value.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void CmbPV1unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbPV1unit.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadPV1bad_CheckedChanged(object sender, EventArgs e)
        {
            RadPV1bad.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPV2class_TextChanged(object sender, EventArgs e)
        {
            TxtPV2class.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPV2value_TextChanged(object sender, EventArgs e)
        {
            TxtPV2value.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void CmbPV2unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbPV2unit.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadPV2good_CheckedChanged(object sender, EventArgs e)
        {
            RadPV2good.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadPV2bad_CheckedChanged(object sender, EventArgs e)
        {
            RadPV2bad.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPV3class_TextChanged(object sender, EventArgs e)
        {
            TxtPV3class.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPV3value_TextChanged(object sender, EventArgs e)
        {
            TxtPV3value.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void CmbPV3unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbPV3unit.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadPV3good_CheckedChanged(object sender, EventArgs e)
        {
            RadPV3good.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadPV3bad_CheckedChanged(object sender, EventArgs e)
        {
            RadPV3bad.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPV4class_TextChanged(object sender, EventArgs e)
        {
            TxtPV4class.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtPV4value_TextChanged(object sender, EventArgs e)
        {
            TxtPV4value.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void CmbPV4unit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbPV4unit.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadPV4good_CheckedChanged(object sender, EventArgs e)
        {
            RadPV4good.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void RadPV4bad_CheckedChanged(object sender, EventArgs e)
        {
            RadPV4bad.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }
        #endregion Tab DevVars

        #region Tab Add Status
        private void linkTable29_31_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=117";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void linkTable32_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=118";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void linkTable27_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://library.fieldcommgroup.org/20183/TS20183/27.0/#page=115";
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }

        private void TxtAddDev0_TextChanged(object sender, EventArgs e)
        {
            TxtAddDev0.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDev1_TextChanged(object sender, EventArgs e)
        {
            TxtAddDev1.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDev2_TextChanged(object sender, EventArgs e)
        {
            TxtAddDev2.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDev3_TextChanged(object sender, EventArgs e)
        {
            TxtAddDev3.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDev4_TextChanged(object sender, EventArgs e)
        {
            TxtAddDev4.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDevStandard0_TextChanged(object sender, EventArgs e)
        {
            TxtAddDevStandard0.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDevStandard1_TextChanged(object sender, EventArgs e)
        {
            TxtAddDevStandard1.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDevStandard2_TextChanged(object sender, EventArgs e)
        {
            TxtAddDevStandard2.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDevStandard3_TextChanged(object sender, EventArgs e)
        {
            TxtAddDevStandard3.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDevStaturated_TextChanged(object sender, EventArgs e)
        {
            TxtAddDevStaturated.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }

        private void TxtAddDevFixed_TextChanged(object sender, EventArgs e)
        {
            TxtAddDevFixed.BackColor = Color.LightYellow;
            butMnuUpdateData.BackColor = Color.LightYellow;
        }
        #endregion Tab Add Status

        #region Tab Options
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
        #endregion Tab Options
        #endregion Event Procedures

        #region Monitor Handling
        internal void HandleMonitorData()
        {
            if (HartDeviceDLL.BAHASL_GetMonitorData(ref HartDeviceDLL.MonFrame) != 0)
            {
                if (CSettings.ModeMonitorFrames != false)
                {
                    if (IsMonitoringAllowed() && !is_max_frame_number)
                    {
                        CFrameData clFrameData = new CFrameData(ref HartDeviceDLL.MonFrame);
                        mo_hart_frames.Add(clFrameData);
                        if (CmbComPort.SelectedIndex == 1)
                        {
                            if (clFrameData.Burst)
                            {
                                HandleMonitorAddData();
                            }
                            else if (clFrameData.Response)
                            {
                                HandleMonitorAddData();
                            }
                        }
                        m_number_frames++;

                        if (m_number_frames == MAX_NUM_FRAMES)
                        {
                            is_max_frame_number = true;
                            SetStatusInfo();
                        }

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
            int monitorStatus = HartDeviceDLL.BAHASL_GetMonitorStatus();

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

            len = HartDeviceDLL.BAHASL_GetMonitorAddData(ref data[0]);
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

            if (CTestSlave.IsComPortOpen)
            {
                if (CSettings.ComPort == com_port_)
                {
                    // No change happened
                    HartDeviceDLL.BAHASL_StopMonitor();
                    result = true;
                }
                else
                {
                    HartDeviceDLL.BAHASL_StopMonitor();
                    HartDeviceDLL.BAHASL_CloseChannel();
                    // Try to open new com port
                    if (HartDeviceDLL.BAHASL_OpenChannel(com_port_) == EN_Bool.TRUE8)
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
                if (HartDeviceDLL.BAHASL_OpenChannel(com_port_) == EN_Bool.TRUE8)
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
                CTestSlave.IsComPortOpen = true;
                HartDeviceDLL.BAHASL_StartMonitor();
                SetModeMonitorFrames();
            }
            else
            {
                CTestSlave.IsComPortOpen = false;
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

            if (is_max_frame_number)
            {
                s += " | Monitoring off | " + "Maximum " + MAX_NUM_FRAMES.ToString() + " Frames. Restart Slave.";
                StatusInfo.BackColor = CSettings.StatBarColBkNotRec;
            }
            else
            {
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
            }

            StatusInfo.Items[1].Text = s;
        }
        #endregion

        #region Helpers
        void DisplayTitle()
        {
            Text = CMALib.CGlobText.TEST_CPP_SLAVE;
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
            butMnuUpdateData.Left = r.Width - butMnuUpdateData.Width;
            if (m_show_menu == true)
            {
                tabMain.Top = panTop.Height;
                tabMain.Left = 0;
                tabMain.Width = r.Width;
                ArrangeWorkArea(panTop.Height + tabMain.Height, 0, r.Width, r.Height - panTop.Height - tabMain.Height - StatusInfo.Height);
                tabMain.Visible = true;
            }
            else
            {
                tabMain.Visible = false;
                ArrangeWorkArea(panTop.Height, 0, r.Width, r.Height - panTop.Height - StatusInfo.Height);
            }
            // Sort out the values on the top panel
            int requiredWidth = butMnuClearDisplay.Width + 5 + butMnuRecord.Width + 5 + butMnuInfo.Width + 5 + butMnuUpdateData.Width;

            if (requiredWidth < r.Width)
            {
                leftPos = r.Width - requiredWidth;
            }
            else
            {
                leftPos = 0;
            }

            butMnuUpdateData.Left = leftPos;
            leftPos += butMnuUpdateData.Width + 5;
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
            return CTestSlave.IsComPortOpen;
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
            if (CTestSlave.IsComPortOpen)
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
        }

        private void ApplyColors()
        {
            if (m_last_color_set != CSettings.ColorSet)
            {
                m_last_color_set = CSettings.ColorSet;
                ApplyMonitorColors();
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

        #region Cyclic handlers
        internal void HandleWatchingData()
        {
            HandleMonitorData();
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

        #region Hart Ip Handling
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

        internal void HandleHartIpStatus()
        {
            ushort total_status = HartDeviceDLL.BAHASL_GetHartIpStatus();
            if (total_status != m_last_ip_status)
            {
                string txt_status = string.Empty;
                m_last_ip_status = total_status;

                // State machine
                byte status = (byte)(((total_status & 0xFF00) >> 8) & 0x1f);
                switch (status)
                {
                    case 0:
                        txt_status = "IDLE";
                        break;
                    case 1:
                        txt_status = "INITIALIZING";
                        break;
                    case 2:
                        txt_status = "WAIT CONNECT";
                        break;
                    case 3:
                        txt_status = "SERVER READY";
                        break;
                    case 4:
                        txt_status = "WAIT INITIATE";
                        break;
                    case 5:
                        txt_status = "WAIT RESPONSE";
                        break;
                    case 6:
                        txt_status = "SHUTTING DOWN";
                        break;
                    default:
                        txt_status = "UNKNOWN";
                        break;
                }

                TxtConnectionStatus.Text = txt_status;

                // Last error
                string txt_last_error;
                byte last_error = (byte)(total_status & 0xFF);
                switch (last_error)
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
                        txt_last_error = "BIND";
                        break;
                    case 5:
                        txt_last_error = "LISTEN";
                        break;
                    case 6:
                        txt_last_error = "ACCEPT";
                        break;
                    case 7:
                        txt_last_error = "TX FAILED";
                        break;
                    case 8:
                        txt_last_error = "SHUTDOWN";
                        break;
                    case 9:
                        txt_last_error = "RECEIVING";
                        break;
                    case 10:
                        txt_last_error = "SET TIMEOUT";
                        break;
                    default:
                        txt_last_error = "UNKNOWN";
                        break;
                }

                TxtLastError.Text = txt_last_error;
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
                    UpdateSlaveData();
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

        private void UpdateSlaveData()
        {
            butMnuUpdateData.Visible = false;
            butMnuUpdateData.BackColor = Color.Transparent;
            butMnuUpdateData.Refresh();
            System.Threading.Thread.Sleep(300);
            CTestSlave.UpdateSlaveData();
            butMnuUpdateData.Visible = true;
            butMnuUpdateData.Refresh();
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
        #endregion Hart Ip Handling 
    }
}
