using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using Nini.Config;
using Huanlin.Helpers;

namespace Huanlin.Braille
{
    /// <summary>
    /// 點字組態。
    /// </summary>
    public class BrailleConfig
    {
        private static bool m_Activated = false;
        private static IConfigSource m_CfgSrc = null;
        private static IConfig m_ConversionCfg = null;

        private BrailleConfig()
        {
        }

        static BrailleConfig()
        {
            Assembly asmb = Assembly.GetEntryAssembly();
            if (asmb != null)
            {
                string fname = StrHelper.ExtractFilePath(asmb.Location) + "Braille.ini";
                if (File.Exists(fname))
                {
                    try
                    {
                        m_CfgSrc = new IniConfigSource(fname);
                        m_ConversionCfg = m_CfgSrc.Configs["Conversion"];
                        m_Activated = true;
                    }
                    catch
                    {
                        m_Activated = false;
                    }
                }
            }
        }

        public static void Save()
        {
            if (!m_Activated)
                return;

            m_CfgSrc.Save();
        }

        public static bool Activated
        {
            get { return m_Activated; }
        }

        #region 組態屬性

		/// <summary>
		/// 當以 # 開頭的編號項目文字折行時，是否要自動內縮一方。
		/// </summary>
		public static bool AutoIndentNumberedLine
        {
			get { return m_ConversionCfg.GetBoolean("AutoIndentNumberedLine", false); }
			set { m_ConversionCfg.Set("AutoIndentNumberedLine", value); }
        }

        #endregion

    }
}
