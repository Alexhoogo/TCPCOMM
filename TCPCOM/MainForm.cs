using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace TCPCOM
{
    public partial class MainForm : Form
    {
        // 存储创建的Socket实例
        private Dictionary<TreeNode, Control> socketForms = new Dictionary<TreeNode, Control>();
        private int clientIndex = 0;
        private int serverIndex = 0;
        private int plcIndex = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 初始化TreeView，添加TCP Client、TCP Server、基恩士PLC和三菱PLC节点
            TreeNode tcpClientNode = new TreeNode("TCP Client");
            tcpClientNode.Tag = "TCPClient";
            
            TreeNode tcpServerNode = new TreeNode("TCP Server");
            tcpServerNode.Tag = "TCPServer";

            TreeNode keyencePLCNode = new TreeNode("KeyencePLC");
            keyencePLCNode.Tag = "KeyencePLC";

            TreeNode mitsubishiPLCNode = new TreeNode("MitsubishiPLC");
            mitsubishiPLCNode.Tag = "MitsubishiPLC";

            TreeNode modbusTCPClientNode = new TreeNode("Modbus TCP Client");
            modbusTCPClientNode.Tag = "ModbusTCPClient";

            TreeNode modbusTCPServerNode = new TreeNode("Modbus TCP Server");
            modbusTCPServerNode.Tag = "ModbusTCPServer";

            treeViewSocket.Nodes.Add(tcpClientNode);
            treeViewSocket.Nodes.Add(tcpServerNode);
            treeViewSocket.Nodes.Add(keyencePLCNode);
            treeViewSocket.Nodes.Add(mitsubishiPLCNode);
            treeViewSocket.Nodes.Add(modbusTCPClientNode);
            treeViewSocket.Nodes.Add(modbusTCPServerNode);

            // 展开节点
            treeViewSocket.ExpandAll();
            
            // 禁用删除按钮（初始时没有选中项）
            btnDelete.Enabled = false;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            // 检查是否有选中的节点
            if (treeViewSocket.SelectedNode == null)
            {
                MessageBox.Show("请先选择要创建的Socket类型（TCP Client、TCP Server、基恩士PLC、三菱PLC、Modbus TCP Client 或 Modbus TCP Server）！", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TreeNode selectedNode = treeViewSocket.SelectedNode;
            string nodeTag = selectedNode.Tag as string;

            // 如果选中的是子节点，使用其父节点
            if (nodeTag == "ClientInstance" || nodeTag == "ServerInstance" || nodeTag == "PLCInstance")
            {
                if (selectedNode.Parent != null)
                {
                    selectedNode = selectedNode.Parent;
                    nodeTag = selectedNode.Tag as string;
                }
                else
                {
                    MessageBox.Show("请选择TCP Client、TCP Server、基恩士PLC、三菱PLC、Modbus TCP Client或Modbus TCP Server节点后再点击创建！", "提示", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (nodeTag == "TCPClient")
            {
                // 创建TCP Client对话框
                DialogCreateClient dialog = new DialogCreateClient();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string ip = dialog.IPAddress;
                    string port = dialog.Port;
                    
                    // 创建子节点
                    clientIndex++;
                    string nodeText = string.Format("{0}[{1}]", ip, port);
                    TreeNode childNode = new TreeNode(nodeText);
                    childNode.Tag = "ClientInstance";
                    selectedNode.Nodes.Add(childNode);
                    treeViewSocket.SelectedNode = childNode;
                    treeViewSocket.ExpandAll();

                    // 创建ucSocketClient控件并添加到Panel中
                    ucSocketClient clientControl = new ucSocketClient();
                    clientControl.Dock = DockStyle.None;
                    
                    // 设置IP和端口
                    clientControl.ClientIP = ip;
                    clientControl.ClientPort = port;

                    panelContent.Controls.Clear();
                    panelContent.Controls.Add(clientControl);

                    // 保存引用
                    socketForms[childNode] = clientControl;
                    
                    btnDelete.Enabled = true;
                }
                dialog.Dispose();
            }
            else if (nodeTag == "TCPServer")
            {
                // 创建TCP Server对话框
                DialogCreateServer dialog = new DialogCreateServer();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string port = dialog.Port;
                    
                    // 创建子节点
                    serverIndex++;
                    string nodeText = string.Format("监听端口[{0}]", port);
                    TreeNode childNode = new TreeNode(nodeText);
                    childNode.Tag = "ServerInstance";
                    selectedNode.Nodes.Add(childNode);
                    treeViewSocket.SelectedNode = childNode;
                    treeViewSocket.ExpandAll();

                    // 创建ucSocketServer控件并添加到Panel中
                    ucSocketServer serverControl = new ucSocketServer();
                    serverControl.Dock = DockStyle.None;
                    
                    // 设置端口（默认IP为0.0.0.0，绑定到所有接口）
                    serverControl.ServerPort = port;
                    
                    // 设置 TreeView 和 ServerNode 引用
                    serverControl.TreeView = treeViewSocket;
                    serverControl.ServerNode = childNode;

                    panelContent.Controls.Clear();
                    panelContent.Controls.Add(serverControl);

                    // 保存引用
                    socketForms[childNode] = serverControl;
                    
                    btnDelete.Enabled = true;
                }
                dialog.Dispose();
            }
            else if (nodeTag == "KeyencePLC")
            {
                // 创建基恩士PLC对话框
                DialogCreatePLC dialog = new DialogCreatePLC();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string ip = dialog.IPAddress;
                    string port = dialog.Port;
                    
                    // 创建子节点
                    plcIndex++;
                    string nodeText = string.Format("PLC[{0}:{1}]", ip, port);
                    TreeNode childNode = new TreeNode(nodeText);
                    childNode.Tag = "PLCInstance";
                    selectedNode.Nodes.Add(childNode);
                    treeViewSocket.SelectedNode = childNode;
                    treeViewSocket.ExpandAll();

                    // 创建ucKeyencePLC控件并添加到Panel中
                    ucKeyencePLC plcControl = new ucKeyencePLC();
                    plcControl.Dock = DockStyle.None;
                    
                    // 设置IP和端口
                    plcControl.PLCIP = ip;
                    plcControl.PLCPort = port;

                    panelContent.Controls.Clear();
                    panelContent.Controls.Add(plcControl);

                    // 保存引用
                    socketForms[childNode] = plcControl;
                    
                    btnDelete.Enabled = true;
                }
                dialog.Dispose();
            }
            else if (nodeTag == "MitsubishiPLC")
            {
                // 创建三菱PLC对话框
                DialogCreateMitsubishiPLC dialog = new DialogCreateMitsubishiPLC();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string ip = dialog.IPAddress;
                    string port = dialog.Port;
                    
                    // 创建子节点
                    plcIndex++;
                    string nodeText = string.Format("PLC[{0}:{1}]", ip, port);
                    TreeNode childNode = new TreeNode(nodeText);
                    childNode.Tag = "PLCInstance";
                    selectedNode.Nodes.Add(childNode);
                    treeViewSocket.SelectedNode = childNode;
                    treeViewSocket.ExpandAll();

                    // 创建ucMitsubishiPLC控件并添加到Panel中
                    ucMitsubishiPLC plcControl = new ucMitsubishiPLC();
                    plcControl.Dock = DockStyle.None;
                    
                    // 设置IP和端口
                    plcControl.PLCIP = ip;
                    plcControl.PLCPort = port;

                    panelContent.Controls.Clear();
                    panelContent.Controls.Add(plcControl);

                    // 保存引用
                    socketForms[childNode] = plcControl;
                    
                    btnDelete.Enabled = true;
                }
                dialog.Dispose();
            }
            else if (nodeTag == "ModbusTCPClient")
            {
                // 创建Modbus TCP客户端对话框
                DialogCreateModbusTCPClient dialog = new DialogCreateModbusTCPClient();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string ip = dialog.IPAddress;
                    string port = dialog.Port;
                    
                    // 创建子节点
                    clientIndex++;
                    string nodeText = string.Format("Modbus[{0}:{1}]", ip, port);
                    TreeNode childNode = new TreeNode(nodeText);
                    childNode.Tag = "ClientInstance";
                    selectedNode.Nodes.Add(childNode);
                    treeViewSocket.SelectedNode = childNode;
                    treeViewSocket.ExpandAll();

                    // 创建ucModbusTCPClient控件并添加到Panel中
                    ucModbusTCPClient clientControl = new ucModbusTCPClient();
                    clientControl.Dock = DockStyle.None;
                    
                    // 设置IP和端口
                    clientControl.ClientIP = ip;
                    clientControl.ClientPort = port;

                    panelContent.Controls.Clear();
                    panelContent.Controls.Add(clientControl);

                    // 保存引用
                    socketForms[childNode] = clientControl;
                    
                    btnDelete.Enabled = true;
                }
                dialog.Dispose();
            }
            else if (nodeTag == "ModbusTCPServer")
            {
                // 创建Modbus TCP服务器对话框
                DialogCreateModbusTCPServer dialog = new DialogCreateModbusTCPServer();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string port = dialog.Port;
                    
                    // 创建子节点
                    serverIndex++;
                    string nodeText = string.Format("监听端口[{0}]", port);
                    TreeNode childNode = new TreeNode(nodeText);
                    childNode.Tag = "ServerInstance";
                    selectedNode.Nodes.Add(childNode);
                    treeViewSocket.SelectedNode = childNode;
                    treeViewSocket.ExpandAll();

                    // 创建ucModbusTCPServer控件并添加到Panel中
                    ucModbusTCPServer serverControl = new ucModbusTCPServer();
                    serverControl.Dock = DockStyle.None;
                    
                    // 设置端口
                    serverControl.ServerPort = port;
                    
                    // 设置 TreeView 和 ServerNode 引用
                    serverControl.TreeView = treeViewSocket;
                    serverControl.ServerNode = childNode;

                    panelContent.Controls.Clear();
                    panelContent.Controls.Add(serverControl);

                    // 保存引用
                    socketForms[childNode] = serverControl;
                    
                    btnDelete.Enabled = true;
                }
                dialog.Dispose();
            }
            else
            {
                MessageBox.Show("请选择TCP Client、TCP Server、基恩士PLC、三菱PLC、Modbus TCP Client或Modbus TCP Server节点后再点击创建！", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (treeViewSocket.SelectedNode == null)
            {
                MessageBox.Show("请选择要删除的Socket实例！", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TreeNode selectedNode = treeViewSocket.SelectedNode;
            
            // 只能删除实例节点，不能删除父节点
            if (selectedNode.Tag as string == "ClientInstance" || selectedNode.Tag as string == "ServerInstance" || selectedNode.Tag as string == "PLCInstance")
            {
                DialogResult result = MessageBox.Show("确定要删除选中的Socket实例吗？", "确认删除", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    // 释放控件
                    if (socketForms.ContainsKey(selectedNode))
                    {
                        Control controlToDispose = socketForms[selectedNode];
                        // 先从父容器中移除
                        if (controlToDispose.Parent != null)
                        {
                            controlToDispose.Parent.Controls.Remove(controlToDispose);
                        }
                        controlToDispose.Dispose();
                        socketForms.Remove(selectedNode);
                    }

                    // 从树中移除节点
                    selectedNode.Remove();
                    
                    // 清空右侧面板
                    panelContent.Controls.Clear();
                    
                    // 如果没有实例了，禁用删除按钮
                    bool hasInstance = false;
                    foreach (TreeNode node in treeViewSocket.Nodes)
                    {
                        if (node.Nodes.Count > 0)
                        {
                            hasInstance = true;
                            break;
                        }
                    }
                    btnDelete.Enabled = hasInstance;
                }
            }
            else
            {
                MessageBox.Show("只能删除Socket实例，不能删除父节点！", "提示", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // 释放所有Socket控件
            foreach (var kvp in socketForms)
            {
                try
                {
                    if (kvp.Value.Parent != null)
                    {
                        kvp.Value.Parent.Controls.Remove(kvp.Value);
                    }
                    kvp.Value.Dispose();
                }
                catch { }
            }
            socketForms.Clear();
            
            Application.Exit();
        }

        private void treeViewSocket_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selectedNode = e.Node;
            
            if (selectedNode == null)
            {
                panelContent.Controls.Clear();
                btnDelete.Enabled = false;
                return;
            }

            string nodeTag = selectedNode.Tag as string;
            
            // 如果选择的是实例节点，显示对应的窗体
            if (nodeTag == "ClientInstance" || nodeTag == "ServerInstance" || nodeTag == "PLCInstance")
            {
                if (socketForms.ContainsKey(selectedNode))
                {
                    panelContent.Controls.Clear();
                    panelContent.Controls.Add(socketForms[selectedNode]);
                    btnDelete.Enabled = true;
                }
            }
            else
            {
                // 选择的是父节点，清空右侧面板
                panelContent.Controls.Clear();
                btnDelete.Enabled = false;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 释放所有Socket控件
            foreach (var kvp in socketForms)
            {
                try
                {
                    if (kvp.Value.Parent != null)
                    {
                        kvp.Value.Parent.Controls.Remove(kvp.Value);
                    }
                    kvp.Value.Dispose();
                }
                catch { }
            }
            socketForms.Clear();
        }

        private void btn_about_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("本软件免费使用"+"\r\n");
            sb.AppendLine("版本号：V1.0" + "\r\n");
            sb.AppendLine("发布日期：2025.11.04" + "\r\n");
            sb.AppendLine("Designed by Alex Hu");

            MessageBox.Show(sb.ToString(), "关于");
        }
    }
}

