using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Security;


namespace Huanlin.Windows.WinApi.TextServices
{
    public class TsfApi
    {
        public static readonly Guid GUID_TFCAT_TIP_KEYBOARD;

        static TsfApi()
        {
            GUID_TFCAT_TIP_KEYBOARD = new Guid(0x34745c63, 0xb2f0, 0x4784, 0x8b, 0x67, 0x5e, 0x12, 200, 0x70, 0x1a, 0x31);
        }

        [SuppressUnmanagedCodeSecurity, DllImport( "msctf.dll" )]
        public static extern int TF_CreateInputProcessorProfiles(out ITfInputProcessorProfiles profiles);
    }
}
