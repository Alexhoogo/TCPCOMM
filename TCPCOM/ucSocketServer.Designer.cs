namespace TCPCOM
{
    partial class ucSocketServer
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
                StopListening();
                
                // 停止定时发送
                if (timerSend != null)
                {
                    timerSend.Stop();
                    timerSend.Dispose();
                }
                
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
            this.components = new System.ComponentModel.Container();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.lblLocalPort = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIPAddress = new System.Windows.Forms.TextBox();
            this.groupBoxReceive = new System.Windows.Forms.GroupBox();
            this.listBoxReceive = new System.Windows.Forms.ListBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.groupBoxSend = new System.Windows.Forms.GroupBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.btnSendGroup = new System.Windows.Forms.Button();
            this.groupBoxTimer = new System.Windows.Forms.GroupBox();
            this.numInterval = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxCount = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnStopTimer = new System.Windows.Forms.Button();
            this.btnPauseTimer = new System.Windows.Forms.Button();
            this.btnStartTimer = new System.Windows.Forms.Button();
            this.timerSend = new System.Windows.Forms.Timer(this.components);
            this.groupBoxStatus.SuspendLayout();
            this.groupBoxReceive.SuspendLayout();
            this.groupBoxSend.SuspendLayout();
            this.groupBoxTimer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Controls.Add(this.lblLocalPort);
            this.groupBoxStatus.Controls.Add(this.lblStatus);
            this.groupBoxStatus.Controls.Add(this.label1);
            this.groupBoxStatus.Controls.Add(this.btnStop);
            this.groupBoxStatus.Controls.Add(this.btnStart);
            this.groupBoxStatus.Location = new System.Drawing.Point(0, 0);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(596, 60);
            this.groupBoxStatus.TabIndex = 0;
            this.groupBoxStatus.TabStop = false;
            this.groupBoxStatus.Text = "Socket状态";
            // 
            // lblLocalPort
            // 
            this.lblLocalPort.AutoSize = true;
            this.lblLocalPort.Location = new System.Drawing.Point(166, 27);
            this.lblLocalPort.Name = "lblLocalPort";
            this.lblLocalPort.Size = new System.Drawing.Size(59, 12);
            this.lblLocalPort.TabIndex = 10;
            this.lblLocalPort.Text = "本地端口:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Red;
            this.lblStatus.Location = new System.Drawing.Point(91, 21);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(58, 22);
            this.lblStatus.TabIndex = 9;
            this.lblStatus.Text = "已停止";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "服务器状态：";
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnStop.Enabled = false;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(515, 21);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(72, 22);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "停止监听";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(427, 21);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(72, 22);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "启动监听";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-1000, -1000);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "IP端口：";
            this.label2.Visible = false;
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(-1000, -1000);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(125, 21);
            this.txtPort.TabIndex = 4;
            this.txtPort.Text = "60000";
            this.txtPort.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-1000, -1000);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "IP地址：";
            this.label3.Visible = false;
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.Location = new System.Drawing.Point(-1000, -1000);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.Size = new System.Drawing.Size(125, 21);
            this.txtIPAddress.TabIndex = 2;
            this.txtIPAddress.Text = "0.0.0.0";
            this.txtIPAddress.Visible = false;
            // 
            // groupBoxReceive
            // 
            this.groupBoxReceive.Controls.Add(this.listBoxReceive);
            this.groupBoxReceive.Controls.Add(this.btnClear);
            this.groupBoxReceive.Location = new System.Drawing.Point(0, 60);
            this.groupBoxReceive.Name = "groupBoxReceive";
            this.groupBoxReceive.Size = new System.Drawing.Size(596, 154);
            this.groupBoxReceive.TabIndex = 2;
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
            // groupBoxSend
            // 
            this.groupBoxSend.Controls.Add(this.txtMessage);
            this.groupBoxSend.Controls.Add(this.btnSendGroup);
            this.groupBoxSend.Location = new System.Drawing.Point(0, 220);
            this.groupBoxSend.Name = "groupBoxSend";
            this.groupBoxSend.Size = new System.Drawing.Size(596, 156);
            this.groupBoxSend.TabIndex = 3;
            this.groupBoxSend.TabStop = false;
            this.groupBoxSend.Text = "数据发送窗口(文本模式)";
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(6, 20);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMessage.Size = new System.Drawing.Size(505, 126);
            this.txtMessage.TabIndex = 3;
            // 
            // btnSendGroup
            // 
            this.btnSendGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(141)))), ((int)(((byte)(230)))));
            this.btnSendGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendGroup.ForeColor = System.Drawing.Color.White;
            this.btnSendGroup.Location = new System.Drawing.Point(517, 20);
            this.btnSendGroup.Name = "btnSendGroup";
            this.btnSendGroup.Size = new System.Drawing.Size(70, 126);
            this.btnSendGroup.TabIndex = 1;
            this.btnSendGroup.Text = "群发消息";
            this.btnSendGroup.UseVisualStyleBackColor = false;
            this.btnSendGroup.Click += new System.EventHandler(this.btnSendGroup_Click);
            // 
            // groupBoxTimer
            // 
            this.groupBoxTimer.Controls.Add(this.numInterval);
            this.groupBoxTimer.Controls.Add(this.label8);
            this.groupBoxTimer.Controls.Add(this.comboBoxCount);
            this.groupBoxTimer.Controls.Add(this.label6);
            this.groupBoxTimer.Controls.Add(this.btnStopTimer);
            this.groupBoxTimer.Controls.Add(this.btnPauseTimer);
            this.groupBoxTimer.Controls.Add(this.btnStartTimer);
            this.groupBoxTimer.Location = new System.Drawing.Point(0, 382);
            this.groupBoxTimer.Name = "groupBoxTimer";
            this.groupBoxTimer.Size = new System.Drawing.Size(596, 53);
            this.groupBoxTimer.TabIndex = 4;
            this.groupBoxTimer.TabStop = false;
            this.groupBoxTimer.Text = "定时发送设置";
            // 
            // numInterval
            // 
            this.numInterval.Location = new System.Drawing.Point(95, 20);
            this.numInterval.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.numInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInterval.Name = "numInterval";
            this.numInterval.Size = new System.Drawing.Size(74, 21);
            this.numInterval.TabIndex = 9;
            this.numInterval.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 12);
            this.label8.TabIndex = 8;
            this.label8.Text = "间隔时间(ms):";
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
            this.comboBoxCount.Location = new System.Drawing.Point(249, 21);
            this.comboBoxCount.Name = "comboBoxCount";
            this.comboBoxCount.Size = new System.Drawing.Size(118, 20);
            this.comboBoxCount.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(178, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "发送次数：";
            // 
            // btnStopTimer
            // 
            this.btnStopTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btnStopTimer.Enabled = false;
            this.btnStopTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopTimer.ForeColor = System.Drawing.Color.White;
            this.btnStopTimer.Location = new System.Drawing.Point(517, 17);
            this.btnStopTimer.Name = "btnStopTimer";
            this.btnStopTimer.Size = new System.Drawing.Size(66, 27);
            this.btnStopTimer.TabIndex = 3;
            this.btnStopTimer.Text = "停止";
            this.btnStopTimer.UseVisualStyleBackColor = false;
            this.btnStopTimer.Click += new System.EventHandler(this.btnStopTimer_Click);
            // 
            // btnPauseTimer
            // 
            this.btnPauseTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnPauseTimer.Enabled = false;
            this.btnPauseTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPauseTimer.ForeColor = System.Drawing.Color.White;
            this.btnPauseTimer.Location = new System.Drawing.Point(445, 17);
            this.btnPauseTimer.Name = "btnPauseTimer";
            this.btnPauseTimer.Size = new System.Drawing.Size(66, 26);
            this.btnPauseTimer.TabIndex = 2;
            this.btnPauseTimer.Text = "暂停";
            this.btnPauseTimer.UseVisualStyleBackColor = false;
            this.btnPauseTimer.Click += new System.EventHandler(this.btnPauseTimer_Click);
            // 
            // btnStartTimer
            // 
            this.btnStartTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnStartTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartTimer.ForeColor = System.Drawing.Color.White;
            this.btnStartTimer.Location = new System.Drawing.Point(373, 18);
            this.btnStartTimer.Name = "btnStartTimer";
            this.btnStartTimer.Size = new System.Drawing.Size(66, 25);
            this.btnStartTimer.TabIndex = 1;
            this.btnStartTimer.Text = "开始";
            this.btnStartTimer.UseVisualStyleBackColor = false;
            this.btnStartTimer.Click += new System.EventHandler(this.btnStartTimer_Click);
            // 
            // timerSend
            // 
            this.timerSend.Tick += new System.EventHandler(this.timerSend_Tick);
            // 
            // ucSocketServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxStatus);
            this.Controls.Add(this.groupBoxTimer);
            this.Controls.Add(this.groupBoxSend);
            this.Controls.Add(this.groupBoxReceive);
            this.Controls.Add(this.txtIPAddress);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Name = "ucSocketServer";
            this.Size = new System.Drawing.Size(596, 450);
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            this.groupBoxReceive.ResumeLayout(false);
            this.groupBoxSend.ResumeLayout(false);
            this.groupBoxSend.PerformLayout();
            this.groupBoxTimer.ResumeLayout(false);
            this.groupBoxTimer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIPAddress;
        private System.Windows.Forms.GroupBox groupBoxReceive;
        private System.Windows.Forms.ListBox listBoxReceive;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBoxSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnSendGroup;
        private System.Windows.Forms.GroupBox groupBoxTimer;
        private System.Windows.Forms.ComboBox comboBoxCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnStopTimer;
        private System.Windows.Forms.Button btnPauseTimer;
        private System.Windows.Forms.Button btnStartTimer;
        private System.Windows.Forms.Timer timerSend;
        private System.Windows.Forms.Label lblLocalPort;
        private System.Windows.Forms.NumericUpDown numInterval;
        private System.Windows.Forms.Label label8;
    }
}

