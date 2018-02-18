using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Huanlin.Helpers
{
	/// <summary>
	/// 此類別提供網路相關工具函式。
	/// </summary>
	public static class NetHelper
	{
		/// <summary>
		/// 偵測指定的伺服器的特定 port 是否可以連接。
		/// </summary>
		/// <param name="host">伺服器名稱或 IP 位址。</param>
		/// <param name="port">Port 號。</param>
		/// <param name="timeOut">連線逾時時間，單位：秒。</param>
		/// <returns></returns>
		public static bool IsServerConnectable(string host, int port, double timeOutSeconds)
		{
			TcpClient tcp = new TcpClient();
			DateTime t = DateTime.Now;

			try
			{
				IAsyncResult ar = tcp.BeginConnect(host, port, null, null);
				while (!ar.IsCompleted)
				{
					if (DateTime.Now > t.AddSeconds(timeOutSeconds))
					{
						throw new Exception("Connection timeout!");
					}
					System.Threading.Thread.Sleep(100);
				}
				tcp.EndConnect(ar);  // Raise exception for async call (if any).
				tcp.Close();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
