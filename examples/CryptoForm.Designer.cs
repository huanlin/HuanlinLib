namespace Example
{
    partial class CryptoForm
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboSymmAlgorithm = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSymmetricKey = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSymmetricDecrypted = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSymmetricEncrypted = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSymmetricDecrypt = new System.Windows.Forms.Button();
            this.btnSymmetricEncrypt = new System.Windows.Forms.Button();
            this.txtRawData = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtAsymmDecrypted = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAsymmEncrypted = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAsymmEncrypt = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnVerifySignature = new System.Windows.Forms.Button();
            this.btnSign = new System.Windows.Forms.Button();
            this.txtSignature = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cboSymmAlgorithm);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSymmetricKey);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtSymmetricDecrypted);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSymmetricEncrypted);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnSymmetricDecrypt);
            this.groupBox1.Controls.Add(this.btnSymmetricEncrypt);
            this.groupBox1.Location = new System.Drawing.Point(12, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(550, 110);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "對稱式加密";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(225, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "(建議用 Rijndael)";
            // 
            // cboSymmAlgorithm
            // 
            this.cboSymmAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSymmAlgorithm.FormattingEnabled = true;
            this.cboSymmAlgorithm.Items.AddRange(new object[] {
            "DES",
            "RC2",
            "Rijndael (AES)",
            "Triple DES"});
            this.cboSymmAlgorithm.Location = new System.Drawing.Point(77, 15);
            this.cboSymmAlgorithm.Name = "cboSymmAlgorithm";
            this.cboSymmAlgorithm.Size = new System.Drawing.Size(142, 20);
            this.cboSymmAlgorithm.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "演算法:";
            // 
            // txtSymmetricKey
            // 
            this.txtSymmetricKey.Location = new System.Drawing.Point(400, 15);
            this.txtSymmetricKey.MaxLength = 20;
            this.txtSymmetricKey.Name = "txtSymmetricKey";
            this.txtSymmetricKey.Size = new System.Drawing.Size(133, 22);
            this.txtSymmetricKey.TabIndex = 10;
            this.txtSymmetricKey.Text = "The secret key";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(362, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "金鑰:";
            // 
            // txtSymmetricDecrypted
            // 
            this.txtSymmetricDecrypted.Location = new System.Drawing.Point(77, 71);
            this.txtSymmetricDecrypted.Name = "txtSymmetricDecrypted";
            this.txtSymmetricDecrypted.ReadOnly = true;
            this.txtSymmetricDecrypted.Size = new System.Drawing.Size(268, 22);
            this.txtSymmetricDecrypted.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "解密還原:";
            // 
            // txtSymmetricEncrypted
            // 
            this.txtSymmetricEncrypted.Location = new System.Drawing.Point(77, 43);
            this.txtSymmetricEncrypted.Name = "txtSymmetricEncrypted";
            this.txtSymmetricEncrypted.ReadOnly = true;
            this.txtSymmetricEncrypted.Size = new System.Drawing.Size(456, 22);
            this.txtSymmetricEncrypted.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "加密之後:";
            // 
            // btnSymmetricDecrypt
            // 
            this.btnSymmetricDecrypt.Location = new System.Drawing.Point(453, 71);
            this.btnSymmetricDecrypt.Name = "btnSymmetricDecrypt";
            this.btnSymmetricDecrypt.Size = new System.Drawing.Size(80, 32);
            this.btnSymmetricDecrypt.TabIndex = 2;
            this.btnSymmetricDecrypt.Text = "解密";
            this.btnSymmetricDecrypt.UseVisualStyleBackColor = true;
            this.btnSymmetricDecrypt.Click += new System.EventHandler(this.btnSymmetricDecrypt_Click);
            // 
            // btnSymmetricEncrypt
            // 
            this.btnSymmetricEncrypt.Location = new System.Drawing.Point(364, 71);
            this.btnSymmetricEncrypt.Name = "btnSymmetricEncrypt";
            this.btnSymmetricEncrypt.Size = new System.Drawing.Size(80, 32);
            this.btnSymmetricEncrypt.TabIndex = 1;
            this.btnSymmetricEncrypt.Text = "加密";
            this.btnSymmetricEncrypt.UseVisualStyleBackColor = true;
            this.btnSymmetricEncrypt.Click += new System.EventHandler(this.btnSymmetricEncrypt_Click);
            // 
            // txtRawData
            // 
            this.txtRawData.Location = new System.Drawing.Point(75, 23);
            this.txtRawData.Name = "txtRawData";
            this.txtRawData.Size = new System.Drawing.Size(268, 22);
            this.txtRawData.TabIndex = 4;
            this.txtRawData.Text = "Hello! 密碼學!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "原始資料:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtAsymmDecrypted);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtAsymmEncrypted);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.btnAsymmEncrypt);
            this.groupBox2.Location = new System.Drawing.Point(12, 176);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(550, 115);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "非對稱式加密";
            // 
            // txtAsymmDecrypted
            // 
            this.txtAsymmDecrypted.Location = new System.Drawing.Point(77, 75);
            this.txtAsymmDecrypted.Name = "txtAsymmDecrypted";
            this.txtAsymmDecrypted.Size = new System.Drawing.Size(268, 22);
            this.txtAsymmDecrypted.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "解密還原:";
            // 
            // txtAsymmEncrypted
            // 
            this.txtAsymmEncrypted.Location = new System.Drawing.Point(77, 21);
            this.txtAsymmEncrypted.Multiline = true;
            this.txtAsymmEncrypted.Name = "txtAsymmEncrypted";
            this.txtAsymmEncrypted.Size = new System.Drawing.Size(456, 48);
            this.txtAsymmEncrypted.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "加密之後:";
            // 
            // btnAsymmEncrypt
            // 
            this.btnAsymmEncrypt.Location = new System.Drawing.Point(364, 75);
            this.btnAsymmEncrypt.Name = "btnAsymmEncrypt";
            this.btnAsymmEncrypt.Size = new System.Drawing.Size(142, 32);
            this.btnAsymmEncrypt.TabIndex = 3;
            this.btnAsymmEncrypt.Text = "公鑰加密，私鑰解密";
            this.btnAsymmEncrypt.UseVisualStyleBackColor = true;
            this.btnAsymmEncrypt.Click += new System.EventHandler(this.btnAsymmEncrypt_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnVerifySignature);
            this.groupBox3.Controls.Add(this.btnSign);
            this.groupBox3.Controls.Add(this.txtSignature);
            this.groupBox3.Location = new System.Drawing.Point(15, 297);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(547, 132);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "數位簽章";
            // 
            // btnVerifySignature
            // 
            this.btnVerifySignature.Location = new System.Drawing.Point(85, 21);
            this.btnVerifySignature.Name = "btnVerifySignature";
            this.btnVerifySignature.Size = new System.Drawing.Size(65, 32);
            this.btnVerifySignature.TabIndex = 16;
            this.btnVerifySignature.Text = "驗證簽章";
            this.btnVerifySignature.UseVisualStyleBackColor = true;
            this.btnVerifySignature.Click += new System.EventHandler(this.btnVerifySignature_Click);
            // 
            // btnSign
            // 
            this.btnSign.Location = new System.Drawing.Point(14, 21);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(65, 32);
            this.btnSign.TabIndex = 13;
            this.btnSign.Text = "產生簽章";
            this.btnSign.UseVisualStyleBackColor = true;
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // txtSignature
            // 
            this.txtSignature.Location = new System.Drawing.Point(14, 59);
            this.txtSignature.Multiline = true;
            this.txtSignature.Name = "txtSignature";
            this.txtSignature.Size = new System.Drawing.Size(455, 56);
            this.txtSignature.TabIndex = 12;
            // 
            // CryptoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 425);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtRawData);
            this.Controls.Add(this.label1);
            this.Name = "CryptoForm";
            this.Text = "CryptoForm";
            this.Load += new System.EventHandler(this.CryptoForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSymmetricDecrypt;
        private System.Windows.Forms.Button btnSymmetricEncrypt;
        private System.Windows.Forms.TextBox txtRawData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSymmetricDecrypted;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSymmetricEncrypted;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSymmetricKey;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboSymmAlgorithm;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAsymmEncrypt;
        private System.Windows.Forms.TextBox txtAsymmDecrypted;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtAsymmEncrypted;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.TextBox txtSignature;
        private System.Windows.Forms.Button btnVerifySignature;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}