using System;
using System.Runtime.InteropServices;

namespace Huanlin.Windows.WinApi.TextServices
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TF_LANGUAGEPROFILE
    {
        public Guid clsId;
        public short langId;
        public Guid catid;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fActive;
        public Guid guidProfile;
    }
}
