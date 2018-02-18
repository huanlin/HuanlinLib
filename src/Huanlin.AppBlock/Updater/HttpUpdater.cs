using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;

namespace Huanlin.AppBlock.Updater
{	
	/*
	    此類別可利用 HTTP 協定檢查以及下載應用程式的更新檔案。若其中有一個檔案下載失敗，就會整批 rollback（把先前下載的檔案刪除）。
	    改寫自 Eduardo Oliveira 的 AutoUpdate.vb，並增加下載進度的事件。
	
        此類別有三個公開屬性：

          - ClientPath : 代表用戶端應用程式的所在路徑。例如 "C:\EasyBrailleEdit"。

          - ServerUri : 遠端伺服器上面，存放新版本檔案的 HTTP 路徑。例如: "http://hostname.com/files/ebe/"。

          - ChangeLogFileName : 版本變更說明文件的檔案名稱。例如 "ChangeLog.txt"。

        方法：

          - RetrieveUpdateList() : 取得可更新的檔案清單。此函式不僅會從伺服器端取得更新清單及剖析其內容，還會檢查本地端的檔案是否需要更新或刪除。

          - HasUpdates() : 傳回 True/False，代表伺服器端是否有新版本。

          - Update() : 下載並更新。

        與進度有關的事件：

          - FileUpdating
          - FileUpdated
          - DownloadProgressChanged

        Wrtiien by Huan-Lin Tsai. 2008-07-02.
    */
    public class HttpUpdater : IDisposable
	{
		public class FileUpdateEventArgs : EventArgs
		{
			public readonly string FileName;	// 準備要下載的檔名
			public readonly int Number;		// 這是第幾個檔案
			public readonly int Total;		// 總共有幾個檔案

			public FileUpdateEventArgs(string filename, int number, int total)
			{
				this.FileName = filename;
				this.Number = number;
				this.Total = total;
			}
		}

		public delegate void FileUpdateEventHandler(object sender, FileUpdateEventArgs args);

		private bool m_Disposed = false;

		private const string ToDeleteExtension = ".ToDelete";
		private const string TempFileExtension = ".UpdTmp";
		private const string UpdateFileName = "Update.txt";

		private const string ErrorClientPathEmpty = "AutoUpdate.ClientPath 不可為空字串!";
		private const string ErrorDownloadingFile = "無法下載檔案: \r\n";
		private const string ErrorMessageUpdate = "自動更新過程發生錯誤.";
		private const string ErrorMessageDelete = "刪除檔案時發生錯誤.";
		private const string ErrorServerUriEmpty = "尚未指定 ServerUri 屬性!";
		private const string ReadyToRollback = "更新檔案過程發生錯誤，準備嘗試回復檔案。";

		private string m_ClientPath;
		private string m_ServerUri;
		private string m_ChangeLogFileName;		// 應用程式的變更記錄檔名，若有指定，在進行更新時會自動下載此檔案.
		private List<UpdateItem> m_UpdateItems;

		// 事件：更新開始、下載進度回報、更新完成
		private event FileUpdateEventHandler m_FileUpdatingEvent;
		private event FileUpdateEventHandler m_FileUpdatedEvent;
		private event DownloadProgressChangedEventHandler m_DownloadProgressChangedEvent;
		

		public HttpUpdater()
		{
			m_ClientPath = "";
			m_ChangeLogFileName = "";

			m_UpdateItems = new List<UpdateItem>();
		}

		public void Dispose()
		{
			lock (this)
			{
				if (!m_Disposed)
				{
					m_FileUpdatingEvent = null;
					m_FileUpdatedEvent = null;
					m_DownloadProgressChangedEvent = null;

					m_UpdateItems.Clear();
					m_UpdateItems = null;

					m_Disposed = true;
					GC.SuppressFinalize(this);
				}
			}
		}

		#region 清理

