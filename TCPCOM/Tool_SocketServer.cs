using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace TCPCOM
{
    public partial class Tool_SocketServer : Form
    {
        public Socket socket = null;
        public IPAddress IPAddress = null;
        public IPEndPoint IPPort = null;

        public Tool_SocketServer()
        {
            InitializeComponent(); //Frm_Start.Projectc = prom;
        }

        private void startserverbtn_Click(object sender, EventArgs e)
        {
            if (ipaddress.Text == string.Empty)
            { infobox.Items.Add("IP 地址错误，请重新输入！"); return; }
            if (ipport.Text == string.Empty)
            { infobox.Items.Add("IP 端口错误，请重新输入！"); return; }
            try
            {
                IPAddress = IPAddress.Parse(ipaddress.Text);//127.0.0.1
                IPPort = new IPEndPoint(IPAddress, Convert.ToInt32(ipport.Text));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(IPPort);
                socket.Listen(10); // 最多允许10个客户端连接队列
                
                // 开始异步接受连接
                socket.BeginAccept(new AsyncCallback(ScoketAccept), socket);
                
                infobox.Items.Add("服务器开启成功，开始监听: " + IPPort.ToString());
                startserverbtn.Enabled = false;
            }
            catch (Exception exp)
            {
                infobox.Items.Add("服务器启动失败：" + exp.Message);
                if (socket != null)
                {
                    socket.Close();
                    socket.Dispose();
                    socket = null;
                }
            }
        }

        private void sendsingle_Click(object sender, EventArgs e)
        {
            string connectionSokKey = clientlist.Text;
            if (!string.IsNullOrEmpty(connectionSokKey))
            {
                if (dictConn.ContainsKey(connectionSokKey))
                {
                    try
                    {
                        //从字典集合中根据键获得 负责与该客户端通信的套接字，并调用send方法发送数据过去
                        dictConn[connectionSokKey].Send(message.Text.Trim());
                        ShowMsg("对客户端:" + connectionSokKey + "发送消息成功");
                    }
                    catch (Exception exp)
                    {
                        ShowMsg("对客户端:" + connectionSokKey + "发送消息失败：" + exp.Message);
                    }
                }
                else
                {
                    MessageBox.Show("客户端连接已断开");
                }
            }
            else
            {
                MessageBox.Show("请选择要发送的客户端");
            }
        }

        private void sendgroup_Click(object sender, EventArgs e)
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
                        kvp.Value.Send(message.Text.Trim());
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

        private void clearinfo_Click(object sender, EventArgs e)
        {
            infobox.Items.Clear();
        }

        Dictionary<string, ConnectionClient> dictConn = new Dictionary<string, ConnectionClient>();

        // 定时发送相关变量
        private int currentSendCount = 0;  // 当前已发送次数
        private int totalSendCount = 0;     // 总发送次数
        private int sendInterval = 0;      // 发送间隔（毫秒）
        private bool isTimerPaused = false; // 是否暂停
        private bool isTimerRunning = false; // 是否正在运行

        // 公共属性，用于从外部设置IP和端口
        public string ServerIP
        {
            get { return ipaddress.Text; }
            set { ipaddress.Text = value; }
        }

        public string ServerPort
        {
            get { return ipport.Text; }
            set { ipport.Text = value; }
        }

        public void ScoketAccept(IAsyncResult AResult)
        {
            Socket serverSocket = (Socket)AResult.AsyncState;
            Socket clientsocket = null;
            
            try
            {
                clientsocket = serverSocket.EndAccept(AResult);

                ConnectionClient connection = new ConnectionClient(clientsocket, ShowMsg, RemoveClientConnection);
                string clientKey = clientsocket.RemoteEndPoint.ToString();
                dictConn.Add(clientKey, connection);

                Action<string> BindClent = Msg => { this.clientlist.Items.Add(Msg); };
                this.Invoke(BindClent, clientKey);
                
                ShowMsg("客户端:" + clientKey + " 服务器已连接");
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
                        socket.BeginAccept(new AsyncCallback(ScoketAccept), socket);
                    }
                    catch (Exception exp)
                    {
                        ShowMsg("继续监听连接时出错：" + exp.Message);
                    }
                }
            }
        }

        public delegate void SetTextBoxValue(string value);
        /// <summary>
        /// 文本框显示新消息
        /// </summary>
        /// <param name="msgStr">消息</param>
        public void ShowMsg(string msgStr)
        {
            Action<string> ShowMsg = Msg => { this.infobox.Items.Add(Msg); this.infobox.TopIndex = this.infobox.Items.Count - 1; };
            this.Invoke(ShowMsg, msgStr);
        }

        /// <summary>
        /// 移除与指定客户端的连接
        /// </summary>
        /// <param name="key">指定客户端的IP和端口</param>
        public void RemoveClientConnection(string key)
        {
            if (dictConn.ContainsKey(key))
            {
                dictConn.Remove(key);
                Action<string> RemoveClent = Msg => { this.clientlist.Items.Remove(Msg); };

                this.Invoke(RemoveClent, key);

                ShowMsg(key + " 服务器已断开");
            }
        }

        private void Tool_SocketServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 停止定时发送
            if (timerSend != null)
            {
                timerSend.Stop();
                timerSend.Dispose();
            }

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
            dictConn.Clear();
            
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
        }

        private void Tool_SocketServer_Load(object sender, EventArgs e)
        {
            //startserverbtn.BackColor = sendsingle.BackColor = sendgroup.BackColor = clearinfo.BackColor = Frm_Start.ThemeColor;
            //ipaddress.Text = Frm_Start.Projectc.Socket_IP;
            //ipport.Text = Frm_Start.Projectc.Socket_Port;
            //if (Frm_Start.Projectc.iotype == IOtype.Socket)
            //{
                //if (Frm_Main.bIniIOResult)
                //{
                //    ipaddress.Enabled = ipport.Enabled = startserverbtn.Enabled = false;
                //}
                //else
                //{
                    ipaddress.Enabled = ipport.Enabled = startserverbtn.Enabled = true;
               // }
            //}
            
            // 初始化定时发送相关控件
            if (comboBoxInterval.Items.Count > 0)
                comboBoxInterval.SelectedIndex = 1; // 默认选择100ms
            if (comboBoxCount.Items.Count > 0)
                comboBoxCount.SelectedIndex = 0; // 默认选择1次
        }

        /// <summary>
        /// 开始定时发送
        /// </summary>
        private void btnStartTimer_Click(object sender, EventArgs e)
        {
            // 验证输入
            if (string.IsNullOrEmpty(message.Text.Trim()))
            {
                MessageBox.Show("请输入要发送的消息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxInterval.SelectedItem == null)
            {
                MessageBox.Show("请选择时间间隔！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                sendInterval = Convert.ToInt32(comboBoxInterval.SelectedItem.ToString());
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
                btnCancelTimer.Enabled = true;
                comboBoxInterval.Enabled = false;
                comboBoxCount.Enabled = false;

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
        /// 取消定时发送
        /// </summary>
        private void btnCancelTimer_Click(object sender, EventArgs e)
        {
            timerSend.Stop();
            isTimerRunning = false;
            isTimerPaused = false;
            currentSendCount = 0;
            totalSendCount = 0;

            // 恢复按钮状态
            btnStartTimer.Enabled = true;
            btnPauseTimer.Enabled = false;
            btnCancelTimer.Enabled = false;
            comboBoxInterval.Enabled = true;
            comboBoxCount.Enabled = true;

            ShowMsg("定时发送已取消");
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
                btnCancelTimer.Enabled = false;
                comboBoxInterval.Enabled = true;
                comboBoxCount.Enabled = true;

                ShowMsg(string.Format("定时发送完成：总共发送 {0} 次", currentSendCount));
                return;
            }

            // 执行发送
            try
            {
                string msgToSend = message.Text.Trim();
                if (string.IsNullOrEmpty(msgToSend))
                {
                    ShowMsg("消息内容为空，停止发送");
                    btnCancelTimer_Click(null, null);
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
                    btnCancelTimer_Click(null, null);
                }
            }
            catch (Exception exp)
            {
                ShowMsg("定时发送出错：" + exp.Message);
            }
        }
    }
}
