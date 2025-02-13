/*
 *          File: FrmSetColors.cs (FrmSetColors)
 *                Configuration of the coloring of the display.
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

namespace BaTestHart
{
    internal partial class FrmSetColors : Form
    {
        #region Private Data
        // Change flags
        private bool m_bMonColChanged;
        private bool m_bStatBarColChanged;
        private bool m_bSlvDispColChanged;
        // Parent connection
        private FrmTestClient? m_frmMonitor;
        #endregion Private Data

        #region Internal Data
        // Monitor colors
        internal Color MonColLinNum;
        internal Color MonColTime;
        internal Color MonColHeader;
        internal Color MonColData;
        internal Color MonColJabber;
        internal Color MonColGarbage;
        internal Color MonColError;
        internal Color MonColTrigger;
        internal Color MonColTriggerBk;
        internal Color MonColTrigSel;
        internal Color MonColDataSel;
        internal Color MonColBack;
        internal Color MonColPrimMaster;
        internal Color MonColScndMaster;
        internal Color MonColWrongData;
        internal Color MonColRespIndication;
        // Slave display colors
        internal Color SlvDispColBack;
        internal Color SlvDispColFore;
        // Status bar colors
        internal Color StatBarColBkNotRec;
        internal Color StatBarColBkRecording;
        // Button colors
        internal Color ButColBkHot;
        #endregion Internal Data

        #region Public Properties
        internal bool MonitorColorsCanged
        {
            get
            {
                return m_bMonColChanged;
            }
        }

        internal bool SlaveDisplayColorsCanged
        {
            get
            {
                return m_bSlvDispColChanged;
            }
        }

        internal bool StatusBarColorsCanged
        {
            get
            {
                return m_bStatBarColChanged;
            }
        }
        #endregion Public Properties

        #region Constructor
        internal FrmSetColors()
        {
            InitializeComponent();
        }

        internal FrmSetColors(FrmTestClient f)
        {
            InitializeComponent();
            m_frmMonitor = f;
        }
        #endregion Constructor

        #region Event Procedures
        #region Form
        internal void FrmSetColors_Load(object sender, EventArgs e)
        {
            InitColors();
            // Changed flags
            m_bMonColChanged = false;
            m_bStatBarColChanged = false;
            m_bSlvDispColChanged = false;
        }

        internal void FrmSetColors_Shown(object sender, EventArgs e)
        {

        }
        #endregion Form

        #region Buttons
        #region Dialog Handling
        private void ButOK_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
            ApplyColors();
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
            m_bMonColChanged = false;
            m_bStatBarColChanged = false;
            m_bSlvDispColChanged = false;
        }
        private void ButApply_Click(object sender, EventArgs e)
        {
            ApplyColors();
            if (m_frmMonitor != null)

            {
                m_frmMonitor.ApplyColorsFromForm(this);
            }
        }
        private void ButSetDefault_Click(object sender, EventArgs e)
        {
            MonColLinNum = Color.FromArgb(0, 0, 0);
            MonColTime = Color.FromArgb(139, 69, 19);
            MonColHeader = Color.FromArgb(0, 0, 255);
            MonColData = Color.FromArgb(0, 0, 0);
            MonColJabber = Color.FromArgb(148, 0, 211);
            MonColGarbage = Color.FromArgb(128, 0, 128);
            MonColError = Color.FromArgb(255, 0, 0);
            MonColTrigger = Color.FromArgb(255, 215, 0);
            MonColTriggerBk = Color.FromArgb(176, 224, 230);
            MonColTrigSel = Color.FromArgb(255, 248, 220);
            MonColDataSel = Color.FromArgb(65, 105, 225);
            MonColBack = Color.FromArgb(255, 228, 196);
            MonColPrimMaster = Color.FromArgb(0, 0, 255);
            MonColScndMaster = Color.FromArgb(139, 69, 19);
            MonColWrongData = Color.FromArgb(139, 0, 0);
            MonColRespIndication = Color.FromArgb(165, 42, 42);
            // Slave display colors
            SlvDispColBack = Color.FromArgb(0, 100, 0);
            SlvDispColFore = Color.FromArgb(255, 255, 0);
            // Status bar colors
            StatBarColBkNotRec = Color.FromArgb(144, 238, 144);
            StatBarColBkRecording = Color.FromArgb(240, 128, 128);
            // Button colors
            ButColBkHot = Color.FromArgb(176, 224, 230);
            // Set change flags
            m_bMonColChanged = true;
            m_bStatBarColChanged = true;
            m_bSlvDispColChanged = true;
            // Init controls
            InitDisplay();
        }
        #endregion Dialog Handling

        #region Color Operation
        private void ButSelMonColBk_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColBack.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColBack.BackColor = colorDialog.Color;
                MonColBack = colorDialog.Color;
            }
        }

        private void ButMonColPrimMaster_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColPrimMaster.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColPrimMaster.BackColor = colorDialog.Color;
                MonColPrimMaster = colorDialog.Color;
            }
        }

        private void ButMonColScndMaster_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColScndMaster.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColScndMaster.BackColor = colorDialog.Color;
                MonColScndMaster = colorDialog.Color;
            }
        }

        private void ButMonColData_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColData.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColData.BackColor = colorDialog.Color;
                MonColData = colorDialog.Color;
            }
        }

        private void ButMonColWrongData_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColWrongData.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColWrongData.BackColor = colorDialog.Color;
                MonColWrongData = colorDialog.Color;
            }
        }

        private void ButMonColHeader_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColHeader.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColHeader.BackColor = colorDialog.Color;
                MonColHeader = colorDialog.Color;
            }
        }

        private void ButMonColTime_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColTime.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColTime.BackColor = colorDialog.Color;
                MonColTime = colorDialog.Color;
            }
        }

        private void ButMonColLinNum_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColLinNum.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColLinNum.BackColor = colorDialog.Color;
                MonColLinNum = colorDialog.Color;
            }
        }

        private void ButMonColJabber_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColJabber.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColJabber.BackColor = colorDialog.Color;
                MonColJabber = colorDialog.Color;
            }
        }

        private void ButMonColGarbage_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColGarbage.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColGarbage.BackColor = colorDialog.Color;
                MonColGarbage = colorDialog.Color;
            }
        }

        private void ButMonColError_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColError.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColError.BackColor = colorDialog.Color;
                MonColError = colorDialog.Color;
            }
        }

        private void ButMonColBusyIndication_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColBusyIndication.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColBusyIndication.BackColor = colorDialog.Color;
                MonColRespIndication = colorDialog.Color;
            }
        }

        private void ButMonColDataSel_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblMonColDataSel.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblMonColDataSel.BackColor = colorDialog.Color;
                MonColDataSel = colorDialog.Color;
            }
        }

        private void ButStatBarColBkNotRec_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblStatBarColBkNotRec.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblStatBarColBkNotRec.BackColor = colorDialog.Color;
                StatBarColBkNotRec = colorDialog.Color;
            }
        }

        private void ButStatBarColBkRecording_Click(object sender, EventArgs e)
        {
            colorDialog.Color = lblStatBarColBkRecording.BackColor;
            DialogResult result = colorDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                lblStatBarColBkRecording.BackColor = colorDialog.Color;
                StatBarColBkRecording = colorDialog.Color;
            }
        }

        #endregion Color Operation
        #endregion Buttons
        #endregion Event Procedures

        #region Helpers
        private void ApplyColors()
        {
            m_bMonColChanged = false;
            m_bStatBarColChanged = false;
            m_bSlvDispColChanged = false;
            // Monitor colors
            if (MonColLinNum != CSettings.MonColLinNum)
            {
                CSettings.MonColLinNum = MonColLinNum;
                m_bMonColChanged = true;
            }
            if (MonColTime != CSettings.MonColTime)
            {
                CSettings.MonColTime = MonColTime;
                m_bMonColChanged = true;
            }
            if (MonColHeader != CSettings.MonColHeader)
            {
                CSettings.MonColHeader = MonColHeader;
                m_bMonColChanged = true;
            }
            if (MonColData != CSettings.MonColData)
            {
                CSettings.MonColData = MonColData;
                m_bMonColChanged = true;
            }
            if (MonColJabber != CSettings.MonColJabber)
            {
                CSettings.MonColJabber = MonColJabber;
                m_bMonColChanged = true;
            }
            if (MonColGarbage != CSettings.MonColGarbage)
            {
                CSettings.MonColGarbage = MonColGarbage;
                m_bMonColChanged = true;
            }
            if (MonColError != CSettings.MonColError)
            {
                CSettings.MonColError = MonColError;
                m_bMonColChanged = true;
            }
            if (MonColDataSel != CSettings.MonColDataSel)
            {
                CSettings.MonColDataSel = MonColDataSel;
                m_bMonColChanged = true;
            }
            if (MonColBack != CSettings.MonColBack)
            {
                CSettings.MonColBack = MonColBack;
                m_bMonColChanged = true;
            }
            if (MonColPrimMaster != CSettings.MonColPrimMaster)
            {
                CSettings.MonColPrimMaster = MonColPrimMaster;
                m_bMonColChanged = true;
            }
            if (MonColScndMaster != CSettings.MonColScndMaster)
            {
                CSettings.MonColScndMaster = MonColScndMaster;
                m_bMonColChanged = true;
            }
            if (MonColWrongData != CSettings.MonColWrongData)
            {
                CSettings.MonColWrongData = MonColWrongData;
                m_bMonColChanged = true;
            }
            if (MonColRespIndication != CSettings.MonColRespIndication)
            {
                CSettings.MonColRespIndication = MonColRespIndication;
                m_bMonColChanged = true;
            }
            // Status bar colors
            if (StatBarColBkNotRec != CSettings.StatBarColBkNotRec)
            {
                CSettings.StatBarColBkNotRec = StatBarColBkNotRec;
                m_bStatBarColChanged = true;
            }
            if (StatBarColBkRecording != CSettings.StatBarColBkRecording)
            {
                CSettings.StatBarColBkRecording = StatBarColBkRecording;
                m_bStatBarColChanged = true;
            }
        }

        private void InitColors()
        {
            // Monitor colors
            MonColLinNum = CSettings.MonColLinNum;
            MonColTime = CSettings.MonColTime;
            MonColHeader = CSettings.MonColHeader;
            MonColData = CSettings.MonColData;
            MonColJabber = CSettings.MonColJabber;
            MonColGarbage = CSettings.MonColGarbage;
            MonColError = CSettings.MonColError;
            MonColDataSel = CSettings.MonColDataSel;
            MonColBack = CSettings.MonColBack;
            MonColPrimMaster = CSettings.MonColPrimMaster;
            MonColScndMaster = CSettings.MonColScndMaster;
            MonColWrongData = CSettings.MonColWrongData;
            MonColRespIndication = CSettings.MonColRespIndication;
            // Status bar colors
            StatBarColBkNotRec = CSettings.StatBarColBkNotRec;
            StatBarColBkRecording = CSettings.StatBarColBkRecording;
            // Init controls
            InitDisplay();
        }

        private void InitDisplay()
        {
            // Monitor colors
            lblMonColLinNum.BackColor = MonColLinNum;
            lblMonColTime.BackColor = MonColTime;
            lblMonColHeader.BackColor = MonColHeader;
            lblMonColData.BackColor = MonColData;
            lblMonColJabber.BackColor = MonColJabber;
            lblMonColGarbage.BackColor = MonColGarbage;
            lblMonColError.BackColor = MonColError;
            lblMonColDataSel.BackColor = MonColDataSel;
            lblMonColBack.BackColor = MonColBack;
            lblMonColPrimMaster.BackColor = MonColPrimMaster;
            lblMonColScndMaster.BackColor = MonColScndMaster;
            lblMonColWrongData.BackColor = MonColWrongData;
            lblMonColBusyIndication.BackColor = MonColRespIndication;
            // Status bar colors
            lblStatBarColBkNotRec.BackColor = StatBarColBkNotRec;
            lblStatBarColBkRecording.BackColor = StatBarColBkRecording;
        }
        #endregion Helpers
    }
}
