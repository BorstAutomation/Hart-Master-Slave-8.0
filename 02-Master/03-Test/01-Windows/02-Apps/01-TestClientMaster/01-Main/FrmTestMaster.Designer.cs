using System.ComponentModel;

namespace BaTestHart
{
    partial class FrmTestClient
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param m_name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmTestClient));
            CtxMnuDecode = new ContextMenuStrip(components);
            tsmnuDecInteger = new ToolStripMenuItem();
            tsmnuDecFloat = new ToolStripMenuItem();
            tsmnuDecDouble = new ToolStripMenuItem();
            tsmnuDecHartUnit = new ToolStripMenuItem();
            tsmnuDecPackedASCII = new ToolStripMenuItem();
            tsmnuDecText = new ToolStripMenuItem();
            tsmnuDecBinary = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            tsmnuCopyToAnyFrame = new ToolStripMenuItem();
            tsmnuBytesToClipBoard = new ToolStripMenuItem();
            TimWatch = new System.Windows.Forms.Timer(components);
            tabMain = new TabControl();
            tabStart = new TabPage();
            ChkViewShortBursts = new CheckBox();
            groupBox8 = new GroupBox();
            TxtConStatus = new TextBox();
            ButDisconnect = new Button();
            ButConnect = new Button();
            groupBox6 = new GroupBox();
            CmbNumPreambles = new ComboBox();
            CmbMasterRole = new ComboBox();
            label5 = new Label();
            lblPreambles = new Label();
            groupBox4 = new GroupBox();
            CmbBaudRate = new ComboBox();
            label4 = new Label();
            label1 = new Label();
            label3 = new Label();
            CmbComPort = new ComboBox();
            CmbPollAddress = new ComboBox();
            groupBox5 = new GroupBox();
            ChkViewStatusBinary = new CheckBox();
            ChkViewDecodedData = new CheckBox();
            ChkViewFrameNumbers = new CheckBox();
            ChkViewTiming = new CheckBox();
            ChkViewAddress = new CheckBox();
            ChkViewPreambles = new CheckBox();
            tabHartIp = new TabPage();
            ChkHartIpSendKeepAlive = new CheckBox();
            ButSetDefault = new Button();
            ChkHartIpEnabled = new CheckBox();
            TxtLastError = new TextBox();
            label72 = new Label();
            label22 = new Label();
            TxtConnectionStatus = new TextBox();
            ChkHartIpUseAddress = new CheckBox();
            label17 = new Label();
            rtxIPFrames = new RichTextBox();
            TxtHartIpPort = new TextBox();
            label14 = new Label();
            TxtHartIpAddress = new TextBox();
            label12 = new Label();
            TxtHartIpHostName = new TextBox();
            label11 = new Label();
            tabCommands = new TabPage();
            label2 = new Label();
            groupBox2 = new GroupBox();
            ButSendUserCmd = new Button();
            ButEditCmd31 = new Button();
            ButSendCmd31 = new Button();
            ButEditUserCmd = new Button();
            groupBox1 = new GroupBox();
            ButSendCmd35 = new Button();
            ButEditCmd35 = new Button();
            groupBox9 = new GroupBox();
            ButSendCmd38 = new Button();
            ButSendCmd48 = new Button();
            ButEditCmd18 = new Button();
            ButSendCmd18 = new Button();
            ButSendCmd15 = new Button();
            ButSendCmd14 = new Button();
            ButSendCmd13 = new Button();
            ButSendCmd12 = new Button();
            ButSendCmd2 = new Button();
            ButSendCmd3 = new Button();
            butSendCmd1 = new Button();
            ButSendCmd0short = new Button();
            tabOptions = new TabPage();
            rtxtTestDisplay = new RichTextBox();
            groupBox22 = new GroupBox();
            ButFreeTest = new Button();
            ButUserTestCommands = new Button();
            ButUserTestCoding = new Button();
            groupBox19 = new GroupBox();
            ChkRetryIfBusy = new CheckBox();
            CmbNumRetries = new ComboBox();
            label19 = new Label();
            groupBox16 = new GroupBox();
            ButEditColors = new Button();
            ChkTopMost = new CheckBox();
            RadColorSetUser = new RadioButton();
            RadColorSet2 = new RadioButton();
            RadColorSet1 = new RadioButton();
            tabDevData = new TabPage();
            ButReadAll = new Button();
            label25 = new Label();
            TxtDevDatCurrent = new TextBox();
            label24 = new Label();
            LblDevDatSpanUnit = new Label();
            TxtDevDatSpan = new TextBox();
            label21 = new Label();
            LblDevDatUnitPV = new Label();
            TxtDevDatPV = new TextBox();
            label18 = new Label();
            LblDevDatUnitSV = new Label();
            TxtDevDatSV = new TextBox();
            label16 = new Label();
            LblDevDatUnitTV = new Label();
            TxtDevDatTV = new TextBox();
            label15 = new Label();
            LblDevDatUnitQV = new Label();
            TxtDevDatQV = new TextBox();
            LblDevDatRangeUnitLo = new Label();
            TxtDevDatLowerRange = new TextBox();
            label13 = new Label();
            LblDevDatRangeUnitUp = new Label();
            TxtDevDatUpperRange = new TextBox();
            label10 = new Label();
            TxtDevDatMessage = new TextBox();
            label9 = new Label();
            TxtDevDatLongTag = new TextBox();
            label8 = new Label();
            TxtDevDatShortTag = new TextBox();
            label7 = new Label();
            TxtDevDatUniqueID = new TextBox();
            label6 = new Label();
            panTop = new Panel();
            butMnuInfo = new Button();
            butMnuClearDisplay = new Button();
            butMnuRecord = new Button();
            StatusInfo = new StatusStrip();
            MainStatusComPortLED = new ToolStripStatusLabel();
            MainStatusInfo = new ToolStripStatusLabel();
            MainStatusLED = new ToolStripStatusLabel();
            MainStatusNumFrames = new ToolStripStatusLabel();
            Tled = new ToolStripStatusLabel();
            TimOperating = new System.Windows.Forms.Timer(components);
            ToolTipDecodedData = new ToolTip(components);
            ToolTipInfo = new ToolTip(components);
            rtxtDisplay = new RichTextBox();
            ImgL_Leds = new ImageList(components);
            CtxMnuDecode.SuspendLayout();
            tabMain.SuspendLayout();
            tabStart.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            tabHartIp.SuspendLayout();
            tabCommands.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox9.SuspendLayout();
            tabOptions.SuspendLayout();
            groupBox22.SuspendLayout();
            groupBox19.SuspendLayout();
            groupBox16.SuspendLayout();
            tabDevData.SuspendLayout();
            panTop.SuspendLayout();
            StatusInfo.SuspendLayout();
            SuspendLayout();
            // 
            // CtxMnuDecode
            // 
            CtxMnuDecode.Items.AddRange(new ToolStripItem[] { tsmnuDecInteger, tsmnuDecFloat, tsmnuDecDouble, tsmnuDecHartUnit, tsmnuDecPackedASCII, tsmnuDecText, tsmnuDecBinary, toolStripSeparator1, tsmnuCopyToAnyFrame, tsmnuBytesToClipBoard });
            CtxMnuDecode.Name = "ctxMnuDecode";
            CtxMnuDecode.Size = new Size(172, 208);
            CtxMnuDecode.Opened += CtxMnuDecode_Opened;
            // 
            // tsmnuDecInteger
            // 
            tsmnuDecInteger.Name = "tsmnuDecInteger";
            tsmnuDecInteger.Size = new Size(171, 22);
            tsmnuDecInteger.Text = "Integer";
            tsmnuDecInteger.Click += TsmnuDecInteger_Click;
            // 
            // tsmnuDecFloat
            // 
            tsmnuDecFloat.Name = "tsmnuDecFloat";
            tsmnuDecFloat.Size = new Size(171, 22);
            tsmnuDecFloat.Text = "Float";
            tsmnuDecFloat.Click += TsmnuDecFloat_Click;
            // 
            // tsmnuDecDouble
            // 
            tsmnuDecDouble.Name = "tsmnuDecDouble";
            tsmnuDecDouble.Size = new Size(171, 22);
            tsmnuDecDouble.Text = "Double";
            tsmnuDecDouble.Click += TsmnuDecDouble_Click;
            // 
            // tsmnuDecHartUnit
            // 
            tsmnuDecHartUnit.Name = "tsmnuDecHartUnit";
            tsmnuDecHartUnit.Size = new Size(171, 22);
            tsmnuDecHartUnit.Text = "HartUnit";
            tsmnuDecHartUnit.Click += TsmnuDecHartUnit_Click;
            // 
            // tsmnuDecPackedASCII
            // 
            tsmnuDecPackedASCII.Name = "tsmnuDecPackedASCII";
            tsmnuDecPackedASCII.Size = new Size(171, 22);
            tsmnuDecPackedASCII.Text = "PackedASCII";
            tsmnuDecPackedASCII.Click += TsmnuDecPackedASCII_Click;
            // 
            // tsmnuDecText
            // 
            tsmnuDecText.Name = "tsmnuDecText";
            tsmnuDecText.Size = new Size(171, 22);
            tsmnuDecText.Text = "Text";
            tsmnuDecText.Click += TsmnuDecText_Click;
            // 
            // tsmnuDecBinary
            // 
            tsmnuDecBinary.Name = "tsmnuDecBinary";
            tsmnuDecBinary.Size = new Size(171, 22);
            tsmnuDecBinary.Text = "Binary";
            tsmnuDecBinary.Click += TsmnuDecBinary_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(168, 6);
            // 
            // tsmnuCopyToAnyFrame
            // 
            tsmnuCopyToAnyFrame.Name = "tsmnuCopyToAnyFrame";
            tsmnuCopyToAnyFrame.Size = new Size(171, 22);
            // 
            // tsmnuBytesToClipBoard
            // 
            tsmnuBytesToClipBoard.Name = "tsmnuBytesToClipBoard";
            tsmnuBytesToClipBoard.Size = new Size(171, 22);
            tsmnuBytesToClipBoard.Text = "Bytes to Clipboard";
            tsmnuBytesToClipBoard.Click += TsmnuBytesToClipBoard_Click;
            // 
            // TimWatch
            // 
            TimWatch.Enabled = true;
            TimWatch.Interval = 10;
            TimWatch.Tick += TimWatch_Tick;
            // 
            // tabMain
            // 
            tabMain.Appearance = TabAppearance.FlatButtons;
            tabMain.Controls.Add(tabStart);
            tabMain.Controls.Add(tabHartIp);
            tabMain.Controls.Add(tabCommands);
            tabMain.Controls.Add(tabOptions);
            tabMain.Controls.Add(tabDevData);
            tabMain.Location = new Point(0, 37);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(784, 165);
            tabMain.TabIndex = 2;
            // 
            // tabStart
            // 
            tabStart.Controls.Add(ChkViewShortBursts);
            tabStart.Controls.Add(groupBox8);
            tabStart.Controls.Add(groupBox6);
            tabStart.Controls.Add(groupBox4);
            tabStart.Controls.Add(groupBox5);
            tabStart.Location = new Point(4, 27);
            tabStart.Name = "tabStart";
            tabStart.Padding = new Padding(3);
            tabStart.Size = new Size(776, 134);
            tabStart.TabIndex = 1;
            tabStart.Text = "Start";
            tabStart.UseVisualStyleBackColor = true;
            // 
            // ChkViewShortBursts
            // 
            ChkViewShortBursts.Location = new Point(392, 104);
            ChkViewShortBursts.Name = "ChkViewShortBursts";
            ChkViewShortBursts.Size = new Size(121, 25);
            ChkViewShortBursts.TabIndex = 41;
            ChkViewShortBursts.Text = "View Short Bursts";
            ToolTipInfo.SetToolTip(ChkViewShortBursts, "Show start time and duration of frames");
            ChkViewShortBursts.UseVisualStyleBackColor = true;
            ChkViewShortBursts.CheckedChanged += ChkViewShortBursts_CheckedChanged;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(TxtConStatus);
            groupBox8.Controls.Add(ButDisconnect);
            groupBox8.Controls.Add(ButConnect);
            groupBox8.Location = new Point(6, 4);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(107, 124);
            groupBox8.TabIndex = 40;
            groupBox8.TabStop = false;
            groupBox8.Text = "Hart Device";
            // 
            // TxtConStatus
            // 
            TxtConStatus.BackColor = SystemColors.Info;
            TxtConStatus.BorderStyle = BorderStyle.FixedSingle;
            TxtConStatus.Location = new Point(6, 94);
            TxtConStatus.Name = "TxtConStatus";
            TxtConStatus.ReadOnly = true;
            TxtConStatus.Size = new Size(90, 23);
            TxtConStatus.TabIndex = 64;
            TxtConStatus.Text = "Not Connected";
            // 
            // ButDisconnect
            // 
            ButDisconnect.BackColor = SystemColors.Control;
            ButDisconnect.Enabled = false;
            ButDisconnect.FlatStyle = FlatStyle.Popup;
            ButDisconnect.Location = new Point(6, 59);
            ButDisconnect.Name = "ButDisconnect";
            ButDisconnect.Size = new Size(83, 25);
            ButDisconnect.TabIndex = 3;
            ButDisconnect.Text = "Disconnect";
            ButDisconnect.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButDisconnect, "Delete Unique Identifier");
            ButDisconnect.UseVisualStyleBackColor = false;
            ButDisconnect.Click += ButDisconnect_Click;
            // 
            // ButConnect
            // 
            ButConnect.BackColor = Color.PowderBlue;
            ButConnect.FlatStyle = FlatStyle.Popup;
            ButConnect.Location = new Point(6, 25);
            ButConnect.Name = "ButConnect";
            ButConnect.Size = new Size(83, 25);
            ButConnect.TabIndex = 2;
            ButConnect.Text = "Connect";
            ButConnect.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButConnect, "Read Unique Identifier");
            ButConnect.UseVisualStyleBackColor = false;
            ButConnect.Click += ButConnect_Click;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(CmbNumPreambles);
            groupBox6.Controls.Add(CmbMasterRole);
            groupBox6.Controls.Add(label5);
            groupBox6.Controls.Add(lblPreambles);
            groupBox6.Location = new Point(326, 4);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(187, 94);
            groupBox6.TabIndex = 39;
            groupBox6.TabStop = false;
            groupBox6.Text = "Hart Behavior";
            // 
            // CmbNumPreambles
            // 
            CmbNumPreambles.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbNumPreambles.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22" });
            CmbNumPreambles.Location = new Point(92, 25);
            CmbNumPreambles.Name = "CmbNumPreambles";
            CmbNumPreambles.Size = new Size(89, 23);
            CmbNumPreambles.TabIndex = 21;
            ToolTipInfo.SetToolTip(CmbNumPreambles, "Range 2..20, the standard is 5");
            CmbNumPreambles.SelectedIndexChanged += CmbNumPreambles_SelectedIndexChanged;
            // 
            // CmbMasterRole
            // 
            CmbMasterRole.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbMasterRole.Items.AddRange(new object[] { "Primary", "Secondary" });
            CmbMasterRole.Location = new Point(92, 58);
            CmbMasterRole.Name = "CmbMasterRole";
            CmbMasterRole.Size = new Size(89, 23);
            CmbMasterRole.TabIndex = 38;
            ToolTipInfo.SetToolTip(CmbMasterRole, "If another master is connected this setting may be required");
            CmbMasterRole.SelectedIndexChanged += CmbMasterRole_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.Location = new Point(6, 57);
            label5.Name = "label5";
            label5.Size = new Size(79, 24);
            label5.TabIndex = 37;
            label5.Text = "Master Type:";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblPreambles
            // 
            lblPreambles.Location = new Point(7, 23);
            lblPreambles.Name = "lblPreambles";
            lblPreambles.Size = new Size(78, 25);
            lblPreambles.TabIndex = 31;
            lblPreambles.Text = "Preambles:";
            lblPreambles.TextAlign = ContentAlignment.MiddleRight;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(CmbBaudRate);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(label1);
            groupBox4.Controls.Add(label3);
            groupBox4.Controls.Add(CmbComPort);
            groupBox4.Controls.Add(CmbPollAddress);
            groupBox4.Location = new Point(119, 4);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(201, 124);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Interface Settings";
            // 
            // CmbBaudRate
            // 
            CmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbBaudRate.Items.AddRange(new object[] { "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" });
            CmbBaudRate.Location = new Point(96, 94);
            CmbBaudRate.Name = "CmbBaudRate";
            CmbBaudRate.Size = new Size(83, 23);
            CmbBaudRate.TabIndex = 34;
            ToolTipInfo.SetToolTip(CmbBaudRate, "The Standard Baud Rate is 1200. For Multiplexers other baud rates may be required");
            CmbBaudRate.SelectedIndexChanged += CmbBaudRate_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.Location = new Point(20, 89);
            label4.Name = "label4";
            label4.Size = new Size(70, 24);
            label4.TabIndex = 33;
            label4.Text = "Baudrate:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Location = new Point(17, 23);
            label1.Name = "label1";
            label1.Size = new Size(73, 25);
            label1.TabIndex = 30;
            label1.Text = "Com Port:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new Point(6, 56);
            label3.Name = "label3";
            label3.Size = new Size(84, 24);
            label3.TabIndex = 8;
            label3.Text = "Poll Address:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbComPort
            // 
            CmbComPort.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbComPort.FlatStyle = FlatStyle.Popup;
            CmbComPort.Items.AddRange(new object[] { "None", "Hart IP", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10", "COM11", "COM12", "COM13", "COM14", "COM15", "COM16", "COM17", "COM18", "COM19", "COM20", "COM21", "COM22", "COM23", "COM24", "COM25", "COM26", "COM27", "COM28", "COM29", "COM30", "COM31", "COM32", "COM33", "COM34", "COM35", "COM36", "COM37", "COM38", "COM39", "COM40", "COM41", "COM42", "COM43", "COM44", "COM45", "COM46", "COM47", "COM48", "COM49", "COM50", "COM51", "COM52", "COM53", "COM54", "COM55", "COM56", "COM57", "COM58", "COM59", "COM60", "COM61", "COM62", "COM63", "COM64", "COM65", "COM66", "COM67", "COM68", "COM69", "COM70", "COM71", "COM72", "COM73", "COM74", "COM75", "COM76", "COM77", "COM78", "COM79", "COM80", "COM81", "COM82", "COM83", "COM84", "COM85", "COM86", "COM87", "COM88", "COM89", "COM90", "COM91", "COM92", "COM93", "COM94", "COM95", "COM96", "COM97", "COM98", "COM99", "COM100", "COM101", "COM102", "COM103", "COM104", "COM105", "COM106", "COM107", "COM108", "COM109", "COM110", "COM111", "COM112", "COM113", "COM114", "COM115", "COM116", "COM117", "COM118", "COM119", "COM120", "COM121", "COM122", "COM123", "COM124", "COM125", "COM126", "COM127", "COM128", "COM129", "COM130", "COM131", "COM132", "COM133", "COM134", "COM135", "COM136", "COM137", "COM138", "COM139", "COM140", "COM141", "COM142", "COM143", "COM144", "COM145", "COM146", "COM147", "COM148", "COM149", "COM150", "COM151", "COM152", "COM153", "COM154", "COM155", "COM156", "COM157", "COM158", "COM159", "COM160", "COM161", "COM162", "COM163", "COM164", "COM165", "COM166", "COM167", "COM168", "COM169", "COM170", "COM171", "COM172", "COM173", "COM174", "COM175", "COM176", "COM177", "COM178", "COM179", "COM180", "COM181", "COM182", "COM183", "COM184", "COM185", "COM186", "COM187", "COM188", "COM189", "COM190", "COM191", "COM192", "COM193", "COM194", "COM195", "COM196", "COM197", "COM198", "COM199", "COM200", "COM201", "COM202", "COM203", "COM204", "COM205", "COM206", "COM207", "COM208", "COM209", "COM210", "COM211", "COM212", "COM213", "COM214", "COM215", "COM216", "COM217", "COM218", "COM219", "COM220", "COM221", "COM222", "COM223", "COM224", "COM225", "COM226", "COM227", "COM228", "COM229", "COM230", "COM231", "COM232", "COM233", "COM234", "COM235", "COM236", "COM237", "COM238", "COM239", "COM240", "COM241", "COM242", "COM243", "COM244", "COM245", "COM246", "COM247", "COM248", "COM249", "COM250" });
            CmbComPort.Location = new Point(96, 28);
            CmbComPort.Name = "CmbComPort";
            CmbComPort.Size = new Size(96, 23);
            CmbComPort.TabIndex = 29;
            ToolTipInfo.SetToolTip(CmbComPort, "Com Port for the Hart Device Connection");
            CmbComPort.SelectedIndexChanged += CmbComPort_SelectedIndexChanged;
            // 
            // CmbPollAddress
            // 
            CmbPollAddress.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPollAddress.FlatStyle = FlatStyle.Flat;
            CmbPollAddress.Items.AddRange(new object[] { "A_00", "A_01", "A_02", "A_03", "A_04", "A_05", "A_06", "A_07", "A_08", "A_09", "A_10", "A_11", "A_12", "A_13", "A_14", "A_15", "A_16", "A_17", "A_18", "A_19", "A_20", "A_21", "A_22", "A_23", "A_24", "A_25", "A_26", "A_27", "A_28", "A_29", "A_30", "A_31", "A_32", "A_33", "A_34", "A_35", "A_36", "A_37", "A_38", "A_39", "A_40", "A_41", "A_42", "A_43", "A_44", "A_45", "A_46", "A_47", "A_48", "A_49", "A_50", "A_51", "A_52", "A_53", "A_54", "A_55", "A_56", "A_57", "A_58", "A_59", "A_60", "A_61", "A_62", "A_63" });
            CmbPollAddress.Location = new Point(96, 61);
            CmbPollAddress.Name = "CmbPollAddress";
            CmbPollAddress.Size = new Size(96, 23);
            CmbPollAddress.TabIndex = 7;
            ToolTipInfo.SetToolTip(CmbPollAddress, "Poll Address used to retrieve the Unique ID (usually 0)");
            CmbPollAddress.SelectedIndexChanged += CmbPollAddress_SelectedIndexChanged;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(ChkViewStatusBinary);
            groupBox5.Controls.Add(ChkViewDecodedData);
            groupBox5.Controls.Add(ChkViewFrameNumbers);
            groupBox5.Controls.Add(ChkViewTiming);
            groupBox5.Controls.Add(ChkViewAddress);
            groupBox5.Controls.Add(ChkViewPreambles);
            groupBox5.Location = new Point(519, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(219, 125);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "Hart Communication View";
            // 
            // ChkViewStatusBinary
            // 
            ChkViewStatusBinary.Location = new Point(99, 91);
            ChkViewStatusBinary.Name = "ChkViewStatusBinary";
            ChkViewStatusBinary.Size = new Size(114, 25);
            ChkViewStatusBinary.TabIndex = 5;
            ChkViewStatusBinary.Text = "Status Details";
            ToolTipInfo.SetToolTip(ChkViewStatusBinary, "Show status details in binary format");
            ChkViewStatusBinary.UseVisualStyleBackColor = true;
            ChkViewStatusBinary.CheckedChanged += ChkViewStatusBinary_CheckedChanged;
            // 
            // ChkViewDecodedData
            // 
            ChkViewDecodedData.Location = new Point(99, 59);
            ChkViewDecodedData.Name = "ChkViewDecodedData";
            ChkViewDecodedData.Size = new Size(114, 24);
            ChkViewDecodedData.TabIndex = 4;
            ChkViewDecodedData.Text = "Decoded Data";
            ToolTipInfo.SetToolTip(ChkViewDecodedData, "Decode the data in known sections (frames)");
            ChkViewDecodedData.UseVisualStyleBackColor = true;
            ChkViewDecodedData.CheckedChanged += ChkViewDecodedData_CheckedChanged;
            // 
            // ChkViewFrameNumbers
            // 
            ChkViewFrameNumbers.Location = new Point(99, 25);
            ChkViewFrameNumbers.Name = "ChkViewFrameNumbers";
            ChkViewFrameNumbers.Size = new Size(114, 24);
            ChkViewFrameNumbers.TabIndex = 3;
            ChkViewFrameNumbers.Text = "Frame Numbers";
            ToolTipInfo.SetToolTip(ChkViewFrameNumbers, "Display numbers with the frames");
            ChkViewFrameNumbers.UseVisualStyleBackColor = true;
            ChkViewFrameNumbers.CheckedChanged += ChkViewFrameNumbers_CheckedChanged;
            // 
            // ChkViewTiming
            // 
            ChkViewTiming.Location = new Point(12, 91);
            ChkViewTiming.Name = "ChkViewTiming";
            ChkViewTiming.Size = new Size(81, 25);
            ChkViewTiming.TabIndex = 2;
            ChkViewTiming.Text = "Timing";
            ToolTipInfo.SetToolTip(ChkViewTiming, "Show start time and duration of frames");
            ChkViewTiming.UseVisualStyleBackColor = true;
            ChkViewTiming.CheckedChanged += ChkViewTiming_CheckedChanged;
            // 
            // ChkViewAddress
            // 
            ChkViewAddress.Location = new Point(12, 58);
            ChkViewAddress.Name = "ChkViewAddress";
            ChkViewAddress.Size = new Size(81, 24);
            ChkViewAddress.TabIndex = 1;
            ChkViewAddress.Text = "Address";
            ToolTipInfo.SetToolTip(ChkViewAddress, "Show the addressing information in the frame");
            ChkViewAddress.UseVisualStyleBackColor = true;
            ChkViewAddress.CheckedChanged += ChkViewAddress_CheckedChanged;
            // 
            // ChkViewPreambles
            // 
            ChkViewPreambles.Location = new Point(12, 25);
            ChkViewPreambles.Name = "ChkViewPreambles";
            ChkViewPreambles.Size = new Size(81, 24);
            ChkViewPreambles.TabIndex = 0;
            ChkViewPreambles.Text = "Preambles";
            ToolTipInfo.SetToolTip(ChkViewPreambles, "Show preambles in frame display");
            ChkViewPreambles.UseVisualStyleBackColor = true;
            ChkViewPreambles.CheckedChanged += ChkViewPreambles_CheckedChanged;
            // 
            // tabHartIp
            // 
            tabHartIp.Controls.Add(ChkHartIpSendKeepAlive);
            tabHartIp.Controls.Add(ButSetDefault);
            tabHartIp.Controls.Add(ChkHartIpEnabled);
            tabHartIp.Controls.Add(TxtLastError);
            tabHartIp.Controls.Add(label72);
            tabHartIp.Controls.Add(label22);
            tabHartIp.Controls.Add(TxtConnectionStatus);
            tabHartIp.Controls.Add(ChkHartIpUseAddress);
            tabHartIp.Controls.Add(label17);
            tabHartIp.Controls.Add(rtxIPFrames);
            tabHartIp.Controls.Add(TxtHartIpPort);
            tabHartIp.Controls.Add(label14);
            tabHartIp.Controls.Add(TxtHartIpAddress);
            tabHartIp.Controls.Add(label12);
            tabHartIp.Controls.Add(TxtHartIpHostName);
            tabHartIp.Controls.Add(label11);
            tabHartIp.Location = new Point(4, 27);
            tabHartIp.Name = "tabHartIp";
            tabHartIp.Size = new Size(776, 134);
            tabHartIp.TabIndex = 8;
            tabHartIp.Text = "Hart IP";
            tabHartIp.UseVisualStyleBackColor = true;
            // 
            // ChkHartIpSendKeepAlive
            // 
            ChkHartIpSendKeepAlive.Location = new Point(553, 41);
            ChkHartIpSendKeepAlive.Name = "ChkHartIpSendKeepAlive";
            ChkHartIpSendKeepAlive.Size = new Size(170, 23);
            ChkHartIpSendKeepAlive.TabIndex = 92;
            ChkHartIpSendKeepAlive.Text = "Send Keep Alive Messages";
            ChkHartIpSendKeepAlive.UseVisualStyleBackColor = true;
            ChkHartIpSendKeepAlive.CheckedChanged += ChkHartIpSendKeepAlive_CheckedChanged;
            // 
            // ButSetDefault
            // 
            ButSetDefault.FlatStyle = FlatStyle.Popup;
            ButSetDefault.Location = new Point(9, 94);
            ButSetDefault.Name = "ButSetDefault";
            ButSetDefault.Size = new Size(75, 26);
            ButSetDefault.TabIndex = 91;
            ButSetDefault.Text = "Set Default";
            ButSetDefault.TextAlign = ContentAlignment.MiddleLeft;
            ButSetDefault.UseVisualStyleBackColor = true;
            ButSetDefault.Click += ButSetDefault_Click;
            // 
            // ChkHartIpEnabled
            // 
            ChkHartIpEnabled.Checked = true;
            ChkHartIpEnabled.CheckState = CheckState.Checked;
            ChkHartIpEnabled.Location = new Point(9, 41);
            ChkHartIpEnabled.Name = "ChkHartIpEnabled";
            ChkHartIpEnabled.Size = new Size(121, 23);
            ChkHartIpEnabled.TabIndex = 90;
            ChkHartIpEnabled.Text = "Hart Ip Enabled";
            ChkHartIpEnabled.UseVisualStyleBackColor = true;
            ChkHartIpEnabled.CheckedChanged += ChkHartIpEnabled_CheckedChanged;
            // 
            // TxtLastError
            // 
            TxtLastError.AcceptsReturn = true;
            TxtLastError.BackColor = SystemColors.Info;
            TxtLastError.BorderStyle = BorderStyle.FixedSingle;
            TxtLastError.Location = new Point(635, 11);
            TxtLastError.Name = "TxtLastError";
            TxtLastError.Size = new Size(133, 23);
            TxtLastError.TabIndex = 89;
            TxtLastError.Text = "No Error";
            // 
            // label72
            // 
            label72.Location = new Point(564, 9);
            label72.Name = "label72";
            label72.Size = new Size(65, 22);
            label72.TabIndex = 88;
            label72.Text = "Last Error:";
            label72.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            label22.Location = new Point(261, 40);
            label22.Name = "label22";
            label22.Size = new Size(153, 22);
            label22.TabIndex = 77;
            label22.Text = "Last Hart IP PDU sent:";
            label22.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtConnectionStatus
            // 
            TxtConnectionStatus.AcceptsReturn = true;
            TxtConnectionStatus.BackColor = SystemColors.Info;
            TxtConnectionStatus.BorderStyle = BorderStyle.FixedSingle;
            TxtConnectionStatus.Location = new Point(349, 11);
            TxtConnectionStatus.Name = "TxtConnectionStatus";
            TxtConnectionStatus.Size = new Size(193, 23);
            TxtConnectionStatus.TabIndex = 74;
            TxtConnectionStatus.Text = "Not Connected";
            // 
            // ChkHartIpUseAddress
            // 
            ChkHartIpUseAddress.AutoSize = true;
            ChkHartIpUseAddress.Checked = true;
            ChkHartIpUseAddress.CheckState = CheckState.Checked;
            ChkHartIpUseAddress.Enabled = false;
            ChkHartIpUseAddress.Location = new Point(136, 43);
            ChkHartIpUseAddress.Name = "ChkHartIpUseAddress";
            ChkHartIpUseAddress.Size = new Size(90, 19);
            ChkHartIpUseAddress.TabIndex = 73;
            ChkHartIpUseAddress.Text = "Use Address";
            ChkHartIpUseAddress.UseVisualStyleBackColor = true;
            ChkHartIpUseAddress.CheckedChanged += ChkHartIpUseAddress_CheckedChanged;
            // 
            // label17
            // 
            label17.Location = new Point(261, 9);
            label17.Name = "label17";
            label17.Size = new Size(82, 22);
            label17.TabIndex = 72;
            label17.Text = "Hart IP Status:";
            label17.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // rtxIPFrames
            // 
            rtxIPFrames.BackColor = SystemColors.Info;
            rtxIPFrames.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtxIPFrames.Location = new Point(261, 66);
            rtxIPFrames.Name = "rtxIPFrames";
            rtxIPFrames.ReadOnly = true;
            rtxIPFrames.Size = new Size(507, 66);
            rtxIPFrames.TabIndex = 71;
            rtxIPFrames.Text = "-/-";
            // 
            // TxtHartIpPort
            // 
            TxtHartIpPort.AcceptsReturn = true;
            TxtHartIpPort.BackColor = SystemColors.Window;
            TxtHartIpPort.BorderStyle = BorderStyle.FixedSingle;
            TxtHartIpPort.Enabled = false;
            TxtHartIpPort.Location = new Point(136, 98);
            TxtHartIpPort.Name = "TxtHartIpPort";
            TxtHartIpPort.Size = new Size(106, 23);
            TxtHartIpPort.TabIndex = 70;
            TxtHartIpPort.Text = "5094";
            TxtHartIpPort.TextChanged += TxtHartIpPort_TextChanged;
            // 
            // label14
            // 
            label14.Location = new Point(90, 96);
            label14.Name = "label14";
            label14.Size = new Size(39, 22);
            label14.TabIndex = 69;
            label14.Text = "Port:";
            label14.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtHartIpAddress
            // 
            TxtHartIpAddress.AcceptsReturn = true;
            TxtHartIpAddress.BackColor = SystemColors.Window;
            TxtHartIpAddress.BorderStyle = BorderStyle.FixedSingle;
            TxtHartIpAddress.Enabled = false;
            TxtHartIpAddress.Location = new Point(136, 68);
            TxtHartIpAddress.Name = "TxtHartIpAddress";
            TxtHartIpAddress.Size = new Size(106, 23);
            TxtHartIpAddress.TabIndex = 67;
            TxtHartIpAddress.Text = "127.0.0.2";
            TxtHartIpAddress.TextChanged += TxtHartIpAddress_TextChanged;
            // 
            // label12
            // 
            label12.Location = new Point(25, 66);
            label12.Name = "label12";
            label12.Size = new Size(104, 22);
            label12.TabIndex = 66;
            label12.Text = "Slave Address:";
            label12.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtHartIpHostName
            // 
            TxtHartIpHostName.AcceptsReturn = true;
            TxtHartIpHostName.BackColor = SystemColors.Window;
            TxtHartIpHostName.BorderStyle = BorderStyle.FixedSingle;
            TxtHartIpHostName.Enabled = false;
            TxtHartIpHostName.Location = new Point(136, 11);
            TxtHartIpHostName.Name = "TxtHartIpHostName";
            TxtHartIpHostName.Size = new Size(106, 23);
            TxtHartIpHostName.TabIndex = 65;
            TxtHartIpHostName.Text = "localhost";
            TxtHartIpHostName.TextChanged += TxtHartIpHostName_TextChanged;
            // 
            // label11
            // 
            label11.Location = new Point(25, 9);
            label11.Name = "label11";
            label11.Size = new Size(105, 22);
            label11.TabIndex = 64;
            label11.Text = "Host name:";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tabCommands
            // 
            tabCommands.Controls.Add(label2);
            tabCommands.Controls.Add(groupBox2);
            tabCommands.Controls.Add(groupBox1);
            tabCommands.Controls.Add(groupBox9);
            tabCommands.Location = new Point(4, 27);
            tabCommands.Name = "tabCommands";
            tabCommands.Size = new Size(776, 134);
            tabCommands.TabIndex = 2;
            tabCommands.Text = "Commands";
            tabCommands.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label2.ForeColor = SystemColors.ControlDarkDark;
            label2.Location = new Point(491, 94);
            label2.Name = "label2";
            label2.Size = new Size(219, 37);
            label2.TabIndex = 49;
            label2.Text = "Hart Master 8.0";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(ButSendUserCmd);
            groupBox2.Controls.Add(ButEditCmd31);
            groupBox2.Controls.Add(ButSendCmd31);
            groupBox2.Controls.Add(ButEditUserCmd);
            groupBox2.Location = new Point(485, 5);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(192, 91);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "User and Extended";
            // 
            // ButSendUserCmd
            // 
            ButSendUserCmd.BackColor = Color.PowderBlue;
            ButSendUserCmd.FlatStyle = FlatStyle.Popup;
            ButSendUserCmd.Location = new Point(6, 25);
            ButSendUserCmd.Name = "ButSendUserCmd";
            ButSendUserCmd.Size = new Size(77, 26);
            ButSendUserCmd.TabIndex = 5;
            ButSendUserCmd.Text = "User Cmd";
            ButSendUserCmd.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendUserCmd, "Send  preconfigured 8 Bit Command");
            ButSendUserCmd.UseVisualStyleBackColor = false;
            ButSendUserCmd.Click += ButSendUserCmd_Click;
            // 
            // ButEditCmd31
            // 
            ButEditCmd31.FlatStyle = FlatStyle.Popup;
            ButEditCmd31.Location = new Point(89, 57);
            ButEditCmd31.Name = "ButEditCmd31";
            ButEditCmd31.Size = new Size(90, 26);
            ButEditCmd31.TabIndex = 36;
            ButEditCmd31.Text = "Edit Ext Cmd";
            ButEditCmd31.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButEditCmd31, "Configure any command in the range of 0..65535.");
            ButEditCmd31.UseVisualStyleBackColor = true;
            ButEditCmd31.Click += ButEditCmd31_Click;
            // 
            // ButSendCmd31
            // 
            ButSendCmd31.BackColor = Color.PowderBlue;
            ButSendCmd31.Enabled = false;
            ButSendCmd31.FlatStyle = FlatStyle.Popup;
            ButSendCmd31.Location = new Point(6, 57);
            ButSendCmd31.Name = "ButSendCmd31";
            ButSendCmd31.Size = new Size(77, 26);
            ButSendCmd31.TabIndex = 34;
            ButSendCmd31.Text = "Ext Cmd";
            ButSendCmd31.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd31, "Send  preconfigured 16 Bit Command");
            ButSendCmd31.UseVisualStyleBackColor = false;
            ButSendCmd31.Click += ButSendCmd31_Click;
            // 
            // ButEditUserCmd
            // 
            ButEditUserCmd.FlatStyle = FlatStyle.Popup;
            ButEditUserCmd.Location = new Point(89, 25);
            ButEditUserCmd.Name = "ButEditUserCmd";
            ButEditUserCmd.Size = new Size(90, 26);
            ButEditUserCmd.TabIndex = 31;
            ButEditUserCmd.Text = "Edit User Cmd";
            ButEditUserCmd.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButEditUserCmd, "Configure any command in the range of 0..255.");
            ButEditUserCmd.UseVisualStyleBackColor = true;
            ButEditUserCmd.Click += ButEditUserCmd_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(ButSendCmd35);
            groupBox1.Controls.Add(ButEditCmd35);
            groupBox1.Location = new Point(314, 5);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(165, 62);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Common Practice";
            // 
            // ButSendCmd35
            // 
            ButSendCmd35.BackColor = Color.PowderBlue;
            ButSendCmd35.FlatStyle = FlatStyle.Popup;
            ButSendCmd35.Location = new Point(6, 25);
            ButSendCmd35.Name = "ButSendCmd35";
            ButSendCmd35.Size = new Size(62, 26);
            ButSendCmd35.TabIndex = 1;
            ButSendCmd35.Text = "Cmd 35";
            ButSendCmd35.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd35, "Write Upper and Lower Range");
            ButSendCmd35.UseVisualStyleBackColor = false;
            ButSendCmd35.Click += ButSendCmd35_Click;
            // 
            // ButEditCmd35
            // 
            ButEditCmd35.FlatStyle = FlatStyle.Popup;
            ButEditCmd35.Location = new Point(74, 25);
            ButEditCmd35.Name = "ButEditCmd35";
            ButEditCmd35.Size = new Size(83, 26);
            ButEditCmd35.TabIndex = 56;
            ButEditCmd35.Text = "Edit Cmd 35";
            ButEditCmd35.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButEditCmd35, "Edit Upper and Lower Range Values");
            ButEditCmd35.UseVisualStyleBackColor = true;
            ButEditCmd35.Click += ButEditCmd35_Click;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(ButSendCmd38);
            groupBox9.Controls.Add(ButSendCmd48);
            groupBox9.Controls.Add(ButEditCmd18);
            groupBox9.Controls.Add(ButSendCmd18);
            groupBox9.Controls.Add(ButSendCmd15);
            groupBox9.Controls.Add(ButSendCmd14);
            groupBox9.Controls.Add(ButSendCmd13);
            groupBox9.Controls.Add(ButSendCmd12);
            groupBox9.Controls.Add(ButSendCmd2);
            groupBox9.Controls.Add(ButSendCmd3);
            groupBox9.Controls.Add(butSendCmd1);
            groupBox9.Controls.Add(ButSendCmd0short);
            groupBox9.Location = new Point(8, 5);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(300, 126);
            groupBox9.TabIndex = 5;
            groupBox9.TabStop = false;
            groupBox9.Text = "Universal";
            // 
            // ButSendCmd38
            // 
            ButSendCmd38.BackColor = Color.PowderBlue;
            ButSendCmd38.FlatStyle = FlatStyle.Popup;
            ButSendCmd38.Location = new Point(211, 25);
            ButSendCmd38.Name = "ButSendCmd38";
            ButSendCmd38.Size = new Size(62, 26);
            ButSendCmd38.TabIndex = 47;
            ButSendCmd38.Text = "Cmd 38";
            ButSendCmd38.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd38, "Reset Configuration Changed Flag");
            ButSendCmd38.UseVisualStyleBackColor = false;
            ButSendCmd38.Click += ButSendCmd38_Click;
            // 
            // ButSendCmd48
            // 
            ButSendCmd48.BackColor = Color.PowderBlue;
            ButSendCmd48.FlatStyle = FlatStyle.Popup;
            ButSendCmd48.Location = new Point(211, 57);
            ButSendCmd48.Name = "ButSendCmd48";
            ButSendCmd48.Size = new Size(62, 26);
            ButSendCmd48.TabIndex = 48;
            ButSendCmd48.Text = "Cmd 48";
            ButSendCmd48.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd48, "Read Additional Device Status");
            ButSendCmd48.UseVisualStyleBackColor = false;
            ButSendCmd48.Click += ButSendCmd48_Click;
            // 
            // ButEditCmd18
            // 
            ButEditCmd18.FlatStyle = FlatStyle.Popup;
            ButEditCmd18.Location = new Point(211, 89);
            ButEditCmd18.Name = "ButEditCmd18";
            ButEditCmd18.Size = new Size(83, 26);
            ButEditCmd18.TabIndex = 56;
            ButEditCmd18.Text = "Edit Cmd 18";
            ButEditCmd18.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButEditCmd18, "Edit Tag, Descriptor and Date");
            ButEditCmd18.UseVisualStyleBackColor = true;
            ButEditCmd18.Click += ButEditCmd18_Click;
            // 
            // ButSendCmd18
            // 
            ButSendCmd18.BackColor = Color.PowderBlue;
            ButSendCmd18.FlatStyle = FlatStyle.Popup;
            ButSendCmd18.Location = new Point(143, 89);
            ButSendCmd18.Name = "ButSendCmd18";
            ButSendCmd18.Size = new Size(62, 26);
            ButSendCmd18.TabIndex = 55;
            ButSendCmd18.Text = "Cmd 18";
            ButSendCmd18.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd18, "Write Tag, Descriptor and Date");
            ButSendCmd18.UseVisualStyleBackColor = false;
            ButSendCmd18.Click += ButSendCmd18_Click;
            // 
            // ButSendCmd15
            // 
            ButSendCmd15.BackColor = Color.PowderBlue;
            ButSendCmd15.FlatStyle = FlatStyle.Popup;
            ButSendCmd15.Location = new Point(143, 57);
            ButSendCmd15.Name = "ButSendCmd15";
            ButSendCmd15.Size = new Size(62, 26);
            ButSendCmd15.TabIndex = 54;
            ButSendCmd15.Text = "Cmd 15";
            ButSendCmd15.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd15, "Read Upper and Lower Range Values");
            ButSendCmd15.UseVisualStyleBackColor = false;
            ButSendCmd15.Click += ButSendCmd15_Click;
            // 
            // ButSendCmd14
            // 
            ButSendCmd14.BackColor = Color.PowderBlue;
            ButSendCmd14.FlatStyle = FlatStyle.Popup;
            ButSendCmd14.Location = new Point(143, 25);
            ButSendCmd14.Name = "ButSendCmd14";
            ButSendCmd14.Size = new Size(62, 26);
            ButSendCmd14.TabIndex = 53;
            ButSendCmd14.Text = "Cmd 14";
            ButSendCmd14.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd14, "Read Primary Variable Transducer Information");
            ButSendCmd14.UseVisualStyleBackColor = false;
            ButSendCmd14.Click += ButSendCmd14_Click;
            // 
            // ButSendCmd13
            // 
            ButSendCmd13.BackColor = Color.PowderBlue;
            ButSendCmd13.FlatStyle = FlatStyle.Popup;
            ButSendCmd13.Location = new Point(75, 89);
            ButSendCmd13.Name = "ButSendCmd13";
            ButSendCmd13.Size = new Size(62, 26);
            ButSendCmd13.TabIndex = 52;
            ButSendCmd13.Text = "Cmd 13";
            ButSendCmd13.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd13, "Read Tag, Descriptor, Date");
            ButSendCmd13.UseVisualStyleBackColor = false;
            ButSendCmd13.Click += ButSendCmd13_Click;
            // 
            // ButSendCmd12
            // 
            ButSendCmd12.BackColor = Color.PowderBlue;
            ButSendCmd12.FlatStyle = FlatStyle.Popup;
            ButSendCmd12.Location = new Point(75, 57);
            ButSendCmd12.Name = "ButSendCmd12";
            ButSendCmd12.Size = new Size(62, 26);
            ButSendCmd12.TabIndex = 51;
            ButSendCmd12.Text = "Cmd 12";
            ButSendCmd12.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd12, "Read Message");
            ButSendCmd12.UseVisualStyleBackColor = false;
            ButSendCmd12.Click += ButSendCmd12_Click;
            // 
            // ButSendCmd2
            // 
            ButSendCmd2.BackColor = Color.PowderBlue;
            ButSendCmd2.FlatStyle = FlatStyle.Popup;
            ButSendCmd2.Location = new Point(7, 89);
            ButSendCmd2.Name = "ButSendCmd2";
            ButSendCmd2.Size = new Size(62, 26);
            ButSendCmd2.TabIndex = 50;
            ButSendCmd2.Text = "Cmd 2";
            ButSendCmd2.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd2, "Read Current and Percent of Range");
            ButSendCmd2.UseVisualStyleBackColor = false;
            ButSendCmd2.Click += ButSendCmd2_Click;
            // 
            // ButSendCmd3
            // 
            ButSendCmd3.BackColor = Color.PowderBlue;
            ButSendCmd3.FlatStyle = FlatStyle.Popup;
            ButSendCmd3.Location = new Point(75, 25);
            ButSendCmd3.Name = "ButSendCmd3";
            ButSendCmd3.Size = new Size(62, 26);
            ButSendCmd3.TabIndex = 3;
            ButSendCmd3.Text = "Cmd 3";
            ButSendCmd3.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd3, "Read Dynamic Variables and Loop Current");
            ButSendCmd3.UseVisualStyleBackColor = false;
            ButSendCmd3.Click += ButSendCmd3_Click;
            // 
            // butSendCmd1
            // 
            butSendCmd1.BackColor = Color.PowderBlue;
            butSendCmd1.FlatStyle = FlatStyle.Popup;
            butSendCmd1.Location = new Point(7, 57);
            butSendCmd1.Name = "butSendCmd1";
            butSendCmd1.Size = new Size(62, 26);
            butSendCmd1.TabIndex = 2;
            butSendCmd1.Text = "Cmd 1";
            butSendCmd1.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(butSendCmd1, "Read Primary Variable");
            butSendCmd1.UseVisualStyleBackColor = false;
            butSendCmd1.Click += ButSendCmd1_Click;
            // 
            // ButSendCmd0short
            // 
            ButSendCmd0short.BackColor = Color.PowderBlue;
            ButSendCmd0short.FlatStyle = FlatStyle.Popup;
            ButSendCmd0short.Location = new Point(7, 25);
            ButSendCmd0short.Name = "ButSendCmd0short";
            ButSendCmd0short.Size = new Size(62, 26);
            ButSendCmd0short.TabIndex = 1;
            ButSendCmd0short.Text = "Cmd 0";
            ButSendCmd0short.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButSendCmd0short, "Read Unique Identifier");
            ButSendCmd0short.UseVisualStyleBackColor = false;
            ButSendCmd0short.Click += ButSendCmd0short_Click;
            // 
            // tabOptions
            // 
            tabOptions.Controls.Add(rtxtTestDisplay);
            tabOptions.Controls.Add(groupBox22);
            tabOptions.Controls.Add(groupBox19);
            tabOptions.Controls.Add(groupBox16);
            tabOptions.Location = new Point(4, 27);
            tabOptions.Name = "tabOptions";
            tabOptions.Size = new Size(776, 134);
            tabOptions.TabIndex = 6;
            tabOptions.Text = "Options/Tests";
            tabOptions.UseVisualStyleBackColor = true;
            // 
            // rtxtTestDisplay
            // 
            rtxtTestDisplay.BackColor = SystemColors.Control;
            rtxtTestDisplay.BorderStyle = BorderStyle.FixedSingle;
            rtxtTestDisplay.ContextMenuStrip = CtxMnuDecode;
            rtxtTestDisplay.DetectUrls = false;
            rtxtTestDisplay.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtxtTestDisplay.Location = new Point(540, 13);
            rtxtTestDisplay.Name = "rtxtTestDisplay";
            rtxtTestDisplay.ReadOnly = true;
            rtxtTestDisplay.Size = new Size(199, 108);
            rtxtTestDisplay.TabIndex = 49;
            rtxtTestDisplay.Text = "";
            rtxtTestDisplay.WordWrap = false;
            // 
            // groupBox22
            // 
            groupBox22.Controls.Add(ButFreeTest);
            groupBox22.Controls.Add(ButUserTestCommands);
            groupBox22.Controls.Add(ButUserTestCoding);
            groupBox22.Location = new Point(402, 4);
            groupBox22.Name = "groupBox22";
            groupBox22.Size = new Size(139, 126);
            groupBox22.TabIndex = 48;
            groupBox22.TabStop = false;
            groupBox22.Text = "White Box Tests";
            // 
            // ButFreeTest
            // 
            ButFreeTest.FlatStyle = FlatStyle.Popup;
            ButFreeTest.Location = new Point(6, 90);
            ButFreeTest.Name = "ButFreeTest";
            ButFreeTest.Size = new Size(126, 26);
            ButFreeTest.TabIndex = 37;
            ButFreeTest.Text = "Free User Test";
            ToolTipInfo.SetToolTip(ButFreeTest, "Look into the Event Procedure for an Example.");
            ButFreeTest.UseVisualStyleBackColor = true;
            ButFreeTest.Click += ButFreeTest_Click;
            // 
            // ButUserTestCommands
            // 
            ButUserTestCommands.FlatStyle = FlatStyle.Popup;
            ButUserTestCommands.Location = new Point(6, 58);
            ButUserTestCommands.Name = "ButUserTestCommands";
            ButUserTestCommands.Size = new Size(126, 26);
            ButUserTestCommands.TabIndex = 36;
            ButUserTestCommands.Text = "User Test Commands";
            ButUserTestCommands.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButUserTestCommands, "Look into the Event Procedure for an Example.");
            ButUserTestCommands.UseVisualStyleBackColor = true;
            ButUserTestCommands.Click += ButUserTestCommands_Click;
            // 
            // ButUserTestCoding
            // 
            ButUserTestCoding.FlatStyle = FlatStyle.Popup;
            ButUserTestCoding.Location = new Point(6, 26);
            ButUserTestCoding.Name = "ButUserTestCoding";
            ButUserTestCoding.Size = new Size(126, 26);
            ButUserTestCoding.TabIndex = 35;
            ButUserTestCoding.Text = "User Test Coding";
            ButUserTestCoding.TextAlign = ContentAlignment.MiddleLeft;
            ToolTipInfo.SetToolTip(ButUserTestCoding, "Look into the Event Procedure for an Example.");
            ButUserTestCoding.UseVisualStyleBackColor = true;
            ButUserTestCoding.Click += ButUserTestCoding_Click;
            // 
            // groupBox19
            // 
            groupBox19.Controls.Add(ChkRetryIfBusy);
            groupBox19.Controls.Add(CmbNumRetries);
            groupBox19.Controls.Add(label19);
            groupBox19.Location = new Point(235, 4);
            groupBox19.Name = "groupBox19";
            groupBox19.Size = new Size(161, 126);
            groupBox19.TabIndex = 45;
            groupBox19.TabStop = false;
            groupBox19.Text = "Service Properties";
            // 
            // ChkRetryIfBusy
            // 
            ChkRetryIfBusy.CheckAlign = ContentAlignment.MiddleRight;
            ChkRetryIfBusy.Location = new Point(50, 56);
            ChkRetryIfBusy.Name = "ChkRetryIfBusy";
            ChkRetryIfBusy.Size = new Size(104, 26);
            ChkRetryIfBusy.TabIndex = 50;
            ChkRetryIfBusy.Text = "Retry if Busy";
            ChkRetryIfBusy.TextAlign = ContentAlignment.MiddleRight;
            ToolTipInfo.SetToolTip(ChkRetryIfBusy, "Retry as long as the Devices is Reporting Busy");
            ChkRetryIfBusy.UseVisualStyleBackColor = true;
            ChkRetryIfBusy.CheckedChanged += ChkRetryIfBusy_CheckedChanged;
            // 
            // CmbNumRetries
            // 
            CmbNumRetries.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbNumRetries.Items.AddRange(new object[] { " 0", " 1", " 2", " 3", " 4", " 5", " 6", " 7", " 8", " 9", "10" });
            CmbNumRetries.Location = new Point(91, 27);
            CmbNumRetries.Name = "CmbNumRetries";
            CmbNumRetries.Size = new Size(63, 23);
            CmbNumRetries.TabIndex = 49;
            CmbNumRetries.SelectedIndexChanged += CmbNumRetries_SelectedIndexChanged;
            // 
            // label19
            // 
            label19.Location = new Point(6, 23);
            label19.Name = "label19";
            label19.Size = new Size(79, 24);
            label19.TabIndex = 48;
            label19.Text = "Num Retries:";
            label19.TextAlign = ContentAlignment.MiddleRight;
            // 
            // groupBox16
            // 
            groupBox16.Controls.Add(ButEditColors);
            groupBox16.Controls.Add(ChkTopMost);
            groupBox16.Controls.Add(RadColorSetUser);
            groupBox16.Controls.Add(RadColorSet2);
            groupBox16.Controls.Add(RadColorSet1);
            groupBox16.Location = new Point(4, 4);
            groupBox16.Name = "groupBox16";
            groupBox16.Size = new Size(224, 126);
            groupBox16.TabIndex = 44;
            groupBox16.TabStop = false;
            groupBox16.Text = "Appearance";
            // 
            // ButEditColors
            // 
            ButEditColors.FlatStyle = FlatStyle.Popup;
            ButEditColors.Location = new Point(131, 91);
            ButEditColors.Name = "ButEditColors";
            ButEditColors.Size = new Size(86, 26);
            ButEditColors.TabIndex = 34;
            ButEditColors.Text = "Edit Colors";
            ButEditColors.TextAlign = ContentAlignment.MiddleLeft;
            ButEditColors.UseVisualStyleBackColor = true;
            ButEditColors.Click += ButEditColors_Click;
            // 
            // ChkTopMost
            // 
            ChkTopMost.Location = new Point(131, 26);
            ChkTopMost.Name = "ChkTopMost";
            ChkTopMost.Size = new Size(80, 26);
            ChkTopMost.TabIndex = 43;
            ChkTopMost.Text = "Topmost";
            ChkTopMost.UseVisualStyleBackColor = true;
            ChkTopMost.CheckedChanged += ChkTopMost_CheckedChanged;
            // 
            // RadColorSetUser
            // 
            RadColorSetUser.Location = new Point(7, 91);
            RadColorSetUser.Name = "RadColorSetUser";
            RadColorSetUser.Size = new Size(106, 26);
            RadColorSetUser.TabIndex = 2;
            RadColorSetUser.TabStop = true;
            RadColorSetUser.Text = "User Colors";
            RadColorSetUser.UseVisualStyleBackColor = true;
            RadColorSetUser.CheckedChanged += RadColorSetUser_CheckedChanged;
            // 
            // RadColorSet2
            // 
            RadColorSet2.Location = new Point(7, 58);
            RadColorSet2.Name = "RadColorSet2";
            RadColorSet2.Size = new Size(106, 26);
            RadColorSet2.TabIndex = 1;
            RadColorSet2.TabStop = true;
            RadColorSet2.Text = "Dark Colors";
            RadColorSet2.UseVisualStyleBackColor = true;
            RadColorSet2.CheckedChanged += RadColorSet2_CheckedChanged;
            // 
            // RadColorSet1
            // 
            RadColorSet1.Location = new Point(7, 25);
            RadColorSet1.Name = "RadColorSet1";
            RadColorSet1.Size = new Size(106, 25);
            RadColorSet1.TabIndex = 0;
            RadColorSet1.TabStop = true;
            RadColorSet1.Text = "Light Colors";
            RadColorSet1.UseVisualStyleBackColor = true;
            RadColorSet1.CheckedChanged += RadColorSet1_CheckedChanged;
            // 
            // tabDevData
            // 
            tabDevData.Controls.Add(ButReadAll);
            tabDevData.Controls.Add(label25);
            tabDevData.Controls.Add(TxtDevDatCurrent);
            tabDevData.Controls.Add(label24);
            tabDevData.Controls.Add(LblDevDatSpanUnit);
            tabDevData.Controls.Add(TxtDevDatSpan);
            tabDevData.Controls.Add(label21);
            tabDevData.Controls.Add(LblDevDatUnitPV);
            tabDevData.Controls.Add(TxtDevDatPV);
            tabDevData.Controls.Add(label18);
            tabDevData.Controls.Add(LblDevDatUnitSV);
            tabDevData.Controls.Add(TxtDevDatSV);
            tabDevData.Controls.Add(label16);
            tabDevData.Controls.Add(LblDevDatUnitTV);
            tabDevData.Controls.Add(TxtDevDatTV);
            tabDevData.Controls.Add(label15);
            tabDevData.Controls.Add(LblDevDatUnitQV);
            tabDevData.Controls.Add(TxtDevDatQV);
            tabDevData.Controls.Add(LblDevDatRangeUnitLo);
            tabDevData.Controls.Add(TxtDevDatLowerRange);
            tabDevData.Controls.Add(label13);
            tabDevData.Controls.Add(LblDevDatRangeUnitUp);
            tabDevData.Controls.Add(TxtDevDatUpperRange);
            tabDevData.Controls.Add(label10);
            tabDevData.Controls.Add(TxtDevDatMessage);
            tabDevData.Controls.Add(label9);
            tabDevData.Controls.Add(TxtDevDatLongTag);
            tabDevData.Controls.Add(label8);
            tabDevData.Controls.Add(TxtDevDatShortTag);
            tabDevData.Controls.Add(label7);
            tabDevData.Controls.Add(TxtDevDatUniqueID);
            tabDevData.Controls.Add(label6);
            tabDevData.Location = new Point(4, 27);
            tabDevData.Name = "tabDevData";
            tabDevData.Size = new Size(776, 134);
            tabDevData.TabIndex = 7;
            tabDevData.Text = "Slave Data";
            tabDevData.UseVisualStyleBackColor = true;
            // 
            // ButReadAll
            // 
            ButReadAll.BackColor = Color.PowderBlue;
            ButReadAll.FlatStyle = FlatStyle.Popup;
            ButReadAll.Location = new Point(653, 95);
            ButReadAll.Name = "ButReadAll";
            ButReadAll.Size = new Size(74, 26);
            ButReadAll.TabIndex = 93;
            ButReadAll.Text = "Read All";
            ToolTipInfo.SetToolTip(ButReadAll, "Read all Data from a connected Slave Device.");
            ButReadAll.UseVisualStyleBackColor = false;
            ButReadAll.Click += ButDetDatReadAll_Click;
            // 
            // label25
            // 
            label25.Location = new Point(715, 34);
            label25.Name = "label25";
            label25.Size = new Size(31, 25);
            label25.TabIndex = 92;
            label25.Text = "mA";
            label25.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtDevDatCurrent
            // 
            TxtDevDatCurrent.AcceptsReturn = true;
            TxtDevDatCurrent.BackColor = SystemColors.Info;
            TxtDevDatCurrent.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatCurrent.Location = new Point(653, 37);
            TxtDevDatCurrent.Name = "TxtDevDatCurrent";
            TxtDevDatCurrent.ReadOnly = true;
            TxtDevDatCurrent.Size = new Size(56, 23);
            TxtDevDatCurrent.TabIndex = 91;
            TxtDevDatCurrent.Text = "-/-";
            TxtDevDatCurrent.TextAlign = HorizontalAlignment.Right;
            // 
            // label24
            // 
            label24.Location = new Point(653, 9);
            label24.Name = "label24";
            label24.Size = new Size(56, 25);
            label24.TabIndex = 90;
            label24.Text = "Current:";
            label24.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // LblDevDatSpanUnit
            // 
            LblDevDatSpanUnit.Location = new Point(423, 68);
            LblDevDatSpanUnit.Name = "LblDevDatSpanUnit";
            LblDevDatSpanUnit.Size = new Size(58, 25);
            LblDevDatSpanUnit.TabIndex = 89;
            LblDevDatSpanUnit.Text = "unit";
            LblDevDatSpanUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtDevDatSpan
            // 
            TxtDevDatSpan.AcceptsReturn = true;
            TxtDevDatSpan.BackColor = SystemColors.Info;
            TxtDevDatSpan.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatSpan.Location = new Point(334, 70);
            TxtDevDatSpan.Name = "TxtDevDatSpan";
            TxtDevDatSpan.ReadOnly = true;
            TxtDevDatSpan.Size = new Size(83, 23);
            TxtDevDatSpan.TabIndex = 88;
            TxtDevDatSpan.Text = "-/-";
            TxtDevDatSpan.TextAlign = HorizontalAlignment.Right;
            // 
            // label21
            // 
            label21.Location = new Point(487, 9);
            label21.Name = "label21";
            label21.Size = new Size(30, 25);
            label21.TabIndex = 87;
            label21.Text = "PV:";
            label21.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LblDevDatUnitPV
            // 
            LblDevDatUnitPV.Location = new Point(612, 9);
            LblDevDatUnitPV.Name = "LblDevDatUnitPV";
            LblDevDatUnitPV.Size = new Size(58, 25);
            LblDevDatUnitPV.TabIndex = 86;
            LblDevDatUnitPV.Text = "unit";
            LblDevDatUnitPV.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtDevDatPV
            // 
            TxtDevDatPV.AcceptsReturn = true;
            TxtDevDatPV.BackColor = SystemColors.Info;
            TxtDevDatPV.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatPV.Location = new Point(523, 12);
            TxtDevDatPV.Name = "TxtDevDatPV";
            TxtDevDatPV.ReadOnly = true;
            TxtDevDatPV.Size = new Size(83, 23);
            TxtDevDatPV.TabIndex = 85;
            TxtDevDatPV.Text = "-/-";
            TxtDevDatPV.TextAlign = HorizontalAlignment.Right;
            // 
            // label18
            // 
            label18.Location = new Point(487, 38);
            label18.Name = "label18";
            label18.Size = new Size(30, 25);
            label18.TabIndex = 84;
            label18.Text = "SV:";
            label18.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LblDevDatUnitSV
            // 
            LblDevDatUnitSV.Location = new Point(612, 38);
            LblDevDatUnitSV.Name = "LblDevDatUnitSV";
            LblDevDatUnitSV.Size = new Size(58, 25);
            LblDevDatUnitSV.TabIndex = 83;
            LblDevDatUnitSV.Text = "unit";
            LblDevDatUnitSV.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtDevDatSV
            // 
            TxtDevDatSV.AcceptsReturn = true;
            TxtDevDatSV.BackColor = SystemColors.Info;
            TxtDevDatSV.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatSV.Location = new Point(523, 41);
            TxtDevDatSV.Name = "TxtDevDatSV";
            TxtDevDatSV.ReadOnly = true;
            TxtDevDatSV.Size = new Size(83, 23);
            TxtDevDatSV.TabIndex = 82;
            TxtDevDatSV.Text = "-/-";
            TxtDevDatSV.TextAlign = HorizontalAlignment.Right;
            // 
            // label16
            // 
            label16.Location = new Point(487, 67);
            label16.Name = "label16";
            label16.Size = new Size(30, 25);
            label16.TabIndex = 81;
            label16.Text = "TV:";
            label16.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LblDevDatUnitTV
            // 
            LblDevDatUnitTV.Location = new Point(612, 67);
            LblDevDatUnitTV.Name = "LblDevDatUnitTV";
            LblDevDatUnitTV.Size = new Size(58, 25);
            LblDevDatUnitTV.TabIndex = 80;
            LblDevDatUnitTV.Text = "unit";
            LblDevDatUnitTV.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtDevDatTV
            // 
            TxtDevDatTV.AcceptsReturn = true;
            TxtDevDatTV.BackColor = SystemColors.Info;
            TxtDevDatTV.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatTV.Location = new Point(523, 70);
            TxtDevDatTV.Name = "TxtDevDatTV";
            TxtDevDatTV.ReadOnly = true;
            TxtDevDatTV.Size = new Size(83, 23);
            TxtDevDatTV.TabIndex = 79;
            TxtDevDatTV.Text = "-/-";
            TxtDevDatTV.TextAlign = HorizontalAlignment.Right;
            // 
            // label15
            // 
            label15.Location = new Point(487, 96);
            label15.Name = "label15";
            label15.Size = new Size(30, 25);
            label15.TabIndex = 78;
            label15.Text = "QV:";
            label15.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LblDevDatUnitQV
            // 
            LblDevDatUnitQV.Location = new Point(612, 96);
            LblDevDatUnitQV.Name = "LblDevDatUnitQV";
            LblDevDatUnitQV.Size = new Size(58, 25);
            LblDevDatUnitQV.TabIndex = 77;
            LblDevDatUnitQV.Text = "unit";
            LblDevDatUnitQV.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtDevDatQV
            // 
            TxtDevDatQV.AcceptsReturn = true;
            TxtDevDatQV.BackColor = SystemColors.Info;
            TxtDevDatQV.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatQV.Location = new Point(523, 99);
            TxtDevDatQV.Name = "TxtDevDatQV";
            TxtDevDatQV.ReadOnly = true;
            TxtDevDatQV.Size = new Size(83, 23);
            TxtDevDatQV.TabIndex = 76;
            TxtDevDatQV.Text = "-/-";
            TxtDevDatQV.TextAlign = HorizontalAlignment.Right;
            // 
            // LblDevDatRangeUnitLo
            // 
            LblDevDatRangeUnitLo.Location = new Point(423, 39);
            LblDevDatRangeUnitLo.Name = "LblDevDatRangeUnitLo";
            LblDevDatRangeUnitLo.Size = new Size(58, 25);
            LblDevDatRangeUnitLo.TabIndex = 75;
            LblDevDatRangeUnitLo.Text = "unit";
            LblDevDatRangeUnitLo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtDevDatLowerRange
            // 
            TxtDevDatLowerRange.AcceptsReturn = true;
            TxtDevDatLowerRange.BackColor = SystemColors.Info;
            TxtDevDatLowerRange.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatLowerRange.Location = new Point(334, 41);
            TxtDevDatLowerRange.Name = "TxtDevDatLowerRange";
            TxtDevDatLowerRange.ReadOnly = true;
            TxtDevDatLowerRange.Size = new Size(83, 23);
            TxtDevDatLowerRange.TabIndex = 74;
            TxtDevDatLowerRange.Text = "-/-";
            TxtDevDatLowerRange.TextAlign = HorizontalAlignment.Right;
            // 
            // label13
            // 
            label13.Location = new Point(231, 39);
            label13.Name = "label13";
            label13.Size = new Size(97, 25);
            label13.TabIndex = 73;
            label13.Text = "Lower Range:";
            label13.TextAlign = ContentAlignment.MiddleRight;
            // 
            // LblDevDatRangeUnitUp
            // 
            LblDevDatRangeUnitUp.Location = new Point(423, 10);
            LblDevDatRangeUnitUp.Name = "LblDevDatRangeUnitUp";
            LblDevDatRangeUnitUp.Size = new Size(58, 25);
            LblDevDatRangeUnitUp.TabIndex = 72;
            LblDevDatRangeUnitUp.Text = "unit";
            LblDevDatRangeUnitUp.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtDevDatUpperRange
            // 
            TxtDevDatUpperRange.AcceptsReturn = true;
            TxtDevDatUpperRange.BackColor = SystemColors.Info;
            TxtDevDatUpperRange.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatUpperRange.Location = new Point(334, 12);
            TxtDevDatUpperRange.Name = "TxtDevDatUpperRange";
            TxtDevDatUpperRange.ReadOnly = true;
            TxtDevDatUpperRange.Size = new Size(83, 23);
            TxtDevDatUpperRange.TabIndex = 71;
            TxtDevDatUpperRange.Text = "-/-";
            TxtDevDatUpperRange.TextAlign = HorizontalAlignment.Right;
            // 
            // label10
            // 
            label10.Location = new Point(231, 10);
            label10.Name = "label10";
            label10.Size = new Size(97, 25);
            label10.TabIndex = 70;
            label10.Text = "Upper Range:";
            label10.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtDevDatMessage
            // 
            TxtDevDatMessage.AcceptsReturn = true;
            TxtDevDatMessage.BackColor = SystemColors.Info;
            TxtDevDatMessage.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatMessage.Location = new Point(119, 99);
            TxtDevDatMessage.Name = "TxtDevDatMessage";
            TxtDevDatMessage.ReadOnly = true;
            TxtDevDatMessage.Size = new Size(362, 23);
            TxtDevDatMessage.TabIndex = 69;
            TxtDevDatMessage.Text = "-/-";
            // 
            // label9
            // 
            label9.Location = new Point(16, 97);
            label9.Name = "label9";
            label9.Size = new Size(97, 22);
            label9.TabIndex = 68;
            label9.Text = "Message:";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtDevDatLongTag
            // 
            TxtDevDatLongTag.AcceptsReturn = true;
            TxtDevDatLongTag.BackColor = SystemColors.Info;
            TxtDevDatLongTag.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatLongTag.Location = new Point(119, 70);
            TxtDevDatLongTag.Name = "TxtDevDatLongTag";
            TxtDevDatLongTag.ReadOnly = true;
            TxtDevDatLongTag.Size = new Size(186, 23);
            TxtDevDatLongTag.TabIndex = 67;
            TxtDevDatLongTag.Text = "-/-";
            // 
            // label8
            // 
            label8.Location = new Point(16, 68);
            label8.Name = "label8";
            label8.Size = new Size(97, 22);
            label8.TabIndex = 66;
            label8.Text = "Long Tag:";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtDevDatShortTag
            // 
            TxtDevDatShortTag.AcceptsReturn = true;
            TxtDevDatShortTag.BackColor = SystemColors.Info;
            TxtDevDatShortTag.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatShortTag.Location = new Point(119, 41);
            TxtDevDatShortTag.Name = "TxtDevDatShortTag";
            TxtDevDatShortTag.ReadOnly = true;
            TxtDevDatShortTag.Size = new Size(106, 23);
            TxtDevDatShortTag.TabIndex = 65;
            TxtDevDatShortTag.Text = "-/-";
            // 
            // label7
            // 
            label7.Location = new Point(16, 39);
            label7.Name = "label7";
            label7.Size = new Size(97, 22);
            label7.TabIndex = 64;
            label7.Text = "Short Tag:";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtDevDatUniqueID
            // 
            TxtDevDatUniqueID.AcceptsReturn = true;
            TxtDevDatUniqueID.BackColor = SystemColors.Info;
            TxtDevDatUniqueID.BorderStyle = BorderStyle.FixedSingle;
            TxtDevDatUniqueID.Location = new Point(119, 12);
            TxtDevDatUniqueID.Name = "TxtDevDatUniqueID";
            TxtDevDatUniqueID.ReadOnly = true;
            TxtDevDatUniqueID.Size = new Size(106, 23);
            TxtDevDatUniqueID.TabIndex = 63;
            TxtDevDatUniqueID.Text = "-/-";
            // 
            // label6
            // 
            label6.Location = new Point(8, 11);
            label6.Name = "label6";
            label6.Size = new Size(105, 22);
            label6.TabIndex = 62;
            label6.Text = "Unique Identifier:";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // panTop
            // 
            panTop.Controls.Add(butMnuInfo);
            panTop.Controls.Add(butMnuClearDisplay);
            panTop.Controls.Add(butMnuRecord);
            panTop.Location = new Point(0, 2);
            panTop.Name = "panTop";
            panTop.Size = new Size(758, 29);
            panTop.TabIndex = 4;
            // 
            // butMnuInfo
            // 
            butMnuInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            butMnuInfo.BackColor = Color.Transparent;
            butMnuInfo.FlatAppearance.BorderSize = 0;
            butMnuInfo.FlatStyle = FlatStyle.Flat;
            butMnuInfo.Location = new Point(701, -2);
            butMnuInfo.Name = "butMnuInfo";
            butMnuInfo.Size = new Size(49, 28);
            butMnuInfo.TabIndex = 5;
            butMnuInfo.Text = "Help";
            butMnuInfo.UseVisualStyleBackColor = false;
            butMnuInfo.Click += ButMnuInfo_Click;
            // 
            // butMnuClearDisplay
            // 
            butMnuClearDisplay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            butMnuClearDisplay.BackColor = Color.Transparent;
            butMnuClearDisplay.FlatAppearance.BorderSize = 0;
            butMnuClearDisplay.FlatStyle = FlatStyle.Flat;
            butMnuClearDisplay.Location = new Point(500, -2);
            butMnuClearDisplay.Name = "butMnuClearDisplay";
            butMnuClearDisplay.Size = new Size(103, 28);
            butMnuClearDisplay.TabIndex = 4;
            butMnuClearDisplay.Text = "Clear Display";
            butMnuClearDisplay.UseVisualStyleBackColor = false;
            butMnuClearDisplay.Click += ButMnuClearDisplay_Click;
            // 
            // butMnuRecord
            // 
            butMnuRecord.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            butMnuRecord.BackColor = Color.Transparent;
            butMnuRecord.FlatAppearance.BorderSize = 0;
            butMnuRecord.FlatStyle = FlatStyle.Flat;
            butMnuRecord.Location = new Point(609, -2);
            butMnuRecord.Name = "butMnuRecord";
            butMnuRecord.Size = new Size(86, 28);
            butMnuRecord.TabIndex = 2;
            butMnuRecord.Text = "Record Off";
            butMnuRecord.UseVisualStyleBackColor = false;
            butMnuRecord.Click += ButMnuRecord_Click;
            // 
            // StatusInfo
            // 
            StatusInfo.AutoSize = false;
            StatusInfo.Items.AddRange(new ToolStripItem[] { MainStatusComPortLED, MainStatusInfo, MainStatusLED, MainStatusNumFrames, Tled });
            StatusInfo.Location = new Point(0, 454);
            StatusInfo.Name = "StatusInfo";
            StatusInfo.Size = new Size(784, 27);
            StatusInfo.TabIndex = 6;
            // 
            // MainStatusComPortLED
            // 
            MainStatusComPortLED.AutoSize = false;
            MainStatusComPortLED.Name = "MainStatusComPortLED";
            MainStatusComPortLED.Size = new Size(20, 22);
            // 
            // MainStatusInfo
            // 
            MainStatusInfo.AutoSize = false;
            MainStatusInfo.ImageAlign = ContentAlignment.MiddleLeft;
            MainStatusInfo.Name = "MainStatusInfo";
            MainStatusInfo.Size = new Size(400, 22);
            MainStatusInfo.Text = "No Text Yet";
            MainStatusInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // MainStatusLED
            // 
            MainStatusLED.AutoSize = false;
            MainStatusLED.Name = "MainStatusLED";
            MainStatusLED.Size = new Size(30, 22);
            MainStatusLED.TextAlign = ContentAlignment.MiddleRight;
            MainStatusLED.TextImageRelation = TextImageRelation.Overlay;
            // 
            // MainStatusNumFrames
            // 
            MainStatusNumFrames.AutoSize = false;
            MainStatusNumFrames.Name = "MainStatusNumFrames";
            MainStatusNumFrames.Size = new Size(60, 22);
            MainStatusNumFrames.Text = "0000000";
            MainStatusNumFrames.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // Tled
            // 
            Tled.AutoSize = false;
            Tled.Name = "Tled";
            Tled.Size = new Size(20, 22);
            // 
            // TimOperating
            // 
            TimOperating.Enabled = true;
            TimOperating.Interval = 50;
            TimOperating.Tick += TimOperating_Tick;
            // 
            // ToolTipDecodedData
            // 
            ToolTipDecodedData.AutomaticDelay = 1;
            ToolTipDecodedData.ShowAlways = true;
            ToolTipDecodedData.ToolTipTitle = "Decoded Data";
            ToolTipDecodedData.UseAnimation = false;
            ToolTipDecodedData.UseFading = false;
            // 
            // ToolTipInfo
            // 
            ToolTipInfo.Popup += ToolTipInfo_Popup;
            // 
            // rtxtDisplay
            // 
            rtxtDisplay.BackColor = SystemColors.ControlLightLight;
            rtxtDisplay.BorderStyle = BorderStyle.FixedSingle;
            rtxtDisplay.ContextMenuStrip = CtxMnuDecode;
            rtxtDisplay.DetectUrls = false;
            rtxtDisplay.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtxtDisplay.Location = new Point(7, 204);
            rtxtDisplay.Name = "rtxtDisplay";
            rtxtDisplay.ReadOnly = true;
            rtxtDisplay.Size = new Size(147, 139);
            rtxtDisplay.TabIndex = 0;
            rtxtDisplay.Text = "";
            rtxtDisplay.WordWrap = false;
            // 
            // ImgL_Leds
            // 
            ImgL_Leds.ColorDepth = ColorDepth.Depth16Bit;
            ImgL_Leds.ImageStream = (ImageListStreamer)resources.GetObject("ImgL_Leds.ImageStream");
            ImgL_Leds.TransparentColor = Color.Magenta;
            ImgL_Leds.Images.SetKeyName(0, "led_grey.bmp");
            ImgL_Leds.Images.SetKeyName(1, "led_yell.bmp");
            ImgL_Leds.Images.SetKeyName(2, "led_gree.bmp");
            ImgL_Leds.Images.SetKeyName(3, "led_red.bmp");
            // 
            // FrmTestClient
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(784, 481);
            Controls.Add(rtxtDisplay);
            Controls.Add(StatusInfo);
            Controls.Add(panTop);
            Controls.Add(tabMain);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "FrmTestClient";
            Text = "Test Hart C++ MASTER, V 8.0";
            TopMost = true;
            Closing += FrmMonitor_Closing;
            FormClosing += FrmMonitor_FormClosing;
            Load += FrmMonitor_Load;
            Resize += FrmMonitor_Resize;
            CtxMnuDecode.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabStart.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox6.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            tabHartIp.ResumeLayout(false);
            tabHartIp.PerformLayout();
            tabCommands.ResumeLayout(false);
            tabCommands.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            tabOptions.ResumeLayout(false);
            groupBox22.ResumeLayout(false);
            groupBox19.ResumeLayout(false);
            groupBox16.ResumeLayout(false);
            tabDevData.ResumeLayout(false);
            tabDevData.PerformLayout();
            panTop.ResumeLayout(false);
            StatusInfo.ResumeLayout(false);
            StatusInfo.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer TimWatch;
        private TabControl tabMain;
        private TabPage tabStart;
        private TabPage tabCommands;
        private GroupBox groupBox4;
        private ComboBox CmbComPort;
        private Label label1;
        private Label label3;
        private ComboBox CmbPollAddress;
        private ComboBox CmbNumPreambles;
        private Label lblPreambles;
        private Label label4;
        private ComboBox CmbBaudRate;
        private Label label5;
        private ComboBox CmbMasterRole;
        private GroupBox groupBox5;
        private CheckBox ChkViewTiming;
        private CheckBox ChkViewAddress;
        private CheckBox ChkViewPreambles;
        private GroupBox groupBox6;
        private CheckBox ChkViewStatusBinary;
        private CheckBox ChkViewDecodedData;
        private CheckBox ChkViewFrameNumbers;
        private TabPage tabOptions;
        private GroupBox groupBox9;
        private Button ButConnect;
        private Button ButDisconnect;
        private Button ButSendCmd0short;
        private Button butSendCmd1;
        private Button ButSendCmd2;
        private Button ButSendCmd3;
        private Button ButSendCmd12;
        private Button ButSendCmd13;
        private Button ButSendCmd14;
        private Button ButSendCmd15;
        private Button ButSendCmd18;
        private Button ButEditCmd18;
        private Button ButEditCmd31;
        private Button ButSendCmd31;
        private Button ButEditCmd35;
        private Button ButSendCmd35;
        private Button ButEditUserCmd;
        private Button ButSendUserCmd;
        private Panel panTop;
        private Button butMnuRecord;
        private Button butMnuClearDisplay;
        private Button butMnuInfo;
        private StatusStrip StatusInfo;
        private ToolStripStatusLabel MainStatusInfo;
        private CheckBox ChkTopMost;
        private GroupBox groupBox16;
        private Button ButEditColors;
        private RadioButton RadColorSetUser;
        private RadioButton RadColorSet2;
        private RadioButton RadColorSet1;
        private GroupBox groupBox19;
        private CheckBox ChkRetryIfBusy;
        private ComboBox CmbNumRetries;
        private Label label19;
        private ToolStripStatusLabel MainStatusLED;
        private System.Windows.Forms.Timer TimOperating;
        private ToolStripStatusLabel MainStatusNumFrames;
        private ToolStripStatusLabel MainStatusComPortLED;
        private ToolStripStatusLabel Tled;
        private Button ButSendCmd48;
        private Button ButSendCmd38;
        private ContextMenuStrip CtxMnuDecode;
        private ToolStripMenuItem tsmnuDecInteger;
        private ToolStripMenuItem tsmnuDecFloat;
        private ToolTip ToolTipDecodedData;
        private ToolStripMenuItem tsmnuDecHartUnit;
        private ToolStripMenuItem tsmnuDecPackedASCII;
        private ToolStripMenuItem tsmnuDecText;
        private ToolStripMenuItem tsmnuDecBinary;
        private ToolTip ToolTipInfo;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmnuCopyToAnyFrame;
        private ToolStripMenuItem tsmnuBytesToClipBoard;
        private ToolStripMenuItem tsmnuDecDouble;
        private RichTextBox rtxtDisplay;
        private GroupBox groupBox22;
        private TextBox TxtDevDatUniqueID;
        private Label label6;
        private TabPage tabDevData;
        private GroupBox groupBox8;
        private TextBox TxtConStatus;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private ImageList ImgL_Leds;
        private Button ButUserTestCoding;
        private Button ButFreeTest;
        private Button ButUserTestCommands;
        internal TextBox TxtDevDatMessage;
        private Label label9;
        internal TextBox TxtDevDatLongTag;
        private Label label8;
        internal TextBox TxtDevDatShortTag;
        private Label label7;
        internal Label LblDevDatRangeUnitUp;
        internal TextBox TxtDevDatUpperRange;
        private Label label10;
        internal Label LblDevDatRangeUnitLo;
        internal TextBox TxtDevDatLowerRange;
        private Label label13;
        private Label label21;
        internal Label LblDevDatUnitPV;
        internal TextBox TxtDevDatPV;
        private Label label18;
        internal Label LblDevDatUnitSV;
        internal TextBox TxtDevDatSV;
        private Label label16;
        internal Label LblDevDatUnitTV;
        internal TextBox TxtDevDatTV;
        private Label label15;
        internal Label LblDevDatUnitQV;
        internal TextBox TxtDevDatQV;
        internal TextBox TxtDevDatCurrent;
        private Label label24;
        internal Label LblDevDatSpanUnit;
        internal TextBox TxtDevDatSpan;
        private Button ButReadAll;
        private Label label25;
        private RichTextBox rtxtTestDisplay;
        private TabPage tabHartIp;
        private TextBox TxtHartIpAddress;
        private Label label12;
        private TextBox TxtHartIpHostName;
        private Label label11;
        private TextBox TxtHartIpPort;
        private Label label14;
        private Label label17;
        private RichTextBox rtxIPFrames;
        private CheckBox ChkHartIpUseAddress;
        private TextBox TxtConnectionStatus;
        private Label label22;
        private CheckBox ChkViewShortBursts;
        private TextBox TxtLastError;
        private Label label72;
        private Label label2;
        private Button ButSetDefault;
        private CheckBox ChkHartIpEnabled;
        private CheckBox ChkHartIpSendKeepAlive;
    }
}
