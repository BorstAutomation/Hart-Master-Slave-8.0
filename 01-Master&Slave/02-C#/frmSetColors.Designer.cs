namespace BaTestHart
{
    partial class FrmSetColors
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
            colorDialog = new ColorDialog();
            grpColMonitor = new GroupBox();
            butMonColDataSel = new Button();
            lblMonColDataSel = new Label();
            label25 = new Label();
            butMonColBusyIndication = new Button();
            lblMonColBusyIndication = new Label();
            label23 = new Label();
            butMonColError = new Button();
            lblMonColError = new Label();
            label21 = new Label();
            butMonColGarbage = new Button();
            lblMonColGarbage = new Label();
            label19 = new Label();
            butMonColJabber = new Button();
            lblMonColJabber = new Label();
            label17 = new Label();
            butMonColLinNum = new Button();
            lblMonColLinNum = new Label();
            label15 = new Label();
            butMonColTime = new Button();
            lblMonColTime = new Label();
            label13 = new Label();
            butMonColHeader = new Button();
            lblMonColHeader = new Label();
            label11 = new Label();
            butMonColWrongData = new Button();
            lblMonColWrongData = new Label();
            label9 = new Label();
            butMonColData = new Button();
            lblMonColData = new Label();
            label7 = new Label();
            butMonColScndMaster = new Button();
            lblMonColScndMaster = new Label();
            label5 = new Label();
            butMonColPrimMaster = new Button();
            lblMonColPrimMaster = new Label();
            label3 = new Label();
            butMonColBack = new Button();
            lblMonColBack = new Label();
            label1 = new Label();
            butOK = new Button();
            butCancel = new Button();
            butApply = new Button();
            groupBox1 = new GroupBox();
            butStatBarColBkRecording = new Button();
            lblStatBarColBkRecording = new Label();
            label37 = new Label();
            butStatBarColBkNotRec = new Button();
            lblStatBarColBkNotRec = new Label();
            label39 = new Label();
            butSetDefault = new Button();
            grpColMonitor.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // grpColMonitor
            // 
            grpColMonitor.Controls.Add(butMonColDataSel);
            grpColMonitor.Controls.Add(lblMonColDataSel);
            grpColMonitor.Controls.Add(label25);
            grpColMonitor.Controls.Add(butMonColBusyIndication);
            grpColMonitor.Controls.Add(lblMonColBusyIndication);
            grpColMonitor.Controls.Add(label23);
            grpColMonitor.Controls.Add(butMonColError);
            grpColMonitor.Controls.Add(lblMonColError);
            grpColMonitor.Controls.Add(label21);
            grpColMonitor.Controls.Add(butMonColGarbage);
            grpColMonitor.Controls.Add(lblMonColGarbage);
            grpColMonitor.Controls.Add(label19);
            grpColMonitor.Controls.Add(butMonColJabber);
            grpColMonitor.Controls.Add(lblMonColJabber);
            grpColMonitor.Controls.Add(label17);
            grpColMonitor.Controls.Add(butMonColLinNum);
            grpColMonitor.Controls.Add(lblMonColLinNum);
            grpColMonitor.Controls.Add(label15);
            grpColMonitor.Controls.Add(butMonColTime);
            grpColMonitor.Controls.Add(lblMonColTime);
            grpColMonitor.Controls.Add(label13);
            grpColMonitor.Controls.Add(butMonColHeader);
            grpColMonitor.Controls.Add(lblMonColHeader);
            grpColMonitor.Controls.Add(label11);
            grpColMonitor.Controls.Add(butMonColWrongData);
            grpColMonitor.Controls.Add(lblMonColWrongData);
            grpColMonitor.Controls.Add(label9);
            grpColMonitor.Controls.Add(butMonColData);
            grpColMonitor.Controls.Add(lblMonColData);
            grpColMonitor.Controls.Add(label7);
            grpColMonitor.Controls.Add(butMonColScndMaster);
            grpColMonitor.Controls.Add(lblMonColScndMaster);
            grpColMonitor.Controls.Add(label5);
            grpColMonitor.Controls.Add(butMonColPrimMaster);
            grpColMonitor.Controls.Add(lblMonColPrimMaster);
            grpColMonitor.Controls.Add(label3);
            grpColMonitor.Controls.Add(butMonColBack);
            grpColMonitor.Controls.Add(lblMonColBack);
            grpColMonitor.Controls.Add(label1);
            grpColMonitor.Location = new Point(9, 12);
            grpColMonitor.Margin = new Padding(4, 3, 4, 3);
            grpColMonitor.Name = "grpColMonitor";
            grpColMonitor.Padding = new Padding(4, 3, 4, 3);
            grpColMonitor.Size = new Size(560, 259);
            grpColMonitor.TabIndex = 0;
            grpColMonitor.TabStop = false;
            grpColMonitor.Text = "Monitor";
            // 
            // butMonColDataSel
            // 
            butMonColDataSel.Location = new Point(520, 152);
            butMonColDataSel.Margin = new Padding(4, 3, 4, 3);
            butMonColDataSel.Name = "butMonColDataSel";
            butMonColDataSel.Size = new Size(29, 28);
            butMonColDataSel.TabIndex = 38;
            butMonColDataSel.Text = "...";
            butMonColDataSel.UseVisualStyleBackColor = true;
            butMonColDataSel.Click += ButMonColDataSel_Click;
            // 
            // lblMonColDataSel
            // 
            lblMonColDataSel.BorderStyle = BorderStyle.FixedSingle;
            lblMonColDataSel.Location = new Point(482, 152);
            lblMonColDataSel.Margin = new Padding(4, 0, 4, 0);
            lblMonColDataSel.Name = "lblMonColDataSel";
            lblMonColDataSel.Size = new Size(31, 27);
            lblMonColDataSel.TabIndex = 37;
            // 
            // label25
            // 
            label25.Location = new Point(290, 157);
            label25.Margin = new Padding(4, 0, 4, 0);
            label25.Name = "label25";
            label25.Size = new Size(184, 23);
            label25.TabIndex = 36;
            label25.Text = "Selected Data Background:";
            label25.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColBusyIndication
            // 
            butMonColBusyIndication.Location = new Point(520, 118);
            butMonColBusyIndication.Margin = new Padding(4, 3, 4, 3);
            butMonColBusyIndication.Name = "butMonColBusyIndication";
            butMonColBusyIndication.Size = new Size(29, 28);
            butMonColBusyIndication.TabIndex = 35;
            butMonColBusyIndication.Text = "...";
            butMonColBusyIndication.UseVisualStyleBackColor = true;
            butMonColBusyIndication.Click += ButMonColBusyIndication_Click;
            // 
            // lblMonColBusyIndication
            // 
            lblMonColBusyIndication.BorderStyle = BorderStyle.FixedSingle;
            lblMonColBusyIndication.Location = new Point(482, 118);
            lblMonColBusyIndication.Margin = new Padding(4, 0, 4, 0);
            lblMonColBusyIndication.Name = "lblMonColBusyIndication";
            lblMonColBusyIndication.Size = new Size(31, 27);
            lblMonColBusyIndication.TabIndex = 34;
            // 
            // label23
            // 
            label23.Location = new Point(290, 122);
            label23.Margin = new Padding(4, 0, 4, 0);
            label23.Name = "label23";
            label23.Size = new Size(184, 23);
            label23.TabIndex = 33;
            label23.Text = "Decoded Response Codes:";
            label23.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColError
            // 
            butMonColError.Location = new Point(520, 83);
            butMonColError.Margin = new Padding(4, 3, 4, 3);
            butMonColError.Name = "butMonColError";
            butMonColError.Size = new Size(29, 28);
            butMonColError.TabIndex = 32;
            butMonColError.Text = "...";
            butMonColError.UseVisualStyleBackColor = true;
            butMonColError.Click += ButMonColError_Click;
            // 
            // lblMonColError
            // 
            lblMonColError.BorderStyle = BorderStyle.FixedSingle;
            lblMonColError.Location = new Point(482, 83);
            lblMonColError.Margin = new Padding(4, 0, 4, 0);
            lblMonColError.Name = "lblMonColError";
            lblMonColError.Size = new Size(31, 27);
            lblMonColError.TabIndex = 31;
            // 
            // label21
            // 
            label21.Location = new Point(290, 88);
            label21.Margin = new Padding(4, 0, 4, 0);
            label21.Name = "label21";
            label21.Size = new Size(184, 23);
            label21.TabIndex = 30;
            label21.Text = "Error Indications:";
            label21.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColGarbage
            // 
            butMonColGarbage.Location = new Point(522, 48);
            butMonColGarbage.Margin = new Padding(4, 3, 4, 3);
            butMonColGarbage.Name = "butMonColGarbage";
            butMonColGarbage.Size = new Size(29, 28);
            butMonColGarbage.TabIndex = 29;
            butMonColGarbage.Text = "...";
            butMonColGarbage.UseVisualStyleBackColor = true;
            butMonColGarbage.Click += ButMonColGarbage_Click;
            // 
            // lblMonColGarbage
            // 
            lblMonColGarbage.BorderStyle = BorderStyle.FixedSingle;
            lblMonColGarbage.Location = new Point(483, 48);
            lblMonColGarbage.Margin = new Padding(4, 0, 4, 0);
            lblMonColGarbage.Name = "lblMonColGarbage";
            lblMonColGarbage.Size = new Size(31, 27);
            lblMonColGarbage.TabIndex = 28;
            // 
            // label19
            // 
            label19.Location = new Point(292, 53);
            label19.Margin = new Padding(4, 0, 4, 0);
            label19.Name = "label19";
            label19.Size = new Size(184, 23);
            label19.TabIndex = 27;
            label19.Text = "Data Garbage:";
            label19.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColJabber
            // 
            butMonColJabber.Location = new Point(522, 14);
            butMonColJabber.Margin = new Padding(4, 3, 4, 3);
            butMonColJabber.Name = "butMonColJabber";
            butMonColJabber.Size = new Size(29, 28);
            butMonColJabber.TabIndex = 26;
            butMonColJabber.Text = "...";
            butMonColJabber.UseVisualStyleBackColor = true;
            butMonColJabber.Click += ButMonColJabber_Click;
            // 
            // lblMonColJabber
            // 
            lblMonColJabber.BorderStyle = BorderStyle.FixedSingle;
            lblMonColJabber.Location = new Point(483, 14);
            lblMonColJabber.Margin = new Padding(4, 0, 4, 0);
            lblMonColJabber.Name = "lblMonColJabber";
            lblMonColJabber.Size = new Size(31, 27);
            lblMonColJabber.TabIndex = 25;
            // 
            // label17
            // 
            label17.Location = new Point(292, 18);
            label17.Margin = new Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new Size(184, 23);
            label17.TabIndex = 24;
            label17.Text = "Jabber Octets:";
            label17.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColLinNum
            // 
            butMonColLinNum.Location = new Point(520, 187);
            butMonColLinNum.Margin = new Padding(4, 3, 4, 3);
            butMonColLinNum.Name = "butMonColLinNum";
            butMonColLinNum.Size = new Size(29, 28);
            butMonColLinNum.TabIndex = 23;
            butMonColLinNum.Text = "...";
            butMonColLinNum.UseVisualStyleBackColor = true;
            butMonColLinNum.Click += ButMonColLinNum_Click;
            // 
            // lblMonColLinNum
            // 
            lblMonColLinNum.BorderStyle = BorderStyle.FixedSingle;
            lblMonColLinNum.Location = new Point(481, 187);
            lblMonColLinNum.Margin = new Padding(4, 0, 4, 0);
            lblMonColLinNum.Name = "lblMonColLinNum";
            lblMonColLinNum.Size = new Size(31, 27);
            lblMonColLinNum.TabIndex = 22;
            // 
            // label15
            // 
            label15.Location = new Point(290, 192);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(184, 23);
            label15.TabIndex = 21;
            label15.Text = "Line Numbers:";
            label15.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColTime
            // 
            butMonColTime.Location = new Point(237, 222);
            butMonColTime.Margin = new Padding(4, 3, 4, 3);
            butMonColTime.Name = "butMonColTime";
            butMonColTime.Size = new Size(29, 28);
            butMonColTime.TabIndex = 20;
            butMonColTime.Text = "...";
            butMonColTime.UseVisualStyleBackColor = true;
            butMonColTime.Click += ButMonColTime_Click;
            // 
            // lblMonColTime
            // 
            lblMonColTime.BorderStyle = BorderStyle.FixedSingle;
            lblMonColTime.Location = new Point(198, 222);
            lblMonColTime.Margin = new Padding(4, 0, 4, 0);
            lblMonColTime.Name = "lblMonColTime";
            lblMonColTime.Size = new Size(31, 27);
            lblMonColTime.TabIndex = 19;
            // 
            // label13
            // 
            label13.Location = new Point(7, 226);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(184, 23);
            label13.TabIndex = 18;
            label13.Text = "Timing:";
            label13.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColHeader
            // 
            butMonColHeader.Location = new Point(237, 187);
            butMonColHeader.Margin = new Padding(4, 3, 4, 3);
            butMonColHeader.Name = "butMonColHeader";
            butMonColHeader.Size = new Size(29, 28);
            butMonColHeader.TabIndex = 17;
            butMonColHeader.Text = "...";
            butMonColHeader.UseVisualStyleBackColor = true;
            butMonColHeader.Click += ButMonColHeader_Click;
            // 
            // lblMonColHeader
            // 
            lblMonColHeader.BorderStyle = BorderStyle.FixedSingle;
            lblMonColHeader.Location = new Point(198, 187);
            lblMonColHeader.Margin = new Padding(4, 0, 4, 0);
            lblMonColHeader.Name = "lblMonColHeader";
            lblMonColHeader.Size = new Size(31, 27);
            lblMonColHeader.TabIndex = 16;
            // 
            // label11
            // 
            label11.Location = new Point(7, 192);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(184, 23);
            label11.TabIndex = 15;
            label11.Text = "Frame Header:";
            label11.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColWrongData
            // 
            butMonColWrongData.Location = new Point(237, 152);
            butMonColWrongData.Margin = new Padding(4, 3, 4, 3);
            butMonColWrongData.Name = "butMonColWrongData";
            butMonColWrongData.Size = new Size(29, 28);
            butMonColWrongData.TabIndex = 14;
            butMonColWrongData.Text = "...";
            butMonColWrongData.UseVisualStyleBackColor = true;
            butMonColWrongData.Click += ButMonColWrongData_Click;
            // 
            // lblMonColWrongData
            // 
            lblMonColWrongData.BorderStyle = BorderStyle.FixedSingle;
            lblMonColWrongData.Location = new Point(198, 152);
            lblMonColWrongData.Margin = new Padding(4, 0, 4, 0);
            lblMonColWrongData.Name = "lblMonColWrongData";
            lblMonColWrongData.Size = new Size(31, 27);
            lblMonColWrongData.TabIndex = 13;
            // 
            // label9
            // 
            label9.Location = new Point(7, 157);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(184, 23);
            label9.TabIndex = 12;
            label9.Text = "Invalid Data:";
            label9.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColData
            // 
            butMonColData.Location = new Point(237, 118);
            butMonColData.Margin = new Padding(4, 3, 4, 3);
            butMonColData.Name = "butMonColData";
            butMonColData.Size = new Size(29, 28);
            butMonColData.TabIndex = 11;
            butMonColData.Text = "...";
            butMonColData.UseVisualStyleBackColor = true;
            butMonColData.Click += ButMonColData_Click;
            // 
            // lblMonColData
            // 
            lblMonColData.BorderStyle = BorderStyle.FixedSingle;
            lblMonColData.Location = new Point(198, 118);
            lblMonColData.Margin = new Padding(4, 0, 4, 0);
            lblMonColData.Name = "lblMonColData";
            lblMonColData.Size = new Size(31, 27);
            lblMonColData.TabIndex = 10;
            // 
            // label7
            // 
            label7.Location = new Point(7, 122);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(184, 23);
            label7.TabIndex = 9;
            label7.Text = "Valid Data:";
            label7.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColScndMaster
            // 
            butMonColScndMaster.Location = new Point(237, 83);
            butMonColScndMaster.Margin = new Padding(4, 3, 4, 3);
            butMonColScndMaster.Name = "butMonColScndMaster";
            butMonColScndMaster.Size = new Size(29, 28);
            butMonColScndMaster.TabIndex = 8;
            butMonColScndMaster.Text = "...";
            butMonColScndMaster.UseVisualStyleBackColor = true;
            butMonColScndMaster.Click += ButMonColScndMaster_Click;
            // 
            // lblMonColScndMaster
            // 
            lblMonColScndMaster.BorderStyle = BorderStyle.FixedSingle;
            lblMonColScndMaster.Location = new Point(198, 83);
            lblMonColScndMaster.Margin = new Padding(4, 0, 4, 0);
            lblMonColScndMaster.Name = "lblMonColScndMaster";
            lblMonColScndMaster.Size = new Size(31, 27);
            lblMonColScndMaster.TabIndex = 7;
            // 
            // label5
            // 
            label5.Location = new Point(7, 88);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(184, 23);
            label5.TabIndex = 6;
            label5.Text = "Secondary Master Delimiter:";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColPrimMaster
            // 
            butMonColPrimMaster.Location = new Point(237, 48);
            butMonColPrimMaster.Margin = new Padding(4, 3, 4, 3);
            butMonColPrimMaster.Name = "butMonColPrimMaster";
            butMonColPrimMaster.Size = new Size(29, 28);
            butMonColPrimMaster.TabIndex = 5;
            butMonColPrimMaster.Text = "...";
            butMonColPrimMaster.UseVisualStyleBackColor = true;
            butMonColPrimMaster.Click += ButMonColPrimMaster_Click;
            // 
            // lblMonColPrimMaster
            // 
            lblMonColPrimMaster.BorderStyle = BorderStyle.FixedSingle;
            lblMonColPrimMaster.Location = new Point(198, 48);
            lblMonColPrimMaster.Margin = new Padding(4, 0, 4, 0);
            lblMonColPrimMaster.Name = "lblMonColPrimMaster";
            lblMonColPrimMaster.Size = new Size(31, 27);
            lblMonColPrimMaster.TabIndex = 4;
            // 
            // label3
            // 
            label3.Location = new Point(7, 53);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(184, 23);
            label3.TabIndex = 3;
            label3.Text = "Primary Master Delimiter:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butMonColBack
            // 
            butMonColBack.Location = new Point(237, 14);
            butMonColBack.Margin = new Padding(4, 3, 4, 3);
            butMonColBack.Name = "butMonColBack";
            butMonColBack.Size = new Size(29, 28);
            butMonColBack.TabIndex = 2;
            butMonColBack.Text = "...";
            butMonColBack.UseVisualStyleBackColor = true;
            butMonColBack.Click += ButSelMonColBk_Click;
            // 
            // lblMonColBack
            // 
            lblMonColBack.BorderStyle = BorderStyle.FixedSingle;
            lblMonColBack.Location = new Point(198, 14);
            lblMonColBack.Margin = new Padding(4, 0, 4, 0);
            lblMonColBack.Name = "lblMonColBack";
            lblMonColBack.Size = new Size(31, 27);
            lblMonColBack.TabIndex = 1;
            // 
            // label1
            // 
            label1.Location = new Point(7, 18);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(184, 23);
            label1.TabIndex = 0;
            label1.Text = "Background:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butOK
            // 
            butOK.Location = new Point(473, 379);
            butOK.Margin = new Padding(4, 3, 4, 3);
            butOK.Name = "butOK";
            butOK.Size = new Size(89, 28);
            butOK.TabIndex = 18;
            butOK.Text = "OK";
            butOK.Click += ButOK_Click;
            // 
            // butCancel
            // 
            butCancel.Location = new Point(196, 379);
            butCancel.Margin = new Padding(4, 3, 4, 3);
            butCancel.Name = "butCancel";
            butCancel.Size = new Size(89, 28);
            butCancel.TabIndex = 19;
            butCancel.Text = "Cancel";
            butCancel.Click += ButCancel_Click;
            // 
            // butApply
            // 
            butApply.Location = new Point(9, 379);
            butApply.Margin = new Padding(4, 3, 4, 3);
            butApply.Name = "butApply";
            butApply.Size = new Size(89, 28);
            butApply.TabIndex = 20;
            butApply.Text = "Apply";
            butApply.Click += ButApply_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(butStatBarColBkRecording);
            groupBox1.Controls.Add(lblStatBarColBkRecording);
            groupBox1.Controls.Add(label37);
            groupBox1.Controls.Add(butStatBarColBkNotRec);
            groupBox1.Controls.Add(lblStatBarColBkNotRec);
            groupBox1.Controls.Add(label39);
            groupBox1.Location = new Point(9, 277);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(276, 96);
            groupBox1.TabIndex = 22;
            groupBox1.TabStop = false;
            groupBox1.Text = "Status Display";
            // 
            // butStatBarColBkRecording
            // 
            butStatBarColBkRecording.Location = new Point(237, 57);
            butStatBarColBkRecording.Margin = new Padding(4, 3, 4, 3);
            butStatBarColBkRecording.Name = "butStatBarColBkRecording";
            butStatBarColBkRecording.Size = new Size(29, 28);
            butStatBarColBkRecording.TabIndex = 8;
            butStatBarColBkRecording.Text = "...";
            butStatBarColBkRecording.UseVisualStyleBackColor = true;
            butStatBarColBkRecording.Click += ButStatBarColBkRecording_Click;
            // 
            // lblStatBarColBkRecording
            // 
            lblStatBarColBkRecording.BorderStyle = BorderStyle.FixedSingle;
            lblStatBarColBkRecording.Location = new Point(198, 57);
            lblStatBarColBkRecording.Margin = new Padding(4, 0, 4, 0);
            lblStatBarColBkRecording.Name = "lblStatBarColBkRecording";
            lblStatBarColBkRecording.Size = new Size(31, 27);
            lblStatBarColBkRecording.TabIndex = 7;
            // 
            // label37
            // 
            label37.Location = new Point(7, 61);
            label37.Margin = new Padding(4, 0, 4, 0);
            label37.Name = "label37";
            label37.Size = new Size(184, 23);
            label37.TabIndex = 6;
            label37.Text = "Backcolor Recording:";
            label37.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butStatBarColBkNotRec
            // 
            butStatBarColBkNotRec.Location = new Point(237, 22);
            butStatBarColBkNotRec.Margin = new Padding(4, 3, 4, 3);
            butStatBarColBkNotRec.Name = "butStatBarColBkNotRec";
            butStatBarColBkNotRec.Size = new Size(29, 28);
            butStatBarColBkNotRec.TabIndex = 5;
            butStatBarColBkNotRec.Text = "...";
            butStatBarColBkNotRec.UseVisualStyleBackColor = true;
            butStatBarColBkNotRec.Click += ButStatBarColBkNotRec_Click;
            // 
            // lblStatBarColBkNotRec
            // 
            lblStatBarColBkNotRec.BorderStyle = BorderStyle.FixedSingle;
            lblStatBarColBkNotRec.Location = new Point(198, 22);
            lblStatBarColBkNotRec.Margin = new Padding(4, 0, 4, 0);
            lblStatBarColBkNotRec.Name = "lblStatBarColBkNotRec";
            lblStatBarColBkNotRec.Size = new Size(31, 27);
            lblStatBarColBkNotRec.TabIndex = 4;
            // 
            // label39
            // 
            label39.Location = new Point(7, 27);
            label39.Margin = new Padding(4, 0, 4, 0);
            label39.Name = "label39";
            label39.Size = new Size(184, 23);
            label39.TabIndex = 3;
            label39.Text = "Backcolor not Recording:";
            label39.TextAlign = ContentAlignment.MiddleRight;
            // 
            // butSetDefault
            // 
            butSetDefault.Location = new Point(473, 335);
            butSetDefault.Margin = new Padding(4, 3, 4, 3);
            butSetDefault.Name = "butSetDefault";
            butSetDefault.Size = new Size(89, 28);
            butSetDefault.TabIndex = 24;
            butSetDefault.Text = "Set Default";
            butSetDefault.Click += ButSetDefault_Click;
            // 
            // FrmSetColors
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(575, 424);
            Controls.Add(butSetDefault);
            Controls.Add(groupBox1);
            Controls.Add(butApply);
            Controls.Add(butCancel);
            Controls.Add(butOK);
            Controls.Add(grpColMonitor);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSetColors";
            ShowInTaskbar = false;
            Text = "Set Colors of Test Client Display";
            Load += FrmSetColors_Load;
            Shown += FrmSetColors_Shown;
            grpColMonitor.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ColorDialog colorDialog;
        private GroupBox grpColMonitor;
        private Label label1;
        private Button butMonColBack;
        private Label lblMonColBack;
        private Button butOK;
        private Button butCancel;
        private Button butApply;
        private Button butMonColTime;
        private Label lblMonColTime;
        private Label label13;
        private Button butMonColHeader;
        private Label lblMonColHeader;
        private Label label11;
        private Button butMonColWrongData;
        private Label lblMonColWrongData;
        private Label label9;
        private Button butMonColData;
        private Label lblMonColData;
        private Label label7;
        private Button butMonColScndMaster;
        private Label lblMonColScndMaster;
        private Label label5;
        private Button butMonColPrimMaster;
        private Label lblMonColPrimMaster;
        private Label label3;
        private Button butMonColDataSel;
        private Label lblMonColDataSel;
        private Label label25;
        private Button butMonColBusyIndication;
        private Label lblMonColBusyIndication;
        private Label label23;
        private Button butMonColError;
        private Label lblMonColError;
        private Label label21;
        private Button butMonColGarbage;
        private Label lblMonColGarbage;
        private Label label19;
        private Button butMonColJabber;
        private Label lblMonColJabber;
        private Label label17;
        private Button butMonColLinNum;
        private Label lblMonColLinNum;
        private Label label15;
        private GroupBox groupBox1;
        private Button butStatBarColBkRecording;
        private Label lblStatBarColBkRecording;
        private Label label37;
        private Button butStatBarColBkNotRec;
        private Label lblStatBarColBkNotRec;
        private Label label39;
        private Button butSetDefault;
    }
}