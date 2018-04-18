using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Huanlin.Windows.Forms
{
    public class ControlHelper
    {
        private ControlHelper() { }

        /// <summary>
        /// 將指定的字串輸入至文字方塊。
        /// </summary>
        /// <param name="txtbox"></param>
        /// <param name="s"></param>
        public static void InputText(TextBox txtbox, string s)
        {
            if (txtbox.SelectionLength > 0)
            {
                txtbox.SelectedText = s;
            }
            else
            {
                txtbox.Text = txtbox.Text.Insert(txtbox.SelectionStart, s);
            }
        }
    }
}
