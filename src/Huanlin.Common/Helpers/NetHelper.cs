using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace Huanlin.Common.Helpers
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
			using (var tcp = new TcpClient())
			{
				try
				{
					IAsyncResult ar = tcp.BeginConnect(host, port, null, null);
					if (ar.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeOutSeconds)))
					{
						tcp.EndConnect(ar);
						return true;
					}
					return false;
				}
				catch
				{
					return false;
				}
			}
		}
	}
}
