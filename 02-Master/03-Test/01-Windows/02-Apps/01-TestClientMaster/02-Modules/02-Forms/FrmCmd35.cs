/*
 *          File: FrmCmd35.cs
 *                Configuration of command 35.
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using BaTestHart.HartUtils;
#endregion Namespaces

namespace BaTestHart
{
    internal partial class FrmCmd35 : Form
    {
        #region Private Data
        private FrmTestClient? mo_frm_parent;
        private bool m_cancelled = false;
        #endregion

        internal FrmCmd35()
        {
            InitializeComponent();
        }
        internal FrmCmd35(FrmTestClient f_)
        {
            InitializeComponent();
            mo_frm_parent = f_;
        }

        private void CmdOK_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void CmdCancel_Click(object sender, System.EventArgs e)
        {
            m_cancelled = true;
            Close();
        }

        private void ButSend_Click(object sender, EventArgs e)
        {
            // save data
            byte last_range_unit_idx = CSettings.RangeUnitIdx;
            float last_upper_range = CSettings.UpperRange;
            float last_lower_range = CSettings.LowerRange;

            DataToSettings();

            mo_frm_parent?.SendCmd35();

            // restore data
            CSettings.RangeUnitIdx = last_range_unit_idx;
            CSettings.UpperRange = last_upper_range;
            CSettings.LowerRange = last_lower_range;
        }

        private void FrmCmd35_Load(object sender, EventArgs e)
        {
            txtUpperRange.Text = CSettings.UpperRange.ToString("00");
            txtLowerRange.Text = CSettings.LowerRange.ToString("00");
            CHartUtils.CHartUnit.InitCombo(cmbRangeUnit, cmbRangeUnit.SelectedIndex);
            cmbRangeUnit.SelectedIndex = CSettings.RangeUnitIdx;
            if (mo_frm_parent != null)
            {
                if (FrmTestClient.IsSendPossible())
                {
                    butSend.BackColor = Color.PowderBlue;
                }
                else
                {
                    butSend.Enabled = false;
                }
            }
        }

        private void DataToSettings()
        {
            float upper_range = Convert.ToSingle(txtUpperRange.Text);
            float lower_range = Convert.ToSingle(txtLowerRange.Text);
            byte range_unit_idx = (byte)cmbRangeUnit.SelectedIndex;

            CSettings.UpperRange = upper_range;
            CSettings.LowerRange = lower_range;
            CSettings.RangeUnitIdx = range_unit_idx;
        }

        private void FrmCmd35_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_cancelled)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                DataToSettings();
                DialogResult = DialogResult.OK;
            }
        }

        private void ButReadRange_Click(object sender, EventArgs e)
        {
            txtLowerRange.Text = "";
            txtUpperRange.Text = "";
            cmbRangeUnit.SelectedIndex = 0;
            this.Refresh();
            mo_frm_parent?.SendCmd15_Wait();
            CTestClient.HandleDoCmdService();
            txtUpperRange.Text = CSettings.UpperRange.ToString("00");
            txtLowerRange.Text = CSettings.LowerRange.ToString("00");
            cmbRangeUnit.SelectedIndex = CSettings.RangeUnitIdx;
        }

        private void CmbRangeUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            CSettings.RangeUnitIdx = (byte)cmbRangeUnit.SelectedIndex;
            lblUnitl.Text = CHartUtils.CHartUnit.GetText(CSettings.RangeUnitIdx);
            lblUnitu.Text = CHartUtils.CHartUnit.GetText(CSettings.RangeUnitIdx);
        }
    }
}
