using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Huanlin.TextServices;

namespace Huanlin.TextServices
{
    public class ChinesePhoneticData
    {
        Dictionary<string, ChineseCharPhoneticInfo> m_MostFreqChars;   // 常用字
        Dictionary<string, ChineseCharPhoneticInfo> m_SecondFreqChars; // 次常用字
        Dictionary<string, ChineseCharPhoneticInfo> m_LeastFreqChars;  // 罕用子
        Dictionary<string, ChineseCharPhoneticInfo>[] m_CharListsByFreq;   // 指向各個字元串列的陣列.

        char[] sepChars = { ' ' };

        public ChinesePhoneticData()
        {
            m_MostFreqChars = new Dictionary<string, ChineseCharPhoneticInfo>();
            m_SecondFreqChars = new Dictionary<string, ChineseCharPhoneticInfo>();
            m_LeastFreqChars = new Dictionary<string, ChineseCharPhoneticInfo>();
            
            m_CharListsByFreq = new Dictionary<string,ChineseCharPhoneticInfo>[3];
            m_CharListsByFreq[0] = m_MostFreqChars;
            m_CharListsByFreq[1] = m_SecondFreqChars;
            m_CharListsByFreq[2] = m_LeastFreqChars;
        }

        private void Parse(string line)
        {
            string[] fields = line.Split(sepChars);
            if (fields.Length < 3)
                return;
            ChineseCharPhoneticInfo charInfo = new ChineseCharPhoneticInfo();
            charInfo.Character = fields[0]; // 中文字元
            charInfo.FreqCode = Convert.ToInt32(fields[1]); // 使用頻率 (0/1/2)
            for (int i = 2; i < fields.Length; i++) // 注音字根串列
            {
                charInfo.Phonetics.Add(fields[i]);
            }

            if (charInfo.FreqCode >= m_CharListsByFreq.Length)  // 防錯：修正使用頻率.
            {
                charInfo.FreqCode = m_CharListsByFreq.Length - 1;
            }

            // 根據使用頻率加入對應的串列.
            m_CharListsByFreq[charInfo.FreqCode].Add(charInfo.Character, charInfo);
        }

        public void Clear()
        {
            foreach (Dictionary<string, ChineseCharPhoneticInfo> charList in m_CharListsByFreq)
            {
                charList.Clear();
            }
        }

        /// <summary>
        /// 從檔案載入注音資料。注意此檔案必須是 UTF-8 編碼。
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadFromFile(string fileName)
        {
            using (StreamReader sr = new StreamReader(fileName, Encoding.UTF8))
            {                
                string line = sr.ReadLine();
                while (line != null)
                {
                    line = line.Trim();
                    if (line.Length > 0 && line[0] != ';')
                    {
                        Parse(line);
                    }
                    line = sr.ReadLine();
                }
            }
        }

        public void LoadFromStream(Stream inStream)
        {
            throw new Exception("Not implemented!");
        }

        /// <summary>
        /// 判斷傳入的中文字是否為多音字。
        /// </summary>
        /// <param name="aChar"></param>
        /// <returns></returns>
        public bool IsMultiReading(string aChar)
        {
            if (!UnicodeHelper.IsCJK(aChar))
                return false;

            ChineseCharPhoneticInfo charInfo = null;
            for (int i = 0; i < m_CharListsByFreq.Length-1; i++)
            {
                Dictionary<string, ChineseCharPhoneticInfo> charList = m_CharListsByFreq[i];                
                if (charList.TryGetValue(aChar, out charInfo))
                {
                    if (charInfo.IsMultiReading)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 取得指定中文字的注音字根鍵盤碼（標準注音鍵盤）。
        /// </summary>
        /// <param name="aChar"></param>
        /// <returns></returns>
        public string[] GetPhoneticKeys(string aChar)
        {
            string[] result = new string[0];    // Empty string array.

            if (!UnicodeHelper.IsCJK(aChar) && !UnicodeHelper.IsBopomofo(aChar))
                return result;
            
            ChineseCharPhoneticInfo charInfo = null;
            foreach (Dictionary<string, ChineseCharPhoneticInfo> charList in m_CharListsByFreq)
            {
                if (charList.TryGetValue(aChar, out charInfo))
                {
                    result = charInfo.Phonetics.ToArray();
                    break;
                }
            }
            return result;
        }
    }
}
