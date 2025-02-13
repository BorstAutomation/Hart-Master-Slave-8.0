/*
 *          File: FrmUserCmd.cs
 *                Configuration of user specific commands.
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
#endregion

namespace BaTestHart
{
    internal partial class FrmUserCmd : Form
    {
        #region Private Data
        private FrmTestClient? mo_frm_parent;
        private bool m_cancelled = false;
        #endregion

        internal FrmUserCmd()
        {
            InitializeComponent();
        }

        internal FrmUserCmd(FrmTestClient f_)
        {
            InitializeComponent();
            mo_frm_parent = f_;
        }

        private void ButOK_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void ButCancel_Click(object sender, System.EventArgs e)
        {
            m_cancelled = true;
            Close();
        }

        private void FrmAnyCmd_Load(object sender, EventArgs e)
        {
            txtCommand.Text = CSettings.CustomCmd.ToString("0");
            txtData.Text = CSettings.CustomCmdData;
            if (FrmTestClient.IsSendPossible())
            {
                butSend.BackColor = Color.PowderBlue;
            }
            else
            {
                butSend.Enabled = false;
            }
        }

        private void ButEdit_Click(object sender, EventArgs e)
        {
            FrmDataSyntax f = new FrmDataSyntax();

            f.SetDataSyntax(txtData.Text);

            System.Windows.Forms.DialogResult diagResult = f.ShowDialog();

            if (diagResult == System.Windows.Forms.DialogResult.OK)
            {
                txtData.Text = f.GetDataSyntax();
            }
        }

        private void ButSend_Click(object sender, EventArgs e)
        {
            byte byLastCmd = CSettings.CustomCmd;
            string sLastCmdData = CSettings.CustomCmdData;

            if (byte.TryParse(txtCommand.Text, out byte byCmd))
            {
                CSettings.CustomCmd = byCmd;
                CSettings.CustomCmdData = txtData.Text;
                mo_frm_parent?.SendUserCmd();
                CSettings.CustomCmd = byLastCmd;
                CSettings.CustomCmdData = sLastCmdData;
            }
        }

        private void FrmUserCmd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_cancelled)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
                _ = byte.TryParse(txtCommand.Text, out byte by_Cmd);
                CSettings.CustomCmd = by_Cmd;
                CSettings.CustomCmdData = txtData.Text;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
