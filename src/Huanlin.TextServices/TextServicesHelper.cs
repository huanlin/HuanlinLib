using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Huanlin.WinApi.TextServices;

namespace Huanlin.TextServices
{
    /// <summary>
    /// Text Services Framework helper class.
    /// Written by Huan-Lin Tsai. (2009-12-13)
    /// </summary>
    public class TextServicesHelper
    {
        public static short[] GetlangIds()
        {
            List<short> langIds = new List<short>();
            ITfInputProcessorProfiles profiles;
            if (TsfApi.TF_CreateInputProcessorProfiles(out profiles) == 0)
            {
                IntPtr langPtrs;
                int fetchCount = 0;
                if (profiles.GetLanguageList(out langPtrs, out fetchCount) == 0)
                {
                    for (int i = 0; i < fetchCount; i++)
                    {
                        short id = Marshal.ReadInt16(langPtrs, sizeof(short) * i);
                        langIds.Add(id);
                    }
                }
                Marshal.ReleaseComObject(profiles);
            }
            return langIds.ToArray();
        }

        /// <summary>
        /// 取得所有的輸入法名稱。
        /// </summary>
        /// <param name="langId">Language ID。</param>
        /// <returns>包含輸入法名稱的字串陣列。</returns>
        public static string[] GetInputMethods(short langId)
        {
            List<string> imeList = new List<string>();
            ITfInputProcessorProfiles profiles;
            if (TsfApi.TF_CreateInputProcessorProfiles(out profiles) == 0)
            {
                try
                {
                    IEnumTfLanguageProfiles enumerator = null;
                    if (profiles.EnumLanguageProfiles(langId, out enumerator) == 0)
                    {
                        if (enumerator != null)
                        {
                            TF_LANGUAGEPROFILE[] langProfile = new TF_LANGUAGEPROFILE[1];
                            int fetchCount = 0;
                            while (enumerator.Next(1, langProfile, out fetchCount) == 0)
                            {
                                IntPtr ptr;
                                if (profiles.GetLanguageProfileDescription(ref langProfile[0].clsId, langProfile[0].langId, ref langProfile[0].guidProfile, out ptr) == 0)
                                {
                                    imeList.Add(Marshal.PtrToStringBSTR(ptr));
                                }
                                Marshal.FreeBSTR(ptr);
                            }
                        }
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(profiles);
                }
            }
            return imeList.ToArray();
        }


        /// <summary>
        /// 取得所有已安裝的輸入法名稱。
        /// </summary>
        /// <param name="langId">Language ID。</param>
        /// <returns>包含輸入法名稱的字串陣列。</returns>
        public static string[] GetEnabledInputMethods(short langId)
        {
            List<string> imeList = new List<string>();
            ITfInputProcessorProfiles profiles;
            if (TsfApi.TF_CreateInputProcessorProfiles(out profiles) == 0)
            {
                try
                {
                    IEnumTfLanguageProfiles enumerator = null;
                    if (profiles.EnumLanguageProfiles(langId, out enumerator) == 0)
                    {
                        if (enumerator != null)
                        {
                            TF_LANGUAGEPROFILE[] langProfile = new TF_LANGUAGEPROFILE[1];
                            int fetchCount = 0;
                            while (enumerator.Next(1, langProfile, out fetchCount) == 0)
                            {
                                IntPtr ptr;
                                if (profiles.GetLanguageProfileDescription(ref langProfile[0].clsId, langProfile[0].langId, ref langProfile[0].guidProfile, out ptr) == 0)
                                {
                                    bool enabled;
                                    if (profiles.IsEnabledLanguageProfile(ref langProfile[0].clsId,
                                    langProfile[0].langId, ref langProfile[0].guidProfile, out enabled) == 0)
                                    {
                                        if (enabled)
                                            imeList.Add(Marshal.PtrToStringBSTR(ptr));
                                    }
                                }
                                Marshal.FreeBSTR(ptr);
                            }
                        }
                    }
                }
                finally
                {
                    Marshal.ReleaseComObject(profiles);
                }
            }
            return imeList.ToArray();
        }

        /// <summary>
        /// 切換到指定的輸入法。
        /// </summary>
        /// <param name="langId">輸入語言的 languagd ID。</param>
        /// <param name="imeName">完整的輸入法名稱。例如：中文 (繁體) - 新注音。</param>
        /// <returns>成功傳回 true，否則傳回 false。</returns>
        public static bool ActivateInputMethod(short langId, string imeName)
        {
            ITfInputProcessorProfiles profiles;
            if (TsfApi.TF_CreateInputProcessorProfiles(out profiles) != 0)
            {
                return false;
            }

            try
            {
                IEnumTfLanguageProfiles enumerator = null;
                if (profiles.EnumLanguageProfiles(langId, out enumerator) == 0)
                {
                    if (enumerator != null)
                    {
                        TF_LANGUAGEPROFILE[] langProfile = new TF_LANGUAGEPROFILE[1];
                        int fetchCount = 0;
                        while (enumerator.Next(1, langProfile, out fetchCount) == 0)
                        {
                            IntPtr ptr;
                            if (profiles.GetLanguageProfileDescription(ref langProfile[0].clsId,
                               langProfile[0].langId, ref langProfile[0].guidProfile, out ptr) == 0)
                            {
                                bool enabled;
                                if (profiles.IsEnabledLanguageProfile(ref langProfile[0].clsId,
                                    langProfile[0].langId, ref langProfile[0].guidProfile, out enabled) == 0)
                                {
                                    if (enabled)
                                    {
                                        string s = Marshal.PtrToStringBSTR(ptr);
                                        if (s.Equals(imeName))
                                        {
                                            if (profiles.ActivateLanguageProfile(
                                                    ref langProfile[0].clsId, langProfile[0].langId, ref langProfile[0].guidProfile) == 0)
                                                return true;
                                        }
                                    }
                                }
                                Marshal.FreeBSTR(ptr);
                            }
                        }
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(profiles);
            }
            return false;
        }

        /// <summary>
        /// 啟用/移除指定的輸入法。作用等同於在控制台的「文字服務和輸入語言」對話窗裡面新增和移除輸入法。
        /// Note: 移除時儘管 API 傳回成功，但有時侯會出現輸入法並未真正移除的情形。
        /// </summary>
        /// <param name="langId">輸入語言的 language ID。</param>
        /// <param name="imeName">完整的輸入法名稱。例如：中文 (繁體) - 新注音。</param>
        /// <param name="isEnable">啟用或是移除。</param>
        /// <returns></returns>
        public static bool EnableInputMethod(short langId, string imeName, bool isEnable)
        {
            ITfInputProcessorProfiles profiles;
            if (TsfApi.TF_CreateInputProcessorProfiles(out profiles) != 0)
            {
                return false;
            }

            try
            {
                IEnumTfLanguageProfiles enumerator = null;
                if (profiles.EnumLanguageProfiles(langId, out enumerator) == 0)
                {
                    if (enumerator != null)
                    {
                        TF_LANGUAGEPROFILE[] langProfile = new TF_LANGUAGEPROFILE[1];
                        int fetchCount = 0;
                        while (enumerator.Next(1, langProfile, out fetchCount) == 0)
                        {
                            IntPtr ptr;
                            if (profiles.GetLanguageProfileDescription(ref langProfile[0].clsId,
                               langProfile[0].langId, ref langProfile[0].guidProfile, out ptr) == 0)
                            {
                                string s = Marshal.PtrToStringBSTR(ptr);
                                if (s.Equals(imeName))
                                {
                                    bool enabled;
                                    if (profiles.IsEnabledLanguageProfile(ref langProfile[0].clsId,
                                        langProfile[0].langId, ref langProfile[0].guidProfile, out enabled) == 0)
                                    {
                                        if (enabled != isEnable)
                                        {
                                            if (profiles.EnableLanguageProfile(
                                                    ref langProfile[0].clsId, langProfile[0].langId, ref langProfile[0].guidProfile, isEnable) == 0)
                                            return true;
                                        }
                                    }
                                }
                                Marshal.FreeBSTR(ptr);
                            }
                        }
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(profiles);
            }
            return false;
        }
    }
}
