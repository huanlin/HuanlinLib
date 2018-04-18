using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Huanlin.Common.Helpers;

namespace Huanlin.Windows.Forms
{
    /// <summary>
    /// 能夠將標籤以特殊顏色顯示的 RichTextBox。
    /// 此元件參考自 Patrik Svensson 的文章：Enabling syntax highlighting in a RichTextBox。
    /// URL: http://www.codeproject.com/cs/miscctrl/SyntaxRichTextBox.asp。
    /// </summary>
    [ToolboxBitmap(typeof(RichTextBoxHL), "RichTextBoxHL.bmp")]
	public class RichTextBoxHL : RichTextBox
	{
		private bool m_SkipTextChanged;
		private bool m_EnableUpdate;

		private string m_Line;
		private int m_ContentLength;
		private int m_LineStartIndex;
		private int m_LineEndIndex;
		private int m_LineLength;
		private int m_CurrentSelection;

		private bool m_EnableTagColor;
		private Color m_TagColor;

		public RichTextBoxHL()
			: base()
		{
			m_EnableUpdate = true;
			m_SkipTextChanged = false;

			m_EnableTagColor = true;
			m_TagColor = Color.Maroon;
		}

		public new void LoadFile(string path)
		{
			m_SkipTextChanged = true;
			try
			{
				base.LoadFile(path);
				ProcessAllLines();
			}
			finally
			{
				m_SkipTextChanged = false;
			}
		}

		public new void LoadFile(string path, RichTextBoxStreamType fileType)
		{
			m_SkipTextChanged = true;
			try
			{
				base.LoadFile(path, fileType);
				ProcessAllLines();
			}
			finally 
			{
				m_SkipTextChanged = false;
			}
		}

		public new void LoadFile(Stream data, RichTextBoxStreamType fileType)
		{
			m_SkipTextChanged = true;
			try
			{
				base.LoadFile(data, fileType);
				ProcessAllLines();
			}
			finally
			{
				m_SkipTextChanged = false;
			}
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 0x00f)
			{
				if (m_EnableUpdate)
					base.WndProc(ref m);
				else
					m.Result = IntPtr.Zero;
			}
			else
				base.WndProc(ref m);
		}

		protected override void OnTextChanged(EventArgs e)
		{
			if (m_SkipTextChanged)
				return;
			
			m_EnableUpdate = false;

			try
			{
				// Calculate sh*t here.
				m_ContentLength = this.TextLength;

				int currSelectionStart = SelectionStart;
				int currSelectionLength = SelectionLength;

				// Find the start of the current line.
				m_LineStartIndex = currSelectionStart;
				while ((m_LineStartIndex > 0) && (Text[m_LineStartIndex - 1] != '\n'))
					m_LineStartIndex--;

				// Find the end of the current line.
				m_LineEndIndex = currSelectionStart;
				while ((m_LineEndIndex < Text.Length) && (Text[m_LineEndIndex] != '\n'))
					m_LineEndIndex++;

				// Calculate the length of the line.
				m_LineLength = m_LineEndIndex - m_LineStartIndex;

				// Get the current line.
				m_Line = Text.Substring(m_LineStartIndex, m_LineLength);

				ProcessLine();

                base.OnTextChanged(e);
			}
			finally
			{
				m_EnableUpdate = true;
			}
		}

		private void ProcessLine()
		{
			// Save the position and make the whole line black
			int pos = base.SelectionStart;
			base.SelectionStart = m_LineStartIndex;
			base.SelectionLength = m_Line.Length;
			base.SelectionColor = Color.Black;

			ProcessTagColor();

			base.SelectionStart = pos;
			base.SelectionLength = 0;
			base.SelectionColor = Color.Black;

			m_CurrentSelection = pos;
		}

		private void ProcessTagColor()
		{
			if (!m_EnableTagColor)
				return;

			MatchCollection matches = StrHelper.FindTags(m_Line);

			int start;

			foreach (Match match in matches)
			{
				// 每個找到的 Match 物件都包含一對標籤: <xxx>abc</xxx>

				// 起始標籤
				start = match.Index;
				//end = match.m_Line.IndexOf('>', start + 1);
				base.SelectionStart = m_LineStartIndex + start;
				base.SelectionLength = match.Length;
				base.SelectionColor = m_TagColor;

				// 結束標籤
				//start = m_Line.IndexOf('<', end + 1);
				//end = m_Line.IndexOf('>', start + 1);
				//base.SelectionStart = m_LineStartIndex + start;
				//base.SelectionLength = end - start + 1;
				//base.SelectionColor = m_TagColor;
			}
		}

		private void ProcessAllLines()
		{
			m_Line = base.Text;
			m_LineStartIndex = 0;
			m_LineEndIndex = m_LineStartIndex + m_Line.Length;

			ProcessLine();
		}

		#region 屬性

		public new string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				m_SkipTextChanged = true;
				try
				{
					base.Text = value;
					ProcessAllLines();
				}
				finally
				{
					m_SkipTextChanged = false;
				}
			}
		}

		public new string[] Lines
		{
			get
			{
				return base.Lines;
			}
			set
			{
				m_SkipTextChanged = true;
				try
				{
					base.Lines = value;
					ProcessAllLines();
				}
				finally
				{
					m_SkipTextChanged = false;
				}
			}
		}

		public bool EnableUpdate
		{
			get
			{
				return m_EnableUpdate;
			}
			set
			{
				m_EnableUpdate = value;
			}
		}

		[Browsable(true), Description("是否將標籤顯示成特殊顏色。")]
		public bool EnableTagColor
		{
			get
			{
				return m_EnableTagColor;
			}
			set
			{
				if (m_EnableTagColor != value)
				{
					m_EnableTagColor = value;
					ProcessAllLines();
				}				
			}
		}

		[Browsable(true), Description("標籤顏色。")]
		public Color TagColor
		{
			get
			{
				return m_TagColor;
			}
			set
			{
				if (m_TagColor != value)
				{
					m_TagColor = value;
					ProcessAllLines();
				}
			}
		}

		#endregion
	}
}
