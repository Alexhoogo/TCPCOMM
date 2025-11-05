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
    public partial class DialogCreateModbusTCPServer : Form
    {
        public string Port { get; private set; }

        public DialogCreateModbusTCPServer()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // 验证端口
            if (string.IsNullOrWhiteSpace(txtPort.Text))
            {
                MessageBox.Show("请输入监听端口！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPort.Focus();
                return;
            }

            int port;
            if (!int.TryParse(txtPort.Text.Trim(), out port) || port < 1 || port > 65535)
            {
                MessageBox.Show("端口号必须在1-65535之间！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPort.Focus();
                return;
            }

            Port = txtPort.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void DialogCreateModbusTCPServer_Load(object sender, EventArgs e)
        {
            // 设置默认值（Modbus TCP默认端口502）
            txtPort.Text = "502";
        }
    }
}