		/// <summary>
		/// 清理上次執行更新時所留下的暫存檔。
		/// </summary>
		/// <returns></returns>
		public void CleanUp()
		{
			CheckClientPath();

			string file;
			DirectoryInfo dir = new DirectoryInfo(m_ClientPath);

			FileInfo[] infos = dir.GetFiles("*" + ToDeleteExtension, SearchOption.AllDirectories);

			foreach (FileInfo info in infos)
			{
				file = info.FullName;
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}

			// 刪除暫存檔
			infos = dir.GetFiles("*" + TempFileExtension, SearchOption.AllDirectories);
			foreach (FileInfo info in infos)
			{
				file = info.FullName;
				File.SetAttributes(file, FileAttributes.Normal);
				File.Delete(file);
			}
		}

		#endregion

		private void CheckServerUrl()
		{
			if (String.IsNullOrEmpty(m_ServerUri))
			{
				throw new Exception(ErrorServerUriEmpty);
			}
		}

		private void CheckClientPath()
		{
			if (String.IsNullOrEmpty(m_ClientPath))
			{
				throw new Exception(ErrorClientPathEmpty);
			}
		}


		private void StripComments(ref string[] fileList)
		{
			// Parse the file list to strip comments
			StringBuilder sb = new StringBuilder();
			int i;

			foreach (string file in fileList)
			{
				string fileAux;

				i = file.IndexOf("\'");
				if (i >= 0)	// 去掉註解（以單引號開始）
				{
					fileAux = file.Substring(0, i).Trim();
				}
				else
				{
					fileAux = file.Trim();
				}

				if (fileAux != string.Empty)
				{
					if (sb.Length > 0)
					{
						sb.Append('\n');
					}
					sb.Append(fileAux);
				}
			}

			// Parse the file list again
			fileList = sb.ToString().Split('\n');
		}

