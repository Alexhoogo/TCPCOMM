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
    public partial class ucMitsubishiPLC : UserControl
    {
        public Socket socket = null;
        public Thread threadClient = null;
        bool isRec = true;
        public IPAddress IPAddress = null;
        public IPEndPoint IPPort = null;

        // MC协议相关常量（3E帧二进制格式）
        private const int MC_PROTOCOL_PORT = 5007; // 三菱PLC默认端口
        private const byte SUBHEADER_LOW = 0x50; // 副标头低字节
        private const byte SUBHEADER_HIGH = 0x00; // 副标头高字节
        private const byte NETWORK_NUMBER = 0x00; // 网络编号
        private const byte PC_NUMBER = 0xFF; // PC编号
        private const byte DESTINATION_MODULE_IO_LOW = 0xFF; // 目标模块IO号低字节
        private const byte DESTINATION_MODULE_IO_HIGH = 0x03; // 目标模块IO号高字节
        private const byte DESTINATION_MODULE_STATION = 0x00; // 目标模块站号
        private const ushort MONITOR_TIMER = 0x0064; // 监视定时器 (100 * 10ms = 1000ms)

        // 公共属性
        public string PLCIP
        {
            get { return txtIPAddress.Text; }
            set { txtIPAddress.Text = value; }
        }

        public string PLCPort
        {
            get { return txtPort.Text; }
            set { txtPort.Text = value; }
        }

        // 是否自动连接
        private bool autoConnect = true;

        public ucMitsubishiPLC()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 设置默认端口
            if (string.IsNullOrEmpty(txtPort.Text))
            {
                txtPort.Text = MC_PROTOCOL_PORT.ToString();
            }

            // 初始化寄存器类型下拉框
            if (cmbRegisterType.Items.Count == 0)
            {
                cmbRegisterType.Items.AddRange(new string[] { "D", "M", "X", "Y", "R", "W", "L" });
                cmbRegisterType.SelectedIndex = 0; // 默认D寄存器
            }

            // 自动连接
            if (autoConnect && !string.IsNullOrEmpty(txtIPAddress.Text) && !string.IsNullOrEmpty(txtPort.Text))
            {
                System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
                timer.Interval = 100;
                timer.Tick += (s, args) =>
                {
                    timer.Stop();
                    timer.Dispose();
                    ConnectToPLC();
                };
                timer.Start();
            }
        }

        private void ConnectToPLC()
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
                    lblRemoteIP.Text = "PLC IP:" + remoteEP.Address.ToString();
                    lblRemotePort.Text = "PLC端口:" + remoteEP.Port.ToString();
                }

                threadClient = new Thread(ReceiveMsg);
                threadClient.IsBackground = true;
                threadClient.SetApartmentState(ApartmentState.STA);
                threadClient.Start();

                ShowMsg("连接PLC成功");
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
                ShowMsg("连接PLC失败：" + exp.Message);
                UpdateConnectionStatus(false);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToPLC();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            CloseConnection();
        }

        private void CloseConnection()
        {
            isRec = false;

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
            ShowMsg("和PLC断开连接成功");
        }

        private void UpdateConnectionStatus(bool connected)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.Invoke(new Action<bool>(UpdateConnectionStatus), connected);
                    }
                    return;
                }

                if (this.IsDisposed)
                    return;

                if (connected)
                {
                    if (lblStatus != null) lblStatus.Text = "已连接";
                    if (lblStatus != null) lblStatus.ForeColor = Color.Green;
                    if (btnConnect != null) btnConnect.Enabled = false;
                    if (btnDisconnect != null) btnDisconnect.Enabled = true;
                    if (btnRead != null) btnRead.Enabled = true;
                    if (btnWrite != null) btnWrite.Enabled = true;
                }
                else
                {
                    if (lblStatus != null) lblStatus.Text = "已断开";
                    if (lblStatus != null) lblStatus.ForeColor = Color.Red;
                    if (btnConnect != null) btnConnect.Enabled = true;
                    if (btnDisconnect != null) btnDisconnect.Enabled = false;
                    if (lblLocalPort != null) lblLocalPort.Text = "本地端口:0";
                    if (lblRemoteIP != null) lblRemoteIP.Text = "PLC IP:";
                    if (lblRemotePort != null) lblRemotePort.Text = "PLC端口:";
                    if (btnRead != null) btnRead.Enabled = false;
                    if (btnWrite != null) btnWrite.Enabled = false;
                }
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        /// <summary>
        /// 接收PLC返回的消息
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
                        // 解析MC协议响应
                        string responseHex = BitConverter.ToString(msgArr, 0, length).Replace("-", " ");
                        ShowMsg("接收: " + responseHex);

                        // 解析响应数据（MC协议3E帧格式）
                        // 响应格式：副标头(2字节) + 网络号(1字节) + PC号(1字节) + IO号(2字节) + 站号(1字节) + 
                        //           数据长度(2字节) + 结束代码(2字节) + 数据
                        if (length >= 11)
                        {
                            // 数据长度在索引7-8位置（低字节在前）
                            ushort responseDataLength = (ushort)(msgArr[7] | (msgArr[8] << 8));
                            
                            // 结束代码在索引9-10位置（低字节在前）
                            byte endCodeLow = msgArr[9];
                            byte endCodeHigh = msgArr[10];
                            ushort endCode = (ushort)(endCodeLow | (endCodeHigh << 8));

                            if (endCode == 0)
                            {
                                // 成功，提取数据
                                // 数据从索引11开始
                                if (length > 11 && responseDataLength >= 2)
                                {
                                    // 实际数据长度 = responseDataLength - 2（减去结束代码的2字节）
                                    int actualDataLength = responseDataLength - 2;
                                    if (actualDataLength > 0 && length >= 11 + actualDataLength)
                                    {
                                        string dataHex = BitConverter.ToString(msgArr, 11, actualDataLength).Replace("-", " ");
                                        ShowMsg("响应数据: " + dataHex);

                                        // 如果是读取操作，显示解析后的值（每个字2字节）
                                        int wordCount = actualDataLength / 2;
                                        if (wordCount > 0)
                                        {
                                            StringBuilder sb = new StringBuilder();
                                            sb.Append("读取值: ");
                                            for (int i = 0; i < wordCount; i++)
                                            {
                                                int value = (msgArr[11 + i * 2] | (msgArr[11 + i * 2 + 1] << 8));
                                                sb.Append(value.ToString());
                                                if (i < wordCount - 1) sb.Append(", ");
                                            }
                                            ShowMsg(sb.ToString());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                ShowMsg(string.Format("错误代码: {0:X4}", endCode));
                            }
                        }
                    }
                    else
                    {
                        isRec = false;
                        ShowMsg("PLC已断开连接");
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                if (isRec)
                {
                    ShowMsg("接收消息出错：" + exp.Message);
                }
            }
            finally
            {
                try
                {
                    if (this.InvokeRequired)
                    {
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
                catch { }
            }
        }

        /// <summary>
        /// 构建MC协议读取命令（3E帧二进制格式）
        /// </summary>
        private byte[] BuildReadCommand(string registerType, int address, int count)
        {
            List<byte> command = new List<byte>();

            // 固定头部
            command.Add(SUBHEADER_LOW); // 副标头低字节
            command.Add(SUBHEADER_HIGH); // 副标头高字节
            command.Add(NETWORK_NUMBER); // 网络编号
            command.Add(PC_NUMBER); // PC编号
            command.Add(DESTINATION_MODULE_IO_LOW); // IO号低字节
            command.Add(DESTINATION_MODULE_IO_HIGH); // IO号高字节
            command.Add(DESTINATION_MODULE_STATION); // 站号

            // 数据长度（后续数据的字节数，低字节在前）
            ushort dataLength = (ushort)(2 + 2 + 2 + 1 + 3 + 2); // 监视定时器(2) + 命令代码(2) + 软元件代码(1) + 地址(3) + 点数(2)
            command.Add((byte)(dataLength & 0xFF)); // 低字节
            command.Add((byte)((dataLength >> 8) & 0xFF)); // 高字节

            // 监视定时器（单位：10ms，低字节在前）
            command.Add((byte)(MONITOR_TIMER & 0xFF)); // 低字节
            command.Add((byte)((MONITOR_TIMER >> 8) & 0xFF)); // 高字节

            // 命令代码（批量读取字: 0104，低字节在前）
            command.Add(0x04);
            command.Add(0x01);

            // 软元件代码
            byte deviceCode = GetDeviceCode(registerType);
            command.Add(deviceCode);

            // 起始地址（3字节，低字节在前）
            command.Add((byte)(address & 0xFF));
            command.Add((byte)((address >> 8) & 0xFF));
            command.Add((byte)((address >> 16) & 0xFF));

            // 读取数量（2字节，低字节在前）
            command.Add((byte)(count & 0xFF));
            command.Add((byte)((count >> 8) & 0xFF));

            return command.ToArray();
        }

        /// <summary>
        /// 构建MC协议写入命令（3E帧二进制格式）
        /// </summary>
        private byte[] BuildWriteCommand(string registerType, int address, int[] values)
        {
            List<byte> command = new List<byte>();

            // 固定头部
            command.Add(SUBHEADER_LOW);
            command.Add(SUBHEADER_HIGH);
            command.Add(NETWORK_NUMBER);
            command.Add(PC_NUMBER);
            command.Add(DESTINATION_MODULE_IO_LOW);
            command.Add(DESTINATION_MODULE_IO_HIGH);
            command.Add(DESTINATION_MODULE_STATION);

            // 数据长度（后续数据的字节数，低字节在前）
            ushort dataLength = (ushort)(2 + 2 + 2 + 1 + 3 + 2 + values.Length * 2); // 监视定时器(2) + 命令代码(2) + 软元件代码(1) + 地址(3) + 点数(2) + 数据
            command.Add((byte)(dataLength & 0xFF));
            command.Add((byte)((dataLength >> 8) & 0xFF));

            // 监视定时器（单位：10ms，低字节在前）
            command.Add((byte)(MONITOR_TIMER & 0xFF));
            command.Add((byte)((MONITOR_TIMER >> 8) & 0xFF));

            // 命令代码（批量写入字: 0141，低字节在前）
            command.Add(0x41);
            command.Add(0x01);

            // 软元件代码
            byte deviceCode = GetDeviceCode(registerType);
            command.Add(deviceCode);

            // 起始地址（3字节，低字节在前）
            command.Add((byte)(address & 0xFF));
            command.Add((byte)((address >> 8) & 0xFF));
            command.Add((byte)((address >> 16) & 0xFF));

            // 写入数量（2字节，低字节在前）
            command.Add((byte)(values.Length & 0xFF));
            command.Add((byte)((values.Length >> 8) & 0xFF));

            // 写入数据（每个字2字节，低字节在前）
            foreach (int value in values)
            {
                command.Add((byte)(value & 0xFF));
                command.Add((byte)((value >> 8) & 0xFF));
            }

            return command.ToArray();
        }

        /// <summary>
        /// 获取设备代码
        /// </summary>
        private byte GetDeviceCode(string registerType)
        {
            switch (registerType.ToUpper())
            {
                case "D": return 0xA8; // 数据寄存器
                case "M": return 0x90; // 内部继电器
                case "X": return 0x9C; // 输入继电器
                case "Y": return 0x9D; // 输出继电器
                case "R": return 0xAF; // 文件寄存器
                case "W": return 0xB4; // 链接寄存器
                case "L": return 0x92; // 锁存继电器
                default: return 0xA8; // 默认D寄存器
            }
        }

        /// <summary>
        /// 读取寄存器
        /// </summary>
        private void btnRead_Click(object sender, EventArgs e)
        {
            if (socket == null || !socket.Connected)
            {
                MessageBox.Show("未连接到PLC，无法读取！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string registerType = (cmbRegisterType.SelectedItem != null) ? cmbRegisterType.SelectedItem.ToString() : "D";
                int address = Convert.ToInt32(txtAddress.Text);
                int count = Convert.ToInt32(txtCount.Text);

                if (count <= 0 || count > 960)
                {
                    MessageBox.Show("读取数量必须在1-960之间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] command = BuildReadCommand(registerType, address, count);
                string cmdHex = BitConverter.ToString(command).Replace("-", " ");
                ShowMsg("发送读取命令: " + cmdHex);

                socket.Send(command);
                ShowMsg(string.Format("读取 {0}{1} 共 {2} 个字", registerType, address, count));
            }
            catch (Exception exp)
            {
                ShowMsg("读取失败：" + exp.Message);
                MessageBox.Show("读取失败：" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 写入寄存器
        /// </summary>
        private void btnWrite_Click(object sender, EventArgs e)
        {
            if (socket == null || !socket.Connected)
            {
                MessageBox.Show("未连接到PLC，无法写入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string registerType = (cmbRegisterType.SelectedItem != null) ? cmbRegisterType.SelectedItem.ToString() : "D";
                int address = Convert.ToInt32(txtAddress.Text);

                // 解析写入值
                string[] valueStrings = txtWriteValue.Text.Split(new char[] { ',', ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                List<int> values = new List<int>();
                foreach (string valStr in valueStrings)
                {
                    values.Add(Convert.ToInt32(valStr.Trim()));
                }

                if (values.Count == 0 || values.Count > 960)
                {
                    MessageBox.Show("写入数量必须在1-960之间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                byte[] command = BuildWriteCommand(registerType, address, values.ToArray());
                string cmdHex = BitConverter.ToString(command).Replace("-", " ");
                ShowMsg("发送写入命令: " + cmdHex);

                socket.Send(command);
                ShowMsg(string.Format("写入 {0}{1} 共 {2} 个字", registerType, address, values.Count));
            }
            catch (Exception exp)
            {
                ShowMsg("写入失败：" + exp.Message);
                MessageBox.Show("写入失败：" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 显示消息
        /// </summary>
        public void ShowMsg(string msgStr)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        Action<string> ShowMsgAction = Msg =>
                        {
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
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBoxReceive.Items.Clear();
        }
    }
}

