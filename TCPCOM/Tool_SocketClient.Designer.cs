namespace TCPCOM
{
    partial class Tool_SocketClient
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tool_SocketClient));
            this.disconserver = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.infobox = new System.Windows.Forms.ListBox();
            this.conserver = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.message = new System.Windows.Forms.TextBox();
            this.sendinfo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ipport = new System.Windows.Forms.TextBox();
            this.ipaddress = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxInterval = new System.Windows.Forms.ComboBox();
            this.comboBoxCount = new System.Windows.Forms.ComboBox();
            this.btnStartTimer = new System.Windows.Forms.Button();
            this.btnPauseTimer = new System.Windows.Forms.Button();
            this.btnCancelTimer = new System.Windows.Forms.Button();
            this.timerSend = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // disconserver
            // 
            this.disconserver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.disconserver.Enabled = false;
            this.disconserver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.disconserver.ForeColor = System.Drawing.Color.White;
            this.disconserver.Location = new System.Drawing.Point(310, 39);
            this.disconserver.Name = "disconserver";
            this.disconserver.Size = new System.Drawing.Size(270, 52);
            this.disconserver.TabIndex = 13;
            this.disconserver.Text = "断开连接";
            this.disconserver.UseVisualStyleBackColor = false;
            this.disconserver.Click += new System.EventHandler(this.disconserver_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "当前信息：";
            // 
            // infobox
            // 
            this.infobox.FormattingEnabled = true;
            this.infobox.ItemHeight = 12;
            this.infobox.Location = new System.Drawing.Point(3, 153);
            this.infobox.Name = "infobox";
            this.infobox.Size = new System.Drawing.Size(577, 208);
            this.infobox.TabIndex = 11;
            // 
            // conserver
            // 
            this.conserver.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.conserver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.conserver.ForeColor = System.Drawing.Color.White;
            this.conserver.Location = new System.Drawing.Point(5, 40);
            this.conserver.Name = "conserver";
            this.conserver.Size = new System.Drawing.Size(299, 52);
            this.conserver.TabIndex = 10;
            this.conserver.Text = "连接服务器";
            this.conserver.UseVisualStyleBackColor = false;
            this.conserver.Click += new System.EventHandler(this.conserver_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "发送信息：";
            // 
            // message
            // 
            this.message.Location = new System.Drawing.Point(74, 109);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(382, 21);
            this.message.TabIndex = 8;
            // 
            // sendinfo
            // 
            this.sendinfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.sendinfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendinfo.ForeColor = System.Drawing.Color.White;
            this.sendinfo.Location = new System.Drawing.Point(462, 102);
            this.sendinfo.Name = "sendinfo";
            this.sendinfo.Size = new System.Drawing.Size(118, 33);
            this.sendinfo.TabIndex = 7;
            this.sendinfo.Text = "发送消息";
            this.sendinfo.UseVisualStyleBackColor = false;
            this.sendinfo.Click += new System.EventHandler(this.sendinfo_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(269, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "IP端口：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "IP地址：";
            // 
            // ipport
            // 
            this.ipport.Location = new System.Drawing.Point(328, 12);
            this.ipport.Name = "ipport";
            this.ipport.Size = new System.Drawing.Size(252, 21);
            this.ipport.TabIndex = 14;
            this.ipport.Text = "9005";
            // 
            // ipaddress
            // 
            this.ipaddress.Location = new System.Drawing.Point(74, 12);
            this.ipaddress.Name = "ipaddress";
            this.ipaddress.Size = new System.Drawing.Size(182, 21);
            this.ipaddress.TabIndex = 15;
            this.ipaddress.Text = "127.0.0.1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 370);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "时间间隔(ms)：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(199, 370);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "发送次数：";
            // 
            // comboBoxInterval
            // 
            this.comboBoxInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInterval.FormattingEnabled = true;
            this.comboBoxInterval.Items.AddRange(new object[] {
            "10",
            "100",
            "200",
            "500",
            "1000",
            "2000"});
            this.comboBoxInterval.Location = new System.Drawing.Point(86, 367);
            this.comboBoxInterval.Name = "comboBoxInterval";
            this.comboBoxInterval.Size = new System.Drawing.Size(100, 20);
            this.comboBoxInterval.TabIndex = 21;
            // 
            // comboBoxCount
            // 
            this.comboBoxCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCount.FormattingEnabled = true;
            this.comboBoxCount.Items.AddRange(new object[] {
            "1",
            "10",
            "100",
            "1000",
            "2000",
            "5000",
            "10000"});
            this.comboBoxCount.Location = new System.Drawing.Point(270, 367);
            this.comboBoxCount.Name = "comboBoxCount";
            this.comboBoxCount.Size = new System.Drawing.Size(100, 20);
            this.comboBoxCount.TabIndex = 22;
            // 
            // btnStartTimer
            // 
            this.btnStartTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.btnStartTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartTimer.ForeColor = System.Drawing.Color.White;
            this.btnStartTimer.Location = new System.Drawing.Point(376, 365);
            this.btnStartTimer.Name = "btnStartTimer";
            this.btnStartTimer.Size = new System.Drawing.Size(60, 25);
            this.btnStartTimer.TabIndex = 23;
            this.btnStartTimer.Text = "开始";
            this.btnStartTimer.UseVisualStyleBackColor = false;
            this.btnStartTimer.Click += new System.EventHandler(this.btnStartTimer_Click);
            // 
            // btnPauseTimer
            // 
            this.btnPauseTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.btnPauseTimer.Enabled = false;
            this.btnPauseTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPauseTimer.ForeColor = System.Drawing.Color.White;
            this.btnPauseTimer.Location = new System.Drawing.Point(442, 365);
            this.btnPauseTimer.Name = "btnPauseTimer";
            this.btnPauseTimer.Size = new System.Drawing.Size(60, 25);
            this.btnPauseTimer.TabIndex = 24;
            this.btnPauseTimer.Text = "暂停";
            this.btnPauseTimer.UseVisualStyleBackColor = false;
            this.btnPauseTimer.Click += new System.EventHandler(this.btnPauseTimer_Click);
            // 
            // btnCancelTimer
            // 
            this.btnCancelTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.btnCancelTimer.Enabled = false;
            this.btnCancelTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelTimer.ForeColor = System.Drawing.Color.White;
            this.btnCancelTimer.Location = new System.Drawing.Point(508, 365);
            this.btnCancelTimer.Name = "btnCancelTimer";
            this.btnCancelTimer.Size = new System.Drawing.Size(72, 25);
            this.btnCancelTimer.TabIndex = 25;
            this.btnCancelTimer.Text = "取消";
            this.btnCancelTimer.UseVisualStyleBackColor = false;
            this.btnCancelTimer.Click += new System.EventHandler(this.btnCancelTimer_Click);
            // 
            // timerSend
            // 
            this.timerSend.Tick += new System.EventHandler(this.timerSend_Tick);
            // 
            // Tool_SocketClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 393);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ipport);
            this.Controls.Add(this.ipaddress);
            this.Controls.Add(this.disconserver);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.infobox);
            this.Controls.Add(this.conserver);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.message);
            this.Controls.Add(this.sendinfo);
            this.Controls.Add(this.btnCancelTimer);
            this.Controls.Add(this.btnPauseTimer);
            this.Controls.Add(this.btnStartTimer);
            this.Controls.Add(this.comboBoxCount);
            this.Controls.Add(this.comboBoxInterval);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Tool_SocketClient";
            this.Text = "Socket 客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tool_SocketClient_FormClosing);
            this.Load += new System.EventHandler(this.Tool_SocketClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button disconserver;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox infobox;
        private System.Windows.Forms.Button conserver;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox message;
        private System.Windows.Forms.Button sendinfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ipport;
        private System.Windows.Forms.TextBox ipaddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxInterval;
        private System.Windows.Forms.ComboBox comboBoxCount;
        private System.Windows.Forms.Button btnStartTimer;
        private System.Windows.Forms.Button btnPauseTimer;
        private System.Windows.Forms.Button btnCancelTimer;
        private System.Windows.Forms.Timer timerSend;
    }
}