		private bool FileExistsAndNotEmpty(string filename)
		{
			FileInfo fi = new FileInfo(filename);
			if (fi.Exists && fi.Length > 0)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// 取得更新清單。此函式不僅會從伺服器端取得更新清單及剖析其內容，
		/// 還會檢查本地端的檔案是否需要更新或刪除。處理的結果是儲存在
		/// m_UpdateItems 屬性中。
		/// </summary>
		public void RetrieveUpdateList()
		{
			CheckClientPath();
			CheckServerUrl();

			WebClient myWebClient = new WebClient();
			
			try
			{
				// 取得 Update.txt 的內容
				myWebClient.Encoding = Encoding.UTF8;
				string contents = myWebClient.DownloadString(m_ServerUri + UpdateFileName);

				// Strip the "LF" from CR+LF and break it down by line
				contents = contents.Replace("\n", "");
				string[] fileList = contents.Split('\r');

				StripComments(ref fileList);

				string[] info;
				string infoFilePath;
				String infoParam;
				List<string> fileNameList = new List<string>();

				Version verRemote;	// 遠端檔案的版本
				Version verLocal;	// 本地端檔案的版本
				FileVersionInfo fileVerInfo;

				m_UpdateItems.Clear();	// 清除更新項目清單

				foreach (string file in fileList)
				{
					info = file.Split(';');
					infoFilePath = info[0].Trim();			// 從檔案清單項目中取出一個檔名（包含完整路徑）

					if (m_ChangeLogFileName.Equals(infoFilePath, StringComparison.CurrentCultureIgnoreCase))
					{
						// 忽略變更記錄檔。
						continue;
					}
					if (info.Length > 1)
					{
						infoParam = info[1].Trim().ToLower();	// 從檔案清單項目中取出欲對該檔案執行的動作參數
					}
					else
					{
						infoParam = "?";	// 沒有指定分號，給預設值：用戶端沒有該檔案時才更新
					}

					// 去掉開頭的 '.' 和兩個反斜線 '\'。
					while (infoFilePath[0] == '.' || infoFilePath[0] == '\\')
					{
						infoFilePath = infoFilePath.Substring(1);
					}

					UpdateItem item = new UpdateItem();
					item.FileName = infoFilePath;

					// 忽略已經存在清單中（重複）的檔案
					if (m_UpdateItems.Contains(item))
					{
						continue;
					}

					// 根據指定的更新動作和參數調整 NeedDelete 和 NeedUpdate 旗號

					string clientFileName = ClientPath + item.FileName;

					bool fileExists = File.Exists(clientFileName);	// FileExistsAndNotEmpty(clientFileName);

					item.Operation = UpdateAction.None;	

					if ((infoParam == "delete"))
					{
						// The second parameter is "delete"
						if (fileExists)
						{
							item.Operation = UpdateAction.Delete;	// 刪除
						}
					}
					else if ((infoParam == "?"))
					{
						// The second parameter is "?" : 檢查檔案是否存在，若存在則不更新，不存在才更新
						if (!fileExists)
						{
							item.Operation = UpdateAction.Overwrite;
						}
					}
					else if (infoParam != string.Empty && (infoParam[0] == '=' && fileExists))
					{
						// 第二個參數若以 '=' 字元開始，表示只有 local 跟遠端的檔案版本不同就要更新
						fileVerInfo = FileVersionInfo.GetVersionInfo(clientFileName);
						verRemote = new Version(infoParam.Substring(1, infoParam.Length - 1));
						verLocal = new Version(fileVerInfo.FileVersion);

						if (verRemote != verLocal)
						{
							// 只要兩個版本不同就要更新
							item.Operation = UpdateAction.Overwrite;
						}
					}
					else
					{
						// infoParam 指定的是版本編號

						if (fileExists)
						{
							// If 2nd parameter is empty, do nothing as file already exists
							if (infoParam != string.Empty)
							{
								// Check the version of local and remote files
								fileVerInfo = FileVersionInfo.GetVersionInfo(clientFileName);

								if (fileVerInfo.FileVersion == null)
								{
									// 用戶端有這個檔案，可是沒有版本編號
									item.Operation = UpdateAction.Overwrite;
								}
								else
								{
									// 比對兩邊的版本編號
									verRemote = new Version(infoParam);
									verLocal = new Version(fileVerInfo.FileVersion);

									if (verRemote > verLocal)
									{
										item.Operation = UpdateAction.Overwrite;
									}									
								}
							}
						}
						else
						{
							// 用戶端沒有這個檔案，所以需要更新。
							item.Operation = UpdateAction.Overwrite;
						}
					} 

					// 若有需要更新才加入更新項目清單.
					if (item.Operation != UpdateAction.None)
					{
						m_UpdateItems.Add(item);
					}
				}
			}
			finally
			{
				myWebClient.Dispose();
			}		
		}

		public bool HasUpdates()
		{
			return (m_UpdateItems.Count > 0);
		}

		/// <summary>
		/// 執行線上更新。
		/// </summary>
		/// <returns>已更新的檔案數量（包含刪除的檔案）。/// </returns>
		public int Update() 
		{
			if (!HasUpdates())
			{
				return 0;
			}

			CleanUp();

			// 加入預設的變更記錄檔名.
			if (!String.IsNullOrEmpty(m_ChangeLogFileName)) 
			{
				UpdateItem updItem = new UpdateItem(m_ChangeLogFileName, UpdateAction.Overwrite);
				if (!m_UpdateItems.Contains(updItem)) 
				{
					m_UpdateItems.Add(updItem);
				}
			}

			int updCount = 0;

			// execute the following line even for check runs
			List<RollbackItem> rollBackList = new List<RollbackItem>();

			WebClient myWebClient = new WebClient();

			// 訂閱事件
			myWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(WebClient_DownloadProgressChanged);
			myWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(WebClient_DownloadFileCompleted);

		    try
			{
				foreach (UpdateItem item in m_UpdateItems)
				{
					var serverFileUrl = ServerUri + item.FileName;
					var clientFileName = ClientPath + item.FileName;
					var tempFileName = clientFileName + DateTime.Now.Ticks.ToString() + TempFileExtension;
					var toDeleteFileName = tempFileName + ToDeleteExtension;

					// 開始執行更新作業
					FileUpdateEventArgs updEvtArgs = new FileUpdateEventArgs(item.FileName, updCount+1, m_UpdateItems.Count);
					OnFileUpdating(updEvtArgs);

				    RollbackItem rollBackItem;
				    switch (item.Operation)
					{
						case UpdateAction.Overwrite:
							// 1.下載檔案並存成暫時檔名
							myWebClient.DownloadFileAsync(new Uri(serverFileUrl), tempFileName);
							while (myWebClient.IsBusy)
							{
								Application.DoEvents();	// 讓主執行緒能夠繼續處理更新 UI 的動作
							}
							if (!FileExistsAndNotEmpty(tempFileName))	// 檢查檔案下載是否成功
							{
								throw new Exception(ErrorDownloadingFile + item.FileName);
								// note: 錯誤訊息中不要顯示完整檔案路徑，以免被看出下載路徑，使用者就能自行下載檔案了。
							}

							// 2.將欲覆蓋的原有檔案更名為待刪除檔案
							if (File.Exists(clientFileName))	// 再檢查一次檔案是否存在，以確保不會發生錯誤
							{
								// 此時不真的刪除檔案，而是用 rename 的方式將目前的檔案更名（因為檔案可能
								// 正在使用中），待下次執行更新作業時，便會由 Cleanup 真正將它們刪除。
								File.Move(clientFileName, toDeleteFileName);
							}

							// 3.將下載的檔案更名為欲覆蓋的檔案
							File.Move(tempFileName, clientFileName);

							// 4.建立 rollback 項目
							rollBackItem = new RollbackItem(toDeleteFileName, clientFileName, RollbackAction.Rename);
							rollBackList.Add(rollBackItem);
							break;
						case UpdateAction.Delete:
							if (File.Exists(clientFileName))	// 檢查檔案是否存在，以確保不會發生錯誤
							{
								// 此時不真的刪除檔案，而是用 rename 的方式將目前的檔案更名（因為檔案可能
								// 正在使用中），待下次執行更新作業時，便會由 Cleanup 真正將它們刪除。
								File.Move(clientFileName, toDeleteFileName);

								// 建立 rollback 資訊
								rollBackItem = new RollbackItem(toDeleteFileName, clientFileName, RollbackAction.Rename);
								rollBackList.Add(rollBackItem);
							}
							break;
						default:
							break;
					}

					// 注意這裡用了一個技巧：先利用 File.Move 把 client 端要更新的檔案 rename 為待下次
					// 刪除的暫存檔名，然後才把下載下來的新版檔案 rename 成目標檔案。這樣的話，即使目
					// 標檔案是正在執行中的主程式（自己），也一樣可以成功更新。這是因為執行中的檔案是
					// 允許改名的。

					OnFileUpdated(updEvtArgs);

					updCount++;
				}

				return updCount;
			}
			catch (Exception ex)
			{
				// Rollback 
				RollbackItem rollback;

				for (int i = rollBackList.Count-1; i >= 0; i--)
				{
					rollback = rollBackList[i];
					if (rollback.Operation == RollbackAction.Rename)
					{
						if (File.Exists(rollback.TargetFileName))
						{
							File.Delete(rollback.TargetFileName);
						}
						if (File.Exists(rollback.SourceFileName))
						{
							File.Move(rollback.SourceFileName, rollback.TargetFileName);
						}
					}
				}
				throw ex;
			}
			finally
			{
				// 解除訂閱 WebClient 事件
				myWebClient.DownloadProgressChanged -= new DownloadProgressChangedEventHandler(WebClient_DownloadProgressChanged);
				myWebClient.DownloadFileCompleted -= new AsyncCompletedEventHandler(WebClient_DownloadFileCompleted);				

				myWebClient.Dispose();
				rollBackList.Clear();
			}
		}

		void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			// Nothing to do here, yet.
		}

