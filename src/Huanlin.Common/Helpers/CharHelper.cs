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
            int value = Convert.ToInt32(ch);
            return (value >= 1 && value <= 127);
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
            int value = Convert.ToInt32(ch);
            return (value >= 48 && value <= 57);
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
            int value = Convert.ToInt32(ch);
            if (value >= 0x41 && value <= 0x5a)
                return true;
            if (value >= 0x61 && value <= 0x7a)
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
            int bigA = Convert.ToInt32('Ａ');
            int smallA = Convert.ToInt32('ａ');
            int value = Convert.ToInt32(ch);

            value -= bigA;
            if (value >= 0 && value < 26)
            {
                return true;
            }
            value = Convert.ToInt32(ch) - smallA;
            if (value >= 0 && value < 26)
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
            int value = Convert.ToInt32(ch);
            if (value >= Convert.ToInt32('０') && value <= Convert.ToInt32('９'))
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
            int bigA = Convert.ToInt32('Ａ');
            int smallA = Convert.ToInt32('ａ');
            int value = Convert.ToInt32(ch);
            
            value -= bigA;
            if (value >= 0 && value < 26)
            {
                return Convert.ToChar(value + 0x41);
            }
            value = Convert.ToInt32(ch) - smallA;
            if (value >= 0 && value < 26)
            {
                return Convert.ToChar(value + 0x61);
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
            int zero = Convert.ToInt32('０');
            int nine = Convert.ToInt32('９');
            int value = Convert.ToInt32(ch);
            value -= zero;
            if (value < 0 || value > 9)
                throw new ArgumentException("無法轉換成半形數字: " + ch.ToString());
            return Convert.ToChar(value + 48);
        }
    }
}
