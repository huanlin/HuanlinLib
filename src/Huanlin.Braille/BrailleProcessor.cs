﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Huanlin.Braille.Converters;
using Huanlin.Braille.Data;
using Huanlin.Helpers;
using NChinese.Phonetic;

namespace Huanlin.Braille
{
    public struct CharPosition
	{
		public char CharValue;		// 字元
		public int LineNumber;		// 第幾列
		public int CharIndex;		// 第幾個字元
	}

	public class ConvertionFailedEventArgs : EventArgs
	{
		private CharPosition m_InvalidChar;
        private string m_OriginalText;
        private bool m_Stop;    // 中止轉換。

        public string OriginalText
        {
            get { return m_OriginalText; }
        }

        public CharPosition InvalidChar
        {
            get { return m_InvalidChar; }
        }

        public bool Stop
        {
            get { return m_Stop; }
            set { m_Stop = value; }
        }

		internal void SetArgs(int lineNumber, int charIndex, string line, char ch)
		{
			m_InvalidChar.LineNumber = lineNumber;
			m_InvalidChar.CharIndex = charIndex;
			m_InvalidChar.CharValue = ch;
			m_OriginalText = line;
		}
	}

	public class TextConvertedEventArgs : EventArgs 
	{
		private string m_Text;
		private int m_LineNumber;

		internal void SetArgValues(int lineNum, string text)
		{
			m_Text = text;
			m_LineNumber = lineNum;
		}	

		public string Text
		{
			get { return m_Text; }
		}
		public int LineNumber
		{
			get { return m_LineNumber; }
		}
	}

	public delegate void ConvertionFailedEventHandler(object sender, ConvertionFailedEventArgs e);

	public delegate void TextConvertedEventHandler(object sender, TextConvertedEventArgs e);

    internal class TextTag
    {
        public const string Name = "<私名號>";
        public const string BookName = "<書名號>";
        public const string NumericItem = "<編號>";
        public const string OrgPageNumber = "<P>";  // 原書頁碼
        public const string DocTitle = "<標題>";    // 文件標題
        public const string Unit1End = "<大單元結束>";
        public const string Unit2End = "<小單元結束>";
        public const string Unit3End = "<小題結束>";
        public const string BrailleComment = "<點譯者註>";
		public const string DeleteBegin = "<刪>";
		public const string DeleteEnd = "</刪>";
    }

	/// <summary>
	/// 此類別可用來將明眼字轉換成點字。可處理一個字、一行、或者多行。
	/// 錯誤處理機制：
	/// 1. 所有 exception 訊息會存入 ErrorMessage 屬性。
	/// 2. 所有無法轉換的字元會丟到 InvalidChars 屬性。
	/// </summary>
	public class BrailleProcessor
	{
		private static BrailleProcessor s_Processor = null;
        private static string s_DashesForOrgPageNumber;

        // Predefined converters       
        private ContextTagConverter m_ContextTagConverter;
        private MathConverter m_MathConverter;
        private ChineseWordConverter m_ChineseConverter;
        private EnglishWordConverter m_EnglishConverter;
        private CoordinateConverter m_CoordConverter;
		private TableConverter m_TableConverter;
		private PhoneticConverter m_PhoneticConverter;

        // Extended converters
        private List<WordConverter> m_Converters;

		private Hashtable m_Tags;
        private ContextTagManager m_ContextTagManager;

		private List<CharPosition> m_InvalidChars;	// 轉換過程中所有無法轉換的字元。
		private StringBuilder m_ErrorMsg;			// 轉換過程中發生的錯誤訊息。

        private bool m_SuppressEvents;  // 是否抑制轉換的事件觸發。

		private event ConvertionFailedEventHandler m_ConvertionFailedEvent;
		private event TextConvertedEventHandler m_TextConvertedEvent;
       

        #region 建構函式

        static BrailleProcessor()
        {
            s_DashesForOrgPageNumber = GetDashForOrgPageNumber();
        }

        private BrailleProcessor(ZhuyinReverseConverter zhuyinConverter)
		{
            m_Converters = new List<WordConverter>();

            m_ContextTagConverter = new ContextTagConverter();
            m_ChineseConverter = new ChineseWordConverter(zhuyinConverter);
            m_EnglishConverter = new EnglishWordConverter();
            m_MathConverter = new MathConverter();
            m_CoordConverter = new CoordinateConverter();
			m_TableConverter = new TableConverter();
			m_PhoneticConverter = new PhoneticConverter();

			m_Tags = new Hashtable();
			m_Tags.Add(TextTag.Name, "╴╴"); // key/value = 標籤/替換字元
			m_Tags.Add(TextTag.BookName, "﹏﹏");
			m_Tags.Add(TextTag.NumericItem, "#");
            m_Tags.Add(TextTag.OrgPageNumber, s_DashesForOrgPageNumber);   // 原書頁碼
            m_Tags.Add(TextTag.Unit1End, new string('ˍ', 20)); // 大單元結束
            m_Tags.Add(TextTag.Unit2End, new string('﹍', 20)); // 小單元結束
            m_Tags.Add(TextTag.Unit3End, new string('﹋', 20)); // 小題結束
            m_Tags.Add(TextTag.BrailleComment, "★");   // 點譯者註

            m_ContextTagManager = new ContextTagManager();

            m_InvalidChars = new List<CharPosition>();
			m_ErrorMsg = new StringBuilder();
            m_SuppressEvents = false;
		}

