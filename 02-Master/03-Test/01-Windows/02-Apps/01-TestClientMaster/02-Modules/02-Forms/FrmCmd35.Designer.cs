namespace BaTestHart
{
    /// <summary>
    /// Zusammenfassung für FrmCmd35.
    /// </summary>
    partial class FrmCmd35
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtUpperRange = new TextBox();
            txtLowerRange = new TextBox();
            label2 = new Label();
            cmdOK = new Button();
            cmdCancel = new Button();
            butSend = new Button();
            cmbRangeUnit = new ComboBox();
            label3 = new Label();
            lblUnitu = new Label();
            lblUnitl = new Label();
            butReadRange = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(12, 5);
            label1.Name = "label1";
            label1.Size = new Size(88, 20);
            label1.TabIndex = 0;
            label1.Text = "Upper:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtUpperRange
            // 
            txtUpperRange.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtUpperRange.Location = new Point(106, 5);
            txtUpperRange.MaxLength = 8;
            txtUpperRange.Name = "txtUpperRange";
            txtUpperRange.Size = new Size(85, 21);
            txtUpperRange.TabIndex = 1;
            // 
            // txtLowerRange
            // 
            txtLowerRange.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtLowerRange.Location = new Point(106, 34);
            txtLowerRange.MaxLength = 16;
            txtLowerRange.Name = "txtLowerRange";
            txtLowerRange.Size = new Size(85, 21);
            txtLowerRange.TabIndex = 3;
            // 
            // label2
            // 
            label2.Location = new Point(12, 34);
            label2.Name = "label2";
            label2.Size = new Size(88, 20);
            label2.TabIndex = 2;
            label2.Text = "Lower:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // cmdOK
            // 
            cmdOK.FlatStyle = FlatStyle.Popup;
            cmdOK.Location = new Point(211, 103);
            cmdOK.Name = "cmdOK";
            cmdOK.Size = new Size(82, 30);
            cmdOK.TabIndex = 10;
            cmdOK.Text = "OK";
            cmdOK.Click += CmdOK_Click;
            // 
            // cmdCancel
            // 
            cmdCancel.FlatStyle = FlatStyle.Popup;
            cmdCancel.Location = new Point(5, 103);
            cmdCancel.Name = "cmdCancel";
            cmdCancel.Size = new Size(81, 30);
            cmdCancel.TabIndex = 11;
            cmdCancel.Text = "Cancel";
            cmdCancel.Click += CmdCancel_Click;
            // 
            // butSend
            // 
            butSend.FlatStyle = FlatStyle.Popup;
            butSend.Location = new Point(109, 103);
            butSend.Name = "butSend";
            butSend.Size = new Size(82, 30);
            butSend.TabIndex = 26;
            butSend.Text = "Send";
            butSend.Click += ButSend_Click;
            // 
            // cmbRangeUnit
            // 
            cmbRangeUnit.FormattingEnabled = true;
            cmbRangeUnit.Location = new Point(105, 66);
            cmbRangeUnit.Name = "cmbRangeUnit";
            cmbRangeUnit.Size = new Size(86, 23);
            cmbRangeUnit.TabIndex = 27;
            cmbRangeUnit.SelectedIndexChanged += CmbRangeUnit_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.Location = new Point(12, 69);
            label3.Name = "label3";
            label3.Size = new Size(87, 20);
            label3.TabIndex = 28;
            label3.Text = "Units code:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblUnitu
            // 
            lblUnitu.AutoSize = true;
            lblUnitu.Location = new Point(197, 8);
            lblUnitu.Name = "lblUnitu";
            lblUnitu.Size = new Size(22, 15);
            lblUnitu.TabIndex = 29;
            lblUnitu.Text = "-/-";
            // 
            // lblUnitl
            // 
            lblUnitl.AutoSize = true;
            lblUnitl.Location = new Point(197, 37);
            lblUnitl.Name = "lblUnitl";
            lblUnitl.Size = new Size(22, 15);
            lblUnitl.TabIndex = 30;
            lblUnitl.Text = "-/-";
            // 
            // butReadRange
            // 
            butReadRange.FlatStyle = FlatStyle.Popup;
            butReadRange.Location = new Point(211, 66);
            butReadRange.Name = "butReadRange";
            butReadRange.Size = new Size(82, 25);
            butReadRange.TabIndex = 31;
            butReadRange.Text = "Read Data";
            butReadRange.Click += ButReadRange_Click;
            // 
            // FrmCmd35
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(302, 139);
            Controls.Add(butReadRange);
            Controls.Add(lblUnitl);
            Controls.Add(lblUnitu);
            Controls.Add(label3);
            Controls.Add(cmbRangeUnit);
            Controls.Add(butSend);
            Controls.Add(cmdCancel);
            Controls.Add(cmdOK);
            Controls.Add(txtLowerRange);
            Controls.Add(label2);
            Controls.Add(txtUpperRange);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCmd35";
            ShowInTaskbar = false;
            Text = "Hart Cmd 35 (Set Range)";
            FormClosing += FrmCmd35_FormClosing;
            Load += FrmCmd35_Load;
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label label1;
        private TextBox txtUpperRange;
        private TextBox txtLowerRange;
        private Label label2;
        private Button cmdOK;
        private Button cmdCancel;
        private Button butSend;
        private ComboBox cmbRangeUnit;
        private Label label3;
        private Label lblUnitu;
        private Label lblUnitl;
        private Button butReadRange;
    }
}
