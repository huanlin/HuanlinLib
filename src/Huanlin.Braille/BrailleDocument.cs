﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Huanlin.Helpers;
using NChinese.Phonetic;

namespace Huanlin.Braille
{
    /// <summary>
    /// 代表一份點字文件。
    /// 注意：編寫明眼字文件時，不要自行斷行，就按照應有的段落內容輸入；斷行的部分由程式來處理。
    /// 因為程式是以一行為轉換的基本單位，句子裡面的引號必須成對，若自行斷行，
    /// 可能會造成程式無法在一行裡面找對應的右引號。
    /// </summary>
    [Serializable]
    [DataContract]
    public class BrailleDocument
    {
        [DataMember(Name="Lines")]
        private List<BrailleLine> m_Lines;

        [DataMember(Name="CellsPerLine")]
        private int m_CellsPerLine = BrailleConst.DefaultCellsPerLine;
        //private BrailleLine m_Title; // 文件標題

        [OptionalField]
        [DataMember(Name="PageTitles")]
        private List<BraillePageTitle> m_PageTitles;    // 所有的頁標題。

        [NonSerialized]
        private string m_FileName;

        [NonSerialized]
        private BrailleProcessor m_Processor;	// 點字轉換器。

        #region 建構函式

        public BrailleDocument()
        {
            m_Lines = new List<BrailleLine>();
            m_PageTitles = new List<BraillePageTitle>();
        }

        public BrailleDocument(string filename) 
            : this()
        {
            m_FileName = filename;
        }

        public BrailleDocument(BrailleProcessor processor)
            : this()
        {
            m_Processor = processor;
        }


        public BrailleDocument(string filename, BrailleProcessor processor)
            : this(filename)
        {
            m_Processor = processor;
        }

        public BrailleDocument(BrailleProcessor processer, int cellsPerLine)
            : this(processer)
        {
            m_CellsPerLine = cellsPerLine;
        }

        public BrailleDocument(string filename, BrailleProcessor processor, int cellsPerLine)
            : this(filename, processor)
        {
            m_CellsPerLine = cellsPerLine;
        }

        #endregion

        /// <summary>
        /// 載入明眼字文件並轉換成點字。
        /// </summary>
        public void LoadAndConvert()
        {
            if (StrHelper.IsEmpty(m_FileName))
                throw new Exception("檔案名稱尚未指定!");
            if (m_Processor == null)
                throw new Exception("在呼叫 BrailleDocument.Load 之前，請先指定 BrailleProcessor。");

            Encoding enc = Encoding.Default;
            if (FileHelper.IsUTF8Encoded(m_FileName))
            {
                enc = Encoding.UTF8;
            }
            using (StreamReader sr = new StreamReader(m_FileName, enc, true))
            {
                try
                {
                    LoadAndConvert(sr);
                }
                finally
                {
                    sr.Close();
                }
            }
        }

        public void Convert(string text)
        {
            using (StringReader sr = new StringReader(text))
            {
                try
                {
                    LoadAndConvert(sr);
                }
                finally
                {
                    sr.Close();
                }
            }
        }

        /// <summary>
        /// 載入明眼字文件並轉換成點字。
        /// </summary>
        /// <param name="filename">明眼字檔名。</param>
        public void LoadAndConvert(string filename)
        {
            FileName = filename;
            LoadAndConvert();
        }

        public void LoadAndConvert(TextReader reader)
        {
            int lineNumber = 0;

            string line;

            this.Clear();

            m_Processor.InitializeForConvertion();

            while (true)
            {
                line = reader.ReadLine();
                if (line == null)
                    break;
                lineNumber++;
                ProcessLine(lineNumber, line);
            }

            m_Processor.FormatDocument(this);   // 斷行

            FetchPageTitles();      // 提取標題列
        }