		/// <summary>
		/// Get singleton instance.
		/// </summary>
		/// <returns></returns>
		public static BrailleProcessor GetInstance(ZhuyinReverseConverter zhuyinConverter = null)
		{
			if (s_Processor != null)
			{
				return s_Processor;
			}

			s_Processor = new BrailleProcessor(zhuyinConverter);
			return s_Processor;
		}

        #endregion

        #region 屬性

        /// <summary>
        /// 取得或設定中文點字轉換器。
        /// </summary>
        public ChineseWordConverter ChineseConverter
        {
            get { return m_ChineseConverter; }
            set { m_ChineseConverter = value; }
        }

        /// <summary>
        /// 取得或設定英文點字轉換器。
        /// </summary>
        public EnglishWordConverter EnglishConverter
        {
            get { return m_EnglishConverter; }
            set { m_EnglishConverter = value; }
        }

        public ContextTagConverter ControlTagConverter
        {
            get { return m_ContextTagConverter; }
            set { m_ContextTagConverter = value; }
        }

        public MathConverter MathConverter
        {
            get { return m_MathConverter; }
            set { m_MathConverter = value; }
        }

        /// <summary>
        /// 是否抑制點字轉換的回饋事件。
        /// 範例：
        ///     brProcessor.SuppressEvents = true;  // 關閉點字處理器事件
        ///     BrailleLine brLine = brProcessor.ConvertLine("測試");
        ///     brProcessor.SuppressEvents = false; // 恢復點字處理器事件
        /// </summary>
        public bool SuppressEvents
        {
            get { return m_SuppressEvents; }
            set { m_SuppressEvents = value; }
        }

        public List<CharPosition> InvalidChars
        {
            get { return m_InvalidChars; }
        }

		public bool HasError
		{
			get
			{
				if (m_ErrorMsg.Length > 0 || m_InvalidChars.Count > 0)
				{
					return true;
				}
				return false;
			}
		}

		public string ErrorMessage
		{
			get { return m_ErrorMsg.ToString(); }
		}

        #endregion

		#region 事件

		public event ConvertionFailedEventHandler ConvertionFailed
		{
			add
			{
				m_ConvertionFailedEvent += value;
			}
			remove
			{
				m_ConvertionFailedEvent -= value;
			}
		}

		public event TextConvertedEventHandler TextConverted
		{
			add
			{
				m_TextConvertedEvent += value;
			}
			remove
			{
				m_TextConvertedEvent -= value;
			}
		}

		#endregion

		#region 事件方法

		protected virtual void OnConvertionFailed(ConvertionFailedEventArgs args)
		{
            // 將無效字元記錄於內部變數。

			m_InvalidChars.Add(args.InvalidChar);

            if (m_SuppressEvents)
                return;

			if (m_ConvertionFailedEvent != null)
			{
				m_ConvertionFailedEvent(this, args);
			}
		}

		protected virtual void OnTextConverted(TextConvertedEventArgs args)
		{
            if (m_SuppressEvents)
                return;

			if (m_TextConvertedEvent != null)
			{
				m_TextConvertedEvent(this, args);
			}
		}

		#endregion

		public void AddConverter(WordConverter cvt)
        {
            // 設定已知的轉換器。

            if (cvt is ContextTagConverter)
            {
                m_ContextTagConverter = (ContextTagConverter)cvt;
                return;
            }
            if (cvt is ChineseWordConverter)
            {
                m_ChineseConverter = (ChineseWordConverter) cvt;
                return;
            }
            if (cvt is EnglishWordConverter)
            {
                m_EnglishConverter = (EnglishWordConverter) cvt;
                return;
            }
            if (cvt is MathConverter)   // 數學符號轉換器.
            {
                m_MathConverter = (MathConverter)cvt;
                return;
            }
			if (cvt is PhoneticConverter)	// 音標轉換器.
			{
				m_PhoneticConverter = (PhoneticConverter)cvt;
				return;
			}

            // 加入其他未知的轉換器。
            if (m_Converters.IndexOf(cvt) < 0)
                m_Converters.Add(cvt);
        }

        public void RemoveConverter(WordConverter cvt)
        {
            // 移除已知的轉換器。

            if (cvt is ContextTagConverter)
            {
                m_ContextTagConverter = null;
                return;
            }
            if (cvt is ChineseWordConverter)
            {
                m_ChineseConverter = null;
                return;
            }
            if (cvt is EnglishWordConverter)
            {
                m_EnglishConverter = null;
                return;
            }
            if (cvt is MathConverter)
            {
                m_MathConverter = null;
                return;
            }
			if (cvt is PhoneticConverter)	// 音標轉換器.
			{
				m_PhoneticConverter = null;
				return;
			}
            
            m_Converters.Remove(cvt);
        }

