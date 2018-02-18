namespace GenPhoneticData
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtInFileName = new System.Windows.Forms.TextBox();
            this.btnBrowsePhoneCin = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPickOutputFile = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.txtMsg = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnBrowseFreqHan = new System.Windows.Forms.Button();
            this.txtFreqHanFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.grpInput = new System.Windows.Forms.GroupBox();
            this.lblSecondFreqHanFileName = new System.Windows.Forms.Label();
            this.lblMostFreqHan2FileName = new System.Windows.Forms.Label();
            this.lblMostFreqHan1FileName = new System.Windows.Forms.Label();
            this.lblPhoneCinFileName = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.lblInFile1Exist = new System.Windows.Forms.Label();
            this.lblInFile2Exist = new System.Windows.Forms.Label();
            this.lblInFile3Exist = new System.Windows.Forms.Label();
            this.lblInFile4Exist = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.grpInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "輸入的新酷音資料檔 (phone.cin)：";
            // 
            // txtInFileName
            // 
            this.txtInFileName.Location = new System.Drawing.Point(15, 41);
            this.txtInFileName.Name = "txtInFileName";
            this.txtInFileName.ReadOnly = true;
            this.txtInFileName.Size = new System.Drawing.Size(409, 22);
            this.txtInFileName.TabIndex = 1;
            // 
            // btnBrowsePhoneCin
            // 
            this.btnBrowsePhoneCin.Location = new System.Drawing.Point(430, 41);
            this.btnBrowsePhoneCin.Name = "btnBrowsePhoneCin";
            this.btnBrowsePhoneCin.Size = new System.Drawing.Size(50, 23);
            this.btnBrowsePhoneCin.TabIndex = 2;
            this.btnBrowsePhoneCin.Text = "瀏覽";
            this.btnBrowsePhoneCin.UseVisualStyleBackColor = true;
            this.btnBrowsePhoneCin.Click += new System.EventHandler(this.btnBrowsePhoneCin_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(94, 260);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(355, 22);
            this.txtOutputPath.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 265);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "輸出目錄：";
            // 
            // btnPickOutputFile
            // 
            this.btnPickOutputFile.Location = new System.Drawing.Point(455, 260);
            this.btnPickOutputFile.Name = "btnPickOutputFile";
            this.btnPickOutputFile.Size = new System.Drawing.Size(50, 23);
            this.btnPickOutputFile.TabIndex = 3;
            this.btnPickOutputFile.Text = "瀏覽";
            this.btnPickOutputFile.UseVisualStyleBackColor = true;
            this.btnPickOutputFile.Click += new System.EventHandler(this.btnPickOutputFile_Click);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(28, 298);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "執行";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // txtMsg
            // 
            this.txtMsg.Location = new System.Drawing.Point(25, 338);
            this.txtMsg.Multiline = true;
            this.txtMsg.Name = "txtMsg";
            this.txtMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMsg.Size = new System.Drawing.Size(465, 80);
            this.txtMsg.TabIndex = 5;
            this.txtMsg.Text = "此工具需要讀取新酷音的 UTF-8 版本的 phone.cin 檔案，並且會產生兩個輸出檔案:\r\n\r\n1. 所有漢字注音對照表.txt - 完整的漢字與對應的注音" +
                "。\r\n2. 所有多音字.txt - 此檔案包含所有的多音字（僅字元，不含注音）。\r\n";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 421);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(543, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(60, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // btnBrowseFreqHan
            // 
            this.btnBrowseFreqHan.Location = new System.Drawing.Point(430, 87);
            this.btnBrowseFreqHan.Name = "btnBrowseFreqHan";
            this.btnBrowseFreqHan.Size = new System.Drawing.Size(50, 23);
            this.btnBrowseFreqHan.TabIndex = 5;
            this.btnBrowseFreqHan.Text = "瀏覽";
            this.btnBrowseFreqHan.UseVisualStyleBackColor = true;
            this.btnBrowseFreqHan.Click += new System.EventHandler(this.btnBrowseFreqHan_Click);
            // 
            // txtFreqHanFolder
            // 
            this.txtFreqHanFolder.Location = new System.Drawing.Point(15, 87);
            this.txtFreqHanFolder.Name = "txtFreqHanFolder";
            this.txtFreqHanFolder.ReadOnly = true;
            this.txtFreqHanFolder.Size = new System.Drawing.Size(409, 22);
            this.txtFreqHanFolder.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "常用漢字檔案所在目錄：";
            // 
            // grpInput
            // 
            this.grpInput.Controls.Add(this.lblInFile4Exist);
            this.grpInput.Controls.Add(this.lblInFile3Exist);
            this.grpInput.Controls.Add(this.lblInFile2Exist);
            this.grpInput.Controls.Add(this.lblInFile1Exist);
            this.grpInput.Controls.Add(this.lblSecondFreqHanFileName);
            this.grpInput.Controls.Add(this.lblMostFreqHan2FileName);
            this.grpInput.Controls.Add(this.lblMostFreqHan1FileName);
            this.grpInput.Controls.Add(this.lblPhoneCinFileName);
            this.grpInput.Controls.Add(this.label4);
            this.grpInput.Controls.Add(this.txtInFileName);
            this.grpInput.Controls.Add(this.btnBrowseFreqHan);
            this.grpInput.Controls.Add(this.label1);
            this.grpInput.Controls.Add(this.txtFreqHanFolder);
            this.grpInput.Controls.Add(this.btnBrowsePhoneCin);
            this.grpInput.Controls.Add(this.label3);
            this.grpInput.Location = new System.Drawing.Point(25, 12);
            this.grpInput.Name = "grpInput";
            this.grpInput.Size = new System.Drawing.Size(499, 242);
            this.grpInput.TabIndex = 0;
            this.grpInput.TabStop = false;
            this.grpInput.Text = "輸入";
            // 
            // lblSecondFreqHanFileName
            // 
            this.lblSecondFreqHanFileName.AutoSize = true;
            this.lblSecondFreqHanFileName.Location = new System.Drawing.Point(25, 208);
            this.lblSecondFreqHanFileName.Name = "lblSecondFreqHanFileName";
            this.lblSecondFreqHanFileName.Size = new System.Drawing.Size(33, 12);
            this.lblSecondFreqHanFileName.TabIndex = 10;
            this.lblSecondFreqHanFileName.Text = "label7";
            // 
            // lblMostFreqHan2FileName
            // 
            this.lblMostFreqHan2FileName.AutoSize = true;
            this.lblMostFreqHan2FileName.Location = new System.Drawing.Point(25, 187);
            this.lblMostFreqHan2FileName.Name = "lblMostFreqHan2FileName";
            this.lblMostFreqHan2FileName.Size = new System.Drawing.Size(33, 12);
            this.lblMostFreqHan2FileName.TabIndex = 9;
            this.lblMostFreqHan2FileName.Text = "label6";
            // 
            // lblMostFreqHan1FileName
            // 
            this.lblMostFreqHan1FileName.AutoSize = true;
            this.lblMostFreqHan1FileName.Location = new System.Drawing.Point(25, 166);
            this.lblMostFreqHan1FileName.Name = "lblMostFreqHan1FileName";
            this.lblMostFreqHan1FileName.Size = new System.Drawing.Size(33, 12);
            this.lblMostFreqHan1FileName.TabIndex = 8;
            this.lblMostFreqHan1FileName.Text = "label5";
            // 
            // lblPhoneCinFileName
            // 
            this.lblPhoneCinFileName.AutoSize = true;
            this.lblPhoneCinFileName.Location = new System.Drawing.Point(25, 145);
            this.lblPhoneCinFileName.Name = "lblPhoneCinFileName";
            this.lblPhoneCinFileName.Size = new System.Drawing.Size(33, 12);
            this.lblPhoneCinFileName.TabIndex = 7;
            this.lblPhoneCinFileName.Text = "label5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "所有輸入檔案：";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(300, 16);
            // 
            // lblInFile1Exist
            // 
            this.lblInFile1Exist.AutoSize = true;
            this.lblInFile1Exist.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblInFile1Exist.ForeColor = System.Drawing.Color.Red;
            this.lblInFile1Exist.Location = new System.Drawing.Point(13, 145);
            this.lblInFile1Exist.Name = "lblInFile1Exist";
            this.lblInFile1Exist.Size = new System.Drawing.Size(14, 12);
            this.lblInFile1Exist.TabIndex = 11;
            this.lblInFile1Exist.Text = "X";
            // 
            // lblInFile2Exist
            // 
            this.lblInFile2Exist.AutoSize = true;
            this.lblInFile2Exist.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblInFile2Exist.ForeColor = System.Drawing.Color.Red;
            this.lblInFile2Exist.Location = new System.Drawing.Point(13, 166);
            this.lblInFile2Exist.Name = "lblInFile2Exist";
            this.lblInFile2Exist.Size = new System.Drawing.Size(14, 12);
            this.lblInFile2Exist.TabIndex = 12;
            this.lblInFile2Exist.Text = "X";
            // 
            // lblInFile3Exist
            // 
            this.lblInFile3Exist.AutoSize = true;
            this.lblInFile3Exist.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblInFile3Exist.ForeColor = System.Drawing.Color.Red;
            this.lblInFile3Exist.Location = new System.Drawing.Point(13, 187);
            this.lblInFile3Exist.Name = "lblInFile3Exist";
            this.lblInFile3Exist.Size = new System.Drawing.Size(14, 12);
            this.lblInFile3Exist.TabIndex = 13;
            this.lblInFile3Exist.Text = "X";
            // 
            // lblInFile4Exist
            // 
            this.lblInFile4Exist.AutoSize = true;
            this.lblInFile4Exist.Font = new System.Drawing.Font("PMingLiU", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lblInFile4Exist.ForeColor = System.Drawing.Color.Red;
            this.lblInFile4Exist.Location = new System.Drawing.Point(13, 208);
            this.lblInFile4Exist.Name = "lblInFile4Exist";
            this.lblInFile4Exist.Size = new System.Drawing.Size(14, 12);
            this.lblInFile4Exist.TabIndex = 14;
            this.lblInFile4Exist.Text = "X";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(543, 443);
            this.Controls.Add(this.grpInput);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.txtMsg);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.btnPickOutputFile);
            this.Controls.Add(this.txtOutputPath);
            this.Controls.Add(this.label2);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "產生注音資料檔 by Huanlin Tsai";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.grpInput.ResumeLayout(false);
            this.grpInput.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInFileName;
        private System.Windows.Forms.Button btnBrowsePhoneCin;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPickOutputFile;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtMsg;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button btnBrowseFreqHan;
        private System.Windows.Forms.TextBox txtFreqHanFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox grpInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblSecondFreqHanFileName;
        private System.Windows.Forms.Label lblMostFreqHan2FileName;
        private System.Windows.Forms.Label lblMostFreqHan1FileName;
        private System.Windows.Forms.Label lblPhoneCinFileName;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Label lblInFile4Exist;
        private System.Windows.Forms.Label lblInFile3Exist;
        private System.Windows.Forms.Label lblInFile2Exist;
        private System.Windows.Forms.Label lblInFile1Exist;
    }
}

