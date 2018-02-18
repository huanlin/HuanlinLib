namespace Example
{
    partial class SysInfoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.lbxMacAddresses = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblMachineName = new System.Windows.Forms.Label();
            this.lbxDnsServers = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDnsHostName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblDnsName = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbxIPaddresses = new System.Windows.Forms.ListBox();
            this.btnGetDnsHostNameByIP = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lblNetworkConnected = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 46);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "網路卡:";
            // 
            // lbxMacAddresses
            // 
            this.lbxMacAddresses.FormattingEnabled = true;
            this.lbxMacAddresses.ItemHeight = 15;
            this.lbxMacAddresses.Location = new System.Drawing.Point(12, 64);
            this.lbxMacAddresses.Name = "lbxMacAddresses";
            this.lbxMacAddresses.Size = new System.Drawing.Size(146, 79);
            this.lbxMacAddresses.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Machine name:";
            // 
            // lblMachineName
            // 
            this.lblMachineName.AutoSize = true;
            this.lblMachineName.Location = new System.Drawing.Point(120, 176);
            this.lblMachineName.Name = "lblMachineName";
            this.lblMachineName.Size = new System.Drawing.Size(104, 15);
            this.lblMachineName.TabIndex = 3;
            this.lblMachineName.Text = "lblMachineName";
            // 
            // lbxDnsServers
            // 
            this.lbxDnsServers.FormattingEnabled = true;
            this.lbxDnsServers.ItemHeight = 15;
            this.lbxDnsServers.Location = new System.Drawing.Point(167, 64);
            this.lbxDnsServers.Name = "lbxDnsServers";
            this.lbxDnsServers.Size = new System.Drawing.Size(146, 79);
            this.lbxDnsServers.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(164, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "DNS 伺服器:";
            // 
            // lblDnsHostName
            // 
            this.lblDnsHostName.AutoSize = true;
            this.lblDnsHostName.Location = new System.Drawing.Point(120, 202);
            this.lblDnsHostName.Name = "lblDnsHostName";
            this.lblDnsHostName.Size = new System.Drawing.Size(103, 15);
            this.lblDnsHostName.TabIndex = 7;
            this.lblDnsHostName.Text = "lblDnsHostName";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 15);
            this.label5.TabIndex = 6;
            this.label5.Text = "DNS host name:";
            // 
            // lblDnsName
            // 
            this.lblDnsName.AutoSize = true;
            this.lblDnsName.Location = new System.Drawing.Point(120, 227);
            this.lblDnsName.Name = "lblDnsName";
            this.lblDnsName.Size = new System.Drawing.Size(77, 15);
            this.lblDnsName.TabIndex = 9;
            this.lblDnsName.Text = "lblDnsName";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 15);
            this.label7.TabIndex = 8;
            this.label7.Text = "網域/工作群組:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(316, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "IP address:";
            // 
            // lbxIPaddresses
            // 
            this.lbxIPaddresses.FormattingEnabled = true;
            this.lbxIPaddresses.ItemHeight = 15;
            this.lbxIPaddresses.Location = new System.Drawing.Point(319, 64);
            this.lbxIPaddresses.Name = "lbxIPaddresses";
            this.lbxIPaddresses.Size = new System.Drawing.Size(146, 79);
            this.lbxIPaddresses.TabIndex = 11;
            // 
            // btnGetDnsHostNameByIP
            // 
            this.btnGetDnsHostNameByIP.Font = new System.Drawing.Font("PMingLiU", 9F);
            this.btnGetDnsHostNameByIP.Location = new System.Drawing.Point(319, 149);
            this.btnGetDnsHostNameByIP.Name = "btnGetDnsHostNameByIP";
            this.btnGetDnsHostNameByIP.Size = new System.Drawing.Size(146, 29);
            this.btnGetDnsHostNameByIP.TabIndex = 12;
            this.btnGetDnsHostNameByIP.Text = "以 IP 反查 DNS name";
            this.btnGetDnsHostNameByIP.UseVisualStyleBackColor = true;
            this.btnGetDnsHostNameByIP.Click += new System.EventHandler(this.btnGetDnsHostNameByIP_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "網路連線狀態:";
            // 
            // lblNetworkConnected
            // 
            this.lblNetworkConnected.AutoSize = true;
            this.lblNetworkConnected.Location = new System.Drawing.Point(116, 18);
            this.lblNetworkConnected.Name = "lblNetworkConnected";
            this.lblNetworkConnected.Size = new System.Drawing.Size(130, 15);
            this.lblNetworkConnected.TabIndex = 14;
            this.lblNetworkConnected.Text = "lblNetworkConnected";
            // 
            // SysInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 402);
            this.Controls.Add(this.lblNetworkConnected);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnGetDnsHostNameByIP);
            this.Controls.Add(this.lbxIPaddresses);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblDnsName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblDnsHostName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbxDnsServers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMachineName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbxMacAddresses);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("PMingLiU", 11F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SysInfoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SysInfoForm";
            this.Load += new System.EventHandler(this.SysInfoForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbxMacAddresses;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMachineName;
        private System.Windows.Forms.ListBox lbxDnsServers;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDnsHostName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblDnsName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox lbxIPaddresses;
        private System.Windows.Forms.Button btnGetDnsHostNameByIP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblNetworkConnected;
    }
}