using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace TCPCOM
{
    public partial class Tool_SocketClient : Form
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
            get { return ipaddress.Text; }
            set { ipaddress.Text = value; }
        }

        public string ClientPort
        {
            get { return ipport.Text; }
            set { ipport.Text = value; }
        }

        public Tool_SocketClient()
        {
            InitializeComponent(); //Frm_Start.Projectc = prom;
        }

        private void conserver_Click(object sender, EventArgs e)
        {
            if (ipaddress.Text == string.Empty)
            { infobox.Items.Add("IP 地址错误，请重新输入！"); return; }
            if (ipport.Text == string.Empty)
            { infobox.Items.Add("IP 端口错误，请重新输入！"); return; }
            try
            {
                isRec = true;
                IPAddress = IPAddress.Parse(ipaddress.Text);//127.0.0.1
                IPPort = new IPEndPoint(IPAddress, Convert.ToInt32(ipport.Text));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(IPPort);
                threadClient = new Thread(ReceiveMsg);
                threadClient.IsBackground = true;
                threadClient.SetApartmentState(ApartmentState.STA);
                threadClient.Start();
                infobox.Items.Add("连接服务器成功");
                disconserver.Enabled = true;
                conserver.Enabled = false;
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
                infobox.Items.Add("连接服务器失败：" + exp.Message);
            }
        }

        private void disconserver_Click(object sender, EventArgs e)
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
            
            disconserver.Enabled = false;
            conserver.Enabled = true;
            infobox.Items.Add("和服务器断开连接成功");
        }

        private void sendinfo_Click(object sender, EventArgs e)
        {
            SendMsg(message.Text);
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
                        ShowMsg("服务器发来消息：" + strMsg);
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
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        disconserver.Enabled = false;
                        conserver.Enabled = true;
                    }));
                }
                else
                {
                    disconserver.Enabled = false;
                    conserver.Enabled = true;
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
                    infobox.Items.Add("发送消息成功（" + bytesSent + " 字节）");
                }
                catch (Exception exp)
                {
                    infobox.Items.Add("发送消息失败：" + exp.Message);
                    // 如果发送失败，可能是连接已断开
                    isRec = false;
                }
            }
            else
            {
                infobox.Items.Add("发送消息失败：未连接到服务器");
            }
        }
        /// <summary>
        /// 向文本框显示消息
        /// </summary>
        /// <param name="msgStr">消息</param>
        public void ShowMsg(string msgStr)
        {
            Action<string> ShowMsg = Msg => { this.infobox.Items.Add(Msg); this.infobox.TopIndex = this.infobox.Items.Count - 1; };
            this.Invoke(ShowMsg, msgStr);
        }

        private void Tool_SocketClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 停止定时发送
            if (timerSend != null)
            {
                timerSend.Stop();
                timerSend.Dispose();
            }

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
        }

        private void Tool_SocketClient_Load(object sender, EventArgs e)
        {
            //conserver.BackColor = disconserver.BackColor = sendinfo.BackColor = Frm_Start.ThemeColor;
            //if (Frm_Start.Projectc.iotype == IOtype.Socket)
            //{
            //    if (Frm_Main.bIniIOResult)
            //    {
            //        ipaddress.Enabled = ipport.Enabled = conserver.Enabled = false;
            //    }
            //    else
            //    {
                    ipaddress.Enabled = ipport.Enabled = conserver.Enabled = true;
             //   }
           // }
            
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
                    infobox.Items.Add("定时发送已恢复");
                    infobox.TopIndex = infobox.Items.Count - 1;
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

                infobox.Items.Add(string.Format("开始定时发送：间隔 {0}ms，总共 {1} 次", sendInterval, totalSendCount));
                infobox.TopIndex = infobox.Items.Count - 1;
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
                infobox.Items.Add("定时发送已暂停");
                infobox.TopIndex = infobox.Items.Count - 1;
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

            infobox.Items.Add("定时发送已取消");
            infobox.TopIndex = infobox.Items.Count - 1;
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

                infobox.Items.Add(string.Format("定时发送完成：总共发送 {0} 次", currentSendCount));
                infobox.TopIndex = infobox.Items.Count - 1;
                return;
            }

            // 执行发送
            try
            {
                string msgToSend = message.Text.Trim();
                if (string.IsNullOrEmpty(msgToSend))
                {
                    infobox.Items.Add("消息内容为空，停止发送");
                    infobox.TopIndex = infobox.Items.Count - 1;
                    btnCancelTimer_Click(null, null);
                    return;
                }

                // 检查连接状态
                if (socket == null || !socket.Connected)
                {
                    infobox.Items.Add("连接已断开，停止定时发送");
                    infobox.TopIndex = infobox.Items.Count - 1;
                    btnCancelTimer_Click(null, null);
                    return;
                }

                // 发送消息
                SendMsg(msgToSend);
                currentSendCount++;
                infobox.Items.Add(string.Format("定时发送 [{0}/{1}]", currentSendCount, totalSendCount));
                infobox.TopIndex = infobox.Items.Count - 1;
            }
            catch (Exception exp)
            {
                infobox.Items.Add("定时发送出错：" + exp.Message);
                infobox.TopIndex = infobox.Items.Count - 1;
            }
        }
    }
}
