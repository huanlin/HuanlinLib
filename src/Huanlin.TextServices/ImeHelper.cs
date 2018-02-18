using System;
using System.Collections.Generic;
using System.Text;
using Huanlin.WinApi;
using Huanlin.WinApi.TextServices;
using Huanlin.TextServices.Chinese;
using ImeLib;

namespace Huanlin.TextServices
{
    /// <summary>
    /// 輸入法輔助工具。
    /// 
    /// 微軟新注音 API 則支援一次傳入多個中文字，取得所有對應的注音字根，而且它會根據中文
    /// 字的詞庫自動傳回正確的字根，例如：「一兵一卒」的兩個「一」都會傳回「　ㄧ　ˋ」。
    /// 此類別的 GetBopomofo(string) 就是利用微軟新注音 API。更棒的是，新注音 API 
    /// 也會使用新注音輸入法的使用者造詞檔，因此，如果使用者發現 GetBopomofo 傳回
    /// 的注音不是他要的，例如：「德行」預設會傳回「ㄉ　ㄜˊㄒㄧㄥˋ」，使用者只要在他
    /// 的機器上執行新注音造詞工具，新增「德行」，並指定其字根為「ㄉ　ㄜˊㄒㄧㄥˊ」，
    /// 如此便可變更此 API 傳回的結果。這等於是利用了新注音的使用者造詞功能，就不用自己
    /// 寫了。不過，為了節省使用者建立詞庫的時間，此類別也內建了詞庫，以自動修正一些常
    /// 用的辭彙（見 ZhuyinPhrase.tbl）。
    /// 
    /// 由於新注音 API 可一次取得多個中文字的整串注音字根，因此自然就無法取得多音字的所
    /// 有字根。若需取得多音字的全部注音字根，請使用 ZhuyinQueryHelper 類別。
    /// </summary>
    public static class ImeHelper
    {
        private static MsImeFacade m_MsIme;

        public static readonly bool IFELanguageReady;		// 微軟智慧型注音字根是否可用

        static ImeHelper()
        {
            // 檢查微軟智慧型注音服務是否可用.
            try
            {
                m_MsIme = new MsImeFacade(ImeClass.Taiwan);
                if (m_MsIme != null && m_MsIme.IsReady)
                {
                    IFELanguageReady = true;
                }
            }
            catch
            {
                IFELanguageReady = false;
            }
        }

        /// <summary>
        /// 判斷微軟注音輸入法是否已安裝。
        /// </summary>
        /// <returns></returns>
        public static bool IsZhuyinImeInstalled()
        {            
            short[] langIds = TextServicesHelper.GetlangIds();
            if (langIds.Length > 0)
            {
                string[] inputMethods = TextServicesHelper.GetEnabledInputMethods(langIds[0]);
                foreach (string ime in inputMethods)
                {
                    if (ime.EndsWith(" 注音"))  // 注意前面有多一個空白
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 判斷微軟新注音輸入法是否已安裝。
        /// </summary>
        /// <returns></returns>
        public static bool IzNewZhuyinInstalled()
        {
            short[] langIds = TextServicesHelper.GetlangIds();
            if (langIds.Length > 0)
            {
                string[] inputMethods = TextServicesHelper.GetEnabledInputMethods(langIds[0]);
                foreach (string ime in inputMethods)
                {
                    if (ime.EndsWith(" 新注音"))   // 注意前面有多一個空白
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 利用 IFELanguage 取得整串中文字的注音碼。
        /// </summary>
        /// <param name="aChineseText">中文字串。</param>
        /// <returns>長度固定為 4 的注音符號字串，例如："ㄅ　ㄢ　"。</returns>
        public static string[] GetBopomofo(string aChineseText)
        {
            if (m_MsIme == null)
            {
                throw new Exception("無法取得注音字根：IFELanguage 服務不存在!");
            }

            string[] bopomofoArray = m_MsIme.GetBopomofo(aChineseText);

            // 調整注音碼，使其長度補滿四個字元.
            for (int i = 0; i < bopomofoArray.Length; i++)
            {
                bopomofoArray[i] = Zhuyin.FillSpaces(bopomofoArray[i]);
            }

            return bopomofoArray;
        }

        /// <summary>
        /// 利用 IFELanguage 取得整串中文字的注音碼，同時根據預先指定的詞庫來修正注音。
        /// </summary>
        /// <param name="aChineseText">中文字串。</param>
        /// <returns>包含注音字根的字串陣列。每個元素代表輸入字串中對應位置的字元的注音字根，而且長度固定為 4。</returns>
        public static string[] GetBopomofoWithPhraseTable(string aChineseText)
        {
            string[] bopomofoArray = GetBopomofo(aChineseText);

            // 利用擴充詞庫字根表修正 API 傳回的字根。

            ZhuyinPhraseTable phraseTbl = ZhuyinPhraseTable.GetInstance();
            SortedList<int, ZhuyinPhrase> matchedPhrases = phraseTbl.FindPhrases(aChineseText);
            int srcIndex;
            ZhuyinPhrase phrase;

            // 由於可能會有多次置換字串的動作，因此必須由字串的尾部往前進行置換。
            for (int i = matchedPhrases.Count - 1; i >= 0; i--)
            {
                srcIndex = matchedPhrases.Keys[i];   // 取得片語在輸入字串中的來源索引。
                phrase = matchedPhrases.Values[i];   // 取得代表片語的物件。

                //DebugOut("\r\nimmPhrase.Text=" + immPhrase.Text);

                foreach (Zhuyin zy in phrase.ZhuyinList)
                {
                    bopomofoArray[srcIndex] = zy.ToString(true);    // 儲存注音字根時，會以全型空白補足 4 碼。
                    srcIndex++;
                }
            }
            return bopomofoArray;
        }
    }
}
