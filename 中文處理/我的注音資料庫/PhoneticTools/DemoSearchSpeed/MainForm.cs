using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Collections;
using Huanlin.TextServices;

namespace DemoSearchSpeed
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
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

        void ShowPhoneticFileNames()
        {
            lblPhoneFileName.Text = txtPhoneDataPath.Text + @"\漢字注音對照表.txt";
            lblMultiReadingFileName.Text = txtPhoneDataPath.Text + @"\多音字-常用.txt";

            ShowFileExistFlag(lblPhoneFileName.Text, lblInFile1Exist);
            ShowFileExistFlag(lblMultiReadingFileName.Text, lblInFile2Exist);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowPhoneticFileNames();

            string str = "ㄅ"; //𩗴";
            int codePoint = Char.ConvertToUtf32(str, 0);
            int[] info = StringInfo.ParseCombiningCharacters(str);
            if (codePoint >= 0x3105 && codePoint <= 0x3129) // ㄅㄆㄇㄈ
                Text = "Y";
            if (codePoint >= 0x4e00 && codePoint <= 0x9fcb)
                Text = "Y";
            else if (codePoint >= 0x3400 && codePoint <= 0x4db5)
                Text = "Y";
            else if (codePoint >= 0x20000 && codePoint <= 0x2a2d6)
                Text = "Y";
            else if (codePoint >= 0x2a700 && codePoint <= 0x2b734)
                Text = "Y";
        }

        private void btnBrowsePhoneCin_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtInFileName.Text = dlg.FileName;
            }

        }

        private void btnSelectInputPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = Path.GetDirectoryName(txtInFileName.Text);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtPhoneDataPath.Text = dlg.SelectedPath;
                ShowPhoneticFileNames();
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            TestMultiReading();
        }

        void TestMultiReading()
        {
            txtMsg.Clear();
            TestMultiReadingWithString();
            TestMultiReadingWithSortedList();
            TestMultiReadingWithHashSet();
            TestMultiReadingWithPhoneticData();
        }

        private void TestMultiReadingWithPhoneticData()
        {
            // 讀取注音資料檔.
            ChinesePhoneticData phoneData = new ChinesePhoneticData();
            phoneData.LoadFromFile(lblPhoneFileName.Text);

            DateTime startTime = DateTime.Now;
            int multiReadingCount = 0;
            using (StreamReader sr = new StreamReader(txtInFileName.Text))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    TextElementEnumerator itor = StringInfo.GetTextElementEnumerator(line);
                    while (itor.MoveNext())
                    {
                        string hanChar = itor.GetTextElement();
                        if (phoneData.IsMultiReading(hanChar))
                        {
                            multiReadingCount++;
                        }
                    }
                    line = sr.ReadLine();
                }
            }
            DateTime endTime = DateTime.Now;
            txtMsg.AppendText("\r\n判斷多音字-使用 Huanlin.TextServices.ChinesePhoneticData 類別：");
            txtMsg.AppendText("\r\n  共發現 " + multiReadingCount + " 個多音字。");
            txtMsg.AppendText("\r\n  時間： " + (endTime - startTime).ToString());
        }

        private void TestMultiReadingWithHashSet()
        {
            // 讀取多音字資料檔.
            HashSet<string> multiReadingChars = new HashSet<string>();
            string chars = File.ReadAllText(lblMultiReadingFileName.Text);
            TextElementEnumerator itorInput = StringInfo.GetTextElementEnumerator(chars);
            while (itorInput.MoveNext())
            {
                string hanChar = itorInput.GetTextElement();
                multiReadingChars.Add(hanChar);
            }

            DateTime startTime = DateTime.Now;
            int multiReadingCount = 0;
            using (StreamReader sr = new StreamReader(txtInFileName.Text))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    TextElementEnumerator itor = StringInfo.GetTextElementEnumerator(line);
                    while (itor.MoveNext())
                    {
                        string hanChar = itor.GetTextElement();
                        if (multiReadingChars.Contains(hanChar))
                        {
                            multiReadingCount++;
                        }
                    }
                    line = sr.ReadLine();
                }
            }
            DateTime endTime = DateTime.Now;
            txtMsg.AppendText("\r\n判斷多音字-使用 HashSet 類別 (.NET 3.5+)：");
            txtMsg.AppendText("\r\n  共發現 " + multiReadingCount + " 個多音字。");
            txtMsg.AppendText("\r\n  時間： " + (endTime - startTime).ToString());
        }

        private void TestMultiReadingWithSortedList()
        {
            // 讀取多音字資料檔.
            SortedList<string, byte> multiReadingChars = new SortedList<string, byte>();
            string chars = File.ReadAllText(lblMultiReadingFileName.Text);
            TextElementEnumerator itorInput = StringInfo.GetTextElementEnumerator(chars);
            while (itorInput.MoveNext())
            {
                string hanChar = itorInput.GetTextElement();
                multiReadingChars.Add(hanChar, 0);
            }

            DateTime startTime = DateTime.Now;
            int multiReadingCount = 0;
            using (StreamReader sr = new StreamReader(txtInFileName.Text))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    TextElementEnumerator itor = StringInfo.GetTextElementEnumerator(line);
                    while (itor.MoveNext())
                    {
                        string hanChar = itor.GetTextElement();
                        if (multiReadingChars.ContainsKey(hanChar))
                        {
                            multiReadingCount++;
                        }
                    }
                    line = sr.ReadLine();
                }
            }
            DateTime endTime = DateTime.Now;
            txtMsg.AppendText("\r\n判斷多音字-使用 SortedList 類別：");
            txtMsg.AppendText("\r\n  共發現 " + multiReadingCount + " 個多音字。");
            txtMsg.AppendText("\r\n  時間： " + (endTime - startTime).ToString());
        }

        private void TestMultiReadingWithString()
        {
            // 讀取多音字資料檔.
            string multiReadingChars = File.ReadAllText(lblMultiReadingFileName.Text);

            DateTime startTime = DateTime.Now;
            int multiReadingCount = 0;
            using (StreamReader sr = new StreamReader(txtInFileName.Text))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    TextElementEnumerator itor = StringInfo.GetTextElementEnumerator(line);
                    while (itor.MoveNext())
                    {
                        string hanChar = itor.GetTextElement();
                        if (multiReadingChars.IndexOf(hanChar) >= 0)
                        {
                            multiReadingCount++;
                        }
                    }
                    line = sr.ReadLine();
                }
            }
            DateTime endTime = DateTime.Now;
            txtMsg.AppendText("\r\n判斷多音字-使用 String 類別：");
            txtMsg.AppendText("\r\n  共發現 " + multiReadingCount + " 個多音字。");
            txtMsg.AppendText("\r\n  時間： " + (endTime - startTime).ToString());
        }
    }
}
