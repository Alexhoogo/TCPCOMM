using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCPCOM
{
    public partial class ucSocketClient : UserControl
    {
        public Socket socket = null;
        public Thread threadClient = null;//负责 监听 服务端发送来的消息的线程
        bool isRec = true;//是否循环接收服务端数据
        public IPAddress IPAddress = null;
        public IPEndPoint IPPort = null;

        // 定时发送相关变量
        private int currentSendCount = 0;  // 当前已发送次数
        private int totalSendCount = 0;     // 总发送次数
        private int sendInterval = 0;      // 发送间隔（毫秒）
        private bool isTimerPaused = false; // 是否暂停
        private bool isTimerRunning = false; // 是否正在运行

        // 公共属性，用于从外部设置IP和端口
        public string ClientIP
        {
            get { return txtIPAddress.Text; }
            set { txtIPAddress.Text = value; }
        }

        public string ClientPort
        {
            get { return txtPort.Text; }
            set { txtPort.Text = value; }
        }

        // 是否自动连接
        private bool autoConnect = true;

        public ucSocketClient()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            // 初始化定时发送相关控件
            if (comboBoxCount.Items.Count > 0)
                comboBoxCount.SelectedIndex = 0; // 默认选择1次
            
            numInterval.Value = 100; // 默认间隔100ms
            numInterval.Minimum = 1;
            numInterval.Maximum = 60000;
            
            // 自动连接
            if (autoConnect && !string.IsNullOrEmpty(txtIPAddress.Text) && !string.IsNullOrEmpty(txtPort.Text))
            {
                // 延迟一下再连接，确保界面已加载
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 100;
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    timer.Dispose();
                    ConnectToServer();
                };
                timer.Start();
            }
        }

        private void ConnectToServer()
        {
            if (string.IsNullOrEmpty(txtIPAddress.Text))
            { 
                ShowMsg("IP 地址错误，请重新输入！"); 
                UpdateConnectionStatus(false);
                return; 
            }
            if (string.IsNullOrEmpty(txtPort.Text))
            { 
                ShowMsg("IP 端口错误，请重新输入！"); 
                UpdateConnectionStatus(false);
                return; 
            }
            
            try
            {
                isRec = true;
                IPAddress = System.Net.IPAddress.Parse(txtIPAddress.Text);
                IPPort = new IPEndPoint(IPAddress, Convert.ToInt32(txtPort.Text));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(IPPort);
                
                // 获取本地端口
                if (socket.LocalEndPoint != null)
                {
                    lblLocalPort.Text = "本地端口:" + ((IPEndPoint)socket.LocalEndPoint).Port;
                }
                
                // 显示远程IP和端口
                if (socket.RemoteEndPoint != null)
                {
                    IPEndPoint remoteEP = (IPEndPoint)socket.RemoteEndPoint;
                    lblRemoteIP.Text = "对方IP:" + remoteEP.Address.ToString();
                    lblRemotePort.Text = "对方端口:" + remoteEP.Port.ToString();
                }
                
                threadClient = new Thread(ReceiveMsg);
                threadClient.IsBackground = true;
                threadClient.SetApartmentState(ApartmentState.STA);
                threadClient.Start();
                
                ShowMsg("连接服务器成功");
                UpdateConnectionStatus(true);
            }
            catch (Exception exp)
            {
                if (socket != null)
                {
                    try
                    {
                        socket.Close();
                        socket.Dispose();
                    }
                    catch { }
                    socket = null;
                }
                ShowMsg("连接服务器失败：" + exp.Message);
                UpdateConnectionStatus(false);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToServer();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            CloseConnection();
        }

        private void CloseConnection()
        {
            isRec = false;
            
            // 关闭socket
            if (socket != null)
            {
                try
                {
                    if (socket.Connected)
                    {
                        socket.Shutdown(SocketShutdown.Both);
                    }
                    socket.Close();
                    socket.Dispose();
                }
                catch { }
                socket = null;
            }
            
            // 停止接收线程
            if (threadClient != null && threadClient.IsAlive)
            {
                try
                {
                    threadClient.Abort();
                }
                catch { }
                threadClient = null;
            }
            
            UpdateConnectionStatus(false);
            ShowMsg("和服务器断开连接成功");
        }

        private void UpdateConnectionStatus(bool connected)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    // 检查控件是否仍然有效
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.Invoke(new Action<bool>(UpdateConnectionStatus), connected);
                    }
                    return;
                }
                
                // 再次检查控件是否已被释放
                if (this.IsDisposed)
                    return;
                
                if (connected)
                {
                    if (lblStatus != null) lblStatus.Text = "已连接";
                    if (lblStatus != null) lblStatus.ForeColor = Color.Green;
                    if (btnConnect != null) btnConnect.Enabled = false;
                    if (btnDisconnect != null) btnDisconnect.Enabled = true;
                }
                else
                {
                    if (lblStatus != null) lblStatus.Text = "已断开";
                    if (lblStatus != null) lblStatus.ForeColor = Color.Red;
                    if (btnConnect != null) btnConnect.Enabled = true;
                    if (btnDisconnect != null) btnDisconnect.Enabled = false;
                    if (lblLocalPort != null) lblLocalPort.Text = "本地端口:0";
                    if (lblRemoteIP != null) lblRemoteIP.Text = "对方IP:";
                    if (lblRemotePort != null) lblRemotePort.Text = "对方端口:";
                }
            }
            catch (ObjectDisposedException)
            {
                // 控件已被释放，忽略错误
            }
            catch (InvalidOperationException)
            {
                // 控件句柄无效，忽略错误
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendMsg(txtMessage.Text);
        }

        /// <summary>
        /// 接收服务端发送来的消息数据
        /// </summary>
        void ReceiveMsg()
        {
            try
            {
                while (isRec && socket != null && socket.Connected)
                {
                    byte[] msgArr = new byte[1024];
                    int length = socket.Receive(msgArr);
                    
                    if (length > 0)
                    {
                        // 只转换实际接收到的字节数，避免多余的0字节
                        string strMsg = System.Text.Encoding.UTF8.GetString(msgArr, 0, length);
                        ShowMsg(strMsg);
                    }
                    else
                    {
                        // 连接已关闭
                        isRec = false;
                        ShowMsg("服务器已断开连接");
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                if (isRec) // 只有在正常接收时出错才显示错误
                {
                    ShowMsg("接收消息出错：" + exp.Message);
                }
            }
            finally
            {
                // 连接断开后重置UI状态
                try
                {
                    if (this.InvokeRequired)
                    {
                        // 检查控件是否仍然有效
                        if (this.IsHandleCreated && !this.IsDisposed)
                        {
                            this.Invoke(new Action(() =>
                            {
                                if (!this.IsDisposed)
                                {
                                    UpdateConnectionStatus(false);
                                }
                            }));
                        }
                    }
                    else
                    {
                        if (!this.IsDisposed)
                        {
                            UpdateConnectionStatus(false);
                        }
                    }
                }
                catch (ObjectDisposedException)
                {
                    // 控件已被释放，忽略错误
                }
                catch (InvalidOperationException)
                {
                    // 控件句柄无效，忽略错误
                }
            }
        }

        public void SendMsg(string msg)
        {
            if (socket != null && socket.Connected)
            {
                try
                {
                    byte[] byteSend = System.Text.Encoding.UTF8.GetBytes(msg);
                    int bytesSent = socket.Send(byteSend);
                    ShowMsg("发送消息成功（" + bytesSent + " 字节）");
                }
                catch (Exception exp)
                {
                    ShowMsg("发送消息失败：" + exp.Message);
                    // 如果发送失败，可能是连接已断开
                    isRec = false;
                    UpdateConnectionStatus(false);
                }
            }
            else
            {
                ShowMsg("发送消息失败：未连接到服务器");
            }
        }

        /// <summary>
        /// 向文本框显示消息
        /// </summary>
        /// <param name="msgStr">消息</param>
        public void ShowMsg(string msgStr)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    // 检查控件是否仍然有效
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        Action<string> ShowMsgAction = Msg => { 
                            if (!this.IsDisposed && this.listBoxReceive != null)
                            {
                                this.listBoxReceive.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " - " + Msg); 
                                this.listBoxReceive.TopIndex = this.listBoxReceive.Items.Count - 1; 
                            }
                        };
                        this.Invoke(ShowMsgAction, msgStr);
                    }
                }
                else
                {
                    if (!this.IsDisposed && this.listBoxReceive != null)
                    {
                        this.listBoxReceive.Items.Add(DateTime.Now.ToString("HH:mm:ss") + " - " + msgStr);
                        this.listBoxReceive.TopIndex = this.listBoxReceive.Items.Count - 1;
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // 控件已被释放，忽略错误
            }
            catch (InvalidOperationException)
            {
                // 控件句柄无效，忽略错误
            }
        }

        /// <summary>
        /// 开始定时发送
        /// </summary>
        private void btnStartTimer_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (string.IsNullOrEmpty(txtMessage.Text.Trim()))
            {
                MessageBox.Show("请输入要发送的消息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxCount.SelectedItem == null)
            {
                MessageBox.Show("请选择发送次数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 检查是否已连接到服务器
            if (socket == null || !socket.Connected)
            {
                MessageBox.Show("未连接到服务器，无法发送消息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 如果是暂停状态，恢复发送
                if (isTimerPaused)
                {
                    isTimerPaused = false;
                    timerSend.Start();
                    btnPauseTimer.Enabled = true;
                    btnStartTimer.Enabled = false;
                    ShowMsg("定时发送已恢复");
                    return;
                }

                // 开始新的定时发送
                sendInterval = (int)numInterval.Value;
                totalSendCount = Convert.ToInt32(comboBoxCount.SelectedItem.ToString());
                currentSendCount = 0;
                isTimerPaused = false;
                isTimerRunning = true;

                // 设置定时器间隔
                timerSend.Interval = sendInterval;
                timerSend.Start();

                // 更新按钮状态
                btnStartTimer.Enabled = false;
                btnPauseTimer.Enabled = true;
                btnStopTimer.Enabled = true;
                comboBoxCount.Enabled = false;
                numInterval.Enabled = false;

                ShowMsg(string.Format("开始定时发送：间隔 {0}ms，总共 {1} 次", sendInterval, totalSendCount));
            }
            catch (Exception exp)
            {
                MessageBox.Show("启动定时发送失败：" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 暂停定时发送
        /// </summary>
        private void btnPauseTimer_Click(object sender, EventArgs e)
        {
            if (isTimerRunning && !isTimerPaused)
            {
                timerSend.Stop();
                isTimerPaused = true;
                btnStartTimer.Enabled = true;
                btnPauseTimer.Enabled = false;
                ShowMsg("定时发送已暂停");
            }
        }

        /// <summary>
        /// 停止定时发送
        /// </summary>
        private void btnStopTimer_Click(object sender, EventArgs e)
        {
            timerSend.Stop();
            isTimerRunning = false;
            isTimerPaused = false;
            currentSendCount = 0;
            totalSendCount = 0;

            // 恢复按钮状态
            btnStartTimer.Enabled = true;
            btnPauseTimer.Enabled = false;
            btnStopTimer.Enabled = false;
            //comboBoxInterval.Enabled = true;
            comboBoxCount.Enabled = true;
            numInterval.Enabled = true;

            ShowMsg("定时发送已停止");
        }

        /// <summary>
        /// 定时器事件，执行发送
        /// </summary>
        private void timerSend_Tick(object sender, EventArgs e)
        {
            if (isTimerPaused || !isTimerRunning)
                return;

            // 检查是否达到发送次数
            if (currentSendCount >= totalSendCount)
            {
                timerSend.Stop();
                isTimerRunning = false;
                isTimerPaused = false;

                // 恢复按钮状态
                btnStartTimer.Enabled = true;
                btnPauseTimer.Enabled = false;
                btnStopTimer.Enabled = false;
                comboBoxCount.Enabled = true;
                numInterval.Enabled = true;

                ShowMsg(string.Format("定时发送完成：总共发送 {0} 次", currentSendCount));
                return;
            }

            // 执行发送
            try
            {
                string msgToSend = txtMessage.Text.Trim();
                if (string.IsNullOrEmpty(msgToSend))
                {
                    ShowMsg("消息内容为空，停止发送");
                    btnStopTimer_Click(null, null);
                    return;
                }

                // 检查连接状态
                if (socket == null || !socket.Connected)
                {
                    ShowMsg("连接已断开，停止定时发送");
                    btnStopTimer_Click(null, null);
                    return;
                }

                // 发送消息
                SendMsg(msgToSend);
                currentSendCount++;
                ShowMsg(string.Format("定时发送 [{0}/{1}]", currentSendCount, totalSendCount));
            }
            catch (Exception exp)
            {
                ShowMsg("定时发送出错：" + exp.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBoxReceive.Items.Clear();
        }
    }
}

