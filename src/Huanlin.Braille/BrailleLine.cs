using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Huanlin.Braille
{
    /// <summary>
    /// �Ψ��x�s�@�C�I�r�C
    /// </summary>
    [Serializable]
    [DataContract]
	public class BrailleLine : ICloneable
	{
		private List<BrailleWord> m_Words;

		public BrailleLine()
		{
			m_Words = new List<BrailleWord>();
		}

		public void Clear()
		{
			m_Words.Clear();
		}

        [DataMember]
		public List<BrailleWord> Words
		{
			get { return m_Words; }
            set { m_Words = value; }
		}

        public int WordCount
        {
            get { return m_Words.Count; }
        }

		public BrailleWord this[int index]
		{
			get
			{
				return m_Words[index];
			}
		}

		/// <summary>
		/// �Ǧ^�Ҧ��I�r���`��ơC
		/// </summary>
		public int CellCount
		{
			get
			{
				int cnt = 0;
				foreach (BrailleWord brWord in m_Words)
				{
					cnt += brWord.Cells.Count;
				}
				return cnt;
			}
		}

		/// <summary>
		/// �p���_�檺�I�r���ަ�m�C
		/// ���B�ȮھڶǤJ���̤j��ƨӭp��i�_�檺�I�r���ޡA�å��[�J��L�_��W�h���P�_�C
		/// </summary>
		/// <param name="cellsPerLine">�@��i���\�h�֤�ơC</param>
		/// <returns>�i�_�檺�I�r���ޡC�Ҧp�A�Y���޽s���� 29 �Ӧr�]0-based�^�������U�@��A
		/// �Ǧ^�ȴN�O 29�C�Y���ݭn�_��A�h�Ǧ^��檺�r�ơC</returns>
		public int CalcBreakPoint(int cellsPerLine)
		{
			if (cellsPerLine < 4)
			{
				throw new ArgumentException("cellsPerLine �ѼƭȤ��i�p�� 4�C");
			}

			int cellCnt = 0;
			int index = 0;
			while (index < m_Words.Count)
			{
				cellCnt += m_Words[index].Cells.Count;
				if (cellCnt > cellsPerLine)
				{
					break;
				}
				index++;
			}
			return index;
		}

		/// <summary>
		/// �q���w���_�l��m�ƻs���w�Ӽƪ��I�r (BrailleWord) ��s�إߪ��I�r��C�C
		/// </summary>
		/// <param name="index">�_�l��m</param>
		/// <param name="count">�n�ƻs�X���I�r�C</param>
		/// <returns>�s���I�r��C�C</returns>
		public BrailleLine Copy(int index, int count)
		{
			BrailleLine brLine = new BrailleLine();
			BrailleWord newWord = null;
			while (index < m_Words.Count && count > 0)
			{
				//newWord = m_Words[index].Copy();
				newWord = m_Words[index]; 
				brLine.Words.Add(newWord);

				index++;
				count--;

			}
			return brLine;
		}

        public void RemoveAt(int index)
        {
            m_Words.RemoveAt(index);
        }

		public void RemoveRange(int index, int count)
		{
            if ((index + count) > m_Words.Count)    // ����n�����ƶq�W�X��ɡC
            {
                count = m_Words.Count - index;
            }
			m_Words.RemoveRange(index, count);
		}

        /// <summary>
        /// �N���w���I�r�C���[�ܦ��I�r�C�C
        /// </summary>
        /// <param name="brLine"></param>
        public void Append(BrailleLine brLine)
        {
            if (brLine == null || brLine.WordCount < 1)
                return;

            m_Words.AddRange(brLine.Words);
        }

        public void Insert(int index, BrailleWord brWord)
        {
            m_Words.Insert(index, brWord);
        }

        /// <summary>
        /// �h���}�Y���ťզr���C
        /// </summary>
		public void TrimStart()
		{
			int i = 0;
			while (i < m_Words.Count)
			{
				if (BrailleWord.IsBlank(m_Words[i]) || BrailleWord.IsEmpty(m_Words[i]))
				{
					m_Words.RemoveAt(i);
					continue;
				}
				break;
			}
		}

        /// <summary>
        /// �h���������ťզr���C
        /// </summary>
		public void TrimEnd()
		{
			int i = m_Words.Count - 1;
			while (i >= 0)
			{
				if (BrailleWord.IsBlank(m_Words[i]) || BrailleWord.IsEmpty(m_Words[i]))
				{
					m_Words.RemoveAt(i);
					i--;
					continue;
				}
				break;
			}
		}

		/// <summary>
		/// ���Y�����ťեh���C
		/// </summary>
		public void Trim()
		{
			TrimStart();
			TrimEnd();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			foreach (BrailleWord brWord in m_Words)
			{
				sb.Append(brWord.ToString());
			}
			return sb.ToString();
		}

        /// <summary>
        /// �O�_�]�t���D���Ҽ��ҡC
        /// </summary>
        /// <returns></returns>
        public bool ContainsTitleTag()
        {
            if (m_Words.Count > 0 && m_Words[0].Text.Equals(ContextTagNames.Title))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// �����Ҧ����Ҽ��ҡC
        /// </summary>
        public void RemoveContextTags()
        {
            BrailleWord brWord;

            for (int i = this.WordCount - 1; i >= 0; i--)
            {
                brWord = this.m_Words[i];
                if (brWord.IsContextTag)
                {
                    this.m_Words.RemoveAt(i);
                }
            }
        }

		/// <summary>
		/// �b��C���M����w���r��A�q��C������ startIndex �Ӧr�}�l��_�C
		/// </summary>
		/// <param name="value"></param>
		/// <param name="startIndex"></param>
		/// <param name="comparisonType"></param>
		/// <returns></returns>
		public int IndexOf(string value, int startIndex, StringComparison comparisonType)
		{
			if (startIndex + value.Length > this.WordCount)
			{
				return -1;
			}

			int i;
			StringBuilder sb = new StringBuilder();
			for (i = startIndex; i < this.WordCount; i++)
			{
				sb.Append(m_Words[i].Text);
			}

			int idx = sb.ToString().IndexOf(value, comparisonType);
			if (idx < 0)
			{
				return -1;
			}

			// �����A���o�O�r�����ޡA�٥����ץ����I�r���ޡC
			for (i = startIndex; i < this.WordCount; i++)
			{
				idx = idx - m_Words[i].Text.Length + 1;
			}

			return startIndex + idx;
		}

		#region ICloneable Members

		/// <summary>
		/// �`�h�ƻs�C
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			BrailleLine brLine = new BrailleLine();
			BrailleWord newWord = null;

			foreach (BrailleWord brWord in m_Words)
			{
				newWord = brWord.Copy();
				brLine.Words.Add(newWord);
			}
			return brLine;
		}

		#endregion
	}
}
