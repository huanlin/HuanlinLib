using System;
using System.Text;
using System.Web;

namespace Huanlin.Common.Http
{
    public static class WebHelper
    {
/*
        /// <summary>
        /// �Ǧ^���w�� HTTP �ШD���ڵ������|�C
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
		public static string GetRootUrl(HttpRequest req)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(req.Url.Scheme);
			sb.Append("://");
			sb.Append(req.Url.Authority);
			sb.Append(req.ApplicationPath);
			sb.Append("/");

			return sb.ToString();
		}

		/// <summary>
		/// ���o�Τ�ݪ� IP ��}�C
		/// </summary>
		/// <param name="req">Request ����C</param>
		/// <param name="detectProxy">�O�_���� proxy�C</param>
		/// <returns></returns>
		public static string GetClientIPAddress(HttpRequest req, bool detectProxy)
		{
			string ip = null;

			if (detectProxy)
			{
				ip = req.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if (String.IsNullOrEmpty(ip))
				{
					ip = req.ServerVariables["REMOTE_ADDR"];
				}
			}
			else
			{
				ip = req.ServerVariables["REMOTE_ADDR"];
			}

			return ip;
		}

        /// <summary>
        /// �z�L HTTP �q���ݾ����U���ɮסC
        /// </summary>
        /// <param name="url">���� URL�C</param>
        /// <param name="fileName">���]�t���|���ɮצW�١]�Τ�ݤU���ɮ׮ɬݨ쪺�ɦW�^�C</param>
        public static void RemoteDownloadFile(string url, string fileName)
        {
            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] buffer = wc.DownloadData(url);

            HttpContext.Current.Response.ClearHeaders();
            HttpContext.Current.Response.Clear();
            string encodedFileName = HttpUtility.UrlEncode(fileName);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + encodedFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", buffer.Length.ToString());
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            HttpContext.Current.Response.BinaryWrite(buffer);
            HttpContext.Current.Response.End();
        }
*/
    }
}
