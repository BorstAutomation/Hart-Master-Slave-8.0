/*
 *          File: FrmExtCmd.cs (FrmCmd31)
 *                Configuration of an extended command.
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#endregion Namespaces

namespace BaTestHart.Forms
{
    internal partial class FrmCmd31 : Form
    {
        #region Private Data
        private FrmTestClient? mo_frm_parent = null;
        private bool m_cancelled = false;
        #endregion

        internal FrmCmd31()
        {
            InitializeComponent();
        }
        internal FrmCmd31(FrmTestClient f)
        {
            InitializeComponent();
            mo_frm_parent = f;
        }

        private void CmdCancel_Click(object sender, EventArgs e)
        {
            m_cancelled = true;
            Close();
        }

        private void CmdOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FrmExtCmd_Load(object sender, EventArgs e)
        {
            txtExtCmd.Text = CSettings.ExtCmd.ToString("0");
            txtExtCmdData.Text = CSettings.ExtCmdData;
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

            f.SetDataSyntax(txtExtCmdData.Text);

            if ((f.ShowDialog() == System.Windows.Forms.DialogResult.OK))
            {
                txtExtCmdData.Text = f.GetDataSyntax();
            }
        }

        private void ButSend_Click(object sender, EventArgs e)
        {
            ushort usLastExtCmd = CSettings.ExtCmd;
            string sLastExtCmdData = CSettings.ExtCmdData;

            _ = ushort.TryParse(txtExtCmd.Text, out ushort usExtCmd);
            CSettings.ExtCmd = usExtCmd;
            CSettings.ExtCmdData = txtExtCmdData.Text;
            mo_frm_parent?.SendCmd31();

            CSettings.ExtCmd = usLastExtCmd;
            CSettings.ExtCmdData = sLastExtCmdData;
        }

        private void FrmExtCmd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_cancelled)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {

                _ = ushort.TryParse(txtExtCmd.Text, out ushort us_ExtCmd);
                CSettings.ExtCmd = us_ExtCmd;
                CSettings.ExtCmdData = txtExtCmdData.Text;
                DialogResult = DialogResult.OK;
            }
        }
    }
}
