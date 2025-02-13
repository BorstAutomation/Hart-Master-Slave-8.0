namespace BaTestHart.Forms
{
  partial class FrmCmd31
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
      this.label17 = new System.Windows.Forms.Label();
      this.txtExtCmd = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.txtExtCmdData = new System.Windows.Forms.TextBox();
      this.cmdOK = new System.Windows.Forms.Button();
      this.cmdCancel = new System.Windows.Forms.Button();
      this.butSend = new System.Windows.Forms.Button();
      this.butEdit = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // label17
      // 
      this.label17.Location = new System.Drawing.Point(10, 11);
      this.label17.Name = "label17";
      this.label17.Size = new System.Drawing.Size(71, 21);
      this.label17.TabIndex = 57;
      this.label17.Text = "Command:";
      this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtExtCmd
      // 
      this.txtExtCmd.AcceptsReturn = true;
      this.txtExtCmd.Location = new System.Drawing.Point(87, 12);
      this.txtExtCmd.MaxLength = 8;
      this.txtExtCmd.Name = "txtExtCmd";
      this.txtExtCmd.Size = new System.Drawing.Size(62, 20);
      this.txtExtCmd.TabIndex = 56;
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(155, 11);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(71, 21);
      this.label1.TabIndex = 58;
      this.label1.Text = "(0..65535)";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label2
      // 
      this.label2.Location = new System.Drawing.Point(10, 37);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(71, 21);
      this.label2.TabIndex = 60;
      this.label2.Text = "Data:";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // txtExtCmdData
      // 
      this.txtExtCmdData.AcceptsReturn = true;
      this.txtExtCmdData.Location = new System.Drawing.Point(87, 38);
      this.txtExtCmdData.MaxLength = 10000;
      this.txtExtCmdData.Name = "txtExtCmdData";
      this.txtExtCmdData.Size = new System.Drawing.Size(479, 20);
      this.txtExtCmdData.TabIndex = 59;
      // 
      // cmdOK
      // 
      this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cmdOK.Location = new System.Drawing.Point(498, 71);
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.Size = new System.Drawing.Size(68, 24);
      this.cmdOK.TabIndex = 62;
      this.cmdOK.Text = "OK";
      this.cmdOK.Click += new System.EventHandler(this.CmdOK_Click);
      // 
      // cmdCancel
      // 
      this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cmdCancel.Location = new System.Drawing.Point(12, 71);
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.Size = new System.Drawing.Size(68, 24);
      this.cmdCancel.TabIndex = 61;
      this.cmdCancel.Text = "Cancel";
      this.cmdCancel.Click += new System.EventHandler(this.CmdCancel_Click);
      // 
      // butSend
      // 
      this.butSend.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.butSend.Location = new System.Drawing.Point(226, 71);
      this.butSend.Name = "butSend";
      this.butSend.Size = new System.Drawing.Size(68, 24);
      this.butSend.TabIndex = 64;
      this.butSend.Text = "Send";
      this.butSend.Click += new System.EventHandler(this.ButSend_Click);
      // 
      // butEdit
      // 
      this.butEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.butEdit.Location = new System.Drawing.Point(367, 71);
      this.butEdit.Name = "butEdit";
      this.butEdit.Size = new System.Drawing.Size(68, 24);
      this.butEdit.TabIndex = 63;
      this.butEdit.Text = "Edit";
      this.butEdit.Click += new System.EventHandler(this.ButEdit_Click);
      // 
      // FrmCmd31
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(578, 103);
      this.Controls.Add(this.butSend);
      this.Controls.Add(this.butEdit);
      this.Controls.Add(this.cmdOK);
      this.Controls.Add(this.cmdCancel);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.txtExtCmdData);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.label17);
      this.Controls.Add(this.txtExtCmd);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FrmCmd31";
      this.ShowInTaskbar = false;
      this.Text = "Hart Extended Command";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmExtCmd_FormClosing);
      this.Load += new System.EventHandler(this.FrmExtCmd_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label17;
    private System.Windows.Forms.TextBox txtExtCmd;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox txtExtCmdData;
    private System.Windows.Forms.Button cmdOK;
    private System.Windows.Forms.Button cmdCancel;
    private System.Windows.Forms.Button butSend;
    private System.Windows.Forms.Button butEdit;
  }
}