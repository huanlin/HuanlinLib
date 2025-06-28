using System;
using System.Runtime.InteropServices;

namespace Huanlin.Windows.WinApi;

public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

/// <summary>
/// Win32 API。
/// </summary>
public sealed class WinKernel
{
	private WinKernel() { }

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	public static extern IntPtr GetModuleHandle(string lpModuleName);

	// 設置掛鉤.
	[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

	// 將之前設置的掛鉤移除。記得在應用程式結束前呼叫此函式.
	[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	public static extern bool UnhookWindowsHookEx(int idHook);

	// 呼叫下一個掛鉤處理常式（若不這麼做，會令其他掛鉤處理常式失效）.
	[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

	// 取得目前的執行緒 ID。
	[DllImport("kernel32.dll")]
	public static extern int GetCurrentThreadId();
}

/// <summary>
/// Win32 API 常數。
/// </summary>
public sealed class WinApiConst
{
	private WinApiConst() { }

	public const int WH_KEYBOARD = 2;
	public const int WH_KEYBOARD_LL = 13;
}