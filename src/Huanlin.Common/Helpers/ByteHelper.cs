using System;
using System.Collections.Generic;
using System.Text;

namespace Huanlin.Common.Helpers
{
	/// <summary>
	/// 處理位元組的工具類別。
	/// </summary>
	public static class ByteHelper
	{
		/// <summary>
		/// 將傳入的位元組的位元反轉。
		/// 即 bit-7 移到 bit-0，bit-6 移到 bit-1，bit-5 移到 bit-2，....。
		/// </summary>
		/// <param name="aByte">要反轉的位元組。</param>
		/// <returns>反轉之後的位元組。</returns>
		public static byte Reverse(byte aByte)
		{
			byte result = 0x00;
			byte mask = 0x00;

			for (mask = 0x80; Convert.ToInt32(mask) > 0; mask >>= 1)
			{
				result >>= 1;
				byte tempbyte = (byte)(aByte & mask);
				if (tempbyte != 0x00)
					result |= 0x80;
			}
			return (result);
		}
	}
}
