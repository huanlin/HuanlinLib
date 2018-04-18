using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace Huanlin.Common.Helpers
{
	/// <summary>
	/// 用來代表 IP 位址的樣式，例如：192.168.*.*。
	/// 也可以用來儲存 IP 位址。例如：192.168.0.1。
	/// </summary>
	public class IPAddressPattern
	{
		public string[] m_Parts;

		public IPAddressPattern()
		{
			m_Parts = new string[] { "0", "0", "0", "0" };
		}

		public IPAddressPattern(string pattern) : this()
		{
			char[] sep = new char[] { '.' };
			string[] parts = pattern.Split(sep, 4);

			SetParts(parts);
		}

		public IPAddressPattern(string[] parts) : this()
		{
			SetParts(parts);
		}

		protected void SetParts(string[] parts)
		{
			string errmsg = "無效的 IP 位址樣式!";

			for (int i = 0; i < 4; i++)
			{
				try
				{
					// 把數字的前導 0 去掉。
					int k = Convert.ToInt32(parts[i]);
					if (k >= 0 && k <= 255)
					{
						m_Parts[i] = k.ToString();
					}
					else
					{
						throw new ArgumentException(errmsg);
					}
				}
				catch
				{
					if (parts[i] == "*")
					{
						m_Parts[i] = parts[i];
					}
					else
					{
						throw new ArgumentException(errmsg);
					}

				}
			}
		}

		public bool IsValidPart(string s)
		{
			try
			{
				int k = Convert.ToInt32(s);
				return (k >= 0 && k <= 255);
			}
			catch
			{
				return (s == "*");
			}
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < 4; i++) {
				sb.Append(m_Parts[i]);
				if (i < 3) 
				{
					sb.Append(".");
				}
			}
			return sb.ToString();
		}

		public override int GetHashCode()
		{
			int[] mul = new int[] {1000, 100, 10, 1};
			int value = 0;
			for (int i = 0; i < m_Parts.Length; i++)
			{
				try 
				{
					value += Convert.ToInt32(m_Parts[i]) * mul[i];
				}
				catch 
				{
					value += 256 * mul[i];
				}				
			}
			return value;
		}

		public override bool Equals(object obj)
		{
			IPAddressPattern ipap = (IPAddressPattern)obj;

			if (this == obj)
				return true;
			
			if (obj == null)
				return false;

			for (int i = 0; i < 4; i++)
			{
				if (!this[i].Equals(ipap[i]))
					return false;
			}

			return true;
		}

		/// <summary>
		/// 將字串剖析成 IPAddressPattern 物件。
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static IPAddressPattern Parse(string s)
		{
			char[] sep = new char[] { '.' };
			string[] parts = s.Split(sep, 4);

			return new IPAddressPattern(parts);
		}

		/// <summary>
		/// 判斷是否為合法的 IP 位址。
		/// </summary>
		/// <param name="ip">IP 位址</param>
		/// <returns></returns>
		public static bool IsValidIPAddress(string ip)
		{
			string exp = @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$";
			return Regex.IsMatch(ip, exp);
		}

		/// <summary>
		/// 索引子。
		/// </summary>
		/// <param name="idx"></param>
		/// <returns></returns>
		public string this[int idx]
		{
			get
			{
				if (idx < 0 || idx > 3)
				{
					throw new IndexOutOfRangeException();
				}
				return m_Parts[idx];
			}

			set
			{
				if (idx < 0 || idx > 3)
				{
					throw new IndexOutOfRangeException();
				}
				if (!IsValidPart(value))
				{
					throw new Exception("指定的值不是合法的 IP 位址!");
				}

				if (value == "*")
				{
					m_Parts[idx] = value;
				}
				else
				{
					m_Parts[idx] = Convert.ToInt32(value).ToString();	// 把數字的前導 0 去掉。
				}
			}
		}

		/// <summary>
		/// 判斷某個 IP 位址是否符合指定的樣式。
		/// </summary>
		/// <param name="ip">IP 位址</param>
		/// <returns></returns>
		public bool IsMatch(string ip)
		{
			// 先檢查是否為合法的 IP 位址。
			if (!IPAddressPattern.IsValidIPAddress(ip))
			{
				throw new ArgumentException(ip + " 不是合法的 IP 位址!");
			}

			IPAddressPattern ipaddr = IPAddressPattern.Parse(ip);
			
			for (int i = 0; i < 4; i++) {
				if (this[i] != "*" && Convert.ToInt32(this[i]) != Convert.ToInt32(ipaddr[i]))
					return false;
			}
			return true;
		}
	}
}
