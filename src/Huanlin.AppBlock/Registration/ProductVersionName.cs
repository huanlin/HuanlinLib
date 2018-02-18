using System;
using System.Collections.Generic;
using System.Text;

namespace Huanlin.AppBlock.Registration
{
	/// <summary>
	/// 產品的版本名稱。
	/// </summary>
	public class ProductVersionName
	{
		public const string Enterprise = "ENT";
		public const string Community = "CMU";
		public const string Standard = "STD";
		public const string Professional = "PRO";
		public const string Personal = "PER";
		public const string Trial = "TRI";

		private ProductVersionName()
		{
		}

		public static string GetLongName(string shortName) 
		{
			switch (shortName.ToUpper())
			{
				case Enterprise:
				case "ENTERPRISE":
					return "企業版";
				case Community:
				case "COMMUNITY":
					return "社群版";
				case Standard:
				case "STANDARD":
					return "標準版";
				case Professional:
				case "PROFESSIONAL":
					return "專業版";
				case Personal:
				case "PERSONAL":
					return "個人版";
				case Trial:
				case "TRIAL":
					return "試用版";
				default:
					break;
			}
			return "";
		}

		public static bool IsTrial(string verName)
		{
			return Trial.Equals(verName, StringComparison.CurrentCultureIgnoreCase);
		}

		public static bool IsPersonal(string verName)
		{
			return Personal.Equals(verName, StringComparison.CurrentCultureIgnoreCase);
		}

		public static bool IsStandard(string verName)
		{
			return Standard.Equals(verName, StringComparison.CurrentCultureIgnoreCase);
		}

		public static bool IsProfessional(string verName)
		{
			return Professional.Equals(verName, StringComparison.CurrentCultureIgnoreCase);
		}

		public static bool IsEnterprise(string verName)
		{
			return Enterprise.Equals(verName, StringComparison.CurrentCultureIgnoreCase);
		}

		public static bool IsProfessionalOrEnterprise(string verName)
		{
			if (Professional.Equals(verName, StringComparison.CurrentCultureIgnoreCase) ||
				Enterprise.Equals(verName, StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
			return false;
		}
	}
}
