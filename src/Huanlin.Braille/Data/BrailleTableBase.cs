using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Huanlin.Braille.Data
{
	/// <summary>
	/// �I�r��������¦���O�C
	/// </summary>
	internal abstract class BrailleTableBase
	{
		public abstract void Load();
		public abstract void Load(string filename);
        public abstract void LoadFromResource(Assembly asmb, string resourceName);

		public abstract string this[string text]
		{
			get;
		}

		/// <summary>
		/// �j�M�Y�Ӥ�r�Ÿ��A�öǦ^�������I�r�X�C
		/// <b>�`�N�G</b>���j�M��k�O�j�M��ӹ�Ӫ�A��ĳ�ϥΨ�L�������j�M��k�A
		/// �H�K�����~�����G�C�ר�O�`���Ÿ��M�n�աA�@�w�n���O�I�s
		/// FindPhonetic �M FindTone�A�_�h�|�]����J���r�꦳���ΪťզӶǦ^���~�����G�C
		/// </summary>
		/// <param name="text">���j�M���Ÿ��C</param>
		/// <returns>�Y�����A�h�Ǧ^�������I�r�X�A�_�h�Ǧ^�Ŧr��C</returns>
		public abstract string Find(string text);
	}
}
