/*
 *          File: FrmDataSyntax.cs (FrmDataSyntax)
 *                Data syntax is kind of a description language
 *                to define data sets. The form is an editor for this.
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
using BaTestHart.DataSyntax;
#endregion Namespaces

namespace BaTestHart
{
    internal partial class FrmDataSyntax : Form
    {
        #region Private Data
        private int m_num_items;
        string m_data_syntax = string.Empty;
        CDataSytax mo_data_syntax = new CDataSytax();
        int m_selected_row = -1;
        private bool m_cancelled = false;
        #endregion

        #region Component Management
        internal FrmDataSyntax()
        {
            InitializeComponent();
        }
        #endregion

        #region Events
        #region Form
        private void FrmDataSyntax_Load(object sender, EventArgs e)
        {
            mo_data_syntax.InitFromString(m_data_syntax);
            InitFromDatStxList();
        }

        private void FrmDataSyntax_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_cancelled)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            else
            {
                mo_data_syntax = new CDataSytax();

                dtgrdDataSyntaxEdit.EndEdit();

                if (dtgrdDataSyntaxEdit.RowCount > 0)
                {
                    for (int i = 0; i < dtgrdDataSyntaxEdit.RowCount; i++)
                    {
                        
                        if (dtgrdDataSyntaxEdit[1, i].Value != null)
                        {
                            string? sType = dtgrdDataSyntaxEdit[1, i].Value.ToString();
                            sType ??= "?";

                            string? sData = dtgrdDataSyntaxEdit[2, i].Value.ToString();
                            sData ??= "?";

                            mo_data_syntax.Add(sType, sData);
                        }
                    }
                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
        #endregion Form

        #region Buttons
        private void CmdOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButClearAll_Click(object sender, EventArgs e)
        {
            dtgrdDataSyntaxEdit.Rows.Clear();
            m_num_items = 0;
            AddEmptyItem();
        }

        private void ButNewLine_Click(object sender, EventArgs e)
        {
            AddEmptyItem();
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            m_cancelled = true;
            this.Close();
        }

        private void ButDelete_Click(object sender, EventArgs e)
        {
            if (m_selected_row != -1)
            {
                if (m_selected_row < (dtgrdDataSyntaxEdit.Rows.Count - 1))
                {
                    for (int i = m_selected_row; i < (dtgrdDataSyntaxEdit.Rows.Count - 1); i++)
                    {
                        dtgrdDataSyntaxEdit[1, i].Value = dtgrdDataSyntaxEdit[1, i + 1].Value;
                        dtgrdDataSyntaxEdit[2, i].Value = dtgrdDataSyntaxEdit[2, i + 1].Value;
                    }
                    // delete the last line
                    dtgrdDataSyntaxEdit.Rows.RemoveAt(dtgrdDataSyntaxEdit.Rows.Count - 1);
                    if (m_num_items > 0)
                    {
                        m_num_items--;
                    }
                }
                else if (m_selected_row == (dtgrdDataSyntaxEdit.Rows.Count - 1))
                {
                    // delete the last line
                    dtgrdDataSyntaxEdit.Rows.RemoveAt(dtgrdDataSyntaxEdit.Rows.Count - 1);
                    if (m_num_items > 0)
                    {
                        m_num_items--;
                    }
                }
            }
        }

        private void ButInsert_Click(object sender, EventArgs e)
        {
            AddEmptyItem();
            if (m_num_items > 1)
            {
                if (m_selected_row != -1)
                {
                    for (int i = dtgrdDataSyntaxEdit.Rows.Count - 1; i > m_selected_row; i--)
                    {
                        dtgrdDataSyntaxEdit[1, i].Value = dtgrdDataSyntaxEdit[1, i - 1].Value;
                        dtgrdDataSyntaxEdit[2, i].Value = dtgrdDataSyntaxEdit[2, i - 1].Value;
                    }
                    dtgrdDataSyntaxEdit[1, m_selected_row].Value = "Byte";
                    dtgrdDataSyntaxEdit[2, m_selected_row].Value = string.Empty;
                }
            }
        }
        #endregion Buttons

        #region DataGrid
        private void DtgrdScriptEdit_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            m_selected_row = e.RowIndex;
        }

        private void DtgrdScriptEdit_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
        }
        #endregion DataGrid
        #endregion Events

        #region Helpers
        private void AddEmptyItem()
        {
            m_num_items++;
            string[] row = [m_num_items.ToString("0"), "Byte", string.Empty];
            dtgrdDataSyntaxEdit.Rows.Add(row);
            dtgrdDataSyntaxEdit.Rows[m_num_items - 1].Height = 17;
        }

        private void AddErrorItem(CDataSyntaxItem item_)
        {
            m_num_items++;
            string[] row = [m_num_items.ToString("0"), "String32", item_.FormattedValue];
            dtgrdDataSyntaxEdit.Rows.Add(row);
            dtgrdDataSyntaxEdit.Rows[m_num_items - 1].Height = 17;
        }

        private void AddItem(CDataSyntaxItem item_)
        {
            m_num_items++;
            string[] row = [m_num_items.ToString("0"), item_.TypeText, item_.FormattedValue];
            dtgrdDataSyntaxEdit.Rows.Add(row);
            dtgrdDataSyntaxEdit.Rows[m_num_items - 1].Height = 17;
        }
        #endregion

        #region Internal Methods
        internal void SetDataSyntax(string s_)
        {
            m_data_syntax = s_;
        }

        internal string GetDataSyntax()
        {
            return mo_data_syntax.DatStxString;
        }
        #endregion Internal Methods

        #region Private Methods
        private void InitFromDatStxList()
        {
            List<CDataSyntaxItem> clDtStxItems = mo_data_syntax.GetDatStxList();

            m_num_items = 0;
            if (clDtStxItems.Count != 0)
            {
                foreach (CDataSyntaxItem ci in clDtStxItems)
                {
                    if (ci.Type == CDataSyntaxItem.StxItemType.ERROR)
                    {
                        AddErrorItem(ci);
                    }
                    else
                    {
                        AddItem(ci);
                    }
                }
            }
            else
            {
                AddEmptyItem();
            }
        }
        #endregion
    }
}
