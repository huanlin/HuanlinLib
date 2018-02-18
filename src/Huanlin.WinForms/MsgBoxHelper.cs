using System.Windows.Forms;

namespace Huanlin.WinForms
{
    public sealed class MsgBoxHelper
    {
        public static void ShowError(string msg)
        {
            MessageBox.Show(msg, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowInfo(string msg)
        {
            MessageBox.Show(msg, "訊息", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowWarning(string msg)
        {
            MessageBox.Show(msg, "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowOkCancel(string msg)
        {
            return MessageBox.Show(msg, "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
        }

        public static DialogResult ShowOkCancel(string msg, MessageBoxDefaultButton defBtn)
        {
            return MessageBox.Show(msg, "確認", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, defBtn); 
        }

        public static DialogResult ShowYesNo(string msg)
        {
            return MessageBox.Show(msg, "詢問", MessageBoxButtons.YesNo, MessageBoxIcon.Question); 
        }

        public static DialogResult ShowYesNo(string msg, MessageBoxDefaultButton defBtn)
        {
            return MessageBox.Show(msg, "詢問", MessageBoxButtons.YesNo, MessageBoxIcon.Question, defBtn); 
        }

        public static DialogResult ShowYesNoCancel(string msg)
        {
            return MessageBox.Show(msg, "詢問", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        public static DialogResult ShowYesNoCancel(string msg, MessageBoxDefaultButton defBtn)
        {
            return MessageBox.Show(msg, "詢問", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, defBtn);
        }

        public static DialogResult ShowRetryCancel(string msg)
        {
            return MessageBox.Show(msg, "詢問", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
        }

        public static DialogResult ShowRetryCancel(string msg, MessageBoxDefaultButton defBtn)
        {
            return MessageBox.Show(msg, "詢問", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question, defBtn);
        }

        public static DialogResult ShowAbortRetryIgnore(string msg)
        {
            return MessageBox.Show(msg, "詢問", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question);
        }

        public static DialogResult ShowAbortRetryIgnore(string msg, MessageBoxDefaultButton defBtn)
        {
            return MessageBox.Show(msg, "詢問", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Question, defBtn);
        }
    }
}