        /// <summary>
        /// 根據指定的類別名稱傳回對應之 word converter 物件。
        /// 主要是用在需要單獨轉換一個中文字的時候。
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public WordConverter GetConverter(string className)
        {            
            foreach (WordConverter cvt in m_Converters)
            {
                if (cvt.GetType().Name.Equals(className, StringComparison.CurrentCultureIgnoreCase))
                    return cvt;
            }
            return null;
        }

        #region 轉換函式

        /// <summary>
        /// 在開始轉換文件之前，先呼叫此函式以進行初始化。
        /// </summary>
        public void InitializeForConvertion()
        {
			m_ErrorMsg.Length = 0;
            m_InvalidChars.Clear();
            m_ContextTagManager.Reset();
        }

        /// <summary>
        /// 把一行明眼字串轉換成點字串列。
        /// 此時不考慮一行幾方和斷行的問題，只進行單純的轉換。
        /// 斷行由其他函式負責處理，因為有些點字規則必須在斷行時才能處理。
        /// </summary>
        /// <param name="lineNumber">字串的行號。此參數只是用來當轉換失敗時，傳給轉換失敗事件處理常式的資訊。</param>
        /// <param name="line">輸入的明眼字串。</param>
        /// <param name="isTitle">輸出參數，是否為標題。</param>
        /// <returns>點字串列。若則傳回 null，表示該列不需要轉成點字。</returns>
        public BrailleLine ConvertLine(int lineNumber, string line)
        {
            if (line == null)
                return null;

            BrailleLine brLine = new BrailleLine();

			string orgLine = line;	// 保存原始的字串。

            // 把換行符號之後的字串去掉
            int i = line.IndexOfAny(new char[] { '\r', '\n' });
            if (i >= 0)
            {
                line = line.Substring(0, i);
            }

            // 若去掉換行字元之後變成空字串，則傳回只包含一個空方的列。
            if (String.IsNullOrEmpty(line))
            {
                brLine.Words.Add(BrailleWord.NewBlank());
                return brLine;
            }

			// 預先處理特殊標籤的字元替換。
			line = PreprocessTagsForLine(line);
            if (line == null)
                return null;

			// 如果是原書頁碼，先檢查格式是否正確。
			try
			{
				GetOrgPageNumber(line);
			}
			catch (Exception ex)
			{
				m_ErrorMsg.Append(String.Format("第 {0} 列 : ", lineNumber));
				m_ErrorMsg.Append(ex.Message);
				m_ErrorMsg.Append("\r\n");
				return null;
			}

			line = StrHelper.Reverse(line);
            Stack<char> charStack = new Stack<char>(line);

			char ch;
			List<BrailleWord> brWordList;
			StringBuilder text = new StringBuilder();

			ConvertionFailedEventArgs cvtFailedArgs = new ConvertionFailedEventArgs();
			TextConvertedEventArgs textCvtArgs = new TextConvertedEventArgs();

			while (charStack.Count > 0)
			{
				brWordList = ConvertWord(charStack);

				if (brWordList != null && brWordList.Count > 0)	
				{
					// 成功轉換成點字，有 n 個字元會從串流中取出
					brLine.Words.AddRange(brWordList);

					text.Length = 0;
					foreach (BrailleWord brWord in brWordList) 
					{
						text.Append(brWord.Text);
					}
					textCvtArgs.SetArgValues(lineNumber, text.ToString());
					OnTextConverted(textCvtArgs);
				}
				else
				{
					// 無法判斷和處理的字元應該會留存在串流中，將之取出。
                    ch = charStack.Pop();

					int charIndex = line.Length - charStack.Count;

					// 引發事件。
					cvtFailedArgs.SetArgs(lineNumber, charIndex, orgLine, ch);
					OnConvertionFailed(cvtFailedArgs);
					if (cvtFailedArgs.Stop)
                    {
                        break;
                    }
				}				

				// 如果進入分數情境，就把整個分數處理完。
				if (m_ContextTagManager.IsActive(ContextTagNames.Fraction))
				{
					try
					{
						brWordList = ConvertFraction(lineNumber, charStack);
						if (brWordList != null && brWordList.Count > 0)
						{
							// 成功轉換成點字，有 n 個字元會從串流中取出
							brLine.Words.AddRange(brWordList);
						}
					}
					catch (Exception ex)
					{
						m_ErrorMsg.Append(String.Format("第 {0} 列 : ", lineNumber));
						m_ErrorMsg.Append(ex.Message);
						m_ErrorMsg.Append("\r\n");
					}
				}
			}

            ChineseBrailleRule.ApplyPunctuationRules(brLine);	// 套用中文標點符號規則。

			// 不刪除多餘空白，因為原本輸入時可能就希望縮排。
            //ChineseBrailleRule.ShrinkSpaces(brLine);	// 把連續全形空白刪到只剩一個。

			// 將編號的數字修正成上位點。
            if (m_EnglishConverter != null)
            {
                EnglishBrailleRule.FixNumbers(brLine, m_EnglishConverter.BrailleTable as EnglishBrailleTable);
            }

            EnglishBrailleRule.ApplyCapitalRule(brLine);    // 套用大寫規則。
			EnglishBrailleRule.ApplyDigitRule(brLine);		// 套用數字規則。
            EnglishBrailleRule.AddSpaces(brLine);           // 補加必要的空白。

			ChineseBrailleRule.ApplyBracketRule(brLine);	// 套用括弧規則。

			// 不刪除多於空白，因為原本輸入時可能就希望縮排。
            //EnglishBrailleRule.ShrinkSpaces(brLine);        // 把連續空白刪到只剩一個。

            return brLine;
        }

