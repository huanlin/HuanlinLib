using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Huanlin.Common.Helpers
{
	public static class FileHelper
	{
		/// <summary>
		/// 尋找檔案，傳回包含檔案清單的字串陣列。
		/// </summary>
		/// <param name="path">尋找此路徑底下的檔案。</param>
		/// <param name="searchPattern">檔名比對樣式，例如："*.mp3;*.wmv"。</param>
		/// <returns></returns>
		public static string[] FindFiles(string path, string searchPattern)
		{
			string[] files;
			string[] patterns;
			HashSet<string> allFiles = new HashSet<string>();

			patterns = searchPattern.Split(";".ToCharArray());

			foreach (string pattern in patterns) {
				files = Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly);

				// 把找到的檔案加入結果清單
				foreach (string fname in files)
				{
					allFiles.Add(fname);
				}
			}
			var sortedFiles = new List<string>(allFiles);
            sortedFiles.Sort();
            return sortedFiles.ToArray();
		}

		/// <summary>
		/// 判斷檔案是否為 UTF-8 編碼。
		/// </summary>
		/// <param name="filename"></param>
		/// <returns></returns>
		public static bool IsUTF8Encoded(string filename)
		{
			FileStream fs = File.OpenRead(filename);
			BinaryReader br = new BinaryReader(fs, Encoding.ASCII);
			try
			{
				byte[] buf = br.ReadBytes(3);
				if (buf.Length >= 3)
				{
					if (buf[0] == 0xef && buf[1] == 0xbb && buf[2] == 0xbf)
					{
						return true;
					}
				}
				return false;
			}
			finally
			{
				br.Close();
				fs.Close();
			}
		}
	}
}