		private void OnFileUpdating(FileUpdateEventArgs e)
		{
			if (m_FileUpdatingEvent != null)
			{
				m_FileUpdatingEvent(this, e);
			}
		}

		private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			if (m_DownloadProgressChangedEvent != null)
			{
				m_DownloadProgressChangedEvent(sender, e);
			}
		}

		private void OnFileUpdated(FileUpdateEventArgs e)
		{
			if (m_FileUpdatedEvent != null)
			{
				m_FileUpdatedEvent(this, e);
			}
		}


		#region 屬性

		public string ClientPath
		{
			get { return m_ClientPath; }
			set 
			{ 
				if (String.IsNullOrEmpty(value)) 
				{
					throw new InvalidOperationException(ErrorClientPathEmpty);
				}
				m_ClientPath = value;
				if (!value.EndsWith("\\"))	// 自動附加反斜線
				{
					m_ClientPath = value + "\\";
				}
			}
		}

		public string ServerUri
		{
			get { return m_ServerUri; }
			set 
			{
				if (String.IsNullOrEmpty(value))
				{
					throw new InvalidOperationException("AutoUpdate.ServerUri 不可為空字串!");
				}
				m_ServerUri = value;
				if (!value.EndsWith("/"))	// 自動附加斜線
				{
					m_ServerUri = value + "/";
				}
			}
		}