        /// <summary>
        /// 把一行明眼字串轉換成點字串列。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        /// <see cref="BrailleProcessor.ConvertLine(int,string)"/>
        public BrailleLine ConvertLine(string line)
        {
            return this.ConvertLine(1, line);
        }

		/// <summary>
		/// 把一個明眼字元轉換成點字（BrailleWord）。
		/// 原則上，能夠在這裡處理掉點字特殊規則的，就盡量在這裡處理掉，
		/// 特別是不可斷行分開的點字，例如：一個中文字的所有點字碼、特殊單音字附加的「ㄦ」等等。
		/// </summary>
		/// <param name="reader">字串流。</param>
        /// <returns>若成功轉換成點字，則傳回已轉換的點字 BrailleWord 物件串列，否則傳回 null。</returns>
		/// <remarks>若轉換成功，則已轉換的字元會從串流中讀出，否則該字元仍會保留在串流中。</remarks>
		public List<BrailleWord> ConvertWord(Stack<char> chars)
		{
			List<BrailleWord> brWordList = null;

            // Two-pass 處理（因為有些點字必須再交給其它點字轉換器，故需兩次）。
            for (int pass = 0; pass < 2; pass++)    
            {
                if (chars.Count < 1)
                    break;

                // 1. 轉換情境標籤。NOTE: 情境標籤一定要先處理!
				if (chars.Count > 0 && m_ContextTagConverter != null)
                {
                    brWordList = m_ContextTagConverter.Convert(chars, m_ContextTagManager);
                    if (brWordList != null && brWordList.Count > 0)
                        return brWordList;
                }

                // 2. 轉換座標符號
				if (chars.Count > 0 && m_CoordConverter != null && m_ContextTagManager.IsActive(ContextTagNames.Coordinate))
                {
                    brWordList = m_CoordConverter.Convert(chars, m_ContextTagManager);
                    if (brWordList != null && brWordList.Count > 0)
                        return brWordList;
                }                

                // 3. 轉換數學符號。
                if (chars.Count > 0 && m_ContextTagManager.IsActive(ContextTagNames.Math) && m_MathConverter != null)
                {
                    brWordList = m_MathConverter.Convert(chars, m_ContextTagManager);
                    if (brWordList != null && brWordList.Count > 0)
                        return brWordList;
                }

				// 4. 轉換表格符號。
				if (chars.Count > 0 && m_ContextTagManager.IsActive(ContextTagNames.Table) && m_TableConverter != null)
				{
					brWordList = m_TableConverter.Convert(chars, m_ContextTagManager);
					if (brWordList != null && brWordList.Count > 0)
						return brWordList;
				}

				// 5. 轉換音標符號.
				if (chars.Count > 0 && m_ContextTagManager.IsActive(ContextTagNames.Phonetic) && m_PhoneticConverter != null)
				{
					brWordList = m_PhoneticConverter.Convert(chars, m_ContextTagManager);
					if (brWordList != null && brWordList.Count > 0)
						return brWordList;
				}				

                // 6. 轉換中文。
				if (chars.Count > 0 && m_ChineseConverter != null)
                {
                    // 若成功轉換成點字，就不再 pass 給其它轉換器。
                    brWordList = m_ChineseConverter.Convert(chars, m_ContextTagManager);
                    if (brWordList != null && brWordList.Count > 0)
                        return brWordList;
                }

                // 7. 轉換英文。
				if (chars.Count > 0 &&  m_EnglishConverter != null)
                {
                    // 若成功轉換成點字，就不再 pass 給其它轉換器。
                    brWordList = m_EnglishConverter.Convert(chars, m_ContextTagManager);
                    if (brWordList != null && brWordList.Count > 0)
                        return brWordList;
                }
            }

			if (chars.Count > 0)
			{
				// 其它註冊的轉換器。
				foreach (WordConverter cvt in m_Converters)
				{
					// 若其中一個轉換器成功轉換成點字，就不再 pass 給其它轉換器。
					brWordList = cvt.Convert(chars, m_ContextTagManager);
					if (brWordList != null && brWordList.Count > 0)
						return brWordList;
				}
			}

			// TODO: 碰到無法轉換成點字的情況時，觸發事件通知呼叫端處理，例如：顯示在在訊息視窗裡。

			return null;
		}

