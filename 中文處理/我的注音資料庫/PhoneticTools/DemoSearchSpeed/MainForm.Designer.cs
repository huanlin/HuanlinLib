namespace DemoSearchSpeed
{
    partial class MainForm
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
            this.btnSelectInputPath = new System.Windows.Forms.Button();
            this.txtPhoneDataPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInFileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnBrowsePhoneCin = new System.Windows.Forms.Button();
            this.lblInFile2Exist = new System.Windows.Forms.Label();
            this.lblInFile1Exist = new System.Windows.Forms.Label();
            this.lblMultiReadingFileName = new System.Windows.Forms.Label();
            this.lblPhoneFileName = new System.Windows.Forms.Label();
            this.btnTestMultiReading = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelectInputPath
            // 
            this.btnSelectInputPath.Location = new System.Drawing.Point(530, 78);
            this.btnSelectInputPath.Name = "btnSelectInputPath";
            this.btnSelectInputPath.Size = new System.Drawing.Size(50, 23);
            this.btnSelectInputPath.TabIndex = 6;
            this.btnSelectInputPath.Text = "瀏覽";
            this.btnSelectInputPath.UseVisualStyleBackColor = true;
            this.btnSelectInputPath.Click += new System.EventHandler(this.btnSelectInputPath_Click);
            // 
            // txtPhoneDataPath
            // 
            this.txtPhoneDataPath.Location = new System.Drawing.Point(169, 78);
            this.txtPhoneDataPath.Name = "txtPhoneDataPath";
            this.txtPhoneDataPath.ReadOnly = true;
            this.txtPhoneDataPath.Size = new System.Drawing.Size(355, 22);
            this.txtPhoneDataPath.TabIndex = 5;
            this.txtPhoneDataPath.Text = "D:\\Projects\\HuanlinLib\\中文處理\\我的注音資料庫";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(137, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "注音資料檔案所在目錄：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "注音資料檔：";
            // 
            // txtInFileName
            // 
            this.txtInFileName.Location = new System.Drawing.Point(28, 44);
            this.txtInFileName.Name = "txtInFileName";
            this.txtInFileName.ReadOnly = true;
            this.txtInFileName.Size = new System.Drawing.Size(496, 22);
            this.txtInFileName.TabIndex = 9;
            this.txtInFileName.Text = "D:\\Projects\\HuanlinLib\\中文處理\\我的注音資料庫\\PhoneticTools\\DemoSearchSpeed\\測試資料.txt";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "測試資料檔（UTF-8 編碼）：";
            // 
            // btnBrowsePhoneCin
            // 
            this.btnBrowsePhoneCin.Location = new System.Drawing.Point(530, 44);
            this.btnBrowsePhoneCin.Name = "btnBrowsePhoneCin";
            this.btnBrowsePhoneCin.Size = new System.Drawing.Size(50, 23);
            this.btnBrowsePhoneCin.TabIndex = 10;
            this.btnBrowsePhoneCin.Text = "瀏覽";
            this.btnBrowsePhoneCin.UseVisualStyleBackColor = true;
            this.btnBrowsePhoneCin.Click += new System.EventHandler(this.btnBrowsePhoneCin_Click);
            // 
            // lblInFile2Exist
            // 
            this.lblInFile2Exist.AutoSize = true;
            this.lblInFile2Exist.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblInFile2Exist.ForeColor = System.Drawing.Color.Red;
            this.lblInFile2Exist.Location = new System.Drawing.Point(63, 152);
            this.lblInFile2Exist.Name = "lblInFile2Exist";
            this.lblInFile2Exist.Size = new System.Drawing.Size(14, 12);
            this.lblInFile2Exist.TabIndex = 18;
            this.lblInFile2Exist.Text = "X";
            // 
            // lblInFile1Exist
            // 
            this.lblInFile1Exist.AutoSize = true;
            this.lblInFile1Exist.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblInFile1Exist.ForeColor = System.Drawing.Color.Red;
            this.lblInFile1Exist.Location = new System.Drawing.Point(63, 131);
            this.lblInFile1Exist.Name = "lblInFile1Exist";
            this.lblInFile1Exist.Size = new System.Drawing.Size(14, 12);
            this.lblInFile1Exist.TabIndex = 17;
            this.lblInFile1Exist.Text = "X";
            // 
            // lblMultiReadingFileName
            // 
            this.lblMultiReadingFileName.AutoSize = true;
            this.lblMultiReadingFileName.Location = new System.Drawing.Point(75, 153);
            this.lblMultiReadingFileName.Name = "lblMultiReadingFileName";
            this.lblMultiReadingFileName.Size = new System.Drawing.Size(33, 12);
            this.lblMultiReadingFileName.TabIndex = 15;
            this.lblMultiReadingFileName.Text = "label5";
            // 
            // lblPhoneFileName
            // 
            this.lblPhoneFileName.AutoSize = true;
            this.lblPhoneFileName.Location = new System.Drawing.Point(75, 132);
            this.lblPhoneFileName.Name = "lblPhoneFileName";
            this.lblPhoneFileName.Size = new System.Drawing.Size(33, 12);
            this.lblPhoneFileName.TabIndex = 14;
            this.lblPhoneFileName.Text = "label5";
            // 
            // btnTestMultiReading
            // 
            this.btnTestMultiReading.Location = new System.Drawing.Point(28, 185);
            this.btnTestMultiReading.Name = "btnTestMultiReading";
            this.btnTestMultiReading.Size = new System.Drawing.Size(169, 23);
            this.btnTestMultiReading.TabIndex = 19;
            this.btnTestMultiReading.Text = "測試多音字查詢速度";
            this.btnTestMultiReading.UseVisualStyleBackColor = true;
            this.btnTestMultiReading.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(28, 224);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(552, 169);
            this.txtMsg.TabIndex = 20;
            this.txtMsg.Text = "結果\r\n";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 418);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnTestMultiReading);
            this.Controls.Add(this.lblInFile2Exist);
            this.Controls.Add(this.lblInFile1Exist);
            this.Controls.Add(this.lblMultiReadingFileName);
            this.Controls.Add(this.lblPhoneFileName);
            this.Controls.Add(this.txtInFileName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBrowsePhoneCin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectInputPath);
            this.Controls.Add(this.txtPhoneDataPath);
            this.Controls.Add(this.label2);
            this.Name = "MainForm";
            this.Text = "展示注音搜尋的速度";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectInputPath;
        private System.Windows.Forms.TextBox txtPhoneDataPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowsePhoneCin;
        private System.Windows.Forms.Label lblInFile2Exist;
        private System.Windows.Forms.Label lblInFile1Exist;
        private System.Windows.Forms.Label lblMultiReadingFileName;
        private System.Windows.Forms.Label lblPhoneFileName;
        private System.Windows.Forms.Button btnTestMultiReading;
        private System.Windows.Forms.TextBox txtMsg;
    }
}

