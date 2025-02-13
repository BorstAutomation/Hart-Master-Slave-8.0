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
            panTop = new Panel();
            butMnuInfo = new Button();
            butMnuClearDisplay = new Button();
            butMnuRecord = new Button();
            butMnuUpdateData = new Button();
            StatusInfo = new StatusStrip();
            MainStatusComPortLED = new ToolStripStatusLabel();
            MainStatusInfo = new ToolStripStatusLabel();
            MainStatusLED = new ToolStripStatusLabel();
            MainStatusNumFrames = new ToolStripStatusLabel();
            Tled = new ToolStripStatusLabel();
            TimOperating = new System.Windows.Forms.Timer(components);
            ToolTipDecodedData = new ToolTip(components);
            ToolTipInfo = new ToolTip(components);
            CmbPollAddress = new ComboBox();
            CmbComPort = new ComboBox();
            CmbBaudRate = new ComboBox();
            CmbReqPreambles = new ComboBox();
            CmbRspPreambles = new ComboBox();
            ChkViewPreambles = new CheckBox();
            ChkViewAddress = new CheckBox();
            ChkViewTiming = new CheckBox();
            ChkViewFrameNumbers = new CheckBox();
            ChkViewDecodedData = new CheckBox();
            ChkViewStatusBinary = new CheckBox();
            rtxtDisplay = new RichTextBox();
            ImgL_Leds = new ImageList(components);
            tabOptions = new TabPage();
            groupBox16 = new GroupBox();
            ButEditColors = new Button();
            ChkTopMost = new CheckBox();
            RadColorSetUser = new RadioButton();
            RadColorSet2 = new RadioButton();
            RadColorSet1 = new RadioButton();
            tabDevVars = new TabPage();
            groupBox10 = new GroupBox();
            RadPV4bad = new RadioButton();
            RadPV4good = new RadioButton();
            label46 = new Label();
            CmbPV4unit = new ComboBox();
            label47 = new Label();
            label48 = new Label();
            TxtPV4value = new TextBox();
            TxtPV4class = new TextBox();
            groupBox8 = new GroupBox();
            RadPV3bad = new RadioButton();
            RadPV3good = new RadioButton();
            label40 = new Label();
            CmbPV3unit = new ComboBox();
            label41 = new Label();
            label42 = new Label();
            TxtPV3value = new TextBox();
            TxtPV3class = new TextBox();
            groupBox7 = new GroupBox();
            RadPV2bad = new RadioButton();
            RadPV2good = new RadioButton();
            label37 = new Label();
            CmbPV2unit = new ComboBox();
            label38 = new Label();
            label39 = new Label();
            TxtPV2value = new TextBox();
            TxtPV2class = new TextBox();
            groupBox3 = new GroupBox();
            RadPV1bad = new RadioButton();
            RadPV1good = new RadioButton();
            label21 = new Label();
            CmbPV1unit = new ComboBox();
            label35 = new Label();
            label36 = new Label();
            TxtPV1value = new TextBox();
            TxtPV1class = new TextBox();
            groupBox2 = new GroupBox();
            label43 = new Label();
            RadCurBad = new RadioButton();
            RadCurGood = new RadioButton();
            label15 = new Label();
            label16 = new Label();
            TxtCurValue = new TextBox();
            TxtCurClass = new TextBox();
            groupBox9 = new GroupBox();
            label18 = new Label();
            RadPercBad = new RadioButton();
            RadPerGood = new RadioButton();
            label45 = new Label();
            label44 = new Label();
            TxtPerValue = new TextBox();
            TxtPerClass = new TextBox();
            tabTransducer = new TabPage();
            linkTable26 = new LinkLabel();
            TxtChanFlags = new TextBox();
            label58 = new Label();
            label57 = new Label();
            CmbTFunction = new ComboBox();
            label56 = new Label();
            CmbAlmSelect = new ComboBox();
            label55 = new Label();
            CmbWrProt = new ComboBox();
            label54 = new Label();
            TxtDamping = new TextBox();
            label53 = new Label();
            TxtLowerRange = new TextBox();
            TxtUpperRange = new TextBox();
            label51 = new Label();
            label52 = new Label();
            label50 = new Label();
            CmbRangeUnit = new ComboBox();
            TxtLowerLimit = new TextBox();
            TxtMinSpan = new TextBox();
            TxtTransdSerNum = new TextBox();
            label49 = new Label();
            label25 = new Label();
            label24 = new Label();
            label13 = new Label();
            label10 = new Label();
            CmbTransdUnit = new ComboBox();
            TxtUpperLimit = new TextBox();
            tabDevice = new TabPage();
            ChkSIunitsOnly = new CheckBox();
            TxtCountryLetters = new TextBox();
            label68 = new Label();
            TxtFinAssNum = new TextBox();
            label59 = new Label();
            TxtYear = new TextBox();
            TxtMonth = new TextBox();
            TxtDay = new TextBox();
            TxtDescriptor = new TextBox();
            TxtMessage = new TextBox();
            TxtLongTag = new TextBox();
            TxtShortTag = new TextBox();
            label34 = new Label();
            label33 = new Label();
            label32 = new Label();
            label31 = new Label();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            tabIdentifier = new TabPage();
            groupBox1 = new GroupBox();
            LinkTable57 = new LinkLabel();
            LinkTable17 = new LinkLabel();
            LinkTable10 = new LinkLabel();
            LinkTable8 = new LinkLabel();
            LinkTable1 = new LinkLabel();
            LinkCmd0 = new LinkLabel();
            TxtProfileTable57 = new TextBox();
            TxtDistributorTable8 = new TextBox();
            TxtManufacturerTable8 = new TextBox();
            TxtExtDevStatus = new TextBox();
            TxtConfigChangedCounter = new TextBox();
            TxtLastDevVarCode = new TextBox();
            TxtDevUniqueID = new TextBox();
            TxtFlagsTable11 = new TextBox();
            TxtSignalTable10 = new TextBox();
            TxtHwRev = new TextBox();
            TxtSwRev = new TextBox();
            TxtDevRev = new TextBox();
            TxtDevTypeTable1 = new TextBox();
            label30 = new Label();
            label29 = new Label();
            label28 = new Label();
            label27 = new Label();
            label26 = new Label();
            label23 = new Label();
            label22 = new Label();
            label20 = new Label();
            label19 = new Label();
            label17 = new Label();
            label14 = new Label();
            label12 = new Label();
            label6 = new Label();
            tabStart = new TabPage();
            ChkSimWriteProtect = new CheckBox();
            ChkViewShortBursts = new CheckBox();
            ChkBurstMode = new CheckBox();
            label5 = new Label();
            groupBox5 = new GroupBox();
            GrpHartFraming = new GroupBox();
            label11 = new Label();
            label2 = new Label();
            groupBox4 = new GroupBox();
            label4 = new Label();
            label1 = new Label();
            label3 = new Label();
            tabMain = new TabControl();
            tabHartIp = new TabPage();
            ChkHartIpEnabled = new CheckBox();
            ButSetDefault = new Button();
            TxtLastError = new TextBox();
            label72 = new Label();
            label71 = new Label();
            TxtConnectionStatus = new TextBox();
            label61 = new Label();
            ChkHartIpUseAddress = new CheckBox();
            rtxIPFrames = new RichTextBox();
            TxtHartIpPort = new TextBox();
            label62 = new Label();
            TxtHartIpAddress = new TextBox();
            label69 = new Label();
            TxtHartIpHostName = new TextBox();
            label70 = new Label();
            tabStatus = new TabPage();
            TxtAddDev5 = new TextBox();
            linkTable27 = new LinkLabel();
            linkTable32 = new LinkLabel();
            TxtAddDevStandard3 = new TextBox();
            linkTable29_31 = new LinkLabel();
            label67 = new Label();
            label66 = new Label();
            TxtAddDevFixed = new TextBox();
            label65 = new Label();
            TxtAddDevStaturated = new TextBox();
            label64 = new Label();
            TxtAddDevStandard2 = new TextBox();
            TxtAddDevStandard1 = new TextBox();
            TxtAddDevStandard0 = new TextBox();
            label63 = new Label();
            TxtAddDev4 = new TextBox();
            TxtAddDev3 = new TextBox();
            TxtAddDev2 = new TextBox();
            TxtAddDev1 = new TextBox();
            TxtAddDev0 = new TextBox();
            label60 = new Label();
            CtxMnuDecode.SuspendLayout();
            panTop.SuspendLayout();
            StatusInfo.SuspendLayout();
            tabOptions.SuspendLayout();
            groupBox16.SuspendLayout();
            tabDevVars.SuspendLayout();
            groupBox10.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox9.SuspendLayout();
            tabTransducer.SuspendLayout();
            tabDevice.SuspendLayout();
            tabIdentifier.SuspendLayout();
            groupBox1.SuspendLayout();
            tabStart.SuspendLayout();
            groupBox5.SuspendLayout();
            GrpHartFraming.SuspendLayout();
            groupBox4.SuspendLayout();
            tabMain.SuspendLayout();
            tabHartIp.SuspendLayout();
            tabStatus.SuspendLayout();
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
            // panTop
            // 
            panTop.Controls.Add(butMnuInfo);
            panTop.Controls.Add(butMnuClearDisplay);
            panTop.Controls.Add(butMnuRecord);
            panTop.Controls.Add(butMnuUpdateData);
            panTop.Location = new Point(0, 2);
            panTop.Name = "panTop";
            panTop.Size = new Size(790, 29);
            panTop.TabIndex = 4;
            // 
            // butMnuInfo
            // 
            butMnuInfo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            butMnuInfo.BackColor = Color.Transparent;
            butMnuInfo.FlatAppearance.BorderSize = 0;
            butMnuInfo.FlatStyle = FlatStyle.Flat;
            butMnuInfo.Location = new Point(741, 0);
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
            butMnuClearDisplay.Location = new Point(540, 0);
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
            butMnuRecord.Location = new Point(649, 0);
            butMnuRecord.Name = "butMnuRecord";
            butMnuRecord.Size = new Size(86, 28);
            butMnuRecord.TabIndex = 2;
            butMnuRecord.Text = "Record Off";
            butMnuRecord.UseVisualStyleBackColor = false;
            butMnuRecord.Click += ButMnuRecord_Click;
            // 
            // butMnuUpdateData
            // 
            butMnuUpdateData.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            butMnuUpdateData.BackColor = Color.Transparent;
            butMnuUpdateData.FlatAppearance.BorderSize = 0;
            butMnuUpdateData.FlatStyle = FlatStyle.Flat;
            butMnuUpdateData.Font = new Font("Segoe UI", 9F);
            butMnuUpdateData.Location = new Point(399, 0);
            butMnuUpdateData.Name = "butMnuUpdateData";
            butMnuUpdateData.Size = new Size(135, 28);
            butMnuUpdateData.TabIndex = 0;
            butMnuUpdateData.Text = "Update Slave Data";
            butMnuUpdateData.UseVisualStyleBackColor = false;
            butMnuUpdateData.Click += ButMnuUpdateData_Click;
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
            // CmbPollAddress
            // 
            CmbPollAddress.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbPollAddress.FlatStyle = FlatStyle.Flat;
            CmbPollAddress.Items.AddRange(new object[] { "A_00", "A_01", "A_02", "A_03", "A_04", "A_05", "A_06", "A_07", "A_08", "A_09", "A_10", "A_11", "A_12", "A_13", "A_14", "A_15", "A_16", "A_17", "A_18", "A_19", "A_20", "A_21", "A_22", "A_23", "A_24", "A_25", "A_26", "A_27", "A_28", "A_29", "A_30", "A_31", "A_32", "A_33", "A_34", "A_35", "A_36", "A_37", "A_38", "A_39", "A_40", "A_41", "A_42", "A_43", "A_44", "A_45", "A_46", "A_47", "A_48", "A_49", "A_50", "A_51", "A_52", "A_53", "A_54", "A_55", "A_56", "A_57", "A_58", "A_59", "A_60", "A_61", "A_62", "A_63" });
            CmbPollAddress.Location = new Point(119, 58);
            CmbPollAddress.Name = "CmbPollAddress";
            CmbPollAddress.Size = new Size(96, 23);
            CmbPollAddress.TabIndex = 7;
            ToolTipInfo.SetToolTip(CmbPollAddress, "Poll Address used to retrieve the Unique ID (usually 0)");
            CmbPollAddress.SelectedIndexChanged += CmbPollAddress_SelectedIndexChanged;
            // 
            // CmbComPort
            // 
            CmbComPort.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbComPort.FlatStyle = FlatStyle.Popup;
            CmbComPort.Items.AddRange(new object[] { "None", "Hart IP", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "COM10", "COM11", "COM12", "COM13", "COM14", "COM15", "COM16", "COM17", "COM18", "COM19", "COM20", "COM21", "COM22", "COM23", "COM24", "COM25", "COM26", "COM27", "COM28", "COM29", "COM30", "COM31", "COM32", "COM33", "COM34", "COM35", "COM36", "COM37", "COM38", "COM39", "COM40", "COM41", "COM42", "COM43", "COM44", "COM45", "COM46", "COM47", "COM48", "COM49", "COM50", "COM51", "COM52", "COM53", "COM54", "COM55", "COM56", "COM57", "COM58", "COM59", "COM60", "COM61", "COM62", "COM63", "COM64", "COM65", "COM66", "COM67", "COM68", "COM69", "COM70", "COM71", "COM72", "COM73", "COM74", "COM75", "COM76", "COM77", "COM78", "COM79", "COM80", "COM81", "COM82", "COM83", "COM84", "COM85", "COM86", "COM87", "COM88", "COM89", "COM90", "COM91", "COM92", "COM93", "COM94", "COM95", "COM96", "COM97", "COM98", "COM99", "COM100", "COM101", "COM102", "COM103", "COM104", "COM105", "COM106", "COM107", "COM108", "COM109", "COM110", "COM111", "COM112", "COM113", "COM114", "COM115", "COM116", "COM117", "COM118", "COM119", "COM120", "COM121", "COM122", "COM123", "COM124", "COM125", "COM126", "COM127", "COM128", "COM129", "COM130", "COM131", "COM132", "COM133", "COM134", "COM135", "COM136", "COM137", "COM138", "COM139", "COM140", "COM141", "COM142", "COM143", "COM144", "COM145", "COM146", "COM147", "COM148", "COM149", "COM150", "COM151", "COM152", "COM153", "COM154", "COM155", "COM156", "COM157", "COM158", "COM159", "COM160", "COM161", "COM162", "COM163", "COM164", "COM165", "COM166", "COM167", "COM168", "COM169", "COM170", "COM171", "COM172", "COM173", "COM174", "COM175", "COM176", "COM177", "COM178", "COM179", "COM180", "COM181", "COM182", "COM183", "COM184", "COM185", "COM186", "COM187", "COM188", "COM189", "COM190", "COM191", "COM192", "COM193", "COM194", "COM195", "COM196", "COM197", "COM198", "COM199", "COM200", "COM201", "COM202", "COM203", "COM204", "COM205", "COM206", "COM207", "COM208", "COM209", "COM210", "COM211", "COM212", "COM213", "COM214", "COM215", "COM216", "COM217", "COM218", "COM219", "COM220", "COM221", "COM222", "COM223", "COM224", "COM225", "COM226", "COM227", "COM228", "COM229", "COM230", "COM231", "COM232", "COM233", "COM234", "COM235", "COM236", "COM237", "COM238", "COM239", "COM240", "COM241", "COM242", "COM243", "COM244", "COM245", "COM246", "COM247", "COM248", "COM249", "COM250" });
            CmbComPort.Location = new Point(119, 25);
            CmbComPort.Name = "CmbComPort";
            CmbComPort.Size = new Size(96, 23);
            CmbComPort.TabIndex = 29;
            ToolTipInfo.SetToolTip(CmbComPort, "Com Port for the Hart Device Connection");
            CmbComPort.SelectedIndexChanged += CmbComPort_SelectedIndexChanged;
            // 
            // CmbBaudRate
            // 
            CmbBaudRate.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbBaudRate.Items.AddRange(new object[] { "1200", "2400", "4800", "9600", "19200", "38400", "57600", "115200" });
            CmbBaudRate.Location = new Point(119, 91);
            CmbBaudRate.Name = "CmbBaudRate";
            CmbBaudRate.Size = new Size(83, 23);
            CmbBaudRate.TabIndex = 34;
            ToolTipInfo.SetToolTip(CmbBaudRate, "The Standard Baud Rate is 1200. For Multiplexers other baud rates may be required");
            CmbBaudRate.SelectedIndexChanged += CmbBaudRate_SelectedIndexChanged;
            // 
            // CmbReqPreambles
            // 
            CmbReqPreambles.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbReqPreambles.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22" });
            CmbReqPreambles.Location = new Point(137, 21);
            CmbReqPreambles.Name = "CmbReqPreambles";
            CmbReqPreambles.Size = new Size(55, 23);
            CmbReqPreambles.TabIndex = 21;
            ToolTipInfo.SetToolTip(CmbReqPreambles, "Range 2..20, the standard is 5");
            CmbReqPreambles.SelectedIndexChanged += CmbNumReqPreambles_SelectedIndexChanged;
            // 
            // CmbRspPreambles
            // 
            CmbRspPreambles.DropDownStyle = ComboBoxStyle.DropDownList;
            CmbRspPreambles.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22" });
            CmbRspPreambles.Location = new Point(137, 50);
            CmbRspPreambles.Name = "CmbRspPreambles";
            CmbRspPreambles.Size = new Size(55, 23);
            CmbRspPreambles.TabIndex = 32;
            ToolTipInfo.SetToolTip(CmbRspPreambles, "Range 2..20, the standard is 5");
            CmbRspPreambles.SelectedIndexChanged += CmbRspPreambles_SelectedIndexChanged;
            // 
            // ChkViewPreambles
            // 
            ChkViewPreambles.Location = new Point(12, 21);
            ChkViewPreambles.Name = "ChkViewPreambles";
            ChkViewPreambles.Size = new Size(103, 24);
            ChkViewPreambles.TabIndex = 0;
            ChkViewPreambles.Text = "Preambles";
            ToolTipInfo.SetToolTip(ChkViewPreambles, "Show preambles in frame display");
            ChkViewPreambles.UseVisualStyleBackColor = true;
            ChkViewPreambles.CheckedChanged += ChkViewPreambles_CheckedChanged;
            // 
            // ChkViewAddress
            // 
            ChkViewAddress.Location = new Point(241, 21);
            ChkViewAddress.Name = "ChkViewAddress";
            ChkViewAddress.Size = new Size(81, 24);
            ChkViewAddress.TabIndex = 1;
            ChkViewAddress.Text = "Address";
            ToolTipInfo.SetToolTip(ChkViewAddress, "Show the addressing information in the frame");
            ChkViewAddress.UseVisualStyleBackColor = true;
            ChkViewAddress.CheckedChanged += ChkViewAddress_CheckedChanged;
            // 
            // ChkViewTiming
            // 
            ChkViewTiming.Location = new Point(121, 48);
            ChkViewTiming.Name = "ChkViewTiming";
            ChkViewTiming.Size = new Size(81, 25);
            ChkViewTiming.TabIndex = 2;
            ChkViewTiming.Text = "Timing";
            ToolTipInfo.SetToolTip(ChkViewTiming, "Show start time and duration of frames");
            ChkViewTiming.UseVisualStyleBackColor = true;
            ChkViewTiming.CheckedChanged += ChkViewTiming_CheckedChanged;
            // 
            // ChkViewFrameNumbers
            // 
            ChkViewFrameNumbers.Location = new Point(121, 20);
            ChkViewFrameNumbers.Name = "ChkViewFrameNumbers";
            ChkViewFrameNumbers.Size = new Size(114, 24);
            ChkViewFrameNumbers.TabIndex = 3;
            ChkViewFrameNumbers.Text = "Frame Numbers";
            ToolTipInfo.SetToolTip(ChkViewFrameNumbers, "Display numbers with the frames");
            ChkViewFrameNumbers.UseVisualStyleBackColor = true;
            ChkViewFrameNumbers.CheckedChanged += ChkViewFrameNumbers_CheckedChanged;
            // 
            // ChkViewDecodedData
            // 
            ChkViewDecodedData.Location = new Point(12, 48);
            ChkViewDecodedData.Name = "ChkViewDecodedData";
            ChkViewDecodedData.Size = new Size(103, 24);
            ChkViewDecodedData.TabIndex = 4;
            ChkViewDecodedData.Text = "Decoded Data";
            ToolTipInfo.SetToolTip(ChkViewDecodedData, "Decode the data in known sections (frames)");
            ChkViewDecodedData.UseVisualStyleBackColor = true;
            ChkViewDecodedData.CheckedChanged += ChkViewDecodedData_CheckedChanged;
            // 
            // ChkViewStatusBinary
            // 
            ChkViewStatusBinary.Location = new Point(241, 48);
            ChkViewStatusBinary.Name = "ChkViewStatusBinary";
            ChkViewStatusBinary.Size = new Size(114, 25);
            ChkViewStatusBinary.TabIndex = 5;
            ChkViewStatusBinary.Text = "Status Details";
            ToolTipInfo.SetToolTip(ChkViewStatusBinary, "Show status details in binary format");
            ChkViewStatusBinary.UseVisualStyleBackColor = true;
            ChkViewStatusBinary.CheckedChanged += ChkViewStatusBinary_CheckedChanged;
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
            // tabOptions
            // 
            tabOptions.Controls.Add(groupBox16);
            tabOptions.Location = new Point(4, 27);
            tabOptions.Name = "tabOptions";
            tabOptions.Size = new Size(776, 134);
            tabOptions.TabIndex = 6;
            tabOptions.Text = "Options";
            tabOptions.UseVisualStyleBackColor = true;
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
            // tabDevVars
            // 
            tabDevVars.Controls.Add(groupBox10);
            tabDevVars.Controls.Add(groupBox8);
            tabDevVars.Controls.Add(groupBox7);
            tabDevVars.Controls.Add(groupBox3);
            tabDevVars.Controls.Add(groupBox2);
            tabDevVars.Controls.Add(groupBox9);
            tabDevVars.Location = new Point(4, 27);
            tabDevVars.Name = "tabDevVars";
            tabDevVars.Size = new Size(776, 134);
            tabDevVars.TabIndex = 8;
            tabDevVars.Text = "Device Variables";
            tabDevVars.UseVisualStyleBackColor = true;
            // 
            // groupBox10
            // 
            groupBox10.Controls.Add(RadPV4bad);
            groupBox10.Controls.Add(RadPV4good);
            groupBox10.Controls.Add(label46);
            groupBox10.Controls.Add(CmbPV4unit);
            groupBox10.Controls.Add(label47);
            groupBox10.Controls.Add(label48);
            groupBox10.Controls.Add(TxtPV4value);
            groupBox10.Controls.Add(TxtPV4class);
            groupBox10.Location = new Point(638, 3);
            groupBox10.Name = "groupBox10";
            groupBox10.Size = new Size(121, 128);
            groupBox10.TabIndex = 109;
            groupBox10.TabStop = false;
            groupBox10.Text = "PV4 (3, 249)";
            // 
            // RadPV4bad
            // 
            RadPV4bad.AutoSize = true;
            RadPV4bad.Location = new Point(65, 107);
            RadPV4bad.Name = "RadPV4bad";
            RadPV4bad.Size = new Size(45, 19);
            RadPV4bad.TabIndex = 77;
            RadPV4bad.Text = "Bad";
            RadPV4bad.UseVisualStyleBackColor = true;
            RadPV4bad.CheckedChanged += RadPV4bad_CheckedChanged;
            // 
            // RadPV4good
            // 
            RadPV4good.AutoSize = true;
            RadPV4good.Checked = true;
            RadPV4good.Location = new Point(5, 107);
            RadPV4good.Name = "RadPV4good";
            RadPV4good.Size = new Size(54, 19);
            RadPV4good.TabIndex = 76;
            RadPV4good.TabStop = true;
            RadPV4good.Text = "Good";
            RadPV4good.UseVisualStyleBackColor = true;
            RadPV4good.CheckedChanged += RadPV4good_CheckedChanged;
            // 
            // label46
            // 
            label46.Location = new Point(5, 19);
            label46.Name = "label46";
            label46.Size = new Size(43, 22);
            label46.TabIndex = 70;
            label46.Text = "Class:";
            label46.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbPV4unit
            // 
            CmbPV4unit.FormattingEnabled = true;
            CmbPV4unit.Location = new Point(53, 78);
            CmbPV4unit.Name = "CmbPV4unit";
            CmbPV4unit.Size = new Size(60, 23);
            CmbPV4unit.TabIndex = 75;
            CmbPV4unit.SelectedIndexChanged += CmbPV4unit_SelectedIndexChanged;
            // 
            // label47
            // 
            label47.Location = new Point(5, 48);
            label47.Name = "label47";
            label47.Size = new Size(43, 22);
            label47.TabIndex = 72;
            label47.Text = "Value:";
            label47.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label48
            // 
            label48.Location = new Point(5, 77);
            label48.Name = "label48";
            label48.Size = new Size(43, 22);
            label48.TabIndex = 74;
            label48.Text = "Unit:";
            label48.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtPV4value
            // 
            TxtPV4value.AcceptsReturn = true;
            TxtPV4value.BackColor = SystemColors.Window;
            TxtPV4value.BorderStyle = BorderStyle.FixedSingle;
            TxtPV4value.Location = new Point(54, 50);
            TxtPV4value.Name = "TxtPV4value";
            TxtPV4value.Size = new Size(59, 23);
            TxtPV4value.TabIndex = 73;
            TxtPV4value.Text = "20.0";
            TxtPV4value.TextChanged += TxtPV4value_TextChanged;
            // 
            // TxtPV4class
            // 
            TxtPV4class.AcceptsReturn = true;
            TxtPV4class.BackColor = SystemColors.Window;
            TxtPV4class.BorderStyle = BorderStyle.FixedSingle;
            TxtPV4class.Location = new Point(54, 21);
            TxtPV4class.Name = "TxtPV4class";
            TxtPV4class.Size = new Size(32, 23);
            TxtPV4class.TabIndex = 71;
            TxtPV4class.Text = "64";
            TxtPV4class.TextChanged += TxtPV4class_TextChanged;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(RadPV3bad);
            groupBox8.Controls.Add(RadPV3good);
            groupBox8.Controls.Add(label40);
            groupBox8.Controls.Add(CmbPV3unit);
            groupBox8.Controls.Add(label41);
            groupBox8.Controls.Add(label42);
            groupBox8.Controls.Add(TxtPV3value);
            groupBox8.Controls.Add(TxtPV3class);
            groupBox8.Location = new Point(511, 3);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(121, 128);
            groupBox8.TabIndex = 108;
            groupBox8.TabStop = false;
            groupBox8.Text = "PV3 (2, 248)";
            // 
            // RadPV3bad
            // 
            RadPV3bad.AutoSize = true;
            RadPV3bad.Location = new Point(65, 107);
            RadPV3bad.Name = "RadPV3bad";
            RadPV3bad.Size = new Size(45, 19);
            RadPV3bad.TabIndex = 77;
            RadPV3bad.Text = "Bad";
            RadPV3bad.UseVisualStyleBackColor = true;
            RadPV3bad.CheckedChanged += RadPV3bad_CheckedChanged;
            // 
            // RadPV3good
            // 
            RadPV3good.AutoSize = true;
            RadPV3good.Checked = true;
            RadPV3good.Location = new Point(5, 107);
            RadPV3good.Name = "RadPV3good";
            RadPV3good.Size = new Size(54, 19);
            RadPV3good.TabIndex = 76;
            RadPV3good.TabStop = true;
            RadPV3good.Text = "Good";
            RadPV3good.UseVisualStyleBackColor = true;
            RadPV3good.CheckedChanged += RadPV3good_CheckedChanged;
            // 
            // label40
            // 
            label40.Location = new Point(5, 19);
            label40.Name = "label40";
            label40.Size = new Size(43, 22);
            label40.TabIndex = 70;
            label40.Text = "Class:";
            label40.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbPV3unit
            // 
            CmbPV3unit.FormattingEnabled = true;
            CmbPV3unit.Location = new Point(53, 78);
            CmbPV3unit.Name = "CmbPV3unit";
            CmbPV3unit.Size = new Size(60, 23);
            CmbPV3unit.TabIndex = 75;
            CmbPV3unit.SelectedIndexChanged += CmbPV3unit_SelectedIndexChanged;
            // 
            // label41
            // 
            label41.Location = new Point(5, 48);
            label41.Name = "label41";
            label41.Size = new Size(43, 22);
            label41.TabIndex = 72;
            label41.Text = "Value:";
            label41.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label42
            // 
            label42.Location = new Point(5, 77);
            label42.Name = "label42";
            label42.Size = new Size(43, 22);
            label42.TabIndex = 74;
            label42.Text = "Unit:";
            label42.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtPV3value
            // 
            TxtPV3value.AcceptsReturn = true;
            TxtPV3value.BackColor = SystemColors.Window;
            TxtPV3value.BorderStyle = BorderStyle.FixedSingle;
            TxtPV3value.Location = new Point(54, 50);
            TxtPV3value.Name = "TxtPV3value";
            TxtPV3value.Size = new Size(59, 23);
            TxtPV3value.TabIndex = 73;
            TxtPV3value.Text = "1.0";
            TxtPV3value.TextChanged += TxtPV3value_TextChanged;
            // 
            // TxtPV3class
            // 
            TxtPV3class.AcceptsReturn = true;
            TxtPV3class.BackColor = SystemColors.Window;
            TxtPV3class.BorderStyle = BorderStyle.FixedSingle;
            TxtPV3class.Location = new Point(54, 21);
            TxtPV3class.Name = "TxtPV3class";
            TxtPV3class.Size = new Size(32, 23);
            TxtPV3class.TabIndex = 71;
            TxtPV3class.Text = "73";
            TxtPV3class.TextChanged += TxtPV3class_TextChanged;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(RadPV2bad);
            groupBox7.Controls.Add(RadPV2good);
            groupBox7.Controls.Add(label37);
            groupBox7.Controls.Add(CmbPV2unit);
            groupBox7.Controls.Add(label38);
            groupBox7.Controls.Add(label39);
            groupBox7.Controls.Add(TxtPV2value);
            groupBox7.Controls.Add(TxtPV2class);
            groupBox7.Location = new Point(384, 3);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(121, 128);
            groupBox7.TabIndex = 107;
            groupBox7.TabStop = false;
            groupBox7.Text = "PV2 (1, 247)";
            // 
            // RadPV2bad
            // 
            RadPV2bad.AutoSize = true;
            RadPV2bad.Location = new Point(65, 107);
            RadPV2bad.Name = "RadPV2bad";
            RadPV2bad.Size = new Size(45, 19);
            RadPV2bad.TabIndex = 77;
            RadPV2bad.Text = "Bad";
            RadPV2bad.UseVisualStyleBackColor = true;
            RadPV2bad.CheckedChanged += RadPV2bad_CheckedChanged;
            // 
            // RadPV2good
            // 
            RadPV2good.AutoSize = true;
            RadPV2good.Checked = true;
            RadPV2good.Location = new Point(5, 107);
            RadPV2good.Name = "RadPV2good";
            RadPV2good.Size = new Size(54, 19);
            RadPV2good.TabIndex = 76;
            RadPV2good.TabStop = true;
            RadPV2good.Text = "Good";
            RadPV2good.UseVisualStyleBackColor = true;
            RadPV2good.CheckedChanged += RadPV2good_CheckedChanged;
            // 
            // label37
            // 
            label37.Location = new Point(5, 19);
            label37.Name = "label37";
            label37.Size = new Size(43, 22);
            label37.TabIndex = 70;
            label37.Text = "Class:";
            label37.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbPV2unit
            // 
            CmbPV2unit.FormattingEnabled = true;
            CmbPV2unit.Location = new Point(53, 78);
            CmbPV2unit.Name = "CmbPV2unit";
            CmbPV2unit.Size = new Size(60, 23);
            CmbPV2unit.TabIndex = 75;
            CmbPV2unit.SelectedIndexChanged += CmbPV2unit_SelectedIndexChanged;
            // 
            // label38
            // 
            label38.Location = new Point(5, 48);
            label38.Name = "label38";
            label38.Size = new Size(43, 22);
            label38.TabIndex = 72;
            label38.Text = "Value:";
            label38.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label39
            // 
            label39.Location = new Point(5, 77);
            label39.Name = "label39";
            label39.Size = new Size(43, 22);
            label39.TabIndex = 74;
            label39.Text = "Unit:";
            label39.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtPV2value
            // 
            TxtPV2value.AcceptsReturn = true;
            TxtPV2value.BackColor = SystemColors.Window;
            TxtPV2value.BorderStyle = BorderStyle.FixedSingle;
            TxtPV2value.Location = new Point(54, 50);
            TxtPV2value.Name = "TxtPV2value";
            TxtPV2value.Size = new Size(59, 23);
            TxtPV2value.TabIndex = 73;
            TxtPV2value.Text = "1250.0";
            TxtPV2value.TextChanged += TxtPV2value_TextChanged;
            // 
            // TxtPV2class
            // 
            TxtPV2class.AcceptsReturn = true;
            TxtPV2class.BackColor = SystemColors.Window;
            TxtPV2class.BorderStyle = BorderStyle.FixedSingle;
            TxtPV2class.Location = new Point(54, 21);
            TxtPV2class.Name = "TxtPV2class";
            TxtPV2class.Size = new Size(32, 23);
            TxtPV2class.TabIndex = 71;
            TxtPV2class.Text = "65";
            TxtPV2class.TextChanged += TxtPV2class_TextChanged;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(RadPV1bad);
            groupBox3.Controls.Add(RadPV1good);
            groupBox3.Controls.Add(label21);
            groupBox3.Controls.Add(CmbPV1unit);
            groupBox3.Controls.Add(label35);
            groupBox3.Controls.Add(label36);
            groupBox3.Controls.Add(TxtPV1value);
            groupBox3.Controls.Add(TxtPV1class);
            groupBox3.Location = new Point(257, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(121, 128);
            groupBox3.TabIndex = 106;
            groupBox3.TabStop = false;
            groupBox3.Text = "PV1 (0, 246)";
            // 
            // RadPV1bad
            // 
            RadPV1bad.AutoSize = true;
            RadPV1bad.Location = new Point(65, 107);
            RadPV1bad.Name = "RadPV1bad";
            RadPV1bad.Size = new Size(45, 19);
            RadPV1bad.TabIndex = 77;
            RadPV1bad.Text = "Bad";
            RadPV1bad.UseVisualStyleBackColor = true;
            RadPV1bad.CheckedChanged += RadPV1bad_CheckedChanged;
            // 
            // RadPV1good
            // 
            RadPV1good.AutoSize = true;
            RadPV1good.Checked = true;
            RadPV1good.Location = new Point(5, 107);
            RadPV1good.Name = "RadPV1good";
            RadPV1good.Size = new Size(54, 19);
            RadPV1good.TabIndex = 76;
            RadPV1good.TabStop = true;
            RadPV1good.Text = "Good";
            RadPV1good.UseVisualStyleBackColor = true;
            RadPV1good.CheckedChanged += RadPV1good_CheckedChanged;
            // 
            // label21
            // 
            label21.Location = new Point(5, 19);
            label21.Name = "label21";
            label21.Size = new Size(43, 22);
            label21.TabIndex = 70;
            label21.Text = "Class:";
            label21.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbPV1unit
            // 
            CmbPV1unit.FormattingEnabled = true;
            CmbPV1unit.Location = new Point(53, 78);
            CmbPV1unit.Name = "CmbPV1unit";
            CmbPV1unit.Size = new Size(60, 23);
            CmbPV1unit.TabIndex = 75;
            CmbPV1unit.SelectedIndexChanged += CmbPV1unit_SelectedIndexChanged;
            // 
            // label35
            // 
            label35.Location = new Point(5, 48);
            label35.Name = "label35";
            label35.Size = new Size(43, 22);
            label35.TabIndex = 72;
            label35.Text = "Value:";
            label35.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label36
            // 
            label36.Location = new Point(5, 77);
            label36.Name = "label36";
            label36.Size = new Size(43, 22);
            label36.TabIndex = 74;
            label36.Text = "Unit:";
            label36.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtPV1value
            // 
            TxtPV1value.AcceptsReturn = true;
            TxtPV1value.BackColor = SystemColors.Window;
            TxtPV1value.BorderStyle = BorderStyle.FixedSingle;
            TxtPV1value.Location = new Point(54, 50);
            TxtPV1value.Name = "TxtPV1value";
            TxtPV1value.Size = new Size(59, 23);
            TxtPV1value.TabIndex = 73;
            TxtPV1value.Text = "7.5";
            TxtPV1value.TextChanged += TxtPV1value_TextChanged;
            // 
            // TxtPV1class
            // 
            TxtPV1class.AcceptsReturn = true;
            TxtPV1class.BackColor = SystemColors.Window;
            TxtPV1class.BorderStyle = BorderStyle.FixedSingle;
            TxtPV1class.Location = new Point(54, 21);
            TxtPV1class.Name = "TxtPV1class";
            TxtPV1class.Size = new Size(32, 23);
            TxtPV1class.TabIndex = 71;
            TxtPV1class.Text = "69";
            TxtPV1class.TextChanged += TxtPV1class_TextChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label43);
            groupBox2.Controls.Add(RadCurBad);
            groupBox2.Controls.Add(RadCurGood);
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(label16);
            groupBox2.Controls.Add(TxtCurValue);
            groupBox2.Controls.Add(TxtCurClass);
            groupBox2.Location = new Point(130, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(121, 128);
            groupBox2.TabIndex = 105;
            groupBox2.TabStop = false;
            groupBox2.Text = "Current ( 245)";
            // 
            // label43
            // 
            label43.Location = new Point(54, 79);
            label43.Name = "label43";
            label43.Size = new Size(43, 22);
            label43.TabIndex = 79;
            label43.Text = "mA";
            label43.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RadCurBad
            // 
            RadCurBad.AutoSize = true;
            RadCurBad.Location = new Point(65, 107);
            RadCurBad.Name = "RadCurBad";
            RadCurBad.Size = new Size(45, 19);
            RadCurBad.TabIndex = 77;
            RadCurBad.Text = "Bad";
            RadCurBad.UseVisualStyleBackColor = true;
            RadCurBad.CheckedChanged += RadCurBad_CheckedChanged;
            // 
            // RadCurGood
            // 
            RadCurGood.AutoSize = true;
            RadCurGood.Checked = true;
            RadCurGood.Location = new Point(5, 107);
            RadCurGood.Name = "RadCurGood";
            RadCurGood.Size = new Size(54, 19);
            RadCurGood.TabIndex = 76;
            RadCurGood.TabStop = true;
            RadCurGood.Text = "Good";
            RadCurGood.UseVisualStyleBackColor = true;
            RadCurGood.CheckedChanged += RadCurGood_CheckedChanged;
            // 
            // label15
            // 
            label15.Location = new Point(5, 19);
            label15.Name = "label15";
            label15.Size = new Size(43, 22);
            label15.TabIndex = 70;
            label15.Text = "Class:";
            label15.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            label16.Location = new Point(5, 48);
            label16.Name = "label16";
            label16.Size = new Size(43, 22);
            label16.TabIndex = 72;
            label16.Text = "Value:";
            label16.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtCurValue
            // 
            TxtCurValue.AcceptsReturn = true;
            TxtCurValue.BackColor = SystemColors.Window;
            TxtCurValue.BorderStyle = BorderStyle.FixedSingle;
            TxtCurValue.Location = new Point(54, 50);
            TxtCurValue.Name = "TxtCurValue";
            TxtCurValue.Size = new Size(59, 23);
            TxtCurValue.TabIndex = 73;
            TxtCurValue.Text = "16.0";
            TxtCurValue.TextChanged += TxtCurValue_TextChanged;
            // 
            // TxtCurClass
            // 
            TxtCurClass.AcceptsReturn = true;
            TxtCurClass.BackColor = SystemColors.Control;
            TxtCurClass.BorderStyle = BorderStyle.None;
            TxtCurClass.Location = new Point(54, 23);
            TxtCurClass.Name = "TxtCurClass";
            TxtCurClass.ReadOnly = true;
            TxtCurClass.Size = new Size(32, 16);
            TxtCurClass.TabIndex = 71;
            TxtCurClass.Text = "84";
            TxtCurClass.TextChanged += TxtCurClass_TextChanged;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(label18);
            groupBox9.Controls.Add(RadPercBad);
            groupBox9.Controls.Add(RadPerGood);
            groupBox9.Controls.Add(label45);
            groupBox9.Controls.Add(label44);
            groupBox9.Controls.Add(TxtPerValue);
            groupBox9.Controls.Add(TxtPerClass);
            groupBox9.Location = new Point(3, 3);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(121, 128);
            groupBox9.TabIndex = 104;
            groupBox9.TabStop = false;
            groupBox9.Text = "Percent ( 244)";
            // 
            // label18
            // 
            label18.Location = new Point(54, 79);
            label18.Name = "label18";
            label18.Size = new Size(43, 22);
            label18.TabIndex = 78;
            label18.Text = "%";
            label18.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // RadPercBad
            // 
            RadPercBad.AutoSize = true;
            RadPercBad.Location = new Point(65, 107);
            RadPercBad.Name = "RadPercBad";
            RadPercBad.Size = new Size(45, 19);
            RadPercBad.TabIndex = 77;
            RadPercBad.Text = "Bad";
            RadPercBad.UseVisualStyleBackColor = true;
            RadPercBad.CheckedChanged += RadPercBad_CheckedChanged;
            // 
            // RadPerGood
            // 
            RadPerGood.AutoSize = true;
            RadPerGood.Checked = true;
            RadPerGood.Location = new Point(5, 107);
            RadPerGood.Name = "RadPerGood";
            RadPerGood.Size = new Size(54, 19);
            RadPerGood.TabIndex = 76;
            RadPerGood.TabStop = true;
            RadPerGood.Text = "Good";
            RadPerGood.UseVisualStyleBackColor = true;
            RadPerGood.CheckedChanged += RadPerGood_CheckedChanged;
            // 
            // label45
            // 
            label45.Location = new Point(5, 19);
            label45.Name = "label45";
            label45.Size = new Size(43, 22);
            label45.TabIndex = 70;
            label45.Text = "Class:";
            label45.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label44
            // 
            label44.Location = new Point(5, 48);
            label44.Name = "label44";
            label44.Size = new Size(43, 22);
            label44.TabIndex = 72;
            label44.Text = "Value:";
            label44.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtPerValue
            // 
            TxtPerValue.AcceptsReturn = true;
            TxtPerValue.BackColor = SystemColors.Window;
            TxtPerValue.BorderStyle = BorderStyle.FixedSingle;
            TxtPerValue.Location = new Point(54, 50);
            TxtPerValue.Name = "TxtPerValue";
            TxtPerValue.Size = new Size(59, 23);
            TxtPerValue.TabIndex = 73;
            TxtPerValue.Text = "75.0";
            TxtPerValue.TextChanged += TxtPerValue_TextChanged;
            // 
            // TxtPerClass
            // 
            TxtPerClass.AcceptsReturn = true;
            TxtPerClass.BackColor = SystemColors.Control;
            TxtPerClass.BorderStyle = BorderStyle.None;
            TxtPerClass.Location = new Point(54, 23);
            TxtPerClass.Name = "TxtPerClass";
            TxtPerClass.ReadOnly = true;
            TxtPerClass.Size = new Size(32, 16);
            TxtPerClass.TabIndex = 71;
            TxtPerClass.Text = "0";
            TxtPerClass.TextChanged += TxtPerClass_TextChanged;
            // 
            // tabTransducer
            // 
            tabTransducer.Controls.Add(linkTable26);
            tabTransducer.Controls.Add(TxtChanFlags);
            tabTransducer.Controls.Add(label58);
            tabTransducer.Controls.Add(label57);
            tabTransducer.Controls.Add(CmbTFunction);
            tabTransducer.Controls.Add(label56);
            tabTransducer.Controls.Add(CmbAlmSelect);
            tabTransducer.Controls.Add(label55);
            tabTransducer.Controls.Add(CmbWrProt);
            tabTransducer.Controls.Add(label54);
            tabTransducer.Controls.Add(TxtDamping);
            tabTransducer.Controls.Add(label53);
            tabTransducer.Controls.Add(TxtLowerRange);
            tabTransducer.Controls.Add(TxtUpperRange);
            tabTransducer.Controls.Add(label51);
            tabTransducer.Controls.Add(label52);
            tabTransducer.Controls.Add(label50);
            tabTransducer.Controls.Add(CmbRangeUnit);
            tabTransducer.Controls.Add(TxtLowerLimit);
            tabTransducer.Controls.Add(TxtMinSpan);
            tabTransducer.Controls.Add(TxtTransdSerNum);
            tabTransducer.Controls.Add(label49);
            tabTransducer.Controls.Add(label25);
            tabTransducer.Controls.Add(label24);
            tabTransducer.Controls.Add(label13);
            tabTransducer.Controls.Add(label10);
            tabTransducer.Controls.Add(CmbTransdUnit);
            tabTransducer.Controls.Add(TxtUpperLimit);
            tabTransducer.Location = new Point(4, 27);
            tabTransducer.Name = "tabTransducer";
            tabTransducer.Size = new Size(776, 134);
            tabTransducer.TabIndex = 9;
            tabTransducer.Text = "Transducer";
            tabTransducer.UseVisualStyleBackColor = true;
            // 
            // linkTable26
            // 
            linkTable26.ActiveLinkColor = Color.Red;
            linkTable26.AutoSize = true;
            linkTable26.Location = new Point(657, 74);
            linkTable26.Name = "linkTable26";
            linkTable26.Size = new Size(50, 15);
            linkTable26.TabIndex = 153;
            linkTable26.TabStop = true;
            linkTable26.Text = "Table 26";
            linkTable26.LinkClicked += linkTable26_LinkClicked_1;
            // 
            // TxtChanFlags
            // 
            TxtChanFlags.AcceptsReturn = true;
            TxtChanFlags.BackColor = SystemColors.Window;
            TxtChanFlags.BorderStyle = BorderStyle.FixedSingle;
            TxtChanFlags.Location = new Point(657, 12);
            TxtChanFlags.Name = "TxtChanFlags";
            TxtChanFlags.Size = new Size(80, 23);
            TxtChanFlags.TabIndex = 152;
            TxtChanFlags.Text = "10100101";
            // 
            // label58
            // 
            label58.Location = new Point(611, 10);
            label58.Name = "label58";
            label58.Size = new Size(40, 22);
            label58.TabIndex = 151;
            label58.Text = "Flags:";
            label58.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label57
            // 
            label57.Location = new Point(431, 10);
            label57.Name = "label57";
            label57.Size = new Size(79, 22);
            label57.TabIndex = 150;
            label57.Text = "T-Function:";
            label57.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbTFunction
            // 
            CmbTFunction.FormattingEnabled = true;
            CmbTFunction.Items.AddRange(new object[] { "Lin(0)", "Sqr(1)" });
            CmbTFunction.Location = new Point(516, 11);
            CmbTFunction.Name = "CmbTFunction";
            CmbTFunction.Size = new Size(65, 23);
            CmbTFunction.TabIndex = 149;
            // 
            // label56
            // 
            label56.Location = new Point(431, 70);
            label56.Name = "label56";
            label56.Size = new Size(79, 22);
            label56.TabIndex = 148;
            label56.Text = "Alarm Select:";
            label56.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbAlmSelect
            // 
            CmbAlmSelect.AutoCompleteCustomSource.AddRange(new string[] { "Hi(0)", "Lo(1)", "Last(239)", "No(250)" });
            CmbAlmSelect.FormattingEnabled = true;
            CmbAlmSelect.Items.AddRange(new object[] { "Hi(0)", "Lo(1)", "Last(239)", "None(250)" });
            CmbAlmSelect.Location = new Point(516, 71);
            CmbAlmSelect.Name = "CmbAlmSelect";
            CmbAlmSelect.Size = new Size(65, 23);
            CmbAlmSelect.TabIndex = 147;
            // 
            // label55
            // 
            label55.Location = new Point(261, 70);
            label55.Name = "label55";
            label55.Size = new Size(93, 22);
            label55.TabIndex = 146;
            label55.Text = "Write Protect:";
            label55.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbWrProt
            // 
            CmbWrProt.FormattingEnabled = true;
            CmbWrProt.Items.AddRange(new object[] { "Off(0)", "On(1)", "None/250)" });
            CmbWrProt.Location = new Point(360, 71);
            CmbWrProt.Name = "CmbWrProt";
            CmbWrProt.Size = new Size(65, 23);
            CmbWrProt.TabIndex = 145;
            // 
            // label54
            // 
            label54.Location = new Point(709, 38);
            label54.Name = "label54";
            label54.Size = new Size(20, 22);
            label54.TabIndex = 144;
            label54.Text = "s";
            label54.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtDamping
            // 
            TxtDamping.AcceptsReturn = true;
            TxtDamping.BackColor = SystemColors.Window;
            TxtDamping.BorderStyle = BorderStyle.FixedSingle;
            TxtDamping.Location = new Point(657, 40);
            TxtDamping.Name = "TxtDamping";
            TxtDamping.Size = new Size(46, 23);
            TxtDamping.TabIndex = 143;
            TxtDamping.Text = "1.0";
            // 
            // label53
            // 
            label53.Location = new Point(587, 38);
            label53.Name = "label53";
            label53.Size = new Size(64, 22);
            label53.TabIndex = 142;
            label53.Text = "Damping:";
            label53.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtLowerRange
            // 
            TxtLowerRange.AcceptsReturn = true;
            TxtLowerRange.BackColor = SystemColors.Window;
            TxtLowerRange.BorderStyle = BorderStyle.FixedSingle;
            TxtLowerRange.Location = new Point(516, 40);
            TxtLowerRange.Name = "TxtLowerRange";
            TxtLowerRange.Size = new Size(65, 23);
            TxtLowerRange.TabIndex = 141;
            TxtLowerRange.Text = "0.0";
            // 
            // TxtUpperRange
            // 
            TxtUpperRange.AcceptsReturn = true;
            TxtUpperRange.BackColor = SystemColors.Window;
            TxtUpperRange.BorderStyle = BorderStyle.FixedSingle;
            TxtUpperRange.Location = new Point(360, 40);
            TxtUpperRange.Name = "TxtUpperRange";
            TxtUpperRange.Size = new Size(65, 23);
            TxtUpperRange.TabIndex = 139;
            TxtUpperRange.Text = "10.0";
            // 
            // label51
            // 
            label51.Location = new Point(261, 38);
            label51.Name = "label51";
            label51.Size = new Size(93, 22);
            label51.TabIndex = 138;
            label51.Text = "Upper Range:";
            label51.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label52
            // 
            label52.Location = new Point(431, 38);
            label52.Name = "label52";
            label52.Size = new Size(79, 22);
            label52.TabIndex = 140;
            label52.Text = "Lower Range:";
            label52.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label50
            // 
            label50.Location = new Point(312, 10);
            label50.Name = "label50";
            label50.Size = new Size(43, 22);
            label50.TabIndex = 136;
            label50.Text = "Unit:";
            label50.TextAlign = ContentAlignment.MiddleRight;
            // 
            // CmbRangeUnit
            // 
            CmbRangeUnit.FormattingEnabled = true;
            CmbRangeUnit.Location = new Point(360, 11);
            CmbRangeUnit.Name = "CmbRangeUnit";
            CmbRangeUnit.Size = new Size(65, 23);
            CmbRangeUnit.TabIndex = 137;
            // 
            // TxtLowerLimit
            // 
            TxtLowerLimit.AcceptsReturn = true;
            TxtLowerLimit.BackColor = SystemColors.Window;
            TxtLowerLimit.BorderStyle = BorderStyle.FixedSingle;
            TxtLowerLimit.Location = new Point(107, 70);
            TxtLowerLimit.Name = "TxtLowerLimit";
            TxtLowerLimit.Size = new Size(65, 23);
            TxtLowerLimit.TabIndex = 119;
            TxtLowerLimit.Text = "500.0";
            TxtLowerLimit.TextChanged += TxtLowerLimit_TextChanged;
            // 
            // TxtMinSpan
            // 
            TxtMinSpan.AcceptsReturn = true;
            TxtMinSpan.BackColor = SystemColors.Window;
            TxtMinSpan.BorderStyle = BorderStyle.FixedSingle;
            TxtMinSpan.Location = new Point(107, 99);
            TxtMinSpan.Name = "TxtMinSpan";
            TxtMinSpan.Size = new Size(65, 23);
            TxtMinSpan.TabIndex = 121;
            TxtMinSpan.Text = "100.0";
            TxtMinSpan.TextChanged += TxtMinSpan_TextChanged;
            // 
            // TxtTransdSerNum
            // 
            TxtTransdSerNum.AcceptsReturn = true;
            TxtTransdSerNum.BackColor = SystemColors.Window;
            TxtTransdSerNum.BorderStyle = BorderStyle.FixedSingle;
            TxtTransdSerNum.Location = new Point(107, 12);
            TxtTransdSerNum.Name = "TxtTransdSerNum";
            TxtTransdSerNum.Size = new Size(65, 23);
            TxtTransdSerNum.TabIndex = 113;
            TxtTransdSerNum.Text = "999999";
            TxtTransdSerNum.TextChanged += TxtTransdSerNum_TextChanged;
            // 
            // label49
            // 
            label49.Location = new Point(31, 97);
            label49.Name = "label49";
            label49.Size = new Size(70, 22);
            label49.TabIndex = 120;
            label49.Text = "Min Span:";
            label49.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label25
            // 
            label25.Location = new Point(22, 68);
            label25.Name = "label25";
            label25.Size = new Size(79, 22);
            label25.TabIndex = 118;
            label25.Text = "Lower Limit:";
            label25.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label24
            // 
            label24.Location = new Point(8, 10);
            label24.Name = "label24";
            label24.Size = new Size(93, 22);
            label24.TabIndex = 112;
            label24.Text = "Serial Number:";
            label24.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            label13.Location = new Point(8, 39);
            label13.Name = "label13";
            label13.Size = new Size(93, 22);
            label13.TabIndex = 116;
            label13.Text = "Upper Limit:";
            label13.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            label10.Location = new Point(178, 44);
            label10.Name = "label10";
            label10.Size = new Size(43, 22);
            label10.TabIndex = 114;
            label10.Text = "Unit:";
            label10.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CmbTransdUnit
            // 
            CmbTransdUnit.FormattingEnabled = true;
            CmbTransdUnit.Location = new Point(178, 69);
            CmbTransdUnit.Name = "CmbTransdUnit";
            CmbTransdUnit.Size = new Size(65, 23);
            CmbTransdUnit.TabIndex = 115;
            CmbTransdUnit.SelectedIndexChanged += CmbTransdUnit_SelectedIndexChanged;
            // 
            // TxtUpperLimit
            // 
            TxtUpperLimit.AcceptsReturn = true;
            TxtUpperLimit.BackColor = SystemColors.Window;
            TxtUpperLimit.BorderStyle = BorderStyle.FixedSingle;
            TxtUpperLimit.Location = new Point(107, 41);
            TxtUpperLimit.Name = "TxtUpperLimit";
            TxtUpperLimit.Size = new Size(65, 23);
            TxtUpperLimit.TabIndex = 117;
            TxtUpperLimit.Text = "5000.0";
            TxtUpperLimit.TextChanged += TxtUpperLimit_TextChanged;
            // 
            // tabDevice
            // 
            tabDevice.Controls.Add(ChkSIunitsOnly);
            tabDevice.Controls.Add(TxtCountryLetters);
            tabDevice.Controls.Add(label68);
            tabDevice.Controls.Add(TxtFinAssNum);
            tabDevice.Controls.Add(label59);
            tabDevice.Controls.Add(TxtYear);
            tabDevice.Controls.Add(TxtMonth);
            tabDevice.Controls.Add(TxtDay);
            tabDevice.Controls.Add(TxtDescriptor);
            tabDevice.Controls.Add(TxtMessage);
            tabDevice.Controls.Add(TxtLongTag);
            tabDevice.Controls.Add(TxtShortTag);
            tabDevice.Controls.Add(label34);
            tabDevice.Controls.Add(label33);
            tabDevice.Controls.Add(label32);
            tabDevice.Controls.Add(label31);
            tabDevice.Controls.Add(label9);
            tabDevice.Controls.Add(label8);
            tabDevice.Controls.Add(label7);
            tabDevice.Location = new Point(4, 27);
            tabDevice.Name = "tabDevice";
            tabDevice.Size = new Size(776, 134);
            tabDevice.TabIndex = 7;
            tabDevice.Text = "Device";
            tabDevice.UseVisualStyleBackColor = true;
            // 
            // ChkSIunitsOnly
            // 
            ChkSIunitsOnly.AutoSize = true;
            ChkSIunitsOnly.Location = new Point(426, 70);
            ChkSIunitsOnly.Name = "ChkSIunitsOnly";
            ChkSIunitsOnly.Size = new Size(91, 19);
            ChkSIunitsOnly.TabIndex = 117;
            ChkSIunitsOnly.Text = "SI Units only";
            ChkSIunitsOnly.UseVisualStyleBackColor = true;
            ChkSIunitsOnly.CheckedChanged += ChkSIunitsOnly_CheckedChanged;
            // 
            // TxtCountryLetters
            // 
            TxtCountryLetters.AcceptsReturn = true;
            TxtCountryLetters.BackColor = SystemColors.Window;
            TxtCountryLetters.BorderStyle = BorderStyle.FixedSingle;
            TxtCountryLetters.Location = new Point(389, 69);
            TxtCountryLetters.Name = "TxtCountryLetters";
            TxtCountryLetters.Size = new Size(31, 23);
            TxtCountryLetters.TabIndex = 116;
            TxtCountryLetters.Text = "de";
            TxtCountryLetters.TextChanged += TxtCountryLetters_TextChanged;
            // 
            // label68
            // 
            label68.Location = new Point(279, 67);
            label68.Name = "label68";
            label68.Size = new Size(104, 22);
            label68.TabIndex = 115;
            label68.Text = "Country Code:";
            label68.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtFinAssNum
            // 
            TxtFinAssNum.AcceptsReturn = true;
            TxtFinAssNum.BackColor = SystemColors.Window;
            TxtFinAssNum.BorderStyle = BorderStyle.FixedSingle;
            TxtFinAssNum.Location = new Point(389, 41);
            TxtFinAssNum.Name = "TxtFinAssNum";
            TxtFinAssNum.Size = new Size(65, 23);
            TxtFinAssNum.TabIndex = 114;
            TxtFinAssNum.Text = "777777";
            TxtFinAssNum.TextChanged += TxtFinAssNum_TextChanged;
            // 
            // label59
            // 
            label59.Location = new Point(239, 39);
            label59.Name = "label59";
            label59.Size = new Size(144, 22);
            label59.TabIndex = 102;
            label59.Text = "Final Assembly Number:";
            label59.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtYear
            // 
            TxtYear.AcceptsReturn = true;
            TxtYear.BackColor = SystemColors.Window;
            TxtYear.BorderStyle = BorderStyle.FixedSingle;
            TxtYear.Location = new Point(202, 101);
            TxtYear.MaxLength = 32;
            TxtYear.Name = "TxtYear";
            TxtYear.Size = new Size(34, 23);
            TxtYear.TabIndex = 101;
            TxtYear.Text = "2024";
            TxtYear.TextChanged += TxtYear_TextChanged;
            // 
            // TxtMonth
            // 
            TxtMonth.AcceptsReturn = true;
            TxtMonth.BackColor = SystemColors.Window;
            TxtMonth.BorderStyle = BorderStyle.FixedSingle;
            TxtMonth.Location = new Point(131, 101);
            TxtMonth.MaxLength = 32;
            TxtMonth.Name = "TxtMonth";
            TxtMonth.Size = new Size(22, 23);
            TxtMonth.TabIndex = 99;
            TxtMonth.Text = "7";
            TxtMonth.TextChanged += TxtMonth_TextChanged;
            // 
            // TxtDay
            // 
            TxtDay.AcceptsReturn = true;
            TxtDay.BackColor = SystemColors.Window;
            TxtDay.BorderStyle = BorderStyle.FixedSingle;
            TxtDay.Location = new Point(47, 101);
            TxtDay.MaxLength = 32;
            TxtDay.Name = "TxtDay";
            TxtDay.Size = new Size(22, 23);
            TxtDay.TabIndex = 97;
            TxtDay.Text = "1";
            TxtDay.TextChanged += TxtDay_TextChanged;
            // 
            // TxtDescriptor
            // 
            TxtDescriptor.AcceptsReturn = true;
            TxtDescriptor.BackColor = SystemColors.Window;
            TxtDescriptor.BorderStyle = BorderStyle.FixedSingle;
            TxtDescriptor.CharacterCasing = CharacterCasing.Upper;
            TxtDescriptor.Location = new Point(87, 69);
            TxtDescriptor.MaxLength = 16;
            TxtDescriptor.Name = "TxtDescriptor";
            TxtDescriptor.Size = new Size(186, 23);
            TxtDescriptor.TabIndex = 94;
            TxtDescriptor.Text = "16 CH DESCRIPTOR";
            TxtDescriptor.TextChanged += TxtDescriptor_TextChanged;
            // 
            // TxtMessage
            // 
            TxtMessage.AcceptsReturn = true;
            TxtMessage.BackColor = SystemColors.Window;
            TxtMessage.BorderStyle = BorderStyle.FixedSingle;
            TxtMessage.CharacterCasing = CharacterCasing.Upper;
            TxtMessage.Location = new Point(389, 13);
            TxtMessage.MaxLength = 32;
            TxtMessage.Name = "TxtMessage";
            TxtMessage.Size = new Size(331, 23);
            TxtMessage.TabIndex = 69;
            TxtMessage.Text = "32 CAPITAL LETTER CHARACTERS";
            TxtMessage.TextChanged += TxtMessage_TextChanged;
            // 
            // TxtLongTag
            // 
            TxtLongTag.AcceptsReturn = true;
            TxtLongTag.BackColor = SystemColors.Window;
            TxtLongTag.BorderStyle = BorderStyle.FixedSingle;
            TxtLongTag.Location = new Point(87, 13);
            TxtLongTag.MaxLength = 32;
            TxtLongTag.Name = "TxtLongTag";
            TxtLongTag.Size = new Size(186, 23);
            TxtLongTag.TabIndex = 67;
            TxtLongTag.Text = "32 Characters Iso Latin-1";
            TxtLongTag.TextChanged += TxtLongTag_TextChanged;
            // 
            // TxtShortTag
            // 
            TxtShortTag.AcceptsReturn = true;
            TxtShortTag.BackColor = SystemColors.Window;
            TxtShortTag.BorderStyle = BorderStyle.FixedSingle;
            TxtShortTag.CharacterCasing = CharacterCasing.Upper;
            TxtShortTag.Location = new Point(87, 41);
            TxtShortTag.MaxLength = 8;
            TxtShortTag.Name = "TxtShortTag";
            TxtShortTag.Size = new Size(106, 23);
            TxtShortTag.TabIndex = 65;
            TxtShortTag.Text = "8CHR TAG";
            TxtShortTag.TextChanged += TxtShortTag_TextChanged;
            // 
            // label34
            // 
            label34.Location = new Point(159, 99);
            label34.Name = "label34";
            label34.Size = new Size(37, 22);
            label34.TabIndex = 100;
            label34.Text = "Year:";
            label34.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label33
            // 
            label33.Location = new Point(75, 99);
            label33.Name = "label33";
            label33.Size = new Size(50, 22);
            label33.TabIndex = 98;
            label33.Text = "Month:";
            label33.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label32
            // 
            label32.Location = new Point(3, 99);
            label32.Name = "label32";
            label32.Size = new Size(38, 22);
            label32.TabIndex = 96;
            label32.Text = "Day:";
            label32.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label31
            // 
            label31.Location = new Point(3, 67);
            label31.Name = "label31";
            label31.Size = new Size(78, 22);
            label31.TabIndex = 93;
            label31.Text = "Description:";
            label31.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.Location = new Point(280, 11);
            label9.Name = "label9";
            label9.Size = new Size(103, 22);
            label9.TabIndex = 68;
            label9.Text = "Message:";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.Location = new Point(3, 11);
            label8.Name = "label8";
            label8.Size = new Size(78, 22);
            label8.TabIndex = 66;
            label8.Text = "Long Tag:";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.Location = new Point(3, 39);
            label7.Name = "label7";
            label7.Size = new Size(78, 22);
            label7.TabIndex = 64;
            label7.Text = "Short Tag:";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tabIdentifier
            // 
            tabIdentifier.Controls.Add(groupBox1);
            tabIdentifier.Controls.Add(TxtProfileTable57);
            tabIdentifier.Controls.Add(TxtDistributorTable8);
            tabIdentifier.Controls.Add(TxtManufacturerTable8);
            tabIdentifier.Controls.Add(TxtExtDevStatus);
            tabIdentifier.Controls.Add(TxtConfigChangedCounter);
            tabIdentifier.Controls.Add(TxtLastDevVarCode);
            tabIdentifier.Controls.Add(TxtDevUniqueID);
            tabIdentifier.Controls.Add(TxtFlagsTable11);
            tabIdentifier.Controls.Add(TxtSignalTable10);
            tabIdentifier.Controls.Add(TxtHwRev);
            tabIdentifier.Controls.Add(TxtSwRev);
            tabIdentifier.Controls.Add(TxtDevRev);
            tabIdentifier.Controls.Add(TxtDevTypeTable1);
            tabIdentifier.Controls.Add(label30);
            tabIdentifier.Controls.Add(label29);
            tabIdentifier.Controls.Add(label28);
            tabIdentifier.Controls.Add(label27);
            tabIdentifier.Controls.Add(label26);
            tabIdentifier.Controls.Add(label23);
            tabIdentifier.Controls.Add(label22);
            tabIdentifier.Controls.Add(label20);
            tabIdentifier.Controls.Add(label19);
            tabIdentifier.Controls.Add(label17);
            tabIdentifier.Controls.Add(label14);
            tabIdentifier.Controls.Add(label12);
            tabIdentifier.Controls.Add(label6);
            tabIdentifier.Location = new Point(4, 27);
            tabIdentifier.Name = "tabIdentifier";
            tabIdentifier.Size = new Size(776, 134);
            tabIdentifier.TabIndex = 2;
            tabIdentifier.Text = "Identifier";
            tabIdentifier.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(LinkTable57);
            groupBox1.Controls.Add(LinkTable17);
            groupBox1.Controls.Add(LinkTable10);
            groupBox1.Controls.Add(LinkTable8);
            groupBox1.Controls.Add(LinkTable1);
            groupBox1.Controls.Add(LinkCmd0);
            groupBox1.Location = new Point(629, 42);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(144, 87);
            groupBox1.TabIndex = 93;
            groupBox1.TabStop = false;
            groupBox1.Text = "Details";
            // 
            // LinkTable57
            // 
            LinkTable57.ActiveLinkColor = Color.Red;
            LinkTable57.AutoSize = true;
            LinkTable57.Location = new Point(65, 64);
            LinkTable57.Name = "LinkTable57";
            LinkTable57.Size = new Size(50, 15);
            LinkTable57.TabIndex = 87;
            LinkTable57.TabStop = true;
            LinkTable57.Text = "Table 57";
            LinkTable57.LinkClicked += LinkTable57_LinkClicked;
            // 
            // LinkTable17
            // 
            LinkTable17.ActiveLinkColor = Color.Red;
            LinkTable17.AutoSize = true;
            LinkTable17.Location = new Point(65, 42);
            LinkTable17.Name = "LinkTable17";
            LinkTable17.Size = new Size(50, 15);
            LinkTable17.TabIndex = 86;
            LinkTable17.TabStop = true;
            LinkTable17.Text = "Table 17";
            LinkTable17.LinkClicked += LinkTable17_LinkClicked;
            // 
            // LinkTable10
            // 
            LinkTable10.ActiveLinkColor = Color.Red;
            LinkTable10.AutoSize = true;
            LinkTable10.Location = new Point(6, 64);
            LinkTable10.Name = "LinkTable10";
            LinkTable10.Size = new Size(50, 15);
            LinkTable10.TabIndex = 85;
            LinkTable10.TabStop = true;
            LinkTable10.Text = "Table 10";
            LinkTable10.LinkClicked += LinkTable10_LinkClicked;
            // 
            // LinkTable8
            // 
            LinkTable8.ActiveLinkColor = Color.Red;
            LinkTable8.AutoSize = true;
            LinkTable8.Location = new Point(6, 42);
            LinkTable8.Name = "LinkTable8";
            LinkTable8.Size = new Size(44, 15);
            LinkTable8.TabIndex = 84;
            LinkTable8.TabStop = true;
            LinkTable8.Text = "Table 8";
            LinkTable8.LinkClicked += LinkTable8_LinkClicked;
            // 
            // LinkTable1
            // 
            LinkTable1.ActiveLinkColor = Color.Red;
            LinkTable1.AutoSize = true;
            LinkTable1.Location = new Point(65, 19);
            LinkTable1.Name = "LinkTable1";
            LinkTable1.Size = new Size(44, 15);
            LinkTable1.TabIndex = 83;
            LinkTable1.TabStop = true;
            LinkTable1.Text = "Table 1";
            LinkTable1.LinkClicked += LinkTable1_LinkClicked;
            // 
            // LinkCmd0
            // 
            LinkCmd0.ActiveLinkColor = Color.Red;
            LinkCmd0.AutoSize = true;
            LinkCmd0.Location = new Point(6, 19);
            LinkCmd0.Name = "LinkCmd0";
            LinkCmd0.Size = new Size(42, 15);
            LinkCmd0.TabIndex = 82;
            LinkCmd0.TabStop = true;
            LinkCmd0.Text = "Cmd 0";
            LinkCmd0.LinkClicked += LinkCmd0_LinkClicked;
            // 
            // TxtProfileTable57
            // 
            TxtProfileTable57.AcceptsReturn = true;
            TxtProfileTable57.BackColor = SystemColors.Window;
            TxtProfileTable57.BorderStyle = BorderStyle.FixedSingle;
            TxtProfileTable57.Location = new Point(734, 13);
            TxtProfileTable57.Name = "TxtProfileTable57";
            TxtProfileTable57.Size = new Size(34, 23);
            TxtProfileTable57.TabIndex = 92;
            TxtProfileTable57.Text = "000";
            TxtProfileTable57.TextChanged += TxtProfileTable57_TextChanged;
            // 
            // TxtDistributorTable8
            // 
            TxtDistributorTable8.AcceptsReturn = true;
            TxtDistributorTable8.BackColor = SystemColors.Window;
            TxtDistributorTable8.BorderStyle = BorderStyle.FixedSingle;
            TxtDistributorTable8.Location = new Point(578, 98);
            TxtDistributorTable8.Name = "TxtDistributorTable8";
            TxtDistributorTable8.Size = new Size(45, 23);
            TxtDistributorTable8.TabIndex = 90;
            TxtDistributorTable8.Text = "0x00E1";
            TxtDistributorTable8.TextChanged += TxtDistributorTable8_TextChanged;
            // 
            // TxtManufacturerTable8
            // 
            TxtManufacturerTable8.AcceptsReturn = true;
            TxtManufacturerTable8.BackColor = SystemColors.Window;
            TxtManufacturerTable8.BorderStyle = BorderStyle.FixedSingle;
            TxtManufacturerTable8.Location = new Point(578, 69);
            TxtManufacturerTable8.Name = "TxtManufacturerTable8";
            TxtManufacturerTable8.Size = new Size(45, 23);
            TxtManufacturerTable8.TabIndex = 88;
            TxtManufacturerTable8.Text = "0x00E0";
            TxtManufacturerTable8.TextChanged += TxtManufacturerTable8_TextChanged;
            // 
            // TxtExtDevStatus
            // 
            TxtExtDevStatus.AcceptsReturn = true;
            TxtExtDevStatus.BackColor = SystemColors.Window;
            TxtExtDevStatus.BorderStyle = BorderStyle.FixedSingle;
            TxtExtDevStatus.Location = new Point(578, 40);
            TxtExtDevStatus.Name = "TxtExtDevStatus";
            TxtExtDevStatus.Size = new Size(45, 23);
            TxtExtDevStatus.TabIndex = 86;
            TxtExtDevStatus.Text = "000";
            TxtExtDevStatus.TextChanged += TxtExtDevStatus_TextChanged;
            // 
            // TxtConfigChangedCounter
            // 
            TxtConfigChangedCounter.AcceptsReturn = true;
            TxtConfigChangedCounter.BackColor = SystemColors.Window;
            TxtConfigChangedCounter.BorderStyle = BorderStyle.FixedSingle;
            TxtConfigChangedCounter.Location = new Point(578, 11);
            TxtConfigChangedCounter.Name = "TxtConfigChangedCounter";
            TxtConfigChangedCounter.Size = new Size(45, 23);
            TxtConfigChangedCounter.TabIndex = 84;
            TxtConfigChangedCounter.Text = "65535";
            TxtConfigChangedCounter.TextChanged += TxtConfigChangedCounter_TextChanged;
            // 
            // TxtLastDevVarCode
            // 
            TxtLastDevVarCode.AcceptsReturn = true;
            TxtLastDevVarCode.BackColor = SystemColors.Window;
            TxtLastDevVarCode.BorderStyle = BorderStyle.FixedSingle;
            TxtLastDevVarCode.Location = new Point(355, 98);
            TxtLastDevVarCode.Name = "TxtLastDevVarCode";
            TxtLastDevVarCode.Size = new Size(45, 23);
            TxtLastDevVarCode.TabIndex = 81;
            TxtLastDevVarCode.Text = "0";
            TxtLastDevVarCode.TextChanged += TxtLastDevVarCode_TextChanged;
            // 
            // TxtDevUniqueID
            // 
            TxtDevUniqueID.AcceptsReturn = true;
            TxtDevUniqueID.BackColor = SystemColors.Window;
            TxtDevUniqueID.BorderStyle = BorderStyle.FixedSingle;
            TxtDevUniqueID.Location = new Point(355, 69);
            TxtDevUniqueID.Name = "TxtDevUniqueID";
            TxtDevUniqueID.Size = new Size(65, 23);
            TxtDevUniqueID.TabIndex = 79;
            TxtDevUniqueID.Text = "0x030405";
            TxtDevUniqueID.TextChanged += TxtDevUniqueID_TextChanged;
            // 
            // TxtFlagsTable11
            // 
            TxtFlagsTable11.AcceptsReturn = true;
            TxtFlagsTable11.BackColor = SystemColors.Window;
            TxtFlagsTable11.BorderStyle = BorderStyle.FixedSingle;
            TxtFlagsTable11.Location = new Point(355, 40);
            TxtFlagsTable11.Name = "TxtFlagsTable11";
            TxtFlagsTable11.Size = new Size(45, 23);
            TxtFlagsTable11.TabIndex = 77;
            TxtFlagsTable11.Text = "0";
            TxtFlagsTable11.TextChanged += TxtFlagsTable11_TextChanged;
            // 
            // TxtSignalTable10
            // 
            TxtSignalTable10.AcceptsReturn = true;
            TxtSignalTable10.BackColor = SystemColors.Window;
            TxtSignalTable10.BorderStyle = BorderStyle.FixedSingle;
            TxtSignalTable10.Location = new Point(355, 11);
            TxtSignalTable10.Name = "TxtSignalTable10";
            TxtSignalTable10.Size = new Size(45, 23);
            TxtSignalTable10.TabIndex = 75;
            TxtSignalTable10.Text = "0";
            TxtSignalTable10.TextChanged += TxtSignalTable10_TextChanged;
            // 
            // TxtHwRev
            // 
            TxtHwRev.AcceptsReturn = true;
            TxtHwRev.BackColor = SystemColors.Window;
            TxtHwRev.BorderStyle = BorderStyle.FixedSingle;
            TxtHwRev.Location = new Point(156, 98);
            TxtHwRev.Name = "TxtHwRev";
            TxtHwRev.Size = new Size(45, 23);
            TxtHwRev.TabIndex = 73;
            TxtHwRev.Text = "3";
            TxtHwRev.TextChanged += TxtHwRev_TextChanged;
            // 
            // TxtSwRev
            // 
            TxtSwRev.AcceptsReturn = true;
            TxtSwRev.BackColor = SystemColors.Window;
            TxtSwRev.BorderStyle = BorderStyle.FixedSingle;
            TxtSwRev.Location = new Point(156, 69);
            TxtSwRev.Name = "TxtSwRev";
            TxtSwRev.Size = new Size(45, 23);
            TxtSwRev.TabIndex = 71;
            TxtSwRev.Text = "2";
            TxtSwRev.TextChanged += TxtSwRev_TextChanged;
            // 
            // TxtDevRev
            // 
            TxtDevRev.AcceptsReturn = true;
            TxtDevRev.BackColor = SystemColors.Window;
            TxtDevRev.BorderStyle = BorderStyle.FixedSingle;
            TxtDevRev.Location = new Point(156, 40);
            TxtDevRev.Name = "TxtDevRev";
            TxtDevRev.Size = new Size(45, 23);
            TxtDevRev.TabIndex = 69;
            TxtDevRev.Text = "1";
            TxtDevRev.TextChanged += TxtDevRev_TextChanged;
            // 
            // TxtDevTypeTable1
            // 
            TxtDevTypeTable1.AcceptsReturn = true;
            TxtDevTypeTable1.BackColor = SystemColors.Window;
            TxtDevTypeTable1.BorderStyle = BorderStyle.FixedSingle;
            TxtDevTypeTable1.Location = new Point(156, 11);
            TxtDevTypeTable1.Name = "TxtDevTypeTable1";
            TxtDevTypeTable1.Size = new Size(57, 23);
            TxtDevTypeTable1.TabIndex = 67;
            TxtDevTypeTable1.Text = "0x00F9";
            TxtDevTypeTable1.TextChanged += TxtDevTypeTable1_TextChanged;
            // 
            // label30
            // 
            label30.Location = new Point(626, 11);
            label30.Name = "label30";
            label30.Size = new Size(102, 22);
            label30.TabIndex = 91;
            label30.Text = "Profile (Table 57):";
            label30.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label29
            // 
            label29.Location = new Point(432, 96);
            label29.Name = "label29";
            label29.Size = new Size(140, 22);
            label29.TabIndex = 89;
            label29.Text = "Distributor (Table8):";
            label29.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label28
            // 
            label28.Location = new Point(432, 67);
            label28.Name = "label28";
            label28.Size = new Size(140, 22);
            label28.TabIndex = 87;
            label28.Text = "Manufacturer (Table8):";
            label28.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label27
            // 
            label27.Location = new Point(406, 38);
            label27.Name = "label27";
            label27.Size = new Size(166, 22);
            label27.TabIndex = 85;
            label27.Text = "Extended Status (Table 17):";
            label27.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            label26.Location = new Point(432, 9);
            label26.Name = "label26";
            label26.Size = new Size(140, 22);
            label26.TabIndex = 83;
            label26.Text = "Config Change Counter:";
            label26.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            label23.Location = new Point(218, 96);
            label23.Name = "label23";
            label23.Size = new Size(131, 22);
            label23.TabIndex = 80;
            label23.Text = "Last Dev Variable Code:";
            label23.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label22
            // 
            label22.Location = new Point(218, 67);
            label22.Name = "label22";
            label22.Size = new Size(131, 22);
            label22.TabIndex = 78;
            label22.Text = "Device Unique ID (hex):";
            label22.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            label20.Location = new Point(231, 38);
            label20.Name = "label20";
            label20.Size = new Size(118, 22);
            label20.TabIndex = 76;
            label20.Text = "Flags (Table 11):";
            label20.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label19
            // 
            label19.Location = new Point(231, 9);
            label19.Name = "label19";
            label19.Size = new Size(118, 22);
            label19.TabIndex = 74;
            label19.Text = "Signaling (Table 10):";
            label19.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label17
            // 
            label17.Location = new Point(8, 96);
            label17.Name = "label17";
            label17.Size = new Size(142, 22);
            label17.TabIndex = 72;
            label17.Text = "Hardware Revision Level:";
            label17.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            label14.Location = new Point(8, 67);
            label14.Name = "label14";
            label14.Size = new Size(142, 22);
            label14.TabIndex = 70;
            label14.Text = "Software Revision Level:";
            label14.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            label12.Location = new Point(8, 38);
            label12.Name = "label12";
            label12.Size = new Size(142, 22);
            label12.TabIndex = 68;
            label12.Text = "Device Revision Level:";
            label12.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            label6.Location = new Point(8, 9);
            label6.Name = "label6";
            label6.Size = new Size(142, 22);
            label6.TabIndex = 66;
            label6.Text = "Device Type (Table 1):";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tabStart
            // 
            tabStart.Controls.Add(ChkSimWriteProtect);
            tabStart.Controls.Add(ChkViewShortBursts);
            tabStart.Controls.Add(ChkBurstMode);
            tabStart.Controls.Add(label5);
            tabStart.Controls.Add(groupBox5);
            tabStart.Controls.Add(GrpHartFraming);
            tabStart.Controls.Add(groupBox4);
            tabStart.Location = new Point(4, 27);
            tabStart.Name = "tabStart";
            tabStart.Padding = new Padding(3);
            tabStart.Size = new Size(776, 134);
            tabStart.TabIndex = 1;
            tabStart.Text = "Start";
            tabStart.UseVisualStyleBackColor = true;
            // 
            // ChkSimWriteProtect
            // 
            ChkSimWriteProtect.AutoSize = true;
            ChkSimWriteProtect.Location = new Point(374, 109);
            ChkSimWriteProtect.Name = "ChkSimWriteProtect";
            ChkSimWriteProtect.Size = new Size(118, 19);
            ChkSimWriteProtect.TabIndex = 52;
            ChkSimWriteProtect.Text = "Sim Write Protect";
            ChkSimWriteProtect.UseVisualStyleBackColor = true;
            ChkSimWriteProtect.CheckedChanged += ChkSimWriteProtect_CheckedChanged;
            // 
            // ChkViewShortBursts
            // 
            ChkViewShortBursts.AutoSize = true;
            ChkViewShortBursts.Location = new Point(237, 109);
            ChkViewShortBursts.Name = "ChkViewShortBursts";
            ChkViewShortBursts.Size = new Size(117, 19);
            ChkViewShortBursts.TabIndex = 51;
            ChkViewShortBursts.Text = "View Short Bursts";
            ChkViewShortBursts.UseVisualStyleBackColor = true;
            ChkViewShortBursts.CheckedChanged += ChkViewShortBursts_CheckedChanged;
            // 
            // ChkBurstMode
            // 
            ChkBurstMode.AutoSize = true;
            ChkBurstMode.Location = new Point(237, 89);
            ChkBurstMode.Name = "ChkBurstMode";
            ChkBurstMode.Size = new Size(106, 19);
            ChkBurstMode.TabIndex = 49;
            ChkBurstMode.Text = "Burst Mode On";
            ChkBurstMode.UseVisualStyleBackColor = true;
            ChkBurstMode.CheckedChanged += ChkBurstMode_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
            label5.ForeColor = SystemColors.ControlDarkDark;
            label5.Location = new Point(579, 92);
            label5.Name = "label5";
            label5.Size = new Size(197, 37);
            label5.TabIndex = 48;
            label5.Text = "Hart Slave 8.0";
            label5.TextAlign = ContentAlignment.TopRight;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(ChkViewStatusBinary);
            groupBox5.Controls.Add(ChkViewDecodedData);
            groupBox5.Controls.Add(ChkViewFrameNumbers);
            groupBox5.Controls.Add(ChkViewTiming);
            groupBox5.Controls.Add(ChkViewAddress);
            groupBox5.Controls.Add(ChkViewPreambles);
            groupBox5.Location = new Point(442, 6);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(334, 79);
            groupBox5.TabIndex = 47;
            groupBox5.TabStop = false;
            groupBox5.Text = "Hart Communication View";
            // 
            // GrpHartFraming
            // 
            GrpHartFraming.Controls.Add(CmbRspPreambles);
            GrpHartFraming.Controls.Add(label11);
            GrpHartFraming.Controls.Add(CmbReqPreambles);
            GrpHartFraming.Controls.Add(label2);
            GrpHartFraming.Location = new Point(237, 3);
            GrpHartFraming.Name = "GrpHartFraming";
            GrpHartFraming.Size = new Size(199, 82);
            GrpHartFraming.TabIndex = 39;
            GrpHartFraming.TabStop = false;
            GrpHartFraming.Text = "Hart Framing";
            // 
            // label11
            // 
            label11.Location = new Point(5, 48);
            label11.Name = "label11";
            label11.Size = new Size(126, 25);
            label11.TabIndex = 33;
            label11.Text = "Response Preambles:";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Location = new Point(6, 19);
            label2.Name = "label2";
            label2.Size = new Size(125, 25);
            label2.TabIndex = 31;
            label2.Text = "Request Preambles:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(CmbBaudRate);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(label1);
            groupBox4.Controls.Add(label3);
            groupBox4.Controls.Add(CmbComPort);
            groupBox4.Controls.Add(CmbPollAddress);
            groupBox4.Location = new Point(6, 4);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(225, 124);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Interface Settings";
            // 
            // label4
            // 
            label4.Location = new Point(28, 90);
            label4.Name = "label4";
            label4.Size = new Size(84, 24);
            label4.TabIndex = 33;
            label4.Text = "Baudrate:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Location = new Point(28, 23);
            label1.Name = "label1";
            label1.Size = new Size(84, 25);
            label1.TabIndex = 30;
            label1.Text = "Com Port:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new Point(28, 57);
            label3.Name = "label3";
            label3.Size = new Size(84, 24);
            label3.TabIndex = 8;
            label3.Text = "Poll Address:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tabMain
            // 
            tabMain.Appearance = TabAppearance.FlatButtons;
            tabMain.Controls.Add(tabStart);
            tabMain.Controls.Add(tabHartIp);
            tabMain.Controls.Add(tabIdentifier);
            tabMain.Controls.Add(tabTransducer);
            tabMain.Controls.Add(tabDevice);
            tabMain.Controls.Add(tabDevVars);
            tabMain.Controls.Add(tabStatus);
            tabMain.Controls.Add(tabOptions);
            tabMain.Location = new Point(0, 37);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new Size(784, 165);
            tabMain.TabIndex = 2;
            // 
            // tabHartIp
            // 
            tabHartIp.Controls.Add(ChkHartIpEnabled);
            tabHartIp.Controls.Add(ButSetDefault);
            tabHartIp.Controls.Add(TxtLastError);
            tabHartIp.Controls.Add(label72);
            tabHartIp.Controls.Add(label71);
            tabHartIp.Controls.Add(TxtConnectionStatus);
            tabHartIp.Controls.Add(label61);
            tabHartIp.Controls.Add(ChkHartIpUseAddress);
            tabHartIp.Controls.Add(rtxIPFrames);
            tabHartIp.Controls.Add(TxtHartIpPort);
            tabHartIp.Controls.Add(label62);
            tabHartIp.Controls.Add(TxtHartIpAddress);
            tabHartIp.Controls.Add(label69);
            tabHartIp.Controls.Add(TxtHartIpHostName);
            tabHartIp.Controls.Add(label70);
            tabHartIp.Location = new Point(4, 27);
            tabHartIp.Name = "tabHartIp";
            tabHartIp.Size = new Size(776, 134);
            tabHartIp.TabIndex = 11;
            tabHartIp.Text = "Hart Ip";
            tabHartIp.UseVisualStyleBackColor = true;
            // 
            // ChkHartIpEnabled
            // 
            ChkHartIpEnabled.Checked = true;
            ChkHartIpEnabled.CheckState = CheckState.Checked;
            ChkHartIpEnabled.Location = new Point(8, 39);
            ChkHartIpEnabled.Name = "ChkHartIpEnabled";
            ChkHartIpEnabled.Size = new Size(121, 23);
            ChkHartIpEnabled.TabIndex = 89;
            ChkHartIpEnabled.Text = "Hart Ip Enabled";
            ChkHartIpEnabled.UseVisualStyleBackColor = true;
            ChkHartIpEnabled.CheckedChanged += ChkHartIpEnabled_CheckedChanged;
            // 
            // ButSetDefault
            // 
            ButSetDefault.FlatStyle = FlatStyle.Popup;
            ButSetDefault.Location = new Point(8, 91);
            ButSetDefault.Name = "ButSetDefault";
            ButSetDefault.Size = new Size(75, 26);
            ButSetDefault.TabIndex = 88;
            ButSetDefault.Text = "Set Default";
            ButSetDefault.TextAlign = ContentAlignment.MiddleLeft;
            ButSetDefault.UseVisualStyleBackColor = true;
            ButSetDefault.Click += ButSetDefault_Click;
            // 
            // TxtLastError
            // 
            TxtLastError.AcceptsReturn = true;
            TxtLastError.BackColor = SystemColors.Info;
            TxtLastError.BorderStyle = BorderStyle.FixedSingle;
            TxtLastError.Location = new Point(645, 12);
            TxtLastError.Name = "TxtLastError";
            TxtLastError.Size = new Size(123, 23);
            TxtLastError.TabIndex = 87;
            TxtLastError.Text = "No Error";
            // 
            // label72
            // 
            label72.Location = new Point(574, 10);
            label72.Name = "label72";
            label72.Size = new Size(65, 22);
            label72.TabIndex = 86;
            label72.Text = "Last Error:";
            label72.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label71
            // 
            label71.Location = new Point(261, 38);
            label71.Name = "label71";
            label71.Size = new Size(151, 22);
            label71.TabIndex = 85;
            label71.Text = "Last Hart IP PDU sent:";
            label71.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TxtConnectionStatus
            // 
            TxtConnectionStatus.AcceptsReturn = true;
            TxtConnectionStatus.BackColor = SystemColors.Info;
            TxtConnectionStatus.BorderStyle = BorderStyle.FixedSingle;
            TxtConnectionStatus.Location = new Point(349, 12);
            TxtConnectionStatus.Name = "TxtConnectionStatus";
            TxtConnectionStatus.Size = new Size(201, 23);
            TxtConnectionStatus.TabIndex = 84;
            TxtConnectionStatus.Text = "Not Connected";
            // 
            // label61
            // 
            label61.Location = new Point(261, 10);
            label61.Name = "label61";
            label61.Size = new Size(82, 22);
            label61.TabIndex = 83;
            label61.Text = "Hart IP Status:";
            label61.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // ChkHartIpUseAddress
            // 
            ChkHartIpUseAddress.AutoSize = true;
            ChkHartIpUseAddress.Checked = true;
            ChkHartIpUseAddress.CheckState = CheckState.Checked;
            ChkHartIpUseAddress.Location = new Point(135, 41);
            ChkHartIpUseAddress.Name = "ChkHartIpUseAddress";
            ChkHartIpUseAddress.Size = new Size(90, 19);
            ChkHartIpUseAddress.TabIndex = 82;
            ChkHartIpUseAddress.Text = "Use Address";
            ChkHartIpUseAddress.UseVisualStyleBackColor = true;
            ChkHartIpUseAddress.CheckedChanged += ChkHartIpUseAddress_CheckedChanged;
            // 
            // rtxIPFrames
            // 
            rtxIPFrames.BackColor = SystemColors.Info;
            rtxIPFrames.Location = new Point(261, 66);
            rtxIPFrames.Name = "rtxIPFrames";
            rtxIPFrames.ReadOnly = true;
            rtxIPFrames.Size = new Size(507, 65);
            rtxIPFrames.TabIndex = 80;
            rtxIPFrames.Text = "";
            // 
            // TxtHartIpPort
            // 
            TxtHartIpPort.AcceptsReturn = true;
            TxtHartIpPort.BackColor = SystemColors.Window;
            TxtHartIpPort.BorderStyle = BorderStyle.FixedSingle;
            TxtHartIpPort.Location = new Point(135, 95);
            TxtHartIpPort.Name = "TxtHartIpPort";
            TxtHartIpPort.Size = new Size(106, 23);
            TxtHartIpPort.TabIndex = 79;
            TxtHartIpPort.Text = "5094";
            TxtHartIpPort.TextChanged += TxtHartIpPort_TextChanged;
            // 
            // label62
            // 
            label62.Location = new Point(90, 93);
            label62.Name = "label62";
            label62.Size = new Size(39, 22);
            label62.TabIndex = 78;
            label62.Text = "Port:";
            label62.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtHartIpAddress
            // 
            TxtHartIpAddress.AcceptsReturn = true;
            TxtHartIpAddress.BackColor = SystemColors.Window;
            TxtHartIpAddress.BorderStyle = BorderStyle.FixedSingle;
            TxtHartIpAddress.Location = new Point(135, 66);
            TxtHartIpAddress.Name = "TxtHartIpAddress";
            TxtHartIpAddress.Size = new Size(106, 23);
            TxtHartIpAddress.TabIndex = 77;
            TxtHartIpAddress.Text = "127.0.0.2";
            TxtHartIpAddress.TextChanged += TxtHartIpAddress_TextChanged;
            // 
            // label69
            // 
            label69.Location = new Point(24, 65);
            label69.Name = "label69";
            label69.Size = new Size(105, 22);
            label69.TabIndex = 76;
            label69.Text = "Address:";
            label69.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtHartIpHostName
            // 
            TxtHartIpHostName.AcceptsReturn = true;
            TxtHartIpHostName.BackColor = SystemColors.Window;
            TxtHartIpHostName.BorderStyle = BorderStyle.FixedSingle;
            TxtHartIpHostName.Location = new Point(135, 11);
            TxtHartIpHostName.Name = "TxtHartIpHostName";
            TxtHartIpHostName.Size = new Size(106, 23);
            TxtHartIpHostName.TabIndex = 75;
            TxtHartIpHostName.Text = "localhost";
            TxtHartIpHostName.TextChanged += TxtHartIpHostName_TextChanged;
            // 
            // label70
            // 
            label70.Location = new Point(24, 10);
            label70.Name = "label70";
            label70.Size = new Size(105, 22);
            label70.TabIndex = 74;
            label70.Text = "Host name:";
            label70.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tabStatus
            // 
            tabStatus.Controls.Add(TxtAddDev5);
            tabStatus.Controls.Add(linkTable27);
            tabStatus.Controls.Add(linkTable32);
            tabStatus.Controls.Add(TxtAddDevStandard3);
            tabStatus.Controls.Add(linkTable29_31);
            tabStatus.Controls.Add(label67);
            tabStatus.Controls.Add(label66);
            tabStatus.Controls.Add(TxtAddDevFixed);
            tabStatus.Controls.Add(label65);
            tabStatus.Controls.Add(TxtAddDevStaturated);
            tabStatus.Controls.Add(label64);
            tabStatus.Controls.Add(TxtAddDevStandard2);
            tabStatus.Controls.Add(TxtAddDevStandard1);
            tabStatus.Controls.Add(TxtAddDevStandard0);
            tabStatus.Controls.Add(label63);
            tabStatus.Controls.Add(TxtAddDev4);
            tabStatus.Controls.Add(TxtAddDev3);
            tabStatus.Controls.Add(TxtAddDev2);
            tabStatus.Controls.Add(TxtAddDev1);
            tabStatus.Controls.Add(TxtAddDev0);
            tabStatus.Controls.Add(label60);
            tabStatus.Location = new Point(4, 27);
            tabStatus.Name = "tabStatus";
            tabStatus.Size = new Size(776, 134);
            tabStatus.TabIndex = 10;
            tabStatus.Text = "Additional Status";
            tabStatus.UseVisualStyleBackColor = true;
            // 
            // TxtAddDev5
            // 
            TxtAddDev5.AcceptsReturn = true;
            TxtAddDev5.BackColor = SystemColors.Window;
            TxtAddDev5.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDev5.Location = new Point(508, 11);
            TxtAddDev5.Name = "TxtAddDev5";
            TxtAddDev5.Size = new Size(62, 23);
            TxtAddDev5.TabIndex = 175;
            TxtAddDev5.Text = "11111111";
            // 
            // linkTable27
            // 
            linkTable27.ActiveLinkColor = Color.Red;
            linkTable27.AutoSize = true;
            linkTable27.Location = new Point(453, 107);
            linkTable27.Name = "linkTable27";
            linkTable27.Size = new Size(50, 15);
            linkTable27.TabIndex = 174;
            linkTable27.TabStop = true;
            linkTable27.Text = "Table 27";
            linkTable27.LinkClicked += linkTable27_LinkClicked;
            // 
            // linkTable32
            // 
            linkTable32.ActiveLinkColor = Color.Red;
            linkTable32.AutoSize = true;
            linkTable32.Location = new Point(393, 107);
            linkTable32.Name = "linkTable32";
            linkTable32.Size = new Size(50, 15);
            linkTable32.TabIndex = 173;
            linkTable32.TabStop = true;
            linkTable32.Text = "Table 32";
            linkTable32.LinkClicked += linkTable32_LinkClicked;
            // 
            // TxtAddDevStandard3
            // 
            TxtAddDevStandard3.AcceptsReturn = true;
            TxtAddDevStandard3.BackColor = SystemColors.Window;
            TxtAddDevStandard3.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDevStandard3.Location = new Point(372, 40);
            TxtAddDevStandard3.Name = "TxtAddDevStandard3";
            TxtAddDevStandard3.Size = new Size(62, 23);
            TxtAddDevStandard3.TabIndex = 166;
            TxtAddDevStandard3.Text = "10000";
            TxtAddDevStandard3.TextChanged += TxtAddDevStandard3_TextChanged;
            // 
            // linkTable29_31
            // 
            linkTable29_31.ActiveLinkColor = Color.Red;
            linkTable29_31.AutoSize = true;
            linkTable29_31.Location = new Point(316, 107);
            linkTable29_31.Name = "linkTable29_31";
            linkTable29_31.Size = new Size(67, 15);
            linkTable29_31.TabIndex = 172;
            linkTable29_31.TabStop = true;
            linkTable29_31.Text = "Table 29-31";
            linkTable29_31.LinkClicked += linkTable29_31_LinkClicked;
            // 
            // label67
            // 
            label67.Location = new Point(168, 103);
            label67.Name = "label67";
            label67.Size = new Size(62, 22);
            label67.TabIndex = 171;
            label67.Text = "Set to 0 ..";
            label67.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label66
            // 
            label66.Location = new Point(8, 103);
            label66.Name = "label66";
            label66.Size = new Size(154, 22);
            label66.TabIndex = 170;
            label66.Text = "Specific Status 14-24:";
            label66.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtAddDevFixed
            // 
            TxtAddDevFixed.AcceptsReturn = true;
            TxtAddDevFixed.BackColor = SystemColors.Window;
            TxtAddDevFixed.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDevFixed.Location = new Point(440, 69);
            TxtAddDevFixed.Name = "TxtAddDevFixed";
            TxtAddDevFixed.Size = new Size(62, 23);
            TxtAddDevFixed.TabIndex = 169;
            TxtAddDevFixed.Text = "0010";
            TxtAddDevFixed.TextChanged += TxtAddDevFixed_TextChanged;
            // 
            // label65
            // 
            label65.Location = new Point(280, 67);
            label65.Name = "label65";
            label65.Size = new Size(154, 22);
            label65.TabIndex = 168;
            label65.Text = "Analog Channel Fixed:";
            label65.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtAddDevStaturated
            // 
            TxtAddDevStaturated.AcceptsReturn = true;
            TxtAddDevStaturated.BackColor = SystemColors.Window;
            TxtAddDevStaturated.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDevStaturated.Location = new Point(168, 69);
            TxtAddDevStaturated.Name = "TxtAddDevStaturated";
            TxtAddDevStaturated.Size = new Size(62, 23);
            TxtAddDevStaturated.TabIndex = 167;
            TxtAddDevStaturated.Text = "0100";
            TxtAddDevStaturated.TextChanged += TxtAddDevStaturated_TextChanged;
            // 
            // label64
            // 
            label64.Location = new Point(8, 67);
            label64.Name = "label64";
            label64.Size = new Size(154, 22);
            label64.TabIndex = 166;
            label64.Text = "Analog Channel Saturated:";
            label64.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtAddDevStandard2
            // 
            TxtAddDevStandard2.AcceptsReturn = true;
            TxtAddDevStandard2.BackColor = SystemColors.Window;
            TxtAddDevStandard2.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDevStandard2.Location = new Point(304, 40);
            TxtAddDevStandard2.Name = "TxtAddDevStandard2";
            TxtAddDevStandard2.Size = new Size(62, 23);
            TxtAddDevStandard2.TabIndex = 165;
            TxtAddDevStandard2.Text = "00100";
            TxtAddDevStandard2.TextChanged += TxtAddDevStandard2_TextChanged;
            // 
            // TxtAddDevStandard1
            // 
            TxtAddDevStandard1.AcceptsReturn = true;
            TxtAddDevStandard1.BackColor = SystemColors.Window;
            TxtAddDevStandard1.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDevStandard1.Location = new Point(236, 40);
            TxtAddDevStandard1.Name = "TxtAddDevStandard1";
            TxtAddDevStandard1.Size = new Size(62, 23);
            TxtAddDevStandard1.TabIndex = 164;
            TxtAddDevStandard1.Text = "000001";
            TxtAddDevStandard1.TextChanged += TxtAddDevStandard1_TextChanged;
            // 
            // TxtAddDevStandard0
            // 
            TxtAddDevStandard0.AcceptsReturn = true;
            TxtAddDevStandard0.BackColor = SystemColors.Window;
            TxtAddDevStandard0.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDevStandard0.Location = new Point(168, 40);
            TxtAddDevStandard0.Name = "TxtAddDevStandard0";
            TxtAddDevStandard0.Size = new Size(62, 23);
            TxtAddDevStandard0.TabIndex = 163;
            TxtAddDevStandard0.Text = "00010000";
            TxtAddDevStandard0.TextChanged += TxtAddDevStandard0_TextChanged;
            // 
            // label63
            // 
            label63.Location = new Point(8, 38);
            label63.Name = "label63";
            label63.Size = new Size(154, 22);
            label63.TabIndex = 162;
            label63.Text = "Standardized Status 0-3:";
            label63.TextAlign = ContentAlignment.MiddleRight;
            // 
            // TxtAddDev4
            // 
            TxtAddDev4.AcceptsReturn = true;
            TxtAddDev4.BackColor = SystemColors.Window;
            TxtAddDev4.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDev4.Location = new Point(440, 11);
            TxtAddDev4.Name = "TxtAddDev4";
            TxtAddDev4.Size = new Size(62, 23);
            TxtAddDev4.TabIndex = 159;
            TxtAddDev4.Text = "10101010";
            TxtAddDev4.TextChanged += TxtAddDev4_TextChanged;
            // 
            // TxtAddDev3
            // 
            TxtAddDev3.AcceptsReturn = true;
            TxtAddDev3.BackColor = SystemColors.Window;
            TxtAddDev3.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDev3.Location = new Point(372, 11);
            TxtAddDev3.Name = "TxtAddDev3";
            TxtAddDev3.Size = new Size(62, 23);
            TxtAddDev3.TabIndex = 158;
            TxtAddDev3.Text = "01000100";
            TxtAddDev3.TextChanged += TxtAddDev3_TextChanged;
            // 
            // TxtAddDev2
            // 
            TxtAddDev2.AcceptsReturn = true;
            TxtAddDev2.BackColor = SystemColors.Window;
            TxtAddDev2.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDev2.Location = new Point(304, 11);
            TxtAddDev2.Name = "TxtAddDev2";
            TxtAddDev2.Size = new Size(62, 23);
            TxtAddDev2.TabIndex = 157;
            TxtAddDev2.Text = "00111100";
            TxtAddDev2.TextChanged += TxtAddDev2_TextChanged;
            // 
            // TxtAddDev1
            // 
            TxtAddDev1.AcceptsReturn = true;
            TxtAddDev1.BackColor = SystemColors.Window;
            TxtAddDev1.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDev1.Location = new Point(236, 11);
            TxtAddDev1.Name = "TxtAddDev1";
            TxtAddDev1.Size = new Size(62, 23);
            TxtAddDev1.TabIndex = 156;
            TxtAddDev1.Text = "10000001";
            TxtAddDev1.TextChanged += TxtAddDev1_TextChanged;
            // 
            // TxtAddDev0
            // 
            TxtAddDev0.AcceptsReturn = true;
            TxtAddDev0.BackColor = SystemColors.Window;
            TxtAddDev0.BorderStyle = BorderStyle.FixedSingle;
            TxtAddDev0.Location = new Point(168, 11);
            TxtAddDev0.Name = "TxtAddDev0";
            TxtAddDev0.Size = new Size(62, 23);
            TxtAddDev0.TabIndex = 155;
            TxtAddDev0.Text = "00011000";
            TxtAddDev0.TextChanged += TxtAddDev0_TextChanged;
            // 
            // label60
            // 
            label60.Location = new Point(8, 9);
            label60.Name = "label60";
            label60.Size = new Size(154, 22);
            label60.TabIndex = 154;
            label60.Text = "Device Specific Status 0-5:";
            label60.TextAlign = ContentAlignment.MiddleRight;
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
            Text = "Test Hart C++ SLAVE, V 8.0";
            TopMost = true;
            Activated += FrmTestSlave_Activated;
            Closing += FrmTestSlave_Closing;
            FormClosing += FrmTestSlave_FormClosing;
            Load += FrmTestSlave_Load;
            Resize += FrmTestSlave_Resize;
            CtxMnuDecode.ResumeLayout(false);
            panTop.ResumeLayout(false);
            StatusInfo.ResumeLayout(false);
            StatusInfo.PerformLayout();
            tabOptions.ResumeLayout(false);
            groupBox16.ResumeLayout(false);
            tabDevVars.ResumeLayout(false);
            groupBox10.ResumeLayout(false);
            groupBox10.PerformLayout();
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox9.ResumeLayout(false);
            groupBox9.PerformLayout();
            tabTransducer.ResumeLayout(false);
            tabTransducer.PerformLayout();
            tabDevice.ResumeLayout(false);
            tabDevice.PerformLayout();
            tabIdentifier.ResumeLayout(false);
            tabIdentifier.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabStart.ResumeLayout(false);
            tabStart.PerformLayout();
            groupBox5.ResumeLayout(false);
            GrpHartFraming.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            tabMain.ResumeLayout(false);
            tabHartIp.ResumeLayout(false);
            tabHartIp.PerformLayout();
            tabStatus.ResumeLayout(false);
            tabStatus.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer TimWatch;
        private Panel panTop;
        private Button butMnuRecord;
        private Button butMnuClearDisplay;
        private Button butMnuInfo;
        private StatusStrip StatusInfo;
        private ToolStripStatusLabel MainStatusInfo;
        private ToolStripStatusLabel MainStatusLED;
        private System.Windows.Forms.Timer TimOperating;
        private ToolStripStatusLabel MainStatusNumFrames;
        private ToolStripStatusLabel MainStatusComPortLED;
        private ToolStripStatusLabel Tled;
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
        private ImageList ImgL_Leds;
        internal Button butMnuUpdateData;
        private TabPage tabOptions;
        private GroupBox groupBox16;
        private Button ButEditColors;
        private CheckBox ChkTopMost;
        private RadioButton RadColorSetUser;
        private RadioButton RadColorSet2;
        private RadioButton RadColorSet1;
        private TabPage tabDevVars;
        private GroupBox groupBox10;
        internal RadioButton RadPV4bad;
        internal RadioButton RadPV4good;
        private Label label46;
        internal ComboBox CmbPV4unit;
        private Label label47;
        private Label label48;
        internal TextBox TxtPV4value;
        internal TextBox TxtPV4class;
        private GroupBox groupBox8;
        internal RadioButton RadPV3bad;
        internal RadioButton RadPV3good;
        private Label label40;
        internal ComboBox CmbPV3unit;
        private Label label41;
        private Label label42;
        internal TextBox TxtPV3value;
        internal TextBox TxtPV3class;
        private GroupBox groupBox7;
        internal RadioButton RadPV2bad;
        internal RadioButton RadPV2good;
        private Label label37;
        internal ComboBox CmbPV2unit;
        private Label label38;
        private Label label39;
        internal TextBox TxtPV2value;
        internal TextBox TxtPV2class;
        private GroupBox groupBox3;
        internal RadioButton RadPV1bad;
        internal RadioButton RadPV1good;
        private Label label21;
        internal ComboBox CmbPV1unit;
        private Label label35;
        private Label label36;
        internal TextBox TxtPV1value;
        internal TextBox TxtPV1class;
        private GroupBox groupBox2;
        private Label label43;
        internal RadioButton RadCurBad;
        internal RadioButton RadCurGood;
        private Label label15;
        private Label label16;
        internal TextBox TxtCurValue;
        internal TextBox TxtCurClass;
        private GroupBox groupBox9;
        private Label label18;
        internal RadioButton RadPercBad;
        internal RadioButton RadPerGood;
        private Label label45;
        private Label label44;
        internal TextBox TxtPerValue;
        internal TextBox TxtPerClass;
        private TabPage tabTransducer;
        internal TextBox TxtMinSpan;
        internal TextBox TxtLowerLimit;
        internal TextBox TxtUpperLimit;
        internal TextBox TxtTransdSerNum;
        private Label label49;
        private Label label25;
        private Label label13;
        internal ComboBox CmbTransdUnit;
        private Label label10;
        private Label label24;
        private TabPage tabDevice;
        internal TextBox TxtYear;
        internal TextBox TxtMonth;
        internal TextBox TxtDay;
        internal TextBox TxtDescriptor;
        internal TextBox TxtMessage;
        internal TextBox TxtLongTag;
        internal TextBox TxtShortTag;
        private Label label34;
        private Label label33;
        private Label label32;
        private Label label31;
        private Label label9;
        private Label label8;
        private Label label7;
        private TabPage tabIdentifier;
        private GroupBox groupBox1;
        private LinkLabel LinkTable57;
        private LinkLabel LinkTable17;
        private LinkLabel LinkTable10;
        private LinkLabel LinkTable8;
        private LinkLabel LinkTable1;
        private LinkLabel LinkCmd0;
        internal TextBox TxtProfileTable57;
        internal TextBox TxtDistributorTable8;
        internal TextBox TxtManufacturerTable8;
        internal TextBox TxtExtDevStatus;
        internal TextBox TxtConfigChangedCounter;
        internal TextBox TxtLastDevVarCode;
        internal TextBox TxtDevUniqueID;
        internal TextBox TxtFlagsTable11;
        internal TextBox TxtSignalTable10;
        internal TextBox TxtHwRev;
        internal TextBox TxtSwRev;
        internal TextBox TxtDevRev;
        internal TextBox TxtDevTypeTable1;
        private Label label30;
        private Label label29;
        private Label label28;
        private Label label27;
        private Label label26;
        private Label label23;
        private Label label22;
        private Label label20;
        private Label label19;
        private Label label17;
        private Label label14;
        private Label label12;
        private Label label6;
        private TabPage tabStart;
        private CheckBox ChkBurstMode;
        private Label label5;
        private GroupBox groupBox5;
        private CheckBox ChkViewStatusBinary;
        private CheckBox ChkViewDecodedData;
        private CheckBox ChkViewFrameNumbers;
        private CheckBox ChkViewTiming;
        private CheckBox ChkViewAddress;
        private CheckBox ChkViewPreambles;
        private ComboBox CmbRspPreambles;
        private Label label11;
        private ComboBox CmbReqPreambles;
        private Label label2;
        private GroupBox groupBox4;
        private ComboBox CmbBaudRate;
        private Label label4;
        private Label label1;
        private Label label3;
        private ComboBox CmbComPort;
        private ComboBox CmbPollAddress;
        private TabControl tabMain;
        private TabPage tabStatus;
        private LinkLabel linkTable26;
        internal TextBox TxtChanFlags;
        private Label label58;
        private Label label57;
        internal ComboBox CmbTFunction;
        private Label label56;
        internal ComboBox CmbAlmSelect;
        private Label label55;
        internal ComboBox CmbWrProt;
        private Label label54;
        internal TextBox TxtDamping;
        private Label label53;
        internal TextBox TxtLowerRange;
        internal TextBox TxtUpperRange;
        private Label label51;
        private Label label52;
        private Label label50;
        internal ComboBox CmbRangeUnit;
        private Label label59;
        internal TextBox TxtFinAssNum;
        internal TextBox TxtAddDev0;
        private Label label60;
        internal TextBox TxtAddDev4;
        internal TextBox TxtAddDev3;
        internal TextBox TxtAddDev2;
        internal TextBox TxtAddDev1;
        internal TextBox TxtAddDevStandard2;
        internal TextBox TxtAddDevStandard1;
        internal TextBox TxtAddDevStandard0;
        private Label label63;
        private Label label67;
        private Label label66;
        internal TextBox TxtAddDevFixed;
        private Label label65;
        internal TextBox TxtAddDevStaturated;
        private Label label64;
        private LinkLabel linkTable29_31;
        internal TextBox TxtAddDevStandard3;
        private LinkLabel linkTable32;
        private LinkLabel linkTable27;
        internal TextBox TxtAddDev5;
        internal TextBox TxtCountryLetters;
        private Label label68;
        internal CheckBox ChkSIunitsOnly;
        private TabPage tabHartIp;
        private RichTextBox rtxIPFrames;
        private Label label62;
        private Label label69;
        private Label label70;
        internal CheckBox ChkHartIpUseAddress;
        internal TextBox TxtHartIpPort;
        internal TextBox TxtHartIpAddress;
        internal TextBox TxtHartIpHostName;
        private TextBox TxtLastError;
        private Label label72;
        private Label label71;
        private TextBox TxtConnectionStatus;
        private Label label61;
        private CheckBox ChkViewShortBursts;
        private CheckBox ChkSimWriteProtect;
        internal GroupBox GrpHartFraming;
        private Button ButSetDefault;
        private CheckBox ChkHartIpEnabled;
    }
}