		/// <summary>
		/// 剖析分數格式的字串，分別取出整數部份、分子、以及分母。
		/// 分數格式範例： 三又五分之一的分數字串表示為： 3&1/5。
		/// </summary>
		/// <param name="s"></param>
		/// <param name="intPart"></param>
		/// <param name="numerator"></param>
		/// <param name="denumerator"></param>
		private void ParseFraction(string s, out string intPart, out string numerator, out string denumerator)
		{
			// 取出整數部份
			intPart = "";
			int idxAmpersand = s.IndexOf('&');
			if (idxAmpersand > 0)
			{
				intPart = s.Substring(0, idxAmpersand);
			}

			// 取出分子
			numerator = "";
			int idxBar = s.IndexOf("/");
			if (idxBar <= 0 || idxBar < idxAmpersand)
			{
				throw new Exception("分數的格式不正確: 沒有分數的分隔符號 ('/') 或者分隔符號的位置不對。");
			}
			numerator = s.Substring(idxAmpersand + 1, idxBar - idxAmpersand - 1);

			// 取出分母
			denumerator = s.Substring(idxBar + 1);

			if (String.IsNullOrEmpty(numerator) || String.IsNullOrEmpty(denumerator))
			{
				throw new Exception("分數的格式不正確!! 無法剖析分子或分母。");
			}
		}

		/// <summary>
		/// 轉換分數。
		/// </summary>
		/// <param name="lineNumber"></param>
		/// <param name="chars"></param>
		/// <returns></returns>
		private List<BrailleWord> ConvertFraction(int lineNumber, Stack<char> chars)
		{
			char[] charAry = chars.ToArray();
			string s = new string(charAry);
			int idxEof = s.IndexOf(ContextTag.GetEndTagName(ContextTagNames.Fraction));	// end of fraction
			if (idxEof < 0) 
			{
				throw new Exception("<分數> 標籤有起始但沒有結束標籤!");
			}

			s = s.Substring(0, idxEof);	// 從字串頭取到 </分數> 標籤之前。

			string intPart;
			string numerator;
			string denumerator;

			ParseFraction(s, out intPart, out numerator, out denumerator);

			// Note: 整數部份、分子、分母都有可能是英文字母（代數）。

			string temp;
			Stack<char> charStack;
			List<BrailleWord> brWordList = null;

			List<BrailleWord> brWordListIntPart = new List<BrailleWord>();

			if (!String.IsNullOrEmpty(intPart))
			{
				// 將整數部份轉換成點字串列。
				temp = StrHelper.Reverse(intPart);
				charStack = new Stack<char>(temp);

				while (charStack.Count > 0)
				{
					brWordList = ConvertWord(charStack);
					if (brWordList == null)
					{
						throw new Exception("無法轉換分數的整數部份!");
					}
					else
					{
						brWordListIntPart.AddRange(brWordList);
					}
				}
			}

			// 將分子部份轉換成點字串列。
			temp = StrHelper.Reverse(numerator + "/");
			charStack = new Stack<char>(temp);
			List<BrailleWord> brWordListNumerator = new List<BrailleWord>();
			while (charStack.Count > 0)
			{
				brWordList = ConvertWord(charStack);
				if (brWordList == null)
				{
					throw new Exception("無法轉換分數的分子部份!");
				}
				else
				{
					brWordListNumerator.AddRange(brWordList);
				}
			}
			// 分子的數字不要加數符
			foreach (BrailleWord brWord in brWordListNumerator)
			{
				brWord.NoDigitCell = true;
			}
			// 補上分子的點字符號
			brWordListNumerator[0].Cells.Insert(0, BrailleCell.GetInstance(new int[] {1, 4, 5, 6}));
			if (brWordListIntPart.Count > 0) 
			{
				brWordListNumerator[0].Cells.Insert(0, BrailleCell.GetInstance(new int[] {4, 5, 6}));
			}

			// 將分母部份轉換成點字串列。
			temp = StrHelper.Reverse(denumerator);
			charStack = new Stack<char>(temp);
			List<BrailleWord> brWordListDenumerator = new List<BrailleWord>();
			while (charStack.Count > 0)
			{
				brWordList = ConvertWord(charStack);
				if (brWordList == null)
				{
					throw new Exception("無法轉換分數的分母部份!");
				}
				else
				{
					brWordListDenumerator.AddRange(brWordList);
				}
			}
			// 分母的數字不要加數符
			foreach (BrailleWord brWord in brWordListDenumerator)
			{
				brWord.NoDigitCell = true;
			}

			// 補上分母後面的點字符號
			BrailleWord lastBrWord = brWordListDenumerator[brWordListDenumerator.Count-1];
			if (brWordListIntPart.Count > 0) 
			{
				lastBrWord.Cells.Add(BrailleCell.GetInstance(new int[] {4, 5, 6}));
			}
			lastBrWord.Cells.Add(BrailleCell.GetInstance(new int[] {3, 4, 5, 6}));
		
			// 結合整數部份、分子、分母至同一個串列。
			List<BrailleWord> brWordListFraction = new List<BrailleWord>();
			brWordListFraction.AddRange(brWordListIntPart);
			brWordListFraction.AddRange(brWordListNumerator);
			brWordListFraction.AddRange(brWordListDenumerator);

			// 完成! 從傳入的字元堆疊中取出已經處理的字元。
			while (idxEof > 0)
			{
				chars.Pop();
				idxEof--;
			}

			return brWordListFraction;
		}

