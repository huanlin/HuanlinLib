using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using Huanlin.Helpers;

namespace Huanlin.Braille
{
    [Serializable]
    [DataContract]
    public class BrailleCellList
    {
        private List<BrailleCell> m_Cells;

        public BrailleCellList()
        {
            m_Cells = new List<BrailleCell>();
        }

        /// <summary>
        /// �N�Q���i�쪺�I�r�X�r���ন�������I�r����A�å[�J�I�r��C�C
        /// </summary>
        /// <param name="brCodes">�Q���i�쪺�I�r�X�r��C���r�ꪺ�������� 2 �����ơC</param>
        /// <returns></returns>
        public void Add(string brCodes)
        {
            if (String.IsNullOrEmpty(brCodes))
            {
                return; // �����Ū��I�r�X�]�]���I�s�ݥi��`�`�|�ǤJ�Ū��I�r�X�^
            }

            for (int i = 0; i < brCodes.Length; i += 2)
            {
                string s = brCodes.Substring(i, 2);
                byte aByte = StrHelper.HexStrToByte(s);
                BrailleCell cell = BrailleCell.GetInstance(aByte);
                m_Cells.Add(cell);
            }
        }

        public void Assign(BrailleCellList aCellList)
        {
            m_Cells.Clear();
            m_Cells.AddRange(aCellList.m_Cells);
        }

        public void Add(BrailleCell cell)
        {
            m_Cells.Add(cell);
        }

        public void Insert(int index, BrailleCell cell)
        {
            m_Cells.Insert(index, cell);
        }

        public void Clear()
        {
            m_Cells.Clear();
        }

        public BrailleCell this[int index]
        {
            get
            {
                return m_Cells[index];
            }
            set
            {
                m_Cells[index] = value;
            }
        }

        [DataMember]
        public List<BrailleCell> Items
        {
            get
            {
                return m_Cells;
            }

            set
            {
                m_Cells = value;
            }
        }

        public int Count
        {
            get 
            {
                return m_Cells.Count;
            }
        }

        /// <summary>
        /// �N�Ҧ��I�r�ন�������Q���i��X�r��A�C�� 16 �i��r�ꤧ���S�����j�r���C
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.ToString(null);
        }

        /// <summary>
        /// �N�Ҧ��I�r�ন�������Q���i��X�r��A�i���w�C�� 16 �i��r�ꤧ�������j�r��C
        /// </summary>
        /// <param name="separator">�C�� 16 �i��r�ꤧ�������j�r��C</param>
        /// <returns></returns>
        public string ToString(string separator)
        {
            StringBuilder sb = new StringBuilder();
            foreach (BrailleCell cell in m_Cells)
            {
                sb.Append(cell.ToString());
                if (!String.IsNullOrEmpty(separator))
                    sb.Append(separator);
            }
            // �h���h�l�����j�r��
            if (!String.IsNullOrEmpty(separator))
            {
                if (sb.Length >= 2)
                {
                    sb.Length -= 2;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// ��ǤJ���I�r��C����A��̪����e�O�_�ۦP�C
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj))
                return true;

            BrailleCellList cells2 = (BrailleCellList)obj;

            if (this.Count != cells2.Count)
                return false;

            for (int i = 0; i < m_Cells.Count; i++)
            {
                if (!m_Cells[i].Equals(cells2[i]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int hash = 0;
            for (int i = 0; i < m_Cells.Count; i++)
            {
                hash += (int)m_Cells[i].Value;
            }
            return hash;
        }
    }
}
