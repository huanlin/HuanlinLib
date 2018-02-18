using System;
using System.Collections.Generic;
using System.Text;

namespace Huanlin.Helpers
{
	public sealed class RegExpPatterns
	{
		private RegExpPatterns() {}

		// ���˦��|�䦨�諸���ҡA���p�G�r�ꤤ�X�{���ƪ����ҡA�N�u�|���Ĥ@�ӡC�Ҧp�G<�m�W>Michael</�m�W>�C
		public const string OneTagPair = @"<(?<tag>\\w*)>(?<text>.*)</\\k<tag>>";

		// ���˦��u�n�O�_�l���ҩε����Ыe���|�M��A�����ެO�_����C�Ҧp�G<�m�W>�B</aa>�C
		public const string Tags = @"<([^<>\s]*)(\s[^<>]*)?>";

	}
}
