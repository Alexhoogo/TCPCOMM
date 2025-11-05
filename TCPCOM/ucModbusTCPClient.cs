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
    public partial class ucModbusTCPClient : UserControl
    {
        public Socket socket = null;
        public Thread threadClient = null;
        bool isRec = true;
        public IPAddress IPAddress = null;
        public IPEndPoint IPPort = null;

        // Modbus TCP相关常量
        private const int MODBUS_TCP_PORT = 502; // Modbus TCP默认端口
        private ushort transactionId = 0; // 事务标识符
        private const ushort PROTOCOL_ID = 0x0000; // 协议标识符（固定0）
        private const byte UNIT_ID = 0xFF; // 单元标识符（通常255）

        // 公共属性
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

        public ucModbusTCPClient()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 设置默认端口
            if (string.IsNullOrEmpty(txtPort.Text))
            {
                txtPort.Text = MODBUS_TCP_PORT.ToString();
            }

            // 初始化功能码下拉框
            if (cmbFunctionCode.Items.Count == 0)
            {
                cmbFunctionCode.Items.AddRange(new string[] { 
                    "0x03 - 读保持寄存器", 
                    "0x04 - 读输入寄存器",
                    "0x06 - 写单个寄存器",
                    "0x10 - 写多个寄存器" 
                });
                cmbFunctionCode.SelectedIndex = 0;
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
                    lblRemoteIP.Text = "服务器IP:" + remoteEP.Address.ToString();
                    lblRemotePort.Text = "服务器端口:" + remoteEP.Port.ToString();
                }

                threadClient = new Thread(ReceiveMsg);
                threadClient.IsBackground = true;
                threadClient.SetApartmentState(ApartmentState.STA);
                threadClient.Start();

                ShowMsg("连接Modbus服务器成功");
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
                ShowMsg("连接Modbus服务器失败：" + exp.Message);
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
            ShowMsg("和Modbus服务器断开连接成功");
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
                    if (btnSend != null) btnSend.Enabled = true;
                }
                else
                {
                    if (lblStatus != null) lblStatus.Text = "已断开";
                    if (lblStatus != null) lblStatus.ForeColor = Color.Red;
                    if (btnConnect != null) btnConnect.Enabled = true;
                    if (btnDisconnect != null) btnDisconnect.Enabled = false;
                    if (lblLocalPort != null) lblLocalPort.Text = "本地端口:0";
                    if (lblRemoteIP != null) lblRemoteIP.Text = "服务器IP:";
                    if (lblRemotePort != null) lblRemotePort.Text = "服务器端口:";
                    if (btnSend != null) btnSend.Enabled = false;
                }
            }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        /// <summary>
        /// 接收Modbus服务器响应
        /// </summary>
        void ReceiveMsg()
        {
            try
            {
                while (isRec && socket != null && socket.Connected)
                {
                    byte[] msgArr = new byte[256];
                    int length = socket.Receive(msgArr);

                    if (length > 0)
                    {
                        // 解析Modbus TCP响应
                        if (length >= 9) // MBAP Header(7) + Function Code(1) + 至少1字节数据
                        {
                            ushort recvTransactionId = (ushort)((msgArr[0] << 8) | msgArr[1]);
                            ushort recvProtocolId = (ushort)((msgArr[2] << 8) | msgArr[3]);
                            ushort recvLength = (ushort)((msgArr[4] << 8) | msgArr[5]);
                            byte recvUnitId = msgArr[6];
                            byte functionCode = msgArr[7];

                            string responseHex = BitConverter.ToString(msgArr, 0, length).Replace("-", " ");
                            ShowMsg("接收: " + responseHex);

                            // 检查异常响应
                            if ((functionCode & 0x80) != 0)
                            {
                                // 异常响应
                                byte exceptionCode = msgArr[8];
                                ShowMsg(string.Format("异常响应 - 功能码: 0x{0:X2}, 异常代码: 0x{1:X2}", functionCode & 0x7F, exceptionCode));
                            }
                            else
                            {
                                // 正常响应
                                switch (functionCode)
                                {
                                    case 0x03: // 读保持寄存器
                                    case 0x04: // 读输入寄存器
                                        if (length >= 10)
                                        {
                                            byte byteCount = msgArr[8];
                                            StringBuilder sb = new StringBuilder();
                                            sb.Append("读取值: ");
                                            for (int i = 0; i < byteCount / 2; i++)
                                            {
                                                if (9 + i * 2 + 1 < length)
                                                {
                                                    ushort value = (ushort)((msgArr[9 + i * 2] << 8) | msgArr[9 + i * 2 + 1]);
                                                    sb.Append(value.ToString());
                                                    if (i < byteCount / 2 - 1) sb.Append(", ");
                                                }
                                            }
                                            ShowMsg(sb.ToString());
                                        }
                                        break;
                                    case 0x06: // 写单个寄存器
                                        if (length >= 12)
                                        {
                                            ushort address = (ushort)((msgArr[8] << 8) | msgArr[9]);
                                            ushort value = (ushort)((msgArr[10] << 8) | msgArr[11]);
                                            ShowMsg(string.Format("写入成功 - 地址: {0}, 值: {1}", address, value));
                                        }
                                        break;
                                    case 0x10: // 写多个寄存器
                                        if (length >= 12)
                                        {
                                            ushort startAddress = (ushort)((msgArr[8] << 8) | msgArr[9]);
                                            ushort quantity = (ushort)((msgArr[10] << 8) | msgArr[11]);
                                            ShowMsg(string.Format("写入成功 - 起始地址: {0}, 数量: {1}", startAddress, quantity));
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        isRec = false;
                        ShowMsg("Modbus服务器已断开连接");
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
        /// 构建Modbus TCP请求
        /// </summary>
        private byte[] BuildModbusRequest(byte functionCode, byte[] pduData)
        {
            List<byte> request = new List<byte>();

            // MBAP Header
            transactionId++;
            request.Add((byte)((transactionId >> 8) & 0xFF)); // Transaction ID 高字节
            request.Add((byte)(transactionId & 0xFF)); // Transaction ID 低字节
            request.Add(0x00); // Protocol ID 高字节
            request.Add(0x00); // Protocol ID 低字节
            ushort length = (ushort)(2 + pduData.Length); // Unit ID(1) + Function Code(1) + PDU Data
            request.Add((byte)((length >> 8) & 0xFF)); // Length 高字节
            request.Add((byte)(length & 0xFF)); // Length 低字节
            request.Add(UNIT_ID); // Unit ID

            // PDU
            request.Add(functionCode); // Function Code
            request.AddRange(pduData); // PDU Data

            return request.ToArray();
        }

        /// <summary>
        /// 发送Modbus请求
        /// </summary>
        private void SendModbusRequest(byte functionCode, byte[] pduData)
        {
            if (socket != null && socket.Connected)
            {
                try
                {
                    byte[] request = BuildModbusRequest(functionCode, pduData);
                    string requestHex = BitConverter.ToString(request).Replace("-", " ");
                    ShowMsg("发送: " + requestHex);
                    
                    int bytesSent = socket.Send(request);
                    ShowMsg("发送成功（" + bytesSent + " 字节）");
                }
                catch (Exception exp)
                {
                    ShowMsg("发送请求失败：" + exp.Message);
                    isRec = false;
                    UpdateConnectionStatus(false);
                }
            }
            else
            {
                ShowMsg("发送请求失败：未连接到Modbus服务器");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (cmbFunctionCode.SelectedItem == null)
            {
                MessageBox.Show("请选择功能码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string functionStr = cmbFunctionCode.SelectedItem.ToString();
            byte functionCode = 0;

            // 解析功能码
            if (functionStr.Contains("0x03"))
                functionCode = 0x03;
            else if (functionStr.Contains("0x04"))
                functionCode = 0x04;
            else if (functionStr.Contains("0x06"))
                functionCode = 0x06;
            else if (functionStr.Contains("0x10"))
                functionCode = 0x10;
            else
            {
                MessageBox.Show("不支持的功能码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (functionCode == 0x03 || functionCode == 0x04) // 读保持寄存器/读输入寄存器
                {
                    ushort startAddress = Convert.ToUInt16(txtStartAddress.Text);
                    ushort quantity = Convert.ToUInt16(txtQuantity.Text);

                    if (quantity < 1 || quantity > 125)
                    {
                        MessageBox.Show("读取数量必须在1-125之间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    List<byte> pdu = new List<byte>();
                    pdu.Add((byte)((startAddress >> 8) & 0xFF)); // 起始地址高字节
                    pdu.Add((byte)(startAddress & 0xFF)); // 起始地址低字节
                    pdu.Add((byte)((quantity >> 8) & 0xFF)); // 数量高字节
                    pdu.Add((byte)(quantity & 0xFF)); // 数量低字节

                    SendModbusRequest(functionCode, pdu.ToArray());
                }
                else if (functionCode == 0x06) // 写单个寄存器
                {
                    ushort address = Convert.ToUInt16(txtStartAddress.Text);
                    ushort value = Convert.ToUInt16(txtValue.Text);

                    List<byte> pdu = new List<byte>();
                    pdu.Add((byte)((address >> 8) & 0xFF)); // 地址高字节
                    pdu.Add((byte)(address & 0xFF)); // 地址低字节
                    pdu.Add((byte)((value >> 8) & 0xFF)); // 值高字节
                    pdu.Add((byte)(value & 0xFF)); // 值低字节

                    SendModbusRequest(functionCode, pdu.ToArray());
                }
                else if (functionCode == 0x10) // 写多个寄存器
                {
                    ushort startAddress = Convert.ToUInt16(txtStartAddress.Text);
                    ushort quantity = Convert.ToUInt16(txtQuantity.Text);

                    if (quantity < 1 || quantity > 123)
                    {
                        MessageBox.Show("写入数量必须在1-123之间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 解析值（逗号分隔）
                    string[] valueStrs = txtValue.Text.Split(',');
                    if (valueStrs.Length != quantity)
                    {
                        MessageBox.Show(string.Format("值的数量必须等于写入数量（{0}）！", quantity), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    List<byte> pdu = new List<byte>();
                    pdu.Add((byte)((startAddress >> 8) & 0xFF)); // 起始地址高字节
                    pdu.Add((byte)(startAddress & 0xFF)); // 起始地址低字节
                    pdu.Add((byte)((quantity >> 8) & 0xFF)); // 数量高字节
                    pdu.Add((byte)(quantity & 0xFF)); // 数量低字节
                    pdu.Add((byte)(quantity * 2)); // 字节数

                    foreach (string valStr in valueStrs)
                    {
                        ushort value = Convert.ToUInt16(valStr.Trim());
                        pdu.Add((byte)((value >> 8) & 0xFF)); // 值高字节
                        pdu.Add((byte)(value & 0xFF)); // 值低字节
                    }

                    SendModbusRequest(functionCode, pdu.ToArray());
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("参数错误：" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

