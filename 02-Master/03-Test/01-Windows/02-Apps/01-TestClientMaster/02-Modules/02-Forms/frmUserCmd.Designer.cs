namespace BaTestHart
{
    partial class FrmUserCmd
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
            txtCommand = new TextBox();
            label3 = new Label();
            label1 = new Label();
            txtData = new TextBox();
            butCancel = new Button();
            butOK = new Button();
            label2 = new Label();
            butEdit = new Button();
            butSend = new Button();
            SuspendLayout();
            // 
            // txtCommand
            // 
            txtCommand.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtCommand.Location = new Point(86, 5);
            txtCommand.MaxLength = 3;
            txtCommand.Name = "txtCommand";
            txtCommand.Size = new Size(44, 21);
            txtCommand.TabIndex = 7;
            txtCommand.Text = "0";
            txtCommand.TextAlign = HorizontalAlignment.Right;
            // 
            // label3
            // 
            label3.Location = new Point(10, 10);
            label3.Name = "label3";
            label3.Size = new Size(76, 20);
            label3.TabIndex = 6;
            label3.Text = "Command:";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Location = new Point(10, 44);
            label1.Name = "label1";
            label1.Size = new Size(76, 20);
            label1.TabIndex = 8;
            label1.Text = "Data:";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtData
            // 
            txtData.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtData.Location = new Point(86, 39);
            txtData.MaxLength = 1000;
            txtData.Name = "txtData";
            txtData.Size = new Size(485, 21);
            txtData.TabIndex = 9;
            // 
            // butCancel
            // 
            butCancel.FlatStyle = FlatStyle.Popup;
            butCancel.Location = new Point(10, 79);
            butCancel.Name = "butCancel";
            butCancel.Size = new Size(81, 29);
            butCancel.TabIndex = 13;
            butCancel.Text = "Cancel";
            butCancel.Click += ButCancel_Click;
            // 
            // butOK
            // 
            butOK.FlatStyle = FlatStyle.Popup;
            butOK.Location = new Point(490, 79);
            butOK.Name = "butOK";
            butOK.Size = new Size(81, 29);
            butOK.TabIndex = 12;
            butOK.Text = "OK";
            butOK.Click += ButOK_Click;
            // 
            // label2
            // 
            label2.Location = new Point(137, 7);
            label2.Name = "label2";
            label2.Size = new Size(77, 20);
            label2.TabIndex = 14;
            label2.Text = "(0..255)";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // butEdit
            // 
            butEdit.FlatStyle = FlatStyle.Popup;
            butEdit.Location = new Point(332, 79);
            butEdit.Name = "butEdit";
            butEdit.Size = new Size(82, 29);
            butEdit.TabIndex = 15;
            butEdit.Text = "Edit";
            butEdit.Click += ButEdit_Click;
            // 
            // butSend
            // 
            butSend.FlatStyle = FlatStyle.Popup;
            butSend.Location = new Point(163, 79);
            butSend.Name = "butSend";
            butSend.Size = new Size(82, 29);
            butSend.TabIndex = 16;
            butSend.Text = "Send";
            butSend.Click += ButSend_Click;
            // 
            // FrmUserCmd
            // 
            AutoScaleBaseSize = new Size(6, 16);
            ClientSize = new Size(584, 121);
            Controls.Add(butSend);
            Controls.Add(butEdit);
            Controls.Add(label2);
            Controls.Add(butCancel);
            Controls.Add(butOK);
            Controls.Add(txtData);
            Controls.Add(label1);
            Controls.Add(txtCommand);
            Controls.Add(label3);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmUserCmd";
            Text = "Send an Individually Specified Command";
            FormClosing += FrmUserCmd_FormClosing;
            Load += FrmAnyCmd_Load;
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        internal TextBox txtCommand;
        private Label label3;
        private Label label1;
        internal TextBox txtData;
        private Button butCancel;
        private Button butOK;
        private Label label2;
        private Button butEdit;
        private Button butSend;

    }
}
