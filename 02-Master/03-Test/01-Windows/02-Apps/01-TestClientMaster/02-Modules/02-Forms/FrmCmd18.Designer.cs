namespace BaTestHart
{
    /// <summary>
    /// Zusammenfassung für FrmCmd18.
    /// </summary>
    partial class FrmCmd18
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
            txtTagName = new TextBox();
            txtDescriptor = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txtDay = new TextBox();
            txtMonth = new TextBox();
            label4 = new Label();
            txtYear = new TextBox();
            label5 = new Label();
            cmdOK = new Button();
            cmdCancel = new Button();
            butSend = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(5, 10);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 0;
            label1.Text = "Tag Name:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtTagName
            // 
            txtTagName.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtTagName.Location = new Point(106, 5);
            txtTagName.MaxLength = 8;
            txtTagName.Name = "txtTagName";
            txtTagName.Size = new Size(100, 21);
            txtTagName.TabIndex = 1;
            // 
            // txtDescriptor
            // 
            txtDescriptor.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtDescriptor.Location = new Point(106, 34);
            txtDescriptor.MaxLength = 16;
            txtDescriptor.Name = "txtDescriptor";
            txtDescriptor.Size = new Size(187, 21);
            txtDescriptor.TabIndex = 3;
            // 
            // label2
            // 
            label2.Location = new Point(5, 39);
            label2.Name = "label2";
            label2.Size = new Size(101, 20);
            label2.TabIndex = 2;
            label2.Text = "Descriptor:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Location = new Point(5, 69);
            label3.Name = "label3";
            label3.Size = new Size(38, 20);
            label3.TabIndex = 4;
            label3.Text = "Day:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtDay
            // 
            txtDay.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtDay.Location = new Point(43, 64);
            txtDay.MaxLength = 2;
            txtDay.Name = "txtDay";
            txtDay.Size = new Size(39, 21);
            txtDay.TabIndex = 5;
            // 
            // txtMonth
            // 
            txtMonth.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtMonth.Location = new Point(154, 64);
            txtMonth.MaxLength = 2;
            txtMonth.Name = "txtMonth";
            txtMonth.Size = new Size(38, 21);
            txtMonth.TabIndex = 7;
            // 
            // label4
            // 
            label4.Location = new Point(106, 69);
            label4.Name = "label4";
            label4.Size = new Size(48, 20);
            label4.TabIndex = 6;
            label4.Text = "Month:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtYear
            // 
            txtYear.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtYear.Location = new Point(250, 64);
            txtYear.MaxLength = 4;
            txtYear.Name = "txtYear";
            txtYear.Size = new Size(43, 21);
            txtYear.TabIndex = 9;
            txtYear.Text = "2000";
            // 
            // label5
            // 
            label5.Location = new Point(211, 69);
            label5.Name = "label5";
            label5.Size = new Size(39, 20);
            label5.TabIndex = 8;
            label5.Text = "Year:";
            label5.TextAlign = ContentAlignment.MiddleRight;
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
            // FrmCmd18
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(302, 139);
            Controls.Add(butSend);
            Controls.Add(cmdCancel);
            Controls.Add(cmdOK);
            Controls.Add(txtYear);
            Controls.Add(label5);
            Controls.Add(txtMonth);
            Controls.Add(label4);
            Controls.Add(txtDay);
            Controls.Add(label3);
            Controls.Add(txtDescriptor);
            Controls.Add(label2);
            Controls.Add(txtTagName);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmCmd18";
            ShowInTaskbar = false;
            Text = "Hart Cmd 18";
            FormClosing += FrmCmd18_FormClosing;
            Load += FrmCmd18_Load;
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label label1;
        private TextBox txtTagName;
        private TextBox txtDescriptor;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button cmdOK;
        private TextBox txtDay;
        private TextBox txtMonth;
        private TextBox txtYear;
        private Button cmdCancel;
        private Button butSend;

    }
}
