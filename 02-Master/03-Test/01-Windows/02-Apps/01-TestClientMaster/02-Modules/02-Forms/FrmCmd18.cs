/*
 *          File: FrmCmd18.cs
 *                Configuration of command 18.
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
#endregion Namespaces

namespace BaTestHart
{
    internal partial class FrmCmd18 : Form
    {
        #region Private Data
        private FrmTestClient? mo_frm_parent;
        private bool m_ancelled = false;
        #endregion

        internal FrmCmd18()
        {
            InitializeComponent();
        }
        internal FrmCmd18(FrmTestClient f_)
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
            m_ancelled = true;
            Close();
        }

        private void ButSend_Click(object sender, EventArgs e)
        {
            // save data
            string sLastTagNameShort = CSettings.TagNameShort;
            string sLastDescriptor = CSettings.Descriptor;
            byte byLastDay = CSettings.Day;
            byte byLastMonth = CSettings.Month;
            byte byLastYear = CSettings.Year;

            DataToSettings();

            mo_frm_parent?.SendCmd18();

            // restore data
            CSettings.TagNameShort = sLastTagNameShort;
            CSettings.Descriptor = sLastDescriptor;
            CSettings.Day = byLastDay;
            CSettings.Month = byLastMonth;
            CSettings.Year = byLastYear;
        }

        private void FrmCmd18_Load(object sender, EventArgs e)
        {
            ushort usYear;

            txtTagName.Text = CSettings.TagNameShort;
            txtDescriptor.Text = CSettings.Descriptor;
            txtDay.Text = CSettings.Day.ToString("00");
            txtMonth.Text = CSettings.Month.ToString("00");
            usYear = (ushort)(CSettings.Year + 1900);
            txtYear.Text = usYear.ToString("0000");
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
            ushort usYear = Convert.ToUInt16(txtYear.Text);
            byte byYear;

            if (usYear < 1900)
            {
                usYear += 1900;
            }
            byYear = (byte)(usYear - 1900);
            if (txtTagName.Text.Length > 8)
            {
                txtTagName.Text = txtTagName.Text.Remove(8);
            }
            CSettings.TagNameShort = txtTagName.Text.Trim().ToUpper().PadRight(8, ' ');
            if (txtDescriptor.Text.Length > 16)
            {
                txtDescriptor.Text = txtDescriptor.Text.Remove(16);
            }
            CSettings.Descriptor = txtDescriptor.Text.Trim().ToUpper().PadRight(16, ' ');
            CSettings.Day = CFrameHelper.GetByteFromTextBox(txtDay);
            CSettings.Month = CFrameHelper.GetByteFromTextBox(txtMonth);
            CSettings.Year = byYear;
        }

        private void FrmCmd18_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_ancelled)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                DataToSettings();
                DialogResult = DialogResult.OK;
            }
        }
    }
}
