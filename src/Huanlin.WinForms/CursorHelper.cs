using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Huanlin.WinForms
{
    /// <summary>
    /// 處理滑鼠指標的工具類別。
    /// </summary>
    public class CursorHelper
    {
        private CursorHelper() { }

        private static Stack<Cursor> m_CursorStack = new Stack<Cursor>();

        public static void ShowCursor(Cursor cs)
        {
            m_CursorStack.Push(Cursor.Current);
            Cursor.Current = cs;
            Application.DoEvents();
        }

        public static void ShowWaitCursor() 
        {
            CursorHelper.ShowCursor(Cursors.WaitCursor);
        }

        public static void RestoreCursor() 
        {
            if (m_CursorStack.Count > 0)
            {
                Cursor.Current = m_CursorStack.Pop();
            }
        }

        public static void ResetToDefault()
        {
            m_CursorStack.Clear();
            Cursor.Current = Cursors.Default;
            Application.DoEvents();
        }

    }
}