		public string ChangeLogFileName
		{
			get { return m_ChangeLogFileName; }
			set { m_ChangeLogFileName = value; }
		}

		#endregion

		#region 事件

		public event FileUpdateEventHandler FileUpdating
		{
			add
			{
				m_FileUpdatingEvent += value;
			}
			remove
			{
				m_FileUpdatingEvent -= value;
			}
		}

		public event FileUpdateEventHandler FileUpdated
		{
			add
			{
				m_FileUpdatedEvent += value;
			}
			remove
			{
				m_FileUpdatedEvent -= value;
			}
		}

		public event DownloadProgressChangedEventHandler DownloadProgressChanged
		{
			add
			{
				m_DownloadProgressChangedEvent += value;
			}
			remove
			{
				m_DownloadProgressChangedEvent -= value;
			}
		}

		#endregion

	}

	public enum UpdateAction
	{
		None,
		Overwrite,
		Delete
	}

	/// <summary>
	/// 更新項目。
	/// </summary>
	public class UpdateItem
	{
		public string FileName = null;
		public UpdateAction Operation;

		public UpdateItem()
		{
			Operation = UpdateAction.None;
		}

		public UpdateItem(string filename, UpdateAction updAction)
		{
			FileName = filename;
			Operation = updAction;
		}

		public override bool Equals(object obj)
		{
			UpdateItem item = obj as UpdateItem;

			if (item == null)
				return false;

			return item.FileName.Equals(this.FileName, StringComparison.CurrentCultureIgnoreCase);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	public enum RollbackAction
	{
		Rename
	}

	/// <summary>
	/// 自動更新的回覆項目。其內容記錄的不是當初執行的更新動作，
	/// 而是在復原時需要執行的補償動作。
	/// </summary>
	public class RollbackItem
	{
		#region Properties

		private string m_SourceFileName;
		private string m_TargetFileName;
		private RollbackAction m_Operation;	// 原本執行的操作

		public RollbackAction Operation
		{
			get { return m_Operation; }
			set { m_Operation = value; }
		}

		public string SourceFileName
		{
			get { return m_SourceFileName; }
			set { m_SourceFileName = value; }
		}

		public string TargetFileName
		{
			get { return m_TargetFileName; }
			set { m_TargetFileName = value; }
		}

		#endregion

		#region Constructor

		public RollbackItem(string original, string renamed, RollbackAction operation)
		{
			m_SourceFileName = original;
			m_TargetFileName = renamed;
			m_Operation = operation;
		}

		#endregion
	}	
}
