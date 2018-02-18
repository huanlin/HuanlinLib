using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenPhoneticData
{
    /// <summary>
    /// 字元與其所屬的注音串列。
    /// </summary>
    public class CharPhon
    {
        private string theChar;     // 記住：Unicode 字元必須以 string 儲存，不可用 char !! (搜尋: surrogate pairs)
        private List<string> phonetics; // 注音字根串列
        private int freqCode;   // 使用頻率代碼：0/1/2 = 常用/次常用/罕用。

        public CharPhon() 
        {
            theChar = "";
            phonetics = new List<string>();
            int freqCode = 0;
        }

        public string Character 
        { 
            get { return theChar; }
            set { theChar = value; }
        }

        public List<string> Phonetics
        {
            get { return phonetics; }
        }

        /// <summary>
        /// 是否為多音字.
        /// </summary>
        public bool IsMultiReading
        {
            get
            {
                return phonetics.Count > 1;
            }
        }

        public int FreqCode
        {
            get
            {
                return freqCode;
            }

            set
            {
                freqCode = value;
            }
        }
    }
}
