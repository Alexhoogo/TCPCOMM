using System;
using System.Net.Sockets;
using System.Threading;

namespace TCPCOM
{
    /// <summary>
    /// Modbus TCP连接客户端类
    /// </summary>
    public class ModbusConnectionClient
    {
        public Socket sokMsg;
        public Action<string> dgShowMsg;
        public Action<string> dgRemoveConnection;
        public Action<ModbusConnectionClient, byte[]> dgProcessModbusRequest; // 处理Modbus请求的委托
        public Thread threadMsg;
        private string remoteEndPointStr;
        public bool isRec = true;

        public ModbusConnectionClient(Socket sokMsg, Action<string> dgShowMsg, Action<string> dgRemoveConnection, Action<ModbusConnectionClient, byte[]> dgProcessModbusRequest)
        {
            this.sokMsg = sokMsg;
            this.dgShowMsg = dgShowMsg;
            this.dgRemoveConnection = dgRemoveConnection;
            this.dgProcessModbusRequest = dgProcessModbusRequest;

            if (sokMsg != null && sokMsg.RemoteEndPoint != null)
            {
                this.remoteEndPointStr = sokMsg.RemoteEndPoint.ToString();
            }

            this.threadMsg = new Thread(RecMsg);
            this.threadMsg.IsBackground = true;
            this.threadMsg.Start();
        }

        void RecMsg()
        {
            while (isRec)
            {
                try
                {
                    byte[] arrMsg = new byte[256];
                    int length = sokMsg.Receive(arrMsg);
                    if (length > 0)
                    {
                        // 处理Modbus TCP请求
                        if (dgProcessModbusRequest != null)
                        {
                            byte[] request = new byte[length];
                            Array.Copy(arrMsg, request, length);
                            dgProcessModbusRequest(this, request);
                        }
                    }
                    else
                    {
                        isRec = false;
                        if (dgRemoveConnection != null && !string.IsNullOrEmpty(remoteEndPointStr))
                        {
                            try
                            {
                                dgRemoveConnection(remoteEndPointStr);
                            }
                            catch { }
                        }
                        threadMsg.Abort();
                        threadMsg = null;
                    }
                }
                catch
                {
                    isRec = false;
                    if (dgRemoveConnection != null && !string.IsNullOrEmpty(remoteEndPointStr))
                    {
                        try
                        {
                            dgRemoveConnection(remoteEndPointStr);
                        }
                        catch { }
                    }
                    threadMsg.Abort();
                    threadMsg = null;
                }
            }
        }

        /// <summary>
        /// 发送Modbus响应
        /// </summary>
        public void SendResponse(byte[] response)
        {
            if (sokMsg != null && sokMsg.Connected)
            {
                try
                {
                    sokMsg.Send(response);
                }
                catch
                {
                    isRec = false;
                    if (dgRemoveConnection != null && !string.IsNullOrEmpty(remoteEndPointStr))
                    {
                        try
                        {
                            dgRemoveConnection(remoteEndPointStr);
                        }
                        catch { }
                    }
                    throw;
                }
            }
            else
            {
                throw new Exception("客户端连接已断开");
            }
        }
    }
}

