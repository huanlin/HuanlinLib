﻿using System;
using System.Collections.Generic;
using System.Text;
using Huanlin.Braille.Data;

namespace Huanlin.Braille.Converters
{
    public sealed class MathConverter : WordConverter
    {
        private MathBrailleTable m_Table;

        public MathConverter()
            : base()
        {
            m_Table = MathBrailleTable.GetInstance();
        }

        /// <summary>
        /// 從堆疊中讀取字元，並轉成點字。
        /// </summary>
        /// <param name="charStack">字元堆疊。</param>
        /// <returns>傳回轉換後的點字物件串列，若串列為空串列，表示沒有成功轉換的字元。</returns>
        public override List<BrailleWord> Convert(Stack<char> charStack, ContextTagManager context)
        {
            if (charStack.Count < 1)
                throw new ArgumentException("傳入空的字元堆疊!");

            // 注意: 此函式只能一次處理一個字元，不可取多個字元!
            //       因為可能會碰到 "</數學>" 標籤，這些必須在 BrailleProcessor 中事先處理掉。

            char ch;
            string text;
            bool isExtracted;	// 目前處理的字元是否已從堆疊中移出。
            BrailleWord brWord;
            List<BrailleWord> brWordList = null;

            ch = charStack.Peek();   // 只讀取但不從堆疊移走。
            isExtracted = false;

            // 小於跟大於符號交由 ContextTagConverter 或 EnglishWordConverter 處理）。
            if (ch == '<' || ch == '>') 
                return null;

            text = ch.ToString();

            brWord = base.ConvertToBrailleWord(text);

            if (brWord != null)
            {
                if (brWordList == null)
                {
                    brWordList = new List<BrailleWord>();
                }
                brWordList.Add(brWord);

                if (!isExtracted)
                {
                    charStack.Pop();
                }
            }
            return brWordList;
        }

        internal override BrailleTableBase BrailleTable
        {
            get { return m_Table; }
        }

    }
}
