using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace GenPhoneticData
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblMostFreqHan1FileName.Text = "";
            lblMostFreqHan2FileName.Text = "";
            lblSecondFreqHanFileName.Text = "";
            lblPhoneCinFileName.Text = "";
        }

        void ShowFileExistFlag(string filename, Label aLabel)
        {
            if (File.Exists(filename))
            {
                aLabel.Text = "O";
                aLabel.ForeColor = Color.Green;
            }
            else
            {
                aLabel.Text = "X";
                aLabel.ForeColor = Color.Red;
            }
        }

        private void btnBrowsePhoneCin_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtInFileName.Text = dlg.FileName;
                lblPhoneCinFileName.Text = txtInFileName.Text;
                txtOutputPath.Text = Path.GetDirectoryName(txtInFileName.Text);

                ShowFileExistFlag(lblPhoneCinFileName.Text, lblInFile1Exist);
            }
        }

        private void btnBrowseFreqHan_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = Path.GetDirectoryName(txtInFileName.Text);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtFreqHanFolder.Text = dlg.SelectedPath;
                lblMostFreqHan1FileName.Text = txtFreqHanFolder.Text + @"\常用漢字2500 (繁體).txt";
                lblMostFreqHan2FileName.Text = txtFreqHanFolder.Text + @"\一級閱讀字表.txt";
                lblSecondFreqHanFileName.Text = txtFreqHanFolder.Text + @"\次常用字1000 (繁體).txt";

                ShowFileExistFlag(lblMostFreqHan1FileName.Text, lblInFile2Exist);
                ShowFileExistFlag(lblMostFreqHan2FileName.Text, lblInFile3Exist);
                ShowFileExistFlag(lblSecondFreqHanFileName.Text, lblInFile4Exist);
            }
        }

        private void btnPickOutputFile_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = Path.GetDirectoryName(txtInFileName.Text);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtOutputPath.Text = dlg.SelectedPath;
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            GenerateData(lblPhoneCinFileName.Text, lblMostFreqHan1FileName.Text, lblMostFreqHan2FileName.Text, lblSecondFreqHanFileName.Text, txtOutputPath.Text);
        }

        void GenerateData(string phoneCinFileName, string mostFreqHan1FileName, string mostFreqHan2FileName, string secondFreqHanFileName, 
            string outPath)
        {
            if (String.IsNullOrEmpty(phoneCinFileName) || String.IsNullOrEmpty(outPath))
            {
                MessageBox.Show("請指定輸入與輸出檔名!");
                return;
            }

            btnGo.Enabled = false;

            string mostFreqHan1 = File.ReadAllText(mostFreqHan1FileName, Encoding.UTF8);    // 常用字表 1
            string mostFreqHan2 = File.ReadAllText(mostFreqHan2FileName, Encoding.UTF8);    // 常用字表 2
            string secondFreqHan = File.ReadAllText(secondFreqHanFileName, Encoding.UTF8);  // 次常用字表

            txtMsg.Clear();

            List<CharPhon> charList = new List<CharPhon>();
            int linesTotal = 58000;   // Phone.cin 的所有漢字資料筆數的約略值，只是用來計算進度.
            int linesRead = 0;        // 已讀取的漢字資料筆數.

            using (StreamReader sr = new StreamReader(phoneCinFileName, Encoding.UTF8))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    line = line.Trim();
                    if (line.Length <= 1)
                    {
                        line = sr.ReadLine();
                        continue;
                    }
                    if (line[0] == '%') 
                    { 
                        // 參數
                        if (line.Equals("%keyname  begin"))
                        {
                            // 略過整個 keyname 區塊
                            line = sr.ReadLine();
                            while (line != null)
                            {
                                line = line.Trim();
                                if (line.Equals("%keyname  end"))
                                {
                                    break;
                                }
                                line = sr.ReadLine();
                            }
                            if (line == null)
                                break;
                            line = sr.ReadLine();
                            continue;
                        }
                        else if (line.Equals("%chardef  begin"))
                        {
                            linesRead = 0;
                            // 讀取整個字元定義區塊
                            line = sr.ReadLine();                            
                            while (line != null)
                            {
                                line = line.Trim();
                                if (line.Equals("%chardef  end"))
                                {
                                    break;
                                }
                                // 處理字元與注音
                                string[] char_phon = line.Split(new char[] {' '}, 2);
                                if (char_phon.Length == 2)
                                {                                    
                                    string thePhon = char_phon[0];
                                    string theChar = char_phon[1];    // NOTE: Unicode 字元必須以 string 儲存，不可用 char!!
                                    
                                    theChar = StringInfo.GetNextTextElement(char_phon[1]);   // 取出第一個字元

                                    CharPhon aCharPhon = charList.Find(e => e.Character.Equals(theChar));
                                    if (aCharPhon == null )
                                    {
                                        aCharPhon = new CharPhon();
                                        charList.Add(aCharPhon);
                                        aCharPhon.Character = theChar;

                                        // 設定使用頻率
                                        if (mostFreqHan1.IndexOf(theChar) >= 0 || mostFreqHan2.IndexOf(theChar) >= 0)
                                        {
                                            aCharPhon.FreqCode = 0;
                                        }
                                        else if (secondFreqHan.IndexOf(theChar) >= 0)
                                        {
                                            aCharPhon.FreqCode = 1;
                                        }
                                        else
                                        {
                                            aCharPhon.FreqCode = 2;
                                        }
                                    }
                                    aCharPhon.Phonetics.Add(thePhon);

                                    linesRead++;

                                    // 更新狀態列.
                                    statusStrip1.Items[0].Text = aCharPhon.Character;
                                    toolStripProgressBar1.Value = (linesRead * 100) / linesTotal;
                                    Application.DoEvents();
                                }
                                line = sr.ReadLine();
                            }
                            if (line == null)
                                break;
                            line = sr.ReadLine();
                            continue;
                        }
                    }
                    line = sr.ReadLine();
                }
            } // end of using.            

            // 輸出字元注音表
            string fname = outPath + @"\漢字注音對照表.txt";
            using (StreamWriter swChar2Phon = new StreamWriter(fname, false, Encoding.UTF8))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(";UTF-8, Format: Character Frequence(0/1/2) Phonetics");
                sb.Append(Environment.NewLine);

                foreach (CharPhon aCharPhon in charList)
                {
                    sb.Length = 0;
                    sb.Append(aCharPhon.Character);
                    sb.Append(' ');
                    sb.Append(aCharPhon.FreqCode.ToString());
                    foreach (string phon in aCharPhon.Phonetics)
                    {
                        sb.Append(' ');
                        sb.Append(phon);
                    }
                    if (aCharPhon.Phonetics.Count > 9)
                    {
                        txtMsg.AppendText("\r\nWarning: " + aCharPhon.Character);
                    }

                    swChar2Phon.WriteLine(sb.ToString());
                }
            }

            // 把所有多音字儲存在一個文字檔中
            fname = outPath + @"\多音字-全部.txt";
            int multiPhonCharCount = 0;
            using (StreamWriter swMultiPhonChar = new StreamWriter(fname, false, Encoding.UTF8))
            {
                foreach (CharPhon aCharPhon in charList)
                {
                    if (aCharPhon.IsMultiReading)
                    {
                        swMultiPhonChar.Write(aCharPhon.Character);
                        multiPhonCharCount++;
                    }
                }
            }

            // 這個檔案只包含常見的多音字，Unicode point 大於 65535 的都忽略。
            fname = outPath + @"\多音字-常用.txt";
            int freqMultiPhonCharCount = 0;
            using (StreamWriter swMultiPhonChar = new StreamWriter(fname, false, Encoding.UTF8))
            {
                foreach (CharPhon aCharPhon in charList)
                {
                    if (aCharPhon.FreqCode <= 1 && aCharPhon.IsMultiReading)
                    {
                        swMultiPhonChar.Write(aCharPhon.Character);
                        freqMultiPhonCharCount++;
                    }
                }
            }

            statusStrip1.Items[0].Text = "完成!";
            toolStripProgressBar1.Value = 100;
            txtMsg.AppendText("\r\n共有 " + charList.Count.ToString() + " 個字");
            txtMsg.AppendText("\r\n多音字共有 " + multiPhonCharCount.ToString() + " 個");
            txtMsg.AppendText("\r\n常用多音字共有 " + freqMultiPhonCharCount.ToString() + " 個");

            btnGo.Enabled = true;
        }

    }
}
