namespace TCPCOM
{
    partial class Tool_SocketServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Tool_SocketServer));
            this.startserverbtn = new System.Windows.Forms.Button();
            this.clearinfo = new System.Windows.Forms.Button();
            this.sendsingle = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.clientlist = new System.Windows.Forms.ComboBox();
            this.infobox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.message = new System.Windows.Forms.TextBox();
            this.sendgroup = new System.Windows.Forms.Button();
            this.ipaddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ipport = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
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
            // startserverbtn
            // 
            this.startserverbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.startserverbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startserverbtn.ForeColor = System.Drawing.Color.White;
            this.startserverbtn.Location = new System.Drawing.Point(460, 10);
            this.startserverbtn.Name = "startserverbtn";
            this.startserverbtn.Size = new System.Drawing.Size(135, 59);
            this.startserverbtn.TabIndex = 1;
            this.startserverbtn.Text = "启动服务器";
            this.startserverbtn.UseVisualStyleBackColor = false;
            this.startserverbtn.Click += new System.EventHandler(this.startserverbtn_Click);
            // 
            // clearinfo
            // 
            this.clearinfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.clearinfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.clearinfo.ForeColor = System.Drawing.Color.White;
            this.clearinfo.Location = new System.Drawing.Point(462, 238);
            this.clearinfo.Name = "clearinfo";
            this.clearinfo.Size = new System.Drawing.Size(135, 43);
            this.clearinfo.TabIndex = 18;
            this.clearinfo.Text = "清空消息";
            this.clearinfo.UseVisualStyleBackColor = false;
            this.clearinfo.Click += new System.EventHandler(this.clearinfo_Click);
            // 
            // sendsingle
            // 
            this.sendsingle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.sendsingle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendsingle.ForeColor = System.Drawing.Color.White;
            this.sendsingle.Location = new System.Drawing.Point(460, 79);
            this.sendsingle.Name = "sendsingle";
            this.sendsingle.Size = new System.Drawing.Size(135, 47);
            this.sendsingle.TabIndex = 17;
            this.sendsingle.Text = "单发消息";
            this.sendsingle.UseVisualStyleBackColor = false;
            this.sendsingle.Click += new System.EventHandler(this.sendsingle_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 16;
            this.label3.Text = "客户端集合：";
            // 
            // clientlist
            // 
            this.clientlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clientlist.FormattingEnabled = true;
            this.clientlist.Location = new System.Drawing.Point(87, 45);
            this.clientlist.Name = "clientlist";
            this.clientlist.Size = new System.Drawing.Size(369, 20);
            this.clientlist.TabIndex = 15;
            // 
            // infobox
            // 
            this.infobox.FormattingEnabled = true;
            this.infobox.ItemHeight = 12;
            this.infobox.Location = new System.Drawing.Point(6, 109);
            this.infobox.Name = "infobox";
            this.infobox.Size = new System.Drawing.Size(450, 172);
            this.infobox.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "发送信息：";
            // 
            // message
            // 
            this.message.Location = new System.Drawing.Point(75, 79);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(381, 21);
            this.message.TabIndex = 11;
            // 
            // sendgroup
            // 
            this.sendgroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.sendgroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendgroup.ForeColor = System.Drawing.Color.White;
            this.sendgroup.Location = new System.Drawing.Point(460, 132);
            this.sendgroup.Name = "sendgroup";
            this.sendgroup.Size = new System.Drawing.Size(135, 45);
            this.sendgroup.TabIndex = 10;
            this.sendgroup.Text = "群发消息";
            this.sendgroup.UseVisualStyleBackColor = false;
            this.sendgroup.Click += new System.EventHandler(this.sendgroup_Click);
            // 
            // ipaddress
            // 
            this.ipaddress.Location = new System.Drawing.Point(87, 12);
            this.ipaddress.Name = "ipaddress";
            this.ipaddress.Size = new System.Drawing.Size(149, 21);
            this.ipaddress.TabIndex = 11;
            this.ipaddress.Text = "127.0.0.1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "IP地址：";
            // 
            // ipport
            // 
            this.ipport.Location = new System.Drawing.Point(307, 12);
            this.ipport.Name = "ipport";
            this.ipport.Size = new System.Drawing.Size(149, 21);
            this.ipport.TabIndex = 11;
            this.ipport.Text = "9000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(248, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "IP端口：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 289);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "时间间隔(ms)：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(200, 289);
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
            this.comboBoxInterval.Location = new System.Drawing.Point(87, 286);
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
            this.comboBoxCount.Location = new System.Drawing.Point(271, 286);
            this.comboBoxCount.Name = "comboBoxCount";
            this.comboBoxCount.Size = new System.Drawing.Size(112, 20);
            this.comboBoxCount.TabIndex = 22;
            // 
            // btnStartTimer
            // 
            this.btnStartTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.btnStartTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartTimer.ForeColor = System.Drawing.Color.White;
            this.btnStartTimer.Location = new System.Drawing.Point(389, 284);
            this.btnStartTimer.Name = "btnStartTimer";
            this.btnStartTimer.Size = new System.Drawing.Size(65, 25);
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
            this.btnPauseTimer.Location = new System.Drawing.Point(460, 284);
            this.btnPauseTimer.Name = "btnPauseTimer";
            this.btnPauseTimer.Size = new System.Drawing.Size(65, 25);
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
            this.btnCancelTimer.Location = new System.Drawing.Point(532, 284);
            this.btnCancelTimer.Name = "btnCancelTimer";
            this.btnCancelTimer.Size = new System.Drawing.Size(65, 25);
            this.btnCancelTimer.TabIndex = 25;
            this.btnCancelTimer.Text = "取消";
            this.btnCancelTimer.UseVisualStyleBackColor = false;
            this.btnCancelTimer.Click += new System.EventHandler(this.btnCancelTimer_Click);
            // 
            // timerSend
            // 
            this.timerSend.Tick += new System.EventHandler(this.timerSend_Tick);
            // 
            // Tool_SocketServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 314);
            this.Controls.Add(this.btnCancelTimer);
            this.Controls.Add(this.btnPauseTimer);
            this.Controls.Add(this.btnStartTimer);
            this.Controls.Add(this.comboBoxCount);
            this.Controls.Add(this.comboBoxInterval);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.clearinfo);
            this.Controls.Add(this.sendsingle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clientlist);
            this.Controls.Add(this.infobox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ipport);
            this.Controls.Add(this.ipaddress);
            this.Controls.Add(this.message);
            this.Controls.Add(this.sendgroup);
            this.Controls.Add(this.startserverbtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Tool_SocketServer";
            this.Text = "Socket 客户端";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Tool_SocketServer_FormClosing);
            this.Load += new System.EventHandler(this.Tool_SocketServer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startserverbtn;
        private System.Windows.Forms.Button clearinfo;
        private System.Windows.Forms.Button sendsingle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox clientlist;
        private System.Windows.Forms.ListBox infobox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox message;
        private System.Windows.Forms.Button sendgroup;
        private System.Windows.Forms.TextBox ipaddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ipport;
        private System.Windows.Forms.Label label2;
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