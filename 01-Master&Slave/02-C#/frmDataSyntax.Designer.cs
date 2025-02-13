namespace BaTestHart
{
    partial class FrmDataSyntax
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param m_name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code
        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            butCancel = new Button();
            butOK = new Button();
            butClearAll = new Button();
            dtgrdDataSyntaxEdit = new DataGridView();
            No = new DataGridViewTextBoxColumn();
            SyntaxItem = new DataGridViewComboBoxColumn();
            Value = new DataGridViewTextBoxColumn();
            butNewLine = new Button();
            butDelete = new Button();
            butInsert = new Button();
            ((System.ComponentModel.ISupportInitialize)dtgrdDataSyntaxEdit).BeginInit();
            SuspendLayout();
            // 
            // butCancel
            // 
            butCancel.FlatStyle = FlatStyle.Popup;
            butCancel.Location = new Point(361, 372);
            butCancel.Name = "butCancel";
            butCancel.Size = new Size(82, 29);
            butCancel.TabIndex = 15;
            butCancel.Text = "Cancel";
            butCancel.Click += ButCancel_Click;
            // 
            // butOK
            // 
            butOK.FlatStyle = FlatStyle.Popup;
            butOK.Location = new Point(361, 409);
            butOK.Name = "butOK";
            butOK.Size = new Size(82, 29);
            butOK.TabIndex = 14;
            butOK.Text = "OK";
            butOK.Click += CmdOK_Click;
            // 
            // butClearAll
            // 
            butClearAll.FlatStyle = FlatStyle.Popup;
            butClearAll.Location = new Point(361, 15);
            butClearAll.Name = "butClearAll";
            butClearAll.Size = new Size(82, 29);
            butClearAll.TabIndex = 21;
            butClearAll.Text = "Clear All";
            butClearAll.Click += ButClearAll_Click;
            // 
            // dtgrdDataSyntaxEdit
            // 
            dtgrdDataSyntaxEdit.AllowUserToAddRows = false;
            dtgrdDataSyntaxEdit.AllowUserToDeleteRows = false;
            dtgrdDataSyntaxEdit.AllowUserToResizeColumns = false;
            dtgrdDataSyntaxEdit.AllowUserToResizeRows = false;
            dtgrdDataSyntaxEdit.BorderStyle = BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dtgrdDataSyntaxEdit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dtgrdDataSyntaxEdit.ColumnHeadersHeight = 18;
            dtgrdDataSyntaxEdit.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dtgrdDataSyntaxEdit.Columns.AddRange(new DataGridViewColumn[] { No, SyntaxItem, Value });
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Courier New", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dtgrdDataSyntaxEdit.DefaultCellStyle = dataGridViewCellStyle2;
            dtgrdDataSyntaxEdit.EnableHeadersVisualStyles = false;
            dtgrdDataSyntaxEdit.GridColor = SystemColors.Control;
            dtgrdDataSyntaxEdit.Location = new Point(0, 0);
            dtgrdDataSyntaxEdit.Margin = new Padding(0);
            dtgrdDataSyntaxEdit.MultiSelect = false;
            dtgrdDataSyntaxEdit.Name = "dtgrdDataSyntaxEdit";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dtgrdDataSyntaxEdit.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dtgrdDataSyntaxEdit.RowHeadersVisible = false;
            dtgrdDataSyntaxEdit.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dtgrdDataSyntaxEdit.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dtgrdDataSyntaxEdit.Size = new Size(348, 455);
            dtgrdDataSyntaxEdit.TabIndex = 137;
            dtgrdDataSyntaxEdit.CellEnter += DtgrdScriptEdit_CellEnter;
            dtgrdDataSyntaxEdit.CellLeave += DtgrdScriptEdit_CellLeave;
            // 
            // No
            // 
            No.HeaderText = "No";
            No.MinimumWidth = 30;
            No.Name = "No";
            No.ReadOnly = true;
            No.Resizable = DataGridViewTriState.False;
            No.SortMode = DataGridViewColumnSortMode.NotSortable;
            No.Width = 30;
            // 
            // SyntaxItem
            // 
            SyntaxItem.HeaderText = "Item";
            SyntaxItem.Items.AddRange(new object[] { "Byte", "Float32", "Float64", "Bin8", "Unit8", "Pasc6", "Pasc12", "Pasc24", "String8", "String16", "String32", "Word", "Int24", "DWord", "Hex8", "Hex16", "Hex24", "Hex32", "Float64", "Bin16", "Bin24", "Bin32" });
            SyntaxItem.MinimumWidth = 100;
            SyntaxItem.Name = "SyntaxItem";
            SyntaxItem.Resizable = DataGridViewTriState.False;
            SyntaxItem.ToolTipText = "Hart Command 0..255";
            // 
            // Value
            // 
            Value.HeaderText = "Data";
            Value.Name = "Value";
            Value.SortMode = DataGridViewColumnSortMode.NotSortable;
            Value.ToolTipText = "Data to be sent with the command in data syntax. Enter empty m_text if no data should be sent.";
            Value.Width = 156;
            // 
            // butNewLine
            // 
            butNewLine.FlatStyle = FlatStyle.Popup;
            butNewLine.Location = new Point(361, 126);
            butNewLine.Name = "butNewLine";
            butNewLine.Size = new Size(82, 29);
            butNewLine.TabIndex = 138;
            butNewLine.Text = "Append";
            butNewLine.Click += ButNewLine_Click;
            // 
            // butDelete
            // 
            butDelete.FlatStyle = FlatStyle.Popup;
            butDelete.Location = new Point(361, 52);
            butDelete.Name = "butDelete";
            butDelete.Size = new Size(82, 29);
            butDelete.TabIndex = 139;
            butDelete.Text = "Delete";
            butDelete.Click += ButDelete_Click;
            // 
            // butInsert
            // 
            butInsert.FlatStyle = FlatStyle.Popup;
            butInsert.Location = new Point(361, 89);
            butInsert.Name = "butInsert";
            butInsert.Size = new Size(82, 29);
            butInsert.TabIndex = 140;
            butInsert.Text = "Insert";
            butInsert.Click += ButInsert_Click;
            // 
            // FrmDataSyntax
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(467, 467);
            Controls.Add(butInsert);
            Controls.Add(butDelete);
            Controls.Add(butNewLine);
            Controls.Add(dtgrdDataSyntaxEdit);
            Controls.Add(butClearAll);
            Controls.Add(butCancel);
            Controls.Add(butOK);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDataSyntax";
            Text = "Edit Data Syntax Byte Stream";
            FormClosing += FrmDataSyntax_FormClosing;
            Load += FrmDataSyntax_Load;
            ((System.ComponentModel.ISupportInitialize)dtgrdDataSyntaxEdit).EndInit();
            ResumeLayout(false);
        }
        #endregion

        private Button butCancel;
        private Button butOK;
        private Button butClearAll;
        private DataGridView dtgrdDataSyntaxEdit;
        private Button butNewLine;
        private Button butDelete;
        private Button butInsert;
        private DataGridViewTextBoxColumn No;
        private DataGridViewComboBoxColumn SyntaxItem;
        private DataGridViewTextBoxColumn Value;
    }
}