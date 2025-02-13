namespace BaTestHart
{
    partial class FrmAbout
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            label3 = new Label();
            label4 = new Label();
            lblDate = new Label();
            label5 = new Label();
            lblVersion = new Label();
            butOpenUserManual = new Button();
            textBox1 = new TextBox();
            label7 = new Label();
            label8 = new Label();
            ButVersionInfo = new Button();
            label1 = new Label();
            label2 = new Label();
            ButDataSheet = new Button();
            ButExit = new Button();
            label6 = new Label();
            ButHartCoding = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.FlatStyle = FlatStyle.Popup;
            button1.Location = new Point(14, 379);
            button1.Name = "button1";
            button1.Size = new Size(105, 30);
            button1.TabIndex = 1;
            button1.Text = "OK";
            button1.Click += Button1_Click;
            // 
            // label3
            // 
            label3.Location = new Point(18, 9);
            label3.Name = "label3";
            label3.Size = new Size(394, 55);
            label3.TabIndex = 4;
            label3.Text = "This application is the adapter for checking the Hart Slave written in C++. With this software, which was created with C# and .NET, it is possible to test all of the functions of the Hart Slave.";
            // 
            // label4
            // 
            label4.Location = new Point(172, 67);
            label4.Margin = new Padding(3);
            label4.Name = "label4";
            label4.Size = new Size(54, 26);
            label4.TabIndex = 5;
            label4.Text = "Date:";
            label4.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblDate
            // 
            lblDate.Location = new Point(232, 67);
            lblDate.Margin = new Padding(3);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(89, 26);
            lblDate.TabIndex = 6;
            lblDate.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Location = new Point(18, 67);
            label5.Margin = new Padding(3);
            label5.Name = "label5";
            label5.Size = new Size(68, 26);
            label5.TabIndex = 7;
            label5.Text = "Version:";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblVersion
            // 
            lblVersion.Location = new Point(92, 67);
            lblVersion.Margin = new Padding(3);
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(90, 26);
            lblVersion.TabIndex = 10;
            lblVersion.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // butOpenUserManual
            // 
            butOpenUserManual.BackColor = SystemColors.Info;
            butOpenUserManual.FlatStyle = FlatStyle.Popup;
            butOpenUserManual.Location = new Point(341, 379);
            butOpenUserManual.Name = "butOpenUserManual";
            butOpenUserManual.Size = new Size(125, 30);
            butOpenUserManual.TabIndex = 20;
            butOpenUserManual.Text = "User's Manual";
            butOpenUserManual.UseVisualStyleBackColor = false;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Control;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Location = new Point(222, 128);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(242, 87);
            textBox1.TabIndex = 23;
            textBox1.Text = "Fon: +49 (0)4721 6985100\r\n\r\n\r\nEmail: HartTools@borst-automation.de\r\nHome: https://www.borst-automation.de";
            textBox1.TextAlign = HorizontalAlignment.Right;
            // 
            // label7
            // 
            label7.Location = new Point(18, 99);
            label7.Margin = new Padding(3);
            label7.Name = "label7";
            label7.Size = new Size(318, 25);
            label7.TabIndex = 12;
            label7.Text = "If there are problems or questions please contact:";
            // 
            // label8
            // 
            label8.Location = new Point(18, 128);
            label8.Name = "label8";
            label8.Size = new Size(208, 67);
            label8.TabIndex = 13;
            label8.Text = "Walter Borst\r\nKapitaen-Alexander-Strasse 39\r\n27472 Cuxhaven\r\nGERMANY";
            // 
            // ButVersionInfo
            // 
            ButVersionInfo.FlatStyle = FlatStyle.Popup;
            ButVersionInfo.Location = new Point(112, 208);
            ButVersionInfo.Name = "ButVersionInfo";
            ButVersionInfo.Size = new Size(58, 27);
            ButVersionInfo.TabIndex = 24;
            ButVersionInfo.Text = "View";
            ButVersionInfo.Click += ButVersionInfo_Click;
            // 
            // label1
            // 
            label1.Location = new Point(18, 209);
            label1.Margin = new Padding(3);
            label1.Name = "label1";
            label1.Size = new Size(88, 25);
            label1.TabIndex = 25;
            label1.Text = "Release notes:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Location = new Point(18, 242);
            label2.Margin = new Padding(3);
            label2.Name = "label2";
            label2.Size = new Size(88, 25);
            label2.TabIndex = 27;
            label2.Text = "Data sheet:";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ButDataSheet
            // 
            ButDataSheet.FlatStyle = FlatStyle.Popup;
            ButDataSheet.Location = new Point(112, 241);
            ButDataSheet.Name = "ButDataSheet";
            ButDataSheet.Size = new Size(58, 27);
            ButDataSheet.TabIndex = 26;
            ButDataSheet.Text = "View";
            ButDataSheet.Click += ButDataSheet_Click;
            // 
            // ButExit
            // 
            ButExit.FlatStyle = FlatStyle.Popup;
            ButExit.Location = new Point(406, 241);
            ButExit.Name = "ButExit";
            ButExit.Size = new Size(58, 27);
            ButExit.TabIndex = 28;
            ButExit.Text = "Exit";
            ButExit.Click += ButExit_Click;
            // 
            // label6
            // 
            label6.Location = new Point(184, 242);
            label6.Margin = new Padding(3);
            label6.Name = "label6";
            label6.Size = new Size(88, 25);
            label6.TabIndex = 30;
            label6.Text = "Hart Coding:";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // ButHartCoding
            // 
            ButHartCoding.FlatStyle = FlatStyle.Popup;
            ButHartCoding.Location = new Point(278, 241);
            ButHartCoding.Name = "ButHartCoding";
            ButHartCoding.Size = new Size(58, 27);
            ButHartCoding.TabIndex = 29;
            ButHartCoding.Text = "View";
            ButHartCoding.Click += ButHartCoding_Click;
            // 
            // FrmAbout
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(478, 279);
            Controls.Add(label6);
            Controls.Add(ButHartCoding);
            Controls.Add(ButExit);
            Controls.Add(label2);
            Controls.Add(ButDataSheet);
            Controls.Add(label1);
            Controls.Add(ButVersionInfo);
            Controls.Add(textBox1);
            Controls.Add(butOpenUserManual);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(lblVersion);
            Controls.Add(label5);
            Controls.Add(lblDate);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(button1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmAbout";
            Text = "About the Hart C++ Slave Test";
            Load += FrmAbout_Load;
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private Label label1;
        private Button button1;
        private Label label2;
        private Label label3;
        private Label label4;
        internal Label lblDate;
        private Label label5;
        internal Label lblVersion;
        private Button butOpenUserManual;
        private TextBox textBox1;
        private Label label7;
        private Label label8;
        private Button ButVersionInfo;
        private Button ButDataSheet;
        private Button ButExit;
        private Label label6;
        private Button ButHartCoding;
    }
}
