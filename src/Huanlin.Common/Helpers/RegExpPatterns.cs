using System;
using System.Collections.Generic;
using System.Text;

namespace Huanlin.Common.Helpers
{
	public sealed class RegExpPatterns
	{
		private RegExpPatterns() {}

		// 此樣式會找成對的標籤，但如果字串中出現重複的標籤，就只會找到第一個。例如：<姓名>Michael</姓名>。
		public const string OneTagPair = @"<(?<tag>\\w*)>(?<text>.*)</\\k<tag>>";

		// 此樣式只要是起始標籤或結束標前都會尋找，但不管是否成對。例如：<姓名>、</aa>。
		public const string Tags = @"<([^<>\s]*)(\s[^<>]*)?>";

	}
}
