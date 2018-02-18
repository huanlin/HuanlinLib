using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace Huanlin.Braille.Data
{
	internal sealed class EnglishBrailleTable : XmlBrailleTable
	{
        private static EnglishBrailleTable m_Instance = null;

        private EnglishBrailleTable() : base()
        {
        }

        // ���}��o�� method
		private EnglishBrailleTable(string filename) : base(filename)
		{
		}

        /// <summary>
        /// �Ǧ^ singleton ����A�ø��J�귽�C
        /// </summary>
        /// <returns></returns>
        public static EnglishBrailleTable GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new EnglishBrailleTable();
                m_Instance.LoadFromResource();
            }
            return m_Instance;
        }

		/// <summary>
		/// �j�M�Y�Ӧr���A�öǦ^�������I�r�X�C
		/// </summary>
		/// <param name="text">���j�M���r���C�Ҧp�G'A'�C</param>
		/// <returns>�Y�����A�h�Ǧ^�������I�r�X�A�_�h�Ǧ^�Ŧr��C</returns>
		public string FindLetter(string text)
		{
			CheckLoaded();

			string filter = "type='Letter' and text='" + text.ToUpper() + "'";
			DataRow[] rows = m_Table.Select(filter);
			if (rows.Length > 0)
				return rows[0]["code"].ToString();
			return null;
		}

		/// <summary>
		/// �j�M�Y�ӼƦr�A�öǦ^�������I�r�X�C
		/// </summary>
		/// <param name="text">���j�M���Ʀr�C�Ҧp�G'9'�C</param>
		/// <param name="upper">True/False = �Ǧ^�W���I/�U���I�C</param>
		/// <returns>�Y�����A�h�Ǧ^�������I�r�X�A�_�h�Ǧ^�Ŧr��C</returns>
		public string FindDigit(string text, bool upper)
		{
			CheckLoaded();

			string filter = "type='Digit' and text='" + text + "'";
			DataRow[] rows = m_Table.Select(filter);
			if (rows.Length > 0)
			{
				if (upper)	// �W���I?
					return rows[0]["code"].ToString();
				return rows[0]["code2"].ToString();
			}
			return null;
		}
	}
}
