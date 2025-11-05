using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TCPCOM
{
    public partial class ucSocketServer : UserControl
    {
        public Socket socket = null;
        public IPAddress IPAddress = null;
        public IPEndPoint IPPort = null;

        Dictionary<string, ConnectionClient> dictConn = new Dictionary<string, ConnectionClient>();
        Dictionary<string, TreeNode> clientNodes = new Dictionary<string, TreeNode>(); // 存储客户端节点

        // TreeView 相关属性
        public TreeNode ServerNode { get; set; }  // 对应的 ServerInstance TreeNode
        public TreeView TreeView { get; set; }    // MainForm 的 TreeView 引用

        // 定时发送相关变量
        private int currentSendCount = 0;  // 当前已发送次数
        private int totalSendCount = 0;     // 总发送次数
        private int sendInterval = 0;      // 发送间隔（毫秒）
        private bool isTimerPaused = false; // 是否暂停
        private bool isTimerRunning = false; // 是否正在运行

        // 公共属性，用于从外部设置IP和端口
        public string ServerIP
        {
            get { return txtIPAddress.Text; }
            set { txtIPAddress.Text = value; }
        }

        public string ServerPort
        {
            get { return txtPort.Text; }
            set { txtPort.Text = value; }
        }

        // 是否自动启动监听
        private bool autoStart = true;

        public ucSocketServer()
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
            
            // 自动启动监听
            if (autoStart && !string.IsNullOrEmpty(txtIPAddress.Text) && !string.IsNullOrEmpty(txtPort.Text))
            {
                // 延迟一下再启动，确保界面已加载
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 100;
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    timer.Dispose();
                    StartListening();
                };
                timer.Start();
            }
        }

        private void StartListening()
        {
            // IP地址可以为空或"0.0.0.0"，将绑定到所有接口
            if (string.IsNullOrEmpty(txtPort.Text))
            { 
                ShowMsg("IP 端口错误，请重新输入！"); 
                UpdateServerStatus(false);
                return; 
            }
            
            try
            {
                // 如果IP为空或为"0.0.0.0"，则绑定到所有接口
                if (string.IsNullOrWhiteSpace(txtIPAddress.Text) || txtIPAddress.Text.Trim() == "0.0.0.0")
                {
                    IPAddress = System.Net.IPAddress.Any;
                }
                else
                {
                    IPAddress = System.Net.IPAddress.Parse(txtIPAddress.Text);
                }
                IPPort = new IPEndPoint(IPAddress, Convert.ToInt32(txtPort.Text));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(IPPort);
                socket.Listen(10); // 最多允许10个客户端连接队列
                
                // 显示本地端口
                lblLocalPort.Text = "本地端口:" + IPPort.Port;
                
                // 开始异步接受连接
                socket.BeginAccept(new AsyncCallback(SocketAccept), socket);
                
                ShowMsg("服务器开启成功，开始监听: " + IPPort.ToString());
                UpdateServerStatus(true);
            }
            catch (Exception exp)
            {
                ShowMsg("服务器启动失败：" + exp.Message);
                if (socket != null)
                {
                    socket.Close();
                    socket.Dispose();
                    socket = null;
                }
                UpdateServerStatus(false);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartListening();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopListening();
        }

        private void StopListening()
        {
            // 关闭所有客户端连接
            foreach (var kvp in dictConn)
            {
                try
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.isRec = false;
                        if (kvp.Value.sokMsg != null && kvp.Value.sokMsg.Connected)
                        {
                            kvp.Value.sokMsg.Close();
                            kvp.Value.sokMsg.Dispose();
                        }
                        if (kvp.Value.threadMsg != null && kvp.Value.threadMsg.IsAlive)
                        {
                            kvp.Value.threadMsg.Abort();
                        }
                    }
                }
                catch { }
            }
            
            // 清除 TreeView 中的客户端节点
            foreach (var kvp in clientNodes)
            {
                try
                {
                    if (kvp.Value != null && kvp.Value.Parent != null)
                    {
                        kvp.Value.Remove();
                    }
                }
                catch { }
            }
            
            dictConn.Clear();
            clientNodes.Clear();
            
            // 关闭服务器socket
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
            
            UpdateServerStatus(false);
            ShowMsg("服务器已停止监听");
        }

        private void UpdateServerStatus(bool started)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    // 检查控件是否仍然有效
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.Invoke(new Action<bool>(UpdateServerStatus), started);
                    }
                    return;
                }
                
                // 再次检查控件是否已被释放
                if (this.IsDisposed)
                    return;
                
                if (started)
                {
                    if (lblStatus != null) lblStatus.Text = "已启动";
                    if (lblStatus != null) lblStatus.ForeColor = Color.Green;
                    if (btnStart != null) btnStart.Enabled = false;
                    if (btnStop != null) btnStop.Enabled = true;
                }
                else
                {
                    if (lblStatus != null) lblStatus.Text = "已停止";
                    if (lblStatus != null) lblStatus.ForeColor = Color.Red;
                    if (btnStart != null) btnStart.Enabled = true;
                    if (btnStop != null) btnStop.Enabled = false;
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

        public void SocketAccept(IAsyncResult AResult)
        {
            Socket serverSocket = (Socket)AResult.AsyncState;
            Socket clientsocket = null;
            
            try
            {
                clientsocket = serverSocket.EndAccept(AResult);

                ConnectionClient connection = new ConnectionClient(clientsocket, ShowMsg, RemoveClientConnection);
                string clientKey = clientsocket.RemoteEndPoint.ToString();
                dictConn.Add(clientKey, connection);

                // 添加到 TreeView
                AddClientToTree(clientKey);
                
                ShowMsg("客户端:" + clientKey + " 已连接");
            }
            catch (Exception exp)
            {
                ShowMsg("接受客户端连接时出错：" + exp.Message);
                if (clientsocket != null)
                {
                    try
                    {
                        clientsocket.Close();
                        clientsocket.Dispose();
                    }
                    catch { }
                }
            }
            finally
            {
                //再次监听新的连接（如果服务器socket还在）
                if (socket != null && socket.IsBound)
                {
                    try
                    {
                        socket.BeginAccept(new AsyncCallback(SocketAccept), socket);
                    }
                    catch (Exception exp)
                    {
                        ShowMsg("继续监听连接时出错：" + exp.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 文本框显示新消息
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
        /// 移除与指定客户端的连接
        /// </summary>
        /// <param name="key">指定客户端的IP和端口</param>
        public void RemoveClientConnection(string key)
        {
            try
            {
                if (dictConn.ContainsKey(key))
                {
                    dictConn.Remove(key);
                    RemoveClientFromTree(key);
                    ShowMsg(key + " 已断开");
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
        /// 添加客户端到 TreeView
        /// </summary>
        /// <param name="clientKey">客户端标识</param>
        private void AddClientToTree(string clientKey)
        {
            if (ServerNode == null || TreeView == null)
                return;

            try
            {
                if (this.InvokeRequired)
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        Action<string> AddClient = key =>
                        {
                            if (ServerNode != null && !clientNodes.ContainsKey(key))
                            {
                                TreeNode clientNode = new TreeNode(key);
                                clientNode.Tag = "ClientConnection";
                                ServerNode.Nodes.Add(clientNode);
                                clientNodes[key] = clientNode;
                                TreeView.ExpandAll();
                            }
                        };
                        this.Invoke(AddClient, clientKey);
                    }
                }
                else
                {
                    if (ServerNode != null && !clientNodes.ContainsKey(clientKey))
                    {
                        TreeNode clientNode = new TreeNode(clientKey);
                        clientNode.Tag = "ClientConnection";
                        ServerNode.Nodes.Add(clientNode);
                        clientNodes[clientKey] = clientNode;
                        TreeView.ExpandAll();
                    }
                }
            }
            catch (Exception exp)
            {
                ShowMsg("添加客户端到树视图时出错：" + exp.Message);
            }
        }

        /// <summary>
        /// 从 TreeView 移除客户端
        /// </summary>
        /// <param name="clientKey">客户端标识</param>
        private void RemoveClientFromTree(string clientKey)
        {
            if (ServerNode == null || TreeView == null)
                return;

            try
            {
                if (this.InvokeRequired)
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        Action<string> RemoveClient = key =>
                        {
                            if (clientNodes.ContainsKey(key))
                            {
                                TreeNode clientNode = clientNodes[key];
                                if (clientNode.Parent != null)
                                {
                                    clientNode.Remove();
                                }
                                clientNodes.Remove(key);
                            }
                        };
                        this.Invoke(RemoveClient, clientKey);
                    }
                }
                else
                {
                    if (clientNodes.ContainsKey(clientKey))
                    {
                        TreeNode clientNode = clientNodes[clientKey];
                        if (clientNode.Parent != null)
                        {
                            clientNode.Remove();
                        }
                        clientNodes.Remove(clientKey);
                    }
                }
            }
            catch (Exception exp)
            {
                ShowMsg("从树视图移除客户端时出错：" + exp.Message);
            }
        }


        private void btnSendGroup_Click(object sender, EventArgs e)
        {
            //循环群发消息
            if (dictConn.Count > 0)
            {
                int successCount = 0;
                int failCount = 0;
                List<string> keysToRemove = new List<string>();
                
                foreach (var kvp in dictConn)
                {
                    try
                    {
                        kvp.Value.Send(txtMessage.Text.Trim());
                        successCount++;
                    }
                    catch
                    {
                        failCount++;
                        keysToRemove.Add(kvp.Key);
                    }
                }
                
                // 移除已断开的连接
                foreach (string key in keysToRemove)
                {
                    RemoveClientConnection(key);
                }

                ShowMsg(string.Format("群发送消息完成：成功 {0} 个，失败 {1} 个", successCount, failCount));
            }
            else
            {
                ShowMsg("没有连接的客户端");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBoxReceive.Items.Clear();
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

            // 检查是否有客户端连接
            if (dictConn.Count == 0)
            {
                MessageBox.Show("没有连接的客户端，无法发送消息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                // 群发消息
                int successCount = 0;
                int failCount = 0;
                List<string> keysToRemove = new List<string>();

                foreach (var kvp in dictConn)
                {
                    try
                    {
                        kvp.Value.Send(msgToSend);
                        successCount++;
                    }
                    catch
                    {
                        failCount++;
                        keysToRemove.Add(kvp.Key);
                    }
                }

                // 移除已断开的连接
                foreach (string key in keysToRemove)
                {
                    RemoveClientConnection(key);
                }

                currentSendCount++;
                ShowMsg(string.Format("定时发送 [{0}/{1}]：成功 {2} 个，失败 {3} 个", 
                    currentSendCount, totalSendCount, successCount, failCount));

                // 如果所有客户端都断开，停止发送
                if (dictConn.Count == 0)
                {
                    ShowMsg("所有客户端已断开，停止定时发送");
                    btnStopTimer_Click(null, null);
                }
            }
            catch (Exception exp)
            {
                ShowMsg("定时发送出错：" + exp.Message);
            }
        }
    }
}

