using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Huanlin.Interop;

namespace Huanlin.WinForms
{
	public class KeyboardHookEventArgs : EventArgs 
	{
		public KeyboardHookEventArgs(int hookCode, int wparam, int lparam, Keys key, bool isPressed)
		{
			HookCode = hookCode;
			WParam = wparam;
			LParam = lparam;
			Key = key;
			IsPressed = isPressed;
			IsHandled = false;
		}

		public readonly int HookCode;	// Hook callback 函式的 nCode 參數
		public readonly int WParam;		// Hook callback 函式的 wParam 參數
		public readonly int LParam;		// Hook callback 函式的 lParam 參數
		public readonly Keys Key;		// 按鍵
		public readonly bool IsPressed;	// 此按鍵是按下還是放開，true=按下
		public bool IsHandled;			// 此按鍵是否已經被處理掉（不傳給後續的鍵盤掛鉤）
	}

	public delegate void KeyboardHookEvent (object sender, KeyboardHookEventArgs args);

	/// <summary>
	/// 區域鍵盤掛鉤（有效攔截範圍僅限於應用程式）。
	/// </summary>
	public class KeyboardHook
	{
		private event KeyboardHookEvent m_HookEvent = null;

		protected int m_HookHandle = 0;		// Hook handle
		protected HookProc m_KbdHookProc;	// 鍵盤掛鉤函式指標

		protected void OnHookInvoked(KeyboardHookEventArgs args)
		{
			if (m_HookEvent != null)
			{
				m_HookEvent(this, args);
			}
		}

		public event KeyboardHookEvent HookInvoked
		{
			add
			{
				m_HookEvent += value;
			}
			remove
			{
				m_HookEvent -= value;
			}
		}

		public bool IsInstalled
		{
			get { return m_HookHandle != 0; }
		}

		/// <summary>
		/// 設置鍵盤掛鉤。
		/// </summary>
		/// <returns></returns>
		virtual public bool Install()
		{
			if (!IsInstalled)
			{
				m_KbdHookProc = new HookProc(this.KeyboardHookProc);
				int threadId = WinApi.GetCurrentThreadId();
				m_HookHandle = WinApi.SetWindowsHookEx(WinApiConst.WH_KEYBOARD, m_KbdHookProc, IntPtr.Zero, threadId);
				return (m_HookHandle != 0);
			}
			return true;
		}

		/// <summary>
		/// 移除鍵盤掛鉤。
		/// </summary>
		/// <returns></returns>
		virtual public bool Remove()
		{
			if (m_HookHandle == 0)
			{
				throw new Exception("尚未設置鍵盤掛鉤!");
			}

			bool ok = WinApi.UnhookWindowsHookEx(m_HookHandle);
			if (ok)
			{
				m_HookHandle = 0;
			}
			return ok;
		}

		protected int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode < 0)
			{
				return WinApi.CallNextHookEx(m_HookHandle, nCode, wParam, lParam);
			}

			// 當按鍵按下及鬆開時都會觸發此函式，故先判斷是按下還是鬆開。
			bool isPressed = (lParam.ToInt32() & 0x80000000) == 0;
			Keys key = (Keys)wParam.ToInt32();

			KeyboardHookEventArgs args = new KeyboardHookEventArgs(nCode, wParam.ToInt32(), lParam.ToInt32(), key, isPressed);

			OnHookInvoked(args);

			if (args.IsHandled)
			{
				return 1;
			}
			return WinApi.CallNextHookEx(m_HookHandle, nCode, wParam, lParam);
		}
	}

	/// <summary>
	/// 全域鍵盤掛鉤（有效攔截範圍橫跨整個作業環境）。
	/// </summary>
	public class GlobalKeyboardHook : KeyboardHook
	{
		/// <summary>
		/// 設置鍵盤掛鉤。
		/// </summary>
		/// <returns></returns>
		public override bool Install()
		{
			if (!IsInstalled)
			{
				using (Process curProcess = Process.GetCurrentProcess())
				using (ProcessModule curModule = curProcess.MainModule)
				{
					m_KbdHookProc = new HookProc(this.KeyboardHookProc);

					m_HookHandle = WinApi.SetWindowsHookEx(WinApiConst.WH_KEYBOARD_LL, m_KbdHookProc,
						WinApi.GetModuleHandle(curModule.ModuleName), 0);
				}
				return (m_HookHandle != 0);
			}
			return true;
		}		
	}
}
