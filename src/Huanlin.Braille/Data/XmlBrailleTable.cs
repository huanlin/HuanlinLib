using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Reflection;
using Huanlin.Helpers;

namespace Huanlin.Braille.Data
{
	/// <summary>
	/// �q XML �ɮ�Ū���I�r��Ӫ�A�ô��ѷj�M�\��C
	/// </summary>
	internal class XmlBrailleTable : BrailleTableBase
	{
		private string m_FileName;
		private bool m_Loaded;
		protected DataTable m_Table;

		public XmlBrailleTable()
		{
			m_Table = new DataTable();
			m_Table.CaseSensitive = true;	// ������ true�A�_�h���ǥb�Φr���|�M���βŸ��V�c�C
            m_Table.Locale = CultureInfo.CurrentUICulture;
			m_FileName = "";
		}

		public XmlBrailleTable(string filename)
			: this()
		{
			Load(filename);
		}

		public override void Load()
		{
			Load(m_FileName);
		}

		/// <summary>
		/// �q XML �ɮ׸��J�I�r��Ӫ�C
		/// </summary>
		/// <param name="filename"></param>
		public override void Load(string filename)
		{
			if (String.IsNullOrEmpty(filename))
			{
				throw new ArgumentException("�ɦW�����w!");
			}

			if (m_Loaded && (String.Compare(m_FileName, filename, true, CultureInfo.CurrentUICulture) == 0))
			{
				return;
			}

            using (StreamReader sr = new StreamReader(filename))
            {
                LoadFromStreamReader(sr);
                m_FileName = filename;
            }
		}

        /// <summary>
        /// �q���w�ե�귽���J�I�r��Ӫ�C
        /// </summary>
        /// <param name="asmb"></param>
        /// <param name="resourceName"></param>
        public override void LoadFromResource(Assembly asmb, string resourceName)
        {
            Stream stream = asmb.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                throw new Exception("XmlBrailleTable.LoadFromResource �䤣��귽: " + resourceName);
            }
            using (stream)
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    LoadFromStreamReader(sr);
                }
            }
        }

        /// <summary>
        /// �q�w�]���귽�W�١]�Y�������O�W�٥[�W .xml ���ɦW�^���J�I�r��Ӫ�C
        /// </summary>
        public virtual void LoadFromResource()
        {
            Assembly asmb = Assembly.GetExecutingAssembly();
            string resName = this.GetType().FullName + ".xml"; // Note: �o�ؼg�k�i�H�קK�g���� namsepace�A�ӥB�Ω� obfuscator �ɤ]�ॿ�`�B�@�C
            this.LoadFromResource(asmb, resName);
        }

        private void LoadFromStreamReader(StreamReader sr)
        {
            using (DataSet ds = new DataSet())
            {
                ds.Locale = CultureInfo.CurrentUICulture;
                ds.ReadXml(sr);
                m_Table = ds.Tables[0].Copy();
                m_Table.CaseSensitive = true;	// ������ true�A�_�h���ǥb�Φr���|�M���βŸ��V�c�C
                m_Table.PrimaryKey = new DataColumn[] { m_Table.Columns["text"] };

                m_Loaded = true;
            }

            //Debug
            //for (int i = 0; i < m_Table.Columns.Count; i++)
            //{
            //    System.Diagnostics.Debug.WriteLine(m_Table.Columns[i].ColumnName);
            //}
        }

		/// <summary>
		/// �q XML �r����J�I�r��Ӫ�C
		/// </summary>
		/// <param name="xml"></param>
		public void LoadFromXmlString(string xml)
		{
			StringReader sr = new StringReader(xml);
			DataSet ds = new DataSet();
            ds.Locale = CultureInfo.CurrentUICulture;
			ds.ReadXml(sr);
			m_Table = ds.Tables[0].Copy();
			m_Table.PrimaryKey = new DataColumn[] { m_Table.Columns["text"] };
			sr.Close();

			m_Loaded = true;
		}


		/// <summary>
		/// �ˬd�I�r��Ӫ�O�_�w�g���J�A�Y�_�A�h��X exception�C
		/// </summary>
		/// <returns></returns>
		protected void CheckLoaded()
		{
			if (!m_Loaded)
			{
				throw new Exception("�I�r��Ӫ�|�����J���!");
			}
		}

		/// <summary>
		/// �ˬd�O�_���X�k���I�r�X�C
		/// </summary>
		/// <param name="code">�I�r�X���Q���i��r��C�Ҧp�G"A0"�C</param>
		protected void CheckCode(string code)
		{
			if (String.IsNullOrEmpty(code) || code.Length % 2 != 0)
			{
				throw new Exception("�I�r��Ӫ���Ƥ����T: " + code);
			}
		}

		/// <summary>
		/// ���ޤl�C�q��r�Ÿ����o�������I�r�X�]16 �i��^�r��C
		/// </summary>
		/// <param name="text">��r�Ÿ��A�Ҧp�G�t�B�G�C</param>
		/// <returns>�I�r�X�r��A�Y�䤣��������Ÿ��A�|��X�ҥ~�C</returns>
        /// <remarks>�p�G�A�Ʊ�䤣��������I�r�X�ɤ��n��X�ҥ~�A�ӬO�Ǧ^�Ŧr��A�Шϥ� Find ��k�C</remarks>
		public override string this[string text]
		{
			get 
			{
				string brCode = Find(text);
                if (String.IsNullOrEmpty(brCode))
                {
                    throw new Exception("�䤣��������I�r�X: " + text);
                }
                return brCode;
			}
		}

		/// <summary>
		/// �j�M�Y�Ӥ�r�Ÿ��A�öǦ^�������I�r�X�C
		/// </summary>
		/// <param name="text">���j�M���Ÿ��C</param>
		/// <returns>�Y�����A�h�Ǧ^�������I�r�X�A�_�h�Ǧ^�Ŧr��C</returns>
        /// <remarks>�p�G�A�Ʊ�䤣��������I�r�X�ɥ�X�ҥ~�A�Шϥί��ޤl�C</remarks>
		public override string Find(string text)
		{
			CheckLoaded();

			DataRow row = m_Table.Rows.Find(text);
			if (row == null)
			{
				return "";
			}
			string code = row["code"].ToString();
			CheckCode(code);
			return code;
		}

		protected string FindFromDataRows(DataRow[] rows, string text)
		{
			foreach (DataRow row in rows)
			{
				if (row["text"].ToString() == text)
				{
					string code = row["code"].ToString();
					CheckCode(code);
					return code;
				}
			}
			return "";
		}

	}
}
