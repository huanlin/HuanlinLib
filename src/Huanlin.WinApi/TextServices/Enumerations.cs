using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Security;

namespace Huanlin.WinApi.TextServices
{
    [ComImport, SecurityCritical, SuppressUnmanagedCodeSecurity]
    [Guid("3d61bf11-ac5f-42c8-a4cb-931bcc28c744")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumTfLanguageProfiles
    {
        void Clone(out IEnumTfLanguageProfiles enumIPP);

        [PreserveSig]
        int Next(int count, [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]
                            TF_LANGUAGEPROFILE[] profiles, out int fetched);
        void Reset();
        void Skip(int count);
    } 
}
