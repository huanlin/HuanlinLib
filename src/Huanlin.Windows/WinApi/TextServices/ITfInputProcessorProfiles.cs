using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Security;

namespace Huanlin.Windows.WinApi.TextServices;

[ComImport, SecurityCritical, SuppressUnmanagedCodeSecurity]
[Guid("1F02B6C5-7842-4EE6-8A0B-9A24183A95CA")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface ITfInputProcessorProfiles
{
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Register([In] ref Guid clsId);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void Unregister([In] ref Guid clsId);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void AddLanguageProfile(); // Declaration need to be fixed!!

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void RemoveLanguageProfile([In] ref Guid clsId, [In] short langId, [Out] out Guid guidProfile);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnumInputProcessorInfo(); // Declaration need to be fixed!!

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetDefaultLanguageProfile([In] short langId, [In] ref Guid catId, [Out] out Guid clsId, [Out] out Guid guidProfile);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SetDefaultLanguageProfile([In] short langId, [In] ref Guid clsId, [In] ref Guid guidProfile);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int ActivateLanguageProfile([In] ref Guid clsId, [In] short langId, [In] ref Guid guidProfile);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetActiveLanguageProfile([In] ref Guid clsId, out short langId, out Guid profile);

    [SecurityCritical]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetLanguageProfileDescription([In] ref Guid clsId, [In] short langId, [In] ref Guid guidProfile, [Out] out IntPtr pbstrProfileDesc);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void GetCurrentLanguage([Out] out short langId);

    [PreserveSig]
    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int ChangeCurrentLanguage([In] short langId);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int GetLanguageList([Out] out IntPtr plangIds, [Out] out int count);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int EnumLanguageProfiles([In] short langId, [Out] out IEnumTfLanguageProfiles enumIPP);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int EnableLanguageProfile([In] ref Guid clsId, [In] short langId, [In] ref Guid guidProfile, [In] bool fEnable);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    int IsEnabledLanguageProfile([In] ref Guid clsId, [In] short langId, [In] ref Guid profile, [Out] out bool enabled);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void EnableLanguageProfileByDefault([In] ref Guid clsId, [In] short langId, [In] ref Guid guidProfile, [In] bool fEnable);

    [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
    void SubstituteKeyboardLayout(); // Declaration need to be fixed!! 
}