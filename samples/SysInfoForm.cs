using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Huanlin.Sys;

namespace Example
{
    public partial class SysInfoForm : Form
    {
        public SysInfoForm()
        {
            InitializeComponent();
        }

        private void SysInfoForm_Load(object sender, EventArgs e)
        {
            if (SysInfo.IsNetworkConnected())
            {
                lblNetworkConnected.Text = "已連接";
            }
            else
            {
                lblNetworkConnected.Text = "未連接";
            }

            lbxMacAddresses.Items.Clear();
            lbxMacAddresses.Items.AddRange(SysInfo.GetMacAddresses());

            lbxDnsServers.Items.Clear();
            lbxDnsServers.Items.AddRange(SysInfo.GetDnsServers());

            lbxIPaddresses.Items.Clear();
            lbxIPaddresses.Items.AddRange(SysInfo.GetIPAddresses());

            lblMachineName.Text = System.Environment.MachineName;
            lblDnsHostName.Text = SysInfo.GetDnsHostName();
            lblDnsName.Text = SysInfo.GetDnsName();
        }

        private void btnGetDnsHostNameByIP_Click(object sender, EventArgs e)
        {
            if (lbxIPaddresses.SelectedIndex < 0)
                return;
            string host = SysInfo.GetDnsHostName(lbxIPaddresses.SelectedItem.ToString());
            MessageBox.Show(host);
        }
    }
}