using System;

namespace Huanlin.Common.Helpers
{
    public static class CharHelper
    {
        /// <summary>
        /// 檢查傳入的字元是否為 ASCII 字元 (0x01～0x7F)。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsAscii(char ch)
        {
            return (ch >= 0x01 && ch <= 0x7F);
        }

        /// <summary>
        /// 檢查傳入的字元是否為 ASCII 的數字 0～9。
        /// 由於 Char.IsDigit 方法會把中文的全形數字１～９也視為數字，
        /// 因此有些需要判斷 ASCII 數字 0～9 的場合，就可以用此方法。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsAsciiDigit(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }

        /// <summary>
        /// 檢查傳入的字元是否為 ASCII 的英文字母。
        /// 由於 Char.IsLetter 方法會把中文字也視為字母，因此有些需要
        /// 判斷 ASCII 英文字母的場合，就可以用此方法。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsAsciiLetter(char ch)
        {
            if (ch >= 'A' && ch <= 'Z')
                return true;
            if (ch >= 'a' && ch <= 'z')
                return true;
            return false;
        }

        /// <summary>
        /// 檢查傳入的字元是否為 ASCII 的英文字母或數字 0～9。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsAsciiLetterOrDigit(char ch)
        {
            return (CharHelper.IsAsciiLetter(ch) || CharHelper.IsAsciiDigit(ch));
        }

        /// <summary>
        /// 檢查傳入的字元是否為全形的英文字母。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsFullShapeLetter(char ch)
        {
            if (ch >= 'Ａ' && ch <= 'Ｚ')
            {
                return true;
            }
            if (ch >= 'ａ' && ch <= 'ｚ')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 檢查傳入的字元是否為全形的數字０～９。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static bool IsFullShapeDigit(char ch)
        {
            if (ch >= '０' && ch <= '９')
                return true;
            return false;
        }

        /// <summary>
        /// 將全形英文字母轉成半形英文字母。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static char FullShapeToAsciiLetter(char ch)
        {
            if (ch >= 'Ａ' && ch <= 'Ｚ')
            {
                return (char)(ch - 'Ａ' + 'A');
            }
            if (ch >= 'ａ' && ch <= 'ｚ')
            {
                return (char)(ch - 'ａ' + 'a');
            }
            throw new ArgumentException("無法轉換成半形英文字母: " + ch.ToString());
        }


        /// <summary>
        /// 將全形數字轉成半形。
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public static char FullShapeToAsciiDigit(char ch)
        {
            if (ch >= '０' && ch <= '９')
                return (char)(ch - '０' + '0');
            throw new ArgumentException("無法轉換成半形數字: " + ch.ToString());
        }
    }
}
