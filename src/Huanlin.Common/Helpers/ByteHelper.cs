using System;
using System.Collections.Generic;
using System.Text;

namespace Huanlin.Common.Helpers
{
	/// <summary>
	/// �B�z�줸�ժ��u�����O�C
	/// </summary>
	public static class ByteHelper
	{
		/// <summary>
		/// �N�ǤJ���줸�ժ��줸����C
		/// �Y bit-7 ���� bit-0�Abit-6 ���� bit-1�Abit-5 ���� bit-2�A....�C
		/// </summary>
		/// <param name="aByte">�n���઺�줸�աC</param>
		/// <returns>���ध�᪺�줸�աC</returns>
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
