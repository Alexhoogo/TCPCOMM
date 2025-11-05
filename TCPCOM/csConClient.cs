using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace TCPCOM
{
    public class ConnectionClient
    {
        public Socket sokMsg;//客户端Socket对象
        public Action<string> dgShowMsg;//负责 向主窗体文本框显示消息的方法委托
        public Action<string> dgRemoveConnection;// 负责 从主窗体 中移除 当前连接
        public Thread threadMsg;
        private string remoteEndPointStr; // 保存远程端点字符串，避免访问已释放的对象
        
        public ConnectionClient(Socket sokMsg, Action<string> dgShowMsg, Action<string> dgRemoveConnection)
        {
            this.sokMsg = sokMsg;
            this.dgShowMsg = dgShowMsg;
            this.dgRemoveConnection = dgRemoveConnection;
            
            // 保存远程端点字符串，避免后续访问已释放的对象
            if (sokMsg != null && sokMsg.RemoteEndPoint != null)
            {
                this.remoteEndPointStr = sokMsg.RemoteEndPoint.ToString();
            }

            this.threadMsg = new Thread(RecMsg);
            this.threadMsg.IsBackground = true;
            this.threadMsg.Start();
        }
        public bool isRec = true;
        /// <summary>
        /// 监听客户端发送来的消息
        /// </summary>
        void RecMsg()
        {
            while (isRec)
            {
                try
                {
                    byte[] arrMsg = new byte[1024];
                    //接收 对应 客户端发来的消息
                    int length = sokMsg.Receive(arrMsg);
                    if (length > 0)
                    {
                        //将接收到的消息数组转成字符串
                        string strMsg = System.Text.Encoding.UTF8.GetString(arrMsg, 0, length);

                        //通过委托 显示消息到 窗体的文本框
                        //dgShowMsg(strMsg);//sokMsg.RemoteEndPoint.ToString() + "发来消息：" + 
                        //dgShowMsg(sokMsg.RemoteEndPoint.ToString() + "发来消息 " + strMsg);
                        dgShowMsg(strMsg);
                    }
                    else
                    {
                        isRec = false;
                        // 安全地调用委托，检查对象是否已释放
                        if (dgRemoveConnection != null && !string.IsNullOrEmpty(remoteEndPointStr))
                        {
                            try
                            {
                                dgRemoveConnection(remoteEndPointStr);
                            }
                            catch
                            {
                                // 窗体可能已被释放，忽略错误
                            }
                        }
                        threadMsg.Abort();
                        threadMsg = null;
                    }
                }
                catch
                {
                    isRec = false;
                    // 安全地调用委托，检查对象是否已释放
                    if (dgRemoveConnection != null && !string.IsNullOrEmpty(remoteEndPointStr))
                    {
                        try
                        {
                            dgRemoveConnection(remoteEndPointStr);
                        }
                        catch
                        {
                            // 窗体可能已被释放，忽略错误
                        }
                    }
                    threadMsg.Abort();
                    threadMsg = null;
                }
            }
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="strMsg"></param>
        public void Send(string strMsg)
        {
            if (sokMsg != null && sokMsg.Connected)
            {
                try
                {
                    byte[] arrMsg = System.Text.Encoding.UTF8.GetBytes(strMsg);
                    sokMsg.Send(arrMsg);
                }
                catch //(Exception exp)
                {
                    // 发送失败，可能是连接已断开
                    isRec = false;
                    if (dgRemoveConnection != null && !string.IsNullOrEmpty(remoteEndPointStr))
                    {
                        try
                        {
                            dgRemoveConnection(remoteEndPointStr);
                        }
                        catch
                        {
                            // 窗体可能已被释放，忽略错误
                        }
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
