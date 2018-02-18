using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;

namespace Huanlin.Helpers
{
    [Flags]
    public enum FileAccess : uint
    {
        GenericRead = 0x80000000,
        GenericWrite = 0x40000000,
        GenericExecute = 0x20000000,
        GenericAll = 0x10000000
    }

    [Flags]
    public enum FileShare : uint
    {
        None = 0x00000000,
        /// <summary>
        /// Enables subsequent open operations on an object to request read access. 
        /// Otherwise, other processes cannot open the object if they request read access. 
        /// If this flag is not specified, but the object has been opened for read access, the function fails.
        /// </summary>
        Read = 0x00000001,
        /// <summary>
        /// Enables subsequent open operations on an object to request write access. 
        /// Otherwise, other processes cannot open the object if they request write access. 
        /// If this flag is not specified, but the object has been opened for write access, the function fails.
        /// </summary>
        Write = 0x00000002,
        /// <summary>
        /// Enables subsequent open operations on an object to request delete access. 
        /// Otherwise, other processes cannot open the object if they request delete access.
        /// If this flag is not specified, but the object has been opened for delete access, the function fails.
        /// </summary>
        Delete = 0x00000004
    }

    public enum CreationDisposition : uint
    {
        /// <summary>
        /// Creates a new file. The function fails if a specified file exists.
        /// </summary>
        New = 1,
        /// <summary>
        /// Creates a new file, always. 
        /// If a file exists, the function overwrites the file, clears the existing attributes, combines the specified file attributes, 
        /// and flags with FILE_ATTRIBUTE_ARCHIVE, but does not set the security descriptor that the SECURITY_ATTRIBUTES structure specifies.
        /// </summary>
        CreateAlways = 2,
        /// <summary>
        /// Opens a file. The function fails if the file does not exist. 
        /// </summary>
        OpenExisting = 3,
        /// <summary>
        /// Opens a file, always. 
        /// If a file does not exist, the function creates a file as if dwCreationDisposition is CREATE_NEW.
        /// </summary>
        OpenAlways = 4,
        /// <summary>
        /// Opens a file and truncates it so that its size is 0 (zero) bytes. The function fails if the file does not exist.
        /// The calling process must open the file with the GENERIC_WRITE access right. 
        /// </summary>
        TruncateExisting = 5
    }

    [Flags]
    public enum FileAttributes : uint
    {
        Readonly = 0x00000001,
        Hidden = 0x00000002,
        System = 0x00000004,
        Directory = 0x00000010,
        Archive = 0x00000020,
        Device = 0x00000040,
        Normal = 0x00000080,
        Temporary = 0x00000100,
        SparseFile = 0x00000200,
        ReparsePoint = 0x00000400,
        Compressed = 0x00000800,
        Offline = 0x00001000,
        NotContentIndexed = 0x00002000,
        Encrypted = 0x00004000,
        Write_Through = 0x80000000,
        Overlapped = 0x40000000,
        NoBuffering = 0x20000000,
        RandomAccess = 0x10000000,
        SequentialScan = 0x08000000,
        DeleteOnClose = 0x04000000,
        BackupSemantics = 0x02000000,
        PosixSemantics = 0x01000000,
        OpenReparsePoint = 0x00200000,
        OpenNoRecall = 0x00100000,
        FirstPipeInstance = 0x00080000
    }

    public class LptPrintHelper
    {
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern SafeFileHandle CreateFile(
            string fileName,
            [MarshalAs(UnmanagedType.U4)] FileAccess fileAccess,
            [MarshalAs(UnmanagedType.U4)] FileShare fileShare,
            IntPtr securityAttributes,
            [MarshalAs(UnmanagedType.U4)] CreationDisposition creationDisposition,
            [MarshalAs(UnmanagedType.U4)] FileAttributes flags,
            IntPtr template);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool CloseHandle(IntPtr handle);

        
        private SafeFileHandle m_FileHandle;
        private FileStream m_FileStream;

        public void OpenPrinter()
        {
            OpenPrinter("LPT1");
        }

        public void OpenPrinter(string portName)
        {
            if (String.IsNullOrEmpty(portName.Trim()))
            {
                portName = "LPT1";
            }

            m_FileHandle = LptPrintHelper.CreateFile(
                portName,
                FileAccess.GenericRead | FileAccess.GenericWrite,
                FileShare.None,
                IntPtr.Zero,
                CreationDisposition.OpenExisting,
                FileAttributes.Normal,
                IntPtr.Zero);

            if (m_FileHandle.IsInvalid)
            {
                int errCode = Marshal.GetLastWin32Error();
                throw new Win32Exception(errCode);
            }

            m_FileStream = new FileStream(m_FileHandle, System.IO.FileAccess.Write);
        }

        public void Print(string s)
        {
            if (m_FileStream == null)
            {
                throw new Exception("尚未建立印表機串流，請先呼叫 OpenPrinter()");
            }

            byte[] buf;

            buf = Encoding.Default.GetBytes(s);
            m_FileStream.Write(buf, 0, buf.Length);
        }

        public void ClosePrinter()
        {
            if (m_FileStream != null)
            {
                m_FileStream.Flush();
                m_FileStream.Close();
                m_FileStream.Dispose();

                m_FileHandle.Close();
                m_FileHandle.Dispose();
            }
        }

        /// <summary>
        /// 送出跳頁控制字元。
        /// </summary>
        public void SendFormFeed()
        {
            Print("\f");
        }
    }
}