        /// <summary>
        /// 從現存的雙視點字檔案載入（反序列化）成新的 BailleDocument 物件。
        /// </summary>
        /// <param name="filename">檔名</param>
        /// <returns>新的 BailleDocument 物件</returns>
        public static BrailleDocument LoadBrailleFile(string filename)
        {
            if (!File.Exists(filename))
            {
                throw new FileNotFoundException("檔案不存在: " + filename);
            }

            BrailleDocument brDoc = null;

            // 舊版的 .btx 反序列化.
            if (Path.GetExtension(filename).Equals(".btx", StringComparison.CurrentCultureIgnoreCase))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open))
                {
                    BinaryReader br = new BinaryReader(fs);

                    int fileVersion = br.ReadInt32();
                    int lineCount = br.ReadInt32();
                    int reserved1 = br.ReadInt32();
                    int reserved2 = br.ReadInt32();

                    IFormatter fmter = new BinaryFormatter();
                    brDoc = (BrailleDocument)fmter.Deserialize(fs);
                    br.Close();

                    // 設定新增的屬性：BrailleWord.IsPolyphnic。等到 .btx 正式除役，這裡就不需要了。
                    foreach (BrailleLine brLine in brDoc.Lines)
                    {
                        foreach (BrailleWord brWord in brLine.Words)
                        {
                            if (!String.IsNullOrEmpty(brWord.PhoneticCode))
                            {
                                brWord.IsPolyphonic = ZhuyinQueryHelper.IsPolyphonic(brWord.Text);
                            }
                        }
                    }

                    return brDoc;
                }
            }

            string jsonStr = File.ReadAllText(filename);
            brDoc = Huanlin.Helpers.JsonHelper.Deserialize<BrailleDocument>(jsonStr);
            return brDoc;
        }

        /// <summary>
        /// 儲存到檔案（序列化）。
        /// </summary>
        /// <param name="filename">檔名</param>
        public void SaveBrailleFile(string filename)
        {
            this.UpdateTitlesLineIndex();

            // 舊版的 .btx 序列化.
            if (Path.GetExtension(filename).Equals(".btx", StringComparison.CurrentCultureIgnoreCase))
            {
                // 版本相容處理
                foreach (BrailleLine brLine in this.Lines)
                {
                    foreach (BrailleWord brWord in brLine.Words)
                    {
                        if (!String.IsNullOrEmpty(brWord.PhoneticCode))
                        {
                            brWord.PhoneticCodes.Clear();
                            brWord.PhoneticCodes.Add(brWord.PhoneticCode);
                        }
                    }
                }

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    BinaryWriter bw = new BinaryWriter(fs);

                    bw.Write(1000);                 // 文件版本: 1.0.0.0
                    bw.Write(this.LineCount);		// 寫入列數
                    bw.Write(0);                    // 保留未用
                    bw.Write(0);                    // 保留未用

                    IFormatter fmter = new BinaryFormatter();
                    fmter.Serialize(fs, this);

                    bw.Flush();
                    bw.Close();
                }
                return;
            }

            string jsonStr = Huanlin.Helpers.JsonHelper.Serialize<BrailleDocument>(this);
            File.WriteAllText(filename, jsonStr);
        }

        private void ProcessLine(int lineNumber, string line)
        {
            BrailleLine brLine = m_Processor.ConvertLine(lineNumber, line);
            if (brLine != null) 
            {
                AddLine(brLine);
            }
        }

        /// <summary>
        /// 加入一列。
        /// </summary>
        /// <param name="brLine"></param>
        public void AddLine(BrailleLine brLine)
        {
            m_Lines.Add(brLine);
        }

        public void InsertLines(int index, IEnumerable<BrailleLine> lines)
        {
            m_Lines.InsertRange(index, lines);
        }

        /// <summary>
        /// 移除指定的列。注意：只做移除列的動作，不可清除列的內容物件!!
        /// </summary>
        /// <param name="index"></param>
        public void RemoveLine(int index)
        {
            BrailleLine brLine = m_Lines[index];
            m_Lines.RemoveAt(index);
        }

        public void Clear()
        {
            m_Lines.Clear();

            if (m_PageTitles != null)
            {
                m_PageTitles.Clear();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (BrailleLine brLine in m_Lines)
            {
                sb.Append(brLine.ToString());
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 從 Lines 集合中取出頁標題，並將標題列自文件中移除。
        /// </summary>
        public void FetchPageTitles()
        {
            m_PageTitles.Clear();

            BrailleLine brLine;
            int idx = 0;
            while (idx < m_Lines.Count)
            {
                brLine = m_Lines[idx];
                if (brLine.ContainsTitleTag())
                {
                    BraillePageTitle title = new BraillePageTitle(this, idx);
                    m_PageTitles.Add(title);
                    m_Lines.RemoveAt(idx);
                }
                else 
                {
                    idx++;
                }
            }
        }

        /// <summary>
        /// 更新所有頁標題的起始列索引。
        /// 使用時機：BrailleDocument 存檔前、列印前。
        /// </summary>
        public void UpdateTitlesLineIndex()
        {
            if (m_PageTitles == null)
            {
                return;
            }

            BraillePageTitle title;
            int idx = 0;
            while (idx < m_PageTitles.Count)
            {
                title = m_PageTitles[idx];
                if (!title.UpdateLineIndex(this))
                {
                    m_PageTitles.RemoveAt(idx);
                }
                else
                {
                    idx++;
                }
            }
        }

        /// <summary>
        /// 根據每個頁標題得起始列索引更新其對應的 BrailleLine 物件。
        /// 使用時機：BrailleDocument 從檔案載入完畢時。
        /// </summary>
        public void UpdateTitlesLineObject()
        {
            if (m_PageTitles == null) 
            {
                return;
            }

            BraillePageTitle title;
            int idx = 0;
            while (idx < m_PageTitles.Count)
            {
                title = m_PageTitles[idx];
                if (!title.UpdateLineObject(this))
                {
                    m_PageTitles.RemoveAt(idx);
                }
                else
                {
                    idx++;
                }
            }
        }

        /// <summary>
        /// 根據指定的列索引判斷該列所在位置的頁標題為何，並傳回頁標題的 BrailleLine 物件。
        /// </summary>
        /// <param name="lineIdx"></param>
        /// <returns></returns>
        public BrailleLine GetPageTitle(int lineIdx)
        {
            if (m_PageTitles == null)
            {
                return null;
            }

            if (lineIdx < 0)        // 注意：不用比對上限!! 因為在列印時，傳入的 lineIdx 有可能大於等於總列數。
                return null;

            BraillePageTitle title;

            int i = m_PageTitles.Count - 1; // 必須從底下往上比較
            while (i >= 0)
            {
                title = m_PageTitles[i];
                if (lineIdx >= title.BeginLineIndex)
                {
                    return title.TitleLine;
                }
                i--;
            }
            return null;
        }

        /// <summary>
        /// 傳回所有頁標題的 BrailleLine 物件串列。
        /// </summary>
        /// <returns></returns>
        public List<BrailleLine> GetPageTitleLines()
        {
            List<BrailleLine> lines = new List<BrailleLine>();
            foreach (BraillePageTitle t in m_PageTitles)
            {
                lines.Add(t.TitleLine);
            }
            return lines;
        }

        #region 序列化事件

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (m_PageTitles == null)
            {
                m_PageTitles = new List<BraillePageTitle>();
            }
            UpdateTitlesLineObject();
        }

        #endregion

        // TODO: Dispose pattern。確保所有不用的記憶體都獲得釋放。

        #region 屬性

        /// <summary>
        /// 取得或設定 BrailleProcessor 物件參考。
        /// </summary>
        public BrailleProcessor Processor
        {
            get { return m_Processor; }
            set { m_Processor = value; }
        }

        /// <summary>
        /// 取得或設定每列最大方數。
        /// </summary>
        public int CellsPerLine
        {
            get { return m_CellsPerLine; }
            set { m_CellsPerLine = value; }
        }

        /// <summary>
        /// 取得或設定檔名。
        /// </summary>
        public string FileName
        {
            get { return m_FileName; }
            set { m_FileName = value; }
        }

        public List<BrailleLine> Lines
        {
            get { return m_Lines; }
        }

        public BrailleLine this[int index]
        {
            get { return m_Lines[index]; }
        }

        /// <summary>
        /// 取得總列數。
        /// </summary>
        public int LineCount
        {
            get { return m_Lines.Count; }
        }

        /// <summary>
        /// 取得字數最長的 line。
        /// </summary>
        public BrailleLine LongestLine
        {
            get
            {
                BrailleLine longestLine = null;
                int maxCount = -1;
                int curCount;
                foreach (BrailleLine brLine in m_Lines)
                {
                    curCount = brLine.WordCount;
                    if (curCount > maxCount)
                    {
                        longestLine = brLine;
                        maxCount = brLine.WordCount;
                    }
                }
                return longestLine;
            }
        }

        /// <summary>
        /// 取得或設定頁標題串列。
        /// </summary>
        public List<BraillePageTitle> PageTitles
        {
            get { return m_PageTitles; }
            set { m_PageTitles = value; }
        }

        #endregion

    }
}
