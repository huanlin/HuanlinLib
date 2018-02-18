using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Huanlin.Sys
{
	/// <summary>
	/// 此類別可用來執行檔案。
	/// </summary>
	public class FileRunner : IDisposable
	{
		private ProcessStartInfo m_StartInfo;
		private Process m_Process;
		private bool m_NeedWait;		// 是否等待應用程式執行結束
		private int m_WaitTime;			// 要等多久（seconds），0 表示採用內定值（30分鐘）

		private string m_ErrMsg;
		private StringBuilder m_StdError;
		private StringBuilder m_StdOutput;
		private event DataReceivedEventHandler m_StdOutputReceivedEvent;
		private event EventHandler m_ExitedEvent;

		public FileRunner()
		{
			m_StdError = new StringBuilder();
			m_StdOutput = new StringBuilder();

			m_StartInfo = new ProcessStartInfo();

			m_NeedWait = true;
			m_WaitTime = 0;
			m_StartInfo.CreateNoWindow = false;
			m_StartInfo.UseShellExecute = false;
			m_StartInfo.WorkingDirectory = "";
			m_StartInfo.WindowStyle = ProcessWindowStyle.Normal;
			m_StartInfo.RedirectStandardError = false;
			m_StartInfo.RedirectStandardOutput = false;
		}

		public void Dispose()
		{
			if (m_Process != null) 
			{
				KillProcess();
			}
		}

		/// <summary>
		/// 執行檔案。
		/// </summary>
		/// <param name="filename">檔案名稱。</param>
		/// <param name="argument">檔案執行時帶的參數，可傳空字串。</param>
		/// <returns>若執行錯誤，則設定錯誤訊息並傳回 false。</returns>
		public bool Run(string filename, string argument)
		{
			if (Running)
			{
				m_ErrMsg = "先前執行的程式尚未結束!";
				return false;
			}

			m_ErrMsg = "";
			m_StdError.Length = 0;
			m_StdOutput.Length = 0;

			if (String.IsNullOrEmpty(filename))
			{
				throw new ArgumentException("未指定欲執行的檔案名稱!");
			}

			// 不要檢查檔案是否存在，因為有些是在 PATH 路徑下的檔案
			/*
			if (!File.Exists(filename))
			{
				m_ErrMsg = "指定的檔案不存在: " + filename;
				return false;
			}*/

			if (!String.IsNullOrEmpty(WorkingDirectory))
			{
				if (!Directory.Exists(WorkingDirectory))
				{
					throw new InvalidOperationException("指定的工作路徑不存在: " + WorkingDirectory);
				}
			}

			m_StartInfo.FileName = filename;
			m_StartInfo.Arguments = argument;

			m_Process = new Process();
			m_Process.StartInfo = m_StartInfo;

			// 訂閱 process 事件
			m_Process.Exited += new EventHandler(Process_Exited); 
			m_Process.OutputDataReceived += new DataReceivedEventHandler(Process_OutputDataReceived);
			m_Process.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);

			try
			{				
				if (m_Process.Start())
				{
					if (RedirectStandardOutput)	// 需要重新導向 console 輸出
					{
						m_Process.BeginOutputReadLine();
					}
					// 不重新導向 console 輸出，那麼等待程式執行完畢的機制必須另尋他法
					if (m_NeedWait)
					{
						WaitToKill();
						KillProcess();						
					}
					return true;
				}
				else
				{
					m_ErrMsg = "沒有啟動新的處理序（可能重複使用既有的處理序）。";
					return false;
				}
			}
			catch (Exception ex)
			{
				m_Process.Close();
				m_Process.Dispose();
				m_Process = null;
				throw ex;
			}			
		}

		void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			lock (this)
			{
				m_StdOutput.Append(e.Data);
				m_StdOutput.Append(System.Environment.NewLine);
			}
			OnStdOutputReceived(e);
		}

		void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			// Standard error 一樣也輸出到 standard output device.
			Process_OutputDataReceived(sender, e);
		}


		private void Process_Exited(object sender, EventArgs e)
		{
			OnProcessExited(e);
		}

		private void OnProcessExited(EventArgs e)
		{
			if (m_ExitedEvent != null)
			{
				m_ExitedEvent(this, e);
			}
		}

		private void WaitToKill()
		{
			int waitTimeToKill;

			if (m_WaitTime <= 0)	// 如果沒有指定等待時間
			{
				waitTimeToKill = 1000 * 60 * 30;	// 則預設等待 30 分鐘
			}
			else
			{
				waitTimeToKill = 1000 * m_WaitTime;
			}
			m_Process.WaitForExit(waitTimeToKill);
		}

		private void OnStdOutputReceived(DataReceivedEventArgs args)
		{
			if (m_StdOutputReceivedEvent != null)
			{				
				// 觸發用戶端事件（Note: 用戶端若要在此事件中更新 UI，務必 marshal 回主執行緒!）
				m_StdOutputReceivedEvent(this, args);

				// 注意：這裡一定要延遲一下，拖延此事件所屬的 worker thread 的速度，
				// 因為 UpdateUI 會導致控制項更新，這更新要花些時間，如果 worker thread 速度很快地
				// 一直塞資料進來，user 又用滑鼠在視窗上東點西點的，會導致整個 main form 訊息佇列
				// 來不及處理，而讓程式看起來像當掉一樣。註: 用 Application.DoEvents() 沒有幫助。

				System.Threading.Thread.Sleep(10);	
			}
		}

		/// <summary>
		/// 強制終止執行的程式。
		/// </summary>
		public void KillProcess()
		{
			if (m_Process == null)
				return;

			if (m_Process.HasExited == false)
			{
				m_Process.Kill();
				m_ErrMsg = "執行的應用程式超過指定等待時間而被強制終止!";
			}

			// 解除訂閱事件
			m_Process.Exited -= this.Process_Exited;
			m_Process.OutputDataReceived -= this.Process_OutputDataReceived;
			m_Process.ErrorDataReceived -= this.Process_ErrorDataReceived;

			m_Process.Close();
			m_Process.Dispose();
			m_Process = null;
		}

		#region 屬性------------------------------

		public bool Running
		{
			get 
			{ 
				if (m_Process == null)
					return false;
				return !m_Process.HasExited;
			}
		}

		public string WorkingDirectory
		{
			get { return m_StartInfo.WorkingDirectory; }
			set { m_StartInfo.WorkingDirectory = value; }
		}

		public bool UseShellExecute
		{
			get { return m_StartInfo.UseShellExecute; }
			set 
			{
				if (value && RedirectStandardOutput)
				{
					throw new InvalidOperationException("UseShellExecute 和 RedirectStandardOutput 不可同時為 true!");
				}
				m_StartInfo.UseShellExecute = value; 
			}
		}

		public bool ShowWindow
		{
			get { return !m_StartInfo.CreateNoWindow; }
			set { m_StartInfo.CreateNoWindow = !value; }
		}

		public bool RedirectStandardOutput
		{
			get { return m_StartInfo.RedirectStandardOutput; }
			set
			{
				if (value && UseShellExecute)
				{
					throw new InvalidOperationException("UseShellExecute 和 RedirectStandardOutput 不可同時為 true!");
				}

				m_StartInfo.RedirectStandardOutput = value;
				m_StartInfo.RedirectStandardError = value;
			}
		}

		public bool NeedWait
		{
			get { return m_NeedWait; }
			set { m_NeedWait = value; }
		}

		/// <summary>
		/// 要等多久（seconds），0 表示採用內定值（30分鐘）。超過此時間 process 將被強制終止。
		/// </summary>
		public int WaitTime
		{
			get { return m_WaitTime; }
			set
			{
				if (value < 0 || value > (24 * 60 * 60))
				{
					throw new Exception("WaitTime 不可超過 24 小時!");
				}
				m_WaitTime = value;
			}
		}

		public string ErrorMsg
		{
			get { return m_ErrMsg; }
		}

		public string StdOutputMsg
		{
			get
			{
				return m_StdOutput.ToString();
			}
		}

		public ProcessWindowStyle WindowStyle
		{
			get { return m_StartInfo.WindowStyle; }
			set { m_StartInfo.WindowStyle = value; }
		}

		public string Verb
		{
			get { return m_StartInfo.Verb; }
			set { m_StartInfo.Verb = value; }
		}

		public bool ErrorDialog
		{
			get { return m_StartInfo.ErrorDialog; }
			set { m_StartInfo.ErrorDialog = value; }
		}

		public int ExitCode 
		{
			get 
			{ 
				if (m_Process == null)
					throw new Exception("沒有執行程式或程式已完全釋放，故無法取得 ExitCode。");
				return m_Process.ExitCode;
			}
		}

		public DateTime ExitTime
		{
			get
			{
				if (m_Process == null)
					throw new Exception("沒有執行程式或程式已完全釋放，故無法取得 ExitTime。");
				return m_Process.ExitTime;
			}
		}

		#endregion

		#region 事件--------------------------------

		/// <summary>
		/// 執行的程式「正常」結束時會觸發此事件。若程式被元件強制終止（直接 Kill），則不會觸發此事件。
		/// 注意：不要在此事件中使用類似 MessageBox.Show 的函式，程式可能會出現怪問題。
		/// </summary>
		public event EventHandler ProcessExited 
		{
			add
			{
				m_ExitedEvent += value;
			}
			remove
			{
				m_ExitedEvent -= value;
			}
		}

		public event DataReceivedEventHandler StdOutputReceived
		{
			add
			{
				m_StdOutputReceivedEvent += value;
			}
			remove
			{
				m_StdOutputReceivedEvent -= value;
			}
		}

		#endregion
	}
}
