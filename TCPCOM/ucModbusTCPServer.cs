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
    public partial class ucModbusTCPServer : UserControl
    {
        public Socket socket = null;
        public IPAddress IPAddress = null;
        public IPEndPoint IPPort = null;

        Dictionary<string, ModbusConnectionClient> dictConn = new Dictionary<string, ModbusConnectionClient>();
        Dictionary<string, TreeNode> clientNodes = new Dictionary<string, TreeNode>();

        // TreeView 相关属性
        public TreeNode ServerNode { get; set; }
        public TreeView TreeView { get; set; }

        // Modbus数据存储
        private bool[] coils = new bool[65536]; // 线圈 (0x01, 0x05, 0x0F)
        private bool[] discreteInputs = new bool[65536]; // 离散输入 (0x02)
        private ushort[] holdingRegisters = new ushort[65536]; // 保持寄存器 (0x03, 0x06, 0x10)
        private ushort[] inputRegisters = new ushort[65536]; // 输入寄存器 (0x04)

        // Modbus TCP相关常量
        private const int MODBUS_TCP_PORT = 502;
        private const ushort PROTOCOL_ID = 0x0000;
        private const byte UNIT_ID = 0xFF;

        // 公共属性
        public string ServerPort
        {
            get { return txtPort.Text; }
            set { txtPort.Text = value; }
        }

        // 是否自动启动监听
        private bool autoStart = true;

        public ucModbusTCPServer()
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

            // 初始化数据视图
            InitializeDataView();

            // 自动启动监听
            if (autoStart && !string.IsNullOrEmpty(txtPort.Text))
            {
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

        private void InitializeDataView()
        {
            // 初始化数据网格视图（如果需要）
            // 这里可以添加数据查看和编辑功能
        }

        private void StartListening()
        {
            if (string.IsNullOrEmpty(txtPort.Text))
            {
                ShowMsg("IP 端口错误，请重新输入！");
                UpdateServerStatus(false);
                return;
            }

            try
            {
                IPAddress = System.Net.IPAddress.Any;
                IPPort = new IPEndPoint(IPAddress, Convert.ToInt32(txtPort.Text));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(IPPort);
                socket.Listen(10);

                lblLocalPort.Text = "本地端口:" + IPPort.Port;

                socket.BeginAccept(new AsyncCallback(SocketAccept), socket);

                ShowMsg("Modbus TCP服务器开启成功，开始监听: " + IPPort.ToString());
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
            ShowMsg("Modbus TCP服务器已停止监听");
        }

        private void UpdateServerStatus(bool started)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    if (this.IsHandleCreated && !this.IsDisposed)
                    {
                        this.Invoke(new Action<bool>(UpdateServerStatus), started);
                    }
                    return;
                }

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
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

        public void SocketAccept(IAsyncResult AResult)
        {
            Socket serverSocket = (Socket)AResult.AsyncState;
            Socket clientsocket = null;

            try
            {
                clientsocket = serverSocket.EndAccept(AResult);

                ModbusConnectionClient connection = new ModbusConnectionClient(
                    clientsocket, 
                    ShowMsg, 
                    RemoveClientConnection,
                    ProcessModbusRequest);
                string clientKey = clientsocket.RemoteEndPoint.ToString();
                dictConn.Add(clientKey, connection);

                AddClientToTree(clientKey);

                ShowMsg("Modbus客户端:" + clientKey + " 已连接");
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
        /// 处理Modbus请求
        /// </summary>
        private void ProcessModbusRequest(ModbusConnectionClient client, byte[] request)
        {
            try
            {
                if (request.Length < 9) // MBAP Header(7) + Function Code(1) + 至少1字节数据
                {
                    ShowMsg("收到无效的Modbus请求（长度不足）");
                    return;
                }

                // 解析MBAP Header
                ushort transactionId = (ushort)((request[0] << 8) | request[1]);
                ushort protocolId = (ushort)((request[2] << 8) | request[3]);
                ushort length = (ushort)((request[4] << 8) | request[5]);
                byte unitId = request[6];

                // 验证协议ID
                if (protocolId != PROTOCOL_ID)
                {
                    ShowMsg("收到非Modbus TCP协议数据");
                    return;
                }

                byte functionCode = request[7];
                string requestHex = BitConverter.ToString(request, 0, request.Length).Replace("-", " ");
                ShowMsg("收到请求: " + requestHex);

                byte[] response = null;

                // 处理不同的功能码
                switch (functionCode)
                {
                    case 0x01: // 读线圈状态
                        response = HandleReadCoils(request, transactionId);
                        break;
                    case 0x02: // 读离散输入状态
                        response = HandleReadDiscreteInputs(request, transactionId);
                        break;
                    case 0x03: // 读保持寄存器
                        response = HandleReadHoldingRegisters(request, transactionId);
                        break;
                    case 0x04: // 读输入寄存器
                        response = HandleReadInputRegisters(request, transactionId);
                        break;
                    case 0x05: // 写单个线圈
                        response = HandleWriteSingleCoil(request, transactionId);
                        break;
                    case 0x06: // 写单个寄存器
                        response = HandleWriteSingleRegister(request, transactionId);
                        break;
                    case 0x0F: // 写多个线圈
                        response = HandleWriteMultipleCoils(request, transactionId);
                        break;
                    case 0x10: // 写多个寄存器
                        response = HandleWriteMultipleRegisters(request, transactionId);
                        break;
                    default:
                        // 不支持的功能码
                        response = BuildExceptionResponse(transactionId, functionCode, 0x01); // 非法功能码
                        break;
                }

                if (response != null)
                {
                    client.SendResponse(response);
                    string responseHex = BitConverter.ToString(response).Replace("-", " ");
                    ShowMsg("发送响应: " + responseHex);
                }
            }
            catch (Exception exp)
            {
                ShowMsg("处理Modbus请求时出错：" + exp.Message);
            }
        }

        /// <summary>
        /// 构建Modbus响应
        /// </summary>
        private byte[] BuildModbusResponse(ushort transactionId, byte functionCode, byte[] pduData)
        {
            List<byte> response = new List<byte>();

            // MBAP Header
            response.Add((byte)((transactionId >> 8) & 0xFF));
            response.Add((byte)(transactionId & 0xFF));
            response.Add(0x00); // Protocol ID 高字节
            response.Add(0x00); // Protocol ID 低字节
            ushort length = (ushort)(2 + pduData.Length);
            response.Add((byte)((length >> 8) & 0xFF));
            response.Add((byte)(length & 0xFF));
            response.Add(UNIT_ID);

            // PDU
            response.Add(functionCode);
            response.AddRange(pduData);

            return response.ToArray();
        }

        /// <summary>
        /// 构建异常响应
        /// </summary>
        private byte[] BuildExceptionResponse(ushort transactionId, byte functionCode, byte exceptionCode)
        {
            List<byte> pdu = new List<byte>();
            pdu.Add((byte)(functionCode | 0x80)); // 异常功能码
            pdu.Add(exceptionCode);

            return BuildModbusResponse(transactionId, (byte)(functionCode | 0x80), pdu.ToArray());
        }

        // 实现各种功能码的处理函数
        private byte[] HandleReadCoils(byte[] request, ushort transactionId)
        {
            if (request.Length < 12) return BuildExceptionResponse(transactionId, 0x01, 0x03); // 非法数据值

            ushort startAddress = (ushort)((request[8] << 8) | request[9]);
            ushort quantity = (ushort)((request[10] << 8) | request[11]);

            if (quantity < 1 || quantity > 2000 || startAddress + quantity > 65536)
                return BuildExceptionResponse(transactionId, 0x01, 0x03); // 非法数据值

            int byteCount = (quantity + 7) / 8;
            List<byte> pdu = new List<byte>();
            pdu.Add((byte)byteCount);

            for (int i = 0; i < byteCount; i++)
            {
                byte coilByte = 0;
                for (int j = 0; j < 8; j++)
                {
                    int coilIndex = startAddress + i * 8 + j;
                    if (coilIndex < startAddress + quantity && coils[coilIndex])
                    {
                        coilByte |= (byte)(1 << j);
                    }
                }
                pdu.Add(coilByte);
            }

            return BuildModbusResponse(transactionId, 0x01, pdu.ToArray());
        }

        private byte[] HandleReadDiscreteInputs(byte[] request, ushort transactionId)
        {
            if (request.Length < 12) return BuildExceptionResponse(transactionId, 0x02, 0x03);

            ushort startAddress = (ushort)((request[8] << 8) | request[9]);
            ushort quantity = (ushort)((request[10] << 8) | request[11]);

            if (quantity < 1 || quantity > 2000 || startAddress + quantity > 65536)
                return BuildExceptionResponse(transactionId, 0x02, 0x03);

            int byteCount = (quantity + 7) / 8;
            List<byte> pdu = new List<byte>();
            pdu.Add((byte)byteCount);

            for (int i = 0; i < byteCount; i++)
            {
                byte inputByte = 0;
                for (int j = 0; j < 8; j++)
                {
                    int inputIndex = startAddress + i * 8 + j;
                    if (inputIndex < startAddress + quantity && discreteInputs[inputIndex])
                    {
                        inputByte |= (byte)(1 << j);
                    }
                }
                pdu.Add(inputByte);
            }

            return BuildModbusResponse(transactionId, 0x02, pdu.ToArray());
        }

        private byte[] HandleReadHoldingRegisters(byte[] request, ushort transactionId)
        {
            if (request.Length < 12) return BuildExceptionResponse(transactionId, 0x03, 0x03);

            ushort startAddress = (ushort)((request[8] << 8) | request[9]);
            ushort quantity = (ushort)((request[10] << 8) | request[11]);

            if (quantity < 1 || quantity > 125 || startAddress + quantity > 65536)
                return BuildExceptionResponse(transactionId, 0x03, 0x03);

            List<byte> pdu = new List<byte>();
            pdu.Add((byte)(quantity * 2)); // 字节数

            for (int i = 0; i < quantity; i++)
            {
                ushort value = holdingRegisters[startAddress + i];
                pdu.Add((byte)((value >> 8) & 0xFF));
                pdu.Add((byte)(value & 0xFF));
            }

            return BuildModbusResponse(transactionId, 0x03, pdu.ToArray());
        }

        private byte[] HandleReadInputRegisters(byte[] request, ushort transactionId)
        {
            if (request.Length < 12) return BuildExceptionResponse(transactionId, 0x04, 0x03);

            ushort startAddress = (ushort)((request[8] << 8) | request[9]);
            ushort quantity = (ushort)((request[10] << 8) | request[11]);

            if (quantity < 1 || quantity > 125 || startAddress + quantity > 65536)
                return BuildExceptionResponse(transactionId, 0x04, 0x03);

            List<byte> pdu = new List<byte>();
            pdu.Add((byte)(quantity * 2));

            for (int i = 0; i < quantity; i++)
            {
                ushort value = inputRegisters[startAddress + i];
                pdu.Add((byte)((value >> 8) & 0xFF));
                pdu.Add((byte)(value & 0xFF));
            }

            return BuildModbusResponse(transactionId, 0x04, pdu.ToArray());
        }

        private byte[] HandleWriteSingleCoil(byte[] request, ushort transactionId)
        {
            if (request.Length < 12) return BuildExceptionResponse(transactionId, 0x05, 0x03);

            ushort address = (ushort)((request[8] << 8) | request[9]);
            ushort value = (ushort)((request[10] << 8) | request[11]);

            coils[address] = (value == 0xFF00);

            // 回显请求
            List<byte> pdu = new List<byte>();
            pdu.Add((byte)((address >> 8) & 0xFF));
            pdu.Add((byte)(address & 0xFF));
            pdu.Add((byte)((value >> 8) & 0xFF));
            pdu.Add((byte)(value & 0xFF));

            return BuildModbusResponse(transactionId, 0x05, pdu.ToArray());
        }

        private byte[] HandleWriteSingleRegister(byte[] request, ushort transactionId)
        {
            if (request.Length < 12) return BuildExceptionResponse(transactionId, 0x06, 0x03);

            ushort address = (ushort)((request[8] << 8) | request[9]);
            ushort value = (ushort)((request[10] << 8) | request[11]);

            holdingRegisters[address] = value;

            // 回显请求
            List<byte> pdu = new List<byte>();
            pdu.Add((byte)((address >> 8) & 0xFF));
            pdu.Add((byte)(address & 0xFF));
            pdu.Add((byte)((value >> 8) & 0xFF));
            pdu.Add((byte)(value & 0xFF));

            return BuildModbusResponse(transactionId, 0x06, pdu.ToArray());
        }

        private byte[] HandleWriteMultipleCoils(byte[] request, ushort transactionId)
        {
            if (request.Length < 13) return BuildExceptionResponse(transactionId, 0x0F, 0x03);

            ushort startAddress = (ushort)((request[8] << 8) | request[9]);
            ushort quantity = (ushort)((request[10] << 8) | request[11]);
            byte byteCount = request[12];

            if (quantity < 1 || quantity > 1968 || startAddress + quantity > 65536)
                return BuildExceptionResponse(transactionId, 0x0F, 0x03);

            if (request.Length < 13 + byteCount)
                return BuildExceptionResponse(transactionId, 0x0F, 0x03);

            // 写入线圈
            for (int i = 0; i < byteCount; i++)
            {
                byte coilByte = request[13 + i];
                for (int j = 0; j < 8; j++)
                {
                    int coilIndex = startAddress + i * 8 + j;
                    if (coilIndex < startAddress + quantity)
                    {
                        coils[coilIndex] = ((coilByte >> j) & 0x01) != 0;
                    }
                }
            }

            // 响应
            List<byte> pdu = new List<byte>();
            pdu.Add((byte)((startAddress >> 8) & 0xFF));
            pdu.Add((byte)(startAddress & 0xFF));
            pdu.Add((byte)((quantity >> 8) & 0xFF));
            pdu.Add((byte)(quantity & 0xFF));

            return BuildModbusResponse(transactionId, 0x0F, pdu.ToArray());
        }

        private byte[] HandleWriteMultipleRegisters(byte[] request, ushort transactionId)
        {
            if (request.Length < 13) return BuildExceptionResponse(transactionId, 0x10, 0x03);

            ushort startAddress = (ushort)((request[8] << 8) | request[9]);
            ushort quantity = (ushort)((request[10] << 8) | request[11]);
            byte byteCount = request[12];

            if (quantity < 1 || quantity > 123 || startAddress + quantity > 65536)
                return BuildExceptionResponse(transactionId, 0x10, 0x03);

            if (request.Length < 13 + byteCount || byteCount != quantity * 2)
                return BuildExceptionResponse(transactionId, 0x10, 0x03);

            // 写入寄存器
            for (int i = 0; i < quantity; i++)
            {
                ushort value = (ushort)((request[13 + i * 2] << 8) | request[13 + i * 2 + 1]);
                holdingRegisters[startAddress + i] = value;
            }

            // 响应
            List<byte> pdu = new List<byte>();
            pdu.Add((byte)((startAddress >> 8) & 0xFF));
            pdu.Add((byte)(startAddress & 0xFF));
            pdu.Add((byte)((quantity >> 8) & 0xFF));
            pdu.Add((byte)(quantity & 0xFF));

            return BuildModbusResponse(transactionId, 0x10, pdu.ToArray());
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
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
        }

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

        private void btnClear_Click(object sender, EventArgs e)
        {
            listBoxReceive.Items.Clear();
        }
    }
}