		/// <summary>
		/// 在進行轉換之前預先處理一列中的所有特殊標籤，將這些標籤替換成特定字元。
		/// </summary>
		/// <param name="line"></param>
		/// <returns>傳回置換過的字串。若傳回 null，表示這行不要轉換成點字。</returns>
		public string PreprocessTagsForLine(string line)
		{
            // 處理標題標籤 (這是舊的程式碼，保留供參考，若未來有需要類似的前置標籤處理，可依樣畫葫蘆)
            //isTitle = false;
            //string tagName = TextTag.DocTitle;
            //int startIdx = line.IndexOf(tagName);
            //int endIdx = line.IndexOf(tagName.Insert(1, "/"));
            //if (startIdx >= 0 && endIdx >= 0 && startIdx < endIdx)
            //{
            //    startIdx = startIdx + tagName.Length;
            //    string title = line.Substring(startIdx, endIdx - startIdx);
            //    isTitle = true; // 傳入的列是標題文字。
            //    return title;
            //}

			string result = Regex.Replace(line, RegExpPatterns.Tags, new MatchEvaluator(this.MatchedTagFound));
			return result;
		}

		/// <summary>
		/// 每當有找到匹配字串時觸發此事件。
		/// </summary>
		/// <param name="token"></param>
		/// <returns>傳回用來把這次找到的 token 置換掉的字串。</returns>
		private string MatchedTagFound(Match token)
		{
			if (m_Tags.ContainsKey(token.Value)) 
			{
				return m_Tags[token.Value].ToString();
			}

			// 結束標籤
			if (token.Value.StartsWith("</")) 
			{
                string key = token.Value.Remove(1, 1);  // 把 '/' 字元去掉，即得到起始標籤名稱。
                if (m_Tags.ContainsKey(key))
                {
                    return " ";
                }
			}            

			return token.Value;
		}

        #endregion

        #region 格式化、編排、與修正函式

		/// <summary>
		/// 編排點字文件。
		/// </summary>
		public void FormatDocument(BrailleDocument doc)
		{
            ContextTagManager context = new ContextTagManager();

    		int index = 0;
			while (index < doc.Lines.Count)
			{
                ProcessIndentTags(doc, index, context);
                index += FormatLine(doc, index, context);
			}
		}

