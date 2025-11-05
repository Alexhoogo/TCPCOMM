namespace TCPCOM
{
    partial class ucKeyencePLC
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 清理自定义资源
                CloseConnection();
                
                // 清理组件
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.lblRemotePort = new System.Windows.Forms.Label();
            this.lblRemoteIP = new System.Windows.Forms.Label();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.groupBoxReceive = new System.Windows.Forms.GroupBox();
            this.listBoxReceive = new System.Windows.Forms.ListBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBoxRead = new System.Windows.Forms.GroupBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbRegisterType = new System.Windows.Forms.ComboBox();
            this.groupBoxWrite = new System.Windows.Forms.GroupBox();
            this.btnWrite = new System.Windows.Forms.Button();
            this.txtWriteValue = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBoxStatus.SuspendLayout();
            this.groupBoxReceive.SuspendLayout();
            this.groupBoxRead.SuspendLayout();
            this.groupBoxWrite.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.lblRemotePort);
            this.groupBoxStatus.Controls.Add(this.lblRemoteIP);
            this.groupBoxStatus.Controls.Add(this.lblLocalPort);
            this.groupBoxStatus.Controls.Add(this.lblStatus);
            this.groupBoxStatus.Controls.Add(this.label1);
            this.groupBoxStatus.Controls.Add(this.btnDisconnect);
            this.groupBoxStatus.Controls.Add(this.btnConnect);
            this.groupBoxStatus.Location = new System.Drawing.Point(0, 0);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(596, 60);
            this.groupBoxStatus.TabIndex = 0;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "PLC连接状态";
            // 
            // lblRemotePort
            // 
            this.lblRemotePort.AutoSize = true;
            this.lblRemotePort.Location = new System.Drawing.Point(272, 18);
            this.lblRemotePort.Name = "lblRemotePort";
            this.lblRemotePort.Size = new System.Drawing.Size(59, 12);
            this.lblRemotePort.TabIndex = 11;
            this.lblRemotePort.Text = "PLC端口:";
            // 
            // lblRemoteIP
            // 
            this.lblRemoteIP.AutoSize = true;
            this.lblRemoteIP.Location = new System.Drawing.Point(145, 24);
            this.lblRemoteIP.Name = "lblRemoteIP";
            this.lblRemoteIP.Size = new System.Drawing.Size(41, 12);
            this.lblRemoteIP.TabIndex = 10;
            this.lblRemoteIP.Text = "PLC IP:";
            // 
            // lblLocalPort
            // 
            this.lblLocalPort.AutoSize = true;
            this.lblLocalPort.Location = new System.Drawing.Point(266, 37);
            this.lblLocalPort.Name = "lblLocalPort";
            this.lblLocalPort.Size = new System.Drawing.Size(65, 12);
            this.lblLocalPort.TabIndex = 9;
            this.lblLocalPort.Text = "本地端口:0";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(80, 18);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(58, 22);
            this.lblStatus.TabIndex = 8;
            this.lblStatus.Text = "已断开";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "连接状态：";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnect.ForeColor = System.Drawing.Color.White;
            this.btnDisconnect.Location = new System.Drawing.Point(517, 20);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(69, 25);
            this.btnDisconnect.TabIndex = 6;
            this.btnDisconnect.Text = "断开";
            this.btnDisconnect.UseVisualStyleBackColor = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(442, 20);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(69, 25);
            this.btnConnect.TabIndex = 5;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1000, -1000);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "IP端口：";
            this.label2.Visible = false;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(-1000, -1000);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(125, 21);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "8501";
            this.txtPort.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1000, -1000);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "IP地址：";
            this.label3.Visible = false;
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(-1000, -1000);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(125, 21);
            this.txtIPAddress.TabIndex = 1;
            this.txtIPAddress.Text = "192.168.0.10";
            this.txtIPAddress.Visible = false;
            // 
            // groupBoxReceive
            // 
            this.groupBoxReceive.Controls.Add(this.listBoxReceive);
            this.groupBoxReceive.Controls.Add(this.btnClear);
            this.groupBoxReceive.Location = new System.Drawing.Point(0, 60);
            this.groupBoxReceive.Name = "groupBoxReceive";
            this.groupBoxReceive.Size = new System.Drawing.Size(596, 154);
            this.groupBoxReceive.TabIndex = 1;
            this.groupBoxReceive.TabStop = false;
            this.groupBoxReceive.Text = "数据接收及提示窗口";
            // 
            // listBoxReceive
            // 
            this.listBoxReceive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxReceive.FormattingEnabled = true;
            this.listBoxReceive.ItemHeight = 12;
            this.listBoxReceive.Location = new System.Drawing.Point(6, 20);
            this.listBoxReceive.Name = "listBoxReceive";
            this.listBoxReceive.Size = new System.Drawing.Size(505, 124);
            this.listBoxReceive.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(517, 20);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(70, 125);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // groupBoxRead
            // 
            this.groupBoxRead.Controls.Add(this.btnRead);
            this.groupBoxRead.Controls.Add(this.txtCount);
            this.groupBoxRead.Controls.Add(this.label5);
            this.groupBoxRead.Controls.Add(this.txtAddress);
            this.groupBoxRead.Controls.Add(this.label4);
            this.groupBoxRead.Controls.Add(this.cmbRegisterType);
            this.groupBoxRead.Location = new System.Drawing.Point(0, 220);
            this.groupBoxRead.Name = "groupBoxRead";
            this.groupBoxRead.Size = new System.Drawing.Size(296, 130);
            this.groupBoxRead.TabIndex = 2;
            this.groupBoxRead.TabStop = false;
            this.groupBoxRead.Text = "读取寄存器";
            // 
            // btnRead
            // 
            this.btnRead.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.btnRead.Enabled = false;
            this.btnRead.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRead.ForeColor = System.Drawing.Color.White;
            this.btnRead.Location = new System.Drawing.Point(6, 85);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(280, 35);
            this.btnRead.TabIndex = 5;
            this.btnRead.Text = "读取";
            this.btnRead.UseVisualStyleBackColor = false;
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(200, 50);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(86, 21);
            this.txtCount.TabIndex = 4;
            this.txtCount.Text = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(150, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "数量(字):";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(80, 50);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(64, 21);
            this.txtAddress.TabIndex = 2;
            this.txtAddress.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "地址：";
            // 
            // cmbRegisterType
            // 
            this.cmbRegisterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRegisterType.FormattingEnabled = true;
            this.cmbRegisterType.Location = new System.Drawing.Point(6, 20);
            this.cmbRegisterType.Name = "cmbRegisterType";
            this.cmbRegisterType.Size = new System.Drawing.Size(280, 20);
            this.cmbRegisterType.TabIndex = 0;
            // 
            // groupBoxWrite
            // 
            this.groupBoxWrite.Controls.Add(this.btnWrite);
            this.groupBoxWrite.Controls.Add(this.txtWriteValue);
            this.groupBoxWrite.Controls.Add(this.label7);
            this.groupBoxWrite.Location = new System.Drawing.Point(302, 220);
            this.groupBoxWrite.Name = "groupBoxWrite";
            this.groupBoxWrite.Size = new System.Drawing.Size(294, 130);
            this.groupBoxWrite.TabIndex = 3;
            this.groupBoxWrite.TabStop = false;
            this.groupBoxWrite.Text = "写入寄存器";
            // 
            // btnWrite
            // 
            this.btnWrite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnWrite.Enabled = false;
            this.btnWrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWrite.ForeColor = System.Drawing.Color.White;
            this.btnWrite.Location = new System.Drawing.Point(6, 85);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(280, 35);
            this.btnWrite.TabIndex = 2;
            this.btnWrite.Text = "写入";
            this.btnWrite.UseVisualStyleBackColor = false;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // txtWriteValue
            // 
            this.txtWriteValue.Location = new System.Drawing.Point(6, 40);
            this.txtWriteValue.Multiline = true;
            this.txtWriteValue.Name = "txtWriteValue";
            this.txtWriteValue.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtWriteValue.Size = new System.Drawing.Size(280, 40);
            this.txtWriteValue.TabIndex = 1;
            this.txtWriteValue.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(197, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "写入值(多个值用逗号或空格分隔):";
            // 
            // ucKeyencePLC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxWrite);
            this.Controls.Add(this.groupBoxRead);
            this.Controls.Add(this.groupBoxStatus);
            this.Controls.Add(this.groupBoxReceive);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Name = "ucKeyencePLC";
            this.Size = new System.Drawing.Size(596, 350);
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            this.groupBoxReceive.ResumeLayout(false);
            this.groupBoxRead.ResumeLayout(false);
            this.groupBoxRead.PerformLayout();
            this.groupBoxWrite.ResumeLayout(false);
            this.groupBoxWrite.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.GroupBox groupBoxReceive;
        private System.Windows.Forms.ListBox listBoxReceive;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBoxRead;
        private System.Windows.Forms.ComboBox cmbRegisterType;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.GroupBox groupBoxWrite;
        private System.Windows.Forms.TextBox txtWriteValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Label lblLocalPort;
        private System.Windows.Forms.Label lblRemoteIP;
        private System.Windows.Forms.Label lblRemotePort;
    }
}