        /// <summary>
        /// 處理縮排情境標籤：碰到縮排標籤時，將縮排次數更新至 ContextTagManager 物件，並移除此縮排標籤。
        /// 
        /// NOTE: 縮排標籤必須位於列首，一列可有多個連續縮排標籤，例如：＜縮排＞＜縮排＞。
        /// </summary>
        /// <param name="brDoc"></param>
        /// <param name="lineIndex"></param>
        /// <param name="context">情境物件。</param>
        /// <returns></returns>
        public void ProcessIndentTags(BrailleDocument brDoc, int lineIndex, ContextTagManager context)
        {
            BrailleLine brLine = brDoc.Lines[lineIndex];
            int wordIdx = 0;
            ContextTag ctag;

            while (brLine.WordCount > 0) 
            {
                ctag = context.Parse(brLine[0].Text, ContextTagNames.Indent);
                if (ctag != null)
                {
                    brLine.RemoveAt(wordIdx);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 編排指定的列。此函式會將指定的列斷行。
        /// </summary>
        /// <param name="brDoc">點字文件。</param>
        /// <param name="lineIndex">欲重新編排的列索引。</param>
        /// <returns>傳回編排後的列數。</returns>
        public int FormatLine(BrailleDocument brDoc, int lineIndex, ContextTagManager context)
        {
            BrailleLine brLine = brDoc.Lines[lineIndex];

            RemoveContextTagsButTitle(brLine);   // 清除情境標籤，除了標題標籤。

            if (brLine.WordCount == 0) 
            {
                brDoc.RemoveLine(lineIndex);
                return 0;
            }

            List<BrailleLine> newLines;
            newLines = BreakLine(brLine, brDoc.CellsPerLine, context);

            if (newLines == null)   // 沒有斷行？
            {
                return 1;
            }

            // 移除原始的 line
            brLine.Clear();
            brDoc.RemoveLine(lineIndex);

            // 加入斷行後的 lines
            brDoc.Lines.InsertRange(lineIndex, newLines);

            return newLines.Count;
        }

        /// <summary>
        /// 移除所有情境標籤，除了標題標籤。
        /// </summary>
        public void RemoveContextTagsButTitle(BrailleLine brLine)
        {
            BrailleWord brWord;

            for (int i = brLine.WordCount - 1; i >= 0; i--)
            {
                brWord = brLine.Words[i];
                if (brWord.IsContextTag && !ContextTag.IsTitleTag(brWord.Text))
                {
                    brLine.Words.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 將一行點字串列斷成多行。
        /// </summary>
        /// <param name="brLine">來源點字串列。</param>
        /// <param name="cellsPerLine">每行最大方數。</param>
        /// <param name="context">情境物件。</param>
        /// <returns>斷行之後的多行串列。若為 null 表示無需斷行（指定的點字串列未超過每行最大方數）。</returns>
        public List<BrailleLine> BreakLine(BrailleLine brLine, int cellsPerLine, ContextTagManager context)
		{
            if (context != null && context.IndentCount > 0) // 若目前位於縮排區塊中
            {
                // 每列最大方數要扣掉縮排數量，並於事後補縮排的空方。
                // NOTE: 必須在斷行之後才補縮排的空方!
                cellsPerLine -= context.IndentCount;    
            }

            // 若指定的點字串列未超過每行最大方數，則無須斷行，傳回 null。
            if (brLine.CellCount <= cellsPerLine)
            {
                // 補縮排的空方。
                if (context != null && context.IndentCount > 0) // 若目前位於縮排區塊中
                {
                    this.Indent(brLine, context.IndentCount);
                }
                return null;
            }

			List<BrailleLine> lines = new List<BrailleLine>();
			BrailleLine newLine = null;
			int wordIndex = 0;
			int breakIndex = 0;
			bool needHyphen = false;
            bool isBroken = false;      // 是否已經斷行了？
            int indents = 0;    // 第一次斷行時，不會有系統自斷加上的縮排，因此初始為 0。
            int maxCells = cellsPerLine;;

            // 計算折行之後的縮排格數。
            indents = BrailleProcessor.CalcNewLineIndents(brLine);            

            while (wordIndex < brLine.WordCount)
			{
				breakIndex = BrailleProcessor.CalcBreakPoint(brLine, maxCells, out needHyphen);

				newLine = brLine.Copy(wordIndex, breakIndex);	// 複製到新行。
				if (needHyphen)	// 是否要附加連字號?
				{
					newLine.Words.Add(new BrailleWord("-", BrailleCellCode.Hyphen));
				}
				newLine.TrimEnd();	// 去尾空白。 

                // 如果是折下來的新行，就自動補上需要縮排的格數。
                if (isBroken)
                {
                    for (int i = 0; i < indents; i++)
                    {
                        newLine.Insert(0, BrailleWord.NewBlank());
                    }
                }

				brLine.RemoveRange(0, breakIndex);				// 從原始串列中刪除掉已經複製到新行的點字。
				wordIndex = 0;
				lines.Add(newLine);

                // 防錯：檢驗每個斷行後的 line 的方數是否超過每列最大方數。
                // 若超過，即表示之前的斷行處理有問題，須立即停止執行，否則錯誤會
                // 直到在雙視編輯的 Grid 顯示時才出現 index out of range，不易抓錯!
                System.Diagnostics.Debug.Assert(newLine.CellCount <= cellsPerLine, "斷行錯誤! 超過每列最大方數!");

				// 被折行之後的第一個字需要再根據規則調整。
				EnglishBrailleRule.ApplyCapitalRule(brLine);    // 套用大寫規則。
				EnglishBrailleRule.ApplyDigitRule(brLine);		// 套用數字規則。

                isBroken = true;    // 已經至少折了一行
                maxCells = cellsPerLine - indents;  // 下一行開始就要自動縮排，共縮 indents 格。
			}

            // 補縮排的空方。
            if (context != null && context.IndentCount > 0) // 若目前位於縮排區塊中
            {
                indents = context.IndentCount;
                foreach (BrailleLine aLine in lines)
                {
                    this.Indent(aLine, indents);
                }
            }

			return lines;
        }

        private void Indent(BrailleLine brLine, int indents)
        {
            for (int i = 0; i < indents; i++)
            {
                brLine.Insert(0, BrailleWord.NewBlank());
            }
        }

        /// <summary>
        /// 計算折行之後的縮排格數。
        /// </summary>
        /// <param name="brLine"></param>
        /// <returns>縮排格數。</returns>
        private static int CalcNewLineIndents(BrailleLine brLine)
        {
			if (BrailleConfig.Activated && BrailleConfig.AutoIndentNumberedLine)
			{
				int count = 0;
				bool foundOrderedItem = false;

				// 如果是以數字編號開頭（空白略過），自動計算折行的列要縮排幾格。
				foreach (BrailleWord brWord in brLine.Words)
				{
					if (BrailleWord.IsBlank(brWord))
					{
						count++;
						continue;
					}

					if (BrailleWord.IsOrderedListItem(brWord))
					{
						count++;
						foundOrderedItem = true;
						break;
					}
				}

				if (foundOrderedItem)
					return count;
			}

            return 0;
        }

		/// <summary>
		/// 計算斷行位置。
		/// </summary>
		/// <param name="brLine">點字串列。</param>
		/// <param name="cellsPerLine">每行最大允許幾方。</param>
		/// <param name="needHyphen">是否在斷行處附加一個連字號 '-'。</param>
		/// <returns>傳回可斷行的點字索引。</returns>
		private static int CalcBreakPoint(BrailleLine brLine, int cellsPerLine, 
			out bool needHyphen)
		{
			needHyphen = false;

            // 先根據每列最大方數取得要斷行的字元索引。
			int fixedBreakIndex = brLine.CalcBreakPoint(cellsPerLine);

			if (fixedBreakIndex >= brLine.WordCount)   // 無需斷行？
			{
				return fixedBreakIndex;
			}

			// 需斷行，根據點字規則調整斷行位置。

            int breakIndex = fixedBreakIndex;

			BrailleWord breakWord;

			// 必須和前一個字元一起斷至下一行的字元。亦即，只要剛好斷在這些字元，就要改成斷前一個字元。
			char[] joinLeftChars = {',', '.', '。', '、', '，', '；', '？', '！', '」', '』', '‧'};
			int loopCount = 0;

			while (breakIndex >= 0)
			{
				loopCount++;
				if (loopCount > 10000)
				{
					throw new Exception("偵測到無窮回圈於 BrailleProcessor.CalcBreakPoint()，請通知程式設計師!");
				}

				breakWord = brLine[breakIndex];

                if (breakWord.DontBreakLineHere)    // 如果之前已經設定這個字不能在此處斷行
                {
                    breakIndex--;
                    continue;
                }				

				if (breakWord.Text.IndexOfAny(joinLeftChars) >= 0)
				{
					// 前一個字要和此字元一起移到下一行。
					breakIndex--;
					continue;	// 繼續判斷前一個字元可否斷行。
				}

                if (breakWord.IsWhiteSpace) // 找到空白處，可斷開
                {
                    breakIndex++;   // 斷在空白右邊的字元。
                    break;
                }

                // 處理數字的斷字：連續數字不可斷開。
                if (breakWord.IsDigit)
                {
                    breakIndex--;
                    while (breakIndex >= 0)
                    {
                        if (!brLine[breakIndex].IsDigit)
                        {
                            break; 
                        }
                        breakIndex--;
                    }                    
                }
                else if (breakWord.IsLetter)    // 英文單字不斷字。
				{
					breakIndex--;
                    while (breakIndex >= 0)
                    {
                        if (!brLine[breakIndex].IsLetter)
                        {
                            break;
                        }
                        breakIndex--;
                    }
				}
                else if (breakWord.Text.Equals("_"))    // 連續底線不斷字。
                {
                    breakIndex--;
                    while (breakIndex >= 0)
                    {
                        if (!brLine[breakIndex].Text.Equals("_"))
                        {
                            break;
                        }
                        breakIndex--;
                    }
                }
                else
                {
                    break;
                }
            } // of while (breakIndex >= 0)

			if (breakIndex <= 0)
			{
				// 若此處 breakIndex < 0，表示找不到任何可斷行的位置；
				// 若此處 breakIndex == 0，表示可斷在第一個字元，那也沒有意義，因此也視為找不到斷行位置。

                //Trace.WriteLine("無法找到適當的斷行位置，使用每列最大方數斷行!");
                breakIndex = fixedBreakIndex;
			}

			// 注意!! 若 breakIndex 傳回 0 會導致呼叫的函式進入無窮迴圈!!

			return breakIndex;
		}

        /// <summary>
        /// 將指定的列與下一列相結合（下一列附加至本列）。
        /// </summary>
        /// <param name="brDoc">點字文件。</param>
        /// <param name="lineIndex">本列的列索引。</param>        
        public void JoinNextLine(BrailleDocument brDoc, int lineIndex)
        {
            BrailleLine brLine = brDoc.Lines[lineIndex];

            // 將下一列附加至本列，以結合成一列。
            int nextIndex = lineIndex + 1;
            if (nextIndex < brDoc.Lines.Count)
            {
                brLine.Append(brDoc.Lines[nextIndex]);
                brDoc.Lines.RemoveAt(nextIndex);
            }
        }

        #endregion


        #region Misc. methods.

        /// <summary>
        /// 傳回表示原書頁次的底線字串。
        /// </summary>
        /// <returns></returns>
        private static string GetDashForOrgPageNumber()
        {
			return new string('_', 36); // 36 個底線符號。
        }

        /// <summary>
        /// 檢查傳入的字串是否為原書頁次。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static bool IsOrgPageNumber(string line)
        {
            // 至少要有 36 個底線符號，加上至少一個數字
            if (String.IsNullOrEmpty(line) || line.Length < 37) 
                return false;
            if (line.StartsWith(s_DashesForOrgPageNumber) && Char.IsDigit(line[36]))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 從傳入的字串中取出原書頁碼。
        /// </summary>
        /// <param name="line"></param>
        /// <returns>若傳入的字串不是原書頁碼，則傳回 -1，否則傳回原書頁碼。</returns>
        public static int GetOrgPageNumber(string line)
        {
            if (BrailleProcessor.IsOrgPageNumber(line))
            {
                line = line.Remove(0, 36);
				try
				{
					return Convert.ToInt32(line);
				}
				catch
				{
					throw new Exception("原書頁碼包含無效的數字: " + line);
				}
            }
            return -1;
        }

        #endregion
    }
}
